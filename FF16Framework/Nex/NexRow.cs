using FF16Framework.Interfaces.Nex;
using FF16Framework.Interfaces.Nex.Structures;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Nex;

public class NexRow : INexRow
{
    public uint Key1 { get; }
    public uint Key2 { get; }
    public uint Key3 { get; }

    public unsafe NexRow(uint key1, uint key2, uint key3, byte* rowDataPtr)
    {
        Key1 = key1;
        Key2 = key2;
        Key3 = key3;
        _rowDataPtr = rowDataPtr;
    }

    private readonly unsafe byte* _rowDataPtr;

    #region Reading
    public byte GetByte(uint columnOffset)
    {
        unsafe
        {
            return _rowDataPtr[columnOffset];
        }
    }

    public sbyte GetSByte(uint columnOffset)
    {
        unsafe
        {
            return ((sbyte*)_rowDataPtr)[columnOffset];
        }
    }

    public short GetInt16(uint columnOffset)
    {
        unsafe
        {
            return *(short*)(_rowDataPtr + columnOffset);
        }
    }

    public ushort GetUInt16(uint columnOffset)
    {
        unsafe
        {
            return *(ushort*)(_rowDataPtr + columnOffset);
        }
    }

    public int GetInt32(uint columnOffset)
    {
        unsafe
        {
            return *(int*)(_rowDataPtr + columnOffset);
        }
    }

    public uint GetUInt32(uint columnOffset)
    {
        unsafe
        {
            return *(uint*)(_rowDataPtr + columnOffset);
        }
    }

    public float GetSingle(uint columnOffset)
    {
        unsafe
        {
            return *(float*)(_rowDataPtr + columnOffset);
        }
    }

    public Span<byte> GetRowDataView(uint rowLength)
    {
        unsafe
        {
            return new Span<byte>(_rowDataPtr, (int)rowLength);
        }
    }

    public byte[] GetRowDataCopy(uint rowLength)
    {
        byte[] buffer = new byte[rowLength];

        unsafe
        {
            new Span<byte>(_rowDataPtr, (int)rowLength).CopyTo(buffer);
        }

        return buffer;
    }

    public Span<byte> GetByteArrayView(uint columnOffset)
    {
        unsafe
        {
            int arrayOffset = *(int*)(_rowDataPtr + columnOffset);
            int arrayLength = *(int*)(_rowDataPtr + columnOffset + 4);

            byte* arrayPtr = _rowDataPtr + arrayOffset;
            return new Span<byte>(arrayPtr, arrayLength);
        }
    }

    public Span<int> GetIntArrayView(uint columnOffset)
    {
        unsafe
        {
            int arrayOffset = *(int*)(_rowDataPtr + columnOffset);
            int arrayLength = *(int*)(_rowDataPtr + columnOffset + 4);

            byte* arrayPtr = _rowDataPtr + arrayOffset;
            return new Span<int>(arrayPtr, arrayLength);
        }
    }

    public Span<float> GetSingleArrayView(uint columnOffset)
    {
        unsafe
        {
            int arrayOffset = *(int*)(_rowDataPtr + columnOffset);
            int arrayLength = *(int*)(_rowDataPtr + columnOffset + 4);

            byte* arrayPtr = _rowDataPtr + arrayOffset;
            return new Span<float>(arrayPtr, arrayLength);
        }
    }

    public string GetString(uint columnOffset, bool relative = false, int relativeOffset = 0)
    {
        unsafe
        {
            int strOffset = *(int*)(_rowDataPtr + columnOffset);

            byte* strPtr;
            if (relative)
                strPtr = _rowDataPtr + columnOffset + strOffset + relativeOffset;
            else
                strPtr = _rowDataPtr + strOffset;

            return Marshal.PtrToStringUTF8((nint)strPtr);
        }
    }

    public string[] GetStringArray(uint columnOffset, bool relative = false, int relativeOffset = 0)
    {
        unsafe
        {
            int arrayOffset = *(int*)(_rowDataPtr + columnOffset);
            int arrayLength = *(int*)(_rowDataPtr + columnOffset + 4);

            string[] array = new string[arrayLength];

            byte* strPtr;
            if (relative)
                strPtr = _rowDataPtr + columnOffset + arrayOffset + relativeOffset;
            else
                strPtr = _rowDataPtr + arrayOffset;

            for (int i = 0; i < arrayLength; i++)
            {
                int strOffset = ((int*)strPtr)[i];
                array[i] = Marshal.PtrToStringUTF8((nint)(strPtr + strOffset));
            }

            return array;
        }
    }
    #endregion

    #region Writing
    public void SetByte(uint columnOffset, byte value)
    {
        unsafe
        {
            _rowDataPtr[columnOffset] = value;
        }
    }

    public void SetSByte(uint columnOffset, sbyte value)
    {
        unsafe
        {
            ((sbyte*)_rowDataPtr)[columnOffset] = value;
        }
    }

    public void SetInt16(uint columnOffset, short value)
    {
        unsafe
        {
            *(short*)(_rowDataPtr + columnOffset) = value;
        }
    }

    public void SetUInt16(uint columnOffset, ushort value)
    {
        unsafe
        {
            *(ushort*)(_rowDataPtr + columnOffset) = value;
        }
    }

    public void SetInt32(uint columnOffset, int value)
    {
        unsafe
        {
            *(int*)(_rowDataPtr + columnOffset) = value;
        }
    }

    public void SetUInt32(uint columnOffset, uint value)
    {
        unsafe
        {
            *(uint*)(_rowDataPtr + columnOffset) = value;
        }
    }

    public void SetSingle(uint columnOffset, float value)
    {
        unsafe
        {
            *(float*)(_rowDataPtr + columnOffset) = value;
        }
    }
    #endregion

    private struct NexArray
    {
        public int Length;
    }
}
