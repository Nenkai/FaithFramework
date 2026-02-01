using System.Runtime.InteropServices;

namespace FF16Framework.Interfaces.GameApis.Structs;

/// <summary>
/// Target position structure passed to SetupMagic.
/// Mirroring the game's internal TargetStruct layout.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x7C)]
public unsafe struct TargetStruct
{
    /// <summary>
    /// VTable pointer.
    /// </summary>
    [FieldOffset(0x00)] public nint VTable;
    
    [FieldOffset(0x08)] public long Field_8;
    [FieldOffset(0x10)] public long Field_10;
    [FieldOffset(0x18)] public long Field_18;
    
    /// <summary>
    /// Pointer to some global offset.
    /// </summary>
    [FieldOffset(0x20)] public nint GlobalOffset;
    
    /// <summary>
    /// Pointer to faith::Node for relative positioning.
    /// </summary>
    [FieldOffset(0x28)] public nint Node;
    
    /// <summary>
    /// Target position in world space.
    /// </summary>
    [FieldOffset(0x30)] public float X;
    [FieldOffset(0x34)] public float Y;
    [FieldOffset(0x38)] public float Z;
    
    [FieldOffset(0x3C)] public int Dword1C;
    
    /// <summary>
    /// Direction or secondary position.
    /// </summary>
    [FieldOffset(0x40)] public float DirectionX;
    [FieldOffset(0x44)] public float DirectionY;
    [FieldOffset(0x48)] public float DirectionZ;
    
    [FieldOffset(0x4C)] public int Padding4C;
    
    /// <summary>
    /// Type of target. 1 = Follow actor, 0 = Position only.
    /// </summary>
    [FieldOffset(0x50)] public int Type;
    
    [FieldOffset(0x54)] public int Field_54;
    [FieldOffset(0x58)] public int Field_58;
    [FieldOffset(0x5C)] public int Field_5C;
    [FieldOffset(0x60)] public int Field_60;
    [FieldOffset(0x64)] public int Field_64;
    [FieldOffset(0x68)] public int Field_68;
    
    /// <summary>
    /// Entity ID for tracking. Matches StaticActorInfo.ActorId.
    /// Used when Type = 1.
    /// </summary>
    [FieldOffset(0x6C)] public int ActorId;
    
    [FieldOffset(0x70)] public float Field_70;
    [FieldOffset(0x74)] public int Field_74;
    [FieldOffset(0x78)] public int Field_78;

    // Helpers
    public static TargetStruct FromPosition(System.Numerics.Vector3 position)
    {
        return new TargetStruct
        {
            Type = 0,
            Node = 0,
            X = position.X,
            Y = position.Y,
            Z = position.Z,
            DirectionX = 0,
            DirectionY = 0,
            DirectionZ = 1
        };
    }

    public static TargetStruct FromPositionAndDirection(System.Numerics.Vector3 position, System.Numerics.Vector3 direction)
    {
        return new TargetStruct
        {
            Type = 0,
            Node = 0,
            X = position.X,
            Y = position.Y,
            Z = position.Z,
            DirectionX = direction.X,
            DirectionY = direction.Y,
            DirectionZ = direction.Z
        };
    }

    public static TargetStruct FromActorId(int actorId, System.Numerics.Vector3 position, int targetType = 1)
    {
        return new TargetStruct
        {
            Type = targetType,
            Node = 0,
            X = position.X,
            Y = position.Y,
            Z = position.Z,
            ActorId = actorId,
            DirectionX = 0,
            DirectionY = 0,
            DirectionZ = 1
        };
    }
}
