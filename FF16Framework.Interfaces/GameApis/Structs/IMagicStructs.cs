using System.Collections.Generic;

namespace FF16Framework.Interfaces.GameApis.Structs;

/// <summary>
/// Specifies which actor to use as source or target when casting magic.
/// </summary>
public enum ActorSelection
{
    /// <summary>
    /// No actor (cast without source/target).
    /// </summary>
    None,
    
    /// <summary>
    /// The player character (Clive).
    /// </summary>
    Player,
    
    /// <summary>
    /// The camera's currently soft-locked or hard-locked target.
    /// Uses the game's targeting system for proper body positioning.
    /// </summary>
    LockedTarget
}

/// <summary>
/// Types of magic modifications that can be applied to a spell.
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
/// Represents a single modification to a magic spell.
/// </summary>
public interface IMagicModification
{
    /// <summary>
    /// The type of modification.
    /// </summary>
    MagicModificationType Type { get; }
    
    /// <summary>
    /// The operation group ID where this modification applies.
    /// </summary>
    int OperationGroupId { get; }
    
    /// <summary>
    /// The operation ID (for operation modifications) or the operation containing the property.
    /// </summary>
    int OperationId { get; }
    
    /// <summary>
    /// Properties to set on the operation. Key = PropertyId, Value = property value.
    /// </summary>
    IDictionary<int, object>? Properties { get; }
    
    /// <summary>
    /// The operation type ID inside an OperationGroup after which to inject this modification.
    /// -1 means inject at the end of the operation group.
    /// </summary>
    int InsertAfterOperationTypeId { get; }
}

/// <summary>
/// JSON-serializable format for magic spell modifications.
/// </summary>
public interface IMagicSpellConfig
{
    /// <summary>
    /// The magic ID this configuration applies to.
    /// </summary>
    int MagicId { get; }
    
    /// <summary>
    /// Human-readable name for this spell configuration.
    /// </summary>
    string? Name { get; }
    
    /// <summary>
    /// Optional description of what this configuration does.
    /// </summary>
    string? Description { get; }
    
    /// <summary>
    /// When true, all existing OperationGroups for this MagicId will be removed
    /// before applying the modifications. This effectively replaces the original
    /// spell definition with the one described in this config.
    /// Defaults to false (merge with existing).
    /// </summary>
    bool ReplaceOriginal { get; }
    
    /// <summary>
    /// List of modifications to apply.
    /// </summary>
    IList<IMagicModificationConfig> Modifications { get; }
}

/// <summary>
/// JSON-serializable format for a single modification.
/// </summary>
public interface IMagicModificationConfig
{
    /// <summary>
    /// Type of modification.
    /// </summary>
    MagicModificationType Type { get; }
    
    /// <summary>
    /// The operation group ID where this modification applies.
    /// </summary>
    int OperationGroupId { get; }
    
    /// <summary>
    /// The operation type ID.
    /// </summary>
    int OperationId { get; }
    
    /// <summary>
    /// For property modifications: the single property ID.
    /// </summary>
    int? PropertyId { get; }
    
    /// <summary>
    /// For property modifications: the value.
    /// </summary>
    object? Value { get; }
    
    /// <summary>
    /// For AddOperation: properties with their values. Key = PropertyId, Value = property value.
    /// </summary>
    IDictionary<int, object>? Properties { get; }
    
    /// <summary>
    /// The operation type ID after which to inject this modification.
    /// -1 (or null/omitted) means inject at the end of the operation group.
    /// </summary>
    int? InsertAfterOperationTypeId { get; }
}
