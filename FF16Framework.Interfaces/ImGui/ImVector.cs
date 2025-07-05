using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;

namespace FF16Framework.Interfaces.ImGui
{
    /// <summary>
    /// Imgui vector.
    /// </summary>
    public readonly unsafe struct ImVector(int size, int capacity, IntPtr data)
    {
        public readonly int Size = size;
        public readonly int Capacity = capacity;
        public readonly IntPtr Data = data;

        public ref T Ref<T>(int index)
        {
            return ref Unsafe.AsRef<T>((byte*)Data + index * Unsafe.SizeOf<T>());
        }

        public IntPtr Address<T>(int index)
        {
            return (IntPtr)((byte*)Data + index * Unsafe.SizeOf<T>());
        }
    }

    /// <summary>
    /// Generic imgui vector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly unsafe struct ImVector<T>
    {
        public readonly int Size;
        public readonly int Capacity;
        public readonly IntPtr Data;

        public ImVector(ImVector vector)
        {
            Size = vector.Size;
            Capacity = vector.Capacity;
            Data = vector.Data;
        }

        public ImVector(int size, int capacity, IntPtr data)
        {
            Size = size;
            Capacity = capacity;
            Data = data;
        }

        public readonly ref T this[int index] => ref Unsafe.AsRef<T>((byte*)Data + index * Unsafe.SizeOf<T>());
    }

    /// <summary>
    /// Represents a vector of struct pointers. Intended for native structs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly unsafe struct ImStructPtrVector<T>
    {
        public readonly int Size;
        public readonly int Capacity;
        public readonly IntPtr Data;

        public ImStructPtrVector(ImVector vector)
        {
            Size = vector.Size;
            Capacity = vector.Capacity;
            Data = vector.Data;
        }

        public ImStructPtrVector(int size, int capacity, IntPtr data)
        {
            Size = size;
            Capacity = capacity;
            Data = data;
        }

        public readonly ref T this[int index] => ref Unsafe.AsRef<T>((void*)*((nint*)Data + (index * sizeof(nint))));
    }

    /// <summary>
    /// Used to wrap ImVectors for vectors of structure pointers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly unsafe struct ImStructPtrVectorWrapper<T>(int size, int capacity, IntPtr data, Func<nint, T> wrapper)
    {
        public readonly int Size = size;
        public readonly int Capacity = capacity;
        public readonly IntPtr Data = data;

        // Required due to interfacing.
        private readonly Func<nint, T> _wrapper = wrapper;

        public ImStructPtrVectorWrapper(ImVector vector, Func<nint, T> wrapper)
            : this(vector.Size, vector.Capacity, vector.Data, wrapper)
        {

        }

        public T this[int index]
        {
            get
            {
                byte* address = (byte*)*((nint*)Data + (index * sizeof(nint)));
                return _wrapper((nint)address);
            }
        }
    }

    /// <summary>
    /// Used to wrap ImVectors for vectors of regular structures
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly unsafe struct ImStructVectorWrapper<T>(int size, int capacity, IntPtr data, int stride, Func<nint, T> wrapper)
    {
        public readonly int Size = size;
        public readonly int Capacity = capacity;
        public readonly IntPtr Data = data;
        private readonly int _stride = stride;

        // Required due to interfacing.
        private readonly Func<nint, T> _wrapper = wrapper;

        public ImStructVectorWrapper(ImVector vector, int stride, Func<nint, T> wrapper)
            : this(vector.Size, vector.Capacity, vector.Data, stride, wrapper)
        {
            
        }

        public T this[int index]
        {
            get
            {
                byte* address = (byte*)Data + index * _stride;
                return _wrapper((nint)address);
            }
        }
    }
}
