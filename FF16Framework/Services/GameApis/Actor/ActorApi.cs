using System.Numerics;
using Reloaded.Hooks.Definitions;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using FF16Framework.Faith.Structs;
using FF16Framework.Faith.Hooks;
using FF16Framework.Interfaces.GameApis.Actor;
using FF16Framework.Interfaces.GameApis.Structs;

namespace FF16Framework.Services.GameApis.Actor;

/// <summary>
/// Actor API implementation for actor management.
/// Provides player info, targeting, and actor lookups for the Magic API.
/// </summary>
public unsafe class ActorApi : IActorApi
{
    // ============================================================
    // DELEGATES
    // ============================================================
    
    public delegate nint UnkSingletonPlayerOrCameraRelated_CtorDelegate(nint @this);
    public delegate nint StaticActorManager_GetOrCreateDelegate(nint @this, nint** outEntityInfo, uint entityId);
    public delegate ActorReference* ActorManager_SetupEntityDelegate(ActorManager* @this, nint entityPtr);
    public delegate ActorReference* ActorManager_GetActorByKeyDelegate(ActorManager* @this, uint actorId);
    public delegate NodePositionPair* StaticActorInfo_GetPositionDelegate(nint pStaticEntityInfo, NodePositionPair* outPair);
    public delegate Vector3* StaticActorInfo_GetRotationDelegate(nint pStaticEntityInfo, Vector3* outRotation);
    public delegate Vector3* StaticActorInfo_GetForwardVectorDelegate(nint pStaticEntityInfo, Vector3* outForward);
    public delegate nint UnkSingletonPlayer_GetList35EntryDelegate(nint @this);
    public delegate TargetStruct* UnkList35Entry_GetCurrentTargettedEnemyDelegate(nint @this, byte forceUnk);
    public delegate bool StaticActorInfo_HasActorDataDelegate(nint* pStaticActorInfo);
    public delegate nint StaticActorInfo_GetActorData35EntryDelegate(nint staticActorInfo);
    
    // ============================================================
    // HOOKS
    // ============================================================
    
    private IHook<StaticActorManager_GetOrCreateDelegate>? _staticActorManagerGetOrCreateHook;
    private IHook<UnkSingletonPlayerOrCameraRelated_CtorDelegate>? _unkSingletonCtorHook;
    private IHook<ActorManager_SetupEntityDelegate>? _actorManagerSetupEntityHook;
    
    // ============================================================
    // FUNCTION WRAPPERS
    // ============================================================
    
    private StaticActorManager_GetOrCreateDelegate? _getOrCreateEntityFunc;
    private ActorManager_GetActorByKeyDelegate? _getActorByKeyFunc;
    private StaticActorInfo_GetPositionDelegate? _getPositionFunc;
    private StaticActorInfo_GetRotationDelegate? _getRotationFunc;
    private StaticActorInfo_GetForwardVectorDelegate? _getForwardVectorFunc;
    private UnkSingletonPlayer_GetList35EntryDelegate? _getList35EntryFunc;
    private UnkList35Entry_GetCurrentTargettedEnemyDelegate? _getCurrentTargetFunc;
    private StaticActorInfo_HasActorDataDelegate? _hasActorDataFunc;
    private StaticActorInfo_GetActorData35EntryDelegate? _getActorData35EntryFunc;
    
    // ============================================================
    // SINGLETONS
    // ============================================================
    
    /// <summary>
    /// Player/Camera singleton. Contains current actor ID at +0xC8.
    /// </summary>
    public nint UnkSingletonPlayerOrCameraRelated { get; private set; }
    
    /// <summary>
    /// Static actor manager. Used to get StaticActorInfo by actor ID.
    /// </summary>
    public nint StaticActorManager { get; private set; }
    
    /// <summary>
    /// Actor manager. Contains actor references and entity lists.
    /// </summary>
    public ActorManager* ActorManager { get; private set; }
    
    /// <summary>
    /// Cached player (Clive) StaticActorInfo pointer.
    /// </summary>
    public nint PlayerStaticActorInfo { get; private set; }
    
    // ============================================================
    // DEPENDENCIES
    // ============================================================
    
    private readonly ILogger _logger;
    private readonly IModConfig _modConfig;
    private readonly long _baseAddress;
    
    // ============================================================
    // IACTORAPI PROPERTIES
    // ============================================================
    
