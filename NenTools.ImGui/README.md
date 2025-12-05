# NenTools.ImGui

> [!WARNING]
> Work in progress and not ready for usage.

Interfaced Dear ImGui bindings and hooks for injection into games. Mainly intended for use with Reloaded-II and Windows based games.

Currently supports DirectX12 exclusively.

## Features

* Interfaced bindings created using [Dear Bindings](https://github.com/dearimgui/dear_bindings) **enabling support for comments**
* Closely mapped certain value types to C# types (`ImVec2` -> `Vector2`, etc), enums supported
* `ImVector` types wrapped to `IEnumerable` types that can be safely iterated
* No unsafe code (for the most part)
* Game injection support
* DirectX 12 support (third-party overlays supported)
* Shell support (main menu & API)
* Texture Manager

## The Problem

Existing ImGui bindings available from other libraries such as [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET), [Hexa.NET.ImGui](https://github.com/HexaEngine/Hexa.NET.ImGui), [Michi.DearImGui](https://github.com/MochiLibraries/Mochi.DearImGui) and [DearImguiSharp](https://github.com/Sewer56/DearImguiSharp) all exhibit a common problem: **it was impossible to share state between consumers aswell as update ImGui, while preserving the majority of ImGui features in any reasonable manner**. 

This is particularly problematic when creating frameworks or plugin systems where a single consumer may be able to interact with one core ImGui instance. Historically, the way to get around this was to wrap ImGui manually for each single method of interest, which often omitted a large chunk of ImGui itself.

This project aims to remediate this by creating bindings in a different manner, by splitting them into three distinct components:
* Methods (Low-level bindings & structures)
* Interface (Sharable to consumers)
* Implementation (Contains wrapped methods, structures and enums, for use in your main framework).

The process of updating ImGui is to diff changes made between one version to another, and provide polyfills as needed in the implementation.

Example usage, using [Reloaded-II](https://github.com/Reloaded-Project/Reloaded-II)'s IExports feature:

```csharp
public Type[] GetTypes() =>
[
    typeof(IImGui),
    // typeof(IImGuiShell), Optional, shell for consumers
];

private IImGui _imGui;

public Mod(ModContext context)
{
    // ...
    _imGui = new NenTools.ImGui.Implementation.ImGui();
    _modLoader.AddOrReplaceController<IImGui>(_owner, _imGui);
}
```

## Server/Framework Usage

```csharp
var config = new ImGuiConfig(); // Or parse from a file
 var imGuiHookDx12 = new ImguiHookDx12();
_imGuiShell = new ImGuiShell(_hooks!, imGuiHookDx12, _imGui, config);
_imGuiShell.OnImGuiConfiguration += ConfigureImgui; // Callback that can be used to setup fonts and components for the shell
_imGuiShell.OnEndMainMenuBarRender += RenderAnimatedTitle; // Can be used to decorate the top menu bar
_imGuiShell.OnLogMessage += (message, color) => _logger.WriteLine(message, color ?? System.Drawing.Color.White);
_imGuiShell.OnFirstRender += OnFirstImGuiRender; // Can be used to display a startup message
_imGuiShell.SetupHooks();

// Share ImGuiShell and IImGui. In Reloaded-II, you can do this using IExports and AddOrUpdateController.
// You should hook the game's inputs to interact with the shell to be able to handle mouse movement without ImGui interfering with the game.

// Once you have a game frame from the game, start injecting.
_imGuiShell.Start();
```

## Consumer Usage

1. Grab `IImGui` **and** `IImGuiShell`:
```csharp
_imGui = _modLoader.GetController<IImGui>();
if (!_imGui.TryGetTarget(out IImGui imGui))
{
    _logger.WriteLine($"[{_modConfig.ModId}] Could not get IImGui.");
    return;
}

_imGuiShell = _modLoader.GetController<IImGuiShell>();
if (!_imGuiShell.TryGetTarget(out IImGuiShell imGuiShell))
{
    _logger.WriteLine($"[{_modConfig.ModId}] Could not get IImGuiShell.");
    return;
}
```

2. Inherit from `IImGuiComponent`:
```csharp
[ImGuiMenu(Category = "Other", Priority = 0, Owner = "MyImGuiMod")]
public class MyImGuiComponent : IImGuiComponent
{
    // If this is enabled, this will also render while the ImGui overlay is currently hidden
    public bool IsOverlay => false;

    private readonly IImGui _imgui;

    public MyImGuiComponent(IImGui imgui)
    {
        _imgui = imgui;
    }

    public void RenderMenu()
    {
        // Write code to render menu entries here.
        // By default, the shell will render a top menu bar which you can add elements to.
        if (_imgui.BeginMenu("MyModName"))
        {
            _imgui.MenuItem("Menu Item 1");
            _imgui.MenuItem("Menu Item 2");
            _imgui.MenuItem("Menu Item 3");

            imgui.EndMenu();
        }
    }

    public void Render(IImGuiShell imguiShell)
    {
        // Insert code to render anything here.
        if (_imgui.Begin("MyWindow", ref _windowOpen, ImGuiWindowFlags.ImGuiWindowFlags_None))
        {
            _imgui.Text("Hello World!");
        }

        _imgui.End();
    }
}
```

3. Register your component to the ImGui system:
```csharp
// category is the top menu entry which this component will render menu items to.
// name is only used for sorting purposes.
// If your component needs to render a menu, it should always render a sub-group, for clarity.
// Example:
// Tools
// -> MyModName <- This is where RenderMenu starts.
//   -> Menu Item 1
//   -> Menu Item 2
//   -> Menu Item 3
imguiShell.AddComponent(new PhotoModeMenu(_imGui));
```

> [!WARNING]
> The user may choose to not load ImGui. If you are running logic that needs to run depending on the overlay, ensure to check that `ImGuiShell.IsOverlayLoaded` is `true`.

That's all you should needed to render something on screen.

### Textures/Images

If you need to render textures, you should:

* Grab a `IImGuiTextureManager` instance from `IImGuiShell`
* Pass it to your component. On `Render`, load the texture using `IImGuiTextureManager.LoadImage`. Make sure to only do this once and not every frame!
* Pass the newly added image's `TexId` to `IImGui.Image`, etc.
* Make sure to dispose of these textures when you don't need to use them anymore (closing window, etc).

## Bindings Creation

The ImGui bindings have been built using a fork of [dear-bindings](https://github.com/Nenkai/dear_bindings) with the following configuration:

* Built with Freetype (and PlutoSVG for svg font/emoji support)
* DX9/11/12 backend support
* Compiled as Release, with asserts enabled

The bindings have been generated using [NenTools.ImGui.Generator](NenTools.ImGui.Generator), which also takes a different approach.

It functions by massaging the output of [ClangSharpPInvokeGenerator](https://github.com/dotnet/ClangSharp) into an interface (for Reloaded and mods), and the implementation/actual bindings.

### Building Bindings

> [!NOTE]
> You only need this if you intend to maintain the bindings.

### Step 1: C Bindings

1. Clone [`imgui`](https://github.com/ocornut/imgui) to own folder. Then `git checkout` a specific tag, such as the latest **docking** tag i.e `v1.92.5-docking`
2. Clone [dear-bindings](https://github.com/Nenkai/dear_bindings) (fork) to own folder, NEXT TO imgui. Important!
    * You may need to rebase if you are updating bindings.
3. Install python/requirements for dear-bindings
4. Run `BuildAllBindings.bat`
5. Open `examples/Examples.sln`. The one project we will be editing is **ImGuiLib**.
6. Install vcpkg. Nowadays Visual Studio has an install option for it, which may be default too.
7. Enable vcpkg. Open `Developer Command Prompt for VS` with Administrator permissions, and run `vcpkg integrate install`.
    * To be sure, ImGuiLib properties > vcpkg -> Use Vcpkg Manifest should be on Yes
8. (Hack) remove `extern` for `cImGui_ImplWin32_WndProcHandler` in `dcimgui_impl_win32.h` such that it looks like this:
```cpp
CIMGUI_IMPL_API LRESULT cImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);
```
9. Compile as Release (only ImGuiLib!). You should get `ImGuiLib.dll` and `ImGuiLib.pdb`.

> [!WARNING]
> You may also need to adjust hardcoded include paths I left in for plutosvg because I couldn't get Visual Studio/vcpkg to detect plutosvg as it should.

### Step 2: C# Bindings

1. Install ClangSharpPInvokeGenerator with `dotnet tool install --global ClangSharpPInvokeGenerator --version 20.1.2`
2. Run `generate_dotnet_bindings.bat`.

> [!NOTE]
> * You need to add `#include <Windows.h>` above `CIMGUI_IMPL_API LRESULT cImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);`     temporarily in `dcimgui_impl_win32.h`
> * And `#include <d3d11.h>` to `dcimgui_impl_dx11.h` so that the bindings generation works.

3. The `generated` folder will have `.cs` files with bindings once done.

### Step 3: NenTools.ImGui.Generator

The solution has a `NenTools.ImGui.Generator` project. Compile and run it, the first argument to the folder should be the path to the `generated` folder.

Once you run it, you will have `ImGui.cs`, `IImGui.cs` and `ImGuiMethods.cs` files in that project's `generated` folder. They will be in their distinct folders ready to copy to the actual solution folder.

If you are updating bindings you may need to diff changes and provide polyfills if needed.

## Credits

* Sewer56 for [DearImguiSharp](https://github.com/Sewer56/DearImguiSharp) and [Reloaded.Imgui](https://github.com/Sewer56/Reloaded.Imgui.Hook), which a good amount of this code is based on
* [REFramework](https://github.com/praydog/REFramework) for having a good amount of DX12 hook code that handles overlays and swapchain hijackers such as DLSS/FSR Frame Gen, but also Proton
* [DearImgui](https://github.com/ocornut/imgui)
