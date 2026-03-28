using System.Numerics;
using Reloaded.Mod.Interfaces;
using FF16Framework.Faith.Structs;
using FF16Framework.Faith.Hooks;
using FF16Framework.Interfaces.GameApis.Structs;
using FF16Framework.Services.Faith.GameApis.Structs;

namespace FF16Framework.Services.Faith.GameApis.Actor;

/// <summary>
/// Actor API implementation for actor management.
/// Provides player info, targeting, and actor lookups for the Magic API.
/// Uses EntityManagerHooks for all function wrappers and singletons.
/// </summary>
internal unsafe class ActorApi
{
    // ============================================================
    // DEPENDENCIES
    // ============================================================
    
    private readonly ILogger _logger;
    private readonly IModConfig _modConfig;
    private readonly EntityManagerHooks _entityHooks;
    private readonly UnkList35Hooks _list35Hooks;
    
    /// <summary>
    /// Cached player (Clive) StaticActorInfo pointer.
    /// </summary>
    public nint PlayerStaticActorInfo { get; private set; }
    
    // ============================================================
    // PROPERTIES
    // ============================================================
    
    /// <inheritdoc/>
    public bool IsInitialized => 
        _entityHooks.UnkSingletonPlayerOrCameraRelated != 0 && 
        _entityHooks.StaticActorManager != 0 && 
        _entityHooks.ActorManager != null;
    
    /// <inheritdoc/>
    public bool HasTargetingFunctions =>
        _list35Hooks.UnkSingletonPlayer_GetList35EntryFunction != null &&
        _list35Hooks.UnkList35Entry_GetCurrentTargettedEnemyFunction != null;
    
    // ============================================================
    // INTERNAL HELPERS
    // ============================================================
    
    /// <summary>
    /// Returns true if position/rotation functions are available.
    /// </summary>
    private bool HasPositionFunctions =>
        _entityHooks.GetPositionFunction != null &&
        _entityHooks.GetRotationFunction != null &&
        _entityHooks.GetForwardVectorFunction != null;
    
    // ============================================================
    // CONSTRUCTOR
    // ============================================================
    
    public ActorApi(ILogger logger, IModConfig modConfig, EntityManagerHooks entityHooks, UnkList35Hooks list35Hooks)
    {
        _logger = logger;
        _modConfig = modConfig;
        _entityHooks = entityHooks;
        _list35Hooks = list35Hooks;
    }
    
