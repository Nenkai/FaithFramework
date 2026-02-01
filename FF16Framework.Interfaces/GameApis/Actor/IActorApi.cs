using System;
using FF16Framework.Interfaces.GameApis.Structs;

namespace FF16Framework.Interfaces.GameApis.Actor;

/// <summary>
/// Interface for actor management operations used by the Magic API.
/// Provides access to player info, targeting, and actor lookups.
/// </summary>
public interface IActorApi
{
    // ============================================================
    // STATE PROPERTIES
    // ============================================================
    
    /// <summary>
    /// Returns true if all required singletons have been captured.
    /// </summary>
    bool IsInitialized { get; }
    
    /// <summary>
    /// Returns true if targeting functions are available.
    /// </summary>
    bool HasTargetingFunctions { get; }
    
    // ============================================================
    // PLAYER API
    // ============================================================
    
    /// <summary>
    /// Gets the StaticActorInfo pointer for the player (Clive).
    /// </summary>
    /// <returns>StaticActorInfo pointer, or nint.Zero if not available.</returns>
    nint GetPlayerStaticActorInfo();
    
    // ============================================================
    // TARGETING API
    // ============================================================
    
    /// <summary>
    /// Gets the currently locked target (soft/hard lock from camera) as StaticActorInfo.
    /// </summary>
    /// <returns>StaticActorInfo pointer of locked target, or nint.Zero if no target.</returns>
    nint GetLockedTargetStaticActorInfo();
    
    /// <summary>
    /// Copies the game's own TargetStruct for the currently locked enemy.
    /// This is the correct way to get body-targeting position (Y≈1.23 instead of Y≈0.26).
    /// The game's targeting system already calculates the correct position.
    /// </summary>
    /// <returns>A copy of the game's TargetStruct with Type=1 forced, or null if no target.</returns>
    TargetStruct? CopyGameTargetStruct();
    
    // ============================================================
    // ACTOR LOOKUP API
    // ============================================================
    
    /// <summary>
    /// Gets the ActorRef from a StaticActorInfo pointer.
    /// This is required for the SetupMagic caster parameter.
    /// </summary>
    /// <param name="staticActorInfo">The StaticActorInfo pointer.</param>
    /// <returns>ActorRef value for SetupMagic, or 0 if failed.</returns>
    long GetActorRef(nint staticActorInfo);
    
    // ============================================================
    // TARGET CREATION API
    // ============================================================
    
    /// <summary>
    /// Creates a TargetStruct from a StaticActorInfo with actor tracking.
    /// Uses Type=1 so the spell will follow/track the target.
    /// </summary>
    /// <param name="staticActorInfo">The target actor's StaticActorInfo pointer.</param>
    /// <returns>A TargetStruct configured for tracking, or null if failed.</returns>
    TargetStruct? CreateTargetFromActorWithTracking(nint staticActorInfo);
    
    /// <summary>
    /// Creates a TargetStruct from a StaticActorInfo's position only.
    /// Uses Type=0 (position-based, no tracking).
    /// </summary>
    /// <param name="staticActorInfo">The target actor's StaticActorInfo pointer.</param>
    /// <returns>A TargetStruct with position only, or null if failed.</returns>
    TargetStruct? CreateTargetFromActor(nint staticActorInfo);
    

    
    // ============================================================
    // ACTOR DATA API
    // ============================================================
    
    /// <summary>
    /// Gets the ActorData35Entry pointer for the player (Clive).
    /// This is required for Blind Justice (Ramuh) satellite count access.
    /// </summary>
    /// <returns>ActorData35Entry pointer, or 0 if not available.</returns>
    long GetPlayerActorData35Entry();
    
    // ============================================================
    // STATE DETECTION API
    // ============================================================
    
    /// <summary>
    /// Detects if an entity is currently airborne (not on the ground).
    /// Uses reverse-engineered offsets from state check logic.
    /// </summary>
    /// <param name="bnpcRow">Pointer to the NpcBaseEntity row.</param>
    /// <returns>True if the entity is airborne.</returns>
    bool IsAirborne(long bnpcRow);
}
