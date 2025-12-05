using NenTools.ImGui.Interfaces;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Implementation;

public unsafe struct RangeStructAccessor<T>(void* data, int count, int stride, Func<nint, T> wrapper) : IRangeStructAccessor<T>
{
    public readonly void* Data { get; } = data;
    public readonly int Count { get; } = count;
    public readonly int Stride { get; } = stride;

    // Required due to interfacing.
    private readonly Func<nint, T> _wrapper = wrapper;

    public RangeStructAccessor(IntPtr data, int count, int stride, Func<nint, T> wrapper) : this(data.ToPointer(), count, stride, wrapper) { }

    public readonly T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count, nameof(index));

            nint address = (nint)Data + (Stride * index);
            return _wrapper(address);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => new RangeStructAccessorEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    struct RangeStructAccessorEnumerator : IEnumerator<T>
    {
        private readonly RangeStructAccessor<T> _vector;
        private int _currentIndex;

        public readonly T Current => _vector[_currentIndex];
        readonly object? IEnumerator.Current => _currentIndex < _vector.Count ? Current : throw new InvalidOperationException();

        public RangeStructAccessorEnumerator(RangeStructAccessor<T> vec)
        {
            _vector = vec;
            _currentIndex = -1;
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            if (_currentIndex + 1 >= _vector.Count)
                return false;

            _currentIndex++;
            return true;
        }

        public void Reset() => _currentIndex = -1;
    }
}
