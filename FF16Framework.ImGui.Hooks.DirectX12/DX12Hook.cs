using FF16Framework.ImGui.Hooks.Definitions;
using FF16Framework.ImGui.Hooks.DirectX12.Definitions;

using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Hooks.Definitions.X64;

using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using SharpDX.DXGI;

using System.Diagnostics;

using CallingConventions = Reloaded.Hooks.Definitions.X86.CallingConventions;

namespace FF16Framework.ImGui.Hooks.DirectX12;

/// <summary>
/// Provides access to DirectX 11 functions.
/// </summary>
public static class DX12Hook
{
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

    public static int? CommandQueueOffset = null;

    static DX12Hook()
    {
        // Uncomment this if you need debug logs with DebugView.
        // NOTE: Enabling this will severely impact framerate!
        // DebugInterface.Get().EnableDebugLayer();

        // Define
        var device = new SharpDX.Direct3D12.Device(null, FeatureLevel.Level_12_0);
        CommandQueue commandQueue = device.CreateCommandQueue(new CommandQueueDescription(CommandListType.Direct));
        var swapChainDesc = new SwapChainDescription()
        {
            BufferCount = 2,
            ModeDescription = new ModeDescription(100, 100, new Rational(60, 1), Format.R8G8B8A8_UNorm),
            Usage = Usage.RenderTargetOutput,
            SwapEffect = SwapEffect.FlipDiscard,
            OutputHandle = Process.GetCurrentProcess().MainWindowHandle,
            //Flags = SwapChainFlags.None,
            SampleDescription = new SampleDescription(1, 0),
            IsWindowed = true
        };

        using (var factory = new Factory2())
        using (var swapChain = new SwapChain(factory, commandQueue, swapChainDesc))
        {
            FactoryVTable = SDK.Hooks.VirtualFunctionTableFromObject(factory.NativePointer, Enum.GetNames(typeof(IDXGIFactory)).Length);
            SwapchainVTable = SDK.Hooks.VirtualFunctionTableFromObject(swapChain.NativePointer, Enum.GetNames(typeof(IDXGISwapChainVTable)).Length);
            ComamndQueueVTable = SDK.Hooks.VirtualFunctionTableFromObject(commandQueue.NativePointer, Enum.GetNames(typeof(ID3D12CommandQueueVTable)).Length);

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
        }

        if (CommandQueueOffset is null)
            throw new Exception("Could not determine command queue offset from DX12 Swapchain pointer.");

        // Cleanup
        device.Dispose();
    }

    [Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    [Reloaded.Hooks.Definitions.X86.Function(CallingConventions.Stdcall)]
    public struct Present { public FuncPtr<nint, int, PresentFlags, nint> Value; }

    [Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    [Reloaded.Hooks.Definitions.X86.Function(CallingConventions.Stdcall)]
    public struct ResizeBuffers { public FuncPtr<nint, uint, uint, uint, Format, SwapChainFlags, nint> Value; }

    [Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    [Reloaded.Hooks.Definitions.X86.Function(CallingConventions.Stdcall)]
    public struct CreateSwapChainForHwnd { public FuncPtr<nint, nint, nint, nint, nint, nint, nint, nint> Value; }

    [Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
    [Reloaded.Hooks.Definitions.X86.Function(CallingConventions.Stdcall)]
    public struct ExecuteCommandLists { public FuncPtr<nint, uint, nint, nint> Value; }
}