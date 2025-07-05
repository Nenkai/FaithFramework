using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.ImGui.Hooks;
using FF16Framework.ImGui.Hooks.DirectX12;
using FF16Framework.ImGuiManager.Windows;
using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;
using FF16Framework.Native.ImGui;

using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;

using SharedScans.Interfaces;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

using Windows.Win32;

namespace FF16Framework.ImGuiManager;

public unsafe class ImguiSupport : IImguiSupport
{
    private readonly IReloadedHooks _hooks;
    private readonly ISharedScans _scans;
    private readonly IModConfig _modConfig;
    private readonly IImGui _imgui;
    private InputManager _inputManager;

    private bool _menuVisible = false;

    private List<IImguiWindow> _windows = [];
    private readonly Dictionary<string, List<IImguiMenuComponent>> _menuCategoryToComponentList = [];

    public delegate bool DestroyWindow(nint hwnd);
    private IHook<DestroyWindow>? _destroyWindowHook;

    private ImguiHookDx12 _imguiHookDx12 = new();

    public bool MouseActiveWhileMenuOpen = false;
    public bool IsMainMenuBarOpen => _menuVisible;
    public void ToggleMenuState() => _menuVisible = !_menuVisible;

    public ImguiSupport(IReloadedHooks hooks, ISharedScans scans, IModConfig modConfig, IImGui imgui)
    {
        _hooks = hooks;
        _scans = scans;
        _modConfig = modConfig;
        _imgui = imgui;
        _inputManager = new InputManager(this, hooks, scans, modConfig);
    }

    public void SetupImgui(string modFolder)
    {
        _inputManager.SetupInputHooks();

        SDK.Init(_hooks);

        var handle = PInvoke.GetModuleHandle("user32.dll");
        nint destroyWindowPtr = PInvoke.GetProcAddress(handle, "DestroyWindow");
        _destroyWindowHook = _hooks.CreateHook<DestroyWindow>(DestroyWindowImpl, destroyWindowPtr).Activate();

        ImguiHook.Create(Render, new ImguiHookOptions()
        {
            EnableViewports = true,
            IgnoreWindowUnactivate = true,
            Implementations = [_imguiHookDx12]
        });

        ConfigureImgui(modFolder);

        AddWindow(OverlayLogger.Instance);
        AddWindow(new DemoWindow(), "Other");
        AddWindow(new AboutWindow(_modConfig), "Other");
    }

    public bool CanRender { get; set; } = false;

    public ImGuiImage LoadImage(string filePath)
    {
        Image<Rgba32>? image = null;
        byte[]? data = null;
        try
        {
            image = Image.Load<Rgba32>(filePath);

            int size = image.Width * image.Height * 4;
            data = ArrayPool<byte>.Shared.Rent(size);
            image.CopyPixelDataTo(data);
            ulong texId = _imguiHookDx12.LoadImage(data.AsSpan(0, size), (uint)image.Width, (uint)image.Height);
            return new ImGuiImage(texId, (uint)image.Width, (uint)image.Height);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            image?.Dispose();
            if (data is not null)
                ArrayPool<byte>.Shared.Return(data);
        }
    }

    public ImGuiImage LoadImage(Span<byte> rgba32Bytes, uint width, uint height)
    {
        if (rgba32Bytes.Length != width * height * 4)
            throw new ArgumentException("The provided bytes does not match the specified dimensions.");

        ulong texId = _imguiHookDx12.LoadImage(rgba32Bytes, width, height);
        return new ImGuiImage(texId, width, height);
    }

    public void FreeImage(ImGuiImage image)
    {
        _imguiHookDx12.FreeImage(image.TexId);
    }

    // Only allow rendering once the splash screen is gone.
    private bool DestroyWindowImpl(nint hwnd)
    {
        Span<char> name = stackalloc char[256];
        PInvoke.GetClassName(new global::Windows.Win32.Foundation.HWND(hwnd), name);
        string trimmed = name.ToString().TrimEnd('\0');
        if (trimmed == "SplashClass")
            CanRender = true;

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

    public void AddMenuSeparator(string category)
    {
        AddComponent(category, new ImguiSeparator());
    }

    public void AddWindow(IImguiWindow window, string? mainMenuCategory = null)
    {
        _windows.Add(window);

        if (!string.IsNullOrEmpty(mainMenuCategory))
            AddComponent(mainMenuCategory, window);
    }

    public void AddComponent(string category, IImguiMenuComponent component)
    {
        if (!_menuCategoryToComponentList.TryGetValue(category, out List<IImguiMenuComponent>? imguiMenuComponents))
            _menuCategoryToComponentList.TryAdd(category, [component]);
        else
            imguiMenuComponents.Add(component);
    }

    private ImGuiImage _image;
    private bool _noticeShown = false;
    private void Render()
    {
        if (!CanRender)
            return;

        // Test zone
        // TestImGui();

        if (!_noticeShown)
        {
            string ver = _imgui.GetVersion();
            OverlayLogger.Instance.AddMessage($"FF16Framework {_modConfig.ModVersion} loaded.");
            OverlayLogger.Instance.AddMessage($"ImGui {ver} loaded.");
            OverlayLogger.Instance.AddMessage("https://nenkai.github.io/ffxvi-modding/");
            OverlayLogger.Instance.AddMessage("Press the INSERT key to show the main menu.");
            _noticeShown = true;
        }

        foreach (IImguiWindow window in _windows)
        {
            if (window.IsOverlay)
                window.Render(this, _imgui);
        }

        if (!_menuVisible)
            return;

        if (_imgui.BeginMainMenuBar())
        {
            foreach (var mainMenuCategory in _menuCategoryToComponentList)
            {
                if (_imgui.BeginMenu(mainMenuCategory.Key))
                {
                    foreach (IImguiMenuComponent component in mainMenuCategory.Value)
                    {
                        component.BeginMenuComponent(_imgui);
                    }

                    _imgui.EndMenu();
                }
            }

            RenderAnimatedTitle();
            _imgui.EndMainMenuBar();
        }

        foreach (var window in _windows)
        {
            if (!window.IsOverlay)
                window.Render(this, _imgui);
        }
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

    private void TestImGui()
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
