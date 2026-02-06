using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Collections.Frozen;
using System.Diagnostics;

using FF16Framework.Faith.Hooks;
using FF16Framework.Services.ResourceManager;
using FF16Framework.ImGuiManager.Windows.Resources.Editors;

using FF16Tools.Files.Magic;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using NenTools.ImGui.Shell;

using Reloaded.Mod.Interfaces;


using FF16Framework.Interfaces.GameApis.Magic;

namespace FF16Framework.ImGuiManager.Windows.Resources;

public class ResourceManagerWindow : IImGuiComponent
{
    private readonly IImGui _imGui;
    private readonly IModConfig _modConfig;
    private readonly IModLoader _modLoader;
    private readonly ResourceManagerService _resourceManagerService;
    private readonly IMagicApi _magicApi;

    public bool IsOverlay => false;
    public bool IsOpen = false;

    private ConcurrentDictionary<nint, MagicEditor> _magicEditors = [];

    public FrozenDictionary<string, string> _resourceNamesPerExtension = new Dictionary<string, string>
    {
        [".anmb"] = "Animation/Havok Binary",
        [".apb"] = "SQEX Sead Audio Library - AudioSe Param Binary",
        [".bnfb"] = "Bonamik F Binary",
        [".bnmb"] = "Bonamik Binary",
        [".ccb"] = "Chara Collision Binary",
        [".cfb"] = "Camera Fade Binary",
        [".csb"] = "Cutscene Set Binary",
        [".dep"] = "Magic Dependencies",
        [".eqsbin"] = "EQS Binary",
        [".fnt"] = "Font Resource",
        [".gid"] = "Map Global Illumination Data",
        [".gtex"] = "Map ? Texture",
        [".idl"] = "Chara Timeline ID List",
        [".ikb"] = "IK Binary",
        [".kdb"] = "KineDriver Binary",
        [".ker"] = "Font Additional Kerning Data",
        [".lsb"] = "Lipsync Binary",
        [".mdl"] = "Model Resource",
        [".mgb"] = "Map Merge Grid Binary",
        [".mpb"] = "Map Binary",
        [".mseq"] = "MSequence",
        [".mtl"] = "Material Resource",
        [".nxd"] = "NEX/Next Excel Data",
        [".nxl"] = "NEX/Next Excel Table List",
        [".magic"] = "Magic/Spell Resource",
        [".pac"] = "Pack Resource",
        [".pzd"] = "Panzer Data",
        [".sab"] = "SQEX Sead Audio Library - Sead Audio Binary",
        [".shb"] = "Shader/Technique Binary",
        [".skl"] = "Skeleton/Havok Binary",
        [".sndenv"] = "Map Sound Environment",
        [".spd8"] = "SpeedTree Resource",
        [".srope"] = "Spline Rope",
        [".ssb"] = "Stage Set Binary",
        [".tec"] = "Shader/Technique Binary",
        [".tera"] = "Map Terrain Binary",
        [".tnb"] = "Tanebi (?) Binary",
        [".tex"] = "Texture Resource",
        [".tlb"] = "Chara Timeline Binary",
        [".uib"] = "UI Binary",
        [".utexpt"] = "UI Texture Parts",
        [".vatb"] = "VFX/Audio Table Binary",
        [".vfxb"] = "VFX Binary",
    }.ToFrozenDictionary();

    private bool _010EditorInstalled = false;

    public ResourceManagerWindow(IImGui imgui, IModConfig modConfig, IModLoader modLoader, ResourceManagerService resourceManagerService, IMagicApi magicApi)
    {
        _imGui = imgui;
        _modConfig = modConfig;
        _modLoader = modLoader;
        _resourceManagerService = resourceManagerService;
        _magicApi = magicApi;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (_imGui.MenuItemEx("Resources Manager"u8, ""u8, false, true))
        {
            IsOpen = true;
        }
    }

