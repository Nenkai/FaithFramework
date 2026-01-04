using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Faith.Hooks;
using FF16Framework.Services.ResourceManager;
using FF16Framework.ImGuiManager.Windows.Resources.Editors;

using FF16Tools.Files.Magic;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using NenTools.ImGui.Shell;

using Reloaded.Mod.Interfaces;

namespace FF16Framework.ImGuiManager.Windows.Resources;

[ImGuiMenu(Category = "Mods", Priority = ImGuiShell.SystemPriority, Owner = nameof(FF16Framework))]
public class ResourceManagerWindow : IImGuiComponent
{
    private readonly IImGui _imGui;
    private readonly IModConfig _modConfig;
    private readonly IModLoader _modLoader;
    private readonly ResourceManagerService _resourceManagerService;

    public bool IsOverlay => false;
    public bool IsOpen = false;

    private Dictionary<nint, MagicEditor> _magicEditors = [];

    public ResourceManagerWindow(IImGui imgui, IModConfig modConfig, IModLoader modLoader, ResourceManagerService resourceManagerService)
    {
        _imGui = imgui;
        _modConfig = modConfig;
        _modLoader = modLoader;
        _resourceManagerService = resourceManagerService;
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

        if (_imGui.Begin("Loaded Resources"u8, ref IsOpen, 0))
        {
            _imGui.Text($"Total Resources: {_resourceManagerService.LoadedHandles.Count}");
            foreach (var group in _resourceManagerService.SortedHandles)
            {
                if (_imGui.TreeNode($"{group.Key} ({group.Value.Count} resources)"))
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
                                        var magic = MagicFile.Open(resource.BufferAddress, resource.FileSize);
                                        _magicEditors.Add(resource.HandleAddress, new MagicEditor(resource, magic));
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
            if (magicEditor.Resource.IsValid)
                magicEditor.Render(imGuiShell, _imGui);
            else
                countInvalid++;
        }

        if (countInvalid > 0)
        {
            List<MagicEditor> oldEditors = new List<MagicEditor>(countInvalid);
            foreach (var magicEditor in _magicEditors.Values)
            {
                if (!magicEditor.Resource.IsValid)
                    oldEditors.Add(magicEditor);
            }

            foreach (MagicEditor oldEditor in oldEditors)
                _magicEditors.Remove(oldEditor.Resource.HandleAddress);
        }
    }
}
