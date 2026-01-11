using NenTools.ImGui.Native;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Faith.Structs;

public unsafe struct FaithVector<T> : IEnumerable<T> where T : struct
{
    private static readonly int s_sizeOfT = Unsafe.SizeOf<T>();

    public nint Allocator;

    /// <summary>
    /// Pointer to the first element/data for this vector.
    /// </summary>
    public nint Begin;

    /// <summary>
    /// Size/Length of the vector.
    /// </summary>
    public nint End;

    /// <summary>
    /// Capacity for this vector.
    /// </summary>
    public nint Capacity;

    /// <summary>
    /// Creates a <see cref="ImVector"/> from another one (does not allocate anything, only copies the size, capacity and data pointer)
    /// </summary>
    /// <param name="vector"></param>
    public FaithVector(ImVector vector)
    {
        End = vector.Size;
        Capacity = vector.Capacity;
        Begin = vector.Data;
    }

    /// <summary>
    /// Creates a <see cref="ImVector"/> from size/capacity/data pointer pair (does not allocate anything, only sets the size, capacity and data pointer)
    /// </summary>
    public FaithVector(int size, int capacity, nint data)
    {
        End = size;
        Capacity = capacity;
        Begin = data;
    }

    /// <summary>
    /// Returns an element by index from this vector.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ref T this[int index] => ref Unsafe.AsRef<T>((byte*)Begin + Check(index) * s_sizeOfT);

    public readonly int Size => (int)((End - Begin) / s_sizeOfT);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int Check(int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, End);
        return index;
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => new ImVectorEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    struct ImVectorEnumerator : IEnumerator<T>
    {
        private FaithVector<T> _vector;
        private int _currentIndex;

        public T Current => _vector[_currentIndex];
        object? IEnumerator.Current => _currentIndex < _vector.End ? Current : throw new InvalidOperationException();

        public ImVectorEnumerator(FaithVector<T> vec)
        {
            _vector = vec;
            _currentIndex = -1;
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            if (_currentIndex + 1 >= _vector.End)
                return false;

            _currentIndex++;
            return true;
        }

        public void Reset() => _currentIndex = -1;
    }
}
