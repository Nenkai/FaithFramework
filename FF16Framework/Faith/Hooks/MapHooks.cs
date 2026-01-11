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

public unsafe class MapHooks : HookGroupBase
{
    public delegate nint Map_GetCurrentGameMapId(nint @this, nint outId);
    public IHook<Map_GetCurrentGameMapId> Map_GetCurrentGameMapIdHook { get; private set; }

    public delegate nint NodePositionPair_ComputeWorldPosition(NodePositionPair* @this, Vector3* outVec);
    public NodePositionPair_ComputeWorldPosition NodePositionPair_ComputeWorldPositionFunction { get; private set; }

    public nint GameMapId { get; private set; }

    public MapHooks(Config config, IModConfig modConfig, ILogger logger)
    : base(config, modConfig, logger)
    {
    }

    public override void SetupHooks()
    {
        //Project.Scans.AddScanHook(nameof(Map_GetCurrentGameMapId), (result, hooks)
        //    => Map_GetCurrentGameMapIdHook = hooks.CreateHook<Map_GetCurrentGameMapId>(Map_GetCurrentGameMapIdImpl, result).Activate());
        Project.Scans.AddScanHook(nameof(NodePositionPair_ComputeWorldPosition), (result, hooks)
            => NodePositionPair_ComputeWorldPositionFunction = hooks.CreateWrapper<NodePositionPair_ComputeWorldPosition>(result, out _));
    }

    public Vector3 ComputeWorldPosition(NodePositionPair* @this)
    {
        Vector3 vec;
        NodePositionPair_ComputeWorldPositionFunction(@this, &vec);
        return vec;
    }

    // Why does this cause a crash when loading into a map?
    private nint Map_GetCurrentGameMapIdImpl(nint @this, nint outId)
    {
        var res = Map_GetCurrentGameMapIdHook.OriginalFunction(@this, outId);
        if (res != nint.Zero)
            GameMapId = *(nint*)res;
        return res;
    }
}