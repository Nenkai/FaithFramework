using System.Numerics;
using System.Runtime.InteropServices;
using Reloaded.Hooks.Definitions;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using FF16Framework;
using FF16Framework.Faith.Structs;
using FF16Framework.Services.GameApis.Actor;
using FF16Framework.Interfaces.GameApis.Actor;
using FF16Framework.Interfaces.GameApis.Magic;
using FF16Framework.Interfaces.GameApis.Structs;

namespace FF16Framework.Services.GameApis.Magic;

/// <summary>
/// Internal engine for casting magic spells.
/// Handles the low-level game hooks and memory management.
/// This is NOT part of the public API.
/// </summary>
internal unsafe class MagicCastingEngine : IDisposable
{
    // ============================================================
    // DELEGATES
    // ============================================================

    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate long SetupMagicDelegate(long battleMagicPtr, int magicId, long casterActorRef, long positionStruct, int commandId, int actionID, byte flag);
    
    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate char CastMagicDelegate(long a1, long unkMagicStructPtr);

    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate char FireMagicProjectileDelegate(long magicManagerPtr, long projectileDataPtr);
    
    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate long* UnkTargetStructCreateDelegate(long manager, long* outResult);
    
    // ============================================================
    // SIGNATURES
    // ============================================================
    private const string SETUP_MAGIC_SIG = "48 8B C4 48 89 58 08 48 89 70 10 57 48 83 EC 60 8B FA 66 C7 40 E8 01 00 48 8B F1 C6 40 EA 00 C5 F9 EF C0 49 8B D1 48 8D 48 D8 C5 FA 7F 40 D8 49 8B D8";
    private const string CAST_MAGIC_SIG = "48 89 5C 24 10 48 89 74 24 18 57 48 83 EC 20 48 8B 41 10 48 8B F2 48 8B 0D";
    private const string INSERT_NEW_MAGIC_SIG = "40 53 48 83 EC 20 48 8B DA 4C 8B D9 8B 92 EC 00";
    private const string FIRE_MAGIC_PROJECTILE_SIG = "48 89 5C 24 10 48 89 74 24 18 48 89 7C 24 20 55 41 54 41 55 41 56 41 57 48 8d 6C 24 90 48 81 EC 70 01 00 00 48 8B 05 2D 0F 1A 01 48 33 C4 48 89 45 60 48 8B 51 38 4C 8B E1 44 8B 42 10 41 83 E8 01 0F 84 B6 01 00 00";
    private const string UNK_TARGET_STRUCT_CREATE_SIG = "48 89 5C 24 ?? 48 89 74 24 ?? 48 89 7C 24 ?? 41 56 48 83 EC ?? 48 8D 99 ?? ?? ?? ?? 48 8B F1";
    
    // ============================================================
    // CONSTANTS
    // ============================================================
    
    private const int DEFAULT_COMMAND_ID = 101;
    private const int DEFAULT_ACTION_ID = 218;
    private const byte DEFAULT_FLAG = 1;
    private const int MAGIC_STRUCT_SIZE = 0x108;
    
    // ============================================================
    // HOOKS & FUNCTION POINTERS
    // ============================================================
    
    private IHook<SetupMagicDelegate>? _setupMagicHook;
    private IHook<CastMagicDelegate>? _castMagicHook;
    private IHook<FireMagicProjectileDelegate>? _fireMagicProjectileHook;
    private IHook<UnkTargetStructCreateDelegate>? _unkTargetStructCreateHook;
    private CastMagicDelegate? _castMagicWrapper;
    
    // ============================================================
    // CACHED CONTEXT
    // ============================================================
    
    // Instead of reusing a single buffer (which caused crashes when the game
    // held references to our buffer), we now allocate fresh buffers per cast.
    // We keep a pool to avoid excessive allocations but let old ones "age out".
    private readonly List<IntPtr> _allocatedMagicBuffers = new();
    private readonly List<IntPtr> _allocatedTargetBuffers = new();
    private const int MAX_BUFFER_POOL_SIZE = 32;  // Keep at most 32 buffers alive
    
    private long _cachedCasterActorRef = 0;
    private long _cachedPositionStruct = 0;
    private nint _cachedTargetVTable = 0;  // VTable from game's TargetStruct
    private int _cachedCommandId = 0;
    private int _cachedActionId = 0;
    private byte _cachedFlag = 0;
    private long _cachedExecutorClient = 0;
    private bool _hasMagicContext = false;
    
