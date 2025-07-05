using FF16Framework.ImGui.Hooks;
using FF16Framework.ImGuiManager;
using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.Nex;
using FF16Framework.Nex;
using FF16Framework.Save;
using FF16Framework.Template;

using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;

using SharedScans.Interfaces;

using System.Diagnostics;

using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace FF16Framework;

/// <summary>
/// Your mod logic goes here.
/// </summary>
public class Mod : ModBase, IExports // <= Do not Remove.
{
    public Type[] GetTypes() =>
    [
        typeof(INextExcelDBApi),
        typeof(INextExcelDBApiManaged),
        typeof(IImGui),
    ];

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

    private static IStartupScanner? _startupScanner = null!;

    private NexHooks _nexHooks;
    private SaveHooks _saveHooks;

    private NextExcelDBApi _nexApi;
    private NextExcelDBApiManaged _nexApiManaged;

    private ImguiSupport _imguiSupport;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _logger = context.Logger;
        _owner = context.Owner;
        _configuration = context.Configuration;
        _modConfig = context.ModConfig;

#if DEBUG
        Debugger.Launch();
#endif
        if (_hooks is null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] Hooks is null. Framework will not load!");
            return;
        }

        var startupScannerController = _modLoader.GetController<IStartupScanner>();
        if (startupScannerController == null || !startupScannerController.TryGetTarget(out _startupScanner))
        {
            return;
        }

        var sharedScansController = _modLoader.GetController<ISharedScans>();
        if (sharedScansController == null || !sharedScansController.TryGetTarget(out ISharedScans? scans))
        {
            _logger.WriteLine($"[{_modConfig.ModId}] Unable to get ISharedScans. Framework will not load!");
            return;
        }

        _nexHooks = new NexHooks(_configuration, _modConfig, scans, _logger);
        _nexHooks.Setup();

        _saveHooks = new SaveHooks(_configuration, _modConfig, scans, _logger);
        _saveHooks.Setup();

        _nexApi = new NextExcelDBApi(_nexHooks);

        _nexApiManaged = new NextExcelDBApiManaged(_nexHooks);
        _modLoader.AddOrReplaceController<INextExcelDBApiManaged>(_owner, _nexApiManaged);

        var imgui = new ImGuiManager.ImGui();
        ImguiHook.imgui = imgui;
        _imguiSupport = new ImguiSupport(_hooks, scans, _modConfig, imgui);
        _imguiSupport.SetupImgui(_modLoader.GetDirectoryForModId(_modConfig.ModId));

        _modLoader.AddOrReplaceController<INextExcelDBApi>(_owner, _nexApi);
        _modLoader.AddOrReplaceController<INextExcelDBApiManaged>(_owner, _nexApiManaged);
        _modLoader.AddOrReplaceController<IImGui>(_owner, imgui);

        _logger.WriteLine($"[{_modConfig.ModId}] Framework {_modConfig.ModVersion} initted.", _logger.ColorGreen);
    }

    #region Standard Overrides
    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        _configuration = configuration;
        _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");

        _nexHooks.UpdateConfig(configuration);
    }

    #endregion

    #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod() { }
#pragma warning restore CS8618
    #endregion
}