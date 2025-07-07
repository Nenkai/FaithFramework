using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGui.Hooks.DirectX12.Definitions;

public enum IDXGIFactory
{
    // IUnknown
    QueryInterface,
    AddRef,
    Release,

    // IDXGIObject
    SetPrivateData,
    SetPrivateDataInterface,
    GetPrivateData,
    GetParent,

    // IDXGIFactory
    EnumAdapters,
    MakeWindowAssociation,
    GetWindowAssociation,
    CreateSwapChain,
    CreateSoftwareAdapter,

    // IDXGIFactory1
    EnumAdapters1,
    IsCurrent,

    // IDXGIFactory2
    IsWindowedStereoEnabled,
    CreateSwapChainForHwnd,
    CreateSwapChainForCoreWindow,
    GetSharedResourceAdapterLuid,
    RegisterStereoStatusWindow,
    RegisterStereoStatusEvent,
    UnregisterStereoStatus,
    RegisterOcclusionStatusWindow,
    RegisterOcclusionStatusEvent,
    UnregisterOcclusionStatus,
    CreateSwapChainForComposition,

    // IDXGIFactory3
    GetCreationFlags,

    // IDXGIFactory4
    EnumAdapterByLuid,
    EnumWarpAdapter,

    // IDXGIFactory5
    CheckFeatureSupport,

    // IDXGIFactory6
    EnumAdapterByGpuPreference,
}
