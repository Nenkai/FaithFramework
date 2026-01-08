using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Faith.Structs;

using NenTools.ImGui.Interfaces.Shell;

using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

namespace FF16Framework.Faith.Hooks;

public unsafe class EntityManagerHooks : HookGroupBase
{
    private readonly IImGuiShell _imGuiShell;
    private readonly FrameworkConfig _frameworkConfig;

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

    public EntityManagerHooks(Config config, IModConfig modConfig, ILogger logger, IImGuiShell imGuiShell, FrameworkConfig frameworkConfig)
    : base(config, modConfig, logger)
    {
        _imGuiShell = imGuiShell;
        _frameworkConfig = frameworkConfig;
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

        if (_frameworkConfig.EntityManager.PrintEntityLoads)
        {
            EntityType type = (EntityType)(res->EntityID >> 24);
            uint id = res->EntityID & 0xFFFFFF;
            if (type == EntityType.ActorBase) // ActorBase:BGParts
            {
                if (id == 3)
                    _imGuiShell.LogWriteLine(nameof(EntityManagerHooks), $"Created {(ActorBase)id}:{entity->EntityBase.Map_LayoutInstanceId} (actor id: {res->ActorId:X} @ {entity->EntityBase.Position})");
                else
                    _imGuiShell.LogWriteLine(nameof(EntityManagerHooks), $"Created {(ActorBase)id} (actor id: {res->ActorId:X} @ {entity->EntityBase.Position})");
            }
            else
                _imGuiShell.LogWriteLine(nameof(EntityManagerHooks), $"Created entity {type}:{id} (actor id: {res->ActorId:X} @ {entity->EntityBase.Position})");
        }
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