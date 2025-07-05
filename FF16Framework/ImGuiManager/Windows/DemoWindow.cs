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

public unsafe class DemoWindow : IImguiWindow, IImguiMenuComponent
{
    public bool IsOverlay => false;
    public bool IsOpen = false;

    public DemoWindow()
    {
        
    }

    public void BeginMenuComponent(IImGui imgui)
    {
        if (imgui.MenuItemEx("ImGui Demo Window", "", false, true))
        {
            IsOpen = true;
        }
    }

    public void Render(IImguiSupport imguiSupport, IImGui imgui)
    {
        if (!IsOpen)
            return;

        imgui.ShowDemoWindow(ref IsOpen);
    }
}
