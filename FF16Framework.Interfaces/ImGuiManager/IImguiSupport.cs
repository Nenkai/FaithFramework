using FF16Framework.Interfaces.ImGui;

using System;
using System.Drawing;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.ImGuiManager;

/// <summary>
/// Provides ImGui specific support.
/// </summary>
public interface IImGuiSupport
{
    /// <summary>
    /// Returns whether the overlay is loaded into the game. <br/>
    /// This may be false if the user disabled injecting the ImGui overlay from the configuration panel.
    /// </summary>
    bool IsOverlayLoaded { get; }

    /// <summary>
    /// Whether the main menu bar is open. Can be used to account for any potential positional offset.
    /// </summary>
    bool IsMainMenuBarOpen { get; }

    /// <summary>
    /// Adds a new renderable component.
    /// </summary>
    /// <param name="component">ImGui component to add.</param>
    /// <param name="category">Top main menu category, by default, 'File', 'Tools' and 'Other' are available. <br/>
    /// If any other name is specified, it will be appended as a new category on the top menu bar. <br/>
    /// <br/>
    /// If empty, no menu can be rendered for this component.</param>
    /// <param name="name">Name, which should be your mod name (or anything else). <br/>
    /// This is only used for sorting and grouping menu entries on the framework side per mod.</param>
    void AddComponent(IImGuiComponent component, string? category = null, string? name = null);

    /// <summary>
    /// Logs a line to the Reloaded console, ImGui logs window, and optionally, the overlay logger.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="message"></param>
    /// <param name="color"></param>
    /// <param name="includeInOverlayLogger"></param>
    void LogWriteLine(string source, string message, Color? color = null, bool includeInOverlayLogger = false);
}