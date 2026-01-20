using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

using NenTools.ImGui.Hooks.DirectX12;
using NenTools.ImGui.Implementation;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Backend;
using NenTools.ImGui.Interfaces.Shell;
using NenTools.ImGui.Interfaces.Shell.Textures;
using NenTools.ImGui.Interfaces.Shell.Fonts;
using NenTools.ImGui.Shell;

using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;

using RyoTune.Reloaded;

using Tomlyn;

using Windows.Win32;

using FF16Framework.ImGuiManager.Hooks;
using FF16Framework.ImGuiManager.Windows;
using FF16Framework.ImGuiManager.Windows.Framework;
using FF16Framework.Interfaces.Nex;
using FF16Framework.Logging;
using FF16Framework.Nex;
using FF16Framework.Template;
using FF16Framework.Faith.Hooks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FF16Framework.Services.ResourceManager;
using FF16Framework.ImGuiManager.Windows.Resources;
using FF16Framework.ImGuiManager.Windows.Visualizers;

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
        typeof(IImGuiShell),
        typeof(IImGuiTextureManager),
        typeof(INextExcelDBApiManagedV2)
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
    private readonly Reloaded.Mod.Interfaces.ILogger _logger;

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

    private IServiceProvider _services;

    private NexHooks _nexHooks;
    private SaveHooks _saveHooks;

    private IBackendHook _backendHook;
    private IImGuiShell _imGuiShell;
    private ImGuiShellConfig _imGuiShellConfig;
    private IImGui _imGui;

    private FrameworkConfig _frameworkConfig;

    // Used to enable ImGui when the splash screen is disabled.
    public delegate bool DestroyWindow(nint hwnd);
    private IHook<DestroyWindow>? _destroyWindowHook;

    // Used to inject ImGui when the first present occurs.
    private static IHook<RenderExecCommandListsAndPresent>? RenderExecCommandListsAndPresentHook;
    private delegate void RenderExecCommandListsAndPresent(nint a1);

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

        _services = BuildServiceCollection();

        // Uncomment this if you need debug logs with DebugView (ensure to also enable debug in DirectX control panel)
        // NOTE: Enabling this will severely impact framerate!
        // NOTE 2: Must be enabled early, before the game itself has initialized D3D12.
        //         It should not be moved to ImGui handlers.
        // DebugInterface.Get().EnableDebugLayer();

        Project.Initialize(_modConfig, _modLoader, _logger);

        if (_hooks is null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] Hooks is null. Framework will not load!");
            return;
        }

        GameContext gameContext = _services.GetRequiredService<GameContext>();
        IApplicationConfigV1 appConfig = _modLoader.GetAppConfig();
        if (appConfig.AppId.Contains("ffxvi"))
            gameContext.SetGameType(FaithGameType.FFXVI);
        else if (appConfig.AppId.Contains("fft_"))
            gameContext.SetGameType(FaithGameType.FFT);
        else
            _logger.WriteLine($"[{_modConfig.ModId}] Could not determine game type. Was the executable renamed?", _logger.ColorRed);

        IEnumerable<HookGroupBase> hookGroups = _services.GetServices<HookGroupBase>();
        foreach (HookGroupBase hookGroup in hookGroups)
            hookGroup.SetupHooks();

        InitNex();
        InitImGui();

        _logger.WriteLine($"[{_modConfig.ModId}] Framework {_modConfig.ModVersion} initted.", _logger.ColorGreen);
    }

    private ServiceProvider BuildServiceCollection()
    {
        ServiceCollection services = new ServiceCollection();

        string shellConfigPath = Path.Combine(_modLoader.GetDirectoryForModId(_modConfig.ModId), "shell_config.toml");
        if (File.Exists(shellConfigPath))
        {
            try
            {
                _imGuiShellConfig = Toml.ToModel<ImGuiShellConfig>(File.ReadAllText(shellConfigPath));
                _imGuiShellConfig.SetPath(shellConfigPath);
            }
            catch (Exception ex)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] Failed to load ImGui Shell config from '{shellConfigPath}' - {ex.Message}");
                _logger.WriteLine("Creating default config...");
                _imGuiShellConfig = new ImGuiShellConfig(shellConfigPath);
                _imGuiShellConfig.Save();
            }
        }
        else
        {
            _imGuiShellConfig = new ImGuiShellConfig(shellConfigPath);
            _imGuiShellConfig.Save();
        }

        string frameworkConfigPath = Path.Combine(_modLoader.GetDirectoryForModId(_modConfig.ModId), "framework_config.toml");
        if (File.Exists(frameworkConfigPath))
        {
            try
            {
                _frameworkConfig = Toml.ToModel<FrameworkConfig>(File.ReadAllText(frameworkConfigPath));
                _frameworkConfig.SetPath(frameworkConfigPath);
            }
            catch (Exception ex)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] Failed to load framework config from '{frameworkConfigPath}' - {ex.Message}");
                _logger.WriteLine("Creating default config...");
                _frameworkConfig = new FrameworkConfig(frameworkConfigPath);
                _frameworkConfig.Save();
            }
        }
        else
        {
            _frameworkConfig = new FrameworkConfig(frameworkConfigPath);
            _frameworkConfig.Save();
        }

        services
            // R2 Primitives
            .AddSingleton(_modConfig)
            .AddSingleton(_modLoader)
            .AddSingleton(_hooks!)
            .AddSingleton(_configuration)
            .AddSingleton(_logger)
            .AddSingleton(LoggerFactory.Create(e => e.AddProvider(new R2LoggerToMSLoggerAdapterProvider(_logger))))

            .AddSingleton<GameContext>()
            .AddSingleton(_frameworkConfig)

            .AddSingleton<ImGuiShellConfig>(_imGuiShellConfig)
            .AddSingleton<INextExcelDBApi, NextExcelDBApi>()
            .AddSingleton<INextExcelDBApiManagedV2, NextExcelDBApiManaged>()
            .AddSingleton<ResourceManagerService>()
            .AddSingleton<IImGui, ImGui>()
            .AddSingleton<IBackendHook, DX12BackendHook>()
            .AddSingleton<IImGuiShell, ImGuiShell>()
            .AddSingleton<ImGuiInputHookManager>()

            // Hooks (Any)
            .AddSingletonAs<HookGroupBase, NexHooks>()
            .AddSingletonAs<HookGroupBase, ResourceManagerHooks>()
            .AddSingletonAs<HookGroupBase, SoundManagerHooks>() // For now, only FFXVI. FFT's SetVolume is obfuscated by denuvo

            // Hooks (FFXVI)
            .AddSingletonAs<HookGroupBase, EntityManagerHooks>() 
            .AddSingletonAs<HookGroupBase, MapHooks>()
            .AddSingletonAs<HookGroupBase, SaveHooks>()
            .AddSingletonAs<HookGroupBase, CameraHooks>()
            .AddSingletonAs<HookGroupBase, MagicHooks>()
            .AddSingletonAs<HookGroupBase, UnkList35Hooks>()
            .AddSingletonAs<HookGroupBase, EidHooks>()

            // ImGui renderable
            .AddSingleton<LogWindow>(provider =>
            {
                var imgui = provider.GetRequiredService<IImGui>();
                var logger = provider.GetRequiredService<Reloaded.Mod.Interfaces.ILogger>();

                var path = Path.Combine(_modLoader.GetDirectoryForModId(_modConfig.ModId), "framework_log.txt");
                var window = new LogWindow(imgui, logger);

                try
                {
                    window.SetupLogPath(path);
                }
                catch (Exception ex)
                {
                    _logger.WriteLine($"[{_modConfig.ModId}] Unable to set log path, file is likely already in use. ({ex.Message})", System.Drawing.Color.Red);
                }

                return window;
            })

            .AddSingleton<SettingsComponent>()
            .AddSingleton<DocumentationComponent>()
            .AddSingleton<FrameworkToolsComponent>()
            .AddSingleton<ResourceManagerWindow>()
            .AddSingleton<GameOverlay>()
            .AddSingleton<EidVisualizerComponent>()
            .AddSingleton<MainVisualizerComponent>()
            .AddSingleton<AboutWindow>();

        return services.BuildServiceProvider();
    }


    private bool imguiRenderable = false;

    // Only allow rendering once the splash screen is gone.
    private bool DestroyWindowImpl(nint hwnd)
    {
        Span<char> name = stackalloc char[256];
        PInvoke.GetClassName(new global::Windows.Win32.Foundation.HWND(hwnd), name);
        string trimmed = name.ToString().TrimEnd('\0');
        if (trimmed == "SplashClass")
            _imGuiShell.EnableOverlay();

        return _destroyWindowHook!.OriginalFunction(hwnd);
    }

    /// <summary>
    /// Fired when the game is rendering a frame.
    /// </summary>
    /// <param name="a1"></param>
    private unsafe void RenderExecCommandListsAndPresentImpl(nint a1)
    {
        RenderExecCommandListsAndPresentHook!.OriginalFunction(a1);

        if (!imguiRenderable)
        {
            if (_configuration.LoadImGuiHook)
            {
                _imGuiShell.Start(new BackendHookOptions()
                {
                    // TODO: Implement viewports
                    // EnableViewports = false, Not yet functional with DX12 hooks, it creates a new swapchain that shouldn't be hooked.
                    Implementations = _services.GetServices<IBackendHook>().ToList()
                }).GetAwaiter().GetResult();

                // Moved to DestroyWindow.
                // _imGuiShell.EnableOverlay();
            }
            else
            {
                _logger.WriteLine($"[{_modConfig.ModId}] ImGui overlay/hook is currently disabled. You can enable it in the framework's configuration options (restart needed).", _logger.ColorYellow);
            }

            imguiRenderable = true;

            // Don't need to hook this anymore. All we wanted is just something to alert us that the game is rendering.
            RenderExecCommandListsAndPresentHook.Disable();
        }
    }

    public static float Repeat(float t, float length)
    {
        return Math.Clamp(t - MathF.Floor(t / length) * length, 0.0f, length);
    }

    public static float PingPong(float t, float length)
    {
        t = Repeat(t, length * 2F);
        return length - MathF.Abs(t - length);
    }


    private void InitImGui()
    {
        _imGui = _services.GetRequiredService<IImGui>();
        _imGuiShell = _services.GetRequiredService<IImGuiShell>();

        // Shell components (add these early, so other mods aren't adding components while we haven't populated yet)
        _imGuiShell.AddComponent(_services.GetRequiredService<GameOverlay>());
        _imGuiShell.AddComponent(_services.GetRequiredService<LogWindow>());
        _imGuiShell.AddComponent(_services.GetRequiredService<FrameworkToolsComponent>());
        _imGuiShell.AddComponent(_services.GetRequiredService<AboutWindow>());

        _imGuiShell.OnImGuiConfiguration += ConfigureImgui;
        _imGuiShell.OnEndMainMenuBarRender += RenderAnimatedTitle;
        _imGuiShell.OnLogMessage += (message, color) => _logger.WriteLine(message, color ?? System.Drawing.Color.White);
        _imGuiShell.OnFirstRender += OnFirstImGuiRender;

        var inputHook = _services.GetRequiredService<ImGuiInputHookManager>();
        inputHook.SetupInputHooks();

        // Hook the splash screen window destroy.
        // Present is technically called before the main viewport is shown to the user, so to make sure that the 
        // initial overlay messages are displayed, enable ImGui rendering once the splash screen is gone.
        // Otherwise the messages may fade before the main window is even shown.
        nint destroyWindowPtr = PInvoke.GetProcAddress(PInvoke.GetModuleHandle("user32.dll"), "DestroyWindow");
        _destroyWindowHook = _hooks!.CreateHook<DestroyWindow>(DestroyWindowImpl, destroyWindowPtr).Activate();

        // We hook the call that performs present (not present itself)
        // We setup our DX12 hooks after the game made the first call.
        Project.Scans.AddScanHook(nameof(RenderExecCommandListsAndPresent),
            (result, hooks) => RenderExecCommandListsAndPresentHook = _hooks.CreateHook<RenderExecCommandListsAndPresent>(RenderExecCommandListsAndPresentImpl, result).Activate());

        _modLoader.AddOrReplaceController<IImGui>(_owner, _imGui);
        _modLoader.AddOrReplaceController<IImGuiShell>(_owner, _imGuiShell);
    }

    private unsafe void ConfigureImgui()
    {
        string modFolder = _modLoader.GetDirectoryForModId(_modConfig.ModId);

        IImGuiIO io = _imGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.ImGuiConfigFlags_DockingEnable;

        // English font
        string robotoFontPath = Path.Combine(modFolder, "ImGuiManager", "Fonts", "Roboto", "Roboto-Medium.ttf");
        _imGuiShell.FontManager.AddFontTTF(_modConfig.ModId, "Roboto-Medium", robotoFontPath, 15.0f, _imGui.ImFontAtlas_GetGlyphRangesDefault(io.Fonts), null!);

        // Japanese font
        string netoSansJpFontPath = Path.Combine(modFolder, "ImGuiManager", "Fonts", "Noto", "NotoSansJP-Medium.ttf");
        _imGuiShell.FontManager.AddFontTTF(_modConfig.ModId, "NotoSansJP-Medium", netoSansJpFontPath, 17.0f, _imGui.ImFontAtlas_GetGlyphRangesJapanese(io.Fonts), new ImFontOptions { MergeMode = true });

        // Emojis
        string twitterColorEmojiFontPath = Path.Combine(modFolder, "ImGuiManager", "Fonts", "TwitterColorEmoji", "twemoji.ttf");
        _imGuiShell.FontManager.AddFontTTF(_modConfig.ModId, "twemoji", twitterColorEmojiFontPath, 14.0f, [0x1, 0x1FFFF], new ImFontOptions()
        {
            FontLoaderFlags = (uint)(ImGuiFreeTypeLoaderFlags.ImGuiFreeTypeBuilderFlags_LoadColor | ImGuiFreeTypeLoaderFlags.ImGuiFreeTypeBuilderFlags_Bitmap),
            MergeMode = true
        });

        var style = _imGui.GetStyle();
        style.FrameRounding = 4.0f;
        style.WindowRounding = 4.0f;
        style.WindowBorderSize = 0.0f;
        style.PopupBorderSize = 0.0f;
        style.GrabRounding = 4.0f;
        style.Colors[(int)ImGuiCol.ImGuiCol_TitleBgActive] = new Vector4(0.7f, 0.3f, 0.3f, 1.00f);
    }

    private void RenderAnimatedTitle()
    {
        uint col1 = 0xAE3E15;
        uint col2 = 0xF1994B;
        float durationSec = 3f;

        var newCol = Vector4.Lerp(
            new Vector4(((col1 >> 16) & 0xFF) / 255.0f, ((col1 >> 8) & 0xFF) / 255.0f, (col1 & 0xFF) / 255.0f, 1.0f),
            new Vector4(((col2 >> 16) & 0xFF) / 255.0f, ((col2 >> 8) & 0xFF) / 255.0f, (col2 & 0xFF) / 255.0f, 1.0f),
            PingPong((float)_imGui.GetTime() / durationSec, 1.0f));

        string text = $"FaithFramework v{_modConfig.ModVersion}";
        Vector2 titleSize = _imGui.CalcTextSize(text);
        _imGui.SameLineEx(_imGui.GetWindowWidth() - (titleSize.X + 16), 0);
        _imGui.TextColored(new Vector4(newCol.X, newCol.Y, newCol.Z, 1.0f), text);

        string fpsText = $"{_imGui.GetIO().Framerate:0.00} FPS ({1000.0f / _imGui.GetIO().Framerate:0.00} ms) / ";
        Vector2 fpsSize = _imGui.CalcTextSize(fpsText);
        _imGui.SameLineEx(_imGui.GetWindowWidth() - titleSize.X - (fpsSize.X + 16), 0);
        _imGui.Text(fpsText);
    }

    private void OnFirstImGuiRender()
    {
        _imGuiShell.LogWriteLine("FaithFramework", $"FaithFramework {_modConfig.ModVersion} by Nenkai loaded.");
        _imGuiShell.LogWriteLine("FaithFramework", $"ImGui {_imGui.GetVersion()} loaded.");
        _imGuiShell.LogWriteLine("FaithFramework", "FFXVI Modding - nenkai.github.io/ffxvi-modding/");
        _imGuiShell.LogWriteLine("FaithFramework", "Press the INSERT key to show the main menu.");
    }


    private void InitNex()
    {
        _nexHooks = _services.GetRequiredService<NexHooks>();

        _modLoader.AddOrReplaceController<INextExcelDBApi>(_owner, _services.GetRequiredService<INextExcelDBApi>());
        _modLoader.AddOrReplaceController<INextExcelDBApiManaged>(_owner, _services.GetRequiredService<INextExcelDBApiManagedV2>());
        _modLoader.AddOrReplaceController<INextExcelDBApiManagedV2>(_owner, _services.GetRequiredService<INextExcelDBApiManagedV2>());
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