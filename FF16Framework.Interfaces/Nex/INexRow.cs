namespace FF16Framework.Interfaces.Nex;

public interface INexRow
{
    uint Key1 { get; }
    uint Key2 { get; }
    uint Key3 { get; }

    byte GetByte(uint columnOffset);
    sbyte GetSByte(uint columnOffset);
    short GetInt16(uint columnOffset);
    ushort GetUInt16(uint columnOffset);
    int GetInt32(uint columnOffset);
    uint GetUInt32(uint columnOffset);
    float GetSingle(uint columnOffset);
    string GetString(uint columnOffset, bool relative = false, int relativeOffset = 0);
    string[] GetStringArray(uint columnOffset, bool relative = false, int relativeOffset = 0);
    Span<byte> GetRowDataView(uint rowLength);
    Span<byte> GetByteArrayView(uint columnOffset);
    Span<int> GetIntArrayView(uint columnOffset);
    Span<float> GetSingleArrayView(uint columnOffset);

    byte[] GetRowDataCopy(uint rowLength);

    void SetByte(uint columnOffset, byte value);
    void SetSByte(uint columnOffset, sbyte value);
    void SetInt16(uint columnOffset, short value);
    void SetUInt16(uint columnOffset, ushort value);
    void SetInt32(uint columnOffset, int value);
    void SetUInt32(uint columnOffset, uint value);
    void SetSingle(uint columnOffset, float value);
}