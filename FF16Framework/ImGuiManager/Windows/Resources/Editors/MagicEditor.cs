using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using FF16Framework.Services.ResourceManager;
using FF16Framework.Services.Faith.GameApis.Magic;
using FF16Framework.Utils;
using FF16Framework.Interfaces.GameApis.Magic;

using FF16Tools.Files.Magic;
using FF16Tools.Files.Magic.Factories;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;

namespace FF16Framework.ImGuiManager.Windows.Resources.Editors;

public enum ActorSelection
{
    None,
    PlayerActor,
    CameraLockedActor
}

public class MagicEditor
{
    public ResourceHandle Resource;
    public MagicFile MagicFile;
    public bool IsOpen = true;

    private readonly IMagicApi _magicApi;
    private MagicEntry? _currentEntry;
    private string _selectedName;

    private ActorSelection _castSource = ActorSelection.PlayerActor;
    private ActorSelection _castTarget = ActorSelection.CameraLockedActor;

    private MagicOperationGroup? _pendingGroupDelete;
    private MagicOperationProperty? _pendingPropertyDelete;
    private IOperation? _pendingOperationDelete;

    private IDisposableHandle<IImGuiTextFilter> _magicFilterHandle;
    private IDisposableHandle<IImGuiTextFilter> _operationFilterHandle;
    private Dictionary<MagicOperationType, IDisposableHandle<IImGuiTextFilter>> _propertyFilters = [];

    private IOperation? _addedOperation;
    private int _inputGroup;

    public MagicEditor(ResourceHandle resource, MagicFile magicFile, IMagicApi magicApi)
    {
        Resource = resource;
        MagicFile = magicFile;
        _magicApi = magicApi;
    }

    public unsafe void Render(IImGuiShell shell, IImGui imgui)
    {
        if (imgui.Begin($"Magic Editor ({Marshal.PtrToStringAnsi(Resource.FileNamePointer)})", ref IsOpen))
        {
            imgui.SeparatorText(Resource.FileNameSpan);

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
                            _currentEntry = null;

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
            if (imgui.IsItemHovered())
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
                if (imgui.InputTextWithHint("##MagicFilter"u8, "🔍 Filter magics..."u8, filter.InputBuf.AsSpan()))
                    imgui.ImGuiTextFilter_Build(filter);

                if (imgui.IsWindowAppearing())
                {
                    imgui.SetKeyboardFocusHere();
                    imgui.ImGuiTextFilter_Clear(filter);
                }

                imgui.Separator();

                foreach (KeyValuePair<uint, MagicEntry> elem in MagicFile.MagicEntries)
                {
                    if (!MagicIdsMapping.IdToName.TryGetValue(elem.Key, out string? name))
                        name = "Unknown";

                    string item = $"{name} ({elem.Key})";
                    if (imgui.ImGuiTextFilter_PassFilter(filter, item, null))
                    {
                        if (imgui.Selectable(item))
                        {
                            _currentEntry = elem.Value;
                            _selectedName = item;
                        }
                    }

                    if (elem.Value == _currentEntry)
                        imgui.SetItemDefaultFocus();
                }

                imgui.EndCombo();
            }

            if (_currentEntry is not null)
            {
                imgui.SeparatorText($"Magic: {_selectedName}");

                imgui.Text("Source:"u8);
                imgui.SameLine();
                imgui.SetNextItemWidth(150f);
                if (imgui.BeginCombo("##Source", _castSource.ToString()))
                {
                    foreach (var val in Enum.GetValues<ActorSelection>())
                    {
                        if (imgui.SelectableEx(val.ToString(), _castSource == val, ImGuiSelectableFlags.ImGuiSelectableFlags_None, Vector2.Zero))
                            _castSource = val;
                    }
                    imgui.EndCombo();
                }

                imgui.SameLine();
                imgui.Text("Target:"u8);
                imgui.SameLine();
                imgui.SetNextItemWidth(150f);
                if (imgui.BeginCombo("##Target", _castTarget.ToString()))
                {
                    foreach (var val in Enum.GetValues<ActorSelection>())
                    {
                        if (imgui.SelectableEx(val.ToString(), _castTarget == val, ImGuiSelectableFlags.ImGuiSelectableFlags_None, Vector2.Zero))
                            _castTarget = val;
                    }
                    imgui.EndCombo();
                }

                imgui.SameLine();
                imgui.PushStyleColor(ImGuiCol.ImGuiCol_Button, ColorUtils.RGBA(0, 112, 192, 255));
                if (imgui.Button("🚀 Cast!"u8))
                {
                    nint source = _castSource switch {
                        ActorSelection.PlayerActor => _magicApi.GetPlayerActor(),
                        ActorSelection.CameraLockedActor => _magicApi.GetLockedTarget(),
                        _ => nint.Zero
                    };

                    bool success;
                    if (_castTarget == ActorSelection.CameraLockedActor)
                    {
                        success = _magicApi.CastWithGameTarget((int)_currentEntry.Id, source);
                    }
                    else
                    {
                        nint target = _castTarget switch {
                            ActorSelection.PlayerActor => _magicApi.GetPlayerActor(),
                            _ => nint.Zero
                        };
                        success = _magicApi.Cast((int)_currentEntry.Id, source, target);
                    }

                    if (!success)
                        shell.LogWriteLine(nameof(MagicEditor), "Failed to cast magic - API not ready?", Color.Red);
                }
                imgui.PopStyleColor();
                
                imgui.SameLine();
                imgui.PushStyleColor(ImGuiCol.ImGuiCol_Button, ColorUtils.RGBA(64, 128, 64, 255));
                if (imgui.Button("📋 Export to Clipboard"u8))
                {
                    ExportCurrentEntryToClipboard(shell, imgui);
                }
                imgui.PopStyleColor();

                if (imgui.BeginChild($"##magicchild_{_currentEntry.Id}", Vector2.Zero))
                    RenderCurrentMagicEntry(shell, imgui);

                imgui.EndChild();
            }
        }


