using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.ImGui.Hooks;
using FF16Framework.ImGuiManager.Hooks;
using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;
using FF16Framework.Native.ImGui;

using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;

using SharedScans.Interfaces;

using Windows.Win32;

namespace FF16Framework.ImGuiManager;

public class ImGuiSupport : IImGuiSupport
{
    private readonly IReloadedHooks _hooks;
    private readonly IModConfig _modConfig;
    private readonly ILogger _logger;
    private readonly IImGui _imgui;
    private readonly IImguiHook _imguiHook;
    private ImGuiInputHookManager _inputManager;
    private ImGuiConfig _imGuiConfig;
    private string _modFolder;

    private bool _menuVisible = false;

    private List<IImGuiComponent> _components = [];
    
    // Categories
    //  name (for sorting) -> list of components to render
    private readonly Dictionary<string, SortedDictionary<string, List<IImGuiComponent>>> _menuCategoryToComponentList = [];

    public delegate bool DestroyWindow(nint hwnd);
    private IHook<DestroyWindow>? _destroyWindowHook;

    public bool ContextCreated { get; private set; } = false;
    public bool IsOverlayLoaded { get; private set; } = false;

    public bool MouseActiveWhileMenuOpen = false;
    public bool IsMainMenuBarOpen => _menuVisible;

    public void ToggleMenuState() => _menuVisible = !_menuVisible;

    public string FileMenuName => "File";
    public string ToolsMenuName => "Tools";
    public string OtherMenuName => "Other";

    private OverlayLogger _overlayLogger;

    public ImGuiSupport(IReloadedHooks hooks, ISharedScans scans, IModConfig modConfig, ILogger logger,
        IImguiHook imguiHook, IImGui imgui, ImGuiConfig imGuiConfig)
    {
        _hooks = hooks;
        _modConfig = modConfig;
        _logger = logger;
        _imguiHook = imguiHook; 
        _imgui = imgui;
        _imGuiConfig = imGuiConfig;
        _inputManager = new ImGuiInputHookManager(this, hooks, scans, modConfig);
    }

    /// <summary>
    /// Sets up all hooks on startup.
    /// </summary>
    /// <param name="modFolder"></param>
    public void SetupHooks(string modFolder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modFolder, nameof(modFolder));

        _modFolder = modFolder;

        _inputManager.SetupInputHooks();
        SDK.Init(_hooks);

