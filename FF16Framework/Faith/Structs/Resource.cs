using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Faith.Structs;

public unsafe struct ResourceManager
{
    public nint vtable;
    public nint qword8;
    public ResourceFactoryLinkedList ResourceFactoryList; // All factories for each extension
    public nint qword20;
    public nint qword28;
    public nint qword30;
    public ResourceBucketManager BucketList1;
    public nint field_4040;
    public ResourceBucketManagerArray BucketLists2;
    public int field_10060;
    public int field_10064;
    public ResourceBucketManager BucketList3;
}

public unsafe struct ResourceBucketManager
{
    public const int BUCKET_SIZE = 512;

    public fixed ulong SRWLockShareds[BUCKET_SIZE]; // RTL_SRWLOCK
    public ResourceBucketArray Buckets;
    public nint Unk;
}

public unsafe struct ResourceFactoryLinkedList
{
    public ResourceFactoryHandlerPair** Entries;
    public nint Count;
};

public unsafe struct ResourceFactoryHandlerPair
{
    public ResourceFactoryHandlerPair* Next;
    public ResourceFactoryHandlerPair* Previous;
    public ResourceFactory Factory;
    public nint Extension;
};

public struct ResourceFactory
{
    public nint CreateWrapCb;
    public nint CreateCb;
    public nint field_10;
    public nint field_18;
};

[InlineArray(3)]
public struct ResourceBucketManagerArray
{
    public ResourceBucketManager Element;
}

[InlineArray(ResourceBucketManager.BUCKET_SIZE)]
public struct ResourceBucketArray
{
    public ResourceBucket Element;
}

public struct ResourceBucket
{
    public nint field_0x00;
    public nint field_0x08;
    public nint field_0x10;
}

public unsafe struct ResourceHandleStruct
{
    // faith::ReferencedObject
    public nint vtable;
    public uint Field_0x08;
    public uint RefCount;
    public nint Field_0x10;

    // faith::Resource::ResourceHandle
    public byte* FileName;
    public nint Field_0x20;
    public nint Allocator;
    public nint FileReader;
    public nint FileBuffer;
    public uint FileSize;
    public int Field_0x44;
    public uint PathHash;
    public uint Field_0x4C;
    public int OpenState;
    public int FormatLoadState;
    public int Field_0x58;
    public int Flags;

    public readonly uint GetBucket() => PathHash % ResourceBucketManager.BUCKET_SIZE; // & 0x1FF
}
