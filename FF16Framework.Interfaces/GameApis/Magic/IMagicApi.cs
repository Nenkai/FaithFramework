using System;
using FF16Framework.Interfaces.GameApis.Structs;

namespace FF16Framework.Interfaces.GameApis.Magic;

/// <summary>
/// Public interface for the Magic API.
/// This interface is designed to be stable and should not change once released.
/// Other mods can depend on this interface to cast magic spells.
/// 
/// Magic Structure in FF16:
/// - Magic → OperationGroups → Operations → Properties
/// - Each Magic has multiple OperationGroups (blocks of operations executed together)
/// - Each OperationGroup has multiple Operations (individual behaviors)
/// - Each Operation has multiple Properties (configuration values)
/// </summary>
public interface IMagicApi
{
    /// <summary>
    /// Returns true if the magic system has captured the necessary game context to cast spells.
    /// The context is captured automatically when any magic spell is cast in-game.
    /// </summary>
    bool IsReady { get; }
    
    /// <summary>
    /// Creates a new magic spell builder for the specified magic ID.
    /// Use <see cref="MagicIds"/> for known magic IDs.
    /// </summary>
    /// <param name="magicId">The ID of the magic spell to cast.</param>
    /// <returns>A builder for configuring and casting the spell.</returns>
    IMagicBuilder CreateSpell(int magicId);
    
    /// <summary>
    /// Casts a magic spell with the specified source and target actors.
    /// </summary>
    /// <param name="magicId">The ID of the magic spell to cast.</param>
    /// <param name="source">The actor casting the spell. Defaults to Player.</param>
    /// <param name="target">The target actor. Defaults to LockedTarget (camera's soft/hard locked target).</param>
    /// <returns>True if the spell was cast successfully.</returns>
    bool Cast(int magicId, ActorSelection source = ActorSelection.Player, ActorSelection target = ActorSelection.LockedTarget);
    
    /// <summary>
    /// Creates a spell builder from a JSON configuration.
    /// </summary>
    /// <param name="json">JSON string containing spell modifications.</param>
    /// <returns>A builder with the imported modifications, or null if parsing failed.</returns>
    IMagicBuilder? ImportFromJson(string json);
    
    /// <summary>
    /// Creates a spell builder from a JSON file.
    /// </summary>
    /// <param name="filePath">Path to the JSON file.</param>
    /// <returns>A builder with the imported modifications, or null if loading failed.</returns>
    IMagicBuilder? ImportFromFile(string filePath);
}