    // ============================================================
    // DEPENDENCIES
    // ============================================================
    
    private readonly ILogger _logger;
    private readonly string _modId;
    private readonly MagicProcessor _processor;
    private readonly long _baseAddress;
    
    // Actor API reference for actor/player management
    private IActorApi? _actorApi;
    
    // Callback for active Eikon detection (still needed for Eikon-specific behavior)
    public Func<int>? GetActiveEikon { get; set; }
    public Func<int, bool>? OnChargedShotDetected { get; set; }
    
    // ============================================================
    // PROPERTIES
    // ============================================================
    
    /// <summary>
    /// Returns true if we can cast spells.
    /// With explicit source/target support, we may not need cached context anymore.
    /// </summary>
    public bool IsReady => _setupMagicHook != null && _castMagicWrapper != null;
    
    // ============================================================
    // CONSTRUCTOR
    // ============================================================
    
    private const int TARGET_STRUCT_SIZE = 0x7C;  // Size of TargetStruct (UnkTargetStruct)
    
    public MagicCastingEngine(ILogger logger, string modId, FrameworkConfig frameworkConfig, IStartupScanner scanner)
    {
        _logger = logger;
        _modId = modId;
        _baseAddress = System.Diagnostics.Process.GetCurrentProcess().MainModule!.BaseAddress;
        
        // Create processor component
        _processor = new MagicProcessor(logger, modId, frameworkConfig, scanner);
        
        _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Initialized (using per-cast buffer allocation)", _logger.ColorGreen);
    }
    
    /// <summary>
    /// Sets the ActorApi reference for actor/player management.
    /// </summary>
    public void SetActorApi(IActorApi actorApi)
    {
        _actorApi = actorApi;
    }
    
    // ============================================================
    // INITIALIZATION
    // ============================================================
    
    public void SetupScans(IStartupScanner scans, IReloadedHooks hooks)
    {
        scans.AddScan(SETUP_MAGIC_SIG, address =>
        {
            _setupMagicHook = hooks.CreateHook<SetupMagicDelegate>(SetupMagicImpl, address).Activate();
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Hooked SetupMagic at 0x{address:X}", _logger.ColorGreen);
        });
        
        scans.AddScan(CAST_MAGIC_SIG, address =>
        {
            _castMagicHook = hooks.CreateHook<CastMagicDelegate>(CastMagicImpl, address).Activate();
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Hooked CastMagic at 0x{address:X}", _logger.ColorGreen);
        });

        scans.AddScan(INSERT_NEW_MAGIC_SIG, address =>
        {
            _castMagicWrapper = hooks.CreateWrapper<CastMagicDelegate>(address, out _);
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Resolved InsertNewMagic at 0x{address:X}", _logger.ColorGreen);
        });

        scans.AddScan(FIRE_MAGIC_PROJECTILE_SIG, address =>
        {
            _fireMagicProjectileHook = hooks.CreateHook<FireMagicProjectileDelegate>(FireMagicProjectileImpl, address).Activate();
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Hooked FireMagicProjectile at 0x{address:X}", _logger.ColorGreen);
        });
        
        // Hook UnkTargetStruct::Create to capture VTable automatically
        scans.AddScan(UNK_TARGET_STRUCT_CREATE_SIG, address =>
        {
            _unkTargetStructCreateHook = hooks.CreateHook<UnkTargetStructCreateDelegate>(UnkTargetStructCreateImpl, address).Activate();
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Hooked UnkTargetStruct::Create at 0x{address:X}", _logger.ColorGreen);
        });
    }
    
    public void InitializeProcessor(IReloadedHooks hooks)
    {
        _processor.SetupScans(hooks);
    }
    
    // ============================================================
    // PUBLIC API
    // ============================================================
    
    /// <summary>
    /// Gets the currently locked target from the camera system.
    /// </summary>
    public nint GetLockedTarget()
    {
        return _actorApi?.GetLockedTargetStaticActorInfo() ?? nint.Zero;
    }
    
    /// <summary>
    /// Gets the player's actor pointer.
    /// </summary>
    public nint GetPlayerActor()
    {
        return _actorApi?.GetPlayerStaticActorInfo() ?? nint.Zero;
    }
    
