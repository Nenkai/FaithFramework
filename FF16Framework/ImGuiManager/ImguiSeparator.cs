using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;

namespace FF16Framework.ImGuiManager;

public class ImguiSeparator : IImguiMenuComponent
{
    public ImguiSeparator()
    {

    }


    public void BeginMenuComponent(IImGui imgui)
    {
        imgui.Separator();
    }
}
