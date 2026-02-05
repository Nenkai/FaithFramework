using System.Collections.Concurrent;
using System.Numerics;
using Reloaded.Mod.Interfaces;
using FF16Framework.Faith.Hooks;
using FF16Framework.Faith.Structs;
using FF16Framework.Interfaces.GameApis.Magic;
using FF16Framework.Services.ResourceManager;
using FF16Tools.Files.Magic;
using FF16Tools.Files.Magic.Factories;
using FF16Tools.Files.Magic.Operations;

namespace FF16Framework.Services.GameApis.Magic;

/// <summary>
/// Represents a set of modifications registered by a mod.
/// </summary>
internal class RegisteredModificationSet
{
    public MagicWriterHandle Handle { get; }
    public string ModId { get; }
    public string MagicFilePath { get; }
    public int MagicId { get; }
    public List<MagicModification> Modifications { get; }
    public DateTime RegisteredAt { get; }
    
    public RegisteredModificationSet(MagicWriterHandle handle, string modId, string magicFilePath, int magicId, List<MagicModification> modifications)
    {
        Handle = handle;
        ModId = modId;
        MagicFilePath = magicFilePath;
        MagicId = magicId;
        Modifications = modifications;
        RegisteredAt = DateTime.Now;
    }
}

/// <summary>
/// MagicWriter manages persistent modifications to .magic files.
/// It listens for resource load events and automatically applies registered modifications
/// when the corresponding .magic file is loaded or reloaded by the game.
/// 
/// This enables mods to register their modifications once and have them automatically
/// applied whenever the game loads (or reloads) the magic files.
/// </summary>
public class MagicWriter : IMagicWriter, IDisposable
{
    private readonly ILogger _logger;
    private readonly ResourceManagerHooks _resourceManagerHooks;
    private readonly ResourceManagerService _resourceManagerService;
    
    // All registered modification sets, keyed by handle
    private readonly ConcurrentDictionary<MagicWriterHandle, RegisteredModificationSet> _registeredSets = new();
    
    // Index: MagicFilePath -> List of handles that affect that file
    private readonly ConcurrentDictionary<string, ConcurrentBag<MagicWriterHandle>> _fileToHandles = new();
    
    // Track which files have been modified (for logging/debugging)
    private readonly ConcurrentDictionary<string, int> _modificationCounts = new();
    
    private bool _disposed;
    
    /// <summary>
    /// Event raised when modifications are applied to a magic file.
    /// </summary>
    public event Action<string, int>? OnModificationsApplied;
    
    public MagicWriter(ILogger logger, ResourceManagerHooks resourceManagerHooks, ResourceManagerService resourceManagerService)
    {
        _logger = logger;
        _resourceManagerHooks = resourceManagerHooks;
        _resourceManagerService = resourceManagerService;
        
        // Subscribe to resource load events
        unsafe
        {
            _resourceManagerHooks.OnResourceLoaded += OnResourceLoaded;
        }
        
        _logger.WriteLine("[MagicWriter] Initialized and listening for .magic file loads", _logger.ColorGreen);
    }
    
    // ========================================
    // REGISTRATION API
    // ========================================
    
    /// <summary>
    /// Registers a set of modifications to be applied to a magic file.
    /// The modifications will be applied automatically when the file is loaded.
    /// If the file is already loaded, the modifications are applied immediately.
    /// </summary>
    /// <param name="modId">The ID of the mod registering the modifications.</param>
    /// <param name="magicId">The magic spell ID to modify.</param>
    /// <param name="modifications">The list of modifications to apply.</param>
    /// <param name="characterId">The character ID (folder name). Default is "c1001".</param>
    /// <param name="magicFileName">Optional magic file name. If null, uses characterId.</param>
    /// <returns>A handle that can be used to unregister the modifications.</returns>
    public MagicWriterHandle Register(
        string modId,
        IMagicBuilder builder,
        string characterId = "c1001",
        string? magicFileName = null)
    {
        var modifications = builder.GetModifications();
        var magicId = builder.MagicId;
        
        if (modifications == null || modifications.Count == 0)
        {
            _logger.WriteLine($"[MagicWriter] [{modId}] No modifications provided for MagicId {magicId}", _logger.ColorYellow);
            return MagicWriterHandle.Invalid;
        }
        
        // Build the file path
        var actualFileName = magicFileName ?? characterId;
        var magicFilePath = $"chara/{characterId}/magic/{actualFileName}.magic";
        
        // Create handle and registration
        var handle = new MagicWriterHandle(Guid.NewGuid());
        var modSet = new RegisteredModificationSet(handle, modId, magicFilePath, magicId, modifications.ToList());
        
        // Register
        _registeredSets[handle] = modSet;
        
        // Update file index
        _fileToHandles.AddOrUpdate(
            magicFilePath,
            _ => new ConcurrentBag<MagicWriterHandle> { handle },
            (_, bag) => { bag.Add(handle); return bag; });
        
        _logger.WriteLine($"[MagicWriter] [{modId}] Registered {modifications.Count} modifications for MagicId {magicId} in {magicFilePath}", _logger.ColorGreen);
        
        // If the file is already loaded, apply modifications with retry
        if (TryGetLoadedResourceHandle(magicFilePath, out var resourceHandle))
        {
            Task.Run(async () =>
            {
                await ApplyModificationsWithRetry(magicFilePath, resourceHandle, maxRetries: 10, delayMs: 200);
            });
        }
        
        return handle;
    }
    