    /// <summary>
    /// Cast a spell using the provided request configuration.
    /// Supports explicit source actor and target position without requiring cached context.
    /// </summary>
    public bool CastSpell(MagicCastRequest request)
    {
        if (!IsReady)
        {
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Cannot cast: Hooks not initialized.", _logger.ColorYellow);
            return false;
        }
        
        // CRITICAL: Allocate fresh buffers for each cast.
        // The game may hold references to our buffers after SetupMagic/InsertNewMagic return.
        // Reusing the same buffer caused crashes when the game accessed stale data later.
        IntPtr magicBuffer = AllocateMagicBuffer();
        IntPtr targetBuffer = AllocateTargetBuffer();
        
        // Enqueue modifications if any
        if (request.Modifications.Count > 0)
        {
            var modEntries = ConvertToMagicModEntries(request.Modifications);
            _processor.EnqueueModifications(request.MagicId, modEntries);
        }
        
        // ========================================================
        // RESOLVE SOURCE ACTOR
        // Priority: Explicit > ActorApi (Player) > Cached
        // ========================================================
        long casterActorRef = 0;
        
        string sourceResolution = "Unknown";
        
        if (request.SourceActor.HasValue && request.SourceActor.Value != nint.Zero)
        {
            // Explicit source actor provided - get ActorRef from StaticActorInfo
            if (_actorApi != null)
            {
                casterActorRef = _actorApi.GetActorRef(request.SourceActor.Value);
                sourceResolution = $"Explicit via ActorApi (StaticActorInfo: 0x{request.SourceActor.Value:X})";
            }
            else
            {
                // Fallback: direct struct access
                var actorInfo = (StaticActorInfo*)request.SourceActor.Value;
                casterActorRef = actorInfo->ActorRef;
                sourceResolution = $"Explicit via Direct Struct (StaticActorInfo: 0x{request.SourceActor.Value:X})";
            }
        }
        else if (_actorApi != null)
        {
            // Get player actor via ActorApi
            var playerInfo = _actorApi.GetPlayerStaticActorInfo();
            if (playerInfo != 0)
            {
                casterActorRef = _actorApi.GetActorRef(playerInfo);
                sourceResolution = $"Player via ActorApi (StaticActorInfo: 0x{playerInfo:X})";
            }
        }
        
        // Fallback to cached
        if (casterActorRef == 0 && _cachedCasterActorRef != 0)
        {
            casterActorRef = _cachedCasterActorRef;
            sourceResolution = "Cached from previous game cast";
        }
        
        // Log source resolution
        _logger.WriteLine($"[{_modId}] [CastSpell] SOURCE: {sourceResolution} -> ActorRef: 0x{casterActorRef:X}", _logger.ColorYellow);
        
        // ========================================================
        // RESOLVE TARGET POSITION STRUCT
        // Priority: UseGameTarget > Explicit Position > Explicit Actor > Locked Target > Source Actor > Cached
        // ========================================================
        
        // Note: targetBuffer is already zero-initialized by AllocateTargetBuffer()
        
        long targetStructPtr = 0;
        string targetResolution = "Unknown";
        
        // HIGHEST PRIORITY: UseGameTarget - copy the game's own TargetStruct directly
        if (request.UseGameTarget && _actorApi != null)
        {
            var gameTarget = _actorApi.CopyGameTargetStruct();
            if (gameTarget.HasValue)
            {
                *(TargetStruct*)targetBuffer = gameTarget.Value;
                targetStructPtr = (long)targetBuffer;
                targetResolution = $"Game Target (ActorId: {gameTarget.Value.ActorId:X}, Type: {gameTarget.Value.Type}, Pos: {gameTarget.Value.X:F2}, {gameTarget.Value.Y:F2}, {gameTarget.Value.Z:F2})";
                _logger.WriteLine($"[{_modId}] [CastSpell] Using GAME TARGET with body position!", _logger.ColorGreen);
            }
            else
            {
                _logger.WriteLine($"[{_modId}] [CastSpell] UseGameTarget requested but no target locked in game", _logger.ColorYellow);
            }
        }
        
        if (targetStructPtr == 0 && request.TargetPosition.HasValue)
        {
            // Create TargetStruct from explicit position
            var targetStruct = request.TargetDirection.HasValue
                ? TargetStruct.FromPositionAndDirection(request.TargetPosition.Value, request.TargetDirection.Value)
                : TargetStruct.FromPosition(request.TargetPosition.Value);
            
            // Copy to our buffer
            *(TargetStruct*)targetBuffer = targetStruct;
            targetStructPtr = (long)targetBuffer;
            targetResolution = $"Explicit Position ({request.TargetPosition.Value.X:F2}, {request.TargetPosition.Value.Y:F2}, {request.TargetPosition.Value.Z:F2})";
        }
        else if (targetStructPtr == 0 && request.TargetActor.HasValue && request.TargetActor.Value != nint.Zero)
        {
            // Create TargetStruct from explicit target actor with tracking
            if (_actorApi != null)
            {
                var targetResult = _actorApi.CreateTargetFromActorWithTracking(request.TargetActor.Value);
                if (targetResult.HasValue)
                {
                    *(TargetStruct*)targetBuffer = targetResult.Value;
                    targetStructPtr = (long)targetBuffer;
                    targetResolution = $"Explicit Actor via ActorApi (StaticActorInfo: 0x{request.TargetActor.Value:X}, ActorId: {targetResult.Value.ActorId}, Type: {targetResult.Value.Type}, Pos: {targetResult.Value.X:F2}, {targetResult.Value.Y:F2}, {targetResult.Value.Z:F2})";
                }
            }
        }
        else if (targetStructPtr == 0 && !request.TargetActor.HasValue)
        {
            // TargetActor is null (not specified) - try to get locked target from camera
            var lockedTarget = GetLockedTarget();
            if (lockedTarget != nint.Zero && _actorApi != null)
            {
                var targetResult = _actorApi.CreateTargetFromActorWithTracking(lockedTarget);
                if (targetResult.HasValue)
                {
                    *(TargetStruct*)targetBuffer = targetResult.Value;
                    targetStructPtr = (long)targetBuffer;
                    targetResolution = $"Locked Target via ActorApi (StaticActorInfo: 0x{lockedTarget:X}, ActorId: {targetResult.Value.ActorId}, Type: {targetResult.Value.Type}, Pos: {targetResult.Value.X:F2}, {targetResult.Value.Y:F2}, {targetResult.Value.Z:F2})";
                }
            }
            else if (lockedTarget == nint.Zero)
            {
                _logger.WriteLine($"[{_modId}] [CastSpell] No locked target available (GetLockedTarget returned Zero)", _logger.ColorYellow);
            }
        }
        
        // If no target yet, try using source actor's position
        if (targetStructPtr == 0 && _actorApi != null)
        {
            // Determine source actor to use for position
            nint sourceForPosition = nint.Zero;
            string sourceType = "Unknown";
            
            if (request.SourceActor.HasValue && request.SourceActor.Value != nint.Zero)
            {
                sourceForPosition = request.SourceActor.Value;
                sourceType = "Explicit Source Actor";
            }
            else
            {
                sourceForPosition = (nint)_actorApi.GetPlayerStaticActorInfo();
                sourceType = "Player via ActorApi";
            }
            
            if (sourceForPosition != nint.Zero)
            {
                var targetResult = _actorApi.CreateTargetFromActor(sourceForPosition);
                if (targetResult.HasValue)
                {
                    *(TargetStruct*)targetBuffer = targetResult.Value;
                    targetStructPtr = (long)targetBuffer;
                    targetResolution = $"Source Actor Position ({sourceType}) via ActorApi (0x{sourceForPosition:X}, Pos: {targetResult.Value.X:F2}, {targetResult.Value.Y:F2}, {targetResult.Value.Z:F2})";
                }
            }
        }
        
        // Fallback to cached position struct
        if (targetStructPtr == 0)
        {
            targetStructPtr = _cachedPositionStruct;
            if (targetStructPtr != 0)
                targetResolution = $"Cached from previous game cast (0x{targetStructPtr:X})";
        }
        
        // Log target resolution
        _logger.WriteLine($"[{_modId}] [CastSpell] TARGET: {targetResolution} -> StructPtr: 0x{targetStructPtr:X}", _logger.ColorYellow);
        
        // ========================================================
        // RESOLVE COMMAND/ACTION IDs
        // ========================================================
        int commandId = _cachedCommandId != 0 ? _cachedCommandId : DEFAULT_COMMAND_ID;
        int actionId = _cachedActionId != 0 ? _cachedActionId : DEFAULT_ACTION_ID;
        byte flag = _cachedFlag != 0 ? _cachedFlag : DEFAULT_FLAG;
        
        // ========================================================
        // VALIDATE REQUIREMENTS
        // ========================================================
        if (casterActorRef == 0)
        {
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Cannot cast: No source actor available.", _logger.ColorRed);
            return false;
        }
        
        if (targetStructPtr == 0)
        {
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Cannot cast: No target position available.", _logger.ColorRed);
            return false;
        }
        
        // ========================================================
        // EXECUTE SPELL
        // ========================================================
        try
        {
            // Inject cached VTable into our TargetStruct buffer if we created it ourselves
            // (targetStructPtr == targetBuffer means we own it)
            if (targetStructPtr == (long)targetBuffer)
            {
                if (_cachedTargetVTable != 0)
                {
                    var targetStruct = (TargetStruct*)targetBuffer;
                    targetStruct->VTable = _cachedTargetVTable;
                    _logger.WriteLine($"[{_modId}] [CastSpell] Injected VTable 0x{_cachedTargetVTable:X} into TargetStruct", _logger.ColorYellow);
                }
                else
                {
                    // No VTable available - cannot cast without it
                    _logger.WriteLine($"[{_modId}] [CastSpell] ERROR: No VTable available. Please cast a spell normally first to capture the VTable.", _logger.ColorRed);
                    _logger.WriteLine($"[{_modId}] [CastSpell] The TargetStruct requires a valid VTable pointer. Cast any spell (Fire, etc.) to capture it.", _logger.ColorRed);
                    return false;
                }
            }
            
            _logger.WriteLine($"[{_modId}] [CastSpell] Calling SetupMagic: MagicId={request.MagicId}, ActorRef=0x{casterActorRef:X}, TargetPtr=0x{targetStructPtr:X}, CmdId={commandId}, ActId={actionId}, Flag={flag}", _logger.ColorYellow);
            
            // Setup the magic struct
            _setupMagicHook!.OriginalFunction(
                (long)magicBuffer, 
                request.MagicId, 
                casterActorRef, 
                targetStructPtr, 
                commandId, 
                actionId, 
                flag
            );
            
            _logger.WriteLine($"[{_modId}] [CastSpell] SetupMagic completed successfully", _logger.ColorGreen);
            
            // Get executor client
            long executorClient = *(long*)(_baseAddress + GlobalOffsets.BattleMagicExecutor);
            _logger.WriteLine($"[{_modId}] [CastSpell] ExecutorClient from global: 0x{executorClient:X}, Cached: 0x{_cachedExecutorClient:X}", _logger.ColorYellow);
            
            if (executorClient == 0) executorClient = _cachedExecutorClient;
            
            if (executorClient != 0)
            {
                _logger.WriteLine($"[{_modId}] [CastSpell] Calling InsertNewMagic with executor 0x{executorClient:X}", _logger.ColorYellow);
                _castMagicWrapper!((long)executorClient, (long)magicBuffer);
                _logger.WriteLine($"[{_modId}] [CastSpell] InsertNewMagic completed successfully", _logger.ColorGreen);
                return true;
            }
            
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Cannot cast: No executor client available", _logger.ColorRed);
            return false;
        }
        catch (Exception ex)
        {
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Error casting spell: {ex.Message}", _logger.ColorRed);
            return false;
        }
    }
    
