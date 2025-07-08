using FF16Framework.ImGui.Hooks;
using FF16Framework.ImGui.Hooks.DirectX12;
using FF16Framework.ImGuiManager;
using FF16Framework.ImGuiManager.Windows;
using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;
using FF16Framework.Interfaces.Nex;
using FF16Framework.Nex;
using FF16Framework.Save;
using FF16Framework.Template;

using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;

using SharedScans.Interfaces;

using System.Diagnostics;
using System.Runtime.CompilerServices;

using Tomlyn;

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
        typeof(IImGuiSupport),
        typeof(IImGuiTextureManager),
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

    private ImGuiSupport _imGuiSupport;
    private ImGuiTextureManager _imGuiTextureManager;
    private ImGuiConfig _imGuiConfig;

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
 
        InitSaveHooks(scans);
        InitNex(scans);
        InitImGui(scans);

        _logger.WriteLine($"[{_modConfig.ModId}] Framework {_modConfig.ModVersion} initted.", _logger.ColorGreen);
    }

    private void InitImGui(ISharedScans scans)
    {
        var imgui = new ImGuiManager.ImGui();
        var imguiHookDx12 = new ImguiHookDx12();
        _imGuiTextureManager = new ImGuiTextureManager(_logger, imguiHookDx12);

        string configPath = Path.Combine(_modLoader.GetDirectoryForModId(_modConfig.ModId), "framework_config.toml");
        if (File.Exists(configPath))
        {
            try
            {
                _imGuiConfig = Toml.ToModel<ImGuiConfig>(File.ReadAllText(configPath));
                _imGuiConfig.SetPath(configPath);
            }
            catch (Exception ex)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] Failed to load ImGui config from '{configPath}' - {ex.Message}");
                _logger.WriteLine("Creating default config...");
                _imGuiConfig = new ImGuiConfig(configPath);
                _imGuiConfig.Save();
            }
        }
        else
        {
            _imGuiConfig = new ImGuiConfig(configPath);
            _imGuiConfig.Save();
        }

        ImguiHook.imgui = imgui;
        _imGuiSupport = new ImGuiSupport(_hooks!, scans, _modConfig, _logger, imguiHookDx12, imgui, _imGuiConfig);

        if (_configuration.LoadImGuiHook)
        {
            _imGuiSupport.SetupImgui(_modLoader.GetDirectoryForModId(_modConfig.ModId));

            string logPath = Path.Combine(_modLoader.GetDirectoryForModId(_modConfig.ModId), "framework_log.txt");
            _imGuiSupport.AddComponent(new LogWindow(_logger, logPath), _imGuiSupport.FileMenuName, nameof(FF16Framework));

            _imGuiSupport.AddMenuSeparator(_imGuiSupport.ToolsMenuName, nameof(FF16Framework), nameof(FF16Framework));
            _imGuiSupport.AddComponent(new SettingsComponent(_imGuiConfig), _imGuiSupport.ToolsMenuName, nameof(FF16Framework));

            _imGuiSupport.AddComponent(new DemoWindow(), _imGuiSupport.OtherMenuName, nameof(FF16Framework));
            _imGuiSupport.AddComponent(new AboutWindow(_modConfig, _modLoader, _imGuiTextureManager), _imGuiSupport.OtherMenuName, nameof(FF16Framework));
        }
        else
        {
            _logger.WriteLine($"[{_modConfig.ModId}] ImGui overlay/hook is currently disabled. You can enable it in the framework's configuration options.", _logger.ColorYellow);
        }

        _modLoader.AddOrReplaceController<IImGui>(_owner, imgui);
        _modLoader.AddOrReplaceController<IImGuiSupport>(_owner, _imGuiSupport);
        _modLoader.AddOrReplaceController<IImGuiTextureManager>(_owner, _imGuiTextureManager);
    }

    private void InitNex(ISharedScans scans)
    {
        _nexHooks = new NexHooks(_configuration, _modConfig, scans, _logger);
        _nexHooks.Setup();

        _nexApi = new NextExcelDBApi(_nexHooks);
        _nexApiManaged = new NextExcelDBApiManaged(_nexHooks);
        _modLoader.AddOrReplaceController<INextExcelDBApi>(_owner, _nexApi);
        _modLoader.AddOrReplaceController<INextExcelDBApiManaged>(_owner, _nexApiManaged);
    }

    private void InitSaveHooks(ISharedScans scans)
    {
        _saveHooks = new SaveHooks(_configuration, _modConfig, scans, _logger);
        _saveHooks.Setup();
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