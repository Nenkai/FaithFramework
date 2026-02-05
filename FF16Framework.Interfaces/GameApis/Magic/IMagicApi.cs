using System;

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
    /// Casts a magic spell with optional source and target actors.
    /// </summary>
    /// <param name="magicId">The ID of the magic spell to cast.</param>
    /// <param name="sourceActor">
    /// The actor casting the spell. If null, defaults to the player (Clive).
    /// Use nint.Zero to explicitly cast without a source.
    /// </param>
    /// <param name="targetActor">
    /// The target actor for the spell. If null, defaults to the camera's soft/hard locked target.
    /// Use nint.Zero to explicitly cast without a target.
    /// </param>
    /// <returns>True if the spell was cast successfully.</returns>
    bool Cast(int magicId, nint? sourceActor = null, nint? targetActor = null);
    
    /// <summary>
    /// Casts a magic spell using the GAME'S OWN TargetStruct for the locked enemy.
    /// This is the correct way to get body-targeting (Y=1.23 vs Y=0.26).
    /// The game's targeting system already calculates the proper body position.
    /// </summary>
    /// <param name="magicId">The ID of the magic spell to cast.</param>
    /// <param name="sourceActor">The actor casting the spell. If null, defaults to player.</param>
    /// <returns>True if the spell was cast successfully.</returns>
    bool CastWithGameTarget(int magicId, nint? sourceActor = null);
    
    /// <summary>
    /// Gets the currently soft-locked or hard-locked target actor from the camera system.
    /// Returns nint.Zero if no target is locked.
    /// </summary>
    nint GetLockedTarget();
    
    /// <summary>
    /// Gets the player's (Clive's) actor pointer.
    /// </summary>
    nint GetPlayerActor();
    
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