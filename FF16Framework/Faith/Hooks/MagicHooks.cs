using System;
using System.Collections;
using System.Collections.Concurrent.Extended;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

using FF16Framework.Faith.Structs;
using FF16Framework.Services.ResourceManager;
using NenTools.ImGui.Interfaces.Shell;

namespace FF16Framework.Faith.Hooks;

public unsafe class MagicHooks : HookGroupBase
{
    private readonly FrameworkConfig _frameworkConfig;
    private readonly IImGuiShell _shell;
    private readonly EntityManagerHooks _entityManager;

    public delegate nint BattleMagicExecutor_InsertMagic(nint @this, BattleMagic* magic);
    public IHook<BattleMagicExecutor_InsertMagic> BattleMagicExecutor_InsertMagicHook { get; private set; }

    public MagicHooks(Config config, IModConfig modConfig, ILogger logger, 
        FrameworkConfig frameworkConfig, IImGuiShell imGuiShell, EntityManagerHooks entityManager)
        : base(config, modConfig, logger)
    {
        _frameworkConfig = frameworkConfig;
        _shell = imGuiShell;
        _entityManager = entityManager;
    }

    public override void SetupHooks()
    {
        Project.Scans.AddScanHook(nameof(BattleMagicExecutor_InsertMagic), (result, hooks)
            => BattleMagicExecutor_InsertMagicHook = hooks.CreateHook<BattleMagicExecutor_InsertMagic>(BattleMagicExecutor_InsertMagicImpl, result).Activate());

    }

    private nint BattleMagicExecutor_InsertMagicImpl(nint @this, BattleMagic* magic)
    {
        var res = BattleMagicExecutor_InsertMagicHook.OriginalFunction(@this, magic);
        if (_frameworkConfig.MagicSystem.PrintMagicCasts)
        {
            nint* staticActorInfo = null;
            if (magic->MagicActorId != 0)
                return res;

            ActorReference* actorRef = _entityManager.ActorManager_GetActorByKeyFunction(_entityManager.ActorManager, magic->CasterActorId);
            _shell.LogWriteLine(nameof(MagicHooks), $"Magic: {magic->MagicId} (from actor: {magic->CasterActorId:X} (entity {actorRef->EntityID:X})) @ {magic->Position:F2})", color: Color.LightBlue);
        }

        return res;
    }
}

public unsafe struct BattleMagic
{
    public nint vtable;
    public Node* ParentNode;
    public Vector3 Position;
    public int dword1C;
    public BattleMagicSub0x20 qword20;
    public nint gap78;
    public int field_80;
    public int field_84;
    public int field_88;
    public int field_8C;
    public int field_90;
    public int field_94;
    public int field_98;
    public int field_9C;
    public nint Field_a0;
    public nint field_A8;
    public nint field_B0;
    public nint field_B8;
    public short field_C0;
    public byte field_C2;
    public byte gapC3;
    public int field_C4;
    public ResourceHandleStruct* MagicFileResource;
    public nint qwordD0;
    public Vector4 Vec4_;
    public uint CasterActorId;
    public uint MagicActorId; // = Entity Id 6, See ActorBase->Magic
    public int Field_f0;
    public int CommandId;
    public int MagicId;
    public int SubId;
    public byte field_100;
    public byte byte101;
    public byte field_102;
    public byte field_103;
    int field_104;

    public struct BattleMagicSub0x20
    {
        public nint qword20;
        public ArrayBuffer0x20 ArrayBuffer_;
        public int dword50;
        public int dword54;
        public int dword58;
        public int dword5C;
        public int dword60;
        public int dword64;
        public int dword68;
        public float dword6C;
        public int dword70;
        public int dword74;
    };

    public unsafe struct ArrayBuffer0x20
    {
        nint Size;
        fixed byte Dst[32];
    };

};
