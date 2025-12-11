using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Abstractions;

namespace FF16Framework.ImGuiManager.Windows;

public unsafe class GameOverlay : IImGuiComponent
{
    public bool IsOverlay => true;

    private bool _open = true;

    private readonly IImGui _imGui;

    public GameOverlay(IImGui imGui)
    {
        _imGui = imGui;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        _imGui.MenuItemBoolPtr("Enable Overlay"u8, ""u8, ref _open, true);
    }

    public void Render(IImGuiShell imGuiShell)
    {
        if (!_open)
            return;

        float barHeight = 0;
        if (imGuiShell.IsMainMenuOpen)
            barHeight += _imGui.GetFrameHeight();

        _imGui.SetNextWindowBgAlpha(0.35f);
        if (_imGui.Begin("overlay"u8, ref _open, ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration |
            ImGuiWindowFlags.ImGuiWindowFlags_NoDocking |
            ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize |
            ImGuiWindowFlags.ImGuiWindowFlags_NoSavedSettings |
            ImGuiWindowFlags.ImGuiWindowFlags_NoFocusOnAppearing |
            ImGuiWindowFlags.ImGuiWindowFlags_NoNav))
        {
            _imGui.SetWindowPos(new Vector2()
            {
                X = _imGui.GetIO().DisplaySize.X - _imGui.GetWindowWidth() - 10,
                Y = barHeight + 5 /* padding */
            }, ImGuiCond.ImGuiCond_Always);
        }

        _imGui.End();
    }
}
