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

public unsafe class UnkList35Hooks : HookGroupBase
{
    public delegate nint UnkSingletonPlayer_GetList35Entry(UnkSingletonPlayer* @this);
    public UnkSingletonPlayer_GetList35Entry UnkSingletonPlayer_GetList35EntryFunction { get; private set; }

    public delegate UnkTargetStruct* UnkList35Entry_GetCurrentTargettedEnemy(nint @this, byte forceUnk);
    public UnkList35Entry_GetCurrentTargettedEnemy UnkList35Entry_GetCurrentTargettedEnemyFunction { get; private set; }
    public delegate nint StaticActorInfo_GetActorData35Entry(nint staticActorInfo);
    public StaticActorInfo_GetActorData35Entry GetActorData35EntryFunction { get; private set; }
    public delegate UnkSingletonPlayer* UnkSingletonPlayer_UnkSingletonPlayer(UnkSingletonPlayer* @this);
    public IHook<UnkSingletonPlayer_UnkSingletonPlayer> UnkSingletonPlayer_UnkSingletonPlayerHook { get; private set; }

    private UnkSingletonPlayer* SingletonPtr;

    public UnkList35Hooks(Config config, IModConfig modConfig, ILogger logger)
    : base(config, modConfig, logger)
    {
    }

    public override void SetupHooks()
    {
        Project.Scans.AddScanHook(nameof(UnkSingletonPlayer_GetList35Entry), (result, hooks)
            => UnkSingletonPlayer_GetList35EntryFunction = hooks.CreateWrapper<UnkSingletonPlayer_GetList35Entry>(result, out _));
        Project.Scans.AddScanHook(nameof(UnkList35Entry_GetCurrentTargettedEnemy), (result, hooks)
            => UnkList35Entry_GetCurrentTargettedEnemyFunction = hooks.CreateWrapper<UnkList35Entry_GetCurrentTargettedEnemy>(result, out _));
        Project.Scans.AddScanHook(nameof(UnkSingletonPlayer_UnkSingletonPlayer), (result, hooks)
            => UnkSingletonPlayer_UnkSingletonPlayerHook = hooks.CreateHook<UnkSingletonPlayer_UnkSingletonPlayer>(UnkSingletonPlayer_UnkSingletonPlayerImpl, result).Activate());
        Project.Scans.AddScanHook(nameof(StaticActorInfo_GetActorData35Entry), (result, hooks)
            => GetActorData35EntryFunction = hooks.CreateWrapper<StaticActorInfo_GetActorData35Entry>(result, out _));

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

        nint list35Entry = UnkSingletonPlayer_GetList35EntryFunction(SingletonPtr);
        if (list35Entry == nint.Zero)
            return null;

        return UnkList35Entry_GetCurrentTargettedEnemyFunction(list35Entry, 0);
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