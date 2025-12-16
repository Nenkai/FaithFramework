using DoomNetFrameworkEngine.DoomEntity;
using DoomNetFrameworkEngine.DoomEntity.MathUtils;
using DoomNetFrameworkEngine.Video;
using FaithFramework.Sample.Doom.Configuration;
using FaithFramework.Sample.Doom.Template;
using FaithFramework.Sample.Doom.Template.Configuration;
using NenTools.ImGui.Abstractions;
using NenTools.ImGui.Interfaces;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System;
using System.Diagnostics;
using System.Drawing;

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

    public bool BlockInput { get; set; }

    private readonly string ReadKeys_Pattern = "48 89 5C 24 ?? 48 89 6C 24 ?? 56 57 41 56 48 83 EC ?? 48 8B 01 4C 8B F1 0F 29 74 24 ?? 0F 28 F1 FF 50";
    private long ReadKeys_Address = 0;

    [Function(CallingConventions.Microsoft)]
    private delegate nint ReadKeys(nint inputManager, float arg2);
    private IHook<ReadKeys> ReadKeys_Hook;

    private nint ReadKeys_Replacement(nint inputManager, float arg2)
    {
        if (_doomComponent?.BlockInput == true)
            return 0;

        return ReadKeys_Hook.OriginalFunction(inputManager, arg2);
    }

    private readonly IImGui _imGui;
    private readonly IImGuiShell _imGuiShell;
    private readonly DoomGameComponent _doomComponent;

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

        var startupScannerController = _modLoader.GetController<IStartupScanner>();
        if (startupScannerController == null || !startupScannerController.TryGetTarget(out var startupScanner))
        {
            _logger.WriteLineAsync($"[{_modConfig.ModId}] Unable to find startupScanner. Ensure Reloaded.Memory.SigScan is installed.", Color.OrangeRed);
            return;
        }

        startupScanner.AddMainModuleScan(ReadKeys_Pattern, result =>
        {
            if (!result.Found)
            {
                _logger.WriteLineAsync($"[{_modConfig.ModId}] Failed to find AoB pattern for ReadKeys function!", Color.OrangeRed);
                return;
            }

            ReadKeys_Address = Process.GetCurrentProcess().MainModule!.BaseAddress + result.Offset;

            _logger.WriteLineAsync($"[{_modConfig.ModId}] ReadKeys function found at 0x{ReadKeys_Address:X}.", Color.LimeGreen);
            ReadKeys_Hook = _hooks!.CreateHook<ReadKeys>(ReadKeys_Replacement, ReadKeys_Address).Activate();
        });

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