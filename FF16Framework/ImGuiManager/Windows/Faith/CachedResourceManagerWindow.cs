using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Faith.Hooks;
using FF16Framework.Utils;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using NenTools.ImGui.Shell;

using Reloaded.Mod.Interfaces;

namespace FF16Framework.ImGuiManager.Windows.Faith;

[ImGuiMenu(Category = "Mods", Priority = ImGuiShell.SystemPriority, Owner = nameof(FF16Framework))]
public class CachedResourceManagerWindow : IImGuiComponent
{
    public bool IsOverlay => false;
    public bool IsOpen = false;

    private readonly IImGui _imGui;
    private readonly IModConfig _modConfig;
    private readonly IModLoader _modLoader;
    private readonly CachedResourceManagerHooks _cachedResManagerHooks;

    public CachedResourceManagerWindow(IImGui imgui, IModConfig modConfig, IModLoader modLoader, CachedResourceManagerHooks cachedResManagerHooks)
    {
        _imGui = imgui;
        _modConfig = modConfig;
        _modLoader = modLoader;
        _cachedResManagerHooks = cachedResManagerHooks;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (_imGui.MenuItemEx("FFXVI Cached Resources"u8, ""u8, false, true))
        {
            IsOpen = true;
        }
    }

    public SortedDictionary<string, SortedDictionary<string, nint>> sortedHandles { get; set; } = [];
    private uint lastSize;

    public unsafe void Render(IImGuiShell imGuiShell)
    {
        if (!IsOpen)
            return;

        if (_imGui.Begin("FFXVI Cached Resources"u8, ref IsOpen, 0))
        {
            if (_cachedResManagerHooks.ManagerPtr == null)
            {
                _imGui.Text("CachedResourceManager not initialized yet."u8);
            }
            else
            {
                StdMap<ResourceHandle>* map = _cachedResManagerHooks.ManagerPtr->Map; // Key is FNV1A
                uint count = map->Count();
                if (count != lastSize)
                {
                    lastSize = count;
                    RegisterAndSortAllHandlesByExtensionAndName(map);
                }

                _imGui.Text($"Total Resources: {count}");
                foreach (var group in sortedHandles)
                {
                    if (_imGui.TreeNode($"{group.Key} ({group.Value.Count} resources)"))
                    {
                        foreach (var ptr in group.Value)
                        {
                            ResourceHandle* resource = (ResourceHandle*)ptr.Value;
                            if (_imGui.TreeNode(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(resource->FileName)))
                            {
                                _imGui.Text("Handle:"u8);
                                _imGui.SameLine();
                                if (_imGui.TextLink($"{(nint)resource:X}"))
                                {
                                    string str = $"{(nint)resource:X}";
                                    _imGui.SetClipboardText(str);
                                    imGuiShell.LogWriteLine("FaithFramework", $"Copied to clipboard: {str}", outputTargetFlags: LoggerOutputTargetFlags.OverlayLogger);
                                }

                                _imGui.Text($"FileBuffer:");
                                _imGui.SameLine();
                                if (_imGui.TextLink($"{resource->FileBuffer:X}"))
                                {
                                    string str = $"{resource->FileBuffer:X}";
                                    _imGui.SetClipboardText(str);
                                    imGuiShell.LogWriteLine("FaithFramework", $"Copied to clipboard: {str}", outputTargetFlags: LoggerOutputTargetFlags.OverlayLogger);
                                }

                                _imGui.Text($"FileSize: {resource->FileSize:X}");
                                _imGui.Text($"OpenState: {resource->OpenState}");
                                _imGui.Text($"FormatLoadState: {resource->FormatLoadState}");
                                _imGui.Text($"Flags: {resource->Flags:X}");

                                _imGui.TreePop();
                            }
                        }

                        _imGui.TreePop();
                    }
                }
            }
        }

        _imGui.End();
    }

    private unsafe void RegisterAndSortAllHandlesByExtensionAndName(StdMap<ResourceHandle>* map)
    {
        sortedHandles.Clear();

        foreach (var entry in *map)
        {
            string fileName = Marshal.PtrToStringAnsi((nint)entry.Value->FileName);
            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            sortedHandles.TryAdd(extension, []);
            sortedHandles[extension].Add(fileName, (nint)entry.Value);
        }
    }
}
