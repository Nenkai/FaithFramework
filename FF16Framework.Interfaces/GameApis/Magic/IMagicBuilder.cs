using System;
using System.Collections.Generic;

using FF16Framework.Interfaces.GameApis.Structs;

namespace FF16Framework.Interfaces.GameApis.Magic;

/// <summary>
/// Builder interface for configuring a magic spell effects before casting.
/// <para>
/// To know which MagicBuilder functions you have to call, you have to know the Magic File structure in FF16:
/// </para>
/// <list type="bullet">
/// <item><description>Flow of structures: MagicID → OperationGroups → Operations → Properties</description></item>
/// <item><description>MagicID: Unique identifier usually for each spell and their upgrades,
///   but sometimes a spell can be composed of multiple MagicIDs, e.g. Thunderstorm from Ramuh.</description></item>
/// <item><description>OperationGroups: Each Magic has multiple OperationGroups, which can be considered as blocks of operations
///   executed in different moments of the spell's lifecycle, e.g. on cast, on hit, on expire, etc.)</description></item>
/// <item><description>Operations: Each OperationGroup has multiple Operations, they usually code a specific functionality:
///   projectile creation, hitbox creation, camera movement, etc.</description></item>
/// <item><description>Properties: Each Operation has multiple Properties, these represent a specific aspect or variable of a funcionality.
///   E.g. for an operation that codes a linear trajectory for a projectile, a property can be speed, tracking degrees, vertical angle, etc.</description></item>
/// </list>
/// <para>
/// IMPORTANT: It's recommended to use FF16Framework's Magic Editor ImGui window to inspect the magic file structure and know the IDs of the OperationGroups, 
/// Operations and Properties. Also it's a great experimental tool and let's you export your modifications as JSON string which you can then use in the 
/// MagicBuilder's ImportFromJson() function to apply the same modifications on the fly when casting the spell or to register them with the MagicWriter 
/// for permanent application.
/// </para>
/// <para>
/// This builder allows you to stack multiple modifications to the magic file structure, and then apply them all at once when casting the spell with the Cast() function. 
/// Also it can export and import all the modifications as a JSON data structure.
/// </para>
/// </summary>
/// <remarks>
/// Example of code using the MagicBuilder API:
/// <code><![CDATA[
/// var spell = magicApi.CreateSpell(214) // Dia's Magic ID
///     .RemoveOperation(4338, 1) // Removes linear trajectory from the projectile
///     .AddOperation(4338, 2493, 
///                   new Dictionary<int, object>
///                   {
///                        { 187, 2 }, // Paralbolic trajectory type
///                        { 8, 55.0 }, // Velocity
///                        { 2430, new float[] { -90.0f, 0.0f, 0.0f } }, // Angles
///                        { 2593, 2.0 } // Intensity
///                   }); // Adds a parabalolic trajectory with custom properties
///
/// spell.Cast(); // Casts the spell with the modifications applied on the fly
/// 
/// // Save modifications as JSON string to reuse later or to register with the MagicWriter for permanent application.
/// spell.ExportFile("my_mod_dia_changes.json");
/// 
/// // Import your saved modifications from a JSON file
/// bool cast_success = magicApi.ImportFromFile("my_mod_dia_changes.json").Cast();
/// ]]></code>
/// </remarks>
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
    /// Modifies an existing property with the given value within a specific operation and operation group.
    /// </summary>
    /// <param name="operationGroupId">The operation group containing the operation.</param>
    /// <param name="operationId">The specific operation containing the property.</param>
    /// <param name="propertyId">The property ID to modify.</param>
    /// <param name="value">The new value (int, float, bool, or Vector3).</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentException">If the operationGroupId or operationId doesn't exist.</exception>
    IMagicBuilder SetProperty(int operationGroupId, int operationId, int propertyId, object value);
    
    /// <summary>
    /// Same as before, but can set multiple properties on a specific operation.
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
    /// Casts the configured spell using actor selection presets.
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