    /// <summary>
    /// Enqueue modifications for a magic ID without casting.
    /// The modifications will be applied when the magic is cast by the game.
    /// </summary>
    public void EnqueueModifications(int magicId, List<MagicModification> modifications)
    {
        var modEntries = ConvertToMagicModEntries(modifications);
        _processor.EnqueueModifications(magicId, modEntries);
    }
    
    // ============================================================
    // HOOK IMPLEMENTATIONS
    // ============================================================
    
    private long SetupMagicImpl(long battleMagicPtr, int magicId, long casterActorRef, long positionStruct, int commandId, int actionID, byte flag)
    {
        // Cache the context from normal game magic casts
        _cachedCasterActorRef = casterActorRef;
        _cachedPositionStruct = positionStruct;
        _cachedCommandId = commandId;
        _cachedActionId = actionID;
        _cachedFlag = flag;
        
        // Capture VTable from game's TargetStruct (first time only)
        if (_cachedTargetVTable == 0 && positionStruct != 0)
        {
            var targetStruct = (TargetStruct*)positionStruct;
            _cachedTargetVTable = targetStruct->VTable;
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Captured TargetStruct VTable: 0x{_cachedTargetVTable:X}", _logger.ColorGreen);
        }
        
        return _setupMagicHook!.OriginalFunction(battleMagicPtr, magicId, casterActorRef, positionStruct, commandId, actionID, flag);
    }
    
