using System;
using FF16Framework.Interfaces.Nex.Structures;

namespace FF16Framework.Interfaces.Nex;

/// <summary>
/// Nex row.
/// </summary>
public interface INexRow
{
    uint Key1 { get; }

    /// <summary>
    /// For double/triple-keyed tables.
    /// </summary>
    uint Key2 { get; }

    /// <summary>
    /// For triple-keyed tables.
    /// </summary>
    uint Key3 { get; }

    /// <summary>
    /// Gets a <see cref="byte"/> from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    byte GetByte(uint columnOffset);

    /// <summary>
    /// Gets a <see cref="sbyte"/> from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    sbyte GetSByte(uint columnOffset);

    /// <summary>
    /// Gets a <see cref="short"/> from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    short GetInt16(uint columnOffset);

    /// <summary>
    /// Gets a <see cref="ushort"/> from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    ushort GetUInt16(uint columnOffset);

    /// <summary>
    /// Gets a <see cref="int"/> from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    int GetInt32(uint columnOffset);

    /// <summary>
    /// Gets a <see cref="uint"/> from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    uint GetUInt32(uint columnOffset);

    /// <summary>
    /// Gets a <see cref="float"/> from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    float GetSingle(uint columnOffset);

    /// <summary>
    /// Gets a <see cref="NexUnionElement"/> from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    NexUnionElement GetUnion(uint columnOffset);

    /// <summary>
    /// Gets a <see cref="string"/> from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    string GetString(uint columnOffset, bool relative = false, int relativeOffset = 0);

    /// <summary>
    /// Gets a <see cref="string"/> array from the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    string[] GetStringArray(uint columnOffset, bool relative = false, int relativeOffset = 0);

    /// <summary>
    /// Gets a memory view of a row for direct manipulation.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    Span<byte> GetRowDataView(uint rowLength);

    /// <summary>
    /// Gets a memory view of a <see cref="byte"/> array for manipulation.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    Span<byte> GetByteArrayView(uint columnOffset);

    /// <summary>
    /// Gets a memory view of a <see cref="int"/> array for manipulation.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    Span<int> GetIntArrayView(uint columnOffset);

    /// <summary>
    /// Gets a memory view of a <see cref="float"/> array for manipulation.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    Span<float> GetSingleArrayView(uint columnOffset);

    /// <summary>
    /// Gets a memory view of a <see cref="NexUnionElement"/> array for manipulation.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <returns></returns>
    Span<NexUnionElement> GetUnionArrayView(uint columnOffset);

    /// <summary>
    /// Gets a copy of the row data based on specified row length. <br/>
    /// NOTE: Not suitable for fetching arrays or strings.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <param name="value"></param>
    byte[] GetRowDataCopy(uint rowLength);

    /// <summary>
    /// Sets a <see cref="byte"/> value at the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <param name="value"></param>
    void SetByte(uint columnOffset, byte value);

    /// <summary>
    /// Sets a <see cref="sbyte"/> value at the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <param name="value"></param>
    void SetSByte(uint columnOffset, sbyte value);

    /// <summary>
    /// Sets a <see cref="short"/> value at the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <param name="value"></param>
    void SetInt16(uint columnOffset, short value);

    /// <summary>
    /// Sets a <see cref="ushort"/> value at the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <param name="value"></param>
    void SetUInt16(uint columnOffset, ushort value);

    /// <summary>
    /// Sets a <see cref="int"/> value at the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <param name="value"></param>
    void SetInt32(uint columnOffset, int value);

    /// <summary>
    /// Sets a <see cref="uint"/> value at the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <param name="value"></param>
    void SetUInt32(uint columnOffset, uint value);

    /// <summary>
    /// Sets a <see cref="float"/> value at the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <param name="value"></param>
    void SetSingle(uint columnOffset, float value);

    /// <summary>
    /// Sets a <see cref="NexUnionElement"/> value at the specified column offset.
    /// </summary>
    /// <param name="columnOffset"></param>
    /// <param name="value"></param>
    void SetUnion(uint columnOffset, NexUnionElement value);
}