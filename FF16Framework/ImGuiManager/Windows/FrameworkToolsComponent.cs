using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using NenTools.ImGui.Shell;

using FF16Framework.ImGuiManager.Windows.Framework;
using FF16Framework.ImGuiManager.Windows.Resources;

namespace FF16Framework.ImGuiManager.Windows;

[ImGuiMenu(Category = "Mods", Priority = ImGuiShell.SystemPriority, Owner = nameof(FF16Framework))]
public class FrameworkToolsComponent : IImGuiComponent
{
    public bool IsOverlay => false;

    private readonly IImGui _imGui;

    private readonly ResourceManagerWindow _resourceManagerComponent;
    private readonly MainVisualizerComponent _visualizerComponent;
    private readonly DocumentationComponent _documentation;
    private readonly SettingsComponent _settings;

    public FrameworkToolsComponent(IImGui imGui, 
        ResourceManagerWindow resourceManagerWindow,
        MainVisualizerComponent visualizerComponent,
        DocumentationComponent documentationComponent, 
        SettingsComponent settingsComponent)
    {
        _imGui = imGui;
        _resourceManagerComponent = resourceManagerWindow;
        _visualizerComponent = visualizerComponent;
        _documentation = documentationComponent;
        _settings = settingsComponent;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (_imGui.BeginMenu("Faith Framework"u8))
        {
            _resourceManagerComponent.RenderMenu(imGuiShell);
            _visualizerComponent.RenderMenu(imGuiShell);
            _imGui.Separator();
            _documentation.RenderMenu(imGuiShell);
            _settings.RenderMenu(imGuiShell);

            _imGui.EndMenu();
        }
    }

    public void Render(IImGuiShell imGuiShell)
    {
        _resourceManagerComponent.Render(imGuiShell);
        _visualizerComponent.Render(imGuiShell);
    }
}
