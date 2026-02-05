using System;
using System.Collections.Generic;

namespace FF16Framework.Interfaces.GameApis.Magic;

/// <summary>
/// Builder interface for configuring a magic spell before casting.
/// Supports fluent API pattern for easy chaining.
/// 
/// IMPORTANT: Magic modifications require specifying the exact location:
/// - OperationGroupId: Which block of operations to modify
/// - OperationId: Which specific operation within the group
/// - PropertyId: Which property to modify
/// 
/// This precision is necessary because the same PropertyId can appear
/// multiple times across different OperationGroups and Operations.
/// </summary>
public interface IMagicBuilder
{
    /// <summary>
    /// Gets the magic ID this builder is configured for.
    /// </summary>
    int MagicId { get; }
    
    // ========================================
    // PROPERTY MODIFICATIONS
    // ========================================
    
    /// <summary>
    /// Sets (modifies) an existing property value within a specific operation.
    /// The operation must already exist in the magic definition.
    /// </summary>
    /// <param name="operationGroupId">The operation group containing the operation.</param>
    /// <param name="operationId">The specific operation containing the property.</param>
    /// <param name="propertyId">The property ID to modify.</param>
    /// <param name="value">The new value (int, float, bool, or Vector3).</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentException">If the operationGroupId or operationId doesn't exist.</exception>
    IMagicBuilder SetProperty(int operationGroupId, int operationId, int propertyId, object value);
    
    /// <summary>
    /// Sets multiple properties on a specific operation.
    /// </summary>
    /// <param name="operationGroupId">The operation group containing the operation.</param>
    /// <param name="operationId">The specific operation containing the properties.</param>
    /// <param name="properties">Dictionary mapping property IDs to their values.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentException">If the operationGroupId or operationId doesn't exist.</exception>
    IMagicBuilder SetProperty(int operationGroupId, int operationId, IDictionary<int, object> properties);
    
    /// <summary>
    /// Removes a property from a specific operation.
    /// </summary>
    /// <param name="operationGroupId">The operation group containing the operation.</param>
    /// <param name="operationId">The specific operation containing the property.</param>
    /// <param name="propertyId">The property ID to remove.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentException">If the operationGroupId or operationId doesn't exist.</exception>
    IMagicBuilder RemoveProperty(int operationGroupId, int operationId, int propertyId);
    
    /// <summary>
    /// Adds a new property to an existing operation.
    /// Use this to inject new properties that don't exist in the original magic.
    /// </summary>
    /// <param name="operationGroupId">The operation group containing the operation.</param>
    /// <param name="operationId">The specific operation to add the property to.</param>
    /// <param name="propertyId">The property ID to add.</param>
    /// <param name="value">The value (int, float, bool, or Vector3).</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentException">If the operationGroupId or operationId doesn't exist.</exception>
    IMagicBuilder AddProperty(int operationGroupId, int operationId, int propertyId, object value);
    
    /// <summary>
    /// Adds multiple new properties to an existing operation.
    /// Use this to inject new properties that don't exist in the original magic.
    /// </summary>
    /// <param name="operationGroupId">The operation group containing the operation.</param>
    /// <param name="operationId">The specific operation to add the properties to.</param>
    /// <param name="properties">Dictionary mapping property IDs to their values.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentException">If the operationGroupId or operationId doesn't exist.</exception>
    IMagicBuilder AddProperty(int operationGroupId, int operationId, IDictionary<int, object> properties);
    
    // ========================================
    // OPERATION MODIFICATIONS
    // ========================================
    
    /// <summary>
    /// Adds a new operation to an operation group with no properties.
    /// </summary>
    /// <param name="operationGroupId">The operation group to add the operation to.</param>
    /// <param name="operationId">The ID of operation to add.</param>
    /// <param name="injectAfterOp">Operation type ID after which to insert. -1 (default) appends to end of group.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentException">If the operationGroupId doesn't exist.</exception>
    IMagicBuilder AddOperation(int operationGroupId, int operationId, int injectAfterOp = -1);
    
    /// <summary>
    /// Adds a new operation to an operation group with multiple properties.
    /// </summary>
    /// <param name="operationGroupId">The operation group to add the operation to.</param>
    /// <param name="operationId">The ID of operation to add.</param>
    /// <param name="properties">Dictionary mapping property IDs to their values.</param>
    /// <param name="injectAfterOp">Operation type ID after which to insert. -1 (default) appends to end of group.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentException">If the operationGroupId doesn't exist.</exception>
    IMagicBuilder AddOperation(int operationGroupId, int operationId, IDictionary<int, object> properties, int injectAfterOp = -1);
    
