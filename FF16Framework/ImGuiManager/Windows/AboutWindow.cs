using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;

using Reloaded.Mod.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGuiManager.Windows;

public class AboutWindow : IImGuiComponent
{
    public bool IsOverlay => false;
    public bool IsOpen = false;

    private IModConfig _modConfig;
    private IModLoader _modLoader;
    private IImGuiTextureManager _textureManager;

    public AboutWindow(IModConfig modConfig, IModLoader modLoader, IImGuiTextureManager textureManager)
    {
        _modConfig = modConfig;
        _modLoader = modLoader;
        _textureManager = textureManager;
    }

    public void RenderMenu(IImGui imgui)
    {
        if (imgui.MenuItemEx("About Window", "", false, true))
        {
            IsOpen = true;
        }
    }

    private IQueuedImGuiImage? _iconImage;
    private CancellationTokenSource _loadCts = new CancellationTokenSource();

    public void Render(IImGuiSupport imguiSupport, IImGui imgui)
    {
        if (!IsOpen)
        {
            UnloadResourcesIfNeeded();
            return;
        }

        if (_iconImage is null)
        {
            string path = _modLoader.GetDirectoryForModId(_modConfig.ModId);
            _iconImage = _textureManager.QueueImageLoad(Path.Combine(path, _modConfig.ModIcon), ct: _loadCts.Token);
        }

        if (imgui.Begin("About Window", ref IsOpen, 0))
        {
            float windowWidth = imgui.GetWindowSize().X;
            if (_iconImage.IsLoaded)
            {
                imgui.SetCursorPosX((windowWidth - _iconImage.Image.Width) * 0.5f);
                imgui.Image(new ImTextureRef() { TexID = _iconImage.Image.TexId }, new System.Numerics.Vector2(_iconImage.Image.Width, _iconImage.Image.Height));
            }

            string mainText = $"{_modConfig.ModId} {_modConfig.ModVersion} by {_modConfig.ModAuthor}";
            imgui.SetCursorPosX((windowWidth - imgui.CalcTextSize(mainText).X) * 0.5f);
            imgui.Text(mainText);
            imgui.Spacing();

            imgui.Text("Support Me:"); imgui.SameLine(); imgui.TextLinkOpenURL("https://ko-fi.com/nenkai");
            imgui.Text("Github:"); imgui.SameLine(); imgui.TextLinkOpenURL("https://github.com/Nenkai/FF16Framework");
            imgui.Text("Nexus Mods:"); imgui.SameLine(); imgui.TextLinkOpenURL("https://www.nexusmods.com/finalfantasy16/mods/138");
            imgui.Spacing();

            imgui.Text("Keys:");
            imgui.Text("- INSERT: Show ImGui Menu");
            imgui.Spacing();

            imgui.Text("NOTE: Logs are also saved as a file in the game's directory as 'modtools_log.txt'.");
        }

        imgui.End();
    }

    private void UnloadResourcesIfNeeded()
    {
        _iconImage?.Image?.Dispose();
        _iconImage = null;
    }
}
