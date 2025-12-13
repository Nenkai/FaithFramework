using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Abstractions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGuiManager.Windows.Framework;

public class SettingsComponent(IImGui imGui, GameContext gameContext, ImGuiShellConfig shellConfig, FrameworkConfig overlayConfig) : IImGuiComponent
{
    public bool IsOverlay => false;

    private readonly IImGui _imGui = imGui;
    private readonly GameContext gameContext = gameContext;
    private readonly ImGuiShellConfig _shellConfig = shellConfig;
    private readonly FrameworkConfig _frameworkConfig = overlayConfig;

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        _imGui.SeparatorText("🔧 Settings"u8);
        if (gameContext.GameType == FaithGameType.FFXVI)
        {
            if (_imGui.BeginMenu("Info Overlay"u8))
            {
                _imGui.MenuItemBoolPtr("Show current actor info"u8, ""u8, ref _frameworkConfig.GameInfoOverlay.ShowActorInfoField, true);
                _imGui.EndMenu();
            }
        }

        if (_imGui.BeginMenu("Logger Overlay"u8))
        {
            if (_imGui.MenuItemBoolPtr("Enable Logger Overlay"u8, ""u8, ref _shellConfig.OverlayLogger.EnabledField, true))
            {
                if (_shellConfig.OverlayLogger.Enabled)
                    imGuiShell.LogWriteLine("FaithFramework", "Overlay logger is now enabled.", outputTargetFlags: LoggerOutputTargetFlags.All);
            }

            _imGui.PushItemWidth(100);
            _imGui.SliderInt("Max lines"u8, ref _shellConfig.OverlayLogger.MaxLinesField, 1, 150);
            _imGui.PopItemWidth();

            _imGui.PushItemWidth(100);
            _imGui.SliderFloat("Fade time"u8, ref _shellConfig.OverlayLogger.FadeTimeSecondsField, 1, 20.0f);
            _imGui.PopItemWidth();

            if (_imGui.Button("Test overlay logger"u8))
            {
                for (int i = 0; i < _shellConfig.OverlayLogger.MaxLines; i++)
                    imGuiShell.LogWriteLine("FaithFramework", $"#{i} Overlay logger test!", outputTargetFlags: LoggerOutputTargetFlags.OverlayLogger);
            }
            _imGui.EndMenu();
        }

        if (_imGui.Button("Save"))
        {
            try
            {
                _shellConfig.Save();
                _frameworkConfig.Save();
                imGuiShell.LogWriteLine("FaithFramework", "Framework config saved.", outputTargetFlags: LoggerOutputTargetFlags.All);
            }
            catch (Exception ex)
            {
                imGuiShell.LogWriteLine("FaithFramework", "Failed to write config.", color: Color.Red, outputTargetFlags: LoggerOutputTargetFlags.All);
            }
        }
    }

    public void Render(IImGuiShell imGuiShell)
    {

    }
}
