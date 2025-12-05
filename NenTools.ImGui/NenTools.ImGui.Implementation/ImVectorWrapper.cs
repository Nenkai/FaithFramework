using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Native;

namespace NenTools.ImGui.Implementation;

public readonly unsafe struct ImVectorWrapper<T>(int size, int capacity, nint data, int stride, Func<nint, T> wrapper) : IImVectorWrapper<T>
{
    /// <inheritdoc/>
    public readonly int Size { get; } = size;

    /// <inheritdoc/>
    public readonly int Capacity { get; } = capacity;

    /// <inheritdoc/>
    public readonly nint Data { get; } = data;

    /// <inheritdoc/>
    public readonly int Stride { get; } = stride;

    public int Count => Size;

    // Required due to interfacing.
    public readonly Func<nint, T> Wrapper { get; } = wrapper;

    /// <summary>
    /// Creates a <see cref="ImVectorWrapper{T}"/> from a regular vector, element size (stride), and a wrapper callback to wrap and get elements from
    /// </summary>
    public ImVectorWrapper(ImVector vector, int stride, Func<nint, T> wrapper)
        : this(vector.Size, vector.Capacity, vector.Data, stride, wrapper)
    {

    }

    /// <inheritdoc/>
    public T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Size, nameof(index));
            byte* address = (byte*)Data + index * Stride;
            return Wrapper((nint)address);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => new ImStructVectorWrapperEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    struct ImStructVectorWrapperEnumerator : IEnumerator<T>
    {
        private ImVectorWrapper<T> _vector;
        private int _currentIndex;

        public readonly T Current => _vector[_currentIndex];
        readonly object? IEnumerator.Current => _currentIndex < _vector.Size ? Current : throw new InvalidOperationException();

        public ImStructVectorWrapperEnumerator(ImVectorWrapper<T> vec)
        {
            _vector = vec;
            _currentIndex = -1;
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            if (_currentIndex + 1 >= _vector.Size)
                return false;

            _currentIndex++;
            return true;
        }

        public void Reset() => _currentIndex = -1;
    }
}
