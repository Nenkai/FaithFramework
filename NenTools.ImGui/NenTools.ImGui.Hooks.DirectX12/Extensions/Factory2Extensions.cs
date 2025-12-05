using SharpDX;
using SharpDX.DXGI;

using System;
using System.Collections.Generic;
using System.Text;

namespace NenTools.ImGui.Hooks.DirectX12.Extensions;

public static class Factory2Extensions
{
    public static unsafe Result CreateSwapChainForComposition(this Factory2 factory, IUnknown deviceRef, ref SwapChainDescription1 descRef, Output restrictToOutputRef, out SwapChain1 swapChainOut)
    {
        IntPtr pDeviceRef = CppObject.ToCallbackPtr<IUnknown>(deviceRef);
        IntPtr pRestrictToOutputRef = CppObject.ToCallbackPtr<Output>(restrictToOutputRef);
        IntPtr pSwapChain = IntPtr.Zero;
        fixed (SwapChainDescription1* pDescRef = &descRef)
        {
            Result result = ((delegate* unmanaged[Stdcall]<void*, void*, void*, void*, void*, int>)(*(IntPtr*)((nint)(*(IntPtr*)factory.NativePointer) + (nint)24 * (nint)sizeof(void*))))
                ((void*)factory.NativePointer, (void*)pDeviceRef, pDescRef, (void*)pRestrictToOutputRef, &pSwapChain);

            swapChainOut = new SwapChain1(pSwapChain);
            return result;
        }
    }

    public static unsafe Result CreateSwapChainForHwnd(this Factory2 factory, IUnknown deviceRef, IntPtr hWnd, ref SwapChainDescription1 descRef, SwapChainFullScreenDescription? fullscreenDescRef, Output restrictToOutputRef, out SwapChain1 swapChainOut)
    {
        IntPtr pDeviceRef = CppObject.ToCallbackPtr<IUnknown>(deviceRef);
        IntPtr pRestrictToOutputRef = CppObject.ToCallbackPtr<Output>(restrictToOutputRef);
        IntPtr pSwapChain = IntPtr.Zero;
        SwapChainFullScreenDescription value = default(SwapChainFullScreenDescription);
        if (fullscreenDescRef.HasValue)
            value = fullscreenDescRef.Value;

        fixed (SwapChainDescription1* pDescRef = &descRef)
        {
            SwapChainFullScreenDescription* pFullScreenDescRef = ((!fullscreenDescRef.HasValue) ? null : (&value));
            Result result = ((delegate* unmanaged[Stdcall]<void*, void*, void*, void*, void*, void*, void*, int>)(*(IntPtr*)((nint)(*(IntPtr*)factory.NativePointer) + (nint)15 * (nint)sizeof(void*))))
                ((void*)factory.NativePointer, (void*)pDeviceRef, (void*)hWnd, pDescRef, pFullScreenDescRef, (void*)pRestrictToOutputRef, &pSwapChain);

            swapChainOut = new SwapChain1(pSwapChain);
            return result;
        }
    }
}
