using DoomNetFrameworkEngine.DoomEntity;
using DoomNetFrameworkEngine.DoomEntity.MathUtils;
using DoomNetFrameworkEngine.Video;
using FaithFramework.Sample.Doom.Configuration;
using FaithFramework.Sample.Doom.Template;
using FaithFramework.Sample.Doom.Template.Configuration;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System;
using System.Diagnostics;
using System.Drawing;

using RyoTune.Reloaded;

#if DEBUG
using System.Diagnostics;
using System.Drawing;
#endif

namespace FaithFramework.Sample.Doom;

/// <summary>
/// Your mod logic goes here.
/// </summary>
public class Mod : ModBase // <= Do not Remove.
{
    /// <summary>
    /// Provides access to the mod loader API.
    /// </summary>
    private readonly IModLoader _modLoader;

    /// <summary>
    /// Provides access to the Reloaded.Hooks API.
    /// </summary>
    /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
    private readonly IReloadedHooks? _hooks;

    /// <summary>
    /// Provides access to the Reloaded logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Entry point into the mod, instance that created this class.
    /// </summary>
    private readonly IMod _owner;

    /// <summary>
    /// Provides access to this mod's configuration.
    /// </summary>
    private Config _configuration;

    /// <summary>
    /// The configuration of the currently executing mod.
    /// </summary>
    private readonly IModConfig _modConfig;

    private readonly IImGui _imGui;
    private readonly IImGuiShell _imGuiShell;
    private readonly DoomGameComponent _doomComponent;

    [Function(CallingConventions.Microsoft)]
    private delegate nint faith_Input_InputManager_Update(nint inputManager, float arg2);
    private IHook<faith_Input_InputManager_Update> HOOK_InputManagerUpdate;
    public bool BlockInput { get; set; }

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _logger = context.Logger;
        _owner = context.Owner;
        _configuration = context.Configuration;
        _modConfig = context.ModConfig;

#if DEBUG
        // Attaches debugger in debug mode; ignored in release.
        Debugger.Launch();
#endif

        Project.Initialize(_modConfig, _modLoader, _logger);

        Project.Scans.AddScanHook(nameof(faith_Input_InputManager_Update),
            (result, hooks) => HOOK_InputManagerUpdate = hooks.CreateHook<faith_Input_InputManager_Update>(ReadKeys_Replacement, result).Activate(), 
            () => _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for ReadKeys function!", Color.OrangeRed));

        _logger.WriteLineAsync($"[{_modConfig.ModId}] Mod initialized.");

        var imGuiController = _modLoader.GetController<IImGui>();
        if (imGuiController?.TryGetTarget(out IImGui? imGui) != true || imGui is null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] Could not get IImGui.");
            return;
        }
        _imGui = imGui;
        _logger.WriteLine($"[{_modConfig.ModId}] IImGui found.");

        var imGuiShellController = _modLoader.GetController<IImGuiShell>();
        if (imGuiShellController?.TryGetTarget(out IImGuiShell? imGuiShell) != true || imGuiShell is null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] Could not get IImGuiShell.");
            return;
        }

        _imGuiShell = imGuiShell;

        _logger.WriteLine($"[{_modConfig.ModId}] IImGuiShell found.");

        _doomComponent = new DoomGameComponent(_imGui, _configuration);
        imGuiShell.AddComponent(_doomComponent);
    }

    private nint ReadKeys_Replacement(nint inputManager, float arg2)
    {
        if (_doomComponent?.BlockInput == true)
            return 0;

        return HOOK_InputManagerUpdate.OriginalFunction(inputManager, arg2);
    }

    #region Standard Overrides
    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        _configuration = configuration;
        _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying...");

        if (_doomComponent != null)
        {
            _doomComponent.WadPath = configuration.WadPath;
        }
    }
    #endregion

    #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod() { }
#pragma warning restore CS8618
    #endregion
}