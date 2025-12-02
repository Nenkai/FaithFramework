using FF16Framework.ImGui.Hooks;
using FF16Framework.ImGui.Hooks.Definitions;
using FF16Framework.ImGui.Hooks.DirectX;
using FF16Framework.ImGui.Hooks.DirectX12.Definitions;
using FF16Framework.Interfaces.ImGui;
using FF16Framework.Native.ImGui;

using Reloaded.Hooks.Definitions;
using Reloaded.Memory.Interfaces;

using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using SharpDX.DXGI;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

using static FF16Framework.ImGui.Hooks.DirectX12.Extensions.Factory2Extensions;
using Device = SharpDX.Direct3D12.Device;

namespace FF16Framework.ImGui.Hooks.DirectX12;

public unsafe class ImguiHookDx12 : IImguiHook
{
    private bool _initializedD3D12 = false;

    private static readonly string[] _supportedDlls =
    [
        "d3d12.dll",
    ];

    private static DescriptorHeapAllocator _textureHeapAllocator;
    private ConcurrentDictionary<ulong, TextureResource> _textureIds = [];

    private IHook<PresentDelegate> _presentHook;
    private nuint _hookedSwapchainVtableAddr;
    private nuint _previousVTableAddr;
    private IHook<ResizeBuffersDelegate> _resizeBuffersHook;
    private IHook<ResizeTargetDelegate> _resizeTargetHook;
    private IHook<CreateSwapChainForHwnd> _createSwapChainForHwndHook;

    delegate nint PresentDelegate(nint swapChainPtr, int syncInterval, PresentFlags flags);
    delegate nint ResizeBuffersDelegate(nint swapchainPtr, uint bufferCount, uint width, uint height, Format newFormat, SwapChainFlags swapchainFlags);
    delegate nint ResizeTargetDelegate(nint swapchainPtr, nint pNewTargetParameters);
    delegate nint CreateSwapChainForHwnd(nint this_, nint pDevice, nint hWnd, SwapChainDescription* pDesc, SwapChainFullScreenDescription* pFullscreenDesc, nint pRestrictToOutput, nint ppSwapChain);

    private bool _convertPointerHooksToVtableHookPhase = false;

    [ThreadStatic]
    private static int _presentDepth;

    [ThreadStatic]
    private static int _resizeBufferDepth;

    [ThreadStatic]
    private static int _resizeTargetDepth;

    [ThreadStatic]
    private bool _createSwapChainRecursionLock;

    private bool _isHookingD3D12 = false; // Used to ensure CreateSwapChainForHwnd hook does not call recursively

    public Device Device { get; private set; }
    public SwapChain3 SwapChain { get; private set; }
    public CommandQueue CommandQueue { get; private set; }
    private List<CommandContext> _commandContexts = [];
    private DescriptorHeap _shaderResourceViewDescHeap;
    private DescriptorHeap _renderTargetViewDescHeap;
    private List<FrameContext> _frameContexts = [];
    public int _commandContextIndex;

    public void* _imGuiBackendRendererData;

    /// <summary>
    /// Contains the DX12 DXGI Factory VTable.
    /// </summary>
    public static IVirtualFunctionTable FactoryVTable { get; private set; }

    /// <summary>
    /// Contains the DX12 DXGI Swapchain VTable.
    /// </summary>
    public static IVirtualFunctionTable SwapchainVTable { get; private set; }

    /// <summary>
    /// Contains the DX12 DXGI Command Queue VTable.
    /// </summary>
    public static IVirtualFunctionTable ComamndQueueVTable { get; private set; }

    /*
    * In some cases (E.g. under DX9 + Viewports enabled), Dear ImGui might call
    * DirectX functions from within its internal logic.
    *
    * We put a lock on the current thread in order to prevent stack overflow.
    */

    public static int? CommandQueueOffset = null;

    public ImguiHookDx12()
    {
        ImguiHookDx12Wrapper.Instance = this;
    }

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

