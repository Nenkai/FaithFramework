using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Shell.Interfaces;

namespace NenTools.ImGui.Shell.Windows;

[ImGuiMenu(Category = "Other", Priority = ImGuiShell.SystemPriority, Owner = nameof(NenTools))]
public unsafe class DemoWindow : IImGuiComponent
{
    public bool IsOverlay => false;
    public bool IsOpen = false;

    private readonly IImGui _imGui;

    public DemoWindow(IImGui imGui)
    {
        _imGui = imGui;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (_imGui.MenuItemEx("ImGui Demo Window", "", false, true))
        {
            IsOpen = true;
        }
    }

    public void Render(IImGuiShell imguiSupport)
    {
        if (!IsOpen)
            return;

        _imGui.ShowDemoWindow(ref IsOpen);
    }
}
