using System.Numerics;
using System.Runtime.InteropServices;

namespace FF16Framework.Faith.Structs;

// ============================================================
// GLOBAL OFFSETS (from base address)
// ============================================================

/// <summary>
/// Global memory offsets from the game's base address.
/// These point to singleton instances or global state.
/// </summary>
internal static class GlobalOffsets
{
    /// <summary>
    /// Singleton that contains player/camera related state.
    /// The current controlling ActorId is at +0xC8.
    /// </summary>
    public const int UnkSingletonPlayerOrCamera = 0x1816608;
    
    /// <summary>
    /// The BattleMagicExecutor singleton used for casting magic spells.
    /// </summary>
    public const int BattleMagicExecutor = 0x18168E8;
}

// ============================================================
// PLAYER/CAMERA SINGLETON
// ============================================================

/// <summary>
/// Offsets within the UnkSingletonPlayerOrCamera structure.
/// </summary>
internal static class UnkSingletonOffsets
{
    /// <summary>
    /// The ActorId of the currently controlled actor (usually Clive).
    /// Type: uint (DWORD)
    /// 
    /// From IDA: *((_DWORD *)g_UnkSingletonPlayer + 50)
    /// Calculation: 50 * sizeof(DWORD) = 50 * 4 = 200 = 0xC8
    /// </summary>
    public const int CurrentActorId = 0xC8;
}

// ============================================================
// ENTITY VTABLE
// ============================================================

/// <summary>
/// VTable function indices for entity objects (StaticActorInfo, etc).
/// These are ref-counted objects with AddRef/Release semantics.
/// </summary>
internal static class EntityVTableOffsets
{
    /// <summary>
    /// Release/Decref function (index 4, offset 0x20).
    /// Called to decrement reference count after using GetOrCreateByActorId.
    /// 
    /// From IDA: (*(void (__fastcall **)(__int64))(*(_QWORD *)entity + 32LL))(entity)
    /// </summary>
    public const int Release = 0x20; // VTable index 4
}

// ============================================================
// PLAYER STATE
// ============================================================

/// <summary>
/// Offsets within the PlayerState structure.
/// </summary>
internal static class PlayerStateOffsets
{
    /// <summary>
    /// Offset to the EikonSummonData array/structure.
    /// Used for checking if a specific Eikon mode is active.
    /// </summary>
    public const int EikonSummonData = 0x4798;
    
    /// <summary>
    /// Offset to the Abyssal Tear structure (Leviathan VentGauge system).
    /// This is a FIXED location - does NOT require Leviathan mode to be active.
    /// Used by Action1094_AbyssalTear::HandleEventId: sub_7FF6DD537E0C(&g_UnkSingletonPlayer->qword1C888, ...)
    /// </summary>
    public const int AbyssalTearBase = 0x1C888;
}

// ============================================================
// ABYSSAL TEAR STRUCTURE (Fixed location in PlayerState)
// ============================================================

/// <summary>
/// Offsets within the Abyssal Tear structure.
/// Base: playerState + PlayerStateOffsets.AbyssalTearBase (0x1C888)
/// 
/// This structure is ALWAYS available and does NOT require Leviathan mode.
/// The Abyssal Tear gauge can build up anytime while fighting.
/// 
/// How it works:
/// - When you start attacking, State changes to 2 (charging)
/// - Gauge accumulates TIME IN SECONDS (not damage units like Zantetsuken)
/// - CurrentLevel increases as time thresholds are reached
/// - When you release Abyssal Tear, State changes to 4 (executed)
/// - State returns to 0 when inactive
/// 
/// </summary>
internal static class AbyssalTearOffsets
{
    /// <summary>
    /// Time in SECONDS the ability has been active (float).
    /// This is NOT damage units - it accumulates time while charging.
    /// From IDA sub_7FF6DD53861C: dword ptr [rcx+378h]
    /// </summary>
    public const int Gauge = 0x378; // 888 decimal
    
