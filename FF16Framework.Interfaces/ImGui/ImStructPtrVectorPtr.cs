using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.ImGui;

/// <summary>
/// Represents a pointer to a vector, containing struct pointers.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="data"></param>
public readonly unsafe struct ImStructPtrVectorPtr<T>(IntPtr data)
{
    public readonly nint NativePointer = data;

    public ImStructPtrVector<T> this[int index]
    {
        get
        {
            byte* vectorAddress = (byte*)*((nint*)NativePointer + (index * sizeof(nint)));
            return new ImStructPtrVector<T>(Unsafe.Read<ImVector>(vectorAddress));
        }
    }

    public ImStructPtrVector<T> GetVector(int index)
    {
        return new ImStructPtrVector<T>(Unsafe.Read<ImVector>((void*)(NativePointer + (index * sizeof(nint)))));
    }
}

public readonly unsafe struct ImStructPtrVectorPtrWrapper<T>(IntPtr data, Func<nint, T> wrapper)
{
    public readonly nint NativePointer = data;

    // Required due to interfacing.
    private readonly Func<nint, T> _wrapper = wrapper;

    public ImStructPtrVectorWrapper<T> this[int index]
    {
        get
        {
            byte* vectorAddress = (byte*)*((nint*)NativePointer + (index * sizeof(nint)));
            return new ImStructPtrVectorWrapper<T>(Unsafe.Read<ImVector>(vectorAddress), _wrapper);
        }
    }
}