        var handle = PInvoke.GetModuleHandle("user32.dll");
        nint destroyWindowPtr = PInvoke.GetProcAddress(handle, "DestroyWindow");
        _destroyWindowHook = _hooks.CreateHook<DestroyWindow>(DestroyWindowImpl, destroyWindowPtr).Activate();
    }

    /// <summary>
    /// Creates and starts the ImGui context/rendering.
    /// </summary>
    /// <returns></returns>
    public async Task Start()
    {
        await ImguiHook.Create(Render, new ImguiHookOptions()
        {
            // We disable viewports. Why?
            // Because I can't for the life of me figure out DX12 hooks properly.

            // When starting the game windowed/on another monitor, ImGui renders to a black square on the main monitor.
            // Or, when switching from borderless/fullscreen to windowed, the same happens.

            // My head hurts figuring this out and I'm clearly not smart enough to figure it out.
            // I give up.
            EnableViewports = false,
            IgnoreWindowUnactivate = true,
            Implementations = [_imguiHook]
        });
        ContextCreated = true;

        ConfigureImgui(_modFolder);

        _menuCategoryToComponentList.Add(FileMenuName, []);
        _menuCategoryToComponentList.Add(ToolsMenuName, []);
        _menuCategoryToComponentList.Add(OtherMenuName, []);

        _overlayLogger = new OverlayLogger(_imGuiConfig);
        AddComponent(_overlayLogger);
    }

    // Only allow rendering once the splash screen is gone.
    private bool DestroyWindowImpl(nint hwnd)
    {
        Span<char> name = stackalloc char[256];
        PInvoke.GetClassName(new global::Windows.Win32.Foundation.HWND(hwnd), name);
        string trimmed = name.ToString().TrimEnd('\0');
        if (trimmed == "SplashClass")
            IsOverlayLoaded = true;

        return _destroyWindowHook!.OriginalFunction(hwnd);
    }

    private static uint[] _emojiRangePtr = [0x100, 0x30000, 0 /* Null termination */];
    private unsafe void ConfigureImgui(string modFolder)
    {
        IImGuiIO io = _imgui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.ImGuiConfigFlags_DockingEnable;

        var config = CreateDefaultFontConfig();

        // English font
        string robotoFontPath = Path.Combine(modFolder, "ImGuiManager", "Fonts", "Roboto", "Roboto-Medium.ttf");
        _imgui.ImFontAtlas_AddFontFromFileTTF(io.Fonts, robotoFontPath, 15.0f, null, ref Unsafe.AsRef<uint>(_imgui.ImFontAtlas_GetGlyphRangesDefault(io.Fonts)));

        // Emojis
        string twitterColorEmojiFontPath = Path.Combine(modFolder, "ImGuiManager", "Fonts", "TwitterColorEmoji", "twemoji.ttf");
        config.FontLoaderFlags |= (uint)(ImGuiFreeTypeLoaderFlags.ImGuiFreeTypeBuilderFlags_LoadColor | ImGuiFreeTypeLoaderFlags.ImGuiFreeTypeBuilderFlags_Bitmap);
        config.MergeMode = true;

        ImGui.ImFontConfig conf = new ImGui.ImFontConfig(&config);
        _imgui.ImFontAtlas_AddFontFromFileTTF(io.Fonts, twitterColorEmojiFontPath, 14.0f, conf, ref _emojiRangePtr[0]);

        // Japanese font
        string netoSansJpFontPath = Path.Combine(modFolder, "ImGuiManager", "Fonts", "Noto", "NotoSansJP-Medium.ttf");
        _imgui.ImFontAtlas_AddFontFromFileTTF(io.Fonts, netoSansJpFontPath, 17.0f, new ImGui.ImFontConfig(&config), ref Unsafe.AsRef<uint>(_imgui.ImFontAtlas_GetGlyphRangesJapanese(io.Fonts)));
        
        var style = _imgui.GetStyle();
        style.FrameRounding = 4.0f;
        style.WindowRounding = 4.0f;
        style.WindowBorderSize = 0.0f;
        style.PopupBorderSize = 0.0f;
        style.GrabRounding = 4.0f;
        style.Colors[(int)ImGuiCol.ImGuiCol_TitleBgActive] = new Vector4(0.7f, 0.3f, 0.3f, 1.00f);
    }

    /// <summary>
    /// This is needed as AddFontFromFileTTF has sanity checks (and will assert/error if some properties are off for a default structure) <br/>
    /// Refer to ImFontConfig constructor - https://github.com/ocornut/imgui/blob/842837e35b421a4c85ca30f6840321f0a3c5a029/imgui_draw.cpp#L2404
    /// </summary>
    /// <returns></returns>
    private static ImFontConfigStruct CreateDefaultFontConfig()
    {
        var conf = new ImFontConfigStruct();
        conf.FontDataOwnedByAtlas = true;
        conf.OversampleH = 0;
        conf.OversampleV = 0;
        conf.GlyphMaxAdvanceX = float.MaxValue;
        conf.RasterizerMultiply = 1.0f;
        conf.RasterizerDensity = 1.0f;
        conf.EllipsisChar = 0;
        return conf;
    }

    public void AddComponent(IImGuiComponent component, string? category = null, string? name = null)
    {
        if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(name))
        {
            if (!_menuCategoryToComponentList.TryGetValue(category, out SortedDictionary<string, List<IImGuiComponent>>? imguiMenuComponents))
            {
                imguiMenuComponents = [];
                _menuCategoryToComponentList.TryAdd(category, imguiMenuComponents);
            }

            if (!imguiMenuComponents.TryGetValue(name, out List<IImGuiComponent>? sortedGroup))
            {
                sortedGroup = [];
                imguiMenuComponents.Add(name, sortedGroup);
            }

            sortedGroup.Add(component);
        }

        _components.Add(component);
    }


    public void AddMenuSeparator(string category, string modId, string? displayName)
    {
        ArgumentException.ThrowIfNullOrEmpty(category, nameof(category));
        ArgumentException.ThrowIfNullOrEmpty(modId, nameof(modId));

        AddComponent(new ImGuiSeparator(displayName), category, modId);

    }

    private ImGuiImage _image;
    private bool _noticeShown = false;
    private void Render()
    {
        if (!IsOverlayLoaded)
            return;

        // Test zone
        // TestImGui();

        if (!_noticeShown)
        {
            string ver = _imgui.GetVersion();
            LogWriteLine(nameof(FF16Framework), $"FF16Framework {_modConfig.ModVersion} loaded.", loggerTargetFlags: LoggerOutputTargetFlags.All);
            LogWriteLine(nameof(FF16Framework), $"ImGui {ver} loaded.", loggerTargetFlags: LoggerOutputTargetFlags.All);
            LogWriteLine(nameof(FF16Framework), "https://nenkai.github.io/ffxvi-modding/", loggerTargetFlags: LoggerOutputTargetFlags.All);
            LogWriteLine(nameof(FF16Framework), "Press the INSERT key to show the main menu.", loggerTargetFlags: LoggerOutputTargetFlags.All);
            _noticeShown = true;
        }

        foreach (IImGuiComponent component in _components)
        {
            if (component.IsOverlay)
                component.Render(this, _imgui);
        }

        if (!_menuVisible)
            return;

        if (_imgui.BeginMainMenuBar())
        {
            foreach (var mainMenuCategory in _menuCategoryToComponentList)
            {
                if (_imgui.BeginMenu(mainMenuCategory.Key))
                {
                    foreach (KeyValuePair<string, List<IImGuiComponent>> sortedGroup in mainMenuCategory.Value)
                    {
                        foreach (IImGuiComponent component in sortedGroup.Value)
                        {
                            component.RenderMenu(this, _imgui);
                        }
                    }

                    _imgui.EndMenu();
                }
            }

            RenderAnimatedTitle();
            _imgui.EndMainMenuBar();
        }

        foreach (var component in _components)
        {
            if (!component.IsOverlay)
                component.Render(this, _imgui);
        }
    }

    public void LogWriteLine(string source, string message, Color? color = null, LoggerOutputTargetFlags loggerTargetFlags = LoggerOutputTargetFlags.AllButOverlayLogger)
    {
        if (loggerTargetFlags.HasFlag(LoggerOutputTargetFlags.ReloadedLog))
        {
            if (color is not null)
                _logger.WriteLine($"[{source}] {message}", color.Value);
            else
                _logger.WriteLine($"[{source}] {message}");
        }

        if (loggerTargetFlags.HasFlag(LoggerOutputTargetFlags.OverlayLogger))
            _overlayLogger.AddMessage(source, message, color);

    }

    private void RenderAnimatedTitle()
    {
        uint col1 = 0xAE3E15;
        uint col2 = 0xF1994B;
        float durationSec = 3f;

        var newCol = Vector4.Lerp(
            new Vector4(((col1 >> 16) & 0xFF) / 255.0f, ((col1 >> 8) & 0xFF) / 255.0f, (col1 & 0xFF) / 255.0f, 1.0f),
            new Vector4(((col2 >> 16) & 0xFF) / 255.0f, ((col2 >> 8) & 0xFF) / 255.0f, (col2 & 0xFF) / 255.0f, 1.0f),
            PingPong((float)_imgui.GetTime() / durationSec, 1.0f));

        string text = $"FF16Framework v{_modConfig.ModVersion}";
        Vector2 titleSize = _imgui.CalcTextSize(text);
        _imgui.SameLineEx(_imgui.GetWindowWidth() - (titleSize.X + 16), 0);
        _imgui.TextColored(new Vector4(newCol.X, newCol.Y, newCol.Z, 1.0f), text);

        string fpsText = $"{_imgui.GetIO().Framerate:0.00} FPS ({1000.0f / _imgui.GetIO().Framerate:0.00} ms) / ";
        Vector2 fpsSize = _imgui.CalcTextSize(fpsText);
        _imgui.SameLineEx(_imgui.GetWindowWidth() - titleSize.X - (fpsSize.X + 16), 0);
        _imgui.Text(fpsText);
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

    private unsafe void TestImGui()
    {
        IImGuiIO io = _imgui.GetIO();
        IImFontAtlas atlas = io.Fonts;
        IImFont firstFont = atlas.Fonts[0];

        IImGuiPlatformIO platIo = _imgui.GetPlatformIO();
        ImStructVectorWrapper<IImGuiPlatformMonitor> monitors = platIo.Monitors;
        IImGuiPlatformMonitor firstMonitor = monitors[0];

        RangeStructAccessor<IImGuiKeyData> keys = io.KeysData;
        IImGuiKeyData key0 = keys[0];
        IImGuiKeyData key1 = keys[1];

        var builderStruct = new ImFontGlyphRangesBuilderStruct();
        var builder = new ImGui.ImFontGlyphRangesBuilder(&builderStruct);
        var range = new ImVector<uint>();
        _imgui.ImFontGlyphRangesBuilder_Clear(builder);
        _imgui.ImFontGlyphRangesBuilder_AddText(builder, "Hello World", null);
        _imgui.ImFontGlyphRangesBuilder_AddChar(builder, 0x7262);
        _imgui.ImFontGlyphRangesBuilder_AddRanges(builder, ref Unsafe.AsRef<uint>(_imgui.ImFontAtlas_GetGlyphRangesJapanese(io.Fonts)));
        _imgui.ImFontGlyphRangesBuilder_BuildRanges(builder, ref range);

        ImStructPtrVectorWrapper<IImTextureData> textures = platIo.Textures;
        IImTextureData texture = textures[0];

        IImDrawList drawList = _imgui.GetWindowDrawList();
        _imgui.ImDrawList_AddCircle(drawList, new Vector2(1.0f, 1.0f), 1, 0xFFFFFFFF);

        string version = _imgui.GetVersion();
        double time = _imgui.GetTime();
    }
}