    /// <summary>
    /// State byte - indicates Abyssal Tear status.
    /// Values:
    ///   0 = Inactive (not charging)
    ///   2 = Charging (Vent Gauge active, accumulating levels)
    ///   4 = Executed (ability fired, consumed the levels)
    /// From IDA: *((_BYTE *)p_qword1C888 + 892)
    /// </summary>
    public const int State = 0x37C; // 892 decimal
    
    /// <summary>
    /// Maximum level for current activation (byte).
    /// Set from Skill::GetPotencyParameter when Abyssal Tear activates.
    /// Typically 3 or 4 depending on skill upgrades.
    /// From IDA: *((_BYTE *)p_qword1C888 + 893) = n3
    /// </summary>
    public const int MaxLevel = 0x37D; // 893 decimal
    
    /// <summary>
    /// Current level progress (byte).
    /// Increments as time thresholds are reached while charging.
    /// Ranges from 0 to MaxLevel.
    /// From IDA: *((_BYTE *)p_qword1C888 + 894)
    /// </summary>
    public const int CurrentLevel = 0x37E; // 894 decimal
}

// ============================================================
// ODIN EIKON STRUCTURE
// ============================================================

/// <summary>
/// Offsets within the Odin Eikon structure.
/// </summary>
internal static class OdinEikonOffsets
{
    /// <summary>
    /// Zantetsuken gauge value (0-7500, where 1500 = 1 bar level).
    /// Type: short (__int16)
    /// </summary>
    public const int ZantetsukenGauge = 0x1C08; // 7176 decimal
}

// ============================================================
// RAMUH / BLIND JUSTICE
// ============================================================

/// <summary>
/// Offsets within ActorData35Entry for Blind Justice (Ramuh satellite system).
/// 
/// Unlike other Eikon gauges, Blind Justice lock count is stored in ActorData35Entry,
/// not in the Eikon summon structure.
/// 
/// From IDA: GetBlindJusticeCurrentLockCount returns ActorData35Entry + 224 (0xE0)
/// Max level comes from Skill::GetPotencyParameter(skill_29)
/// PlayerMode 74 = Blind Justice active
/// </summary>
internal static class ActorData35Offsets
{
    /// <summary>
    /// Blind Justice current lock-on count (number of satellites).
    /// Type: int
    /// From IDA: (ActorData35Entry + 224) & -(__int64)(ActorData35Entry != 0)
    /// </summary>
    public const int BlindJusticeLockCount = 0xE0; // 224 decimal
}

// ============================================================
// ACTOR DATA 35 ENTRY STRUCTURE
// ============================================================

/// <summary>
/// ActorData35Entry - Main actor state structure (sizeof = 0xA98).
/// Contains player mode, Eikon abilities state, and various combat data.
/// 
/// Retrieved via StaticActorInfo::GetActorData35Entry()
/// 
/// From IDA:
/// struct __fixed ActorData35Entry // sizeof=0xA98
/// {
///     ActorData35Entry_vtbl *__vftable;
///     float field_8;
///     __int64 field_10;
///     __int64 field_18;
///     int field_20;
///     unsigned int unsigned_int24;
///     int field_28;
///     int field_2C;
///     int field_30;
///     int field_34;
///     ActorData35EntrySub Sub;  // at 0x38, extends to 0xA98
/// };
/// 
/// PlayerMode List:
/// - field_8 to field_20 form a linked list structure for active PlayerModes
/// - IsPlayerModeActive() iterates this list checking if a specific mode is active
/// - Known PlayerModes: 74 = Blind Justice, 75 = Wings of Light
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0xA98)]
public unsafe struct ActorData35Entry
{
    /// <summary>VTable pointer</summary>
    [FieldOffset(0x00)] public nint VTable;
    
    // ============================================================
    // PlayerMode Linked List (0x08 - 0x28)
    // 
    // Structure of a PlayerMode list node:
    //   +0x18 (24): Next node pointer (or use +0x20 in some paths)
    //   +0x30 (48): Pointer to NexRowInstance
    //   NexRowInstance+0x08: Pointer to int containing PlayerMode ID
    //
    // GetCurrentPlayerMode flow:
    //   1. Get first node from field_28: (field_28 - 24) if field_28 != 0
    //   2. Read node+48 to get NexRowInstance ptr
    //   3. Read NexRowInstance+8 to get ptr to PlayerMode ID
    //   4. Return **(node+48+8) as the current PlayerMode value
    // ============================================================
    
