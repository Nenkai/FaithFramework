using System.Numerics;
using System.Runtime.InteropServices;

namespace FF16Framework.Faith.Structs;

/// <summary>
/// Runtime structures for the magic system.
/// These structs mirror the game's internal memory layout for hooks and casting.
/// For .magic file parsing, use FF16Tools.Files.Magic.MagicFile instead.
/// </summary>
/// 
// ============================================================
// POINTER VALIDATION
// ============================================================

/// <summary>
/// Constants for validating pointers in x64 user-mode address space.
/// </summary>
public static class PointerValidation
{
    public const long MIN_VALID_ADDRESS = 0x10000;
    public const long MAX_VALID_ADDRESS = 0x00007FFFFFFFFFFF;
    
    public static bool IsValidPointer(long ptr) => 
        ptr >= MIN_VALID_ADDRESS && ptr <= MAX_VALID_ADDRESS && ptr % 8 == 0;
    
    public static bool IsValidPointer(nint ptr) => IsValidPointer((long)ptr);
}

// ============================================================
// MAGIC ID VALIDATION
// ============================================================

/// <summary>
/// Constants for validating magic/group IDs.
/// </summary>
public static class MagicIdRanges
{
    public const int MIN_VALID_ID = 1;
    public const int MAX_VALID_ID = 30000;
    public const int MAX_EXTENDED_ID = 1000000;
    
    public static bool IsValidId(int id) => id > MIN_VALID_ID && id < MAX_VALID_ID;
    public static bool IsValidExtendedId(int id) => id > MIN_VALID_ID && id < MAX_EXTENDED_ID;
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

// TargetStruct moved to FF16Framework.Interfaces.GameApis.Structs

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
    
    public readonly bool IsValid => PointerValidation.IsValidPointer(VTable);
    public readonly bool HasValidMagicId => MagicIdRanges.IsValidId(MagicId);
    public readonly bool HasValidGroupId => MagicIdRanges.IsValidId(GroupId) && GroupId != MagicId;
}

