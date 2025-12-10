using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Shell.Interfaces;

namespace FF16Framework.ImGuiManager.Windows;

[ImGuiMenu(Category = "Tools", Priority = ImGuiShell.SystemPriority, Owner = nameof(FF16Framework))]
public class SettingsComponent : IImGuiComponent
{
    public bool IsOverlay => false;

    private readonly ImGuiConfig _config;
    private readonly IImGui _imGui;

    public SettingsComponent(IImGui imGui, ImGuiConfig config)
    {
        _imGui = imGui;
        _config = config;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (_imGui.BeginMenu("Faith Framework"u8))
        {
            _imGui.SeparatorText("Settings"u8);
            if (_imGui.BeginMenu("Overlay"u8))
            {
                if (_imGui.MenuItemBoolPtr("Enable Overlay Logger"u8, ""u8, ref _config.OverlayLogger.EnabledField, true))
                {
                    if (_config.OverlayLogger.Enabled)
                        imGuiShell.LogWriteLine("FaithFramework", "Overlay logger is now enabled.", outputTargetFlags: LoggerOutputTargetFlags.All);
                }

                _imGui.PushItemWidth(100);
                _imGui.SliderInt("Max lines"u8, ref _config.OverlayLogger.MaxLinesField, 1, 150);
                _imGui.PopItemWidth();

                _imGui.PushItemWidth(100);
                _imGui.SliderFloat("Fade time"u8, ref _config.OverlayLogger.FadeTimeSecondsField, 1, 20.0f);
                _imGui.PopItemWidth();

                if (_imGui.Button("Test overlay logger"u8))
                {
                    for (int i = 0; i < _config.OverlayLogger.MaxLines; i++)
                        imGuiShell.LogWriteLine("FaithFramework", $"#{i} Overlay logger test!", outputTargetFlags: LoggerOutputTargetFlags.OverlayLogger);
                }
                _imGui.Separator();

                if (_imGui.Button("Save"))
                {
                    try
                    {
                        _config.Save();
                        imGuiShell.LogWriteLine("FaithFramework", "Framework config saved.", outputTargetFlags: LoggerOutputTargetFlags.All);
                    }
                    catch (Exception ex)
                    {
                        imGuiShell.LogWriteLine("FaithFramework", "Failed to write config.", color: Color.Red, outputTargetFlags: LoggerOutputTargetFlags.All);
                    }
                }
                _imGui.EndMenu();
            }
            _imGui.EndMenu();
        }
    }

    public void Render(IImGuiShell imGuiShell)
    {
        
    }
}
