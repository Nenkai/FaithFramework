using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Faith.Structs;

using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

namespace FF16Framework.Faith.Hooks;

public unsafe class EntityManagerHooks : HookGroupBase
{
    //public delegate ActorReference* ActorManager_CreateNewActor(nint listsGlobal, EntityBase* entityBase);
    //private IHook<ActorManager_CreateNewActor> ActorManager_CreateNewActorHook;

    public delegate ActorReference* ActorManager_SetupEntity(nint @this, Entity* entity);
    public IHook<ActorManager_SetupEntity> ActorManager_SetupEntityHook { get; private set; }

    public delegate nint StaticActorManager_GetOrCreate(nint @this, nint** outEntityInfo, uint entityId);
    public IHook<StaticActorManager_GetOrCreate> StaticActorManager_GetOrCreateHook { get; private set; }

    public ActorManager_GetActorByKey ActorManager_GetActorByKeyFunction { get; private set; }
    public StaticActorInfo_IsValidActor HasEntityDataFunction { get; private set; }
    public StaticActorInfo_GetPosition GetPositionFunction { get; private set; }
    public StaticActorInfo_GetRotation GetRotationFunction { get; private set; }
    public StaticActorInfo_GetForwardVector GetForwardVectorFunction { get; private set; }
    public StaticActorInfo_GetForwardXZ GetForwardXZFunction { get; private set; }

    public delegate ActorReference* ActorManager_GetActorByKey(nint @this, uint actorId);
    public delegate nint StaticActorInfo_IsValidActor(nint pStaticEntityInfo);
    public delegate NodePositionPair* StaticActorInfo_GetPosition(nint pStaticEntityInfo, NodePositionPair* outPair);
    public delegate Vector3* StaticActorInfo_GetRotation(nint pStaticEntityInfo, Vector3* outPair);
    public delegate Vector3* StaticActorInfo_GetForwardVector(nint pStaticEntityInfo, Vector3* outPair);
    public delegate Vector3* StaticActorInfo_GetForwardXZ(nint pStaticEntityInfo, Vector3* outPair);

    public delegate nint UnkSingletonPlayerOrCameraRelated_Ctor(nint @this);
    public IHook<UnkSingletonPlayerOrCameraRelated_Ctor> UnkSingletonPlayerOrCameraRelated_CtorHook;

    public nint ActorManager { get; private set; }
    public nint StaticActorManager { get; private set; }
    public nint UnkSingletonPlayerOrCameraRelated { get; private set; }

    public EntityManagerHooks(Config config, IModConfig modConfig, ILogger logger)
    : base(config, modConfig, logger)
    {
    }

    public override void SetupHooks()
    {
        //Project.Scans.AddScanHook(nameof(ActorManager_CreateNewActor), (result, hooks)
        //    => ActorManager_CreateNewActorHook = hooks.CreateHook<ActorManager_CreateNewActor>(ActorManager_CreateNewActorImpl, result).Activate());
        Project.Scans.AddScanHook(nameof(ActorManager_SetupEntity), (result, hooks)
            => ActorManager_SetupEntityHook = hooks.CreateHook<ActorManager_SetupEntity>(ActorManager_SetupEntityImpl, result).Activate());
        Project.Scans.AddScanHook(nameof(StaticActorManager_GetOrCreate), (result, hooks) 
            => StaticActorManager_GetOrCreateHook = hooks.CreateHook<StaticActorManager_GetOrCreate>(StaticActorManager_GetOrCreateImpl, result).Activate());
        Project.Scans.AddScanHook(nameof(UnkSingletonPlayerOrCameraRelated_Ctor), (result, hooks)
            => UnkSingletonPlayerOrCameraRelated_CtorHook = hooks.CreateHook<UnkSingletonPlayerOrCameraRelated_Ctor>(UnkSingletonPlayerOrCameraRelated_CtorImpl, result).Activate());

        Project.Scans.AddScanHook(nameof(ActorManager_GetActorByKey), (result, hooks) => ActorManager_GetActorByKeyFunction = hooks.CreateWrapper<ActorManager_GetActorByKey>(result, out _));
        Project.Scans.AddScanHook(nameof(StaticActorInfo_IsValidActor), (result, hooks) => HasEntityDataFunction = hooks.CreateWrapper<StaticActorInfo_IsValidActor>(result, out _));
        Project.Scans.AddScanHook(nameof(StaticActorInfo_GetPosition), (result, hooks) => GetPositionFunction = hooks.CreateWrapper<StaticActorInfo_GetPosition>(result, out _));
        Project.Scans.AddScanHook(nameof(StaticActorInfo_GetRotation), (result, hooks) => GetRotationFunction = hooks.CreateWrapper<StaticActorInfo_GetRotation>(result, out _));
        Project.Scans.AddScanHook(nameof(StaticActorInfo_GetForwardVector), (result, hooks) => GetForwardVectorFunction = hooks.CreateWrapper<StaticActorInfo_GetForwardVector>(result, out _));
        Project.Scans.AddScanHook(nameof(StaticActorInfo_GetForwardXZ), (result, hooks) => GetForwardXZFunction = hooks.CreateWrapper<StaticActorInfo_GetForwardXZ>(result, out _));
    }

    /*
    private ActorReference* ActorManager_CreateNewActorImpl(nint listsGlobal, EntityBase* entityBase)
    {
        var res = ActorManager_CreateNewActorHook.OriginalFunction(listsGlobal, entityBase);
        _logger.WriteLine($"Created {res->EntityID:X} with Id {res->ActorId:X}");
        return res;
    }
    */

    private ActorReference* ActorManager_SetupEntityImpl(nint @this, Entity* entity)
    {
        ActorManager = @this;
        var res = ActorManager_SetupEntityHook.OriginalFunction(@this, entity);
       // _logger.WriteLine($"Created entity {res->EntityID:X} (actor id: {res->ActorId:X})");
        return res;
    }

    private nint UnkSingletonPlayerOrCameraRelated_CtorImpl(nint @this)
    {
        UnkSingletonPlayerOrCameraRelated = @this;
        return UnkSingletonPlayerOrCameraRelated_CtorHook.OriginalFunction(@this);
    }

    private nint StaticActorManager_GetOrCreateImpl(nint @this, nint** key, uint entityId)
    {
        StaticActorManager = @this;
        return StaticActorManager_GetOrCreateHook.OriginalFunction(@this, key, entityId);
    }
}

public unsafe struct NodePositionPair // sizeof=0x20
{
    public nint vtable;
    public Node* ParentNode;
    public Vector3 Position;
    public int dword1C;
};

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