    /// <summary>
    /// PlayerMode linked list sentinel/head.
    /// Used as: p_actorId = &amp;this->field_8 (sentinel for list iteration)
    /// IsPlayerModeActive() checks if (node == &amp;field_8) to end iteration.
    /// </summary>
    [FieldOffset(0x08)] public nint PlayerModeListSentinel;
    
    /// <summary>
    /// Part of PlayerMode linked list structure.
    /// </summary>
    [FieldOffset(0x10)] public nint PlayerModeListField10;
    
    /// <summary>
    /// Part of PlayerMode linked list structure.
    /// </summary>
    [FieldOffset(0x18)] public nint PlayerModeListField18;
    
    /// <summary>
    /// Checked for empty list condition.
    /// When field_20 == &amp;field_20, the list is empty.
    /// </summary>
    [FieldOffset(0x20)] public nint PlayerModeListField20;
    
    /// <summary>
    /// Pointer to the current/first PlayerMode list node.
    /// 
    /// GetCurrentPlayerMode uses this when a2=0:
    ///   return (field_28 - 24) &amp; -(field_28 != 0)
    /// 
    /// This gives the first node of the active PlayerModes list.
    /// Type: QWORD (pointer)
    /// </summary>
    [FieldOffset(0x28)] public nint PlayerModeFirstNode;
    
    /// <summary>Unknown int</summary>
    [FieldOffset(0x30)] public int Field_30;
    
    /// <summary>Unknown int</summary>
    [FieldOffset(0x34)] public int Field_34;
    
    // ============================================================
    // ActorData35EntrySub starts at 0x38, extends to 0xA98
    // Fields below are within the Sub structure but accessed as
    // offsets from the base ActorData35Entry pointer.
    // 
    // Example: BlindJusticeLockCount at 0xE0 means:
    //   - Offset 0xE0 from ActorData35Entry base
    //   - Offset 0xA8 from ActorData35EntrySub base (0xE0 - 0x38)
    // ============================================================
    
    /// <summary>
    /// Blind Justice (Ramuh) current satellite/lock-on count.
    /// Type: int
    /// 
    /// Location: ActorData35Entry + 0xE0 (224 decimal)
    /// Within Sub: ActorData35EntrySub + 0xA8 (168 decimal)
    /// 
    /// From IDA GetBlindJusticeCurrentLockCount:
    ///   1. Gets player's ActorData35Entry via StaticActorInfo::GetActorData35Entry
    ///   2. Returns pointer to (ActorData35Entry + 0xE0)
    ///   3. Caller reads int at that address for satellite count
    /// 
    /// Unlike other Eikon gauges (Odin, Bahamut, Leviathan) which are stored
    /// in the EikonPtr summon structure, Blind Justice stores its state
    /// directly in the actor's ActorData35Entry.
    /// </summary>
    [FieldOffset(0xE0)] public int BlindJusticeLockCount;
    
    // ============================================================
    // Additional fields from ActorData35EntrySub (at Entry offsets)
    // These are within Sub but accessed from Entry base pointer
    // ============================================================
    
    /// <summary>
    /// Pointer to ActorReference (secondary).
    /// Entry offset: 0x798, Sub offset: 0x760
    /// </summary>
    [FieldOffset(0x798)] public nint ActorRef2;
    
    /// <summary>
    /// Pointer to BattleBehaviorEntityEntry.
    /// Entry offset: 0x7A0, Sub offset: 0x768
    /// Contains combat behavior state.
    /// </summary>
    [FieldOffset(0x7A0)] public nint BattleBehaviorEntityEntry;
    
    /// <summary>
    /// Pointer to NexRowInstance for PlayerTargetParam.
    /// Entry offset: 0x7B0, Sub offset: 0x778
    /// </summary>
    [FieldOffset(0x7B0)] public nint PlayerTargetParamRow;
    