    public unsafe void Render(IImGuiShell imGuiShell)
    {
        if (!IsOpen)
            return;

        Check010EditorIfNeeded();

        if (_imGui.Begin("Loaded Resources"u8, ref IsOpen, 0))
        {
            _imGui.Text($"Total Resources: {_resourceManagerService.LoadedHandles.Count}");
            foreach (var group in _resourceManagerService.SortedHandles)
            {
                // Span the entire width since we are constructing the text ourselves
                var open = _imGui.TreeNodeEx($"##{group.Key}", ImGuiTreeNodeFlags.ImGuiTreeNodeFlags_SpanAvailWidth);
                _imGui.SameLineEx(0.0f, 0.0f);
                _imGui.Text($"{group.Key} ({group.Value.Count} resources)");

                if (_resourceNamesPerExtension.TryGetValue(group.Key, out string? desc))
                {
                    _imGui.SameLineEx(0.0f, 4.0f);
                    _imGui.TextColored(new Vector4(0.4f, 0.4f, 0.4f, 1.0f), $"- {desc}");
                }


                if (open)
                {
                    foreach (ResourceHandle resource in group.Value.Values)
                    {
                        ReadOnlySpan<byte> fileName = resource.FileNameSpan;
                        if (_imGui.TreeNode(fileName))
                        {
                            _imGui.Text("Handle:"u8);
                            _imGui.SameLine();
                            if (_imGui.TextLink($"{resource.HandleAddress:X}##handle_{resource.HandleAddress:X}"))
                            {
                                string str = $"{resource.HandleAddress:X}";
                                _imGui.SetClipboardText(str);
                                imGuiShell.LogWriteLine("FaithFramework", $"Copied to clipboard: {str}", outputTargetFlags: LoggerOutputTargetFlags.OverlayLogger);
                            }

                            _imGui.Text($"FileBuffer:");
                            _imGui.SameLine();
                            if (_imGui.TextLink($"{resource.BufferAddress:X}##buffer_{resource.HandleAddress:X}"))
                            {
                                string str = $"{resource.BufferAddress:X}";
                                _imGui.SetClipboardText(str);
                                imGuiShell.LogWriteLine("FaithFramework", $"Copied to clipboard: {str}", outputTargetFlags: LoggerOutputTargetFlags.OverlayLogger);
                            }

                            if (resource.BufferAddress != 0)
                            {
                                _imGui.SameLine();
                                if (_010EditorInstalled)
                                {
                                    if (_imGui.SmallButton("Open in 010 Editor"u8))
                                    {
                                        Process.Start(new ProcessStartInfo()
                                        {
                                            FileName = "010editor",
                                            Arguments = $"-process:{Environment.ProcessId} -goto:0x{resource.BufferAddress:X}",
                                            UseShellExecute = true,
                                        });
                                    }
                                }
                                else
                                {
                                    _imGui.BeginDisabled(false);
                                    _imGui.SmallButton("Open in 010 Editor (not found)"u8);
                                    _imGui.EndDisabled();
                                }
                            }

                            _imGui.Text($"FileSize: {resource.FileSize:X}");
                            _imGui.Text($"OpenState: {resource.OpenState}");
                            _imGui.Text($"FormatLoadState: {resource.FormatLoadState}");
                            _imGui.Text($"Flags: {resource.FlagsRaw:X}");

                            if (fileName.EndsWith(".magic"u8))
                            {
                                if (_imGui.Button($"Open Magic##btn_open_magic_{resource.HandleAddress}"))
                                {
                                    if (!_magicEditors.ContainsKey(resource.BufferAddress))
                                    {
                                        try
                                        {
                                            var magic = MagicFile.Open(resource.BufferAddress, resource.FileSize);
                                            _magicEditors.TryAdd(resource.HandleAddress, new MagicEditor(resource, magic, _magicApi));
                                        }
                                        catch (Exception ex)
                                        {
                                            imGuiShell.LogWriteLine(nameof(MagicEditor), $"Failed to read {Marshal.PtrToStringAnsi(resource.FileNamePointer)}: {ex.Message}", Color.Red);
                                        }
                                    }
                                }
                            }

                            _imGui.TreePop();
                        }
                    }

                    _imGui.TreePop();
                }
            }
        }

        _imGui.End();

        int countInvalid = 0;
        foreach (var magicEditor in _magicEditors.Values)
        {
            if (magicEditor.Resource.IsValid && magicEditor.IsOpen)
                magicEditor.Render(imGuiShell, _imGui);
            else
                countInvalid++;
        }

        if (countInvalid > 0)
        {
            List<MagicEditor> oldEditors = new List<MagicEditor>(countInvalid);
            foreach (var magicEditor in _magicEditors.Values)
            {
                if (!magicEditor.Resource.IsValid || !magicEditor.IsOpen)
                    oldEditors.Add(magicEditor);
            }

            foreach (MagicEditor oldEditor in oldEditors)
                _magicEditors.Remove(oldEditor.Resource.HandleAddress, out _);
        }
    }

    private unsafe void Check010EditorIfNeeded()
    {
        string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        if (File.Exists(Path.Combine(programFiles, "010 Editor", "010Editor.exe")))
            _010EditorInstalled = true;
        else
            _010EditorInstalled = false;
        
    }
}
