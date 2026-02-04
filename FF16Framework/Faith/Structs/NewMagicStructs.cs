using System.Runtime.InteropServices;

namespace FF16Framework.Faith.Structs;

// ============================================================
// BATTLE MAGIC STRUCT
// ============================================================

/// <summary>
/// BattleMagic structure used by SetupMagic.
/// This is the 'this' parameter in faith::Battle::Magic::SetupMagic.
/// Size is approximately 0xE0 based on IDA analysis.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0xE0)]
internal unsafe struct BattleMagic
{
    /// <summary>
    /// VTable pointer - initialized to g_off_7FF6A3500598 in game code.
    /// </summary>
    [FieldOffset(0x00)] public nint VTable;
    
    /// <summary>
    /// Unknown qword at offset 0x08.
    /// </summary>
    [FieldOffset(0x08)] public long Field08;
    
    /// <summary>
    /// Position of the magic effect (Vec3).
    /// </summary>
    [FieldOffset(0x10)] public float PositionX;
    [FieldOffset(0x14)] public float PositionY;
    [FieldOffset(0x18)] public float PositionZ;
    
    [FieldOffset(0x1C)] public int Field1C;
    
    /// <summary>
    /// Nested structure at offset 0x20.
    /// </summary>
    [FieldOffset(0x20)] public long Qword20;
    [FieldOffset(0x28)] public long Qword28;
    [FieldOffset(0x30)] public long Qword30;
    [FieldOffset(0x38)] public long Qword38;
    
    /// <summary>
    /// Actor target information starts at 0x40.
    /// </summary>
    [FieldOffset(0x40)] public long ActorTarget_0;
    [FieldOffset(0x48)] public long ActorTarget_1;
    [FieldOffset(0x50)] public long ActorTarget_2;
    [FieldOffset(0x58)] public int ActorTarget_3;
    
    /// <summary>
    /// Parent node pointer.
    /// </summary>
    [FieldOffset(0x60)] public nint ParentNode;
    
    [FieldOffset(0x68)] public long Field68;
    [FieldOffset(0x70)] public long Field70;
    [FieldOffset(0x78)] public long Field78;
    [FieldOffset(0x80)] public long Field80;
    [FieldOffset(0x88)] public long Field88;
    [FieldOffset(0x90)] public long Field90;
    [FieldOffset(0x98)] public long Field98;
    [FieldOffset(0xA0)] public long FieldA0;
    [FieldOffset(0xA8)] public long FieldA8;
    [FieldOffset(0xB0)] public long FieldB0;
    [FieldOffset(0xB8)] public long FieldB8;
    
    /// <summary>
    /// Flags at offset 0xC0.
    /// </summary>
    [FieldOffset(0xC0)] public short FieldC0;  // Set to 1 in initialization
    [FieldOffset(0xC2)] public short FieldC2;  // Set to 0 in initialization
    
    [FieldOffset(0xC4)] public int FieldC4;
    
    /// <summary>
    /// MagicFileResource pointer.
    /// </summary>
    [FieldOffset(0xC8)] public nint MagicFileResource;
    
    /// <summary>
    /// Additional field at 0xD0.
    /// </summary>
    [FieldOffset(0xD0)] public long QwordD0;
    
    [FieldOffset(0xD8)] public long QwordD8;
    
    /// <summary>
    /// Initialize the struct with default values matching the game's initialization.
    /// </summary>
    public void Initialize()
    {
        VTable = 0;  // Should be set to g_off_7FF6A3500598 but we don't have access
        Field08 = 0;
        PositionX = 0;
        PositionY = 0;
        PositionZ = 0;
        Field1C = 0;
        Qword20 = 0;
        Qword28 = 0;
        Qword30 = 0;
        Qword38 = 0;
        ActorTarget_0 = 0;
        ActorTarget_1 = 0;
        ActorTarget_2 = 0;
        ActorTarget_3 = 0;
        ParentNode = 0;
        Field68 = 0;
        Field70 = 0;
        Field78 = 0;
        Field80 = 0;
        Field88 = 0;
        Field90 = 0;
        Field98 = 0;
        FieldA0 = 0;
        FieldA8 = 0;
        FieldB0 = 0;
        FieldB8 = 0;
        FieldC0 = 1;  // Default flag value
        FieldC2 = 0;
        FieldC4 = 0;
        MagicFileResource = 0;
        QwordD0 = 0;
        QwordD8 = 0;
    }
}

// ============================================================
// BATTLE BEHAVIOR ENTITY ENTRY
// ============================================================

/// <summary>
/// BattleBehaviorEntityEntry structure.
/// Contains the MagicFileResource used for spawning magic.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x4200)]
internal unsafe struct BattleBehaviorEntityEntry
{
    [FieldOffset(0x00)] public nint VTable;
    
    /// <summary>
    /// MagicFileResource (ResourceHandle) at offset known from IDA.
    /// The actual offset needs verification - in IDA it's accessed via btlBehaviorEntityEntry->MagicFileResource.
    /// </summary>
    [FieldOffset(0x10)] public nint MagicFileResource;
    
    /// <summary>
    /// Position struct maybe at offset 0x4160 (btlBehaviorEntityEntry->qword4160 in IDA).
    /// </summary>
    [FieldOffset(0x4160)] public nint PositionStructMaybe;
}
