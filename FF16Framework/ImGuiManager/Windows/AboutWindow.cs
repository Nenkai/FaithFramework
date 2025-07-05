using FF16Framework.ImGuiManager;
using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;

using Reloaded.Mod.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGuiManager.Windows;

public unsafe class AboutWindow : IImguiWindow, IImguiMenuComponent
{
    public bool IsOverlay => false;
    public bool IsOpen = false;

    private IModConfig _modConfig;

    public AboutWindow(IModConfig modConfig)
    {
        _modConfig = modConfig;
    }

    public void BeginMenuComponent(IImGui imgui)
    {
        if (imgui.MenuItemEx("About Window", "", false, true))
        {
            IsOpen = true;
        }
    }

    public void Render(IImguiSupport imguiSupport, IImGui imgui)
    {
        if (!IsOpen)
            return;

        if (imgui.Begin("Log Window", ref IsOpen, 0))
        {
            imgui.Text($"{_modConfig.ModId} {_modConfig.ModVersion}");
            imgui.Text($"Made by {_modConfig.ModAuthor}");
            imgui.Spacing();

            imgui.Text("Keys:");
            imgui.Text("- INSERT: Show ImGui Menu");
            imgui.Spacing();

            imgui.Text("NOTE: Logs are also saved as a file in the game's directory as 'modtools_log.txt'.");
        }
        imgui.End();
    }
}
