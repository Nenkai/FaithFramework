using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Reloaded.Mod.Interfaces;
using FF16Framework.Interfaces.GameApis.Magic;
using FF16Framework.Interfaces.GameApis.Structs;
using FF16Tools.Files.Magic;
using FF16Tools.Files.Magic.Factories;

namespace FF16Framework.Services.Faith.GameApis.Magic;

/// <summary>
/// Represents a single modification to a magic spell (runtime representation).
/// This is the internal domain model used in the _modifications dictionary.
/// Uses non-nullable types and init-only properties for immutability.
/// </summary>
/// <remarks>
/// For JSON serialization, use <see cref="MagicModificationConfig"/> instead,
/// which has nullable properties to allow omitting fields in JSON.
/// </remarks>
internal record MagicModification : IMagicModification
{
    public MagicModificationType Type { get; init; }
    public int OperationGroupId { get; init; }
    public int OperationId { get; init; }
    public int PropertyId { get; init; }
    public object? Value { get; init; }
    public IDictionary<int, object>? Properties { get; init; }
    public int InsertAfterOperationTypeId { get; init; } = -1;
}

/// <summary>
/// JSON-serializable configuration for a spell (DTO for import/export).
/// Contains metadata and a list of modification configs.
/// </summary>
internal class MagicSpellConfig : IMagicSpellConfig
{
    public int MagicId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ReplaceOriginal { get; set; }
    public List<MagicModificationConfig> Modifications { get; set; } = new();
    
    IList<IMagicModificationConfig> IMagicSpellConfig.Modifications => 
        Modifications.Cast<IMagicModificationConfig>().ToList();
}

/// <summary>
/// JSON-serializable format for a single modification (DTO for import/export).
/// Uses nullable types so fields can be omitted in JSON when not applicable.
/// </summary>
/// <remarks>
/// Differs from <see cref="MagicModification"/>:
/// - PropertyId is nullable (not used for AddOperation with Properties dictionary)
/// - InsertAfterOperationTypeId is nullable (omitted means -1 / end of group)
/// - Uses mutable { get; set; } for JSON deserialization
/// </remarks>
internal class MagicModificationConfig : IMagicModificationConfig
{
    public MagicModificationType Type { get; set; }
    public int OperationGroupId { get; set; }
    public int OperationId { get; set; }
    public int? PropertyId { get; set; }
    public object? Value { get; set; }
    public IDictionary<int, object>? Properties { get; set; }
    public int? InsertAfterOperationTypeId { get; set; }
}

/// <summary>
/// Implementation of IMagicBuilder.
/// Provides fluent API for configuring magic spell modifications.
/// </summary>
internal class MagicBuilder : IMagicBuilder
{
    private readonly MagicCastingEngine _engine;
    private readonly ILogger _logger;
    private readonly string _modId;
    
    // Dictionary keyed by (Type, GroupId, OpId, PropId) to ensure uniqueness
    // For operations without properties, PropId = -1
    private readonly Dictionary<(MagicModificationType Type, int GroupId, int OpId, int PropId), MagicModification> _modifications = new();
    
    public int MagicId { get; }
    public bool ReplaceOriginal { get; set; }
    
    internal MagicBuilder(int magicId, MagicCastingEngine engine, ILogger logger, string modId)
    {
        MagicId = magicId;
        _engine = engine;
        _logger = logger;
        _modId = modId;
    }
    
    // ========================================
    // VALIDATION (Stubs - always allow, validation happens at runtime)
    // ========================================
    
    public bool HasOperationGroup(int operationGroupId) => true;
    public bool HasOperation(int operationGroupId, int operationId) => true;
    public bool HasProperty(int operationGroupId, int operationId, int propertyId) => true;
    public IReadOnlyList<int> GetOperationGroupIds() => Array.Empty<int>();
    public IReadOnlyList<int> GetoperationIds(int operationGroupId) => Array.Empty<int>();
    
