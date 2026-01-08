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