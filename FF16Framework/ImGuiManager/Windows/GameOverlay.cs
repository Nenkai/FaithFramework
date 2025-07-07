using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.ImGuiManager;
using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;

namespace FF16Framework.ImGuiManager.Windows;

public unsafe class GameOverlay : IImGuiComponent
{
    public bool IsOverlay => true;

    private bool _open = true;

    public GameOverlay()
    {
    }

    public void RenderMenu(IImGui imgui)
    {
        imgui.MenuItemBoolPtr("Enable Overlay", "", ref _open, true);
    }

    public void Render(IImGuiSupport imguiSupport, IImGui imgui)
    {
        if (!_open)
            return;

        float barHeight = 0;
        if (imguiSupport.IsMainMenuBarOpen)
            barHeight += imgui.GetFrameHeight();

        imgui.SetNextWindowBgAlpha(0.35f);
        if (imgui.Begin("overlay", ref _open, ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration |
            ImGuiWindowFlags.ImGuiWindowFlags_NoDocking |
            ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize |
            ImGuiWindowFlags.ImGuiWindowFlags_NoSavedSettings |
            ImGuiWindowFlags.ImGuiWindowFlags_NoFocusOnAppearing |
            ImGuiWindowFlags.ImGuiWindowFlags_NoNav))
        {
            imgui.SetWindowPos(new Vector2()
            {
                X = imgui.GetIO().DisplaySize.X - imgui.GetWindowWidth() - 10,
                Y = barHeight + 5 /* padding */
            }, ImGuiCond.ImGuiCond_Always);
        }

        imgui.End();
    }
}
