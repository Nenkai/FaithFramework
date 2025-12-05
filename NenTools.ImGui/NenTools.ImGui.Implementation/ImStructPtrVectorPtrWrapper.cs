using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Native;
using NenTools.ImGui.Interfaces;
using System.Collections;

namespace NenTools.ImGui.Implementation;

public readonly unsafe struct ImStructPtrVectorPtrWrapper<T>(IntPtr data, Func<nint, T> wrapper) : IImStructPtrVectorPtrWrapper<T>
{
    public nint NativePointer { get; } = data;

    // Required due to interfacing.
    private readonly Func<nint, T> _wrapper = wrapper;

    public IImStructPtrVectorWrapper<T> this[int index]
    {
        get
        {
            byte* vectorAddress = (byte*)((nint*)NativePointer)[index];
            return new ImStructPtrVectorWrapper<T>(Unsafe.Read<ImVector>(vectorAddress), _wrapper);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
