using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using NenTools.ImGui.Shell;

using FF16Framework.ImGuiManager.Windows.Visualizers;

namespace FF16Framework.ImGuiManager.Windows;

[ImGuiMenu(Category = "Mods", Priority = ImGuiShell.SystemPriority, Owner = nameof(FF16Framework))]
public unsafe class MainVisualizerComponent : IImGuiComponent
{
    public bool IsOverlay => true;

    private bool _viewEids = false;

    private readonly IImGui _imGui;
    private readonly GameContext _gameContext;
    private readonly EidVisualizerComponent _eidVisualizer;

    public MainVisualizerComponent(IImGui imGui, FrameworkConfig frameworkConfig,
        GameContext gameContext,
        EidVisualizerComponent eidVisualizer)
    {
        _imGui = imGui;
        _gameContext = gameContext;
        _eidVisualizer = eidVisualizer;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (_gameContext.GameType != FaithGameType.FFXVI)
            return;

        if (_imGui.BeginMenu("Visualizers"u8))
        {
            _imGui.MenuItemBoolPtr("Display Eids on current actor"u8, ""u8, ref _viewEids, true);
            _imGui.EndMenu();
        }
    }

    public void Render(IImGuiShell imGuiShell)
    {
        if (_viewEids)
            _eidVisualizer.Render(imGuiShell);
    }
}
