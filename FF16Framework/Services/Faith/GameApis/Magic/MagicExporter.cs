using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using FF16Framework.Interfaces.GameApis.Magic;
using FF16Framework.Interfaces.GameApis.Structs;
using FF16Tools.Files.Magic;
using FF16Tools.Files.Magic.Factories;

namespace FF16Framework.Services.Faith.GameApis.Magic;

/// <summary>
/// Helper class for exporting magic entries to JSON format.
/// </summary>
internal static class MagicExporter
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }
    };
    
    /// <summary>
    /// Exports a MagicEntry to JSON string.
    /// </summary>
    public static string ExportToJson(MagicEntry entry)
    {
        var config = BuildMagicSpellConfig(entry);
        return JsonSerializer.Serialize(config, _jsonOptions);
    }
    
    /// <summary>
    /// Builds a MagicSpellConfig from a MagicEntry.
    /// Exports the full structure: OperationGroups, Operations, and Properties.
    /// </summary>
    public static MagicSpellConfig BuildMagicSpellConfig(MagicEntry entry)
    {
        var config = new MagicSpellConfig
        {
            MagicId = (int)entry.Id,
            Name = $"Magic_{entry.Id}",
            Description = $"Exported spell configuration",
            Modifications = new List<MagicModificationConfig>()
        };
        
        foreach (var group in entry.OperationGroupList.OperationGroups)
        {
            // Export the OperationGroup itself
            config.Modifications.Add(new MagicModificationConfig
            {
                Type = MagicModificationType.AddOperationGroup,
                OperationGroupId = (int)group.Id
            });
            
            foreach (var operation in group.OperationList.Operations)
            {
                // Export the Operation with all its properties
                var properties = new Dictionary<int, object>();
                foreach (var property in operation.Properties)
                {
                    var value = ExtractPropertyValue(property);
                    if (value != null)
                    {
                        properties[(int)property.Type] = value;
                    }
                }
                
                config.Modifications.Add(new MagicModificationConfig
                {
                    Type = MagicModificationType.AddOperation,
                    OperationGroupId = (int)group.Id,
                    OperationId = (int)operation.Type,
                    Properties = properties.Count > 0 ? properties : null
                });
            }
        }
        
        return config;
    }
    
    /// <summary>
    /// Extracts the value from a MagicOperationProperty in a JSON-serializable format.
    /// </summary>
    public static object? ExtractPropertyValue(MagicOperationProperty property)
    {
        return property.Value switch
        {
            MagicPropertyIdValue idValue => idValue.Id,
            MagicPropertyFloatValue floatValue => floatValue.Value,
            MagicPropertyIntValue intValue => intValue.Value,
            MagicPropertyBoolValue boolValue => boolValue.Value,
            MagicPropertyByteValue byteValue => (int)byteValue.Value,
            MagicPropertyVec3Value vec3Value => new float[] { vec3Value.Value.X, vec3Value.Value.Y, vec3Value.Value.Z },
            _ => null
        };
    }
}
