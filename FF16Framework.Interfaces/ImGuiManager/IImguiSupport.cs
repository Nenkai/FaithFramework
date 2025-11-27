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
    /// Returns whether the overlay is loaded into the game.<br/>
    /// This may be false if the user disabled injecting the ImGui overlay from the configuration panel.
    /// </summary>
    bool IsOverlayLoaded { get; }

    /// <summary>
    /// Returns whether the ImGui context has been created such that ImGui functions can be called (like GetIO()).
    /// </summary>
    bool ContextCreated { get; }

    /// <summary>
    /// Whether the main menu bar is open. Can be used to account for any potential positional offset.
    /// </summary>
    bool IsMainMenuBarOpen { get; }

    /// <summary>
    /// Name of the 'File' menu within the main menu bar.
    /// </summary>
    string FileMenuName { get; }

    /// <summary>
    /// Name of the 'Tools' menu within the main menu bar.
    /// </summary>
    string ToolsMenuName { get; }

    /// <summary>
    /// Name of the 'Other' menu within the main menu bar.
    /// </summary>
    string OtherMenuName { get; }

    /// <summary>
    /// Adds a new renderable component.
    /// </summary>
    /// <param name="component">ImGui component to add.</param>
    /// <param name="category">Top main menu category, by default, 'File', 'Tools' and 'Other' are available. <br/>
    /// If any other name is specified, it will be appended as a new category on the top menu bar. <br/>
    /// <br/>
    /// If empty, no menu can be rendered for this component.</param>
    /// <param name="modIdOrName">Mod Id or Name, which should be your mod name (or anything else). <br/>
    /// <b>This is only used for sorting and grouping menu entries on the framework side per mod.</b></param>
    void AddComponent(IImGuiComponent component, string? category = null, string? modIdOrName = null);

    /// <summary>
    /// Adds a menu separator.
    /// </summary>
    /// <param name="category">Top main menu category, by default, 'File', 'Tools' and 'Other' are available. <br/>
    /// If any other name is specified, it will be appended as a new category on the top menu bar. <br/></param>
    /// <param name="modIdOrName">Mod Id or Name, which should be your mod name (or anything else). <br/>
    /// <b>This is only used for sorting and grouping menu entries on the framework side per mod.</b></param>
    /// <param name="displayName">Display name for the separator.<br/>
    /// <br/>
    /// If not null/empty, a separator with a header will be used (<see cref="IImGui.SeparatorText(string)"/>) <br/>
    /// Otherwise, a blank separator is used (<see cref="IImGui.Separator"/>).</param>
    void AddMenuSeparator(string category, string modIdOrName, string? displayName);

    /// <summary>
    /// Logs a line to the Reloaded console, ImGui logs window, and optionally, the overlay logger.
    /// </summary>
    /// <param name="source">Source, which should be a mod id or mod name. Will be shown in square brackets.</param>
    /// <param name="message">Message to display.</param>
    /// <param name="color">Color of the message.</param>
    /// <param name="outputTargetFlags">Where to output this message. By default, the message is output everywhere but the overlay logger.</param>
    void LogWriteLine(string source, string message, Color? color = null, LoggerOutputTargetFlags outputTargetFlags = LoggerOutputTargetFlags.AllButOverlayLogger);
}

/// <summary>
/// Logging output targets.
/// </summary>
[Flags]
public enum LoggerOutputTargetFlags : ulong
{
    /// <summary>
    /// Text is output to the Reloaded logger.
    /// </summary>
    ReloadedLog = 1 << 0,

    /// <summary>
    /// Text is output to the overlay logger.
    /// </summary>
    OverlayLogger = 1 << 1,

    /// <summary>
    /// Text is output to all output targets, but the overlay logger.
    /// </summary>
    AllButOverlayLogger = ReloadedLog,

    /// <summary>
    /// Text is output to all output targets.
    /// </summary>
    All = AllButOverlayLogger | OverlayLogger,
}