    /// <summary>
    /// Unregisters a set of modifications.
    /// Note: This does NOT undo the modifications already applied in memory.
    /// The original file will be restored when the game reloads it.
    /// </summary>
    /// <param name="handle">The handle returned from Register.</param>
    /// <returns>True if the handle was found and removed.</returns>
    public bool Unregister(MagicWriterHandle handle)
    {
        if (!handle.IsValid)
            return false;
        
        if (_registeredSets.TryRemove(handle, out var modSet))
        {
            _logger.WriteLine($"[MagicWriter] [{modSet.ModId}] Unregistered modifications for MagicId {modSet.MagicId}", _logger.ColorYellow);
            
            // Clean up file index
            if (_fileToHandles.TryGetValue(modSet.MagicFilePath, out var bag))
            {
                // ConcurrentBag doesn't support removal, so we rebuild it
                var remaining = bag.Where(h => h != handle).ToList();
                if (remaining.Count == 0)
                {
                    _fileToHandles.TryRemove(modSet.MagicFilePath, out _);
                }
                else
                {
                    _fileToHandles[modSet.MagicFilePath] = new ConcurrentBag<MagicWriterHandle>(remaining);
                }
            }
            
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// Unregisters all modifications from a specific mod.
    /// </summary>
    /// <param name="modId">The mod ID to unregister all modifications for.</param>
    /// <returns>The number of modification sets unregistered.</returns>
    public int UnregisterAll(string modId)
    {
        var toRemove = _registeredSets.Values
            .Where(s => s.ModId == modId)
            .Select(s => s.Handle)
            .ToList();
        
        foreach (var handle in toRemove)
        {
            Unregister(handle);
        }
        
        return toRemove.Count;
    }
    
    /// <summary>
    /// Gets all registered modification sets for a specific magic file.
    /// </summary>
    public IReadOnlyList<(string ModId, int MagicId, int ModificationCount)> GetRegisteredModifications(string magicFilePath)
    {
        if (!_fileToHandles.TryGetValue(magicFilePath, out var handles))
            return Array.Empty<(string, int, int)>();
        
        return handles
            .Where(h => _registeredSets.TryGetValue(h, out _))
            .Select(h => _registeredSets[h])
            .Select(s => (s.ModId, s.MagicId, s.Modifications.Count))
            .ToList();
    }
    
    /// <summary>
    /// Gets the total count of registered modification sets.
    /// </summary>
    public int RegisteredCount => _registeredSets.Count;
    
    // ========================================
    // RESOURCE LOAD HANDLER
    // ========================================
    
    private unsafe void OnResourceLoaded(string path, ResourceHandleStruct* resourcePointer)
    {
        // Only care about .magic files
        if (!path.EndsWith(".magic", StringComparison.OrdinalIgnoreCase))
            return;
        
        // Check if we have any registered modifications for this file
        // The path from the hook might be different format (e.g., "nxd://chara/c1001/magic/c1001.magic")
        // We need to check if any of our registered paths are contained in the loaded path
        string? matchedFilePath = null;
        foreach (var registeredPath in _fileToHandles.Keys)
        {
            if (path.Contains(registeredPath, StringComparison.OrdinalIgnoreCase) ||
                path.EndsWith(registeredPath, StringComparison.OrdinalIgnoreCase))
            {
                matchedFilePath = registeredPath;
                break;
            }
        }
        
        if (matchedFilePath == null)
            return;
        
        _logger.WriteLine($"[MagicWriter] Detected load of {path} (matched: {matchedFilePath})", _logger.ColorGreen);
        
        // Schedule the application of modifications (can't await in unsafe context)
        ScheduleModificationApplication(matchedFilePath);
    }
    
    private void ScheduleModificationApplication(string path)
    {
        // Get the ResourceHandle from the service
        if (!TryGetLoadedResourceHandle(path, out var resourceHandle))
        {
            _logger.WriteLine($"[MagicWriter] Could not get ResourceHandle for {path}", _logger.ColorYellow);
            return;
        }
        
        // Use retry mechanism to wait for the buffer to be ready
        Task.Run(async () =>
        {
            await ApplyModificationsWithRetry(path, resourceHandle, maxRetries: 10, delayMs: 200);
        });
    }
    
    private async Task ApplyModificationsWithRetry(string magicFilePath, ResourceHandle resourceHandle, int maxRetries, int delayMs)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            // Wait before attempting
            await Task.Delay(delayMs);
            
            // Check if buffer is ready
            if (resourceHandle.BufferAddress == 0 || !resourceHandle.IsLoaded())
            {
                if (attempt < maxRetries)
                {
                    _logger.WriteLine($"[MagicWriter] Buffer not ready for {magicFilePath}, attempt {attempt}/{maxRetries}", _logger.ColorYellow);
                    continue;
                }
                else
                {
                    _logger.WriteLine($"[MagicWriter] Buffer never became ready for {magicFilePath} after {maxRetries} attempts", _logger.ColorRed);
                    return;
                }
            }
            
            // Buffer is ready, apply modifications
            ApplyModificationsToFile(magicFilePath, resourceHandle);
            return;
        }
    }
    
