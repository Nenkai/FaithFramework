using System;
using System.Collections.Concurrent.Extended;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Faith.Structs;
using FF16Framework.Faith.Hooks;

namespace FF16Framework.Services.ResourceManager;

public class ResourceManagerService
{
    private readonly ResourceManagerHooks _resourceManagerHooks;

    public ConcurrentSortedDictionary<string, ConcurrentSortedDictionary<string, ResourceHandle>> SortedHandles { get; private set; } = [];
    public ConcurrentSortedDictionary<uint, ResourceHandle> LoadedHandles { get; private set; } = [];

    public unsafe ResourceManagerService(ResourceManagerHooks resourceManagerhooks)
    {
        _resourceManagerHooks = resourceManagerhooks;

        _resourceManagerHooks.OnResourceLoaded += OnResourceLoaded;
        _resourceManagerHooks.OnResourceUnload += OnResourceUnload;
    }

    public unsafe void OnResourceLoaded(string path, ResourceHandleStruct* resourcePointer)
    {
        string extension = Path.GetExtension(path).ToLowerInvariant();
        
        if (!LoadedHandles.ContainsKey(resourcePointer->PathHash))
        {
            var resource = new ResourceHandle(resourcePointer) { IsValid = true };

            LoadedHandles.TryAdd(resourcePointer->PathHash, resource);

            SortedHandles.TryAdd(extension, []);
            SortedHandles[extension].TryAdd(path, resource);
        }
    }

    public unsafe void OnResourceUnload(ResourceHandleStruct* resourcePtr)
    {
        if (LoadedHandles.TryGetValue(resourcePtr->PathHash, out ResourceHandle resource))
        {
            LoadedHandles.TryRemove(resourcePtr->PathHash);
            resource.IsValid = false;
            resource.Restore();

            string? fileName = Marshal.PtrToStringAnsi((nint)resourcePtr->FileName)!;
            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (SortedHandles.TryGetValue(extension, out ConcurrentSortedDictionary<string, ResourceHandle> group))
            {
                group.TryRemove(fileName);

                if (group.Count == 0)
                    SortedHandles.TryRemove(extension);
            }
        }
    }
}
