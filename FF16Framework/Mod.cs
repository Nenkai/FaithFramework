using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

using NenTools.ImGui.Hooks;
using NenTools.ImGui.Hooks.DirectX12;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Shell.Interfaces;

using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

using Tomlyn;

using Windows.Win32;

using FF16Framework.Interfaces.Nex;
using FF16Framework.Nex;
using FF16Framework.Save;
using FF16Framework.Template;
using FF16Framework.ImGuiManager.Windows;

using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;
using FF16Framework.ImGuiManager.Hooks;

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

    private NexHooks _nexHooks;
    private SaveHooks _saveHooks;

    private NextExcelDBApi _nexApi;
    private NextExcelDBApiManaged _nexApiManaged;

    private ImGuiShell _imGuiShell;
    private ImGuiTextureManager _imGuiTextureManager;
    private ImGuiConfig _imGuiConfig;
    private IImGui _imGui;
    private ImguiHookDx12 _imGuiHookDX12;
    private ImGuiInputHookManager _imGuiInputHook;

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

        InitSaveHooks();
        InitNex();
        InitImGui();

        _logger.WriteLine($"[{_modConfig.ModId}] Framework {_modConfig.ModVersion} initted.", _logger.ColorGreen);
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
                _imGuiShell.Start(new ImguiHookOptions()
                {
                    // TODO: Implement viewports
                    // EnableViewports = false, Not yet functional with DX12 hooks, it creates a new swapchain that shouldn't be hooked.
                    Implementations = [_imGuiHookDX12]
                }).GetAwaiter().GetResult();
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

    private void OnFirstImGuiRender()
    {
        _imGuiShell.LogWriteLine("FaithFramework", $"FaithFramework {_modConfig.ModVersion} loaded.", loggerTargetFlags: LoggerOutputTargetFlags.All);
        _imGuiShell.LogWriteLine("FaithFramework", $"ImGui {_imGui.GetVersion()} loaded.", loggerTargetFlags: LoggerOutputTargetFlags.All);
        _imGuiShell.LogWriteLine("FaithFramework", "https://nenkai.github.io/ffxvi-modding/", loggerTargetFlags: LoggerOutputTargetFlags.All);
        _imGuiShell.LogWriteLine("FaithFramework", "Press the INSERT key to show the main menu.", loggerTargetFlags: LoggerOutputTargetFlags.All);
    }

    private readonly static uint[] _emojiRangePtr = [0x100, 0x30000, 0 /* Null termination */];
    private unsafe void ConfigureImgui()
    {
        string modFolder = _modLoader.GetDirectoryForModId(_modConfig.ModId);

        IImGuiIO io = _imGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.ImGuiConfigFlags_DockingEnable;

        using IDisposableHandle<IImFontConfig> configHandle = _imGui.CreateFontConfig();
        IImFontConfig config = configHandle.Value;

        // English font
        string robotoFontPath = Path.Combine(modFolder, "ImGuiManager", "Fonts", "Roboto", "Roboto-Medium.ttf");
        _imGui.ImFontAtlas_AddFontFromFileTTF(io.Fonts, robotoFontPath, 15.0f, null!, ref Unsafe.AsRef<uint>(_imGui.ImFontAtlas_GetGlyphRangesDefault(io.Fonts)));

        // Emojis
        string twitterColorEmojiFontPath = Path.Combine(modFolder, "ImGuiManager", "Fonts", "TwitterColorEmoji", "twemoji.ttf");
        config.FontLoaderFlags |= (uint)(ImGuiFreeTypeLoaderFlags.ImGuiFreeTypeBuilderFlags_LoadColor | ImGuiFreeTypeLoaderFlags.ImGuiFreeTypeBuilderFlags_Bitmap);
        config.MergeMode = true;
        _imGui.ImFontAtlas_AddFontFromFileTTF(io.Fonts, twitterColorEmojiFontPath, 14.0f, config, ref _emojiRangePtr[0]);

        // Japanese font
        string netoSansJpFontPath = Path.Combine(modFolder, "ImGuiManager", "Fonts", "Noto", "NotoSansJP-Medium.ttf");
        _imGui.ImFontAtlas_AddFontFromFileTTF(io.Fonts, netoSansJpFontPath, 17.0f, config, ref Unsafe.AsRef<uint>(_imGui.ImFontAtlas_GetGlyphRangesJapanese(io.Fonts)));

        var style = _imGui.GetStyle();
        style.FrameRounding = 4.0f;
        style.WindowRounding = 4.0f;
        style.WindowBorderSize = 0.0f;
        style.PopupBorderSize = 0.0f;
        style.GrabRounding = 4.0f;
        style.Colors[(int)ImGuiCol.ImGuiCol_TitleBgActive] = new Vector4(0.7f, 0.3f, 0.3f, 1.00f);

        // Shell components
        string logPath = Path.Combine(_modLoader.GetDirectoryForModId(_modConfig.ModId), "framework_log.txt");
        _imGuiShell.AddComponent(new LogWindow(_imGui, _logger, logPath));
        _imGuiShell.AddComponent(new SettingsComponent(_imGui, _imGuiConfig));
        _imGuiShell.AddComponent(new AboutWindow(_imGui, _modConfig, _modLoader));
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


        _imGui = new NenTools.ImGui.Implementation.ImGui();
        _imGuiHookDX12 = new ImguiHookDx12();
        _imGuiShell = new ImGuiShell(_hooks!, _imGuiHookDX12, _imGui, _imGuiConfig);
        _imGuiInputHook = new ImGuiInputHookManager(_imGuiShell, _hooks, _modConfig);

        _imGuiShell.OnImGuiConfiguration += ConfigureImgui;
        _imGuiShell.OnEndMainMenuBarRender += RenderAnimatedTitle;
        _imGuiShell.OnLogMessage += (message, color) => _logger.WriteLine(message, color ?? System.Drawing.Color.White);
        _imGuiShell.OnFirstRender += OnFirstImGuiRender;
        _imGuiShell.SetupHooks();
        _imGuiInputHook.SetupInputHooks();

        nint destroyWindowPtr = PInvoke.GetProcAddress(PInvoke.GetModuleHandle("user32.dll"), "DestroyWindow");
        _destroyWindowHook = _hooks!.CreateHook<DestroyWindow>(DestroyWindowImpl, destroyWindowPtr).Activate();

        // We hook the call that performs present (not present itself)
        // We setup our DX12 hooks after the game made the first call.
        Project.Scans.AddScanHook(nameof(RenderExecCommandListsAndPresent),
            (result, hooks) => RenderExecCommandListsAndPresentHook = _hooks.CreateHook<RenderExecCommandListsAndPresent>(RenderExecCommandListsAndPresentImpl, result).Activate());

        _modLoader.AddOrReplaceController<IImGui>(_owner, _imGui);
        _modLoader.AddOrReplaceController<IImGuiShell>(_owner, _imGuiShell);
        _modLoader.AddOrReplaceController<IImGuiTextureManager>(_owner, _imGuiTextureManager);
    }

    private void InitNex()
    {
        _nexHooks = new NexHooks(_configuration, _modConfig, _logger);
        _nexHooks.Setup();

        _nexApi = new NextExcelDBApi(_nexHooks);
        _nexApiManaged = new NextExcelDBApiManaged(_nexHooks);
        _modLoader.AddOrReplaceController<INextExcelDBApi>(_owner, _nexApi);
        _modLoader.AddOrReplaceController<INextExcelDBApiManaged>(_owner, _nexApiManaged);
        _modLoader.AddOrReplaceController<INextExcelDBApiManagedV2>(_owner, _nexApiManaged);
    }

    private void InitSaveHooks()
    {
        _saveHooks = new SaveHooks(_configuration, _modConfig, _logger);
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