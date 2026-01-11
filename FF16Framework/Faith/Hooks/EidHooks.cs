using FF16Framework.Faith.Structs;
using FF16Framework.Interfaces.Nex.Structures;

using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

using SixLabors.ImageSharp.PixelFormats;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Faith.Hooks;

public unsafe class EidHooks : HookGroupBase
{
    public delegate byte ActorEidDataEntry_GetPosition(ActorEidDataEntry* @this, uint eidId, NodePositionPair* outNodePos);
    public ActorEidDataEntry_GetPosition GetPosition { get; private set; }

    public nint GameMapId { get; private set; }

    public EidHooks(Config config, IModConfig modConfig, ILogger logger)
    : base(config, modConfig, logger)
    {
    }

    public override void SetupHooks()
    {
        Project.Scans.AddScanHook(nameof(ActorEidDataEntry_GetPosition), (result, hooks)
            => GetPosition = hooks.CreateWrapper<ActorEidDataEntry_GetPosition>(result, out _));
    }
}