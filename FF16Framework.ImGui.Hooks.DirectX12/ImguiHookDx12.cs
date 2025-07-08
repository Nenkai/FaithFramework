using FF16Framework.ImGui.Hooks;
using FF16Framework.ImGui.Hooks.DirectX;
using FF16Framework.ImGui.Hooks.DirectX12.Definitions;
using FF16Framework.Interfaces.ImGui;
using FF16Framework.Native.ImGui;

using Reloaded.Hooks.Definitions;

using SharpDX.Direct3D12;
using SharpDX.DXGI;

using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Windows.Win32;

using static System.Net.Mime.MediaTypeNames;

using Device = SharpDX.Direct3D12.Device;

namespace FF16Framework.ImGui.Hooks.DirectX12;

public unsafe class ImguiHookDx12 : IImguiHook
{
    public static ImguiHookDx12 Instance { get; private set; }

    private IHook<DX12Hook.Present> _presentHook;
    private IHook<DX12Hook.ResizeBuffers> _resizeBuffersHook;
    private IHook<DX12Hook.CreateSwapChainForHwnd> _createSwapChainForHwndHook;

    private bool _initialized = false;
    private Device _device;
    private DescriptorHeap _shaderResourceViewDescHeap;
    private DescriptorHeap renderTargetViewDescHeap;
    private List<FrameContext> _frameContexts = [];
    private GraphicsCommandList _commandList;
    private CommandQueue _commandQueue;

    private static DescriptorHeapAllocator _textureHeapAllocator;
    private ConcurrentDictionary<ulong, TextureResource> _textureIds = [];

    private static readonly string[] _supportedDlls =
    [
        "d3d12.dll",
    ];

    /*
     * In some cases (E.g. under DX9 + Viewports enabled), Dear ImGui might call
     * DirectX functions from within its internal logic.
     *
     * We put a lock on the current thread in order to prevent stack overflow.
     */
    private bool _presentRecursionLock = false;
    private bool _resizeRecursionLock = false;
    private bool _createSwapChainRecursionLock = false;

    public ImguiHookDx12() { }

    public bool IsApiSupported()
    {
        foreach (var dll in _supportedDlls)
        {
            var handle = PInvoke.GetModuleHandle(dll);
            if (!handle.IsInvalid)
                return true;
        }

        // Fallback to detecting D3D12Core
        if (File.Exists(Path.Combine("D3D12", "D3D12Core.dll")))
            return true;

        return false;
    }

    public void Initialize()
    {
        var presentPtr = (long)DX12Hook.SwapchainVTable[(int)IDXGISwapChainVTable.Present].FunctionPointer;
        var resizeBuffersPtr = (long)DX12Hook.SwapchainVTable[(int)IDXGISwapChainVTable.ResizeBuffers].FunctionPointer;
        var createSwapChainForHwndPtr = (long)DX12Hook.FactoryVTable[(int)IDXGIFactory.CreateSwapChainForHwnd].FunctionPointer;
        //var executeCommandListsPtr = (long)DX12Hook.ComamndQueueVTable[(int)ID3D12CommandQueueVTable.ExecuteCommandLists].FunctionPointer;
        Instance = this;
        _presentHook = SDK.Hooks.CreateHook<DX12Hook.Present>(typeof(ImguiHookDx12), nameof(PresentImplStatic), presentPtr).Activate();
        _resizeBuffersHook = SDK.Hooks.CreateHook<DX12Hook.ResizeBuffers>(typeof(ImguiHookDx12), nameof(ResizeBuffersImplStatic), resizeBuffersPtr).Activate();
        _createSwapChainForHwndHook = SDK.Hooks.CreateHook<DX12Hook.CreateSwapChainForHwnd>(typeof(ImguiHookDx12), nameof(CreateSwapChainForHwndImplStatic), createSwapChainForHwndPtr).Activate();
        //_execCmdListHook = SDK.Hooks.CreateHook<DX12Hook.ExecuteCommandLists>(typeof(ImguiHookDx12), nameof(ExecCmdListsImplStatic), executeCommandListsPtr).Activate();
    }

