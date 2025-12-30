using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Utils;

using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

namespace FF16Framework.Faith.Hooks;

public unsafe class CachedResourceManagerHooks : HookGroupBase
{
    public delegate nint UnkCachedResourceManagerCtor(CachedResourceManager* @this);
    public IHook<UnkCachedResourceManagerCtor> UnkCachedResourceManagerCtorHook { get; private set; }

    public CachedResourceManager* ManagerPtr { get; private set; }

    public CachedResourceManagerHooks(Config config, IModConfig modConfig, ILogger logger)
        : base(config, modConfig, logger)
    {
    }

    public override void SetupHooks()
    {
        Project.Scans.AddScanHook(nameof(UnkCachedResourceManagerCtor), (result, hooks)
            => UnkCachedResourceManagerCtorHook = hooks.CreateHook<UnkCachedResourceManagerCtor>(UnkCachedResourceManagerCtorImpl, result).Activate());
    }

    private nint UnkCachedResourceManagerCtorImpl(CachedResourceManager* @this)
    {
        ManagerPtr = @this;
        var res = UnkCachedResourceManagerCtorHook.OriginalFunction(@this);
        return res;
    }
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct CachedResourceManager
{
    [FieldOffset(0x2A0)]
    public StdMap<ResourceHandle>* Map;
}

public unsafe struct ResourceHandle
{
    // faith::ReferencedObject
    public nint vtable;
    public uint Field_0x08;
    public uint RefCount;
    public nint Field_0x10;

    // faith::Resource::ResourceHandle
    public byte* FileName;
    public nint Field_0x20;
    public nint Allocator;
    public nint FileReader;
    public nint FileBuffer;
    public uint FileSize;
    public int Field_0x44;
    public nint Field_0x48;
    public int OpenState;
    public int FormatLoadState;
    public int Field_0x58;
    public int Flags;
}
