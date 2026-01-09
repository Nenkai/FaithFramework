using FF16Framework.Faith.Structs;
using FF16Framework.Interfaces.Nex.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

namespace FF16Framework.Faith.Hooks;

public unsafe class UnkList34Hooks : HookGroupBase
{
    public delegate nint UnkSingletonPlayer_GetList34Entry(UnkSingletonPlayer* @this);
    public UnkSingletonPlayer_GetList34Entry UnkSingletonPlayer_GetList34EntryFunction { get; private set; }

    public delegate UnkTargetStruct* UnkList34Entry_GetCurrentTargettedEnemy(nint @this, byte forceUnk);
    public UnkList34Entry_GetCurrentTargettedEnemy UnkList34Entry_GetCurrentTargettedEnemyFunction { get; private set; }

    public delegate UnkSingletonPlayer* UnkSingletonPlayer_UnkSingletonPlayer(UnkSingletonPlayer* @this);
    public IHook<UnkSingletonPlayer_UnkSingletonPlayer> UnkSingletonPlayer_UnkSingletonPlayerHook { get; private set; }

    private UnkSingletonPlayer* SingletonPtr;

    public UnkList34Hooks(Config config, IModConfig modConfig, ILogger logger)
    : base(config, modConfig, logger)
    {
    }

    public override void SetupHooks()
    {
        Project.Scans.AddScanHook(nameof(UnkSingletonPlayer_GetList34Entry), (result, hooks)
            => UnkSingletonPlayer_GetList34EntryFunction = hooks.CreateWrapper<UnkSingletonPlayer_GetList34Entry>(result, out _));
        Project.Scans.AddScanHook(nameof(UnkList34Entry_GetCurrentTargettedEnemy), (result, hooks)
            => UnkList34Entry_GetCurrentTargettedEnemyFunction = hooks.CreateWrapper<UnkList34Entry_GetCurrentTargettedEnemy>(result, out _));
        Project.Scans.AddScanHook(nameof(UnkSingletonPlayer_UnkSingletonPlayer), (result, hooks)
            => UnkSingletonPlayer_UnkSingletonPlayerHook = hooks.CreateHook<UnkSingletonPlayer_UnkSingletonPlayer>(UnkSingletonPlayer_UnkSingletonPlayerImpl, result).Activate());

    }

    private UnkSingletonPlayer* UnkSingletonPlayer_UnkSingletonPlayerImpl(UnkSingletonPlayer* @this)
    {
        SingletonPtr = @this;
        UnkSingletonPlayer_UnkSingletonPlayerHook.Disable();

        return UnkSingletonPlayer_UnkSingletonPlayerHook.OriginalFunction(@this);
    } 

    public UnkTargetStruct* GetTargettedEnemy()
    {
        if (SingletonPtr is null)
            return null;

        if (SingletonPtr->ControllingActorId == 0)
            return null;

        nint list34Entry = UnkSingletonPlayer_GetList34EntryFunction(SingletonPtr);
        if (list34Entry == nint.Zero)
            return null;

        return UnkList34Entry_GetCurrentTargettedEnemyFunction(list34Entry, 0);
    }
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct UnkSingletonPlayer
{
    [FieldOffset(0xC8)]
    public uint ControllingActorId;
}

public unsafe struct UnkTargetStruct
{
    public void* vtable;
    public nint field_8;
    public nint field_10;
    public nint field_18;
    public nint p_g_off_7FF6A3500598;
    public Node* Node;
    public Vector3 Position;
    public int dword1C;
    public Vector3 g_Vec3Empty;
    public int field_4C;
    public int Type;
    public int field_54;
    public int field_58;
    public int field_5C;
    public int field_60;
    public int field_64;
    public int field_68;
    public int ActorId;
    public float field_70;
    public int field_74;
    public int field_78;
};