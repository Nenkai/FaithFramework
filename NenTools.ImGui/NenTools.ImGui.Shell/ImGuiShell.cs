using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Hooks;
using NenTools.ImGui.Implementation;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Native;
using NenTools.ImGui.Shell.Windows;
using NenTools.ImGui.Shell.Interfaces;

using Reloaded.Hooks.Definitions;

using static NenTools.ImGui.Shell.Interfaces.IImGuiShell;
using Microsoft.Extensions.Logging;

namespace NenTools.ImGui.Shell;

public class ImGuiShell : IImGuiShell
{
    private readonly IReloadedHooks _hooks;
    private readonly IImGui _imGui;
    private readonly IImguiHook _imguiHook;
    private readonly ImGuiConfig _imGuiConfig;

    public IImGuiTextureManager TextureManager { get; }

    private bool _menuVisible = true;

    private List<IImGuiComponent> _components = [];

    public event OnImGuiConfigurationDelegate? OnImGuiConfiguration;
    public event OnFirstRenderDelegate? OnFirstRender;
    public event OnEndMainMenuBarRenderDelegate? OnEndMainMenuBarRender;
    public event OnLogMessageDelegate? OnLogMessage;

    // Categories
    //  top menu categories (for sorting)
    //  -> groups of component lists, grouped by priority
    //    -> list of components, ordered by name
    private readonly Dictionary<string, SortedList<int, SortedDictionary<string, List<IImGuiComponent>>>> _menuCategoryToComponentList = [];

    public const int SystemPriority = -1000;

    public bool ContextCreated { get; private set; } = false;
    public bool IsOverlayEnabled { get; private set; } = false;

    public bool MouseActiveWhileMenuOpen { get; private set; } = true;
    public bool IsMainMenuOpen => _menuVisible;

    public string FileMenuName => "File";
    public string ToolsMenuName => "Tools";
    public string OtherMenuName => "Other";

    private OverlayLogger _overlayLogger;

    public ImGuiShell(IReloadedHooks hooks, IImguiHook imguiHook, IImGui imgui, ImGuiConfig imGuiConfig, ILoggerFactory? loggerFactory = null)
    {
        _hooks = hooks;
        _imguiHook = imguiHook; 
        _imGui = imgui;
        _imGuiConfig = imGuiConfig;

        _overlayLogger = new OverlayLogger(_imGui, _imGuiConfig);
        TextureManager = new ImGuiTextureManager(imguiHook, loggerFactory);
    }

    public void DisableOverlay() => IsOverlayEnabled = false;
    public void EnableOverlay() => IsOverlayEnabled = true;

    public void HideMenu() => _menuVisible = false;
    public void ShowMenu() => _menuVisible = true;
    public void ToggleMenuState() => _menuVisible = !_menuVisible;

    /// <summary>
    /// Sets up all ImGui hooks on startup.
    /// </summary>
    public void SetupHooks()
    {
        SDK.Init(_hooks, debug => OnLogMessage?.Invoke(debug, Color.White));
    }

    /// <summary>
    /// Creates and starts the ImGui context/rendering.
    /// </summary>
    /// <returns></returns>
    public async Task Start()
    {
        await ImguiHook.Create(Render, new ImguiHookOptions()
        {
            // TODO: Implement viewports
            // EnableViewports = false, Not yet functional with DX12 hooks, it creates a new swapchain that shouldn't be hooked.
            IgnoreWindowUnactivate = true,
            Implementations = [_imguiHook]
        });
        ContextCreated = true;

        _menuCategoryToComponentList.Add(FileMenuName, []);
        _menuCategoryToComponentList.Add(ToolsMenuName, []);
        _menuCategoryToComponentList.Add(OtherMenuName, []);

        AddComponent(new DemoWindow(_imGui));
        AddComponent(_overlayLogger);

        OnImGuiConfiguration?.Invoke();
    }

    public void AddComponent(IImGuiComponent component, string? overrideCategory = null, int overridePriority = 0, string? overrideOwner = null)
    {
        ImGuiMenuAttribute? attr = component.GetType().GetCustomAttribute<ImGuiMenuAttribute>();
        string? category = !string.IsNullOrEmpty(overrideCategory) ? overrideCategory : attr?.Category;
        int priority = overridePriority != 0 ? overridePriority : attr?.Priority ?? 0;
        string? owner = !string.IsNullOrEmpty(overrideOwner) ? overrideOwner : attr?.Owner;

        if (!string.IsNullOrEmpty(category))
        {
            if (!_menuCategoryToComponentList.TryGetValue(category, out var priorityGroup))
            {
                priorityGroup = [];
                _menuCategoryToComponentList.TryAdd(category, priorityGroup);
            }

            if (!priorityGroup.TryGetValue(priority, out var componentBucket))
            {
                componentBucket = [];
                priorityGroup.TryAdd(priority, componentBucket);
            }

            owner ??= "<unnamed>";
            if (!componentBucket.TryGetValue(owner, out List<IImGuiComponent>? components))
            {
                components = [];
                componentBucket.Add(owner, components);
            }

            components.Add(component);
        }

        _components.Add(component);
    }


