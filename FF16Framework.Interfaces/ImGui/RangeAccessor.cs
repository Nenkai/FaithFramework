using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.ImGui;

public readonly unsafe struct RangeAccessor<T>(void* data, int count) where T : struct
{
    private static readonly int s_sizeOfT = Unsafe.SizeOf<T>();

    public readonly void* Data = data;
    public readonly int Count = count;

    public RangeAccessor(IntPtr data, int count) : this(data.ToPointer(), count) { }

    public readonly ref T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count, nameof(index));

            return ref Unsafe.AsRef<T>((byte*)Data + s_sizeOfT * index);
        }
    }
}

/// <summary>
/// Used to wrap custom structs for interfacing.
/// </summary>
/// <typeparam name="T"></typeparam>
public unsafe struct RangeStructAccessor<T>(void* data, int count, int stride, Func<nint, T> wrapper)
{
    public readonly void* Data = data;
    public readonly int Count = count;
    private readonly int _stride = stride;

    // Required due to interfacing.
    private readonly Func<nint, T> _wrapper = wrapper;

    public RangeStructAccessor(IntPtr data, int count, int stride, Func<nint, T> wrapper) : this(data.ToPointer(), count, stride, wrapper) { }

    public readonly T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count, nameof(index));

            nint address = (nint)Data + (_stride * index);
            return _wrapper(address);
        }
    }
}
