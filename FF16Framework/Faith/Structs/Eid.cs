using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Faith.Structs;

public unsafe struct ActorEidDataEntry
{
    // ActorDataEntryBase?
    public nint vtable;
    public long ListEntryId;
    public ActorReference* ActorRef;

    // ActorEidDataEntry
    public ActorEidDataImpl Impl;
};

public unsafe struct ActorEidDataImpl
{
    public nint qword18;
    public FaithVector<EidMatrixInfoStruct> List;
    public fixed byte char40[456];
    public fixed byte BitSet[64];
    public int Flags;
    public nint qword250;
};

public struct EidMatrixInfoStruct
{
    public nint field_0;
    public uint EidId;
    public int field_C;
    public char field_10;
    public nint EidSetupData;
    public int field_20;
    public Vector3 VecUnk;
    public Matrix4x4 Mtx;
    public nint matA_;
    public nint field_78;
    public nint field_80;
    public nint field_88;
    public nint field_90;
    public nint field_98;
    public Vector3 field_A0;
    public nint field_B0;
    public nint field_B8;
    public nint field_C0;
    public nint field_C8;
    public nint field_D0;
    public nint field_D8;
    public nint field_E0;
    public nint field_E8;
};

