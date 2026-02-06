using System.Numerics;
using System.Runtime.InteropServices;

namespace FF16Framework.Faith.Structs;

// ============================================================
// MAGIC ID VALIDATION
// ============================================================

/// <summary>
/// Constants for validating magic IDs.
/// Vanilla IDs are within a known range; modded IDs exceed that range.
/// </summary>
public static class MagicIdRanges
{
    public const int MIN_VANILLA_ID = 1;
    public const int MAX_VANILLA_ID = 30000;
    
    public static bool IsVanillaId(int id) => id > MIN_VANILLA_ID && id < MAX_VANILLA_ID;
    public static bool IsModdedId(int id) => id > MAX_VANILLA_ID;
}

// ============================================================
// MAGIC PROPERTY DATA
// ============================================================

/// <summary>
/// Property data passed to MagicFileInstance_CreateOperationAndApplyProperties.
/// The actual value is at dataPtr + 8 (pointer indirection).
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct MagicPropertyData
{
    [FieldOffset(0x00)] public long Unknown;
    [FieldOffset(0x08)] public void* ValuePtr;
    
    public readonly float AsFloat => *(float*)ValuePtr;
    public readonly int AsInt => *(int*)ValuePtr;
    public readonly bool AsBool => *(int*)ValuePtr != 0;
    public readonly Vector3 AsVec3 => *(Vector3*)ValuePtr;
    
    public void SetFloat(float value) => *(float*)ValuePtr = value;
    public void SetInt(int value) => *(int*)ValuePtr = value;
    public void SetVec3(Vector3 value) => *(Vector3*)ValuePtr = value;
}

// ============================================================
// MAGIC INPUT CONFIG (for Charged Shots)
// ============================================================

/// <summary>
/// Shot type enumeration for magic projectiles.
/// </summary>
public enum MagicShotType : int
{
    Normal = 1,
    Charged = 2,
    Precision = 3,
    Burst = 4
}

/// <summary>
/// Offsets for MagicManager structure used in FireMagicProjectile.
/// </summary>
public static class MagicManagerOffsets
{
    public const int VTable = 0x00;
    public const int InputConfigPtr = 0x38;
}

/// <summary>
/// Offsets for MagicInputConfig structure.
/// </summary>
public static class MagicInputConfigOffsets
{
    public const int VTable = 0x00;
    public const int ShotType = 0x10;  // 1=Normal, 2=Charged, 3=Precision, 4=Burst
}

/// <summary>
/// Helper methods for reading MagicManager data from pointers.
/// </summary>
public static unsafe class MagicManagerHelper
{
    public static long GetInputConfigPtr(long magicManagerPtr)
    {
        if (magicManagerPtr == 0) return 0;
        return *(long*)(magicManagerPtr + MagicManagerOffsets.InputConfigPtr);
    }
    
    public static int GetShotType(long inputConfigPtr)
    {
        if (inputConfigPtr == 0) return 0;
        return *(int*)(inputConfigPtr + MagicInputConfigOffsets.ShotType);
    }
    
    public static int GetShotTypeFromManager(long magicManagerPtr)
    {
        long inputConfigPtr = GetInputConfigPtr(magicManagerPtr);
        return GetShotType(inputConfigPtr);
    }
    
    public static bool HasValidInputConfig(long magicManagerPtr)
    {
        return GetInputConfigPtr(magicManagerPtr) != 0;
    }
}

// ============================================================
// TARGET STRUCT (UnkTargetStruct in IDA)
// ============================================================

/// <summary>
/// Target position structure passed to SetupMagic.
/// Based on IDA analysis of faith::Battle::Magic::SetupMagic.
/// Size: 0x7C bytes
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x7C)]
public unsafe struct TargetStruct
{
    [FieldOffset(0x00)] public nint VTable;
    [FieldOffset(0x08)] public long Field_8;
    [FieldOffset(0x10)] public long Field_10;
    [FieldOffset(0x18)] public long Field_18;
    
    /// <summary>
    /// Pointer to some global offset (p_g_off_7FF6A3500598 in IDA).
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
    /// Direction vector.
    /// </summary>
    [FieldOffset(0x40)] public float DirectionX;
    [FieldOffset(0x44)] public float DirectionY;
    [FieldOffset(0x48)] public float DirectionZ;
    
    [FieldOffset(0x4C)] public int Padding4C;
    
    /// <summary>
    /// Type of target. Used for targeting mode.
    /// </summary>
    [FieldOffset(0x50)] public int Type;
    
    [FieldOffset(0x54)] public int Field_54;
    [FieldOffset(0x58)] public int Field_58;
    [FieldOffset(0x5C)] public int Field_5C;
    [FieldOffset(0x60)] public int Field_60;
    [FieldOffset(0x64)] public int Field_64;
    [FieldOffset(0x68)] public int Field_68;
    
    /// <summary>
    /// Target actor ID.
    /// </summary>
    [FieldOffset(0x6C)] public int ActorId;
    
    [FieldOffset(0x70)] public float Field_70;
    [FieldOffset(0x74)] public int Field_74;
    [FieldOffset(0x78)] public int Field_78;
    
    /// <summary>
    /// World position as Vector3.
    /// </summary>
    public Vector3 Position
    {
        readonly get => new(X, Y, Z);
        set { X = value.X; Y = value.Y; Z = value.Z; }
    }
    
    /// <summary>
    /// Direction as Vector3.
    /// </summary>
    public Vector3 Direction
    {
        readonly get => new(DirectionX, DirectionY, DirectionZ);
        set { DirectionX = value.X; DirectionY = value.Y; DirectionZ = value.Z; }
    }
    
    public static TargetStruct FromPosition(Vector3 position)
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
    
    public static TargetStruct FromPositionAndDirection(Vector3 position, Vector3 direction)
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
    
    public static TargetStruct FromActorId(int actorId, Vector3 position, int targetType = 1)
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

// ============================================================
// MAGIC FILE INSTANCE (Runtime only, for hook context)
// ============================================================

/// <summary>
/// Represents a MagicFile instance at runtime.
/// For parsing .magic files, use FF16Tools.Files.Magic.MagicFile instead.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x208)]
public unsafe struct MagicFileInstance
{
    [FieldOffset(0x000)] public long VTable;
    [FieldOffset(0x200)] public int MagicId;
    [FieldOffset(0x204)] public int GroupId;
    
    public readonly bool IsValid => VTable != 0;
    public readonly bool HasValidMagicId => MagicIdRanges.IsVanillaId(MagicId) || MagicIdRanges.IsModdedId(MagicId);
}