    /// <summary>
    /// Creates a dummy device/swapchain used to locate vtables and offsets used by D3D12.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="Exception"></exception>
    private void CreateDummySwapchainAndFindVTables()
    {
        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Creating dummy D3D12 device for hook purposes...");

        SharpDX.Direct3D12.Device device;
        var createDevice = PInvoke.GetProcAddress(PInvoke.GetModuleHandle("d3d12.dll"), "D3D12CreateDevice");

        // D3D12CreateDevice may have been hooked beforehand (Reshade, etc), so unhook it temporarily
        // we're creating a dummy device, so we preferably don't want anyone to treat it as something to interact with
        List<byte>? originalFunctionBytesDiff = HookUtility.GetOriginalBytesIfHooked((nuint)createDevice.Value);
        if (originalFunctionBytesDiff is not null)
        {
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] D3D12CreateDevice appears to be hooked by someone else before us, unhooking temporarily for dummy device creation...");

            Span<byte> previouslyHookedBytes = new byte[originalFunctionBytesDiff.Count];
            Reloaded.Memory.Memory.Instance.SafeRead((nuint)createDevice.Value, previouslyHookedBytes); // Read hooked bytes
            Reloaded.Memory.Memory.Instance.SafeWrite((nuint)createDevice.Value, CollectionsMarshal.AsSpan(originalFunctionBytesDiff)); // Restore original

            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] D3D12CreateDevice unhooked, creating device.");
            device = new SharpDX.Direct3D12.Device(null, FeatureLevel.Level_12_0); // Call original
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] D3D12CreateDevice dummy device created, restoring previous hook.");

            Reloaded.Memory.Memory.Instance.SafeWrite((nuint)createDevice.Value, previouslyHookedBytes); // Restore hooked bytes
        }
        else
        {
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] D3D12CreateDevice is not hooked, creating device.");

            try
            {
                device = new SharpDX.Direct3D12.Device(null, FeatureLevel.Level_12_0);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create D3D12 Device for vtable detection.", ex);
            }
        }
        device.Name = $"[{nameof(ImguiHookDx12)}] Dummy D3D12 device";

        CommandQueue commandQueue = device.CreateCommandQueue(new CommandQueueDescription(CommandListType.Direct));
        commandQueue.Name = $"[{nameof(ImguiHookDx12)}] Dummy command queue";

        var swapChainDesc = new SwapChainDescription1()
        {
            Format = Format.R8G8B8A8_UNorm,
            Usage = Usage.RenderTargetOutput,
            SwapEffect = SwapEffect.FlipDiscard,
            BufferCount = 2,
            SampleDescription = new SampleDescription(1, 0),
            AlphaMode = AlphaMode.Premultiplied,
            Width = 1,
            Height = 1,
        };

        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Creating dummy swapchain.");

        SwapChain1? swapChain = null;
        using (var factory = new Factory6()) // Will call CreateDXGIFactory2
        {
            // We ideally don't want to create a swapchain through CreateSwapChainForHwnd, because it may have been hooked
            // Try multiple ways
            if (!factory.CreateSwapChainForComposition(commandQueue, ref swapChainDesc, null!, out swapChain).Success)
            {
                if (!CreateHandleWithDummyWindow(factory, commandQueue, out swapChainDesc, out swapChain)) // TODO: Verify if this one even works
                {
                    throw new Exception("Could not create dummy swapchain");
                }
                else
                    DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Swapchain created with CreateHandleWithDummyWindow");
            }
            else
                DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Swapchain created with CreateSwapChainForComposition");
            swapChain.DebugName = $"[{nameof(ImguiHookDx12)}] Dummy swapchain";

            // TODO: use this to figure if it's a DLSS or FSR object? could never get that working
            var typeInfo = RTTIUtility.GetLocator((void*)swapChain.NativePointer);

            FactoryVTable = SDK.Hooks.VirtualFunctionTableFromObject(factory.NativePointer, Enum.GetNames(typeof(IDXGIFactory)).Length);
            SwapchainVTable = SDK.Hooks.VirtualFunctionTableFromObject(swapChain.NativePointer, Enum.GetNames(typeof(IDXGISwapChainVTable)).Length);
            ComamndQueueVTable = SDK.Hooks.VirtualFunctionTableFromObject(commandQueue.NativePointer, Enum.GetNames(typeof(ID3D12CommandQueueVTable)).Length);

            // We need to try and find the command queue from the swapchain
            // We are not allowed to simply use it from hooked calls (D3D12 may forbid this)
            // So search for the pointer in the dummy swapchain
            unsafe
            {
                nint* ptr = (nint*)swapChain.NativePointer;
                for (int i = 0; i < 1000; i++)
                {
                    if (ptr[i] == commandQueue.NativePointer)
                    {
                        CommandQueueOffset = i * 8;
                        break;
                    }
                }
            }

            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Swapchain command queue offset: 0x{CommandQueueOffset:X}");

            if (CommandQueueOffset is null)
                throw new Exception("Could not determine command queue offset from DX12 Swapchain pointer.");

            swapChain?.Dispose();
        }

        // Cleanup
        device.Dispose();
    }

    private static bool CreateHandleWithDummyWindow(Factory2 factory, CommandQueue commandQueue, out SwapChainDescription1 swapChainDesc, out SwapChain1 swapChain)
    {
        var handle = PInvoke.GetModuleHandle(null);

        const string className = $"{nameof(ImguiHookDx12)}_DummyClassName";
        fixed (char* pClassName = className)
        {
            var wc = new WNDCLASSEXW()
            {
                cbSize = 0x50,
                style = WNDCLASS_STYLES.CS_HREDRAW | WNDCLASS_STYLES.CS_VREDRAW,
                lpfnWndProc = PInvoke.DefWindowProc,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = new HINSTANCE(handle.DangerousGetHandle()),
                hIcon = HICON.Null,
                hCursor = HCURSOR.Null,
                hbrBackground = Windows.Win32.Graphics.Gdi.HBRUSH.Null,
                lpszClassName = pClassName
            };

            PInvoke.RegisterClassEx(wc);

            var hwnd = PInvoke.CreateWindowEx(0, className, $"[{nameof(ImguiHookDx12)}] DX Dummy Window", WINDOW_STYLE.WS_OVERLAPPEDWINDOW, 0, 0, 100, 100, HWND.Null, null, handle, null);

            swapChainDesc.BufferCount = 3;
            swapChainDesc.Width = 0;
            swapChainDesc.Height = 0;
            swapChainDesc.Format = Format.R8G8B8A8_UNorm;
            swapChainDesc.Flags = SwapChainFlags.FrameLatencyWaitAbleObject;
            swapChainDesc.Usage = Usage.RenderTargetOutput;
            swapChainDesc.SampleDescription.Count = 1;
            swapChainDesc.SampleDescription.Quality = 0;
            swapChainDesc.SwapEffect = SwapEffect.FlipDiscard;
            swapChainDesc.AlphaMode = AlphaMode.Unspecified;
            swapChainDesc.Scaling = Scaling.Stretch;
            swapChainDesc.Stereo = false;

            return factory.CreateSwapChainForHwnd(commandQueue, hwnd, ref swapChainDesc, null, null, out swapChain).Success;
        }
    }

    /// <summary>
    /// Finds and hooks D3D12 functions.
    /// </summary>
    public void InitAndEnableHooks()
    {
        _isHookingD3D12 = true;
        if (CommandQueueOffset == 0 || SwapchainVTable is null && ComamndQueueVTable is null) // We are reusing known pointers if we already found them
            CreateDummySwapchainAndFindVTables();

        // Got our pointers, start hooking functions.
        HookBaseD3D12Functions();
        _isHookingD3D12 = false;
    }

    /// <summary>
    /// Hooks D3D12 functions after their vtables have been located.
    /// </summary>
    private void HookBaseD3D12Functions()
    {
        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Hooking base D3D12 functions...");

        // TODO: Hook streamline 

        DisableHooks();

        _convertPointerHooksToVtableHookPhase = true;

        // Hook vtable entries, least intrusive
        nuint presentPointerAddr = (nuint)SwapchainVTable[(int)IDXGISwapChainVTable.Present].EntryAddress;
        _presentHook = new FunctionPointerHook<PresentDelegate>(presentPointerAddr, PresentImpl).Activate();

        nuint createSwapChainForHwndPointerAddr = (nuint)FactoryVTable[(int)IDXGIFactory.CreateSwapChainForHwnd].EntryAddress;
        _createSwapChainForHwndHook ??= new FunctionPointerHook<CreateSwapChainForHwnd>(createSwapChainForHwndPointerAddr, CreateSwapChainForHwndImpl).Activate();
    }

    ~ImguiHookDx12()
    {
        Dispose();
    }

    public void Dispose()
    {
        ShutdownD3D12(isReinit: false);
        DisableHooks();

        _textureHeapAllocator?.Destroy();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Hooked CreateSwapChainForHwnd. Called when switching to FSR Frame Gen.
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="pDevice"></param>
    /// <param name="hWnd"></param>
    /// <param name="pDesc"></param>
    /// <param name="pFullscreenDesc"></param>
    /// <param name="pRestrictToOutput"></param>
    /// <param name="ppSwapChain"></param>
    /// <returns></returns>
    public nint CreateSwapChainForHwndImpl(nint factory, nint pDevice, nint hWnd, SwapChainDescription* pDesc, SwapChainFullScreenDescription* pFullscreenDesc, nint pRestrictToOutput, nint ppSwapChain)
    {
        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] CreateSwapChainForHwndImpl called");

        if (_isHookingD3D12)
            return _createSwapChainForHwndHook.OriginalFunction.Invoke(factory, pDevice, hWnd, pDesc, pFullscreenDesc, pRestrictToOutput, ppSwapChain);

        DisableHooks(); // FSR/DLSS creates a new swapchain. We have hooks on the vtable pointer of the original swapchain, we need to restore it
        ShutdownD3D12();

        var res = _createSwapChainForHwndHook.OriginalFunction.Invoke(factory, pDevice, hWnd, pDesc, pFullscreenDesc, pRestrictToOutput, ppSwapChain);

        InitAndEnableHooks();

        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] CreateSwapChainForHwndImpl end");
        return res;
    }

    /// <summary>
    /// Initializes D3D12 for ImGui rendering.
    /// </summary>
    /// <returns></returns>
    private bool InitD3D12()
    {
        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] InitD3D12 (buffer count: {SwapChain.Description.BufferCount})");

        ShutdownD3D12();
        ImGuiMethods.cImGui_ImplWin32_Init(SwapChain.Description.OutputHandle);

        for (int i = 0; i < 3; i++)
        {
            var commandContext = new CommandContext();
            commandContext.Setup(this);
            _commandContexts.Add(commandContext);
        }

        // Create SRV Heap
        var descriptorImGuiRender = new DescriptorHeapDescription
        {
            Type = DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView,
            DescriptorCount = 11,
            Flags = DescriptorHeapFlags.ShaderVisible
        };

        _shaderResourceViewDescHeap = Device.CreateDescriptorHeap(descriptorImGuiRender);
        if (_shaderResourceViewDescHeap == null)
        {
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Failed to create shader resource view descriptor heap.");
            return false;
        }
        _shaderResourceViewDescHeap.Name = $"[{nameof(ImguiHookDx12)}] ShaderResourceViewDescHeap";

        // Create RTV Heap
        var renderTargetDesc = new DescriptorHeapDescription
        {
            Type = DescriptorHeapType.RenderTargetView,
            DescriptorCount = SwapChain.Description.BufferCount, // depends based on user's OS settings, don't hardcode a number here
            Flags = DescriptorHeapFlags.None,
            NodeMask = 1
        };
        _renderTargetViewDescHeap = Device.CreateDescriptorHeap(renderTargetDesc);
        if (_renderTargetViewDescHeap is null)
            return false;

        var rtvDescriptorSize = Device.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
        var rtvHandle = _renderTargetViewDescHeap.CPUDescriptorHandleForHeapStart;
        _renderTargetViewDescHeap.Name = $"[{nameof(ImguiHookDx12)}] RenderTargetViewHeap";

        // Get RTVs
        SwapChain swapChain = SwapChain;
        for (var i = 0; i < swapChain.Description.BufferCount; i++)
        {
            _frameContexts.Add(new FrameContext
            {
                MainRenderTargetDescriptor = rtvHandle,
                MainRenderTargetResource = swapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i),
            });
            Device.CreateRenderTargetView(_frameContexts[i].MainRenderTargetResource, null, rtvHandle);
            rtvHandle.Ptr += rtvDescriptorSize;
        }

        var props = new HeapProperties()
        {
            Type = HeapType.Default,
            CPUPageProperty = CpuPageProperty.Unknown,
            MemoryPoolPreference = MemoryPool.Unknown
        };

        // Create our texture heap allocator/pool, for ImGui textures
        // TODO: Do NOT recreate this, because textures may be lost between resets
        _textureHeapAllocator = new DescriptorHeapAllocator();
        var heap = Device.CreateDescriptorHeap(new DescriptorHeapDescription()
        {
            DescriptorCount = 100,
            Type = DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView,
            Flags = DescriptorHeapFlags.ShaderVisible,
        });
        heap.Name = $"[{nameof(ImguiHookDx12)}] Texture Descriptor Heap";
        _textureHeapAllocator.Create(Device, heap);

        var initInfo = new ImGui_ImplDX12_InitInfo_t
        {
            Device = Device.NativePointer,
            CommandQueue = CommandQueue.NativePointer,
            NumFramesInFlight = swapChain.Description.BufferCount,
            RTVFormat = 28, // DXGI_FORMAT_R8G8B8A8_UNORM
            SrvDescriptorHeap = _shaderResourceViewDescHeap.NativePointer,
            SrvDescriptorAllocFn = &ImguiHookDx12Wrapper.SrvDescriptorAllocCallback,
            SrvDescriptorFreeFn = &ImguiHookDx12Wrapper.SrvDescriptorFreeCallback,
        };

        ImGuiMethods.cImGui_ImplDX12_Init((nint)(&initInfo));
        _imGuiBackendRendererData = ImGuiMethods.GetIO()->BackendRendererUserData;

        _initializedD3D12 = true;
        return true;
    }

    /// <summary>
    /// Shuts down D3D12 for ImGui rendering. This will clean up all known resources and call ImGui_ImplDX12_Shutdown.
    /// </summary>
    private void ShutdownD3D12(bool isReinit = true)
    {
        if (isReinit)
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Cleaning up D3D12 for reinitialization");
        else
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] ShutdownD3D12 called ");

        if (_imGuiBackendRendererData is not null)
            ImGuiMethods.cImGui_ImplDX12_Shutdown();

        foreach (var commandContext in _commandContexts)
            commandContext.Reset();
        _commandContexts.Clear();

        // ResizeBuffer requires swapchain resources to be freed.
        foreach (var ctx in _frameContexts)
        {
            // NOTE: This may crash/error here if:
            // The vtable pointer on the swapchain hasn't been restored.
            // The way it happens is:
            // - Turning on and off FSR Frame Gen repeatedly
            // - Switching from fullscreen to windowed
            // - Turning on DirectX debug layer?
            // This should now be solved in CreateSwapChainForHwnd where we restore hooks before shutting down.
            ctx.MainRenderTargetResource?.Dispose();
            ctx.MainRenderTargetResource = null!;
        }

        _frameContexts.Clear();

        _textureHeapAllocator?.Destroy();
        _renderTargetViewDescHeap?.Dispose();
        _shaderResourceViewDescHeap?.Dispose();

        if (ImGuiMethods.GetIO()->BackendPlatformName is not null && Marshal.PtrToStringAnsi((nint)ImGuiMethods.GetIO()->BackendPlatformName)!.Contains("win32"))
            ImGuiMethods.cImGui_ImplWin32_Shutdown();

        _imGuiBackendRendererData = null;
        _initializedD3D12 = false;
    }


    /// <summary>
    /// Disables D3D12 hooks, no D3D12 function will be intercepted.
    /// </summary>
    public void DisableHooks()
    {
        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Disabling D3D12 hooks (if any)");

        _presentHook?.Disable();
        _resizeBuffersHook?.Disable();
        _resizeTargetHook?.Disable();

        // HookedVirtualFunctionTableFromObject/HookedObjectVirtualFunctionTable (Reloaded-II) does not give us a way to restore the vtable pointer
        // So restore it ourselves.
        if (_hookedSwapchainVtableAddr != 0 && _previousVTableAddr != 0)
        {
            // We gotta restore the vtable pointer. It may cause trouble if the game or anybody else tries to free a swapchain.
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Restoring swapchain vtable pointer which we previously hooked");

            Interlocked.Exchange(ref *(nuint*)_hookedSwapchainVtableAddr, _previousVTableAddr);
            _convertPointerHooksToVtableHookPhase = true;

            _hookedSwapchainVtableAddr = 0;
            _previousVTableAddr = 0;
        }
    }

    /// <summary>
    /// Enables D3D12 hooks and starts intercepting D3D12 functions.
    /// </summary>
    public void EnableHooks()
    {
        _presentHook?.Enable();
        _resizeBuffersHook?.Enable();
        _resizeTargetHook?.Enable();
    }

    #region Present Hook
    /// <summary>
    /// Hooked IDXGISwapChain::Present function.
    /// </summary>
    /// <param name="swapChainPtr"></param>
    /// <param name="syncInterval"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public nint PresentImpl(nint swapChainPtr, int syncInterval, PresentFlags flags)
    {
        PresentDelegate originalFunc = _presentHook.OriginalFunction;

        SwapChain3 swapChain = new SwapChain3(swapChainPtr);

        // Ignore windows which don't belong to us.
        var windowHandle = swapChain.Description.OutputHandle;
        if (!ImguiHook.CheckWindowHandle(windowHandle))
        {
            Debug.WriteLine($"[{nameof(ImguiHookDx12)}] Discarding Window Handle {windowHandle:X} due to Mismatch");
            return originalFunc(swapChainPtr, syncInterval, flags);
        }

        if (!_convertPointerHooksToVtableHookPhase && (nuint)swapChainPtr != _hookedSwapchainVtableAddr)
            return originalFunc(swapChainPtr, syncInterval, flags);

        // For safety reasons, we switch from vtable entry pointer hooks to whole vtable hook
        if (_convertPointerHooksToVtableHookPhase)
        {
            _presentHook.Disable();

            // Save the swapchain pointer/vtable pointers
            _hookedSwapchainVtableAddr = (nuint)swapChainPtr;
            _previousVTableAddr = *(nuint*)swapChainPtr;

            // NOTE: Must ensure that IDXGISwapChainVTable has all the members defined for IDXGISwapChain (including inherited ones), since we are copying the whole vtable.
            // It should have all members up to IDXGISwapChain4.
            IVirtualFunctionTable hookedVTable = SDK.Hooks.HookedVirtualFunctionTableFromObject(swapChainPtr, Enum.GetNames<IDXGISwapChainVTable>().Length);
            _presentHook = hookedVTable.CreateFunctionHook<PresentDelegate>((int)IDXGISwapChainVTable.Present, PresentImpl).Activate();
            _resizeBuffersHook = hookedVTable.CreateFunctionHook<ResizeBuffersDelegate>((int)IDXGISwapChainVTable.ResizeBuffers, ResizeBuffersImpl).Activate();
            _resizeTargetHook = hookedVTable.CreateFunctionHook<ResizeTargetDelegate>((int)IDXGISwapChainVTable.ResizeTarget, ResizeTargetImpl).Activate();

            _convertPointerHooksToVtableHookPhase = false;
            originalFunc = _presentHook.OriginalFunction;
        }

        SwapChain = swapChain;
        Device = SwapChain.GetDevice<Device>();

        // TODO proton swapchain
        CommandQueue = new CommandQueue(*(nint*)(swapChainPtr + CommandQueueOffset!.Value));

        if (_presentDepth > 0)
        {
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Present discarding via Recursion Lock");

            _presentDepth++;
            var res_ = originalFunc.Invoke(swapChainPtr, syncInterval, flags);
            if (res_ != nint.Zero)
                DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Present original failed 0x{res_:X8}");
            _presentDepth--;
            return res_;
        }

        OnPresent();

        _presentDepth++;
        var result = originalFunc(swapChainPtr, syncInterval, flags);
        if (result != nint.Zero)
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Present original failed 0x{result:X8}");

        _presentDepth--;

        return result;
    }

    private bool _hasHookedWinProc = false;
    /// <summary>
    /// Our own function for handling present calls.
    /// </summary>
    private void OnPresent()
    {
        if (!_initializedD3D12)
        {
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] Init D3D12 handle for ImGui, Window Handle: {SwapChain.Description.OutputHandle:X}");
            if (!_hasHookedWinProc)
            {
                ImguiHook.InitializeWithHandle(SwapChain.Description.OutputHandle);
                _hasHookedWinProc = true;
            }

            InitD3D12();

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

        // Triple buffer
        CommandContext commandContext = _commandContexts[_commandContextIndex++ % _commandContexts.Count];
        FrameContext currentFrameContext = _frameContexts[SwapChain.CurrentBackBufferIndex];

        commandContext.Wait(TimeSpan.FromSeconds(-1)); // INFINITE
        lock (commandContext.Lock)
        {
            commandContext.HasCommands = true;

            var barrier = new ResourceBarrier
            {
                Type = ResourceBarrierType.Transition,
                Flags = ResourceBarrierFlags.None,
                Transition = new ResourceTransitionBarrier(currentFrameContext.MainRenderTargetResource, -1, ResourceStates.Present, ResourceStates.RenderTarget)
            };
            commandContext.CommandList.ResourceBarrier(barrier);
            commandContext.CommandList.SetRenderTargets(currentFrameContext.MainRenderTargetDescriptor, null);
            commandContext.CommandList.SetDescriptorHeaps(_shaderResourceViewDescHeap);

            ImGuiMethods.cImGui_ImplDX12_RenderDrawData((nint)ImGuiMethods.GetDrawData(), commandContext.CommandList.NativePointer);

            barrier.Transition = new ResourceTransitionBarrier(currentFrameContext.MainRenderTargetResource, -1, ResourceStates.RenderTarget, ResourceStates.Present);
            commandContext.CommandList.ResourceBarrier(barrier);

            commandContext.Execute(CommandQueue);
        }
    }
    #endregion

    #region ResizeBuffers Hook
    /// <summary>
    /// Hook for IDXGISwapChain::ResizeBuffers, for handling window resizes.
    /// </summary>
    /// <param name="swapchainPtr"></param>
    /// <param name="bufferCount"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="newFormat"></param>
    /// <param name="swapchainFlags"></param>
    /// <returns></returns>
    public nint ResizeBuffersImpl(nint swapchainPtr, uint bufferCount, uint width, uint height, Format newFormat, SwapChainFlags swapchainFlags)
    {
        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] ResizeBuffers Start");

        if (_resizeBufferDepth > 0)
        {
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] ResizeBuffers discarding wia Recursion Lock");

            _resizeBufferDepth++;
            var res = _resizeBuffersHook.OriginalFunction(swapchainPtr, bufferCount, width, height, newFormat, swapchainFlags);
            _resizeBufferDepth--;
            return res;
        }

        // Dispose all frame context resources
        OnBuffersResized();

        _resizeBufferDepth++;
        var result = _resizeBuffersHook.OriginalFunction(swapchainPtr, bufferCount, width, height, newFormat, swapchainFlags);
        _resizeBufferDepth--;

        if (result != nint.Zero)
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] ResizeBuffers original failed with {result:X}");

        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] ResizeBuffers end");
        return result;
    }

    private void OnBuffersResized()
    {
        // Deinit everything.
        ShutdownD3D12();
    }
    #endregion

    #region ResizeTarget Hook
    /// <summary>
    /// Hook for IDXGISwapChain::ResizeTarget, for handling windowed/fullscreen switches.
    /// </summary>
    /// <param name="swapchainPtr"></param>
    /// <param name="pNewTargetParameters"></param>
    /// <returns></returns>
    public nint ResizeTargetImpl(nint swapchainPtr, nint pNewTargetParameters)
    {
        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] ResizeTarget end");
        if (_resizeTargetDepth > 0)
        {
            Debug.WriteLine($"[{nameof(ImguiHookDx12)}] ResizeTarget discarding via Recursion Lock");

            _resizeTargetDepth++;
            var res = _resizeTargetHook.OriginalFunction(swapchainPtr, pNewTargetParameters);
            _resizeTargetDepth--;
            return res;
        }

        // Dispose all frame context resources
        OnTargetResized();

        _resizeTargetDepth++;
        var result = _resizeTargetHook.OriginalFunction(swapchainPtr, pNewTargetParameters);
        _resizeTargetDepth--;

        if (result != nint.Zero)
            DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] ResizeTarget original failed with {result:X}");

        DebugLog.WriteLine($"[{nameof(ImguiHookDx12)}] ResizeTarget End {result:X8}");
        return result;
    }

    private void OnTargetResized()
    {
        // Deinit everything.
        ShutdownD3D12();
    }
    #endregion

    public void SrvDescriptorAllocCallback(ImGui_ImplDX12_InitInfo_t* initInfo, CpuDescriptorHandle* cpuHandle, GpuDescriptorHandle* gpuHandle)
    {
        _textureHeapAllocator.Alloc(ref *cpuHandle, ref *gpuHandle);
    }

    public void SrvDescriptorFreeCallback(ImGui_ImplDX12_InitInfo_t* a, CpuDescriptorHandle cpuHandle, GpuDescriptorHandle gpuHandle)
    {
        _textureHeapAllocator.Free(cpuHandle, gpuHandle);
    }

    #region Texture Work
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
        SharpDX.Direct3D12.Resource pTexture = Device.CreateCommittedResource(heapProperties, HeapFlags.None, imageDesc, ResourceStates.CopyDestination);

        const int D3D12_TEXTURE_DATA_PITCH_ALIGNMENT = 256;
        var uploadBufferHeapDesc = new HeapProperties(HeapType.Upload);
        uint uploadPitch = (uint)(imageWidth * 4 + D3D12_TEXTURE_DATA_PITCH_ALIGNMENT - 1u & ~(D3D12_TEXTURE_DATA_PITCH_ALIGNMENT - 1u));
        uint uploadSize = imageHeight * uploadPitch;

        var tempDesc = new ResourceDescription(ResourceDimension.Buffer, 0, uploadSize, 1, 1, 1, Format.Unknown, 1, 0, TextureLayout.RowMajor, ResourceFlags.None);
        SharpDX.Direct3D12.Resource uploadBuffer = Device.CreateCommittedResource(uploadBufferHeapDesc, HeapFlags.None, tempDesc, ResourceStates.GenericRead);

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

        Fence fence = Device.CreateFence(0, FenceFlags.None);
        var @event = PInvoke.CreateEvent(null, false, false, (string)null);
        var queue = Device.CreateCommandQueue(CommandListType.Direct, 1);
        var cmdAllocator = Device.CreateCommandAllocator(CommandListType.Direct);
        var cmdList = Device.CreateCommandList(0, CommandListType.Direct, cmdAllocator, null);

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
        @event.Dispose();
        fence.Dispose();
        uploadBuffer.Dispose();

        var cpuHandle = new CpuDescriptorHandle();
        var gpuHandle = new GpuDescriptorHandle();
        _textureHeapAllocator.Alloc(ref cpuHandle, ref gpuHandle);

        // https://learn.microsoft.com/en-us/windows/win32/api/d3d12/ne-d3d12-d3d12_shader_component_mapping
        const int D3D12_DEFAULT_SHADER_4_COMPONENT_MAPPING = 0 & 0x7 | (1 & 0x7) << 3 | (2 & 0x7) << 3 * 2 | (3 & 0x7) << 3 * 3 | 1 << 3 * 4;
        Device.CreateShaderResourceView(pTexture, new ShaderResourceViewDescription()
        {
            Format = Format.R8G8B8A8_UNorm,
            Dimension = SharpDX.Direct3D12.ShaderResourceViewDimension.Texture2D,
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
    #endregion

}

public unsafe class ImguiHookDx12Wrapper
{
    public static ImguiHookDx12 Instance { get; set; }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void SrvDescriptorAllocCallback(ImGui_ImplDX12_InitInfo_t* initInfo, CpuDescriptorHandle* cpuHandle, GpuDescriptorHandle* gpuHandle) => Instance.SrvDescriptorAllocCallback(initInfo, cpuHandle, gpuHandle);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void SrvDescriptorFreeCallback(ImGui_ImplDX12_InitInfo_t* initInfo, CpuDescriptorHandle cpuHandle, GpuDescriptorHandle gpuHandle) => Instance.SrvDescriptorFreeCallback(initInfo, cpuHandle, gpuHandle);

    #region Hook Functions
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    public static nint ResizeBuffersImplStatic(nint swapchainPtr, uint bufferCount, uint width, uint height, Format newFormat, SwapChainFlags swapchainFlags) => Instance.ResizeBuffersImpl(swapchainPtr, bufferCount, width, height, newFormat, swapchainFlags);

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    public static nint CreateSwapChainForHwndImplStatic(nint this_, nint pDevice, nint hWnd, SwapChainDescription* pDesc, SwapChainFullScreenDescription* pFullscreenDesc, nint pRestrictToOutput, nint ppSwapChain) => Instance.CreateSwapChainForHwndImpl(this_, pDevice, hWnd, pDesc, pFullscreenDesc, pRestrictToOutput, ppSwapChain);

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    public static nint PresentImplStatic(nint swapChainPtr, int syncInterval, PresentFlags flags) => Instance.PresentImpl(swapChainPtr, syncInterval, flags);
    #endregion
}

class FrameContext
{
    public SharpDX.Direct3D12.Resource MainRenderTargetResource;
    public CpuDescriptorHandle MainRenderTargetDescriptor;

    public void Reset()
    {
        MainRenderTargetResource?.Dispose();
    }
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