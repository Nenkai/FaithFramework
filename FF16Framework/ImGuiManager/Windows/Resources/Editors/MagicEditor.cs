using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

using FF16Framework.Services.ResourceManager;
using FF16Framework.Utils;

using FF16Tools.Files.Magic;
using FF16Tools.Files.Magic.Factories;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;

namespace FF16Framework.ImGuiManager.Windows.Resources.Editors;

public class MagicEditor
{
    public ResourceHandle Resource;
    public MagicFile MagicFile;
    public bool IsOpen = true;
    private MagicEntry? CurrentEntry;

    private MagicOperationGroup? _pendingGroupDelete;
    private MagicOperationProperty? _pendingPropertyDelete;
    private IOperation? _pendingOperationDelete;

    private IDisposableHandle<IImGuiTextFilter> _magicFilterHandle;
    private IDisposableHandle<IImGuiTextFilter> _operationFilterHandle;
    private Dictionary<MagicOperationType, IDisposableHandle<IImGuiTextFilter>> _propertyFilters = [];

    private IOperation? _addedOperation;
    public MagicEditor(ResourceHandle resource, MagicFile magicFile)
    {
        Resource = resource;
        MagicFile = magicFile;
    }

    public unsafe void Render(IImGuiShell shell, IImGui imgui)
    {
        if (imgui.Begin("Magic Editor"u8, ref IsOpen, ImGuiWindowFlags.ImGuiWindowFlags_None))
        {
            imgui.PushStyleColor(ImGuiCol.ImGuiCol_Button, ColorUtils.RGBA(0, 156, 65, 255));
            imgui.PushStyleColor(ImGuiCol.ImGuiCol_ButtonHovered, ColorUtils.RGBA(13, 172, 80, 255));
            if (imgui.Button("📤 Apply to game!"u8))
            {
                using (var memStream = new MemoryStream())
                {
                    MagicFile.Write(memStream);

                    byte[] data = memStream.ToArray();
                    Resource.ReplaceBuffer(data);
                }

                shell.LogWriteLine(nameof(MagicEditor), "Saved!");
            }
            imgui.PopStyleColorEx(2);

            imgui.SameLine();
            if (imgui.Button("📂 Load from file"u8))
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Magic File (*.magic)|*.magic";
                dialog.CheckFileExists = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var fileInfo = new FileInfo(dialog.FileName);
                        if (fileInfo.Length > 0x10_0000 * 10) // 10 mb
                            shell.LogWriteLine(nameof(MagicEditor), $"File is too large!", Color.Red);
                        else
                        {
                            MagicFile = MagicFile.Open(dialog.FileName);

                            byte[] bytes = File.ReadAllBytes(dialog.FileName);
                            Resource.ReplaceBuffer(bytes);
                            CurrentEntry = null;

                            shell.LogWriteLine(nameof(MagicEditor), $"Magic file loaded.");
                        }
                    }
                    catch (Exception ex)
                    {
                        shell.LogWriteLine(nameof(MagicEditor), $"Failed to load: {ex.Message}", Color.Red);
                    }
                }
            }

            imgui.SameLine();
            if (imgui.Button("💾 Save as..."u8))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = $"{Path.GetFileName(Encoding.UTF8.GetString(Resource.FileNameSpan))}";
                dialog.Filter = "Magic File (*.magic)|*.magic";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var fs = File.Create(dialog.FileName))
                            MagicFile.Write(fs);

                        shell.LogWriteLine(nameof(MagicEditor), $"Saved magic file to {dialog.FileName}!");
                    }
                    catch (Exception ex)
                    {
                        shell.LogWriteLine(nameof(MagicEditor), $"Failed to save: {ex.Message}", Color.Red);
                    }
                }
            }

            imgui.SameLine();
            var reloadPropertyValueTypes = imgui.Button("🔄 Reload property value types"u8);
            if (imgui.IsItemHovered(ImGuiHoveredFlags.ImGuiHoveredFlags_None))
                imgui.SetTooltip("Reload from Magic/MagicPropertyValueTypes.txt"u8);

            if (reloadPropertyValueTypes)
            {
                try
                {
                    MagicPropertyValueTypeMapping.Read();
                }
                catch (Exception ex)
                {
                    shell.LogWriteLine(nameof(MagicEditor), $"SFailed to load mappings: {ex.Message}", Color.Red);
                }
            }

            if (imgui.BeginCombo("Magics"u8, "Select magic..."u8, ImGuiComboFlags.ImGuiComboFlags_HeightLarge))
            {
                _magicFilterHandle ??= imgui.CreateTextFilter();
                IImGuiTextFilter filter = _magicFilterHandle.Value;

                imgui.SetNextItemWidth(-1.0f);
                if (imgui.InputTextWithHint("##MagicFilter"u8, "Filter magics..."u8, filter.InputBuf.AsSpan(), ImGuiInputTextFlags.ImGuiInputTextFlags_None))
                    imgui.ImGuiTextFilter_Build(filter);

                if (imgui.IsWindowAppearing())
                {
                    imgui.SetKeyboardFocusHere();
                    imgui.ImGuiTextFilter_Clear(filter);
                }

                foreach (KeyValuePair<uint, MagicEntry> elem in MagicFile.MagicEntries)
                {
                    if (!MagicIdsMapping.IdToName.TryGetValue(elem.Key, out string? name))
                        name = "Unknown";

                    string item = $"{name} ({elem.Key})";
                    if (imgui.ImGuiTextFilter_PassFilter(filter, item, null))
                    {
                        if (imgui.Selectable(item))
                            CurrentEntry = elem.Value;
                    }

                    if (elem.Value == CurrentEntry)
                        imgui.SetItemDefaultFocus();
                }

                imgui.EndCombo();
            }

            if (CurrentEntry is not null)
            {
                imgui.SeparatorText($"Magic: {CurrentEntry.Id}");
                if (imgui.BeginChild($"##magicchild_{CurrentEntry.Id}", Vector2.Zero, ImGuiChildFlags.ImGuiChildFlags_None, ImGuiWindowFlags.ImGuiWindowFlags_None))
                    RenderCurrentMagicEntry(shell, imgui);

                imgui.EndChild();
            }
        }


        imgui.End();
    }

    private unsafe void RenderCurrentMagicEntry(IImGuiShell shell, IImGui imgui)
    {
        foreach (MagicOperationGroup group in CurrentEntry!.OperationGroupList.OperationGroups)
        {
            bool visible = true;
            if (imgui.CollapsingHeaderBoolPtr($"Group {group.Id}", ref visible, ImGuiTreeNodeFlags.ImGuiTreeNodeFlags_DefaultOpen))
            {
                if (!visible)
                    imgui.OpenPopup($"Delete Group##delgroup_modal{group.Id}", ImGuiPopupFlags.ImGuiPopupFlags_None);

                imgui.SetNextWindowPosEx(imgui.ImGuiViewport_GetCenter(imgui.GetMainViewport()), ImGuiCond.ImGuiCond_Appearing, pivot: new Vector2(0.5f, 0.5f));
                if (imgui.BeginPopupModal($"Delete Group##delgroup_modal{group.Id}", ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize))
                {
                    imgui.Text("🗑 Delete Group?"u8);
                    imgui.Separator();

                    imgui.PushStyleColor(ImGuiCol.ImGuiCol_Button, ColorUtils.RGBA(0xAA, 0x22, 0x22));
                    imgui.PushStyleColor(ImGuiCol.ImGuiCol_ButtonHovered, ColorUtils.RGBA(0xCC, 0x22, 0x22));
                    bool yes = imgui.ButtonEx("Yes"u8, new Vector2(100, 0));
                    imgui.PopStyleColorEx(2);

                    if (yes)
                    {
                        _pendingGroupDelete = group;
                        imgui.CloseCurrentPopup();
                    }

                    imgui.SameLine();
                    if (imgui.ButtonEx("Close"u8, new Vector2(100, 0)))
                        imgui.CloseCurrentPopup();

                    imgui.EndPopup();
                }
                

                foreach (IOperation op in group.OperationList.Operations)
                {
                    RenderOperation(shell, imgui, group, op);
                }

                imgui.SetNextItemWidth(-1.0f);
                if (imgui.BeginCombo($"##op_picker{group.Id}", "➕ Add operation...", ImGuiComboFlags.ImGuiComboFlags_None))
                {
                    _magicFilterHandle ??= imgui.CreateTextFilter();
                    IImGuiTextFilter filter = _magicFilterHandle.Value;

                    imgui.SetNextItemWidth(-1.0f);
                    if (imgui.InputTextWithHint("##OperationFilter"u8, "Filter operations..."u8, filter.InputBuf.AsSpan(), ImGuiInputTextFlags.ImGuiInputTextFlags_None))
                        imgui.ImGuiTextFilter_Build(filter);

                    if (imgui.IsWindowAppearing())
                    {
                        imgui.SetKeyboardFocusHere();
                        imgui.ImGuiTextFilter_Clear(filter);
                    }

                    foreach (MagicOperationType op in Enum.GetValues<MagicOperationType>())
                    {
                        string name = op.ToString();
                        if (imgui.ImGuiTextFilter_PassFilter(filter, name, null))
                        {
                            if (imgui.Selectable(op.ToString()))
                            {
                                IOperation operation = MagicOperationFactory.Create(op);
                                group.OperationList.Operations.Add(operation);
                                _addedOperation = operation;
                            }
                        }
                    }

                    imgui.EndCombo();
                }

                if (_pendingOperationDelete is not null)
                {
                    group.OperationList.Operations.Remove(_pendingOperationDelete);
                    _pendingOperationDelete = null;
                }
            }
        }

        if (_pendingGroupDelete is not null)
        {
            CurrentEntry!.OperationGroupList.OperationGroups.Remove(_pendingGroupDelete);
            _pendingGroupDelete = null;
        }
    }

    private unsafe void RenderOperation(IImGuiShell shell, IImGui imgui, MagicOperationGroup group, IOperation op)
    {
        string operationId = $"{CurrentEntry!.Id}-{group.Id}-{op.Type}";

        if (_addedOperation == op)
        {
            imgui.SetNextItemOpen(true, ImGuiCond.ImGuiCond_Once);
            _addedOperation = null;
        }

        bool isExpanded = imgui.TreeNodeEx($"{op.Type}##{operationId}", ImGuiTreeNodeFlags.ImGuiTreeNodeFlags_AllowOverlap);
        float right = imgui.GetContentRegionAvail().X;

        float offset = isExpanded ? 0.0f : 24f;  // Not sure why I need to offset when expanded. Otherwise it goes to the left and leaves a gap
        imgui.SameLineEx(right - offset, 0.0f);
        if (imgui.SmallButton($"❌##del_{operationId}"))
            _pendingOperationDelete = op;

        if (isExpanded)
        {
            if (imgui.BeginTable($"##proptable_{operationId}", 3, ImGuiTableFlags.ImGuiTableFlags_Borders))
            {
                imgui.TableSetupColumn("Type"u8, ImGuiTableColumnFlags.ImGuiTableColumnFlags_None);
                imgui.TableSetupColumn("Value"u8, ImGuiTableColumnFlags.ImGuiTableColumnFlags_None);
                imgui.TableHeadersRow();

                foreach (var property in op.Properties)
                    RenderPropertyRow(imgui, operationId, property);

                imgui.TableNextRow();
                imgui.TableSetColumnIndex(0);

                imgui.SetNextItemWidth(-1.0f);
                if (imgui.BeginCombo($"##prop_picker{operationId}", "➕ Add property...", ImGuiComboFlags.ImGuiComboFlags_None))
                {
                    if (!_propertyFilters.TryGetValue(op.Type, out IDisposableHandle<IImGuiTextFilter>? filterHandle))
                        filterHandle = imgui.CreateTextFilter();

                    IImGuiTextFilter filter = filterHandle.Value;

                    imgui.SetNextItemWidth(-1.0f);
                    if (imgui.InputTextWithHint("##PropertyFilter"u8, "Filter property..."u8, filter.InputBuf.AsSpan(), ImGuiInputTextFlags.ImGuiInputTextFlags_None))
                        imgui.ImGuiTextFilter_Build(filter);

                    if (imgui.IsWindowAppearing())
                    {
                        imgui.SetKeyboardFocusHere();
                        imgui.ImGuiTextFilter_Clear(filter);
                    }

                    foreach (MagicPropertyType prop in op.SupportedProperties)
                    {
                        bool pushedColor = false;
                        if (!MagicPropertyValueTypeMapping.TypeToValueType.TryGetValue(prop, out MagicPropertyValueType valueType))
                        {
                            imgui.PushStyleColor(ImGuiCol.ImGuiCol_Text, ColorUtils.RGBA(255, 0, 0, 255));
                            pushedColor = true;
                        }
                        else if (op.Properties.Any(e => e.Type == prop))
                        {
                            imgui.PushStyleColor(ImGuiCol.ImGuiCol_Text, ColorUtils.RGBA(85, 51, 51, 255));
                            pushedColor = true;
                        }

                        string name = valueType != 0 ? $"{prop} ({valueType})" : prop.ToString();

                        if (imgui.ImGuiTextFilter_PassFilter(filter, name, null))
                        {
                            if (imgui.Selectable(name))
                            {
                                var defaultProperty = MagicPropertyFactory.Create(prop);
                                if (defaultProperty is null)
                                    shell.LogWriteLine(nameof(MagicEditor), $"Property {prop}'s value type is not supported!", color: System.Drawing.Color.Red);
                                else
                                    op.Properties.Add(defaultProperty);
                            }
                        }

                        if (pushedColor)
                            imgui.PopStyleColor();
                    }
                    imgui.EndCombo();
                }

                if (_pendingPropertyDelete is not null)
                {
                    op.Properties.Remove(_pendingPropertyDelete);
                    _pendingPropertyDelete = null;
                }

                imgui.EndTable();
            }

            imgui.TreePop();
        }
    }

    private unsafe void RenderPropertyRow(IImGui imgui, string operationId, MagicOperationProperty property)
    {
        string propId = $"{operationId}-{property.Type}";
        imgui.TableNextRow();

        imgui.TableSetColumnIndex(0);
        if (imgui.Button($"❌##del_prop_{propId}"))
            _pendingPropertyDelete = property;
        imgui.SameLine();

        imgui.Text(property.Type.ToString());

        imgui.TableSetColumnIndex(1);
        if (property.Value is not null)
        {
            string valueId = $"##value_{propId}";
            switch (property.Value)
            {
                case MagicPropertyIdValue idValue:
                    imgui.InputInt(valueId, ref idValue.Id);
                    break;
                case MagicPropertyFloatValue floatValue:
                    imgui.InputFloat(valueId, ref floatValue.Value);
                    break;
                case MagicPropertyIntValue intValue:
                    imgui.InputInt(valueId, ref intValue.Value);
                    break;
                case MagicPropertyBoolValue boolValue:
                    imgui.Checkbox(valueId, ref boolValue.Value);
                    break;
                case MagicPropertyByteValue byteValue:
                    imgui.InputScalar(valueId, ImGuiDataType.ImGuiDataType_U8, Unsafe.AsPointer(ref byteValue.Value));
                    break;
                case MagicPropertyVec3Value vec3Value:
                    imgui.InputFloat3(valueId, ref vec3Value.Value);
                    break;
                default:
                    imgui.TextColored(new Vector4(0.8f, 0.0f, 0.0f, 1.0f), "MISSING VALUE HANDLER");
                    break;
            }
        }

        imgui.TableSetColumnIndex(2);
        imgui.Text($"Value: {string.Join(" ", property.Data.Select(e => e.ToString("X2")))}");
    }
}
