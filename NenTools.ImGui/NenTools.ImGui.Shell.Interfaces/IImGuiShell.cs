using System;
using System.Drawing;
using System.Threading.Tasks;

namespace NenTools.ImGui.Shell.Interfaces;

/// <summary>
/// Provides ImGui specific shell.
/// </summary>
public interface IImGuiShell
{
    /// <summary>
    /// Returns whether the overlay is enabled.<br/>
    /// This may be false if the user disabled injecting the ImGui overlay from the configuration panel.
    /// </summary>
    bool IsOverlayEnabled { get; }

    /// <summary>
    /// Returns whether the ImGui context has been created such that ImGui functions can be called (like GetIO()).
    /// </summary>
    bool ContextCreated { get; }

    /// <summary>
    /// Whether the main menu bar is open/shown. Can be used to account for any potential positional offset.
    /// </summary>
    bool IsMainMenuOpen { get; }

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
    /// Returns whether the mouse is active while the shell/menu is open.
    /// </summary>
    bool MouseActiveWhileMenuOpen { get; }

    /// <summary>
    /// Texture manager instance.
    /// </summary>
    IImGuiTextureManager TextureManager { get; }

    /// <summary>
    /// For use with <see cref="OnFirstRender"/>
    /// </summary>
    delegate void OnFirstRenderDelegate();

    /// <summary>
    /// Fired when ImGui is rendering for the first time.
    /// </summary>
    public event OnFirstRenderDelegate OnFirstRender;

    /// <summary>
    /// For use with <see cref="OnImGuiConfiguration"/>
    /// </summary>
    delegate void OnImGuiConfigurationDelegate();

    /// <summary>
    /// Fired to configure ImGui (fonts, etc).
    /// </summary>
    public event OnImGuiConfigurationDelegate OnImGuiConfiguration;

    /// <summary>
    /// For use with <see cref="OnEndMainMenuBarRender"/>
    /// </summary>
    delegate void OnEndMainMenuBarRenderDelegate();

    /// <summary>
    /// Fired when the menu bar and all the elements have been rendered, <b>but just before EndMainMenuBar was called.</b>
    /// </summary>
    public event OnEndMainMenuBarRenderDelegate OnEndMainMenuBarRender;

    /// <summary>
    /// For use with <see cref="OnLogMessage"/>
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="color">Message color.</param>
    delegate void OnLogMessageDelegate(string message, Color? color);

    /// <summary>
    /// Fired on log message.
    /// </summary>
    public event OnLogMessageDelegate OnLogMessage;

    /// <summary>
    /// Disables the overlay (aka ImGui rendering as a whole, not just the top menu).
    /// </summary>
    public void DisableOverlay();

    /// <summary>
    /// Enables the overlay (aka ImGui rendering as a whole, not just the top menu).
    /// </summary>
    public void EnableOverlay();

    /// <summary>
    /// Disables the menu.
    /// </summary>
    public void HideMenu();

    /// <summary>
    /// Enables the menu.
    /// </summary>
    public void ShowMenu();

    /// <summary>
    /// Enables or disables the menu.
    /// </summary>
    public void ToggleMenuState();

    /// <summary>
    /// Adds a new renderable component. <br/>
    /// Display order is determined by your component decorated with <see cref="ImGuiMenuAttribute"/> unless the next override arguments are provided.
    /// </summary>
    /// <param name="component">ImGui component to add.</param>
    /// <param name="overrideCategory">Optional, override for <see cref="ImGuiMenuAttribute.Category"/>. <br/><br/>
    /// Top main menu category, by default, 'File', 'Tools' and 'Other' are available. <br/>
    /// If any other name is specified, it will be appended as a new category on the top menu bar. <br/>
    /// <br/>
    /// If empty, no menu can be rendered for this component.</param>
    /// <param name="overridePriority">Optional, override for <see cref="ImGuiMenuAttribute.Priority"/>. <br/><br/>
    /// Determines the render priority for this component, menu wise. Lower = Highest on the menu.
    /// </param>
    /// <param name="overrideOwner">Optional, override for <see cref="ImGuiMenuAttribute.Owner"/>. <br/><br/>
    /// Owner for this component, which should be shared by all components that you add.<br/><br/>
    /// <b>This is only used for sorting menu entries alphabetically on the framework side per mod, when the priority is the same as another component.</b>
    /// Should normally be your mod id or mod name.<br/></param>
    void AddComponent(IImGuiComponent component, string? overrideCategory = null, int overridePriority = 0, string? overrideOwner = null);

    /// <summary>
    /// Adds a menu separator.
    /// </summary>
    /// <param name="displayName">Display name for the separator.<br/><br/>
    /// If not null/empty, a separator with a header will be used (ImGui.SeparatorText) <br/>
    /// Otherwise, a blank separator is used (ImGui.Separator).</param>
    /// <param name="category">Top main menu category, by default, 'File', 'Tools' and 'Other' are available. <br/>
    /// If any other name is specified, it will be appended as a new category on the top menu bar. <br/>
    /// <br/>
    /// If empty, no menu can be rendered for this component.</param>
    /// <param name="priority">Determines the render priority for this component, menu wise. Lower = Highest on the menu.<br/>
    /// <b>Should match the priority assigned by other components!</b>
    /// </param>
    /// <param name="orderString">Order String, which should be your mod name (or anything else). <br/>
    /// <b>This is only used for sorting menu entries alphabetically on the framework side per mod, when the priority is the same as another component.</b></param>
    /// <br/>
    /// <summary/>
    void AddMenuSeparator(string? displayName, string category, int priority, string orderString);

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
    /// Text is output to any general logger listeners, which is specified by <see cref="IImGuiShell.OnLogMessage"/>
    /// </summary>
    GeneralLoggers = 1 << 0,

    /// <summary>
    /// Text is output to the overlay logger.
    /// </summary>
    OverlayLogger = 1 << 1,

    /// <summary>
    /// Text is output to all output targets, but the overlay logger.
    /// </summary>
    AllButOverlayLogger = GeneralLoggers,

    /// <summary>
    /// Text is output to all output targets.
    /// </summary>
    All = AllButOverlayLogger | OverlayLogger,
}