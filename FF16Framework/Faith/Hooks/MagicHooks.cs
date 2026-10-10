using System;
using System.Collections;
using System.Collections.Concurrent.Extended;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

using FF16Framework.Faith.Structs;
using FF16Framework.Services.ResourceManager;
using NenTools.ImGui.Interfaces.Shell;

namespace FF16Framework.Faith.Hooks;

public unsafe class MagicHooks : HookGroupBase
{
    private readonly FrameworkConfig _frameworkConfig;
    private readonly IImGuiShell _shell;
    private readonly EntityManagerHooks _entityManager;

    public delegate nint BattleMagicExecutor_InsertMagic(nint @this, BattleMagic* magic);
    
    // ============================================================
    // DELEGATES - Magic Casting Engine
    // ============================================================
    
    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate long Magic_Setup(long battleMagicPtr, int magicId, long casterActorRef, long positionStruct, int commandId, int actionID, byte flag);
    
    // Note: BattleMagicExecutor_InsertMagic delegate is used for both hook and wrapper
    
    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate long* TargetStruct_Create(long manager, long* outResult);
    
    // ============================================================
    // DELEGATES - Magic File Processing
    // ============================================================
    
    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate void MagicFile_UnkExecute(long magicFileInstance, int opType, int propertyId, long dataPtr);
    
    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate long MagicFile_Process(long a1, long a2, long a3, long a4);
    
    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate long MagicFile_HandleSubEntry(long a1, long a2, long a3, long a4);
    
    public IHook<BattleMagicExecutor_InsertMagic> BattleMagicExecutor_InsertMagicHook { get; private set; }
    public IHook<Magic_Setup>? Magic_SetupHook { get; private set; }
    public IHook<TargetStruct_Create>? TargetStruct_CreateHook { get; private set; }
    
    // ============================================================
    // HOOKS - Magic File Processing
    // ============================================================
    
    public IHook<MagicFile_UnkExecute>? MagicFile_UnkExecuteHook { get; private set; }
    public IHook<MagicFile_Process>? MagicFile_ProcessHook { get; private set; }
    public IHook<MagicFile_HandleSubEntry>? MagicFile_HandleSubEntryHook { get; private set; }
    
    // ============================================================
    // CALLBACK DELEGATES - Custom delegates for pointer types
    // ============================================================
    
    public delegate long MagicSetupCallback(long battleMagicPtr, int magicId, long casterActorRef, long positionStruct, int commandId, int actionID, byte flag);
    public unsafe delegate long* TargetStructCreateCallback(long manager, long* outResult);
    public delegate void MagicFileUnkExecuteCallback(long magicFileInstance, int opType, int propertyId, long dataPtr);
    public delegate long MagicFileProcessCallback(long a1, long a2, long a3, long a4);
    public delegate long MagicFileHandleSubEntryCallback(long a1, long a2, long a3, long a4);
    
    // ============================================================
    // CALLBACKS - For services to register their implementations
    // ============================================================
    
    public MagicSetupCallback? OnMagicSetup { get; set; }
    public TargetStructCreateCallback? OnTargetStructCreate { get; set; }
    public MagicFileUnkExecuteCallback? OnMagicFileUnkExecute { get; set; }
    public MagicFileProcessCallback? OnMagicFileProcess { get; set; }
    public MagicFileHandleSubEntryCallback? OnMagicFileHandleSubEntry { get; set; }

    public MagicHooks(Config config, IModConfig modConfig, ILogger logger, 
        FrameworkConfig frameworkConfig, IImGuiShell imGuiShell, EntityManagerHooks entityManager)
        : base(config, modConfig, logger)
    {
        _frameworkConfig = frameworkConfig;
        _shell = imGuiShell;
        _entityManager = entityManager;
    }

    public override void SetupHooks()
    {
        Project.Scans.AddScanHook(nameof(BattleMagicExecutor_InsertMagic), (result, hooks)
            => BattleMagicExecutor_InsertMagicHook = hooks.CreateHook<BattleMagicExecutor_InsertMagic>(BattleMagicExecutor_InsertMagicImpl, result).Activate());
        
        // Magic Casting Engine hooks
        Project.Scans.AddScanHook(nameof(Magic_Setup), (result, hooks)
            => Magic_SetupHook = hooks.CreateHook<Magic_Setup>(Magic_SetupImpl, result).Activate());
        
        Project.Scans.AddScanHook(nameof(TargetStruct_Create), (result, hooks)
            => TargetStruct_CreateHook = hooks.CreateHook<TargetStruct_Create>(TargetStruct_CreateImpl, result).Activate());
        
        // Magic File Processing hooks
        Project.Scans.AddScanHook(nameof(MagicFile_UnkExecute), (result, hooks)
            => MagicFile_UnkExecuteHook = hooks.CreateHook<MagicFile_UnkExecute>(MagicFile_UnkExecuteImpl, result).Activate());
        
        Project.Scans.AddScanHook(nameof(MagicFile_Process), (result, hooks)
            => MagicFile_ProcessHook = hooks.CreateHook<MagicFile_Process>(MagicFile_ProcessImpl, result).Activate());
        
        Project.Scans.AddScanHook(nameof(MagicFile_HandleSubEntry), (result, hooks)
            => MagicFile_HandleSubEntryHook = hooks.CreateHook<MagicFile_HandleSubEntry>(MagicFile_HandleSubEntryImpl, result).Activate());
    }
    