    // ============================================================
    // PLAYER
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
        if (_list35Hooks.GetActorData35EntryFunction == null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] GetPlayerActorData35Entry: Function not available", _logger.ColorYellow);
            return 0;
        }
        
        nint playerStaticActorInfo = GetPlayerStaticActorInfo();
        if (playerStaticActorInfo == 0)
            return 0;
        
        // Check if actor has valid data first (optional safety check)
        if (_entityHooks.HasEntityDataFunction != null)
        {
            if (_entityHooks.HasEntityDataFunction(playerStaticActorInfo) == 0)
                return 0;
        }
        
        return _list35Hooks.GetActorData35EntryFunction(playerStaticActorInfo);
    }
    
    private uint GetPlayerActorId()
    {
        nint singleton = _entityHooks.UnkSingletonPlayerOrCameraRelated;
        if (singleton == 0)
            return 0;
        
        return *(uint*)(singleton + UnkSingletonOffsets.CurrentActorId);
    }
    
    // ============================================================
    // TARGETING
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
    public ITargetInfo? CopyGameTargetInfo()
    {
        var gameTarget = GetTargetedEnemy();
        if (gameTarget == null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] CopyGameTargetInfo: No target locked", _logger.ColorYellow);
            return null;
        }
        
        var position = new Vector3(gameTarget->X, gameTarget->Y, gameTarget->Z);
        var direction = new Vector3(gameTarget->DirectionX, gameTarget->DirectionY, gameTarget->DirectionZ);
        
        _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] CopyGameTargetInfo: ActorId={gameTarget->ActorId:X}, Pos=({position.X:F2}, {position.Y:F2}, {position.Z:F2})", _logger.ColorGreen);
        return new TargetInfo(position, direction, gameTarget->ActorId);
    }
    
    private TargetStruct* GetTargetedEnemy()
    {
        nint singleton = _entityHooks.UnkSingletonPlayerOrCameraRelated;
        if (!HasTargetingFunctions || singleton == 0)
            return null;
        
        uint actorId = GetPlayerActorId();
        if (actorId == 0)
            return null;
        
        nint list35Entry = _list35Hooks.UnkSingletonPlayer_GetList35EntryFunction((UnkSingletonPlayer*)singleton);
        if (list35Entry == nint.Zero)
            return null;
        
        var target = _list35Hooks.UnkList35Entry_GetCurrentTargettedEnemyFunction(list35Entry, 0);
        return (TargetStruct*)target;
    }
    
    // ============================================================
    // ACTOR LOOKUP
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
            if (_entityHooks.ActorManager != null && _entityHooks.ActorManager_GetActorByKeyFunction != null && actorId != 0)
            {
                var actorReference = _entityHooks.ActorManager_GetActorByKeyFunction(_entityHooks.ActorManager, actorId);
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
    // TARGET CREATION
    // ============================================================
    
    /// <inheritdoc/>
    public ITargetInfo? CreateTargetFromActorWithTracking(nint staticActorInfo)
    {
        if (staticActorInfo == 0 || _entityHooks.GetPositionFunction == null)
            return null;
        
        NodePositionPair position;
        var result = _entityHooks.GetPositionFunction(staticActorInfo, &position);
        if (result == null)
            return null;
        
        var actorInfo = (StaticActorInfo*)staticActorInfo;
        int actorId = (int)actorInfo->ActorId;
        
        return TargetInfo.FromActor(position.Position, actorId);
    }
    
    /// <inheritdoc/>
    public ITargetInfo? CreateTargetFromActor(nint staticActorInfo)
    {
        if (staticActorInfo == 0 || _entityHooks.GetPositionFunction == null)
            return null;
        
        NodePositionPair position;
        var result = _entityHooks.GetPositionFunction(staticActorInfo, &position);
        if (result == null)
            return null;
        
        return TargetInfo.FromPosition(position.Position);
    }
    
    // ============================================================
    // INTERNAL POSITION HELPER (used by CreateTargetFromActor)
    // ============================================================
    
    private NodePositionPair* GetPositionInternal(nint staticActorInfo)
    {
        if (staticActorInfo == 0 || _entityHooks.GetPositionFunction == null)
            return null;
        
        NodePositionPair position;
        return _entityHooks.GetPositionFunction(staticActorInfo, &position);
    }
    
    // ============================================================
    // INTERNAL METHODS - For MagicCastingEngine (returns game TargetStruct)
    // ============================================================
    
    /// <summary>
    /// Copies the game's own TargetStruct for the currently locked enemy.
    /// Internal use only - returns the raw game struct for SetupMagic.
    /// </summary>
    internal TargetStruct? CopyGameTargetStructInternal()
    {
        var gameTarget = GetTargetedEnemy();
        if (gameTarget == null)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] CopyGameTargetStructInternal: No target locked", _logger.ColorYellow);
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
        
        _logger.WriteLine($"[{_modConfig.ModId}] [ActorApi] CopyGameTargetStructInternal: ActorId={copy.ActorId:X}, Pos=({copy.X:F2}, {copy.Y:F2}, {copy.Z:F2})", _logger.ColorGreen);
        return copy;
    }
    
    /// <summary>
    /// Creates a TargetStruct from a StaticActorInfo with actor tracking.
    /// Internal use only - returns the raw game struct for SetupMagic.
    /// </summary>
    internal TargetStruct? CreateTargetStructWithTracking(nint staticActorInfo)
    {
        if (staticActorInfo == 0 || _entityHooks.GetPositionFunction == null)
            return null;
        
        NodePositionPair position;
        var result = _entityHooks.GetPositionFunction(staticActorInfo, &position);
        if (result == null)
            return null;
        
        var actorInfo = (StaticActorInfo*)staticActorInfo;
        int actorId = (int)actorInfo->ActorId;
        
        var target = TargetStruct.FromActorId(actorId, position.Position);
        target.Node = (nint)position.ParentNode;
        
        return target;
    }
    
    /// <summary>
    /// Creates a TargetStruct from a StaticActorInfo's position only.
    /// Internal use only - returns the raw game struct for SetupMagic.
    /// </summary>
    internal TargetStruct? CreateTargetStructFromPosition(nint staticActorInfo)
    {
        if (staticActorInfo == 0 || _entityHooks.GetPositionFunction == null)
            return null;
        
        NodePositionPair position;
        var result = _entityHooks.GetPositionFunction(staticActorInfo, &position);
        if (result == null)
            return null;
        
        var target = TargetStruct.FromPosition(position.Position);
        target.Node = (nint)position.ParentNode;
        return target;
    }
    
    // ============================================================
    // PRIVATE ACTOR LOOKUP (used internally)
    // ============================================================
    
    /// <summary>
    /// Gets the StaticActorInfo pointer by actor ID. Internal use only.
    /// </summary>
    private nint GetStaticActorInfo(uint actorId)
    {
        nint staticActorManager = _entityHooks.StaticActorManager;
        var getOrCreateHook = _entityHooks.StaticActorManager_GetOrCreateHook;
        
        if (staticActorManager == 0 || getOrCreateHook == null)
            return 0;
        
        nint* staticActorInfo = null;
        getOrCreateHook.OriginalFunction(staticActorManager, &staticActorInfo, actorId);
        return staticActorInfo != null ? (nint)staticActorInfo : 0;
    }
}
