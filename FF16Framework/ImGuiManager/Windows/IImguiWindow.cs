using FF16Framework.ImGuiManager;
using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGuiManager.Windows;

public interface IImguiWindow : IImguiMenuComponent
{
    /// <summary>
    /// Whether to render regardless of menu enabled state.
    /// </summary>
    public bool IsOverlay { get; }

    public void Render(IImguiSupport imguiSupport, IImGui imgui);
}
