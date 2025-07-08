using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;

using Reloaded.Mod.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace FF16Framework.ImGuiManager.Windows;

public class SettingsComponent : IImGuiComponent
{
    public bool IsOverlay => false;

    private ImGuiConfig _config;

    public SettingsComponent(ImGuiConfig config)
    {
        _config = config;
    }

    public void RenderMenu(IImGuiSupport imGuiSupport, IImGui imGui)
    {
        if (imGui.BeginMenu("Configuration"))
        {
            imGui.SeparatorText("Overlay Logger");
            if (imGui.MenuItemBoolPtr("Enable Overlay Logger", string.Empty, ref _config.OverlayLogger.EnabledField, true))
            {
                if (_config.OverlayLogger.Enabled)
                    imGuiSupport.LogWriteLine(nameof(FF16Framework), "Overlay logger is now enabled.", outputTargetFlags: LoggerOutputTargetFlags.All);
            }

            imGui.PushItemWidth(100);
            imGui.SliderInt("Max lines", ref _config.OverlayLogger.MaxLinesField, 1, 150);
            imGui.PopItemWidth();

            imGui.PushItemWidth(100);
            imGui.SliderFloat("Fade time", ref _config.OverlayLogger.FadeTimeSecondsField, 1, 20.0f);
            imGui.PopItemWidth();

            if (imGui.Button("Test overlay logger"))
            {
                for (int i = 0; i < _config.OverlayLogger.MaxLines; i++)
                    imGuiSupport.LogWriteLine(nameof(FF16Framework), $"#{i} Overlay logger test!", outputTargetFlags: LoggerOutputTargetFlags.OverlayLogger);
            }
            imGui.Separator();

            if (imGui.Button("Save"))
            {
                try
                {
                    _config.Save();
                    imGuiSupport.LogWriteLine(nameof(FF16Framework), "Framework config saved.", outputTargetFlags: LoggerOutputTargetFlags.All);
                }
                catch (Exception ex)
                {
                    imGuiSupport.LogWriteLine(nameof(FF16Framework), "Failed to write config.", color: Color.Red, outputTargetFlags: LoggerOutputTargetFlags.All);
                }
            }

            imGui.EndMenu();
        }
    }

    public void Render(IImGuiSupport imGuiSupport, IImGui imGui)
    {
        
    }
}