    // ========================================
    // MODIFICATION APPLICATION
    // ========================================
    
    private void ApplyModificationsToFile(string magicFilePath, ResourceHandle resourceHandle)
    {
        if (!resourceHandle.IsValid)
        {
            _logger.WriteLine($"[MagicWriter] ResourceHandle not valid for {magicFilePath}", _logger.ColorYellow);
            return;
        }
        
        // Double-check buffer is ready
        if (resourceHandle.BufferAddress == 0 || resourceHandle.FileSize == 0)
        {
            _logger.WriteLine($"[MagicWriter] Buffer not ready for {magicFilePath} (Address=0x{resourceHandle.BufferAddress:X}, Size={resourceHandle.FileSize})", _logger.ColorYellow);
            return;
        }
        
        if (!_fileToHandles.TryGetValue(magicFilePath, out var handles) || handles.IsEmpty)
            return;
        
        // Collect all modifications for this file, grouped by MagicId
        var modificationsByMagicId = new Dictionary<int, List<MagicModification>>();
        
        foreach (var handle in handles)
        {
            if (!_registeredSets.TryGetValue(handle, out var modSet))
                continue;
            
            if (!modificationsByMagicId.TryGetValue(modSet.MagicId, out var list))
            {
                list = new List<MagicModification>();
                modificationsByMagicId[modSet.MagicId] = list;
            }
            
            list.AddRange(modSet.Modifications);
        }
        
        if (modificationsByMagicId.Count == 0)
            return;
        
        try
        {
            // Read the magic file from memory
            var magicFile = MagicFile.Open(resourceHandle.BufferAddress, resourceHandle.FileSize);
            
            int totalModifications = 0;
            
            // Apply modifications for each MagicId
            foreach (var kvp in modificationsByMagicId)
            {
                int magicId = kvp.Key;
                var modifications = kvp.Value;
                
                if (!magicFile.MagicEntries.TryGetValue((uint)magicId, out var magicEntry))
                {
                    _logger.WriteLine($"[MagicWriter] MagicId {magicId} not found in {magicFilePath}", _logger.ColorYellow);
                    continue;
                }
                
                foreach (var mod in modifications)
                {
                    ApplySingleModification(magicEntry, mod);
                    totalModifications++;
                }
            }
            
            // Serialize back to bytes
            using var memStream = new MemoryStream();
            magicFile.Write(memStream);
            byte[] newData = memStream.ToArray();
            
            // Replace the buffer in memory
            resourceHandle.ReplaceBuffer(newData);
            
            // Track statistics
            _modificationCounts.AddOrUpdate(magicFilePath, totalModifications, (_, old) => old + totalModifications);
            
            _logger.WriteLine($"[MagicWriter] Applied {totalModifications} modifications to {magicFilePath}", _logger.ColorGreen);
            
            // Raise event
            OnModificationsApplied?.Invoke(magicFilePath, totalModifications);
        }
        catch (Exception ex)
        {
            _logger.WriteLine($"[MagicWriter] Failed to apply modifications to {magicFilePath}: {ex.Message}", _logger.ColorRed);
        }
    }
    
