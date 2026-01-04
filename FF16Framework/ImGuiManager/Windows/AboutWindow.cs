using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Shell;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using NenTools.ImGui.Interfaces.Shell.Textures;

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

                var startPos = _imGui.GetCursorScreenPos();
                _imGui.ImDrawList_AddImageRounded(_imGui.GetWindowDrawList(), _imGui.CreateTextureRef(_iconImage.Image.TexId),
                    startPos, startPos + new Vector2(_iconImage.Image.Width, _iconImage.Image.Height),
                    uv_min: Vector2.Zero, uv_max: Vector2.One,
                    col: 0xFFFFFFFF,
                    rounding: 
                    10f, 
                    (int)ImDrawFlags.ImDrawFlags_None);
                _imGui.Dummy(new Vector2(_iconImage.Image.Height, _iconImage.Image.Height));
            }

            _imGui.PushFontFloat(null, 25.0f);
            string mainText = $"-- FaithFramework {_modConfig.ModVersion} by {_modConfig.ModAuthor} --";
            _imGui.SetCursorPosX((windowWidth - _imGui.CalcTextSize(mainText).X) * 0.5f);
            _imGui.Text(mainText);
            _imGui.Spacing();
            _imGui.PopFont();

            _imGui.Text("🔧 Github:"u8); _imGui.SameLine(); _imGui.TextLinkOpenURL("https://github.com/Nenkai/FaithFramework"u8);
            _imGui.Text("🌐 FFXVI Modding:"u8); _imGui.SameLine(); _imGui.TextLinkOpenURL("https://nenkai.github.io/ffxvi-modding/"u8);
            _imGui.Text("❤ Support Nenkai:"u8); _imGui.SameLine(); _imGui.TextLinkOpenURL("https://ko-fi.com/nenkai"u8);
            _imGui.Spacing();

            _imGui.Text("⌨ Keys:"u8);
            _imGui.Text("- INSERT: Show ImGui Menu"u8);
            _imGui.Spacing();

            _imGui.Text("NOTE: Logs are also saved as a file in the framework's mod directory as 'framework_log.txt'."u8);

            foreach (var font in imGuiShell.FontManager.Fonts)
            {
                _imGui.Text($"Loaded Font: {font.Key} by {font.Value.Owner}");
            }
        }

        _imGui.End();
    }

    private void UnloadResourcesIfNeeded()
    {
        _iconImage?.Image?.Dispose();
        _iconImage = null;
    }
}
