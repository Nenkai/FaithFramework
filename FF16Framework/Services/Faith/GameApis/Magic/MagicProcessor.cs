using System.Diagnostics;
using System.Numerics;
using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;
using FF16Framework;
using FF16Framework.Faith.Hooks;
using FF16Framework.Services.Faith.GameApis.Actor;
using FF16Framework.Faith.Structs;
using FF16Framework.Interfaces.GameApis.Magic;
using FF16Tools.Files.Magic;
using FF16Tools.Files.Magic.Factories;

namespace FF16Framework.Services.Faith.GameApis.Magic;

/// <summary>
/// Handles magic file processing, property modification, and injection logic.
/// Separated from MagicGameSystem to follow single responsibility principle.
/// </summary>
internal unsafe class MagicProcessor
{
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
    private readonly MagicHooks _magicHooks;
    private FrameworkConfig _frameworkConfig;

    // ============================================================
    // CONSTRUCTOR
    // ============================================================
    
    public MagicProcessor(ILogger logger, string modId, FrameworkConfig frameworkConfig, MagicHooks magicHooks)
    {
        _logger = logger;
        _modId = modId;
        _frameworkConfig = frameworkConfig;
        _magicHooks = magicHooks;
        
        // Register callbacks with MagicHooks
        _magicHooks.OnMagicFileUnkExecute = MagicUnkExecuteImpl;
        _magicHooks.OnMagicFileProcess = MagicFileProcessImpl;
        _magicHooks.OnMagicFileHandleSubEntry = MagicFileHandleSubEntryImpl;
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
        var sw = Stopwatch.StartNew();
        
        // Reset trackers for new process call
        _opInstanceTracker.Clear();
        _propInstanceTracker.Clear();
        _lastOpType = -1;
        _pendingInjections.Clear();
        _activeInstanceEntries = null;
        _activeInstanceMagicId = 0;

        try
        {
            long result = _magicHooks.MagicFile_ProcessHook!.OriginalFunction(a1, a2, a3, a4);

            // Process remaining injections at end of group
            if (_pendingInjections.Count > 0)
            {
                _isProcessingInjections = true;
                try
                {
                    foreach (var entry in _pendingInjections)
                    {
                        if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                        {
                            _logger.WriteLine($"[{_modId}] [INJECTOR] Injecting Op {entry.OpType} Prop {entry.PropertyId} AFTER Op {_lastOpType} (End of Group)", _logger.ColorGreen);
                        }
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
                        if (entry.IsOperationOnly && _frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
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
            if (_activeInstanceMagicId != 0)
            {
                _logger.WriteLine($"[{_modId}] [MagicProcessor] MagicId {_activeInstanceMagicId}: " +
                    $"total processing time [{sw.Elapsed.TotalMilliseconds:F3}ms]", _logger.ColorGreen);
            }
            _activeInstanceEntries = null;
            _activeInstanceMagicId = 0;
        }
    }

    private long MagicFileHandleSubEntryImpl(long a1, long a2, long a3, long a4)
    {
        int opType = (int)a2;
        
        // Activate queued entries early (HandleSubEntry fires before the first UnkExecute)
        var (magicId, groupId) = ResolveIds(a1);
        if (_activeInstanceEntries == null && (magicId != 0 || groupId != 0))
        {
            var key = (magicId, groupId);
            if (_groupedQueues.TryGetValue(key, out var queue) && queue.Count > 0)
            {
                _activeInstanceEntries = queue.Dequeue();
                _activeInstanceMagicId = magicId;
                if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                {
                    _logger.WriteLine($"[{_modId}] [ACTIVATE] Linked {_activeInstanceEntries.Count} mods to Magic {magicId} Group {groupId}", _logger.ColorGreen);
                }
            }
        }
        
        CheckOpChange(a1, opType);
        
        // Block HandleSubEntry for RemoveOperation entries.
        // Without this, the game initializes the operation's component with default values
        // (e.g., LinearProjectile with speed=0), which overrides/conflicts with injected
        // replacement operations. DisableOp in UnkExecute only blocks properties, not
        // component initialization.
        if (_activeInstanceEntries != null)
        {
            foreach (var entry in _activeInstanceEntries)
            {
                if (entry.Enabled && entry.DisableOp && entry.PropertyId == -1 && entry.OpType == opType)
                {
                    int opOccurrence = _opInstanceTracker.GetValueOrDefault(opType, 0) - 1;
                    if (opOccurrence < 0) opOccurrence = 0;
                    if (EntryMatchesContext(entry, magicId, groupId, opOccurrence))
                    {
                        if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                        {
                            _logger.WriteLine($"[{_modId}] [REMOVE_OP] Blocked HandleSubEntry for Op {opType} in Magic {magicId} Group {groupId}", _logger.ColorRed);
                        }
                        return 0;
                    }
                }
            }
        }
        
        return _magicHooks.MagicFile_HandleSubEntryHook!.OriginalFunction(a1, a2, a3, a4);
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
                if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                {
                    _logger.WriteLine($"[{_modId}] [ACTIVATE] Linked {_activeInstanceEntries.Count} mods to Magic {magicId} Group {groupId}", _logger.ColorGreen);
                }
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
        
        // Log property value before any processing
        long valuePtr = *(long*)(dataPtr + 8);
        if (_frameworkConfig.GameApis.MagicApi.EnablePropertyLogging)
        {
            LogPropertyValue(magicId, groupId, opType, propertyId, valuePtr);
        }
        
        if (activeEntries == null)
        {
            // Execute original and exit if no active modifications
            _magicHooks.MagicFile_UnkExecuteHook!.OriginalFunction(magicFileInstance, opType, propertyId, dataPtr);
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
                    if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                    {
                        _logger.WriteLine($"[{_modId}] [MAGIC] {magicId} Group {groupId} Op {opType} Prop {propertyId} DISABLED (Occ {targetOcc})", _logger.ColorRed);
                    }
                    return;
                }
            }
        }

        // Apply magic mod override (Fuzzer functionality removed from global config)
        var (isFuzzed, activeEntry, originalValue) = ApplyMagicModOverride(
            activeEntries, true, opType, propertyId, 
            magicId, groupId, propOccurrence, valuePtr);

        // Execute original
        _magicHooks.MagicFile_UnkExecuteHook!.OriginalFunction(magicFileInstance, opType, propertyId, dataPtr);

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

        
        if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
        {
            _logger.WriteLine($"[{_modId}] [INJECTOR] Injecting property: Op {entry.OpType} Prop {entry.PropertyId} = {GetValueString(entry)}", _logger.ColorGreen);
        }
        // Inject by calling the hook implementation directly
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
                    if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                    {
                        _logger.WriteLine($"[{_modId}] [INJECTOR] Injecting Op {entry.OpType} Prop {entry.PropertyId} AFTER Op {_lastOpType} in Magic {magicId} Group {groupId}", _logger.ColorGreen);
                    }
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
                    if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                    {
                        _logger.WriteLine($"[{_modId}] [QUEUE_INJECT] Queued Op {entry.OpType} to inject after Op {opType} (Occ {currentOpOccurrence})", _logger.ColorBlue);
                    }
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
                if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                {
                    _logger.WriteLine($"[{_modId}] [FUZZER] {contextStr} Op {opType} Prop {propertyId} (Vec3) OVERRIDE: {original.v} -> {*(Vector3*)valuePtr} (Occ {occurrence})", _logger.ColorYellow);
                }
            }
            else if (entry.UseFloat)
            {
                original.f = *(float*)valuePtr;
                *(float*)valuePtr = entry.FloatValue;
                if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                {
                    _logger.WriteLine($"[{_modId}] [FUZZER] {contextStr} Op {opType} Prop {propertyId} (Float) OVERRIDE: {original.f:F4} -> {entry.FloatValue:F4} (Occ {occurrence})", _logger.ColorYellow);
                }
            }
            else
            {
                original.i = *(int*)valuePtr;
                *(int*)valuePtr = entry.IntValue;
                if (_frameworkConfig.GameApis.MagicApi.EnableInjectionLogging)
                {
                    _logger.WriteLine($"[{_modId}] [FUZZER] {contextStr} Op {opType} Prop {propertyId} (Int) OVERRIDE: {original.i} -> {entry.IntValue} (Occ {occurrence})", _logger.ColorYellow);
                }
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
    /// If the property is known (defined in MagicPropertyType), logs with the property name and correct type.
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
        
        var propType = (MagicPropertyType)propertyId;
        string? propName = Enum.IsDefined(propType) ? propType.ToString() : null;
        
        if (propName != null && MagicPropertyValueTypeMapping.TypeToValueType.TryGetValue(propType, out var valueType))
        {
            string valStr = valueType switch
            {
                MagicPropertyValueType.Float => $"float={*(float*)valuePtr:F4}",
                MagicPropertyValueType.Int or MagicPropertyValueType.OperationGroupId => $"int={*(int*)valuePtr} (0x{*(int*)valuePtr:X})",
                MagicPropertyValueType.Bool or MagicPropertyValueType.Byte => $"bool={(*(int*)valuePtr != 0)}",
                MagicPropertyValueType.Vec3 => $"vec3=({(*(Vector3*)valuePtr).X:F4}, {(*(Vector3*)valuePtr).Y:F4}, {(*(Vector3*)valuePtr).Z:F4})",
                _ => "unknown"
            };
            _logger.WriteLine($"[{_modId}] [PROP_LOG] {contextStr} Op {opType} Prop {propertyId} ({propName}): {valStr}", _logger.ColorBlue);
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
        if (instance == 0) return (0, 0);
        
        try
        {
            var magicFile = (MagicFileInstance*)instance;
            return (magicFile->MagicId, magicFile->GroupId);
        }
        catch { return (0, 0); }
    }
}
