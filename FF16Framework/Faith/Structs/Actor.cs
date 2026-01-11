using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Faith.Structs;

// FFXVI Only.

public enum ActorBase
{
    Camera = 1,
    Layout = 2,
    BGParts = 3,
    Light = 4,
    CharaEditorAgent = 5,
    Magic = 6,
    Lantern = 7,
    LightShade = 8,
    Afterimage = 9,
    Wind = 10,
    DynamicBodyModel = 11,
    Fog = 12,
}

public unsafe struct Entity
{
    public nint ActorManager;
    public EntityBase EntityBase;
    public nint ActorBaseRow;
    public nint BNpcBaseRow;
    public nint ENpcBaseRow;
    public nint WeaponBaseRow;
    public nint GimmickBaseRow;
    public nint StageSetBaseRow;
    public nint NullActorBaseRow;
    public nint AnimalBaseRow;
    public nint PropBaseRow;
    public nint ModelRow;
    public ActorReference* ActorRef;
    public int ActorId;
    public char field_16C;
    public nint field_170;
};

public unsafe struct ActorManager
{
    public nint __vftable;
    public nint field_8;
    public nint ListManager;
    public nint MemAllocatorUnk;
    public nint field_20;
    public nint field_28;
    public void* field_30;
    public nint lpVtbl;
    public nint field_40;
    public fixed byte gap48[142];
    public fixed byte rtl_critical_sectionD6[0x28];
    public fixed byte gapFE[34];
    public fixed byte rtl_critical_section120[0x28];
    public fixed byte gap148[192];
    public fixed byte Map[0x48];
    public nint Field_250;
    public nint field_258;
    public nint Field_260;
    public nint Field_268;
    public int field_270;
    public int gap274;
    public int dword278;
    public int field_27C;
    public nint p_MainAllocator;
    public nint field_288;
    public nint field_290;
    public nint field_298;
    public int field_2A0;
    public int field_2A4;
    public nint MaxActorIndex;
    public ActorListBaseList Types;
    public fixed byte gap5B0[16];
    public nint field_5C0;
    public nint field_5C8;
    public nint field_5D0;
    public nint field_5D8;
    public nint field_5E0;
    public fixed byte gap5E8[192];
    public int NumEntities;
    public short word6AC;
    public fixed byte gap6AE[122];
    public fixed ulong UnkCounters[3];
    public nint ActorListsManager;
    public nint ActorListsManager2;
    public nint UnkLinkedList;
    public nint p_UnkLinkedList;
    public short word760;
    public fixed byte gap762[70];
    public short word7A8;
    public fixed byte gap7AA[6];
    public nint rtl_srwlock7B0;
    public fixed byte field_0x7B8[704];
    public short wordA78;

    [System.Runtime.CompilerServices.InlineArray(96)]
    public struct ActorListBaseList
    {
        private nint Element;
    }
};

public unsafe struct ActorListBase
{
    public ActorListBase_vt* vtable;
    public int Allocator_2;
    public int field_C;
    public fixed byte Allocator[0x38];
    public nint field_48;
    public nint g_off_7FF630620528;
    public nint field_58;
    public nint Allocator_1;
    public nint field_68;
    public nint field_70;
    public nint field_78;
    public nint field_80;
    public nint field_88;
    public nint field_90;
    public nint field_98;
    public nint field_A0;
    public nint field_A8;
    public nint field_B0;
    public nint field_B8;
    public nint field_C0;
    public nint field_C8;
    public nint field_D0;
    public nint field_D8;
    public nint field_E0;
    public nint field_E8;
    public nint field_F0;
    public nint field_F8;
    public nint field_100;
    public nint field_108;
    public nint field_110;
    public nint field_118;
    public nint field_120;
    public nint field_128;
    public nint field_130;
    public nint field_138;
    public nint field_140;
    public nint field_148;
    public nint field_150;
    public nint field_158;
    public nint field_160;
    public nint field_168;
    public nint field_170;
    public nint field_178;
    public nint field_180;
    public nint field_188;
    public nint field_200;
    public nint field_198;
    public nint field_210;
    public nint _field_218;

    public struct ActorListBase_vt
    {
        public nint field_0;
        public nint field_8;
        public nint field_10;
        public nint field_18;
        public nint field_20;
        public nint field_28;
        public nint field_30;
        public nint field_38;
        public nint field_40;
        public nint field_48;
        public nint field_50;
        public nint field_58;
        public nint field_60;
        public nint GetOrCreate;
        public nint field_70;
        public delegate* unmanaged[Cdecl]<ActorListBase*, uint, nint> GetByActorId;
    };
};


public unsafe struct EntityBase
{
    public uint EntityBaseId;
    public uint Map_LayoutInstanceId;
    public uint Field_0x08;
    public uint Field_0x0C;
    public void* Field_0x10;
    public void* Field_0x18;
    public void* Field_0x20;
    public void* Field_0x28;
    public void* Field_0x30;
    public void* Field_0x38;
    public Vector3 Field_0x40;
    public int Field_0x4C;
    public Vector3 Position;
    public int ParentLayoutNodeNamedInstance;
    public Vector3 Rotation;
    public float Dword6C;
    public int ThisEntityIndex;
    public int dword74;
    public byte UnkCounterIndex;
    public byte PartyMember_UnkMemberColumnValue;
    public byte byte7A;
    public byte byte7B;
    public int dword7C;
    public int dword80;
    public int dword84;
    public int dword88;
    public int dword8C;
    public int dword90;
    public int dword94;
    public int dword98;
    public int dword9C;
    public int dwordA0;
    public int dwordA4;
    public int dwordA8;
    public int dwordAC;
    public int dwordB0;
    public int dwordB4;
    public int dwordB8;
    public int dwordBC;
    public int dwordC0;
    public int dwordC4;
    public int WeaponBaseId;
    public double doubleCC;
    public int dwordD4;
    public int dwordD8;
    public int dwordDC;
    public int dwordE0;
    public int dwordE4;
    public int dwordE8;
    public int dwordEC;
    public int BitFlags;
    public int dwordF4;
    public nint qwordF8;
    public nint qword100;
}

public unsafe struct ActorReference
{
    public nint __vftable;
    public uint ActorId;
    public uint EntityID;
    public Node* Node;
    public Node* Node2;
    public nint field_20;
    public nint field_28;
    public nint field_30;
    public nint field_38;
    public int UnkCounterIndex;
    public int Flags;
    public nint HasTypeBitset;
    public nint HasTypeBitset2;
    public nint field_58;
    public nint ListEntryByListTypeAndActorId;
    public nint field_68;
    public nint g_off_7FF6A3500598;
    public nint field_78;
    public Vector3 UnkVec;
    public int field_8C;
}

public enum EntityType
{
    ActorBase = 0,
    BNpcBase = 1,
    ENpcBase = 2,
    WeaponBase = 3,
    GimmickBase = 4,
    StageSetBase = 5,
    NullActorBase = 7,
    AnimalBase = 8,
    PropBase = 9,
}