    /// <inheritdoc/>
    public bool IsInitialized => 
        UnkSingletonPlayerOrCameraRelated != 0 && 
        StaticActorManager != 0 && 
        ActorManager != null;
    
    /// <inheritdoc/>
    public bool HasTargetingFunctions =>
        _getList35EntryFunc != null &&
        _getCurrentTargetFunc != null;
    
    // ============================================================
    // INTERNAL HELPERS
    // ============================================================
    
    /// <summary>
    /// Returns true if position/rotation functions are available.
    /// </summary>
    private bool HasPositionFunctions =>
        _getPositionFunc != null &&
        _getRotationFunc != null &&
        _getForwardVectorFunc != null;
    
    // ============================================================
    // CONSTRUCTOR
    // ============================================================
    
    public ActorApi(ILogger logger, IModConfig modConfig)
    {
        _logger = logger;
        _modConfig = modConfig;
        _baseAddress = System.Diagnostics.Process.GetCurrentProcess().MainModule!.BaseAddress;
    }
    
    // ============================================================
    // INITIALIZATION
    // ============================================================
    
    /// <summary>
    /// Set up signature scans for all actor-related functions.
    /// </summary>
    public void SetupScans(IStartupScanner scans, IReloadedHooks hooks)
    {
        // UnkSingletonPlayerOrCameraRelated_Ctor (captures player singleton)
        scans.AddMainModuleScan("48 89 5C 24 ?? 48 89 74 24 ?? 48 89 7C 24 ?? 55 41 54 41 55 41 56 41 57 48 8B EC 48 83 EC ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 45 ?? 4C 8D 81", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find UnkSingletonPlayerOrCameraRelated_Ctor", _logger.ColorRed);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _unkSingletonCtorHook = hooks.CreateHook<UnkSingletonPlayerOrCameraRelated_CtorDelegate>(UnkSingletonCtorImpl, addr).Activate();
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Hooked UnkSingletonPlayerOrCameraRelated_Ctor at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // StaticActorManager_GetOrCreate (captures StaticActorManager)
        scans.AddMainModuleScan("48 89 5C 24 ?? 48 89 6C 24 ?? 44 89 44 24 ?? 56 57 41 54 41 56 41 57 48 83 EC ?? 45 33 E4", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find StaticActorManager_GetOrCreate", _logger.ColorRed);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _staticActorManagerGetOrCreateHook = hooks.CreateHook<StaticActorManager_GetOrCreateDelegate>(StaticActorManagerGetOrCreateImpl, addr).Activate();
            _getOrCreateEntityFunc = _staticActorManagerGetOrCreateHook.OriginalFunction;
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Hooked StaticActorManager_GetOrCreate at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // ActorManager_SetupEntity (captures ActorManager)
        scans.AddMainModuleScan("48 89 5C 24 ?? 55 56 57 41 54 41 55 41 56 41 57 48 8B EC 48 83 EC ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 45 ?? 4C 8B F9 48 8B F2", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find ActorManager_SetupEntity", _logger.ColorRed);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _actorManagerSetupEntityHook = hooks.CreateHook<ActorManager_SetupEntityDelegate>(ActorManagerSetupEntityImpl, addr).Activate();
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Hooked ActorManager_SetupEntity at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // ActorManager_GetActorByKey - WRAPPER only
        scans.AddMainModuleScan("89 54 24 ?? 4C 8B D1 85 D2", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find ActorManager_GetActorByKey", _logger.ColorRed);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _getActorByKeyFunc = hooks.CreateWrapper<ActorManager_GetActorByKeyDelegate>(addr, out _);
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Found ActorManager_GetActorByKey at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // StaticActorInfo_GetPosition
        scans.AddMainModuleScan("40 53 48 83 EC ?? 48 8B DA E8 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B D3 48 8B C8 E8 ?? ?? ?? ?? EB ?? 48 83 63", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find StaticActorInfo_GetPosition", _logger.ColorRed);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _getPositionFunc = hooks.CreateWrapper<StaticActorInfo_GetPositionDelegate>(addr, out _);
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Found StaticActorInfo_GetPosition at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // StaticActorInfo_GetRotation
        scans.AddMainModuleScan("40 53 48 83 EC ?? 48 8B DA E8 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B 40", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find StaticActorInfo_GetRotation", _logger.ColorYellow);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _getRotationFunc = hooks.CreateWrapper<StaticActorInfo_GetRotationDelegate>(addr, out _);
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Found StaticActorInfo_GetRotation at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // StaticActorInfo_GetForwardVector
        scans.AddMainModuleScan("40 53 48 83 EC ?? 48 8B DA E8 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B 50", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find StaticActorInfo_GetForwardVector", _logger.ColorYellow);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _getForwardVectorFunc = hooks.CreateWrapper<StaticActorInfo_GetForwardVectorDelegate>(addr, out _);
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Found StaticActorInfo_GetForwardVector at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // UnkSingletonPlayer_GetList35Entry (for targeting)
        scans.AddMainModuleScan("48 89 5C 24 ?? 57 48 83 EC ?? 44 8B 81 ?? ?? ?? ?? 48 8D 54 24 ?? 48 8B 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 4C 24 ?? E8 ?? ?? ?? ?? 48 8B 5C 24 ?? 84 C0 74 ?? 48 8B CB E8 ?? ?? ?? ?? EB", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find UnkSingletonPlayer_GetList35Entry", _logger.ColorYellow);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _getList35EntryFunc = hooks.CreateWrapper<UnkSingletonPlayer_GetList35EntryDelegate>(addr, out _);
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Found UnkSingletonPlayer_GetList35Entry at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // UnkList35Entry_GetCurrentTargettedEnemy
        scans.AddMainModuleScan("48 89 5C 24 ?? 57 48 83 EC ?? 48 8B D9 40 8A FA 48 8B 89 ?? ?? ?? ?? 48 85 C9 0F 84", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find UnkList35Entry_GetCurrentTargettedEnemy", _logger.ColorYellow);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _getCurrentTargetFunc = hooks.CreateWrapper<UnkList35Entry_GetCurrentTargettedEnemyDelegate>(addr, out _);
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Found UnkList35Entry_GetCurrentTargettedEnemy at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // StaticActorInfo::HasActorData - checks if StaticActorInfo has valid ActorData
        // Pattern from IDA: E8 ?? ?? ?? ?? 48 8B F8 84 C0 74 (call HasActorData, test result)
        scans.AddMainModuleScan("40 53 48 83 EC ?? 33 DB 48 39 19", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find StaticActorInfo::HasActorData", _logger.ColorYellow);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _hasActorDataFunc = hooks.CreateWrapper<StaticActorInfo_HasActorDataDelegate>(addr, out _);
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Found StaticActorInfo::HasActorData at 0x{addr:X}", _logger.ColorGreen);
        });
        
        // StaticActorInfo::GetActorData35Entry - gets ActorData35Entry from StaticActorInfo
        // Pattern from IDA: similar structure to HasActorData but returns the entry
        scans.AddMainModuleScan("40 53 48 83 EC ?? 48 8B D9 48 8B 0D ?? ?? ?? ?? 48 8D 51 ?? 48 83 FA ?? 73 ?? 48 8B C2 41 B8 ?? ?? ?? ?? 83 E0 ?? 48 C1 EA ?? C4 42 F9 F7 C0 48 8B 44 D3 ?? 49 85 C0 75 ?? 49 0B C0 48 89 44 D3 ?? 48 8B 05 ?? ?? ?? ?? 8B 53 ?? 48 8B 8C C8 ?? ?? ?? ?? 48 8B 01 FF 50 ?? 48 89 83 ?? ?? ?? ?? 48 8B 83 ?? ?? ?? ?? 48 83 C4 ?? 5B C3 E8 ?? ?? ?? ?? CC CC 40 53 48 83 EC ?? 48 8B D9 48 8B 0D ?? ?? ?? ?? 48 8D 51 ?? 48 83 FA ?? 73 ?? 48 8B C2 41 B8 ?? ?? ?? ?? 83 E0 ?? 48 C1 EA ?? C4 42 F9 F7 C0 48 8B 44 D3 ?? 49 85 C0 75 ?? 49 0B C0 48 89 44 D3 ?? 48 8B 05 ?? ?? ?? ?? 8B 53 ?? 48 8B 8C C8 ?? ?? ?? ?? 48 8B 01 FF 50 ?? 48 89 83 ?? ?? ?? ?? 48 8B 83 ?? ?? ?? ?? 48 83 C4 ?? 5B C3 E8 ?? ?? ?? ?? CC CC 40 53 48 83 EC ?? 48 8B D9 48 8B 0D ?? ?? ?? ?? 48 8D 51 ?? 48 83 FA ?? 73 ?? 48 8B C2 41 B8 ?? ?? ?? ?? 83 E0 ?? 48 C1 EA ?? C4 42 F9 F7 C0 48 8B 44 D3 ?? 49 85 C0 75 ?? 49 0B C0 48 89 44 D3 ?? 48 8B 05 ?? ?? ?? ?? 8B 53 ?? 48 8B 8C C8 ?? ?? ?? ?? 48 8B 01 FF 50 ?? 48 89 43 ?? 48 8B 43 ?? 48 83 C4 ?? 5B C3 E8 ?? ?? ?? ?? CC CC CC CC 40 53 48 83 EC ?? 48 8B D9 48 8B 0D ?? ?? ?? ?? 48 8D 51 ?? 48 83 FA ?? 73 ?? 48 8B C2 41 B8 ?? ?? ?? ?? 83 E0 ?? 48 C1 EA ?? C4 42 F9 F7 C0 48 8B 44 D3 ?? 49 85 C0 75 ?? 49 0B C0 48 89 44 D3 ?? 48 8B 05 ?? ?? ?? ?? 8B 53 ?? 48 8B 8C C8 ?? ?? ?? ?? 48 8B 01 FF 50 ?? 48 89 83 ?? ?? ?? ?? 48 8B 83 ?? ?? ?? ?? 48 83 C4 ?? 5B C3 E8 ?? ?? ?? ?? CC CC 40 53 48 83 EC ?? 48 8B D9 48 8B 0D ?? ?? ?? ?? 48 8D 51 ?? 48 83 FA ?? 73 ?? 48 8B C2 41 B8 ?? ?? ?? ?? 83 E0 ?? 48 C1 EA ?? C4 42 F9 F7 C0 48 8B 44 D3 ?? 49 85 C0 75 ?? 49 0B C0 48 89 44 D3 ?? 48 8B 05 ?? ?? ?? ?? 8B 53 ?? 48 8B 8C C8 ?? ?? ?? ?? 48 8B 01 FF 50 ?? 48 89 83", result =>
        {
            if (!result.Found)
            {
                _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] FAILED to find StaticActorInfo::GetActorData35Entry", _logger.ColorYellow);
                return;
            }
            var addr = (nint)(_baseAddress + result.Offset);
            _getActorData35EntryFunc = hooks.CreateWrapper<StaticActorInfo_GetActorData35EntryDelegate>(addr, out _);
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Found StaticActorInfo::GetActorData35Entry at 0x{addr:X}", _logger.ColorGreen);
        });
    }
    
    // ============================================================
    // HOOK IMPLEMENTATIONS
    // ============================================================
    
    private nint UnkSingletonCtorImpl(nint @this)
    {
        UnkSingletonPlayerOrCameraRelated = @this;
        _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Captured UnkSingletonPlayerOrCameraRelated: 0x{@this:X}", _logger.ColorGreen);
        return _unkSingletonCtorHook!.OriginalFunction(@this);
    }
    
    private nint StaticActorManagerGetOrCreateImpl(nint @this, nint** outEntityInfo, uint entityId)
    {
        if (StaticActorManager == 0)
        {
            StaticActorManager = @this;
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Captured StaticActorManager: 0x{@this:X}", _logger.ColorGreen);
        }
        return _staticActorManagerGetOrCreateHook!.OriginalFunction(@this, outEntityInfo, entityId);
    }
    
    private ActorReference* ActorManagerSetupEntityImpl(ActorManager* @this, nint entityPtr)
    {
        if (ActorManager == null)
        {
            ActorManager = @this;
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] Captured ActorManager: 0x{(nint)@this:X}", _logger.ColorGreen);
        }
        return _actorManagerSetupEntityHook!.OriginalFunction(@this, entityPtr);
    }
    
    // ============================================================
    // IACTORAPI IMPLEMENTATION - PLAYER
    // ============================================================
    
    /// <inheritdoc/>
    public nint GetPlayerStaticActorInfo()
    {
        uint actorId = GetPlayerActorId();
        
        if (actorId == 0)
            return PlayerStaticActorInfo; // Return cached
        
        nint resolved = GetStaticActorInfo(actorId);
        if (resolved != 0)
            PlayerStaticActorInfo = resolved;
        
        return PlayerStaticActorInfo;
    }
    
    /// <inheritdoc/>
    public long GetPlayerActorData35Entry()
    {
        if (_getActorData35EntryFunc == null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] GetPlayerActorData35Entry: Function not available", _logger.ColorYellow);
            return 0;
        }
        
        nint playerStaticActorInfo = GetPlayerStaticActorInfo();
        if (playerStaticActorInfo == 0)
            return 0;
        
        // Check if actor has valid data first (optional safety check)
        if (_hasActorDataFunc != null)
        {
            nint* pInfo = (nint*)playerStaticActorInfo;
            if (!_hasActorDataFunc(pInfo))
                return 0;
        }
        
        return _getActorData35EntryFunc(playerStaticActorInfo);
    }
    
    private uint GetPlayerActorId()
    {
        if (UnkSingletonPlayerOrCameraRelated == 0)
            UnkSingletonPlayerOrCameraRelated = *(nint*)(_baseAddress + GlobalOffsets.UnkSingletonPlayerOrCamera);
        
        if (UnkSingletonPlayerOrCameraRelated == 0)
            return 0;
        
        return *(uint*)(UnkSingletonPlayerOrCameraRelated + UnkSingletonOffsets.CurrentActorId);
    }
    
    // ============================================================
    // IACTORAPI IMPLEMENTATION - TARGETING
    // ============================================================
    
    /// <inheritdoc/>
    public nint GetLockedTargetStaticActorInfo()
    {
        var target = GetTargetedEnemy();
        if (target == null || target->ActorId == 0)
            return nint.Zero;
        
        return GetStaticActorInfo((uint)target->ActorId);
    }
    
    /// <inheritdoc/>
    public TargetStruct? CopyGameTargetStruct()
    {
        var gameTarget = GetTargetedEnemy();
        if (gameTarget == null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] CopyGameTargetStruct: No target locked", _logger.ColorYellow);
            return null;
        }
        
        // Copy all fields, force Type = 1 for tracking
        var copy = new TargetStruct
        {
            VTable = gameTarget->VTable,
            Field_8 = gameTarget->Field_8,
            Field_10 = gameTarget->Field_10,
            Field_18 = gameTarget->Field_18,
            GlobalOffset = gameTarget->GlobalOffset,
            Node = gameTarget->Node,
            X = gameTarget->X,
            Y = gameTarget->Y,
            Z = gameTarget->Z,
            Dword1C = gameTarget->Dword1C,
            DirectionX = gameTarget->DirectionX,
            DirectionY = gameTarget->DirectionY,
            DirectionZ = gameTarget->DirectionZ,
            Padding4C = gameTarget->Padding4C,
            Type = 1,  // Force actor-tracking mode
            Field_54 = gameTarget->Field_54,
            Field_58 = gameTarget->Field_58,
            Field_5C = gameTarget->Field_5C,
            Field_60 = gameTarget->Field_60,
            Field_64 = gameTarget->Field_64,
            Field_68 = gameTarget->Field_68,
            ActorId = gameTarget->ActorId,
            Field_70 = gameTarget->Field_70,
            Field_74 = gameTarget->Field_74,
            Field_78 = gameTarget->Field_78
        };
        
        _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] CopyGameTargetStruct: ActorId={copy.ActorId:X}, Pos=({copy.X:F2}, {copy.Y:F2}, {copy.Z:F2})", _logger.ColorGreen);
        return copy;
    }
    
    private TargetStruct* GetTargetedEnemy()
    {
        if (!HasTargetingFunctions || UnkSingletonPlayerOrCameraRelated == 0)
            return null;
        
        uint actorId = GetPlayerActorId();
        if (actorId == 0)
            return null;
        
        nint list35Entry = _getList35EntryFunc!(UnkSingletonPlayerOrCameraRelated);
        if (list35Entry == nint.Zero)
            return null;
        
        return _getCurrentTargetFunc!(list35Entry, 0);
    }
    
    // ============================================================
    // IACTORAPI IMPLEMENTATION - ACTOR LOOKUP
    // ============================================================
    
    /// <inheritdoc/>
    public long GetActorRef(nint staticActorInfo)
    {
        if (staticActorInfo == 0 || staticActorInfo < 0x10000)
            return 0;
        
        try
        {
            var info = (StaticActorInfo*)staticActorInfo;
            uint actorId = info->ActorId;
            
            // Method 1: GetActorByKey (preferred)
            if (ActorManager != null && _getActorByKeyFunc != null && actorId != 0)
            {
                var actorReference = _getActorByKeyFunc(ActorManager, actorId);
                if (actorReference != null)
                    return (long)actorReference;
            }
            
            // Method 2: Direct struct access
            long directActorRef = info->ActorRef;
            if (directActorRef != 0)
                return directActorRef;
            
            // Method 3: Fallback to pointer itself
            return staticActorInfo;
        }
        catch
        {
            return 0;
        }
    }
    
    // ============================================================
    // IACTORAPI IMPLEMENTATION - TARGET CREATION
    // ============================================================
    
    /// <inheritdoc/>
    public TargetStruct? CreateTargetFromActorWithTracking(nint staticActorInfo)
    {
        if (staticActorInfo == 0 || _getPositionFunc == null)
            return null;
        
        NodePositionPair position;
        var result = _getPositionFunc(staticActorInfo, &position);
        if (result == null)
            return null;
        
        var actorInfo = (StaticActorInfo*)staticActorInfo;
        int actorId = (int)actorInfo->ActorId;
        
        var target = TargetStruct.FromActorId(actorId, position.Position);
        target.Node = (nint)position.ParentNode;
        
        return target;
    }
    
    /// <inheritdoc/>
    public TargetStruct? CreateTargetFromActor(nint staticActorInfo)
    {
        if (staticActorInfo == 0 || _getPositionFunc == null)
            return null;
        
        NodePositionPair position;
        var result = _getPositionFunc(staticActorInfo, &position);
        if (result == null)
            return null;
        
        var target = TargetStruct.FromPosition(position.Position);
        target.Node = (nint)position.ParentNode;
        return target;
    }
    
    // ============================================================
    // INTERNAL POSITION HELPER (used by CreateTargetFromActor)
    // ============================================================
    
    private NodePositionPair* GetPositionInternal(nint staticActorInfo)
    {
        if (staticActorInfo == 0 || _getPositionFunc == null)
            return null;
        
        NodePositionPair position;
        return _getPositionFunc(staticActorInfo, &position);
    }
    
    // ============================================================
    // PRIVATE ACTOR LOOKUP (used internally)
    // ============================================================
    
    /// <summary>
    /// Gets the StaticActorInfo pointer by actor ID. Internal use only.
    /// </summary>
    private nint GetStaticActorInfo(uint actorId)
    {
        if (StaticActorManager == 0 || _getOrCreateEntityFunc == null)
            return 0;
        
        nint* staticActorInfo = null;
        _getOrCreateEntityFunc(StaticActorManager, &staticActorInfo, actorId);
        return staticActorInfo != null ? (nint)staticActorInfo : 0;
    }
    
    // ============================================================
    // IACTORAPI IMPLEMENTATION - STATE DETECTION (PUBLIC)
    // ============================================================
    
    // Track vertical push to detect post-launch state
    private readonly System.Collections.Concurrent.ConcurrentDictionary<long, float> _npcVerticalPush = new();
    
    /// <inheritdoc/>
    public bool IsAirborne(long bnpcRow)
    {
        if (bnpcRow < 0x10000 || bnpcRow > 0x00007FFFFFFFFFFF) return false;

        try
        {
            StaticActorInfo* info = (StaticActorInfo*)*(long*)(bnpcRow + BnpcRowOffsets.StaticActorInfoPtr);
            
            long actorPtr = 0;
            if (info != null && (long)info > 0x10000)
            {
                actorPtr = info->ActorRef;
            }
            
            // Fallback: read bnpcRow + 0 directly (old method)
            if (actorPtr == 0) 
            {
                actorPtr = *(long*)bnpcRow;
            }

            if (actorPtr > 0x10000 && actorPtr < 0x00007FFFFFFFFFFF)
            {
                // ReactionState (Byte):
                // 0x02 = Ground / Neutral
                // 0x03-0x05 = Ground reactions (Step Back/Slide)
                // > 0x05 = Airborne / Launch reaction (0x67, 0xC0, etc)
                byte reactionState = *(byte*)(actorPtr + ActorOffsets.ReactionState);
                if (reactionState > 5) return true;
                
                // If state is 0x02 but we have recent vertical push, maintain airborne
                if (reactionState == 2)
                {
                    if (_npcVerticalPush.TryGetValue(bnpcRow, out float push) && push > 0.1f)
                    {
                        _npcVerticalPush.TryRemove(bnpcRow, out _);
                        return false; 
                    }
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] IsAirborne Error: {ex.Message}", _logger.ColorRed);
        }
        return false;
    }
}
