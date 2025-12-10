using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public unsafe struct NexTableInstance
{
    public nint field_0x00;
    public uint TableId;
    public uint State;
    public NexRowInstance* RowInfos; // ulong, pointer
    public nint field_0x18;
    public nint field_0x20;
    public nint NumRows;
    public FileHandle* FileHandle;
    public uint Type;
    public byte field_0x40;
    public byte Unk_NxdField0x0A;
    public uint BaseRowId;
    public uint field_0x48;
}

public unsafe struct FileHandle
{
    public nint field_0x00;
    public nint field_0x08;
    public nint field_0x10;
    public nint field_0x18;
    public nint field_0x20;
    public nint field_0x28;
    public nint field_0x30;
    public byte* Buffer;
}
#pragma warning restore CS1591