    private void ApplySingleModification(MagicEntry magicEntry, MagicModification mod)
    {
        // Handle operation group level modifications first
        switch (mod.Type)
        {
            case MagicModificationType.AddOperationGroup:
                AddOperationGroupToEntry(magicEntry, mod);
                return;
                
            case MagicModificationType.RemoveOperationGroup:
                RemoveOperationGroupFromEntry(magicEntry, mod);
                return;
        }
        
        // For other modifications, find the operation group first
        var operationGroup = magicEntry.OperationGroupList.OperationGroups
            .FirstOrDefault(g => g.Id == (uint)mod.OperationGroupId);
        
        if (operationGroup == null)
            return;
        
        switch (mod.Type)
        {
            case MagicModificationType.SetProperty:
            case MagicModificationType.AddProperty:
                ApplyPropertyModification(operationGroup, mod);
                break;
                
            case MagicModificationType.RemoveProperty:
                RemovePropertyFromOperation(operationGroup, mod);
                break;
                
            case MagicModificationType.AddOperation:
                AddOperationToGroup(operationGroup, mod);
                break;
                
            case MagicModificationType.RemoveOperation:
                RemoveOperationFromGroup(operationGroup, mod);
                break;
        }
    }
    
    private void ApplyPropertyModification(MagicOperationGroup group, MagicModification mod)
    {
        var operation = group.OperationList.Operations
            .FirstOrDefault(op => (int)op.Type == mod.OperationId);
        
        if (operation == null)
            return;
        
        var propType = (MagicPropertyType)mod.PropertyId;
        var existingProp = operation.Properties.FirstOrDefault(p => p.Type == propType);
        
        if (existingProp != null)
        {
            // Update existing property value
            existingProp.Value = CreatePropertyValue(propType, mod.Value);
            existingProp.Data = existingProp.Value?.GetBytes() ?? Array.Empty<byte>();
        }
        else
        {
            // Create new property using the factory (same as MagicEditor)
            var newProp = MagicPropertyFactory.Create(propType);
            if (newProp != null)
            {
                // Override the default value with our specific value
                newProp.Value = CreatePropertyValue(propType, mod.Value);
                newProp.Data = newProp.Value?.GetBytes() ?? Array.Empty<byte>();
                operation.Properties.Add(newProp);
            }
            else
            {
                // Fallback: create manually if factory doesn't support this property type
                var manualProp = new MagicOperationProperty(propType)
                {
                    Value = CreatePropertyValue(propType, mod.Value)
                };
                manualProp.Data = manualProp.Value?.GetBytes() ?? Array.Empty<byte>();
                operation.Properties.Add(manualProp);
            }
        }
    }
    
    private void RemovePropertyFromOperation(MagicOperationGroup group, MagicModification mod)
    {
        var operation = group.OperationList.Operations
            .FirstOrDefault(op => (int)op.Type == mod.OperationId);
        
        if (operation == null) return;
        
        var propType = (MagicPropertyType)mod.PropertyId;
        operation.Properties.RemoveAll(p => p.Type == propType);
    }
    