    // ========================================
    // PROPERTY MODIFICATIONS
    // ========================================
    
    public IMagicBuilder SetProperty(int operationGroupId, int operationId, int propertyId, object value)
    {
        // Remove any conflicting RemoveProperty for the same target
        var removeKey = (MagicModificationType.RemoveProperty, operationGroupId, operationId, propertyId);
        _modifications.Remove(removeKey);
        
        // Check if there's an existing AddProperty - if so, update its value instead of replacing with SetProperty
        // This is important for properties on added operations: they need IsInjection=true to be injected at end of group
        var addKey = (MagicModificationType.AddProperty, operationGroupId, operationId, propertyId);
        if (_modifications.TryGetValue(addKey, out var existingAdd))
        {
            // Create a new record with the updated value - keep it as AddProperty so it gets injected
            _modifications[addKey] = existingAdd with { Value = NormalizeValue(value) };
            return this;
        }
        
        // No existing AddProperty, create a SetProperty (for properties that exist in the original .magic file)
        var key = (MagicModificationType.SetProperty, operationGroupId, operationId, propertyId);
        _modifications[key] = new MagicModification
        {
            Type = MagicModificationType.SetProperty,
            OperationGroupId = operationGroupId,
            OperationId = operationId,
            PropertyId = propertyId,
            Value = NormalizeValue(value)
        };
        
        return this;
    }
    
    public IMagicBuilder SetProperty(int operationGroupId, int operationId, IDictionary<int, object> properties)
    {
        foreach (var kvp in properties)
        {
            SetProperty(operationGroupId, operationId, kvp.Key, kvp.Value);
        }
        return this;
    }

        public IMagicBuilder AddProperty(int operationGroupId, int operationId, int propertyId, object value)
    {
        // Check if there's already a SetProperty for this target - if so, use SetProperty instead
        var setKey = (MagicModificationType.SetProperty, operationGroupId, operationId, propertyId);
        if (_modifications.ContainsKey(setKey))
        {
            return SetProperty(operationGroupId, operationId, propertyId, value);
        }
        
        var key = (MagicModificationType.AddProperty, operationGroupId, operationId, propertyId);
        
        // Remove any conflicting RemoveProperty for the same target
        var removeKey = (MagicModificationType.RemoveProperty, operationGroupId, operationId, propertyId);
        _modifications.Remove(removeKey);
        
        _modifications[key] = new MagicModification
        {
            Type = MagicModificationType.AddProperty,
            OperationGroupId = operationGroupId,
            OperationId = operationId,
            PropertyId = propertyId,
            Value = NormalizeValue(value)
        };
        
        return this;
    }
    
    public IMagicBuilder AddProperty(int operationGroupId, int operationId, IDictionary<int, object> properties)
    {
        foreach (var kvp in properties)
        {
            AddProperty(operationGroupId, operationId, kvp.Key, kvp.Value);
        }
        return this;
    }
    
    public IMagicBuilder RemoveProperty(int operationGroupId, int operationId, int propertyId)
    {
        var key = (MagicModificationType.RemoveProperty, operationGroupId, operationId, propertyId);
        
        // Remove any conflicting SetProperty or AddProperty for the same target
        var setKey = (MagicModificationType.SetProperty, operationGroupId, operationId, propertyId);
        var addKey = (MagicModificationType.AddProperty, operationGroupId, operationId, propertyId);
        _modifications.Remove(setKey);
        _modifications.Remove(addKey);
        
        _modifications[key] = new MagicModification
        {
            Type = MagicModificationType.RemoveProperty,
            OperationGroupId = operationGroupId,
            OperationId = operationId,
            PropertyId = propertyId
        };
        
        return this;
    }
    
    // ========================================
    // OPERATION MODIFICATIONS
    // ========================================
    
