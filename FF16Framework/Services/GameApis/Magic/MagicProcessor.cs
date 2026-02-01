using System.Numerics;
using Reloaded.Hooks.Definitions;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using FF16Framework;
using FF16Framework.Services.GameApis.Magic.MagicFile;
using FF16Framework.Services.GameApis.Actor;
using FF16Framework.Faith.Structs;
using FF16Framework.Interfaces.GameApis.Magic;
using FF16Framework.Services.GameApis.Magic.MagicFile;

namespace FF16Framework.Services.GameApis.Magic;

/// <summary>
/// Handles magic file processing, property fuzzing, and injection logic.
/// Separated from MagicGameSystem to follow single responsibility principle.
/// </summary>
internal unsafe class MagicProcessor
{
    // ============================================================
    // DELEGATES
    // ============================================================
    
    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate void MagicUnkExecuteDelegate(long magicFileInstance, int opType, int propertyId, long dataPtr);

    [Reloaded.Hooks.Definitions.X64.Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    public delegate long GenericMagicDelegate(long a1, long a2, long a3, long a4);

    // ============================================================
    // CONSTANTS
    // ============================================================
    
    private const string MAGIC_UNK_EXECUTE_SIG = "48 89 5C 24 08 48 89 74 24 10 57 48 83 EC 20 49 8B F9 41 8B D8 8B F2 41 83 F8 02 75 2F 48 8D 59 10 48 8D B9 10 01 00 00 EB 1B 48 8B 0B 48 85 C9 74 0F 39 71 20 75 0A";
    private const string MAGIC_FILE_PROCESS_SIG = "48 8B C4 48 89 58 ?? 55 56 57 41 54 41 55 41 56 41 57 48 8D 68 ?? 48 81 EC ?? ?? ?? ?? C5 F8 29 70 ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 45 ?? 45 33 F6 48 89 55";
    private const string MAGIC_FILE_HANDLE_SUB_ENTRY_SIG = "40 55 53 56 57 41 54 41 56 41 57 48 8B EC 48 83 EC ?? 48 8D 59";

    // ============================================================
    // HOOKS
    // ============================================================
    
    private IHook<MagicUnkExecuteDelegate>? _magicUnkExecuteHook;
    private IHook<GenericMagicDelegate>? _magicFileProcessHook;
    private IHook<GenericMagicDelegate>? _magicFileHandleSubEntryHook;

    // ============================================================
    // STATE - Queues and Trackers
    // ============================================================
    
    private Dictionary<(int magicId, int groupId), Queue<List<MagicModEntry>>> _groupedQueues = new();
    private List<MagicModEntry>? _activeInstanceEntries = null;
    private int _activeInstanceMagicId = 0;
    
    private Dictionary<int, int> _opInstanceTracker = new();
    private Dictionary<long, int> _propInstanceTracker = new();
    private int _lastOpType = -1;
    private List<MagicModEntry> _pendingInjections = new();
    private bool _isProcessingInjections = false;

    // ============================================================
    // DEPENDENCIES
    // ============================================================
    
    private readonly ILogger _logger;
    private readonly string _modId;
    private readonly IStartupScanner _scanner;
    private Config _configuration;

    // ============================================================
    // CONSTRUCTOR
    // ============================================================
    
    public MagicProcessor(ILogger logger, string modId, Config configuration, IStartupScanner scanner)
    {
        _logger = logger;
        _modId = modId;
        _configuration = configuration;
        _scanner = scanner;
    }

    // ============================================================
    // INITIALIZATION
    // ============================================================
    
    /// <summary>
    /// Sets up hooks for magic file processing.
    /// </summary>
    public void SetupScans(IReloadedHooks hooks)
    {
        // Hook: MagicUnkExecute - Main property interception point
        // Called for each property value in a .magic file during execution.
        // Allows fuzzing/overriding values and logging property data.
        _scanner.AddScan(MAGIC_UNK_EXECUTE_SIG, address =>
        {
            _magicUnkExecuteHook = hooks.CreateHook<MagicUnkExecuteDelegate>(MagicUnkExecuteImpl, address).Activate();
            _logger.WriteLine($"[{_modId}] [MagicProcessor] Hooked MagicUnkExecute at 0x{address:X}", _logger.ColorGreen);
        });

        // Hook: MagicFile::Process - Operation group lifecycle management
        // Called once per operation group. Resets all trackers at start.
        // After original execution, processes any remaining pending injections
        // and end-of-group injections (InjectAfterOp == -1).
        _scanner.AddScan(MAGIC_FILE_PROCESS_SIG, address =>
        {
            _magicFileProcessHook = hooks.CreateHook<GenericMagicDelegate>(MagicFileProcessImpl, address).Activate();
            _logger.WriteLine($"[{_modId}] [MagicProcessor] Hooked MagicFile::Process at 0x{address:X}", _logger.ColorGreen);
        });

        // Hook: MagicFile::HandleSubEntry - Operation transition detection
        // Called when processing each sub-entry (individual operation).
        // Detects opType changes and triggers CheckOpChange to process
        // pending injections and queue new ones based on InjectAfterOp.
        _scanner.AddScan(MAGIC_FILE_HANDLE_SUB_ENTRY_SIG, address =>
        {
            _magicFileHandleSubEntryHook = hooks.CreateHook<GenericMagicDelegate>(MagicFileHandleSubEntryImpl, address).Activate();
            _logger.WriteLine($"[{_modId}] [MagicProcessor] Hooked MagicFile::HandleSubEntry at 0x{address:X}", _logger.ColorGreen);
        });
    }

    // ============================================================
    // PUBLIC API
    // ============================================================
    
    /// <summary>
    /// Enqueues modifications to be applied when a magic spell is processed.
    /// </summary>
    public void EnqueueModifications(int magicId, List<MagicModEntry> entries)
    {
        var grouped = entries.GroupBy(e => e.TargetOperationGroupId);
        
        foreach (var group in grouped)
        {
            var key = (magicId, group.Key);
            if (!_groupedQueues.TryGetValue(key, out var queue))
            {
                queue = new Queue<List<MagicModEntry>>();
                _groupedQueues[key] = queue;
            }
            queue.Enqueue(group.ToList());
            _logger.WriteLine($"[{_modId}] [MagicProcessor] Enqueued {group.Count()} entries for Magic {magicId} Group {group.Key}", _logger.ColorYellow);
        }
    }

    /// <summary>
    /// Updates the configuration reference.
    /// </summary>
    public void UpdateConfiguration(Config configuration)
    {
        _configuration = configuration;
    }

    // ============================================================
    // HOOK IMPLEMENTATIONS
    // ============================================================
    
    /// <summary>
    /// MagicFile::Process hook implementation.
    /// 
    /// This function manages the complete lifecycle of processing an operation group:
    /// 1. ENTRY: Resets all state trackers (operation counters, property counters, pending injections)
    /// 2. EXECUTION: Calls the original function which processes all operations in the group
    /// 3. EXIT: Processes any remaining pending injections that didn't trigger during execution
    /// 4. CLEANUP: Injects end-of-group properties (InjectAfterOp == -1) and clears active entries
    /// 
    /// Flow:
    /// MagicFile::Process (START)
    ///   ├─ Reset trackers
    ///   ├─ Original execution (triggers HandleSubEntry and MagicUnkExecute hooks)
    ///   ├─ Process remaining pending injections
    ///   └─ Inject end-of-group properties
    /// </summary>
    private long MagicFileProcessImpl(long a1, long a2, long a3, long a4)
    {
        // Reset trackers for new process call
        _opInstanceTracker.Clear();
        _propInstanceTracker.Clear();
        _lastOpType = -1;
        _pendingInjections.Clear();
        _activeInstanceEntries = null;
        _activeInstanceMagicId = 0;

        try
        {
            long result = _magicFileProcessHook!.OriginalFunction(a1, a2, a3, a4);

            // Process remaining injections at end of group
            if (_pendingInjections.Count > 0)
            {
                _isProcessingInjections = true;
                try
                {
                    foreach (var entry in _pendingInjections)
                    {
                        _logger.WriteLine($"[{_modId}] [INJECTOR] Injecting Op {entry.OpType} Prop {entry.PropertyId} AFTER Op {_lastOpType} (End of Group)", _logger.ColorGreen);
                        PerformInjection(a1, entry);
                    }
                    _pendingInjections.Clear();
                }
                finally
                {
                    _isProcessingInjections = false;
                }
            }

            // Inject end-of-group properties
            if (_activeInstanceEntries != null)
            {
                foreach (var entry in _activeInstanceEntries)
                {
                    if (entry.Enabled && entry.IsInjection && entry.InjectAfterOp == -1)
                    {
                        if (entry.IsOperationOnly)
                        {
                            _logger.WriteLine($"[{_modId}] [INJECTOR] Processing AddOperation {entry.OpType} at END of Group", _logger.ColorBlue);
                        }
                        PerformInjection(a1, entry);
                    }
                }
            }

            return result;
        }
        finally
        {
            _activeInstanceEntries = null;
            _activeInstanceMagicId = 0;
        }
    }

    private long MagicFileHandleSubEntryImpl(long a1, long a2, long a3, long a4)
    {
        int opType = (int)a2;
        CheckOpChange(a1, opType);
        return _magicFileHandleSubEntryHook!.OriginalFunction(a1, a2, a3, a4);
    }

    private void MagicUnkExecuteImpl(long magicFileInstance, int opType, int propertyId, long dataPtr)
    {
        var (magicId, groupId) = ResolveIds(magicFileInstance);

        // Activate queued entries
        if (_activeInstanceEntries == null && (magicId != 0 || groupId != 0))
        {
            var key = (magicId, groupId);
            if (_groupedQueues.TryGetValue(key, out var queue) && queue.Count > 0)
            {
                _activeInstanceEntries = queue.Dequeue();
                _activeInstanceMagicId = magicId;
                _logger.WriteLine($"[{_modId}] [ACTIVATE] Linked {_activeInstanceEntries.Count} mods to Magic {magicId} Group {groupId}", _logger.ColorGreen);
            }
        }

        CheckOpChange(magicFileInstance, opType);

        // Track occurrences
        long propKey = ((long)opType << 32) | (uint)propertyId;
        int propOccurrence = _propInstanceTracker.GetValueOrDefault(propKey, 0);
        _propInstanceTracker[propKey] = propOccurrence + 1;

        int opOccurrence = _opInstanceTracker.GetValueOrDefault(opType, 0) - 1;
        if (opOccurrence < 0) opOccurrence = 0;
        
        // Get active entries for current instance
        var activeEntries = _activeInstanceEntries;
        if (activeEntries == null)
        {
            // Execute original and exit if no active modifications
            _magicUnkExecuteHook!.OriginalFunction(magicFileInstance, opType, propertyId, dataPtr);
            return;
        }

        // Check for DisableOp
        foreach (var entry in activeEntries)
        {
            if (entry.Enabled && entry.DisableOp && entry.OpType == opType)
            {
                int targetOcc = (entry.PropertyId == -1) ? opOccurrence : propOccurrence;
                if (EntryMatchesContext(entry, magicId, groupId, targetOcc) && (entry.PropertyId == -1 || entry.PropertyId == propertyId))
                {
                    _logger.WriteLine($"[{_modId}] [MAGIC] {magicId} Group {groupId} Op {opType} Prop {propertyId} DISABLED (Occ {targetOcc})", _logger.ColorRed);
                    return;
                }
            }
        }

        long valuePtr = *(long*)(dataPtr + 8);

        // Apply magic mod override (Fuzzer functionality removed from global config)
        var (isFuzzed, activeEntry, originalValue) = ApplyMagicModOverride(
            activeEntries, true, opType, propertyId, 
            magicId, groupId, propOccurrence, valuePtr);

        // Log property value if enabled
        if (false)
        {
            LogPropertyValue(magicId, groupId, opType, propertyId, valuePtr);
        }

        // Execute original
        _magicUnkExecuteHook!.OriginalFunction(magicFileInstance, opType, propertyId, dataPtr);

        // Restore original value
        if (isFuzzed && activeEntry != null)
        {
            RestoreOriginalValue(activeEntry, valuePtr, originalValue);
        }
    }

    // ============================================================
    // INTERNAL HELPERS
    // ============================================================
    
    /// <summary>
    /// Performs a property injection by creating a fake data structure and calling MagicUnkExecuteImpl.
    /// This allows adding new properties to an operation group that don't exist in the original .magic file.
    /// </summary>
    /// <param name="magicFileInstance">The magic file instance to inject into.</param>
    /// <param name="entry">The magic mod entry containing the property to inject.</param>
    private void PerformInjection(long magicFileInstance, MagicModEntry entry)
    {
        // Create stack-allocated buffers for the fake property data
        byte* buffer = stackalloc byte[16];
        long* fakeData = stackalloc long[2];
        fakeData[0] = 0;
        fakeData[1] = (long)buffer;

        // Write the value based on the entry's type
        if (entry.UseVec3)
            *(Vector3*)buffer = new Vector3(entry.Vec3X, entry.Vec3Y, entry.Vec3Z);
        else if (entry.UseFloat)
            *(float*)buffer = entry.FloatValue;
        else
            *(int*)buffer = entry.IntValue;

        // Inject by calling the hook implementation directly
        _logger.WriteLine($"[{_modId}] [INJECTOR] Injecting property: Op {entry.OpType} Prop {entry.PropertyId} = {GetValueString(entry)}", _logger.ColorGreen);
        MagicUnkExecuteImpl(magicFileInstance, entry.OpType, entry.PropertyId, (long)fakeData);
    }
    
    private static string GetValueString(MagicModEntry entry)
    {
        if (entry.UseVec3)
            return $"<{entry.Vec3X}, {entry.Vec3Y}, {entry.Vec3Z}>";
        if (entry.UseFloat)
            return entry.FloatValue.ToString("F2");
        return entry.IntValue.ToString();
    }

    /// <summary>
    /// Handles operation type transitions within a magic file processing.
    /// 
    /// When the opType changes, this method:
    /// 1. Executes any pending injections that were queued for "after" the previous operation
    /// 2. Updates the operation occurrence counter
    /// 3. Queues new injections for entries that should run after the NEW operation
    /// 
    /// The _isProcessingInjections flag prevents infinite recursion since injections
    /// also call MagicUnkExecuteImpl which calls this method.
    /// </summary>
    /// <param name="magicFileInstance">The magic file instance being processed.</param>
    /// <param name="opType">The new operation type.</param>
    private void CheckOpChange(long magicFileInstance, int opType)
    {
        if (_isProcessingInjections) return;
        if (opType == _lastOpType) return;

        var (magicId, groupId) = ResolveIds(magicFileInstance);
        
        // Process pending injections from previous operation
        if (_pendingInjections.Count > 0)
        {
            _isProcessingInjections = true;
            try
            {
                foreach (var entry in _pendingInjections)
                {
                    _logger.WriteLine($"[{_modId}] [INJECTOR] Injecting Op {entry.OpType} Prop {entry.PropertyId} AFTER Op {_lastOpType} in Magic {magicId} Group {groupId}", _logger.ColorGreen);
                    PerformInjection(magicFileInstance, entry);
                }
                _pendingInjections.Clear();
            }
            finally
            {
                _isProcessingInjections = false;
            }
        }

        // Update state for new operation
        _lastOpType = opType;
        int currentOpOccurrence = _opInstanceTracker.GetValueOrDefault(opType, 0);
        _opInstanceTracker[opType] = currentOpOccurrence + 1;

        // Check for injections after this operation
        var activeEntries = _activeInstanceEntries;
        
        if (activeEntries != null)
        {
            foreach (var entry in activeEntries)
            {
                if (entry.Enabled && entry.IsInjection && entry.InjectAfterOp == opType && EntryMatchesContext(entry, magicId, groupId, currentOpOccurrence))
                {
                    _pendingInjections.Add(entry);
                    _logger.WriteLine($"[{_modId}] [QUEUE_INJECT] Queued Op {entry.OpType} to inject after Op {opType} (Occ {currentOpOccurrence})", _logger.ColorBlue);
                }
            }
        }
    }

    /// <summary>
    /// Checks if a fuzzer entry matches the current execution context.
    /// Used to filter entries based on MagicId, GroupId, and occurrence number.
    /// A value of -1 in an entry field means "match any".
    /// </summary>
    /// <param name="entry">The magic mod entry to check.</param>
    /// <param name="magicId">Current magic spell ID.</param>
    /// <param name="groupId">Current operation group ID.</param>
    /// <param name="occurrence">Current occurrence number (0-based).</param>
    /// <returns>True if the entry matches the current context.</returns>
    private static bool EntryMatchesContext(MagicModEntry entry, int magicId, int groupId, int occurrence)
    {
        if (entry.TargetMagicId != -1 && entry.TargetMagicId != magicId) return false;
        if (entry.TargetOperationGroupId != -1 && entry.TargetOperationGroupId != groupId) return false;
        if (entry.Occurrence != -1 && entry.Occurrence != occurrence) return false;
        return true;
    }

    /// <summary>
    /// Attempts to apply a fuzzer override to a property value in memory.
    /// 
    /// This method:
    /// 1. Finds the first matching enabled fuzzer entry for the property
    /// 2. Saves the original value from memory
    /// 3. Writes the override value to memory
    /// 4. Returns info needed to restore the original value later
    /// 
    /// The original value is restored after the game processes the property
    /// to prevent memory corruption if the game expects the original value elsewhere.
    /// </summary>
    /// <returns>Tuple of (was fuzzed, matching entry, original value tuple)</returns>
    private (bool isFuzzed, MagicModEntry? entry, (float f, int i, Vector3 v) original) ApplyMagicModOverride(
        List<MagicModEntry> entries, bool enabled, int opType, int propertyId,
        int magicId, int groupId, int occurrence, long valuePtr)
    {
        if (!enabled) return (false, null, default);

        foreach (var entry in entries)
        {
            if (!entry.Enabled || entry.IsInjection || entry.DisableOp) continue;
            if (entry.PropertyId != propertyId) continue;
            if (entry.OpType != -1 && entry.OpType != opType) continue;
            if (!EntryMatchesContext(entry, magicId, groupId, occurrence)) continue;

            string contextStr = $"[Magic {magicId} Group {groupId}]";
            (float f, int i, Vector3 v) original = default;

            if (entry.UseVec3)
            {
                original.v = *(Vector3*)valuePtr;
                *(Vector3*)valuePtr = new Vector3(entry.Vec3X, entry.Vec3Y, entry.Vec3Z);
                _logger.WriteLine($"[{_modId}] [FUZZER] {contextStr} Op {opType} Prop {propertyId} (Vec3) OVERRIDE: {original.v} -> {*(Vector3*)valuePtr} (Occ {occurrence})", _logger.ColorYellow);
            }
            else if (entry.UseFloat)
            {
                original.f = *(float*)valuePtr;
                *(float*)valuePtr = entry.FloatValue;
                _logger.WriteLine($"[{_modId}] [FUZZER] {contextStr} Op {opType} Prop {propertyId} (Float) OVERRIDE: {original.f:F4} -> {entry.FloatValue:F4} (Occ {occurrence})", _logger.ColorYellow);
            }
            else
            {
                original.i = *(int*)valuePtr;
                *(int*)valuePtr = entry.IntValue;
                _logger.WriteLine($"[{_modId}] [FUZZER] {contextStr} Op {opType} Prop {propertyId} (Int) OVERRIDE: {original.i} -> {entry.IntValue} (Occ {occurrence})", _logger.ColorYellow);
            }
            return (true, entry, original);
        }
        return (false, null, default);
    }

    /// <summary>
    /// Restores the original value in memory after fuzzing.
    /// Called after the original function processes the fuzzed value.
    /// </summary>
    /// <param name="entry">The magic mod entry that was applied.</param>
    /// <param name="valuePtr">Pointer to the value in memory.</param>
    /// <param name="original">The original value tuple to restore.</param>
    private void RestoreOriginalValue(MagicModEntry entry, long valuePtr, (float f, int i, Vector3 v) original)
    {
        if (entry.UseVec3)
            *(Vector3*)valuePtr = original.v;
        else if (entry.UseFloat)
            *(float*)valuePtr = original.f;
        else
            *(int*)valuePtr = original.i;
    }

    /// <summary>
    /// Logs a property value for debugging and reverse engineering purposes.
    /// If the property is known (defined in MagicProperties), logs with the property name and correct type.
    /// If unknown, logs all possible interpretations (int, float, vec3).
    /// </summary>
    /// <param name="magicId">Current magic spell ID.</param>
    /// <param name="groupId">Current operation group ID.</param>
    /// <param name="opType">Current operation type.</param>
    /// <param name="propertyId">The property ID being logged.</param>
    /// <param name="valuePtr">Pointer to the property value in memory.</param>
    private void LogPropertyValue(int magicId, int groupId, int opType, int propertyId, long valuePtr)
    {
        string contextStr = $"[Magic {magicId} Group {groupId}]";

        if (MagicProperties.Definitions.TryGetValue(propertyId, out var info))
        {
            string valStr = info.Type switch
            {
                MagicValueType.Float => $"float={*(float*)valuePtr:F4}",
                MagicValueType.Int => $"int={*(int*)valuePtr} (0x{*(int*)valuePtr:X})",
                MagicValueType.Bool => $"bool={(*(int*)valuePtr != 0)}",
                MagicValueType.Vec3Float => $"vec3<f>=({(*(Vector3*)valuePtr).X:F4}, {(*(Vector3*)valuePtr).Y:F4}, {(*(Vector3*)valuePtr).Z:F4})",
                MagicValueType.Vec3Int => $"vec3<i>=({((int*)valuePtr)[0]}, {((int*)valuePtr)[1]}, {((int*)valuePtr)[2]})",
                _ => "unknown"
            };
            _logger.WriteLine($"[{_modId}] [PROP_LOG] {contextStr} Op {opType} Prop {propertyId} ({info.Name}): {valStr}", _logger.ColorBlue);
        }
        else
        {
            float fVal = *(float*)valuePtr;
            int iVal = *(int*)valuePtr;
            var v = *(Vector3*)valuePtr;
            int* iVec = (int*)valuePtr;
            
            _logger.WriteLine($"[{_modId}] [PROP_LOG] {contextStr} Op {opType} Prop {propertyId} (UNKNOWN): " +
                $"int={iVal}, float={fVal:F4}, vec3<f>=({v.X:F4}, {v.Y:F4}, {v.Z:F4}), vec3<i>=({iVec[0]}, {iVec[1]}, {iVec[2]})", _logger.ColorYellow);
        }
    }

    /// <summary>
    /// Resolves MagicId and GroupId from a MagicFileInstance pointer.
    /// </summary>
    private static (int magicId, int groupId) ResolveIds(long instance)
    {
        if (!PointerValidation.IsValidPointer(instance)) return (0, 0);
        
        try
        {
            var magicFile = (MagicFileInstance*)instance;
            return (magicFile->MagicId, magicFile->GroupId);
        }
        catch { return (0, 0); }
    }
}