    /// <summary>Player target parameter ID 0. Entry offset: 0x7B8</summary>
    [FieldOffset(0x7B8)] public int PlayerTargetParamId0;
    
    /// <summary>Player target parameter ID 1. Entry offset: 0x7BC</summary>
    [FieldOffset(0x7BC)] public int PlayerTargetParamId1;
    
    /// <summary>
    /// Pointer to AccessRow2.
    /// Entry offset: 0x920, Sub offset: 0x8E8
    /// </summary>
    [FieldOffset(0x920)] public nint AccessRow2;
    
    /// <summary>
    /// Pointer to ActorReference (primary).
    /// Entry offset: 0x9A8, Sub offset: 0x970
    /// </summary>
    [FieldOffset(0x9A8)] public nint ActorRef1;
    
    /// <summary>
    /// Pointer to List3Entry.
    /// Entry offset: 0xA20, Sub offset: 0x9E8
    /// </summary>
    [FieldOffset(0xA20)] public nint List3Entry;
    
    /// <summary>
    /// Pointer to curve data.
    /// Entry offset: 0xA28, Sub offset: 0x9F0
    /// </summary>
    [FieldOffset(0xA28)] public nint CurveData;
    
    // ============================================================
    // Helper to get Sub pointer
    // ============================================================
    
    /// <summary>
    /// Gets a pointer to the embedded ActorData35EntrySub structure.
    /// The Sub starts at offset 0x38 within this Entry.
    /// 
    /// Usage:
    ///   ActorData35Entry* entry = ...;
    ///   ActorData35EntrySub* sub = entry->GetSub();
    /// </summary>
    public readonly ActorData35EntrySub* GetSub()
    {
        fixed (ActorData35Entry* self = &this)
        {
            return (ActorData35EntrySub*)((byte*)self + 0x38);
        }
    }
}

// ============================================================
// ACTOR DATA 35 ENTRY SUB STRUCTURE
// ============================================================

/// <summary>
/// ActorData35EntrySub - Sub-structure embedded within ActorData35Entry at offset 0x38.
/// Contains combat behavior, targeting, and ability state data.
/// 
/// Size: 0xA60 (extends from 0x38 to 0xA98 within ActorData35Entry)
/// 
/// Offset conversion:
/// - ActorData35Entry offset = ActorData35EntrySub offset + 0x38
/// - Example: Sub.BlindJusticeLockCount (0xA8) = Entry offset 0xE0
/// 
/// Note: Most fields are also accessible directly from ActorData35Entry
/// using the Entry offsets. This struct exists for documentation and
/// when you have a direct Sub pointer.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0xA60)]
public unsafe struct ActorData35EntrySub
{
    // ============================================================
    // Identified fields only - add more as they are discovered
    // ============================================================
    
    /// <summary>
    /// Blind Justice (Ramuh) satellite/lock-on count.
    /// Sub offset: 0xA8, Entry offset: 0xE0
    /// </summary>
    [FieldOffset(0xA8)] public int BlindJusticeLockCount;
    
    /// <summary>
    /// Pointer to ActorReference (secondary).
    /// Sub offset: 0x760, Entry offset: 0x798
    /// </summary>
    [FieldOffset(0x760)] public nint ActorRef2;
    
    /// <summary>
    /// Pointer to BattleBehaviorEntityEntry.
    /// Sub offset: 0x768, Entry offset: 0x7A0
    /// </summary>
    [FieldOffset(0x768)] public nint BattleBehaviorEntityEntry;
    
    /// <summary>
    /// Pointer to NexRowInstance for PlayerTargetParam.
    /// Sub offset: 0x778, Entry offset: 0x7B0
    /// </summary>
    [FieldOffset(0x778)] public nint PlayerTargetParamRow;
    
    /// <summary>Player target parameter ID 0. Sub offset: 0x780</summary>
    [FieldOffset(0x780)] public int PlayerTargetParamId0;
    
    /// <summary>Player target parameter ID 1. Sub offset: 0x784</summary>
    [FieldOffset(0x784)] public int PlayerTargetParamId1;
    
