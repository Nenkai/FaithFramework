using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Shell;
using NenTools.ImGui.Shell.Interfaces;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Native;

using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

using Windows.Win32;
using Windows.Win32.Foundation;

namespace FF16Framework.ImGuiManager.Hooks;

public unsafe class ImGuiInputHookManager
{
    private IHook<KeyboardManager_HandleWindowKeyboardKeyPressed>? _HOOK_KeyboardManager_HandleWindowKeyboardKeyPressed;
    public delegate void KeyboardManager_HandleWindowKeyboardKeyPressed(nint a1, nint a2);

    //private HookContainer<MouseDevice_WindowMessageIntercepter>? _HOOK_MouseDevice_WindowMessageIntercepter;
    //public delegate nint MouseDevice_WindowMessageIntercepter(nint hwnd, uint uMsg, nint wParam, nint lParam, nuint uIdSubClass, nint dwRefData);

    private IHook<KeyboardDevice_WindowMessageIntercepter>? _HOOK_KeyboardDevice_WindowMessageIntercepter;
    public delegate void KeyboardDevice_WindowMessageIntercepter(nint this_, uint uMsg, nint uMsg_, nint wParam, nuint lParam);

    public delegate nint DirectInput8Create(nint hinst, int dwVersion, nint riidltf, nint ppvOut, nint punkOuter);
    private IHook<DirectInput8Create>? _directInputCreateHook;

    public delegate nint CreateDevice(nint instance, Guid rguid, nint lplpDirectInputDevice, nint pUnkOuter);
    private IHook<CreateDevice>? _createDeviceHook;

    public delegate nuint GetDeviceState(nint instance, int cbData, byte* lpvData);
    private IHook<GetDeviceState> _getDeviceStateHook;

    public delegate nuint GetDeviceData(nint this_, nint cbObjectData, nint rgdod, nint pdwInOut, int dwFlags);
    private IHook<GetDeviceData>? _getDeviceDataHook;

    //public delegate void GetCursorPos(POINT* a1);
    //private IHook<GetCursorPos>? _getCursorPosHook;

    public delegate nint SetCursor(nint a1);
    private IHook<SetCursor>? _setCursorHook;

    private IImGuiShell _imGuiShell;
    private IReloadedHooks _hooks;
    private IModConfig _modConfig;

    public ImGuiInputHookManager(IImGuiShell imGuiShell, IReloadedHooks hooks, IModConfig modConfig)
    {
        _imGuiShell = imGuiShell;
        _hooks = hooks;
        _modConfig = modConfig;
    }

    public void SetupInputHooks()
    {
        // Mouse input: dinput8 (movement GetDeviceData (ingame)/GetCursorPos (UI), presses: GetDeviceState)
        // Keyboard input: WndProc messages

        Project.Scans.AddScanHook(nameof(KeyboardManager_HandleWindowKeyboardKeyPressed),
            (result, hooks) => _HOOK_KeyboardManager_HandleWindowKeyboardKeyPressed = hooks.CreateHook<KeyboardManager_HandleWindowKeyboardKeyPressed>(HandleWindowKeyboardKeyPressedImpl, result).Activate());

        //_HOOK_MouseDevice_WindowMessageIntercepter = _scans.CreateHook<MouseDevice_WindowMessageIntercepter>(MouseDevice_WindowMessageIntercepterImpl, _modConfig.ModId);
        Project.Scans.AddScanHook(nameof(KeyboardDevice_WindowMessageIntercepter),
            (result, hooks) => _HOOK_KeyboardDevice_WindowMessageIntercepter = hooks.CreateHook<KeyboardDevice_WindowMessageIntercepter>(KeyboardDeviceWindowMessageIntercepterImpl, result).Activate());

        // Chain hook direct input so imgui inputs don't also get passed to the game.
        var handle = PInvoke.GetModuleHandle("dinput8.dll");
        _directInputCreateHook = _hooks.CreateHook<DirectInput8Create>(DirectInput8CreateImpl, PInvoke.GetProcAddress(handle, "DirectInput8Create")).Activate();

        var user32 = PInvoke.GetModuleHandle("user32.dll");
        //_getCursorPosHook = _hooks.CreateHook<GetCursorPos>(GetCursorPosImpl, PInvoke.GetProcAddress(user32, "GetCursorPos")).Activate();
    }


    // GetCursorPos is used for UI cursor tracking.
    /*
    private void GetCursorPosImpl(POINT* a1)
    {
        if (_imguiSupport.ContextCreated && (ImGuiMethods.GetIO()->WantCaptureMouse || _imguiSupport.IsMainMenuOpen && !_imguiSupport.MouseActiveWhileMenuOpen))
        {
            a1->X = 0;
            a1->Y = 0;
        }
        else
        {
            _getCursorPosHook!.OriginalFunction(a1);
        }
    }
    */

    // This function handles all keyboard events.
    public void KeyboardDeviceWindowMessageIntercepterImpl(nint this_, uint uMsg, nint uMsg_, nint wParam, nuint lParam)
    {
        VirtualKeyStates keyState = (VirtualKeyStates)(uMsg - 0x100);
        if (_imGuiShell.ContextCreated && ImGuiMethods.GetIO()->WantCaptureKeyboard)
            return;

        _HOOK_KeyboardDevice_WindowMessageIntercepter!.OriginalFunction(this_, uMsg, uMsg_, wParam, lParam);
    }


