using System.Numerics;
using FF16Framework.Interfaces.GameApis.Structs;

namespace FF16Framework.Services.GameApis.Structs;

/// <summary>
/// Implementation of ITargetInfo wrapping internal TargetStruct data.
/// </summary>
internal readonly struct TargetInfo : ITargetInfo
{
    public Vector3 Position { get; }
    public Vector3 Direction { get; }
    public int ActorId { get; }
    
    public TargetInfo(Vector3 position, Vector3 direction, int actorId)
    {
        Position = position;
        Direction = direction;
        ActorId = actorId;
    }
    
    /// <summary>
    /// Creates a TargetInfo for a static position (no actor tracking).
    /// </summary>
    public static TargetInfo FromPosition(Vector3 position)
    {
        return new TargetInfo(position, new Vector3(0, 0, 1), 0);
    }
    
    /// <summary>
    /// Creates a TargetInfo for a position with custom direction.
    /// </summary>
    public static TargetInfo FromPositionAndDirection(Vector3 position, Vector3 direction)
    {
        return new TargetInfo(position, direction, 0);
    }
    
    /// <summary>
    /// Creates a TargetInfo for tracking an actor.
    /// </summary>
    public static TargetInfo FromActor(Vector3 position, int actorId)
    {
        return new TargetInfo(position, new Vector3(0, 0, 1), actorId);
    }
}