    public IMagicBuilder AddOperation(int operationGroupId, int operationId, int injectAfterOp = -1)
    {
        // For operations, use PropId = -1 as the key
        var key = (MagicModificationType.AddOperation, operationGroupId, operationId, -1);
        
        // Remove any conflicting RemoveOperation
        var removeKey = (MagicModificationType.RemoveOperation, operationGroupId, operationId, -1);
        _modifications.Remove(removeKey);
        
        _modifications[key] = new MagicModification
        {
            Type = MagicModificationType.AddOperation,
            OperationGroupId = operationGroupId,
            OperationId = operationId,
            InsertAfterOperationTypeId = injectAfterOp
        };
        
        return this;
    }
    
    public IMagicBuilder AddOperation(int operationGroupId, int operationId, IDictionary<int, object> properties, int injectAfterOp = -1)
    {
        // First, add the operation itself with injection point
        AddOperation(operationGroupId, operationId, injectAfterOp);
        
        // Then add each property using AddProperty (which handles validation and uniqueness)
        foreach (var kvp in properties)
        {
            AddProperty(operationGroupId, operationId, kvp.Key, kvp.Value);
        }
        
        return this;
    }
    
    public IMagicBuilder RemoveOperation(int operationGroupId, int operationId)
    {
        var key = (MagicModificationType.RemoveOperation, operationGroupId, operationId, -1);
        
        // Remove any conflicting modifications for the same operation:
        // - AddOperation entries
        // - AddProperty/SetProperty/RemoveProperty entries (properties belong to this operation)
        var keysToRemove = _modifications.Keys
            .Where(k => k.GroupId == operationGroupId && 
                        k.OpId == operationId &&
                        (k.Type == MagicModificationType.AddOperation ||
                         k.Type == MagicModificationType.AddProperty ||
                         k.Type == MagicModificationType.SetProperty ||
                         k.Type == MagicModificationType.RemoveProperty))
            .ToList();
        foreach (var keyToRemove in keysToRemove)
        {
            _modifications.Remove(keyToRemove);
        }
        
        _modifications[key] = new MagicModification
        {
            Type = MagicModificationType.RemoveOperation,
            OperationGroupId = operationGroupId,
            OperationId = operationId
        };
        
        return this;
    }
    
    // ========================================
    // OPERATION GROUP MODIFICATIONS
    // ========================================
    
    public IMagicBuilder AddOperationGroup(int operationGroupId)
    {
        // Key: use OpId = -1 and PropId = -1 for operation group level modifications
        var key = (MagicModificationType.AddOperationGroup, operationGroupId, -1, -1);
        
        // Remove any conflicting RemoveOperationGroup
        var removeKey = (MagicModificationType.RemoveOperationGroup, operationGroupId, -1, -1);
        _modifications.Remove(removeKey);
        
        _modifications[key] = new MagicModification
        {
            Type = MagicModificationType.AddOperationGroup,
            OperationGroupId = operationGroupId
        };
        
        return this;
    }
    
    public IMagicBuilder RemoveOperationGroup(int operationGroupId)
    {
        var key = (MagicModificationType.RemoveOperationGroup, operationGroupId, -1, -1);
        
        // Remove any conflicting modifications for this operation group:
        // - AddOperationGroup
        // - All operations and properties within this group
        var keysToRemove = _modifications.Keys
            .Where(k => k.GroupId == operationGroupId)
            .ToList();
        foreach (var keyToRemove in keysToRemove)
        {
            _modifications.Remove(keyToRemove);
        }
        
        _modifications[key] = new MagicModification
        {
            Type = MagicModificationType.RemoveOperationGroup,
            OperationGroupId = operationGroupId
        };
        
        return this;
    }
    
    // ========================================
    // EXECUTION
    // ========================================
    
    public bool Cast(ActorSelection source = ActorSelection.Player, ActorSelection target = ActorSelection.LockedTarget)
    {
        return _engine.CastSpell(BuildCastRequest(source, target));
    }
    
    // ========================================
    // SERIALIZATION
    // ========================================
    