    // This is fired when the game is attempting to register a key as actually pressed.
    private void HandleWindowKeyboardKeyPressedImpl(nint dwRefData, nint key)
    {
        VirtualKeyStates keyState = (VirtualKeyStates)(key - 0x100);
        if (keyState == VirtualKeyStates.VK_INSERT)
            _imGuiShell.ToggleMenuState(); // Don't return. otherwise the game gets confused input wise

        if (_imGuiShell.ContextCreated && ImGuiMethods.GetIO()->WantCaptureKeyboard)
            return;

        _HOOK_KeyboardManager_HandleWindowKeyboardKeyPressed!.OriginalFunction(dwRefData, key);
    }

    // This one handles mouse button presses. (we don't need it)
    /*
    private nint MouseDevice_WindowMessageIntercepterImpl(nint hwnd, uint uMsg, nint wParam, nint lParam, nuint uIdSubClass, nint dwRefData)
    {
        var io = ImGui.GetIO();
        if (io.WantCaptureMouse || (_imguiSupport.IsMenuOpen && !_imguiSupport.MouseActiveWhileMenuOpen))
            return _HOOK_MouseDevice_WindowMessageIntercepter!.OriginalFunction(hwnd, 0, wParam, lParam, uIdSubClass, dwRefData);

        return _HOOK_MouseDevice_WindowMessageIntercepter!.OriginalFunction(hwnd, uMsg, wParam, lParam, uIdSubClass, dwRefData);
    }
    */

    private nint DirectInput8CreateImpl(nint hinst, int dwVersion, nint riidltf, nint ppvOut, nint punkOuter)
    {
        nint result = _directInputCreateHook!.OriginalFunction(hinst, dwVersion, riidltf, ppvOut, punkOuter);

        // Get location of IDirectInput8::CreateDevice and hook it
        long* instancePtr = (long*)*(long*)ppvOut;
        long** vtbl = (long**)*instancePtr;
        _createDeviceHook = _hooks.CreateHook<CreateDevice>(CreateDeviceImpl, (nint)vtbl[3]).Activate();
        _directInputCreateHook.Disable();

        return result;
    }

    private nint _mouseDevice;
    private nint CreateDeviceImpl(nint this_, Guid rguid, nint lplpDirectInputDevice, nint pUnkOuter)
    {
        if (_directInputCreateHook!.IsHookEnabled)
            _directInputCreateHook.Disable();

        nint result = _createDeviceHook!.OriginalFunction(this_, rguid, lplpDirectInputDevice, pUnkOuter);
        long* instancePtr = (long*)*(long*)lplpDirectInputDevice;

        if (_getDeviceDataHook is null)
        {
            // Get location of IDirectInputDevice8::GetDeviceState and hook it
            long** vtbl = (long**)*instancePtr;

            nint getDeviceStatePtr = (nint)vtbl[9];
            _getDeviceStateHook = _hooks.CreateHook<GetDeviceState>(GetDeviceStateImpl, getDeviceStatePtr).Activate();

            nint getDeviceDataPtr = (nint)vtbl[10];
            _getDeviceDataHook = _hooks.CreateHook<GetDeviceData>(GetDeviceDataImpl, getDeviceDataPtr).Activate();
        }

        if (rguid == PInvoke.GUID_SysMouse)
            _mouseDevice = (nint)instancePtr;

        // Game uses SetWindowSubclass callback for keyboard input, so the keyboard device isn't registered

        return result;
    }

    // This is used for mouse movement (but not mouse button presses)
    private nuint GetDeviceDataImpl(nint this_, nint cbObjectData, nint rgdod, nint pdwInOut, int dwFlags)
    {
        if (this_ == _mouseDevice) // ImGui wants input? don't forward to game
        {
            if (_imGuiShell.ContextCreated && ImGuiMethods.GetIO()->WantCaptureMouse)
                return 0x8007000C; // DIERR_NOTACQUIRED
        }

        if (this_ == _mouseDevice)
        {
            if (_imGuiShell.ContextCreated && _imGuiShell.IsMainMenuOpen && !_imGuiShell.MouseActiveWhileMenuOpen)
                return 0x8007000C; // DIERR_NOTACQUIRED
        }

        return _getDeviceDataHook!.OriginalFunction(this_, cbObjectData, rgdod, pdwInOut, dwFlags);
    }

    // Used for mouse button presses
    private nuint GetDeviceStateImpl(nint instance, int cbData, byte* lpvData)
    {
        if (instance == _mouseDevice) // ImGui wants input? don't forward to game
        {
            if (_imGuiShell.ContextCreated && ImGuiMethods.GetIO()->WantCaptureMouse)
                return 0x8007000C; // DIERR_NOTACQUIRED
        }

        var res = _getDeviceStateHook.OriginalFunction(instance, cbData, lpvData);
        if (instance == _mouseDevice)
        {
            if (_imGuiShell.ContextCreated && _imGuiShell.IsMainMenuOpen && !_imGuiShell.MouseActiveWhileMenuOpen)
                return 0x8007000C; // DIERR_NOTACQUIRED
        }

        return res;
    }
}