    public void AddMenuSeparator(string? displayName, string category, int priority, string orderString)
    {
        ArgumentException.ThrowIfNullOrEmpty(category, nameof(category));
        ArgumentException.ThrowIfNullOrEmpty(orderString, nameof(orderString));

        AddComponent(new ImGuiSeparator(_imGui, displayName), category, priority, orderString);
    }

    private bool _firstRender = false;
    private void Render()
    {
        if (!IsOverlayEnabled)
            return;

        // Test zone
        // TestImGui();

        if (!_firstRender)
        {
            OnFirstRender?.Invoke();
            _firstRender = true;
        }

        foreach (IImGuiComponent component in _components)
        {
            if (component.IsOverlay)
                component.Render(this);
        }

        if (!_menuVisible)
            return;

        if (_imGui.BeginMainMenuBar())
        {
            foreach (var mainMenuCategory in _menuCategoryToComponentList)
            {
                if (_imGui.BeginMenu(mainMenuCategory.Key))
                {
                    foreach (var prorityGroups in mainMenuCategory.Value)
                    {
                        foreach (var componentBucket in prorityGroups.Value)
                        {
                            foreach (IImGuiComponent component in componentBucket.Value)
                            {
                                component.RenderMenu(this);
                            }
                        }
                    }

                    _imGui.EndMenu();
                }
            }

            OnEndMainMenuBarRender?.Invoke();
            _imGui.EndMainMenuBar();
        }

        foreach (var component in _components)
        {
            if (!component.IsOverlay)
                component.Render(this);
        }
    }

    public void LogWriteLine(string source, string message, Color? color = null, LoggerOutputTargetFlags loggerTargetFlags = LoggerOutputTargetFlags.AllButOverlayLogger)
    {
        if (loggerTargetFlags.HasFlag(LoggerOutputTargetFlags.GeneralLoggers))
            OnLogMessage?.Invoke($"[{source}] {message}", color ?? null);

        if (loggerTargetFlags.HasFlag(LoggerOutputTargetFlags.OverlayLogger))
            _overlayLogger.AddMessage(source, message, color);
    }

    private unsafe void TestImGui()
    {
        IImGuiIO io = _imGui.GetIO();
        IImFontAtlas atlas = io.Fonts;
        foreach (IImFont font in atlas.Fonts)
        {

        }

        IImGuiPlatformIO platIo = _imGui.GetPlatformIO();
        IImVectorWrapper<IImGuiPlatformMonitor> monitors = platIo.Monitors;
        foreach (IImGuiPlatformMonitor monitor in monitors)
        {

        }
        RangeStructAccessor<IImGuiKeyData> keys = io.KeysData;
        IImGuiKeyData key0 = keys[0];
        IImGuiKeyData key1 = keys[1];

        var builderStruct = new ImFontGlyphRangesBuilderStruct();
        var builder = new Implementation.ImGui.ImFontGlyphRangesBuilder(&builderStruct);
        _imGui.ImFontGlyphRangesBuilder_Clear(builder);
        _imGui.ImFontGlyphRangesBuilder_AddText(builder, "Hello World", null);
        _imGui.ImFontGlyphRangesBuilder_AddChar(builder, 0x7262);
        _imGui.ImFontGlyphRangesBuilder_AddRanges(builder, ref Unsafe.AsRef<uint>(_imGui.ImFontAtlas_GetGlyphRangesJapanese(io.Fonts)));
        _imGui.ImFontGlyphRangesBuilder_BuildRanges(builder, out var ranges);

        IImStructPtrVectorWrapper<IImTextureData> textures = platIo.Textures;
        IImTextureData texture = textures[0];

        IImDrawList drawList = _imGui.GetWindowDrawList();
        _imGui.ImDrawList_AddCircle(drawList, new Vector2(1.0f, 1.0f), 1, 0xFFFFFFFF);

        string version = _imGui.GetVersion();
        double time = _imGui.GetTime();
    }
}
