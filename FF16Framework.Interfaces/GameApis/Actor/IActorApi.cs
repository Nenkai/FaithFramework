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
    /// Gets target info for the currently locked enemy.
    /// This is the correct way to get body-targeting position.
    /// </summary>
    /// <returns>Target info with position and actor ID, or null if no target.</returns>
    ITargetInfo? CopyGameTargetInfo();
    
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
    /// Creates target info from a StaticActorInfo with actor tracking.
    /// The returned info includes the actor ID for spell tracking.
    /// </summary>
    /// <param name="staticActorInfo">The target actor's StaticActorInfo pointer.</param>
    /// <returns>Target info with actor tracking, or null if failed.</returns>
    ITargetInfo? CreateTargetFromActorWithTracking(nint staticActorInfo);
    
    /// <summary>
    /// Creates target info from a StaticActorInfo's position only.
    /// No actor tracking (position-based targeting).
    /// </summary>
    /// <param name="staticActorInfo">The target actor's StaticActorInfo pointer.</param>
    /// <returns>Target info with position only, or null if failed.</returns>
    ITargetInfo? CreateTargetFromActor(nint staticActorInfo);
    
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
