using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

using NenTools.ImGui.Shell;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;

using FF16Framework.ImGuiManager.Windows.Framework;

namespace FF16Framework.ImGuiManager.Windows;

[ImGuiMenu(Category = "Mods", Priority = ImGuiShell.SystemPriority, Owner = nameof(FF16Framework))]
public class FrameworkToolsComponent : IImGuiComponent
{
    public bool IsOverlay => false;

    private readonly IImGui _imGui;

    private readonly DocumentationComponent _documentation;
    private readonly SettingsComponent _settings;

    public FrameworkToolsComponent(IImGui imGui, DocumentationComponent documentationComponent, SettingsComponent settingsComponent)
    {
        _imGui = imGui;
        _documentation = documentationComponent;
        _settings = settingsComponent;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (_imGui.BeginMenu("Faith Framework"u8))
        {
            _documentation.RenderMenu(imGuiShell);
            _settings.RenderMenu(imGuiShell);

            _imGui.EndMenu();
        }
    }

    public void Render(IImGuiShell imGuiShell)
    {

    }
}
