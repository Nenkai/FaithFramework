using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Shell.Interfaces;

/// <summary>
/// Represents a renderable component for ImGui.<br/>
/// Inherit from this in order to render elements using ImGui.
/// </summary>
public interface IImGuiComponent
{
    /// <summary>
    /// Whether this component is an overlay and should render regardless of menu state/visibility.
    /// </summary>
    bool IsOverlay { get; }

    /// <summary>
    /// Fired on menu component render. Here you should render your menu items as needed. <br/>
    /// 
    /// If your component is decorated with
    /// <code>
    /// [ImGuiMenu(Category = "Tools", Priority = 0, Owner = nameof(FF16Framework))]
    /// </code>
    /// Then:
    /// <code>
    /// * Tools
    ///   * <b>RenderMenu() is called here, feel free to render a sub-menu (BeginMenu) or anything else.</b><br/>
    ///    * Menu Item 1 (Assuming you called BeginMenu, and then MenuItem)
    ///    * ...
    /// </code>
    /// </summary>
    /// <param name="imGuiShell">ImGui Shell instance.</param>
    void RenderMenu(IImGuiShell imGuiShell);

    /// <summary>
    /// Fired on render.
    /// </summary>
    /// <param name="imGuiShell">ImGui Shell instance.</param>
    void Render(IImGuiShell imGuiShell);
}