    ~ImguiHookDx12()
    {
        ReleaseUnmanagedResources();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    private void ReleaseUnmanagedResources()
    {
        if (_initialized)
        {
            Debug.WriteLine($"[DX12 Dispose] Shutdown");
            ImGuiMethods.cImGui_ImplDX12_Shutdown();

            foreach (var resource in _textureIds.Values)
                resource.Dispose();
        }
    }

    private nint CreateSwapChainForHwndImpl(nint this_, nint pDevice, nint hWnd, nint pDesc, nint pFullscreenDesc, nint pRestrictToOutput, nint ppSwapChain)
    {
        /* We hook as it seems we have to; when swapping from borderless -> fullscreen -> borderless, otherwise, this happens:
         * [22012] DXGI ERROR: IDXGIFactory::CreateSwapChain: Only one flip model swap chain can be associate with an HWND, 
         * IWindow, or composition surface at a time. ClearState() and Flush() may need to be called on the D3D11 device context 
         * to trigger deferred destruction of old swapchains. [ MISCELLANEOUS ERROR #297: ]
         *
         * Fun fact: FF16's response to that DXGI/D3D12 error is to simply bring up a messagebox and translate the error code from GetDeviceRemovedReason
         * ...When that happens, the error code is 0, so all you get is a "The operation completed successfully". Very useful
         * 
         * The recursion lock exists as ImGui can also call CreateSwapChainForHwnd within ImGui_ImplDX12_CreateWindow
         */

        if (!_createSwapChainRecursionLock && renderTargetViewDescHeap is not null)
            PreResizeBuffers();

        var res = _createSwapChainForHwndHook.OriginalFunction.Value.Invoke(this_, pDevice, hWnd, pDesc, pFullscreenDesc, pRestrictToOutput, ppSwapChain);

        // I have no idea if this is correct. Resizing buffers of any swapchain sounds wrong, but resizing our own swapchain doesn't work
        // I don't get it. I'm no renderer engineer, I have no idea.
        if (!_createSwapChainRecursionLock && renderTargetViewDescHeap is not null)
            PostResizeBuffers(*(nint*)ppSwapChain);

        return res;
    }

    private nint ResizeBuffersImpl(nint swapchainPtr, uint bufferCount, uint width, uint height, Format newFormat, SwapChainFlags swapchainFlags)
    {
        if (_resizeRecursionLock)
        {
            Debug.WriteLine($"[DX12 ResizeBuffers] Discarding via Recursion Lock");
            return _resizeBuffersHook.OriginalFunction.Value.Invoke(swapchainPtr, bufferCount, width, height, newFormat, swapchainFlags);
        }

        if (!_initialized || renderTargetViewDescHeap is null) // Our device was probably not yet created, fine to just reroute to original
            return _resizeBuffersHook.OriginalFunction.Value.Invoke(swapchainPtr, bufferCount, width, height, newFormat, swapchainFlags);

        _resizeRecursionLock = true;
        try
        {
            // Dispose all frame context resources
            PreResizeBuffers();
            var result = _resizeBuffersHook.OriginalFunction.Value.Invoke(swapchainPtr, bufferCount, width, height, newFormat, swapchainFlags);
            if (result != nint.Zero)
            {
                Debug.DebugWriteLine($"[DX12 ResizeBuffers] ResizeBuffers original failed with {result:X}");
                return result;
            }

            PostResizeBuffers(swapchainPtr);
            return result;
        }
        finally
        {
            _resizeRecursionLock = false;
        }
    }

    private void PreResizeBuffers()
    {
        // ResizeBuffer requires swapchain resources to be freed.
        foreach (var frameCtx in _frameContexts)
            frameCtx.MainRenderTargetResource?.Dispose();

        ImGuiMethods.cImGui_ImplDX12_InvalidateDeviceObjects();
    }

    private Device PostResizeBuffers(nint swapchainPtr)
    {
        var swapChain = new SwapChain(swapchainPtr);
        using var device = swapChain.GetDevice<Device>();

        var windowHandle = swapChain.Description.OutputHandle;
        Debug.DebugWriteLine($"[DX12 ResizeBuffers] Window Handle {windowHandle:X}");

        var rtvDescriptorSize = device.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
        var rtvHandle = renderTargetViewDescHeap.CPUDescriptorHandleForHeapStart;

        for (var i = 0; i < _frameContexts.Count; i++)
        {
            _frameContexts[i].main_render_target_descriptor = rtvHandle;
            var resource = swapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i);
            device.CreateRenderTargetView(resource, null, rtvHandle);
            _frameContexts[i].MainRenderTargetResource = resource;
            rtvHandle.Ptr += rtvDescriptorSize;
        }

        ImGuiMethods.cImGui_ImplDX12_CreateDeviceObjects();
        return device;
    }

