using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Helpers;

using SharpDX;
using SharpDX.DXGI;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGui.Hooks.DirectX12;

[Guid("c1b6694f-ff09-44a9-b03c-77900a0a1d17")]
public class Factory6 : Factory5
{
    public Factory6(IntPtr nativePtr)
    : base(nativePtr)
    {
    }

    public Factory6() : this(IntPtr.Zero)
    {
        CreateDXGIFactory2(Utilities.GetGuidFromType(GetType()), out var factoryOut);
        base.NativePointer = factoryOut;
    }

    public unsafe static void CreateDXGIFactory2(Guid riid, out IntPtr factoryOut)
    {
        Result result;
        fixed (IntPtr* ptr = &factoryOut)
        {
            void* param = ptr;
            result = CreateDXGIFactory2_(0, &riid, param);
        }

        result.CheckError();
    }

    [DllImport("dxgi.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "CreateDXGIFactory2")]
    private unsafe static extern int CreateDXGIFactory2_(uint flags, void* param0, void* param1);
}