    public string ExportToJson()
    {
        // Identify operations that have AddOperation (their properties get embedded)
        var addOperationKeys = _modifications.Values
            .Where(m => m.Type == MagicModificationType.AddOperation)
            .Select(m => (m.OperationGroupId, m.OperationId))
            .ToHashSet();
        
        // Group modifications, excluding AddProperty that belong to an AddOperation
        var groups = _modifications.Values
            .Where(m => !(m.Type == MagicModificationType.AddProperty && 
                          addOperationKeys.Contains((m.OperationGroupId, m.OperationId))))
            .GroupBy(m => (m.Type, m.OperationGroupId, m.OperationId));
        
        var configs = groups.Select(g => ConvertGroupToConfig(g.Key, g.ToList())).ToList();
        
        var config = new MagicSpellConfig
        {
            MagicId = MagicId,
            ReplaceOriginal = ReplaceOriginal,
            Name = $"Magic_{MagicId}",
            Description = $"Exported spell configuration for Magic ID {MagicId}",
            Modifications = configs
        };
        
        return JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        });
    }
    
    private MagicModificationConfig ConvertGroupToConfig(
        (MagicModificationType Type, int GroupId, int OpId) key,
        List<MagicModification> mods)
    {
        var config = new MagicModificationConfig
        {
            Type = key.Type,
            OperationGroupId = key.GroupId,
            OperationId = key.OpId
        };
        
        switch (key.Type)
        {
            case MagicModificationType.SetProperty:
            case MagicModificationType.AddProperty:
                if (mods.Count == 1)
                {
                    config.PropertyId = mods[0].PropertyId;
                    config.Value = SerializeValue(mods[0].Value);
                }
                else
                {
                    config.Properties = mods.ToDictionary(
                        m => m.PropertyId,
                        m => SerializeValue(m.Value)!
                    );
                }
                break;
                
            case MagicModificationType.AddOperation:
                var op = mods[0];
                if (op.InsertAfterOperationTypeId != -1)
                    config.InsertAfterOperationTypeId = op.InsertAfterOperationTypeId;
                
                // Embed AddProperty entries that belong to this operation
                var embeddedProps = _modifications.Values
                    .Where(m => m.Type == MagicModificationType.AddProperty &&
                               m.OperationGroupId == key.GroupId &&
                               m.OperationId == key.OpId)
                    .ToDictionary(m => m.PropertyId, m => SerializeValue(m.Value)!);
                
                if (embeddedProps.Count > 0)
                    config.Properties = embeddedProps;
                break;
                
            // RemoveProperty, RemoveOperation, AddOperationGroup, RemoveOperationGroup: no extra fields
        }
        
        return config;
    }
    
    public bool ExportToFile(string filePath)
    {
        try
        {
            var json = ExportToJson();
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(filePath, json);
            _logger.WriteLine($"[{_modId}] [MagicBuilder] Exported to: {filePath}", _logger.ColorGreen);
            return true;
        }
        catch (Exception ex)
        {
            _logger.WriteLine($"[{_modId}] [MagicBuilder] Failed to export: {ex.Message}", _logger.ColorRed);
            return false;
        }
    }
    
    public IMagicBuilder ImportFromJson(string json)
    {
        try
        {
            var config = JsonSerializer.Deserialize<MagicSpellConfig>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
            
            if (config == null)
            {
                throw new ArgumentException("Failed to parse JSON");
            }
            
            // Validate magic ID matches (or allow if this is a fresh builder)
            if (config.MagicId != 0 && config.MagicId != MagicId)
            {
                _logger.WriteLine($"[{_modId}] [MagicBuilder] Warning: JSON MagicId ({config.MagicId}) differs from builder ({MagicId})", _logger.ColorYellow);
            }
            
            foreach (var modConfig in config.Modifications)
            {
                ApplyModificationConfig(modConfig);
            }
            
            ReplaceOriginal = config.ReplaceOriginal;
            
            return this;
        }
        catch (JsonException ex)
        {
            throw new ArgumentException($"Invalid JSON format: {ex.Message}", ex);
        }
    }
    
    public IMagicBuilder ImportFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Configuration file not found: {filePath}");
        }
        
        var json = File.ReadAllText(filePath);
        return ImportFromJson(json);
    }
    
    public IReadOnlyList<IMagicModification> GetModifications()
    {
        return _modifications.Values.ToList<IMagicModification>().AsReadOnly();
    }
    
    public IMagicBuilder Reset()
    {
        _modifications.Clear();
        return this;
    }
    
    // ========================================
    // INTERNAL HELPERS
    // ========================================
    
    private MagicCastRequest BuildCastRequest(ActorSelection source, ActorSelection target)
    {
        nint? sourceActor = source switch
        {
            ActorSelection.Player => _engine.GetPlayerActor(),
            ActorSelection.LockedTarget => _engine.GetLockedTarget(),
            ActorSelection.None => nint.Zero,
            _ => nint.Zero
        };
        
        nint? targetActor = target switch
        {
            ActorSelection.Player => _engine.GetPlayerActor(),
            ActorSelection.LockedTarget => _engine.GetLockedTarget(),
            ActorSelection.None => nint.Zero,
            _ => nint.Zero
        };
        
        return new MagicCastRequest
        {
            MagicId = MagicId,
            SourceActor = sourceActor,
            TargetActor = targetActor,
            UseGameTarget = target == ActorSelection.LockedTarget,
            Modifications = _modifications.Values.ToList()
        };
    }
    
    private static object NormalizeValue(object value)
    {
        // Ensure consistent value types
        return value switch
        {
            int i => i,
            float f => f,
            double d => (float)d,
            bool b => b,
            Vector3 v => v,
            _ => value
        };
    }
    
    private static object? SerializeValue(object? value)
    {
        if (value is Vector3 v)
        {
            return new float[] { v.X, v.Y, v.Z };
        }
        return value;
    }
    
    private void ApplyModificationConfig(MagicModificationConfig config)
    {
        var type = config.Type;
        int injectAfterOp = config.InsertAfterOperationTypeId ?? -1;  // Default to -1 (end of group) if not specified
        
        switch (type)
        {
            case MagicModificationType.SetProperty:
                if (config.Properties != null && config.Properties.Count > 0)
                {
                    var deserializedProps = config.Properties.ToDictionary(
                        kvp => kvp.Key,
                        kvp => DeserializeValue(kvp.Value, kvp.Key)
                    );
                    SetProperty(config.OperationGroupId, config.OperationId, deserializedProps);
                }
                else if (config.PropertyId.HasValue && config.Value != null)
                {
                    SetProperty(config.OperationGroupId, config.OperationId, config.PropertyId.Value, 
                        DeserializeValue(config.Value, config.PropertyId));
                }
                break;
            
            case MagicModificationType.AddProperty:
                if (config.Properties != null && config.Properties.Count > 0)
                {
                    var deserializedProps = config.Properties.ToDictionary(
                        kvp => kvp.Key,
                        kvp => DeserializeValue(kvp.Value, kvp.Key)
                    );
                    AddProperty(config.OperationGroupId, config.OperationId, deserializedProps);
                }
                else if (config.PropertyId.HasValue && config.Value != null)
                {
                    AddProperty(config.OperationGroupId, config.OperationId, config.PropertyId.Value, 
                        DeserializeValue(config.Value, config.PropertyId));
                }
                break;
                
            case MagicModificationType.RemoveProperty:
                if (config.PropertyId.HasValue)
                {
                    RemoveProperty(config.OperationGroupId, config.OperationId, config.PropertyId.Value);
                }
                break;
                
            case MagicModificationType.AddOperation:
                if (config.Properties != null && config.Properties.Count > 0)
                {
                    // Deserialize values in the dictionary
                    var deserializedProps = config.Properties.ToDictionary(
                        kvp => kvp.Key,
                        kvp => DeserializeValue(kvp.Value, kvp.Key)
                    );
                    AddOperation(config.OperationGroupId, config.OperationId, deserializedProps, injectAfterOp);
                }
                else
                {
                    AddOperation(config.OperationGroupId, config.OperationId, injectAfterOp);
                }
                break;
                
            case MagicModificationType.RemoveOperation:
                RemoveOperation(config.OperationGroupId, config.OperationId);
                break;
                
            case MagicModificationType.AddOperationGroup:
                AddOperationGroup(config.OperationGroupId);
                break;
                
            case MagicModificationType.RemoveOperationGroup:
                RemoveOperationGroup(config.OperationGroupId);
                break;
        }
    }
    
    private static object DeserializeValue(object? value, int? propertyId = null)
    {
        if (value == null) return 0;
        
        object rawValue;
        
        // Handle JSON element types
        if (value is JsonElement element)
        {
            rawValue = element.ValueKind switch
            {
                JsonValueKind.Number => element.TryGetInt32(out int i) ? i : element.GetSingle(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Array when element.GetArrayLength() == 3 => 
                    new Vector3(
                        element[0].GetSingle(),
                        element[1].GetSingle(),
                        element[2].GetSingle()
                    ),
                _ => value
            };
        }
        // Handle arrays (for Vector3)
        else if (value is float[] arr && arr.Length == 3)
        {
            rawValue = new Vector3(arr[0], arr[1], arr[2]);
        }
        else
        {
            rawValue = value;
        }
        
        // Apply type coercion based on MagicPropertyValueTypeMapping
        if (propertyId.HasValue)
        {
            var propType = (MagicPropertyType)propertyId.Value;
            if (MagicPropertyValueTypeMapping.TypeToValueType.TryGetValue(propType, out var valueType))
            {
                rawValue = CoerceToPropertyType(rawValue, valueType);
            }
        }
        
        return rawValue;
    }
    
    /// <summary>
    /// Coerces a value to the expected property type based on MagicPropertyValueType.
    /// </summary>
    private static object CoerceToPropertyType(object value, MagicPropertyValueType expectedType)
    {
        return expectedType switch
        {
            MagicPropertyValueType.Int or MagicPropertyValueType.OperationGroupId => value switch
            {
                int i => i,
                float f => (int)f,
                double d => (int)d,
                long l => (int)l,
                _ => value
            },
            MagicPropertyValueType.Float => value switch
            {
                float f => f,
                int i => (float)i,
                double d => (float)d,
                _ => value
            },
            MagicPropertyValueType.Bool or MagicPropertyValueType.Byte => value switch
            {
                bool b => b,
                int i => i != 0,
                float f => f != 0,
                _ => value
            },
            MagicPropertyValueType.Vec3 => value switch
            {
                Vector3 v => v,
                _ => value
            },
            _ => value
        };
    }
}

/// <summary>
/// Internal request structure for casting a spell.
/// Contains all configuration from the builder.
/// </summary>
internal record MagicCastRequest
{
    public int MagicId { get; init; }
    public nint? SourceActor { get; init; }
    public nint? TargetActor { get; init; }
    
    /// <summary>
    /// Explicit target position in world space. If set, this takes priority over TargetActor.
    /// </summary>
    public Vector3? TargetPosition { get; init; }
    
    /// <summary>
    /// Explicit target direction (for projectiles). If not set, defaults to forward.
    /// </summary>
    public Vector3? TargetDirection { get; init; }
    
    /// <summary>
    /// If true, copies the game's own TargetStruct directly from GetTargetedEnemy().
    /// This is the correct way to get body-targeting (Y=1.23 vs Y=0.26 from StaticActorInfo).
    /// Takes priority over all other targeting options.
    /// </summary>
    public bool UseGameTarget { get; init; } = false;
    
    public List<MagicModification> Modifications { get; init; } = new();
}
