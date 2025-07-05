using System;
using System.Runtime.InteropServices;

using FF16Framework.ImGui.Hooks;
using FF16Framework.ImGui.Hooks.DirectX;
using FF16Framework.ImGui.Hooks.Misc;
using FF16Framework.Native;

using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Internal;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Hooks.Definitions.X64;


using CallingConventions = Reloaded.Hooks.Definitions.X86.CallingConventions;

namespace FF16Framework.ImGui.Hooks
{
    /// <summary>
    /// Hooks the <see cref="WndProc"/> function of a given window.
    /// </summary>
    public class WndProcHook
    {
        public static WndProcHook Instance { get; private set; }

        /// <summary>
        /// The function that gets called when hooked.
        /// </summary>
        public WndProc HookFunction { get; private set; }

        /// <summary>
        /// Window handle of hooked window.
        /// </summary>
        public nint WindowHandle { get; private set; }

        /// <summary>
        /// The hook created for the WndProc function.
        /// Can be used to call the original WndProc.
        /// </summary>
        public IHook<WndProc> Hook { get; private set; }

        private WndProcHook(nint hWnd, WndProc wndProcHandler)
        {
            WindowHandle = hWnd;
            var windowProc = NativeMethods.GetWindowLong(hWnd, Native.GWL.GWL_WNDPROC);
            Debug.WriteLine($"[WndProcHook] WindowProc: {(long)windowProc:X}");
            SetupHook(wndProcHandler, windowProc);
        }

        /// <summary>
        /// Creates a hook for the WindowProc function.
        /// </summary>
        /// <param name="hWnd">Handle of the window to hook.</param>
        /// <param name="wndProcHandler">Handles the WndProc function.</param>
        public static WndProcHook Create(nint hWnd, WndProc wndProcHandler) => Instance ??= new WndProcHook(hWnd, wndProcHandler);

        /// <summary>
        /// Initializes the hook class.
        /// </summary>
        private void SetupHook(WndProc proc, nint address)
        {
            HookFunction = proc;
            Hook = SDK.Hooks.CreateHook(HookFunction, address).Activate();
        }

        public void Disable() => Hook.Disable();
        public void Enable() => Hook.Enable();

        [Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
        [Reloaded.Hooks.Definitions.X86.Function(CallingConventions.Stdcall)]
        public struct WndProc { public FuncPtr<nint, uint, nint, nint, nint> Value; }
    }
}