    private char CastMagicImpl(long a1, long unkMagicStructPtr)
    {
        if (!_hasMagicContext)
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Captured Magic Context", _logger.ColorGreen);
        
        _cachedExecutorClient = a1;
        _hasMagicContext = true;
        
        return _castMagicHook!.OriginalFunction(a1, unkMagicStructPtr);
    }
    
    /// <summary>
    /// Hook for UnkTargetStruct::Create - captures VTable from newly created TargetStructs.
    /// This allows us to get the VTable without the player needing to cast a spell first.
    /// </summary>
    private long* UnkTargetStructCreateImpl(long manager, long* outResult)
    {
        // Call original function first
        var result = _unkTargetStructCreateHook!.OriginalFunction(manager, outResult);
        
        // Capture VTable from the created struct (first time only)
        if (_cachedTargetVTable == 0 && result != null && *result != 0)
        {
            // The result points to the created TargetStruct
            // VTable is at offset 0x00
            var createdStruct = (TargetStruct*)(*result);
            if (createdStruct->VTable != 0)
            {
                _cachedTargetVTable = createdStruct->VTable;
                _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Captured VTable from UnkTargetStruct::Create: 0x{_cachedTargetVTable:X}", _logger.ColorGreen);
            }
        }
        
        return result;
    }

