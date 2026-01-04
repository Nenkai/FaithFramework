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

namespace FF16Framework.Faith.Hooks;

public unsafe class ResourceManagerHooks : HookGroupBase
{
    public delegate nint faith_Resource_ResourceManager_Ctor(ResourceManager* @this);
    public IHook<faith_Resource_ResourceManager_Ctor> CtorHook { get; private set; }

    public delegate nint faith_Resource_ResourceManager_GetOrCreateResource(ResourceManager* @this, ResourceHandleStruct** outResource, 
        uint pathHash, uint bucketIndex, byte* path, byte* fileExtension, byte a7, byte a8, ResourceFactory* outResourceFactory, nint factoryArg);
    public IHook<faith_Resource_ResourceManager_GetOrCreateResource> GetOrCreateResourceHook { get; private set; }

    public delegate void faith_Resource_ResourceManager_UnloadResource(ResourceManager* @this, ResourceHandleStruct* resource);
    public IHook<faith_Resource_ResourceManager_UnloadResource> UnloadResourceHook { get; private set; }

    public ResourceManager* ManagerPtr { get; private set; }

    // For Service
    public delegate void OnResourceLoadedDelegate(string path, ResourceHandleStruct* resourceHandle);
    public event OnResourceLoadedDelegate OnResourceLoaded;

    public delegate void OnResourceUnloadDelegate(ResourceHandleStruct* resourceHandle);
    public event OnResourceUnloadDelegate OnResourceUnload;

    public ResourceManagerHooks(Config config, IModConfig modConfig, ILogger logger)
        : base(config, modConfig, logger)
    {
    }

    public override void SetupHooks()
    {
        //Project.Scans.AddScanHook(nameof(faith_Resource_ResourceManager_Ctor), (result, hooks)
        //    => CtorHook = hooks.CreateHook<faith_Resource_ResourceManager_Ctor>(CtorImpl, result).Activate());
        Project.Scans.AddScanHook(nameof(faith_Resource_ResourceManager_GetOrCreateResource), (result, hooks)
            => GetOrCreateResourceHook = hooks.CreateHook<faith_Resource_ResourceManager_GetOrCreateResource>(GetOrCreateResourceImpl, result).Activate());
        Project.Scans.AddScanHook(nameof(faith_Resource_ResourceManager_UnloadResource), (result, hooks)
            => UnloadResourceHook = hooks.CreateHook<faith_Resource_ResourceManager_UnloadResource>(UnloadResourceImpl, result).Activate());
    }

    private nint CtorImpl(ResourceManager* @this)
    {
        ManagerPtr = @this;
        var res = CtorHook.OriginalFunction(@this);
        CtorHook.Disable();

        return res;
    }

    private nint GetOrCreateResourceImpl(ResourceManager* @this, ResourceHandleStruct** outResource,
        uint pathHash, uint bucketIndex, byte* path, byte* fileExtension, byte a7, byte a8, ResourceFactory* outResourceFactory, nint factoryArg)
    {
        // 0 = created, 1 = already loaded
        var res = GetOrCreateResourceHook.OriginalFunction(@this, outResource, pathHash, bucketIndex, path, fileExtension, a7, a8, outResourceFactory, factoryArg);
        if (res == 0)
        {
            // Resource was just loaded, track it
            string? fileName = Marshal.PtrToStringAnsi((nint)path)!;
            OnResourceLoaded?.Invoke(fileName, *outResource);
        }

        return res;
    }

    private void UnloadResourceImpl(ResourceManager* @this, ResourceHandleStruct* resource)
    {
        // Untrack resource
        OnResourceUnload?.Invoke(resource);

        // Let the game free it.
        UnloadResourceHook.OriginalFunction(@this, resource);
    }
}

public unsafe struct ResourceManager
{
    public nint vtable;
    public nint qword8;
    public ResourceFactoryLinkedList ResourceFactoryList; // All factories for each extension
    public nint qword20;
    public nint qword28;
    public nint qword30;
    public ResourceBucketManager BucketList1;
    public nint field_4040;
    public ResourceBucketManagerArray BucketLists2;
    public int field_10060;
    public int field_10064;
    public ResourceBucketManager BucketList3;
}

public unsafe struct ResourceBucketManager
{
    public const int BUCKET_SIZE = 512;

    public fixed ulong SRWLockShareds[BUCKET_SIZE]; // RTL_SRWLOCK
    public ResourceBucketArray Buckets;
    public nint Unk;
}

public unsafe struct ResourceFactoryLinkedList
{
    public ResourceFactoryHandlerPair** Entries;
    public nint Count;
};

public unsafe struct ResourceFactoryHandlerPair
{
    public ResourceFactoryHandlerPair* Next;
    public ResourceFactoryHandlerPair* Previous;
    public ResourceFactory Factory;
    public nint Extension;
};

public struct ResourceFactory
{
    public nint CreateWrapCb;
    public nint CreateCb;
    public nint field_10;
    public nint field_18;
};

[InlineArray(3)]
public struct ResourceBucketManagerArray
{
    public ResourceBucketManager Element;
}

[InlineArray(ResourceBucketManager.BUCKET_SIZE)]
public struct ResourceBucketArray
{
    public ResourceBucket Element;
}

public struct ResourceBucket
{
    public nint field_0x00;
    public nint field_0x08;
    public nint field_0x10;
}

public unsafe struct ResourceHandleStruct
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
    public uint PathHash;
    public uint Field_0x4C;
    public int OpenState;
    public int FormatLoadState;
    public int Field_0x58;
    public int Flags;

    public readonly uint GetBucket() => PathHash % ResourceBucketManager.BUCKET_SIZE; // & 0x1FF
}

/*
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
*/
