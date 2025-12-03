// Structures here are very unlikely to change, so this is fine to stay in the Interfaces namespace. -Nen
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.ImGui
{
    /// <summary>
    /// Imgui vector.
    /// </summary>
    public unsafe struct ImVector(int size, int capacity, nint data)
    {
        /// <summary>
        /// Size/Length of the vector.
        /// </summary>
        public int Size = size;

        /// <summary>
        /// Capacity for this vector.
        /// </summary>
        public int Capacity = capacity;

        /// <summary>
        /// Pointer to the first element/data for this vector.
        /// </summary>
        public nint Data = data;

        /// <summary>
        /// Returns a reference to the element at the specified index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ref T Ref<T>(int index)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Size, nameof(index));
            return ref Unsafe.AsRef<T>((byte*)Data + index * Unsafe.SizeOf<T>());
        }

        /// <summary>
        /// Returns the address of the specified element index in the vector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly nint Address<T>(int index)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Size, nameof(index));
            return (nint)((byte*)Data + index * Unsafe.SizeOf<T>());
        }
    }

    /// <summary>
    /// Generic imgui vector.<br/>
    /// Supports <see cref="IEnumerable"/> and indexing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public unsafe struct ImVector<T> : IEnumerable<T> where T : struct
    {
        private static readonly int s_sizeOfT = Unsafe.SizeOf<T>();

        /// <summary>
        /// Size/Length of the vector.
        /// </summary>
        public int Size;

        /// <summary>
        /// Capacity for this vector.
        /// </summary>
        public int Capacity;

        /// <summary>
        /// Pointer to the first element/data for this vector.
        /// </summary>
        public nint Data;

        /// <summary>
        /// Creates a <see cref="ImVector"/> from another one (does not allocate anything, only copies the size, capacity and data pointer)
        /// </summary>
        /// <param name="vector"></param>
        public ImVector(ImVector vector)
        {
            Size = vector.Size;
            Capacity = vector.Capacity;
            Data = vector.Data;
        }

        /// <summary>
        /// Creates a <see cref="ImVector"/> from size/capacity/data pointer pair (does not allocate anything, only sets the size, capacity and data pointer)
        /// </summary>
        public ImVector(int size, int capacity, nint data)
        {
            Size = size;
            Capacity = capacity;
            Data = data;
        }

        /// <summary>
        /// Returns an element by index from this vector.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ref T this[int index] => ref Unsafe.AsRef<T>((byte*)Data + Check(index) * s_sizeOfT);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Check(int index)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Size);
            return index;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => new ImVectorEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        struct ImVectorEnumerator : IEnumerator<T>
        {
            private ImVector<T> _vector;
            private int _currentIndex;

            public T Current => _vector[_currentIndex];
            object? IEnumerator.Current => _currentIndex < _vector.Size ? Current : throw new InvalidOperationException();

            public ImVectorEnumerator(ImVector<T> vec)
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

    /// <summary>
    /// Represents a vector of struct pointers (<b>ImVector&lt;T*&gt;</b>). Intended for native structs<br/>
    /// Supports indexing.<br/>
    /// <br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public unsafe struct ImStructPtrVector<T>
    {
        /// <summary>
        /// Size/Length of the vector.
        /// </summary>
        public int Size;

        /// <summary>
        /// Capacity for this vector.
        /// </summary>
        public int Capacity;

        /// <summary>
        /// Pointer to the first element/data for this vector.
        /// </summary>
        public nint Data;

        /// <summary>
        /// Creates a <see cref="ImStructPtrVector{T}"/> from a regular vector (does not allocate anything, only copies the size, capacity and data pointer)
        /// </summary>
        /// <param name="vector"></param>
        public ImStructPtrVector(ImVector vector)
        {
            Size = vector.Size;
            Capacity = vector.Capacity;
            Data = vector.Data;
        }

        /// <summary>
        /// Creates a <see cref="ImStructPtrVector{T}"/> from size/capacity/data pointer pair (does not allocate anything, only sets the size, capacity and data pointer)
        /// </summary>
        public ImStructPtrVector(int size, int capacity, nint data)
        {
            Size = size;
            Capacity = capacity;
            Data = data;
        }

        /// <summary>
        /// Returns an element by index from this vector.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly ref T this[int index] => ref Unsafe.AsRef<T>((void*)((nint*)Data)[Check(index)]);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly int Check(int index)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Size);
            return index;
        }
    }

    /// <summary>
    /// Used to wrap ImVectors for vectors of structure pointers<br/>
    /// Supports <see cref="IEnumerable"/> and indexing.<br/>
    /// <br/>
    /// Essentially this is a wrapper around <b>ImVector&lt;T*&gt;</b>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly unsafe struct ImStructPtrVectorWrapper<T>(int size, int capacity, nint data, Func<nint, T> wrapper) : IEnumerable<T>
    {
        /// <summary>
        /// Size/Length of the vector.
        /// </summary>
        public readonly int Size = size;

        /// <summary>
        /// Capacity for this vector.
        /// </summary>
        public readonly int Capacity = capacity;

        /// <summary>
        /// Pointer to the first element/data for this vector.
        /// </summary>
        public readonly nint Data = data;

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

    /// <summary>
    /// Used to wrap ImVectors for vectors of regular structures<br/>
    /// Supports <see cref="IEnumerable"/> and indexing.<br/>
    /// <br/>
    /// Essentially this is a wrapper around <b>ImVector&lt;T&gt;</b>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly unsafe struct ImStructVectorWrapper<T>(int size, int capacity, nint data, int stride, Func<nint, T> wrapper) : IEnumerable<T>
    {
        /// <summary>
        /// Size/Length of the vector.
        /// </summary>
        public readonly int Size = size;

        /// <summary>
        /// Capacity for this vector.
        /// </summary>
        public readonly int Capacity = capacity;

        /// <summary>
        /// Pointer to the first element/data for this vector.
        /// </summary>
        public readonly nint Data = data;

        /// <summary>
        /// Size of one element/struct, used to seek between elements in the vector.
        /// </summary>
        public readonly int Stride = stride;

        // Required due to interfacing.
        private readonly Func<nint, T> _wrapper = wrapper;

        /// <summary>
        /// Creates a <see cref="ImStructVectorWrapper{T}"/> from a regular vector, element size (stride), and a wrapper callback to wrap and get elements from
        /// </summary>
        public ImStructVectorWrapper(ImVector vector, int stride, Func<nint, T> wrapper)
            : this(vector.Size, vector.Capacity, vector.Data, stride, wrapper)
        {
            
        }

        /// <summary>
        /// Returns an element by index from this vector.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Size, nameof(index));
                byte* address = (byte*)Data + index * Stride;
                return _wrapper((nint)address);
            }
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => new ImStructVectorWrapperEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        struct ImStructVectorWrapperEnumerator : IEnumerator<T>
        {
            private ImStructVectorWrapper<T> _vector;
            private int _currentIndex;

            public readonly T Current => _vector[_currentIndex];
            readonly object? IEnumerator.Current => _currentIndex < _vector.Size ? Current : throw new InvalidOperationException();

            public ImStructVectorWrapperEnumerator(ImStructVectorWrapper<T> vec)
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
}