    private char FireMagicProjectileImpl(long magicManagerPtr, long projectileDataPtr)
    {
        if (magicManagerPtr != 0)
        {
            int shotType = MagicManagerHelper.GetShotTypeFromManager(magicManagerPtr);
            
            if (shotType == (int)MagicShotType.Charged && OnChargedShotDetected != null && GetActiveEikon != null)
            {
                int activeEikon = GetActiveEikon();
                if (OnChargedShotDetected(activeEikon))
                {
                    _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Suppressing Charged Shot for Eikon {activeEikon}", _logger.ColorYellow);
                    return (char)0;
                }
            }
        }

        return _fireMagicProjectileHook!.OriginalFunction(magicManagerPtr, projectileDataPtr);
    }
    
    // ============================================================
    // HELPERS
    // ============================================================
    
    private List<MagicModEntry> ConvertToMagicModEntries(List<MagicModification> modifications)
    {
        var entries = new List<MagicModEntry>();
        
        foreach (var mod in modifications)
        {
            // Skip AddOperation entries - they don't need to be injected
            // The individual AddProperty entries contain all the data needed
            if (mod.Type == MagicModificationType.AddOperation)
            {
                continue;
            }
            
            var entry = new MagicModEntry
            {
                Enabled = true,
                OpType = mod.operationId,
                PropertyId = mod.PropertyId,
                TargetOperationGroupId = mod.OperationGroupId,
                InjectAfterOp = mod.InjectAfterOp  // Propagate injection timing
            };
            
            // Set the value based on type
            SetEntryValue(entry, mod.Value);
            
            // Set action-specific flags
            switch (mod.Type)
            {
                case MagicModificationType.SetProperty:
                    entry.IsInjection = false;
                    entry.DisableOp = false;
                    break;
                case MagicModificationType.RemoveProperty:
                    entry.DisableOp = true;
                    entry.IsInjection = false;
                    break;
                case MagicModificationType.AddProperty:
                    entry.IsInjection = true;  // Inject a new property
                    entry.DisableOp = false;
                    break;
                // AddOperation is skipped at the start of the loop
                case MagicModificationType.RemoveOperation:
                    entry.DisableOp = true;
                    entry.PropertyId = -1;  // Block ALL properties of this operation
                    entry.IsInjection = false;
                    break;
            }
            
            entries.Add(entry);
        }
        
        return entries;
    }
    
    private static void SetEntryValue(MagicModEntry entry, object? value)
    {
        if (value is int intVal)
        {
            entry.UseFloat = false;
            entry.IntValue = intVal;
        }
        else if (value is float floatVal)
        {
            entry.UseFloat = true;
            entry.FloatValue = floatVal;
        }
        else if (value is bool boolVal)
        {
            entry.UseFloat = false;
            entry.IntValue = boolVal ? 1 : 0;
        }
        else if (value is System.Numerics.Vector3 vec3Val)
        {
            entry.UseVec3 = true;
            entry.Vec3X = vec3Val.X;
            entry.Vec3Y = vec3Val.Y;
            entry.Vec3Z = vec3Val.Z;
        }
    }
    