        imgui.End();
    }

    private unsafe void RenderCurrentMagicEntry(IImGuiShell shell, IImGui imgui)
    {
        RenderGroupSelector(shell, imgui);
        imgui.Text("Right clicking groups/operations for deletion is also supported.");
        RenderGroups(shell, imgui);

        if (_pendingGroupDelete is not null)
        {
            _currentEntry!.OperationGroupList.OperationGroups.Remove(_pendingGroupDelete);
            _pendingGroupDelete = null;
        }
    }

    private unsafe void RenderGroupSelector(IImGuiShell shell, IImGui imgui)
    {
        imgui.PushItemWidth(100f);
        imgui.InputInt("Group Id"u8, ref _inputGroup);
        imgui.PopItemWidth();
        imgui.SameLine();
        if (imgui.Button("Add group"u8))
        {
            bool canAdd = true;
            foreach (var magic in MagicFile.MagicEntries)
            {
                foreach (var group in magic.Value.OperationGroupList.OperationGroups)
                {
                    if (group.Id == _inputGroup)
                    {
                        canAdd = false;
                        shell.LogWriteLine(nameof(MagicEditor), $"A group with id {_inputGroup} already exists in this file.", Color.Yellow);
                    }
                }
            }

            if (_inputGroup < 0)
            {
                canAdd = false;
                shell.LogWriteLine(nameof(MagicEditor), $"Group id must be >= 0", Color.Yellow);
            }

            if (canAdd)
                _currentEntry!.OperationGroupList.OperationGroups.Add(new MagicOperationGroup() { Id = (uint)_inputGroup });
        }
    }

    private unsafe void RenderGroups(IImGuiShell shell, IImGui imgui)
    {
        foreach (MagicOperationGroup group in _currentEntry!.OperationGroupList.OperationGroups)
        {
            bool visible = true;
            bool groupOpen = imgui.CollapsingHeaderBoolPtr($"Group {group.Id}", ref visible, ImGuiTreeNodeFlags.ImGuiTreeNodeFlags_DefaultOpen);

            string deleteGroupPopupName = $"Delete Group##delgroup_modal{group.Id}";
            bool deletePopupOpen = false;
            if (imgui.BeginPopupContextItem())
            {
                if (imgui.MenuItem("Delete Group"u8))
                    deletePopupOpen = true;

                imgui.EndPopup();
            }

            if (groupOpen)
            {
                if (!visible)
                    deletePopupOpen = true;
            }

            if (deletePopupOpen)
                imgui.OpenPopup(deleteGroupPopupName);

            imgui.SetNextWindowPosEx(imgui.ImGuiViewport_GetCenter(imgui.GetMainViewport()), ImGuiCond.ImGuiCond_Appearing, pivot: new Vector2(0.5f, 0.5f));
            if (imgui.BeginPopupModal(deleteGroupPopupName, ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize))
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

            if (groupOpen)
            {
                foreach (IOperation op in group.OperationList.Operations)
                {
                    RenderOperation(shell, imgui, group, op);
                }

                imgui.SetNextItemWidth(-1.0f);
                if (imgui.BeginCombo($"##op_picker{group.Id}", "➕ Add operation...", ImGuiComboFlags.ImGuiComboFlags_HeightLarge))
                {
                    _operationFilterHandle ??= imgui.CreateTextFilter();
                    IImGuiTextFilter filter = _operationFilterHandle.Value;

                    imgui.SetNextItemWidth(-1.0f);
                    if (imgui.InputTextWithHint("##OperationFilter"u8, "🔍 Filter operations..."u8, filter.InputBuf.AsSpan()))
                        imgui.ImGuiTextFilter_Build(filter);

                    if (imgui.IsWindowAppearing())
                    {
                        imgui.SetKeyboardFocusHere();
                        imgui.ImGuiTextFilter_Clear(filter);
                    }

                    imgui.Separator();

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
            }
            
            if (_pendingOperationDelete is not null)
            {
                group.OperationList.Operations.Remove(_pendingOperationDelete);
                _pendingOperationDelete = null;
            }
        }
    }

    private unsafe void RenderOperation(IImGuiShell shell, IImGui imgui, MagicOperationGroup group, IOperation op)
    {
        string operationId = $"{_currentEntry!.Id}-{group.Id}-{op.Type}";

        if (_addedOperation == op)
        {
            imgui.SetNextItemOpen(true, ImGuiCond.ImGuiCond_Once);
            _addedOperation = null;
        }

        bool isExpanded = imgui.TreeNodeEx($"{op.Type}##{operationId}", ImGuiTreeNodeFlags.ImGuiTreeNodeFlags_AllowOverlap);
        if (imgui.BeginPopupContextItem())
        {
            if (imgui.MenuItem("Delete Operation"u8))
                _pendingOperationDelete = op;

            imgui.EndPopup();
        }

        float right = imgui.GetContentRegionAvail().X;

        float offset = isExpanded ? 0.0f : 24f;  // Not sure why I need to offset when expanded. Otherwise it goes to the left and leaves a gap
        imgui.SameLineEx(right - offset, 0.0f);
        if (imgui.SmallButton($"❌##del_{operationId}"))
            _pendingOperationDelete = op;

        if (isExpanded)
        {
            if (imgui.BeginTable($"##proptable_{operationId}", 2, ImGuiTableFlags.ImGuiTableFlags_Borders | ImGuiTableFlags.ImGuiTableFlags_Resizable | ImGuiTableFlags.ImGuiTableFlags_RowBg))
            {
                imgui.TableSetupColumn("Type"u8);
                imgui.TableSetupColumn("Value"u8);
                imgui.TableHeadersRow();

                foreach (var property in op.Properties)
                    RenderPropertyRow(imgui, operationId, property);

                imgui.TableNextRow();
                imgui.TableSetColumnIndex(0);

                imgui.SetNextItemWidth(-1.0f);
                if (imgui.BeginCombo($"##prop_picker{operationId}", "➕ Add property...", ImGuiComboFlags.ImGuiComboFlags_HeightLarge))
                {
                    if (!_propertyFilters.TryGetValue(op.Type, out IDisposableHandle<IImGuiTextFilter>? filterHandle))
                        filterHandle = imgui.CreateTextFilter();

                    IImGuiTextFilter filter = filterHandle.Value;

                    imgui.SetNextItemWidth(-1.0f);
                    if (imgui.InputTextWithHint("##PropertyFilter"u8, "🔍 Filter property..."u8, filter.InputBuf.AsSpan()))
                        imgui.ImGuiTextFilter_Build(filter);

                    if (imgui.IsWindowAppearing())
                    {
                        imgui.SetKeyboardFocusHere();
                        imgui.ImGuiTextFilter_Clear(filter);
                    }

                    imgui.Separator();

                    foreach (MagicPropertyType prop in op.SupportedProperties)
                    {
                        bool pushedColor = false;
                        bool canSelect = true;
                        if (!MagicPropertyValueTypeMapping.TypeToValueType.TryGetValue(prop, out MagicPropertyValueType valueType))
                        {
                            imgui.PushStyleColor(ImGuiCol.ImGuiCol_Text, ColorUtils.RGBA(255, 0, 0, 255));
                            pushedColor = true;
                        }
                        else if (op.Properties.Any(e => e.Type == prop))
                        {
                            imgui.PushStyleColor(ImGuiCol.ImGuiCol_Text, ColorUtils.RGBA(95, 61, 61, 255));
                            pushedColor = true;
                            canSelect = false;
                        }

                        string name = valueType != 0 ? $"{prop} ({valueType})" : prop.ToString();

                        if (imgui.ImGuiTextFilter_PassFilter(filter, name, null))
                        {
                            var flags = ImGuiSelectableFlags.ImGuiSelectableFlags_None;
                            if (!canSelect)
                                flags |= ImGuiSelectableFlags.ImGuiSelectableFlags_Disabled;

                            if (imgui.SelectableEx(name, false, flags, Vector2.Zero))
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

        bool pushedColor = false;
        if (!MagicPropertyValueTypeMapping.TypeToValueType.TryGetValue(property.Type, out MagicPropertyValueType valueType))
        {
            imgui.PushStyleColor(ImGuiCol.ImGuiCol_Text, ColorUtils.RGBA(255, 0, 0, 255));
            pushedColor = true;
        }

        imgui.Text(property.Type.ToString());
        if (pushedColor)
            imgui.PopStyleColor();

        if (valueType != MagicPropertyValueType.Unknown && imgui.IsItemHovered())
            imgui.SetTooltip(valueType.ToString());

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
        else
        {
            imgui.Text($"Bytes: {string.Join(" ", property.Data.Select(e => e.ToString("X2")))}");
        }
    }
    
    // ========================================
    // EXPORT TO JSON
    // ========================================
    
    private void ExportCurrentEntryToClipboard(IImGuiShell shell, IImGui imgui)
    {
        if (_currentEntry == null)
        {
            shell.LogWriteLine(nameof(MagicEditor), "No magic entry selected", Color.Yellow);
            return;
        }
        
        try
        {
            var json = MagicExporter.ExportToJson(_currentEntry);
            
            // Copy to clipboard using ImGui API
            imgui.SetClipboardText(json);
            
            var config = MagicExporter.BuildMagicSpellConfig(_currentEntry);
            shell.LogWriteLine(nameof(MagicEditor), $"Exported Magic ID {_currentEntry.Id} to clipboard ({config.Modifications.Count} modifications)", Color.LightGreen);
        }
        catch (Exception ex)
        {
            shell.LogWriteLine(nameof(MagicEditor), $"Export failed: {ex.Message}", Color.Red);
        }
    }
}