    /// <summary>
    /// Removes an operation from an operation group.
    /// </summary>
    /// <param name="operationGroupId">The operation group containing the operation.</param>
    /// <param name="operationId">The ID of operation to remove.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentException">If the operationGroupId doesn't exist.</exception>
    IMagicBuilder RemoveOperation(int operationGroupId, int operationId);
    
    // ========================================
    // OPERATION GROUP MODIFICATIONS
    // ========================================
    
    /// <summary>
    /// Adds a new operation group to the magic entry.
    /// </summary>
    /// <param name="operationGroupId">The ID for the new operation group.</param>
    /// <returns>This builder for chaining.</returns>
    IMagicBuilder AddOperationGroup(int operationGroupId);
    
    /// <summary>
    /// Removes an operation group from the magic entry.
    /// </summary>
    /// <param name="operationGroupId">The ID of the operation group to remove.</param>
    /// <returns>This builder for chaining.</returns>
    IMagicBuilder RemoveOperationGroup(int operationGroupId);
    
    // ========================================
    // VALIDATION
    // ========================================
    
    /// <summary>
    /// Checks if an operation group exists in the current magic definition.
    /// </summary>
    /// <param name="operationGroupId">The operation group ID to check.</param>
    /// <returns>True if the operation group exists.</returns>
    bool HasOperationGroup(int operationGroupId);
    
    /// <summary>
    /// Checks if an operation exists within an operation group.
    /// </summary>
    /// <param name="operationGroupId">The operation group ID.</param>
    /// <param name="operationId">The operation ID to check.</param>
    /// <returns>True if the operation exists.</returns>
    bool HasOperation(int operationGroupId, int operationId);
    
    /// <summary>
    /// Gets all operation group IDs in the current magic definition.
    /// </summary>
    /// <returns>List of operation group IDs.</returns>
    IReadOnlyList<int> GetOperationGroupIds();
    
    /// <summary>
    /// Gets all operation IDs within an operation group.
    /// </summary>
    /// <param name="operationGroupId">The operation group ID.</param>
    /// <returns>List of operation IDs, or empty if group doesn't exist.</returns>
    IReadOnlyList<int> GetoperationIds(int operationGroupId);
    
    // ========================================
    // EXECUTION
    // ========================================
    
    /// <summary>
    /// Casts the configured spell with optional source and target.
    /// </summary>
    /// <param name="sourceActor">
    /// The actor casting the spell. If null, defaults to the player.
    /// </param>
    /// <param name="targetActor">
    /// The target actor. If null, defaults to camera's locked target.
    /// </param>
    /// <returns>True if the spell was cast successfully.</returns>
    bool Cast(nint? sourceActor = null, nint? targetActor = null);
    
    // ========================================
    // SERIALIZATION
    // ========================================
    
    /// <summary>
    /// Exports the current modifications to a JSON string.
    /// The JSON format is designed to be human-readable and editable.
    /// </summary>
    /// <returns>JSON string representing the modifications.</returns>
    string ExportToJson();
    
    /// <summary>
    /// Exports the current modifications to a JSON file.
    /// </summary>
    /// <param name="filePath">Path to save the JSON file.</param>
    /// <returns>True if saved successfully.</returns>
    bool ExportToFile(string filePath);
    
    /// <summary>
    /// Imports modifications from a JSON string, adding to existing modifications.
    /// </summary>
    /// <param name="json">JSON string containing modifications.</param>
    /// <returns>This builder for chaining, or throws if parsing failed.</returns>
    IMagicBuilder ImportFromJson(string json);
    
    /// <summary>
    /// Imports modifications from a JSON file, adding to existing modifications.
    /// </summary>
    /// <param name="filePath">Path to the JSON file containing modifications.</param>
    /// <returns>This builder for chaining, or throws if file not found or parsing failed.</returns>
    IMagicBuilder ImportFromFile(string filePath);
    
    /// <summary>
    /// Gets the modification entries that will be applied.
    /// Useful for debugging or manual serialization.
    /// </summary>
    /// <returns>List of modification entries.</returns>
    IReadOnlyList<MagicModification> GetModifications();
    