    // ============================================================
    // BUFFER POOL MANAGEMENT
    // ============================================================
    
    /// <summary>
    /// Allocates a fresh magic struct buffer for a single cast.
    /// The game may hold references to these buffers, so we can't reuse them immediately.
    /// Old buffers are cleaned up when the pool exceeds MAX_BUFFER_POOL_SIZE.
    /// </summary>
    private IntPtr AllocateMagicBuffer()
    {
        // Clean up old buffers if pool is too large
        CleanupBufferPoolIfNeeded();
        
        // Allocate fresh buffer
        IntPtr buffer = Marshal.AllocHGlobal(MAGIC_STRUCT_SIZE);
        
        // Zero-initialize
        for (int i = 0; i < MAGIC_STRUCT_SIZE; i++)
            *((byte*)buffer + i) = 0;
        
        // Track for later cleanup
        _allocatedMagicBuffers.Add(buffer);
        
        return buffer;
    }
    
    /// <summary>
    /// Allocates a fresh target struct buffer for a single cast.
    /// </summary>
    private IntPtr AllocateTargetBuffer()
    {
        // Clean up old buffers if pool is too large
        CleanupBufferPoolIfNeeded();
        
        // Allocate fresh buffer
        IntPtr buffer = Marshal.AllocHGlobal(TARGET_STRUCT_SIZE);
        
        // Zero-initialize
        for (int i = 0; i < TARGET_STRUCT_SIZE; i++)
            *((byte*)buffer + i) = 0;
        
        // Track for later cleanup
        _allocatedTargetBuffers.Add(buffer);
        
        return buffer;
    }
    
    /// <summary>
    /// Cleans up old buffers when pool exceeds the max size.
    /// We keep the most recent buffers as the game may still be using them.
    /// Only free the oldest buffers that are likely no longer in use.
    /// </summary>
    private void CleanupBufferPoolIfNeeded()
    {
        // Only cleanup if we exceed the max pool size
        if (_allocatedMagicBuffers.Count > MAX_BUFFER_POOL_SIZE)
        {
            // Free the oldest half of the buffers
            int toFree = _allocatedMagicBuffers.Count / 2;
            for (int i = 0; i < toFree; i++)
            {
                Marshal.FreeHGlobal(_allocatedMagicBuffers[i]);
            }
            _allocatedMagicBuffers.RemoveRange(0, toFree);
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Cleaned up {toFree} old magic buffers", _logger.ColorYellow);
        }
        
        if (_allocatedTargetBuffers.Count > MAX_BUFFER_POOL_SIZE)
        {
            int toFree = _allocatedTargetBuffers.Count / 2;
            for (int i = 0; i < toFree; i++)
            {
                Marshal.FreeHGlobal(_allocatedTargetBuffers[i]);
            }
            _allocatedTargetBuffers.RemoveRange(0, toFree);
            _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Cleaned up {toFree} old target buffers", _logger.ColorYellow);
        }
    }
    
    // ============================================================
    // STATE MANAGEMENT
    // ============================================================
    
    public void Reset()
    {
        _hasMagicContext = false;
        _cachedExecutorClient = 0;
        _cachedCasterActorRef = 0;
        _cachedPositionStruct = 0;
        _cachedCommandId = 0;
        _cachedActionId = 0;
        _cachedFlag = 0;
        
        _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Reset", _logger.ColorYellow);
    }
    
    public void Dispose()
    {
        // Free all allocated buffers
        foreach (var buffer in _allocatedMagicBuffers)
        {
            if (buffer != IntPtr.Zero)
                Marshal.FreeHGlobal(buffer);
        }
        _allocatedMagicBuffers.Clear();
        
        foreach (var buffer in _allocatedTargetBuffers)
        {
            if (buffer != IntPtr.Zero)
                Marshal.FreeHGlobal(buffer);
        }
        _allocatedTargetBuffers.Clear();
        
        _logger.WriteLine($"[{_modId}] [MagicCastingEngine] Disposed - freed all buffers", _logger.ColorYellow);
    }
}
