using System.Numerics;

namespace FF16Framework.Interfaces.GameApis.Structs;

/// <summary>
/// Represents target information for magic casting.
/// Provides position, direction, and actor tracking data.
/// </summary>
public interface ITargetInfo
{
    /// <summary>
    /// Target position in world space.
    /// </summary>
    Vector3 Position { get; }
    
    /// <summary>
    /// Direction vector for the target.
    /// </summary>
    Vector3 Direction { get; }
    
    /// <summary>
    /// Actor ID for tracking. Used when targeting an actor.
    /// Zero if targeting a position only.
    /// </summary>
    int ActorId { get; }
}
