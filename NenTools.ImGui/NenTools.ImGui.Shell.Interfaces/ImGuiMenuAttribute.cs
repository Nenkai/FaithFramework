using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Shell.Interfaces;

/// <summary>
/// Declares that a ImGui component is rendered on a menu.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class ImGuiMenuAttribute : Attribute
{
    /// <summary>
    /// Top main menu category, by default, 'File', 'Tools' and 'Other' are available. <br/>
    /// If any other name is specified, it will be appended as a new category on the top menu bar. <br/>
    /// If empty, no menu can be rendered for this component.
    /// </summary>
    public required string Category { get; set; }

    /// <summary>
    /// Determines the render priority for this component, menu wise. Lower = Highest on the menu.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Owner for this component, which should be shared by all components that you add.<br/><br/>
    /// <b>This is only used for sorting menu entries alphabetically on the framework side per mod, when the priority is the same as another component.</b>
    /// Should normally be your mod id or mod name.<br/>
    /// </summary>
    public string? Owner { get; set; }
}