    private unsafe nint PresentImpl(nint swapChainPtr, int syncInterval, PresentFlags flags)
    {
        if (_presentRecursionLock)
        {
            Debug.WriteLine($"[DX12 Present] Discarding via Recursion Lock");
            return _presentHook.OriginalFunction.Value.Invoke(swapChainPtr, syncInterval, flags);
        }

        _presentRecursionLock = true;
        try
        {
            var swapChain = new SwapChain3(swapChainPtr);
            var windowHandle = swapChain.Description.OutputHandle;

            // Ignore windows which don't belong to us.
            if (!ImguiHook.CheckWindowHandle(windowHandle))
            {
                Debug.WriteLine($"[DX12 Present] Discarding Window Handle {windowHandle:X} due to Mismatch");
                return _presentHook.OriginalFunction.Value.Invoke(swapChainPtr, syncInterval, flags);
            }

            _device = swapChain.GetDevice<Device>();
            if (_device is null)
                return _presentHook.OriginalFunction.Value.Invoke(swapChainPtr, syncInterval, flags);

            _commandQueue = new CommandQueue(*(nint*)(swapChain.NativePointer + DX12Hook.CommandQueueOffset!.Value));
            var frameBufferCount = swapChain.Description.BufferCount;
            if (!_initialized)
            {
                var descriptorImGuiRender = new DescriptorHeapDescription
                {
                    Type = DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView,
                    DescriptorCount = frameBufferCount,
                    Flags = DescriptorHeapFlags.ShaderVisible
                };
                _shaderResourceViewDescHeap = _device.CreateDescriptorHeap(descriptorImGuiRender);
                if (_shaderResourceViewDescHeap == null)
                {
                    Debug.WriteLine($"[DX12 Present] Failed to create shader resource view descriptor heap.");
                    return _presentHook.OriginalFunction.Value.Invoke(swapChainPtr, syncInterval, flags);
                }

                Debug.WriteLine($"[DX12 Present] Init DX12, Window Handle: {windowHandle:X}");

                var renderTargetDesc = new DescriptorHeapDescription
                {
                    Type = DescriptorHeapType.RenderTargetView,
                    DescriptorCount = frameBufferCount,
                    Flags = DescriptorHeapFlags.None,
                    NodeMask = 1
                };
                renderTargetViewDescHeap = _device.CreateDescriptorHeap(renderTargetDesc);
                if (renderTargetViewDescHeap == null)
                    return _presentHook.OriginalFunction.Value.Invoke(swapChainPtr, syncInterval, flags);
                
                // Create our texture heap allocator/pool
                _textureHeapAllocator = new DescriptorHeapAllocator();
                _textureHeapAllocator.Create(_device, _device.CreateDescriptorHeap(new DescriptorHeapDescription()
                {
                    DescriptorCount = 10000,
                    Type = DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView,
                    Flags = DescriptorHeapFlags.ShaderVisible,
                }));

                var rtvDescriptorSize = _device.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
                var rtvHandle = renderTargetViewDescHeap.CPUDescriptorHandleForHeapStart;

                for (var i = 0; i < frameBufferCount; i++)
                {
                    _frameContexts.Add(new FrameContext
                    {
                        main_render_target_descriptor = rtvHandle,
                        MainRenderTargetResource = swapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i),
                    });
                    _device.CreateRenderTargetView(_frameContexts[i].MainRenderTargetResource, null, rtvHandle);
                    rtvHandle.Ptr += rtvDescriptorSize;
                }
                
                // Create command list
                for (var i = 0; i < frameBufferCount; i++)
                {
                    _frameContexts[i].CommandAllocator = _device.CreateCommandAllocator(CommandListType.Direct);
                    if (_frameContexts[i].CommandAllocator == null)
                        return _presentHook.OriginalFunction.Value.Invoke(swapChainPtr, syncInterval, flags);
                }

                _commandList = _device.CreateCommandList(0, CommandListType.Direct, _frameContexts[0].CommandAllocator, null);
                if (_commandList == null)
                    return _presentHook.OriginalFunction.Value.Invoke(swapChainPtr, syncInterval, flags);
                _commandList.Close();

                ImguiHook.InitializeWithHandle(windowHandle);
                var initInfo = new ImGui_ImplDX12_InitInfo_t
                {
                    Device = _device.NativePointer,
                    CommandQueue = _commandQueue.NativePointer,
                    NumFramesInFlight = frameBufferCount,
                    RTVFormat = 28, // DXGI_FORMAT_R8G8B8A8_UNORM
                    SrvDescriptorHeap = _shaderResourceViewDescHeap.NativePointer,
                    SrvDescriptorAllocFn = &SrvDescriptorAllocCallback,
                    SrvDescriptorFreeFn = &SrvDescriptorFreeCallback,
                };

                ImGuiMethods.cImGui_ImplDX12_Init((nint)(&initInfo));
                _initialized = true;
            }

            ImGuiMethods.cImGui_ImplDX12_NewFrame();

            // ImGui >=1.92 note
            // When resizing windows outside the game window (viewports), the game wants to
            // Resize windows for some reason using PlatformIO_SetWindowSize
            // ImGui's DX12 implementation for that will call ResizeBuffers which we hook

            // In turn, InvalidateDeviceObjects will be called, and all textures will be destroyed as of 1.92

            // On the next frame for some reason the textures aren't recreated (font mainly?)
            // We may need to also set ImGui->PlatformIO->SetWindowSize to null maybe? Not sure.

            // this may  call CreateSwapChainForHwnd when creating windows, which we hook. we use a lock to make sure we aren't freeing objects again.
            _createSwapChainRecursionLock = true;
            ImguiHook.NewFrame();
            _createSwapChainRecursionLock = false;

            var FrameBufferCountsfgn = swapChain.Description.BufferCount;
            var currentFrameContext = _frameContexts[swapChain.CurrentBackBufferIndex];
            currentFrameContext.CommandAllocator.Reset();

            var barrier = new ResourceBarrier
            {
                Type = ResourceBarrierType.Transition,
                Flags = ResourceBarrierFlags.None,
                Transition = new ResourceTransitionBarrier(currentFrameContext.MainRenderTargetResource, -1, ResourceStates.Present, ResourceStates.RenderTarget)
            };
            _commandList.Reset(currentFrameContext.CommandAllocator, null);
            _commandList.ResourceBarrier(barrier);
            _commandList.SetRenderTargets(currentFrameContext.main_render_target_descriptor, null);
            _commandList.SetDescriptorHeaps(_shaderResourceViewDescHeap);

            ImGuiMethods.cImGui_ImplDX12_RenderDrawData((nint)ImGuiMethods.GetDrawData(), _commandList.NativePointer);

            barrier.Transition = new ResourceTransitionBarrier(currentFrameContext.MainRenderTargetResource, -1, ResourceStates.RenderTarget, ResourceStates.Present);
            _commandList.ResourceBarrier(barrier);
            _commandList.Close();

            // Normally we would hook ExecuteCommandList to grab the command queue, but it causes the following issue in certain games (S.T.A.L.K.E.R Enhanced Edition):
            // "D3D12 ERROR: ID3D12CommandQueue::ExecuteCommandLists:
            // A command list, which writes to a swap chain back buffer, may only be executed on the command queue associated with that buffer.
            // [ STATE_SETTING ERROR #907: EXECUTECOMMANDLISTS_WRONGSWAPCHAINBUFFERREFERENCE]"

            // Grabbing the queue pointer from the swapchain (offset is scanned in DX12Hook.cs) seems to be OK.
            // https://stackoverflow.com/questions/36286425/how-do-i-get-the-directx-12-command-queue-from-a-swap-chain
            if (_commandQueue.NativePointer != 0)
                _commandQueue.ExecuteCommandList(_commandList);

            return _presentHook.OriginalFunction.Value.Invoke(swapChainPtr, syncInterval, flags);
        }
        finally
        {
            _presentRecursionLock = false;
        }
    }

    /// <summary>
    /// Loads an image and returns the ImTextureID for it.
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="imageWidth"></param>
    /// <param name="imageHeight"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Windows-specific code")]
    public ulong LoadTexture(Span<byte> bytes, uint imageWidth, uint imageHeight)
    {
        var heapProperties = new HeapProperties(HeapType.Default);
        var imageDesc = new ResourceDescription(ResourceDimension.Texture2D, 0, imageWidth, (int)imageHeight, 1, 1, Format.R8G8B8A8_UNorm, 1, 0, TextureLayout.Unknown, ResourceFlags.None);
        SharpDX.Direct3D12.Resource pTexture = _device.CreateCommittedResource(heapProperties, HeapFlags.None, imageDesc, ResourceStates.CopyDestination);

        const int D3D12_TEXTURE_DATA_PITCH_ALIGNMENT = 256;
        var uploadBufferHeapDesc = new HeapProperties(HeapType.Upload);
        uint uploadPitch = (uint)(imageWidth * 4 + D3D12_TEXTURE_DATA_PITCH_ALIGNMENT - 1u & ~(D3D12_TEXTURE_DATA_PITCH_ALIGNMENT - 1u));
        uint uploadSize = imageHeight * uploadPitch;

        var tempDesc = new ResourceDescription(ResourceDimension.Buffer, 0, uploadSize, 1, 1, 1, Format.Unknown, 1, 0, TextureLayout.RowMajor, ResourceFlags.None);
        SharpDX.Direct3D12.Resource uploadBuffer = _device.CreateCommittedResource(uploadBufferHeapDesc, HeapFlags.None, tempDesc, ResourceStates.GenericRead);

        var range = new SharpDX.Direct3D12.Range()
        {
            Begin = 0,
            End = uploadSize,
        };
        nint mapped = uploadBuffer.Map(0, range);
        fixed (byte* imageData = bytes)
        {
            for (int y = 0; y < imageHeight; y++)
                Buffer.MemoryCopy(imageData + y * imageWidth * 4, // Src
                    (void*)(nuint)(mapped + y * uploadPitch), // Dst
                    imageWidth * 4, imageWidth * 4); // Len
        }
        uploadBuffer.Unmap(0, range);

        var srcLocation = new TextureCopyLocation(uploadBuffer, new PlacedSubResourceFootprint()
        {
            Footprint = new SubResourceFootprint()
            {
                Format = Format.R8G8B8A8_UNorm,
                Width = (int)imageWidth,
                Height = (int)imageHeight,
                Depth = 1,
                RowPitch = (int)uploadPitch,
            },
        });
        var dstLocation = new TextureCopyLocation(pTexture, 0);

        Fence fence = _device.CreateFence(0, FenceFlags.None);
        var @event = PInvoke.CreateEvent(null, false, false, (string)null);
        var queue = _device.CreateCommandQueue(CommandListType.Direct, 1);
        var cmdAllocator = _device.CreateCommandAllocator(CommandListType.Direct);
        var cmdList = _device.CreateCommandList(0, CommandListType.Direct, cmdAllocator, null);

        cmdList.CopyTextureRegion(dstLocation, 0, 0, 0, srcLocation, null);
        cmdList.ResourceBarrier(new ResourceBarrier(new ResourceTransitionBarrier(pTexture, ResourceStates.CopyDestination, ResourceStates.PixelShaderResource)));
        cmdList.Close();

        queue.ExecuteCommandList(cmdList);
        queue.Signal(fence, 1);
        fence.SetEventOnCompletion(1, @event.DangerousGetHandle());
        PInvoke.WaitForSingleObject(@event, 0xFFFFFFFF);

        // Dispose
        cmdList.Dispose();
        cmdAllocator.Dispose();
        queue.Dispose();
        // @event.Dispose(); Dispose not needed, will be taken care of automatically
        fence.Dispose();
        uploadBuffer.Dispose();

        var cpuHandle = new CpuDescriptorHandle();
        var gpuHandle = new GpuDescriptorHandle();
        _textureHeapAllocator.Alloc(ref cpuHandle, ref gpuHandle);

        // https://learn.microsoft.com/en-us/windows/win32/api/d3d12/ne-d3d12-d3d12_shader_component_mapping
        const int D3D12_DEFAULT_SHADER_4_COMPONENT_MAPPING = 0 & 0x7 | (1 & 0x7) << 3 | (2 & 0x7) << 3 * 2 | (3 & 0x7) << 3 * 3 | 1 << 3 * 4;
        _device.CreateShaderResourceView(pTexture, new ShaderResourceViewDescription()
        {
            Format = Format.R8G8B8A8_UNorm,
            Dimension = ShaderResourceViewDimension.Texture2D,
            Texture2D = new ShaderResourceViewDescription.Texture2DResource()
            {
                MipLevels = 1,
                MostDetailedMip = 0,
            },
            Shader4ComponentMapping = D3D12_DEFAULT_SHADER_4_COMPONENT_MAPPING
        }, cpuHandle);

        _textureIds.TryAdd((ulong)gpuHandle.Ptr, new TextureResource()
        {
            Resource = pTexture,
            CpuDescHandle = cpuHandle,
            GpuDescHandle = gpuHandle
        });

        return (ulong)gpuHandle.Ptr;
    }

    public bool IsTextureLoaded(ulong texId)
    {
        return _textureIds.ContainsKey(texId);
    }

    public void FreeTexture(ulong texId)
    {
        if (!_textureIds.TryGetValue(texId, out TextureResource? textureHandle))
            throw new KeyNotFoundException("Texture was not found.");

        textureHandle.Dispose();
        _textureIds.TryRemove(texId, out _);
    }

    public void Disable()
    {
        _presentHook?.Disable();
        _resizeBuffersHook?.Disable();
        //_execCmdListHook?.Disable();
    }

    public void Enable()
    {
        _presentHook?.Enable();
        _resizeBuffersHook?.Enable();
        //_execCmdListHook?.Enable();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void SrvDescriptorAllocCallback(ImGui_ImplDX12_InitInfo_t* initInfo, CpuDescriptorHandle* cpuHandle, GpuDescriptorHandle* gpuHandle)
    {
        _textureHeapAllocator.Alloc(ref *cpuHandle, ref *gpuHandle);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void SrvDescriptorFreeCallback(ImGui_ImplDX12_InitInfo_t* a, CpuDescriptorHandle cpuHandle, GpuDescriptorHandle gpuHandle)
    {
        _textureHeapAllocator.Free(cpuHandle, gpuHandle);
    }

    #region Hook Functions
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint ResizeBuffersImplStatic(nint swapchainPtr, uint bufferCount, uint width, uint height, Format newFormat, SwapChainFlags swapchainFlags) => Instance.ResizeBuffersImpl(swapchainPtr, bufferCount, width, height, newFormat, swapchainFlags);

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint CreateSwapChainForHwndImplStatic(nint this_, nint pDevice, nint hWnd, nint pDesc, nint pFullscreenDesc, nint pRestrictToOutput, nint ppSwapChain) => Instance.CreateSwapChainForHwndImpl(this_, pDevice, hWnd, pDesc, pFullscreenDesc, pRestrictToOutput, ppSwapChain);

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint PresentImplStatic(nint swapChainPtr, int syncInterval, PresentFlags flags) => Instance.PresentImpl(swapChainPtr, syncInterval, flags);
    #endregion
}

class FrameContext
{
    public CommandAllocator CommandAllocator;
    public SharpDX.Direct3D12.Resource MainRenderTargetResource;
    public CpuDescriptorHandle main_render_target_descriptor;
};

// For ImGui
public unsafe partial struct ImGui_ImplDX12_InitInfo_t
{
    public nint Device;
    public nint CommandQueue;
    public int NumFramesInFlight;
    public int RTVFormat;
    public int DSVFormat;
    public void* UserData;
    public nint SrvDescriptorHeap;
    public delegate* unmanaged[Cdecl]<ImGui_ImplDX12_InitInfo_t*, CpuDescriptorHandle*, GpuDescriptorHandle*, void> SrvDescriptorAllocFn;
    public delegate* unmanaged[Cdecl]<ImGui_ImplDX12_InitInfo_t*, CpuDescriptorHandle, GpuDescriptorHandle, void> SrvDescriptorFreeFn;
    public nint LegacySingleSrvCpuDescriptor;
    public nint LegacySingleSrvGpuDescriptor;
}