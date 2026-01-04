using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Services.ResourceManager;

using FF16Tools.Files.Magic;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;

namespace FF16Framework.ImGuiManager.Windows.Resources.Editors;

public class MagicEditor
{
    public ResourceHandle Resource;
    public MagicFile MagicFile;
    public bool IsOpen = true;
    private MagicEntry? CurrentEntry;

    public MagicEditor(ResourceHandle resource, MagicFile magicFile)
    {
        Resource = resource;
        MagicFile = magicFile;
    }

    public unsafe void Render(IImGuiShell shell, IImGui imgui)
    {
        if (imgui.Begin("Magic Editor"u8, ref IsOpen, ImGuiWindowFlags.ImGuiWindowFlags_None))
        {
            if (imgui.BeginCombo("Magics"u8, "Select magic..."u8, ImGuiComboFlags.ImGuiComboFlags_None))
            {
                foreach (KeyValuePair<uint, MagicEntry> elem in MagicFile.MagicEntries)
                {
                    if (imgui.Selectable($"ID {elem.Key}"))
                        CurrentEntry = elem.Value;

                    if (elem.Value == CurrentEntry)
                        imgui.SetItemDefaultFocus();
                }

                imgui.EndCombo();
            }

            if (CurrentEntry is not null)
            {
                imgui.SeparatorText($"Magic: {CurrentEntry.Id}");
                if (imgui.Button("Apply to game!"u8))
                {
                    using (var memStream = new MemoryStream())
                    {
                        MagicFile.Write(memStream);

                        byte[] data = memStream.ToArray();
                        Resource.ReplaceBuffer(data);
                    }

                    shell.LogWriteLine(nameof(MagicEditor), "Saved!", outputTargetFlags: LoggerOutputTargetFlags.All);
                }
                if (imgui.BeginChild($"##magicchild_{CurrentEntry.Id}", Vector2.Zero, ImGuiChildFlags.ImGuiChildFlags_None, ImGuiWindowFlags.ImGuiWindowFlags_None))
                {
                    RenderCurrentMagicEntry(shell, imgui);
                }
                imgui.EndChild();
            }
        }


        imgui.End();
    }

    private void RenderCurrentMagicEntry(IImGuiShell shell, IImGui imgui)
    {
        foreach (MagicOperationGroup group in CurrentEntry.OperationGroupList.OperationGroups)
        {
            imgui.Text($"Group {group.Id}");

            foreach (IOperation op in group.OperationList.Operations)
            {
                string operationId = $"{CurrentEntry.Id}-{group.Id}-{op.Type}";
                if (imgui.TreeNodeEx($"{op.Type}##{operationId}", ImGuiTreeNodeFlags.ImGuiTreeNodeFlags_DefaultOpen))
                {
                    if (imgui.BeginTable($"##proptable_{operationId}", 3, ImGuiTableFlags.ImGuiTableFlags_Borders))
                    {
                        imgui.TableSetupColumn("Type"u8, ImGuiTableColumnFlags.ImGuiTableColumnFlags_None);
                        imgui.TableSetupColumn("Value"u8, ImGuiTableColumnFlags.ImGuiTableColumnFlags_None);
                        imgui.TableHeadersRow();

                        MagicOperationProperty? toRemove = null;
                        foreach (var property in op.Properties)
                        {
                            string propId = $"{operationId}-{property.Type}";
                            imgui.TableNextRow();

                            imgui.TableSetColumnIndex(0);
                            if (imgui.Button($"X##del_prop_{propId}"))
                                toRemove = property;
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
                                        unsafe
                                        {
                                            imgui.InputScalar(valueId, ImGuiDataType.ImGuiDataType_U8, Unsafe.AsPointer(ref byteValue.Value));
                                        }
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

                        imgui.TableNextRow();
                        imgui.TableSetColumnIndex(0);

                        if (imgui.BeginCombo($"##prop_picker{operationId}", "Add property...", ImGuiComboFlags.ImGuiComboFlags_None))
                        {
                            foreach (MagicPropertyType prop in op.SupportedProperties)
                            {
                                bool pushedColor = false;
                                if (!MagicPropertyValueFactory.TypeToValueType.ContainsKey(prop))
                                {
                                    imgui.PushStyleColor(ImGuiCol.ImGuiCol_Text, 0xFF0000FF); // R255, G0, B0, A255
                                    pushedColor = true;
                                }
                                else if (op.Properties.Any(e => e.Type == prop))
                                {
                                    imgui.PushStyleColor(ImGuiCol.ImGuiCol_Text, 0x553333FF); // R255, G51, B51, A85
                                    pushedColor = true;
                                }

                                if (imgui.Selectable(prop.ToString()))
                                {
                                    var defaultProperty = MagicPropertyValueFactory.CreateDefault(prop);
                                    if (defaultProperty is null)
                                        shell.LogWriteLine(nameof(MagicEditor), $"Property {prop}'s value type is not supported!", color: System.Drawing.Color.Red, outputTargetFlags: LoggerOutputTargetFlags.All);
                                    else
                                        op.Properties.Add(defaultProperty);
                                }

                                if (pushedColor)
                                    imgui.PopStyleColor();
                            }
                            imgui.EndCombo();
                        }

                        if (toRemove is not null)
                            op.Properties.Remove(toRemove);

                        imgui.EndTable();
                    }
                    imgui.TreePop();
                }
            }
        }
    }
}