    /// <summary>
    /// Clears all modifications.
    /// </summary>
    /// <returns>This builder for chaining.</returns>
    IMagicBuilder Reset();
}

/// <summary>
/// Represents a single modification to a magic spell.
/// This is the public representation of modifications that will be applied.
/// </summary>
public record MagicModification
{
    /// <summary>
    /// The type of modification.
    /// </summary>
    public MagicModificationType Type { get; init; }
    
    /// <summary>
    /// The operation group ID where this modification applies.
    /// </summary>
    public int OperationGroupId { get; init; }
    
    /// <summary>
    /// The operation ID (for operation modifications) or the operation containing the property.
    /// </summary>
    public int OperationId { get; init; }
    
    /// <summary>
    /// The property ID (for single property modifications like SetProperty, RemoveProperty, AddProperty).
    /// </summary>
    public int PropertyId { get; init; }
    
    /// <summary>
    /// The value to set (int, float, Vector3, or bool) for single property modifications.
    /// </summary>
    public object? Value { get; init; }
    
    /// <summary>
    /// Properties to set on the operation. Key = PropertyId, Value = property value.
    /// Used for AddOperation with multiple properties.
    /// </summary>
    public IDictionary<int, object>? Properties { get; init; }
    
    /// <summary>
    /// The operation type ID inside an OperationGroup after which to inject this modification.
    /// -1 means inject at the end of the operation group.
    /// </summary>
    public int InsertAfterOperationTypeId { get; init; } = -1;
}

/// <summary>
/// Types of magic modifications.
/// </summary>
public enum MagicModificationType
{
    /// <summary>
    /// Set/modify an existing property value.
    /// </summary>
    SetProperty,
    
    /// <summary>
    /// Remove a property from an operation.
    /// </summary>
    RemoveProperty,
    
    /// <summary>
    /// Add a new property to an existing operation.
    /// </summary>
    AddProperty,
    
    /// <summary>
    /// Add a new operation to an operation group.
    /// </summary>
    AddOperation,
    
    /// <summary>
    /// Remove an operation from an operation group.
    /// </summary>
    RemoveOperation,
    
    /// <summary>
    /// Add a new operation group to the magic entry.
    /// </summary>
    AddOperationGroup,
    
    /// <summary>
    /// Remove an operation group from the magic entry.
    /// </summary>
    RemoveOperationGroup
}

/// <summary>
/// JSON-serializable format for magic spell modifications.
/// This structure is designed to be human-readable and easy to edit manually.
/// </summary>
public class MagicSpellConfig
{
    /// <summary>
    /// The magic ID this configuration applies to.
    /// </summary>
    public int MagicId { get; set; }
    
    /// <summary>
    /// Human-readable name for this spell configuration.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Optional description of what this configuration does.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// List of modifications to apply.
    /// </summary>
    public List<MagicModificationConfig> Modifications { get; set; } = new();
}

/// <summary>
/// JSON-serializable format for a single modification.
/// </summary>
public class MagicModificationConfig
{
    /// <summary>
    /// Type of modification: "SetProperty", "RemoveProperty", "AddProperty", "AddOperation", "RemoveOperation"
    /// </summary>
    public MagicModificationType Type { get; set; }
    
    /// <summary>
    /// The operation group ID where this modification applies.
    /// </summary>
    public int OperationGroupId { get; set; }
    
    /// <summary>
    /// The operation type ID.
    /// For property modifications: the operation containing the property.
    /// For operation modifications: the operation type to add/remove.
    /// </summary>
    public int OperationId { get; set; }
    
    /// <summary>
    /// For property modifications: the single property ID.
    /// Not used for AddOperation with multiple properties.
    /// </summary>
    public int? PropertyId { get; set; }
    
    /// <summary>
    /// For property modifications: the value (can be int, float, bool, or array of 3 floats for Vector3).
    /// Not used for AddOperation with multiple properties.
    /// </summary>
    public object? Value { get; set; }
    
    /// <summary>
    /// For AddOperation: properties with their values. Key = PropertyId, Value = property value.
    /// </summary>
    public IDictionary<int, object>? Properties { get; set; }
    
    /// <summary>
    /// The operation type ID inside an OperationGroup after which to inject this modification.
    /// -1 (or null/omitted) means inject at the end of the operation group.
    /// Only used for IsInjection entries (AddProperty, AddOperation).
    /// </summary>
    public int? InsertAfterOperationTypeId { get; set; }
}
