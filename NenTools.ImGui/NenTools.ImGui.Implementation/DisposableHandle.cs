using NenTools.ImGui.Implementation;
using NenTools.ImGui.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Implementation;

public class DisposableHandle<T> : IDisposableHandle<T> where T : INativeStruct
{
    public T Value { get; }

    public DisposableHandle(T value, int structSize)
    {
        Value = value;
        value.NativePointer = Marshal.AllocHGlobal(structSize);
        unsafe
        {
            Unsafe.InitBlockUnaligned((void*)value.NativePointer, 0, (uint)structSize);
        }
    }

    public void Dispose()
    {
        if (Value?.NativePointer != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(Value.NativePointer);
            Value.NativePointer = IntPtr.Zero;
        }
    }
}