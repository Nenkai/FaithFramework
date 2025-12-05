using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;

namespace NenTools.ImGui.Implementation;

public readonly unsafe struct RangeAccessor<T>(void* data, int count) : IRangeAccessor<T> where T : struct
{
    private static readonly int s_sizeOfT = Unsafe.SizeOf<T>();

    public readonly void* Data { get; } = data;
    public readonly int Count { get; } = count;

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

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => new RangeAccessorEnumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    struct RangeAccessorEnumerator : IEnumerator<T>
    {
        private RangeAccessor<T> _vector;
        private int _currentIndex;

        public readonly T Current => _vector[_currentIndex];
        readonly object? IEnumerator.Current => _currentIndex < _vector.Count ? Current : throw new InvalidOperationException();

        public RangeAccessorEnumerator(RangeAccessor<T> vec)
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
