using System.Numerics;

namespace FF16Framework.Faith.Structs;

// For FFXVI
public unsafe struct UIAddonOrControllerManager
{
    public nint _vftable;
    public UiControllerThingList List; // Game addresses this list by indexing the list directly even though the list entries are added dynamically.
    public nint field_28;
    public nint field_30;
    public nint field_38;
    public nint field_40;
    public nint field_48;
    public nint oword50;
    public nint field_58;
    public fixed byte gap60[32];
    public int dword80;
    public fixed byte gap84[44];
    public int dwordB0;
    public int field_B4;
    public Matrix4x4 ViewProjMatrix;
    public Size ViewportSize;
};

public unsafe struct UiControllerThingList // Actually a std::vector, but the game literally accesses this by index
{
    public nint Allocator;
    public UiControllerThingListEntries* Begin;
    public nint end;
    public nint capacity;
};

public struct UiControllerThingListEntries
{
    public nint field_0;
    public nint field_8;
    public nint field_10;
    public nint field_18;
    public nint field_20;
    public nint field_28;
    public nint field_30;
    public nint field_38;
    public nint field_40;
    public nint field_48;
    public nint field_50;
    public nint field_58;
    public nint field_60;
    public nint field_68;
    public nint field_70;
    public nint field_78;
    public nint field_80;
    public nint field_88;
    public nint BattleUI;
    public nint field_98;
    public nint field_A0;
    public nint field_A8;
    public nint field_B0;
    public nint field_B8;
    public nint field_C0;
    public nint field_C8;
    public nint field_D0;
    public nint field_D8;
    public nint field_E0;
    public nint field_E8;
    public nint field_F0;
    public nint field_F8;
    public nint field_100;
    public nint field_108;
    public nint field_110;
    public nint field_118;
    public nint field_120;
    public nint field_128;
    public nint field_130;
    public nint field_138;
    public nint field_140;
    public nint field_148;
    public nint field_150;
    public nint field_158;
    public nint field_160;
    public nint field_168;
    public nint field_170;
    public nint field_178;
    public nint field_180;
    public nint field_188;
    public nint field_190;
    public nint field_198;
    public nint field_1A0;
    public nint field_1A8;
    public nint field_1B0;
    public nint field_1B8;
    public nint field_1C0;
    public nint field_1C8;
    public nint field_1D0;
};


