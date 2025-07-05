using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Interfaces.ImGui;

namespace FF16Framework.Interfaces.ImGuiManager;

public interface IImguiMenuComponent
{
    public void BeginMenuComponent(IImGui imgui);
}
