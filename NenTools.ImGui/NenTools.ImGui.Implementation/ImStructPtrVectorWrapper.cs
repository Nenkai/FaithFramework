using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Native;

namespace NenTools.ImGui.Implementation;

public readonly unsafe struct ImStructPtrVectorWrapper<T>(int size, int capacity, nint data, Func<nint, T> wrapper) : IImStructPtrVectorWrapper<T>
{
    /// <inheritdoc/>
    public readonly int Size { get; } = size;

    /// <inheritdoc/>
    public readonly int Capacity { get; } = capacity;

    /// <inheritdoc/>
    public readonly nint Data { get; } = data;

    public int Count => Size;

    // Required due to interfacing.
    private readonly Func<nint, T> _wrapper = wrapper;

    /// <summary>
    /// Creates a <see cref="ImStructPtrVectorWrapper{T}"/> from a regular vector, and a wrapper callback to wrap and get elements from
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="wrapper"></param>
    public ImStructPtrVectorWrapper(ImVector vector, Func<nint, T> wrapper)
        : this(vector.Size, vector.Capacity, vector.Data, wrapper)
    {

    }

    /// <summary>
    /// Returns an element by index from this vector.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public readonly T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Size, nameof(index));
            nint address = ((nint*)Data)[index];
            return _wrapper(address);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => new ImStructPtrVectorWrapperEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    struct ImStructPtrVectorWrapperEnumerator : IEnumerator<T>
    {
        private ImStructPtrVectorWrapper<T> _vector;
        private int _currentIndex;

        public readonly T Current => _vector[_currentIndex];
        readonly object? IEnumerator.Current => _currentIndex < _vector.Size ? Current : throw new InvalidOperationException();

        public ImStructPtrVectorWrapperEnumerator(ImStructPtrVectorWrapper<T> vec)
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