    /// <summary>
    /// Pointer to AccessRow2.
    /// Sub offset: 0x8E8, Entry offset: 0x920
    /// </summary>
    [FieldOffset(0x8E8)] public nint AccessRow2;
    
    /// <summary>
    /// Pointer to ActorReference (primary).
    /// Sub offset: 0x970, Entry offset: 0x9A8
    /// </summary>
    [FieldOffset(0x970)] public nint ActorRef1;
    
    /// <summary>
    /// Pointer to List3Entry.
    /// Sub offset: 0x9E8, Entry offset: 0xA20
    /// </summary>
    [FieldOffset(0x9E8)] public nint List3Entry;
    
    /// <summary>
    /// Pointer to curve data.
    /// Sub offset: 0x9F0, Entry offset: 0xA28
    /// </summary>
    [FieldOffset(0x9F0)] public nint CurveData;
}

/// <summary>
/// Known PlayerMode values used with ActorData35Entry functions.
/// 
/// Two related functions exist:
/// 
/// 1. IsPlayerModeActive(uint playerMode) - Returns bool
///    Iterates the PlayerMode linked list checking if a specific mode is active.
///    Used for: Checking if Blind Justice satellites are active
///    Example: IsPlayerModeActive(74) for Blind Justice
/// 
/// 2. GetCurrentPlayerMode() - Returns uint
///    Returns the current active player mode value.
///    Used for: Checking Wings of Light in Megaflare UI update
///    Example: GetCurrentPlayerMode() == 75 for Wings of Light
/// 
/// Note: Megaflare gauge (0x1C38) is stored in EikonPtr (Bahamut structure),
/// NOT in ActorData35Entry. Only Wings activation check uses ActorData35Entry.
/// </summary>
public static class PlayerModes
{
    /// <summary>Unknown mode checked in some code paths</summary>
    public const uint Unknown_0x48 = 0x48; // 72 decimal
    
    /// <summary>Ramuh's Blind Justice satellite mode</summary>
    public const uint BlindJustice = 74;
    
    /// <summary>
    /// Bahamut's Wings of Light mode.
    /// Checked via: ActorData35Entry::GetCurrentPlayerMode() == 75
    /// Used in Megaflare UI to show wings-activated state.
    /// </summary>
    public const uint WingsOfLight = 75;
}

// ============================================================
// BAHAMUT EIKON STRUCTURE
// ============================================================

/// <summary>
/// Offsets within the Bahamut Eikon structure.
/// Used for Megaflare gauge management.
/// 
/// Megaflare system:
/// - 4000 units = 1 level
/// - Max level depends on skill potency (typically 1-4)
/// 
/// Note: Wings activation is NOT stored here.
/// It's checked via ActorData35Entry::GetCurrentPlayerMode() == 75
/// </summary>
internal static class BahamutEikonOffsets
{
    /// <summary>
    /// Megaflare gauge total units.
    /// Level = units / 4000, UnitsInLevel = units % 4000
    /// Type: float (dword, converted to int via vcvttss2si)
    /// </summary>
    public const int MegaflareGauge = 0x1C38; // 7224 decimal
    
    /// <summary>
    /// Unknown state byte (checked == 2 for some UI trigger).
    /// Type: byte
    /// </summary>
    public const int UnkState1C4A = 0x1C4A; // 7242 decimal
    
    /// <summary>
    /// Unknown state byte (checked for some condition).
    /// Type: byte
    /// </summary>
    public const int UnkState1C4B = 0x1C4B; // 7243 decimal
}

// ============================================================
// LEVIATHAN EIKON STRUCTURE (DLC) - For Serpent's Cry
// ============================================================

