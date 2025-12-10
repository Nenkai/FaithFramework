using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Shell.Interfaces;

using Reloaded.Mod.Interfaces;

namespace FF16Framework.ImGuiManager.Windows;

[ImGuiMenu(Category = "Other", Priority = ImGuiShell.SystemPriority, Owner = nameof(FF16Framework))]
public class AboutWindow : IImGuiComponent
{
    public bool IsOverlay => false;
    public bool IsOpen = false;

    private readonly IImGui _imGui;
    private readonly IModConfig _modConfig;
    private readonly IModLoader _modLoader;

    public AboutWindow(IImGui imgui, IModConfig modConfig, IModLoader modLoader)
    {
        _imGui = imgui;
        _modConfig = modConfig;
        _modLoader = modLoader;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (_imGui.MenuItemEx("About Window"u8, ""u8, false, true))
        {
            IsOpen = true;
        }
    }

    private IQueuedImGuiImage? _iconImage;
    private CancellationTokenSource _loadCts = new CancellationTokenSource();

    public void Render(IImGuiShell imGuiShell)
    {
        if (!IsOpen)
        {
            UnloadResourcesIfNeeded();
            return;
        }

        if (_iconImage is null)
        {
            string path = _modLoader.GetDirectoryForModId(_modConfig.ModId);
            _iconImage = imGuiShell.TextureManager.QueueImageLoad(Path.Combine(path, _modConfig.ModIcon), ct: _loadCts.Token);
        }

        if (_imGui.Begin("About Window"u8, ref IsOpen, 0))
        {
            float windowWidth = _imGui.GetWindowSize().X;
            if (_iconImage.IsLoaded)
            {
                _imGui.SetCursorPosX((windowWidth - _iconImage.Image.Width) * 0.5f);
                _imGui.Image(_imGui.CreateTextureRef(_iconImage.Image.TexId), new System.Numerics.Vector2(_iconImage.Image.Width, _iconImage.Image.Height));
            }

            string mainText = $"{_modConfig.ModId} {_modConfig.ModVersion} by {_modConfig.ModAuthor}";
            _imGui.SetCursorPosX((windowWidth - _imGui.CalcTextSize(mainText).X) * 0.5f);
            _imGui.Text(mainText);
            _imGui.Spacing();

            _imGui.Text("Support Me:"u8); _imGui.SameLine(); _imGui.TextLinkOpenURL("https://ko-fi.com/nenkai"u8);
            _imGui.Text("Github:"u8); _imGui.SameLine(); _imGui.TextLinkOpenURL("https://github.com/Nenkai/FF16Framework"u8);
            _imGui.Text("Nexus Mods:"u8); _imGui.SameLine(); _imGui.TextLinkOpenURL("https://www.nexusmods.com/finalfantasy16/mods/138"u8);
            _imGui.Spacing();

            _imGui.Text("Keys:"u8);
            _imGui.Text("- INSERT: Show ImGui Menu"u8);
            _imGui.Spacing();

            _imGui.Text("NOTE: Logs are also saved as a file in the game's directory as 'modtools_log.txt'."u8);
        }

        _imGui.End();
    }

    private void UnloadResourcesIfNeeded()
    {
        _iconImage?.Image?.Dispose();
        _iconImage = null;
    }
}
