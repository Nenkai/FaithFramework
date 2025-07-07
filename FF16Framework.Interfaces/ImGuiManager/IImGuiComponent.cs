using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Interfaces.ImGui;

namespace FF16Framework.Interfaces.ImGuiManager;

/// <summary>
/// Represents a renderable component for ImGui. <br/>
/// Inherit from this in order to render elements using ImGui.
/// </summary>
public interface IImGuiComponent
{
    /// <summary>
    /// Whether this component is an overlay and should render regardless of menu state/visibility.
    /// </summary>
    bool IsOverlay { get; }

    /// <summary>
    /// Fired on menu component render. Here you should render your menu items as needed.
    /// </summary>
    /// <param name="imgui"></param>
    void RenderMenu(IImGui imgui);

    /// <summary>
    /// Fired on render.
    /// </summary>
    /// <param name="imguiSupport">ImGui Support instance.</param>
    /// <param name="imgui">ImGui instance.</param>
    void Render(IImGuiSupport imguiSupport, IImGui imgui);
}