/// <summary>
/// Offsets within the Leviathan Eikon structure (from IsSummonModeActive).
/// These offsets are for SERPENT'S CRY and require Leviathan mode to be active.
/// 
/// For ABYSSAL TEAR, use AbyssalTearOffsets (fixed location at playerState + 0x1C888).
/// 
/// Serpent's Cry system (from BattleUi::UpdateLeviathan):
/// - VentGauge at 0x1C08 (appears to be decrement tracking, not gauge value)
/// - Unlimited Tidal seconds timer at 0x1C0C as float
/// - Tidal recovery timer at 0x1C10
/// - TidalUnitsUsed at 0x1C18 as ushort
/// - State bytes at 0x1C1C-0x1C1F
/// 
/// Key functions discovered via IDA MCP:
/// - sub_7FF6DD537428: Main update function, decrements timers and TidalUnits
/// - sub_7FF6DD537748: Returns TidalUnitsUsed at offset 0x1C18
/// - sub_7FF6DD076A04: CommonGaugeLeviathan update, calculates UI percentage
/// - Action1014_SerpentsCry::HandleEventId at 0x7ff6dd22e770: Triggers Serpent's Cry
/// </summary>
internal static class LeviathanEikonOffsets
{
    // ================================================================
    // NOTE: Abyssal Tear uses AbyssalTearOffsets, not these offsets!
    // VentGauge here is for internal tracking, not the actual gauge value.
    // ================================================================
    
    /// <summary>
    /// Internal tracking value (float) - NOT the actual Abyssal Tear gauge!
    /// Use AbyssalTearOffsets.Gauge for the real Abyssal Tear gauge value.
    /// This appears to track decrements while in Leviathan mode.
    /// From IDA sub_7FF6DD537428: vsubss xmm0, xmm2, xmm3; vmovss [rdi+1C08h], xmm0
    /// </summary>
    public const int VentGauge = 0x1C08; // 7176 decimal
    
    // ================================================================
    // SERPENT'S CRY - Tidal system (timer + units)
    // ================================================================
    
    /// <summary>
    /// Unlimited Tidal Gauge seconds remaining (float) for Serpent's Cry.
    /// When > 0, player has unlimited tidal gauge.
    /// Set when Serpent's Cry activates via sub_7FF6DD537A04.
    /// Decrements each frame via sub_7FF6DD537428.
    /// </summary>
    public const int UnlimitedTidalSeconds = 0x1C0C; // 7180 decimal
    
    // Legacy alias for backwards compatibility
    public const int SecondaryGauge = UnlimitedTidalSeconds;
    
    /// <summary>
    /// Timer accumulator (float) used internally for Serpent's Cry calculations.
    /// When timer >= threshold, TidalUnitsUsed decrements.
    /// From IDA: vmovss dword ptr [rcx+1C10h], xmm3
    /// </summary>
    public const int TidalTimer = 0x1C10; // 7184 decimal
    
    /// <summary>
    /// Timer accumulator 2 (float) used for timing between tidal unit decrements.
    /// From IDA: vmovss dword ptr [rcx+1C14h], xmm0
    /// </summary>
    public const int TidalTimer2 = 0x1C14; // 7188 decimal
    
    /// <summary>
    /// Tidal units for Serpent's Cry (ushort/WORD).
    /// Value ranges from 0 to MaxTidalUnits (100 base, 150 with upgraded skill).
    /// UI shows percentage = 100 * TidalUnitsUsed / MaxTidalUnits
    /// Example: TidalUnitsUsed=47, Max=150 → displays as 103%
    /// From IDA sub_7FF6DD076A04: v15 = 100 * dword8_1 / n100
    /// Decremented by 1 per threshold in sub_7FF6DD537428: add [rdi+1C18h], ax where ax=0xFFFF (-1)
    /// </summary>
    public const int TidalUnitsUsed = 0x1C18; // 7192 decimal
    
    /// <summary>
    /// Max tidal units for current state (ushort/WORD).
    /// Used with GetPotencyParameter for skill 0x36.
    /// From IDA: *(_WORD *)(a1 + 7194)
    /// </summary>
    public const int MaxTidalUnits = 0x1C1A; // 7194 decimal
    
    // ================================================================
    // STATE BYTES - Control UI animations and effects
    // ================================================================
    
    /// <summary>
    /// State byte 1 - Abyssal Tear active flag.
    /// Set to 1 when Abyssal Tear starts (byte at 0x1C1C).
    /// Set to 0 when finished or in specific conditions.
    /// From IDA: *(_BYTE *)(_RDI + 7196) = 0/1
    /// </summary>
    public const int State1 = 0x1C1C; // 7196 decimal
    