    // ============================================================
    // HOOK IMPLEMENTATIONS - Delegate to registered callbacks
    // ============================================================
    
    private long Magic_SetupImpl(long battleMagicPtr, int magicId, long casterActorRef, long positionStruct, int commandId, int actionID, byte flag)
    {
        if (OnMagicSetup != null)
            return OnMagicSetup(battleMagicPtr, magicId, casterActorRef, positionStruct, commandId, actionID, flag);
        return Magic_SetupHook!.OriginalFunction(battleMagicPtr, magicId, casterActorRef, positionStruct, commandId, actionID, flag);
    }
    
    private long* TargetStruct_CreateImpl(long manager, long* outResult)
    {
        if (OnTargetStructCreate != null)
            return OnTargetStructCreate(manager, outResult);
        return TargetStruct_CreateHook!.OriginalFunction(manager, outResult);
    }
    
    private void MagicFile_UnkExecuteImpl(long magicFileInstance, int opType, int propertyId, long dataPtr)
    {
        if (OnMagicFileUnkExecute != null)
        {
            OnMagicFileUnkExecute(magicFileInstance, opType, propertyId, dataPtr);
            return;
        }
        MagicFile_UnkExecuteHook!.OriginalFunction(magicFileInstance, opType, propertyId, dataPtr);
    }
    
    private long MagicFile_ProcessImpl(long a1, long a2, long a3, long a4)
    {
        if (OnMagicFileProcess != null)
            return OnMagicFileProcess(a1, a2, a3, a4);
        return MagicFile_ProcessHook!.OriginalFunction(a1, a2, a3, a4);
    }
    
    private long MagicFile_HandleSubEntryImpl(long a1, long a2, long a3, long a4)
    {
        if (OnMagicFileHandleSubEntry != null)
            return OnMagicFileHandleSubEntry(a1, a2, a3, a4);
        return MagicFile_HandleSubEntryHook!.OriginalFunction(a1, a2, a3, a4);
    }

    private nint BattleMagicExecutor_InsertMagicImpl(nint @this, BattleMagic* magic)
    {
        var res = BattleMagicExecutor_InsertMagicHook.OriginalFunction(@this, magic);
        if (_frameworkConfig.MagicSystem.PrintMagicCasts)
        {
            nint* staticActorInfo = null;
            if (magic->MagicActorId != 0)
                return res;

            ActorReference* actorRef = _entityManager.ActorManager_GetActorByKeyFunction(_entityManager.ActorManager, magic->CasterActorId);
            _shell.LogWriteLine(nameof(MagicHooks), $"Magic: {magic->MagicId} (from actor: {magic->CasterActorId:X} (entity {actorRef->EntityID:X})) @ {magic->Position:F2})", color: Color.LightBlue);
        }

        return res;
    }
}

public unsafe struct BattleMagic
{
    public nint vtable;
    public Node* ParentNode;
    public Vector3 Position;
    public int dword1C;
    public BattleMagicSub0x20 qword20;
    public nint gap78;
    public int field_80;
    public int field_84;
    public int field_88;
    public int field_8C;
    public int field_90;
    public int field_94;
    public int field_98;
    public int field_9C;
    public nint Field_a0;
    public nint field_A8;
    public nint field_B0;
    public nint field_B8;
    public short field_C0;
    public byte field_C2;
    public byte gapC3;
    public int field_C4;
    public ResourceHandleStruct* MagicFileResource;
    public nint qwordD0;
    public Vector4 Vec4_;
    public uint CasterActorId;
    public uint MagicActorId; // = Entity Id 6, See ActorBase->Magic
    public int Field_f0;
    public int CommandId;
    public int MagicId;
    public int SubId;
    public byte field_100;
    public byte byte101;
    public byte field_102;
    public byte field_103;
    int field_104;

    public struct BattleMagicSub0x20
    {
        public nint qword20;
        public ArrayBuffer0x20 ArrayBuffer_;
        public int dword50;
        public int dword54;
        public int dword58;
        public int dword5C;
        public int dword60;
        public int dword64;
        public int dword68;
        public float dword6C;
        public int dword70;
        public int dword74;
    };

    public unsafe struct ArrayBuffer0x20
    {
        nint Size;
        fixed byte Dst[32];
    };

};
