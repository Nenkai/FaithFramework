using System;
using System.Collections.Generic;

using FF16Framework.Interfaces.GameApis.Structs;

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
    // EXECUTION
    // ========================================
    
    /// <summary>
    /// Casts the configured spell with the specified source and target.
    /// </summary>
    /// <param name="source">The actor casting the spell. Defaults to Player.</param>
    /// <param name="target">The target actor. Defaults to LockedTarget.</param>
    /// <returns>True if the spell was cast successfully.</returns>
    bool Cast(ActorSelection source = ActorSelection.Player, ActorSelection target = ActorSelection.LockedTarget);
    
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
    IReadOnlyList<IMagicModification> GetModifications();
    
    /// <summary>
    /// Clears all modifications.
    /// </summary>
    /// <returns>This builder for chaining.</returns>
    IMagicBuilder Reset();
}