    /// <summary>
    /// State byte 2 - Reload animation state machine.
    /// Values: 0 = idle, 1 = pending, 2 = active
    /// When == 2, triggers "Reload_timing_hit" timeline in UI.
    /// Transitions: 1 → 2 → 0 (see sub_7FF6DD537428)
    /// </summary>
    public const int State2 = 0x1C1D; // 7197 decimal
    
    /// <summary>
    /// State byte 3 - Boolean gate for State4 check.
    /// From IDA: *(_BYTE *)(_RCX + 7198)
    /// </summary>
    public const int State3 = 0x1C1E; // 7198 decimal
    
    /// <summary>
    /// State byte 4 - Special effect state machine.
    /// Values: 0 = idle, 1 = pending, 2 = active
    /// When == 2 (and State3 true), triggers special effect.
    /// Transitions: 1 → 2 → 0 (see sub_7FF6DD537428)
    /// </summary>
    public const int State4 = 0x1C1F; // 7199 decimal
}

// ============================================================
// BNPC ROW (NPC Entity)
// ============================================================

/// <summary>
/// Offsets within the BnpcRow (NPC base entity) structure.
/// </summary>
internal static class BnpcRowOffsets
{
    /// <summary>
    /// Pointer to the StaticActorInfo wrapper.
    /// Type: StaticActorInfo*
    /// </summary>
    public const int StaticActorInfoPtr = 0x20;
}

// ============================================================
// ACTOR (Internal game Actor object)
// ============================================================

/// <summary>
/// Offsets within the internal Actor object.
/// This is accessed via StaticActorInfo.ActorRef.
/// </summary>
internal static class ActorOffsets
{
    /// <summary>
    /// Reaction/State byte.
    /// Values:
    /// - 0x02 = Ground/Neutral
    /// - 0x03-0x05 = Ground reactions (Step Back/Slide)
    /// - > 0x05 = Airborne/Launch reaction (0x67, 0xC0, etc)
    /// Type: byte
    /// </summary>
    public const int ReactionState = 0x158;
}

// ============================================================
// STATIC ACTOR INFO
// ============================================================

/// <summary>
/// Represents the StaticActorInfo structure (known as 'Actor' in FaithFramework).
/// This is the wrapper found at bnpcRow + 0x20.
/// Updated to match Nenkai's FaithFramework structures.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x7400)] 
public unsafe struct StaticActorInfo
{
    [FieldOffset(0x00)] public long* VTable;
    
    [FieldOffset(0x10)] public uint ActorId;
    [FieldOffset(0x14)] public uint EntityId;

    [FieldOffset(0x20)] public long Node; // Node*
    
    [FieldOffset(0x28)] public long ActionActor; // ActionActor*
    
    [FieldOffset(0x30)] public long WorldContext;
    
    /// <summary>
    /// Reference to the internal game Actor object.
    /// Use ActorOffsets to access fields within.
    /// </summary>
    [FieldOffset(0x58)] public long ActorRef;

    /// <summary>
    /// Global Battle Behavior entry. 
    /// Contains the MagicFileResource (factory) at offset 0.
    /// </summary>
    [FieldOffset(0x7298)] public long BattleBehavior; // BattleBehavior*
}

/// <summary>
/// Mapping of the BattleBehavior structure based on FaithFramework
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x300)]
public unsafe struct BattleBehavior
{
    [FieldOffset(0x00)] public long* VTable;
    
    /// <summary>
    /// This is the 'MagicFileInstance' or Resource Container
    /// used for spawning projectiles and VFX.
    /// Found at offset 0x10 in FaithFramework.
    /// </summary>
    [FieldOffset(0x10)] public long MagicFileInstance;

    // Found at +0x200 in IDA analysis for Airborne checks
    [FieldOffset(0x200)] public long StateList;
}

/// <summary>
/// StateList structure for airborne state detection.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public unsafe struct StateList
{
    // 0x234 is the bit used for Airborne/InAir state in IDA logic
    [FieldOffset(0x234)] public uint IsAirborneBit;
}