    private void AddOperationToGroup(MagicOperationGroup group, MagicModification mod)
    {
        var opType = (MagicOperationType)mod.OperationId;
        
        if (group.OperationList.Operations.Any(op => op.Type == opType))
            return;
        
        // Use factory to create operation (same as MagicEditor)
        var newOperation = MagicOperationFactory.Create(opType);
        
        if (mod.PropertyId >= 0 && mod.Value != null)
        {
            var propType = (MagicPropertyType)mod.PropertyId;
            // Use factory to create property when possible
            var prop = MagicPropertyFactory.Create(propType);
            if (prop != null)
            {
                prop.Value = CreatePropertyValue(propType, mod.Value);
                prop.Data = prop.Value?.GetBytes() ?? Array.Empty<byte>();
                newOperation.Properties.Add(prop);
            }
            else
            {
                // Fallback for unsupported property types
                var manualProp = new MagicOperationProperty(propType)
                {
                    Value = CreatePropertyValue(propType, mod.Value)
                };
                manualProp.Data = manualProp.Value?.GetBytes() ?? Array.Empty<byte>();
                newOperation.Properties.Add(manualProp);
            }
        }
        
        group.OperationList.Operations.Add(newOperation);
    }
    
    private void RemoveOperationFromGroup(MagicOperationGroup group, MagicModification mod)
    {
        var opType = (MagicOperationType)mod.OperationId;
        var operationsToRemove = group.OperationList.Operations
            .Where(op => op.Type == opType)
            .ToList();
        
        foreach (var op in operationsToRemove)
        {
            group.OperationList.Operations.Remove(op);
        }
    }
    
    private void AddOperationGroupToEntry(MagicEntry magicEntry, MagicModification mod)
    {
        var groupId = (uint)mod.OperationGroupId;
        
        // Check if operation group already exists
        if (magicEntry.OperationGroupList.OperationGroups.Any(g => g.Id == groupId))
        {
            return;
        }
        
        // Create a new operation group
        var newGroup = new MagicOperationGroup
        {
            Id = groupId,
            OperationList = new OperationList()
        };
        
        magicEntry.OperationGroupList.OperationGroups.Add(newGroup);
    }
    
    private void RemoveOperationGroupFromEntry(MagicEntry magicEntry, MagicModification mod)
    {
        var groupId = (uint)mod.OperationGroupId;
        
        var groupToRemove = magicEntry.OperationGroupList.OperationGroups
            .FirstOrDefault(g => g.Id == groupId);
        
        if (groupToRemove != null)
        {
            magicEntry.OperationGroupList.OperationGroups.Remove(groupToRemove);
        }
    }
    
    private static MagicPropertyValueBase? CreatePropertyValue(MagicPropertyType propType, object? value)
    {
        if (value == null) return null;
        
        if (!MagicPropertyValueTypeMapping.TypeToValueType.TryGetValue(propType, out var valueType))
        {
            valueType = MagicPropertyValueType.Int;
        }
        
        return valueType switch
        {
            MagicPropertyValueType.Int or MagicPropertyValueType.OperationGroupId => new MagicPropertyIntValue(Convert.ToInt32(value)),
            MagicPropertyValueType.Float => new MagicPropertyFloatValue(Convert.ToSingle(value)),
            MagicPropertyValueType.Bool => new MagicPropertyBoolValue(Convert.ToBoolean(value)),
            MagicPropertyValueType.Byte => new MagicPropertyByteValue(Convert.ToByte(value)),
            MagicPropertyValueType.Vec3 => value switch
            {
                Vector3 v => new MagicPropertyVec3Value(v),
                float[] arr when arr.Length >= 3 => new MagicPropertyVec3Value(new Vector3(arr[0], arr[1], arr[2])),
                _ => null
            },
            _ => new MagicPropertyIntValue(Convert.ToInt32(value))
        };
    }
    
    // ========================================
    // HELPERS
    // ========================================
    
    private bool TryGetLoadedResourceHandle(string magicFilePath, out ResourceHandle resourceHandle)
    {
        resourceHandle = null!;
        
        if (!_resourceManagerService.SortedHandles.TryGetValue(".magic", out var magicHandles))
            return false;
        
        foreach (var kvp in magicHandles)
        {
            if (kvp.Key.EndsWith(magicFilePath, StringComparison.OrdinalIgnoreCase) ||
                kvp.Key.Contains(magicFilePath, StringComparison.OrdinalIgnoreCase))
            {
                resourceHandle = kvp.Value;
                return true;
            }
        }
        
        return false;
    }
    
    // ========================================
    // DISPOSE
    // ========================================
    
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        
        unsafe
        {
            _resourceManagerHooks.OnResourceLoaded -= OnResourceLoaded;
        }
        _registeredSets.Clear();
        _fileToHandles.Clear();
        
        _logger.WriteLine("[MagicWriter] Disposed", _logger.ColorYellow);
    }
}
