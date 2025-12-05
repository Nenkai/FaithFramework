using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Hooks.DirectX12.Definitions;

public enum IDXGISwapChainVTable
{
    // IUnknown
    QueryInterface = 0,
    AddRef = 1,
    Release = 2,

    // IDXGIObject
    SetPrivateData = 3,
    SetPrivateDataInterface = 4,
    GetPrivateData = 5,
    GetParent = 6,

    // IDXGIDeviceSubObject
    GetDevice = 7,

    // IDXGISwapChain
    Present = 8,
    GetBuffer = 9,
    SetFullscreenState = 10,
    GetFullscreenState = 11,
    GetDesc = 12,
    ResizeBuffers = 13,
    ResizeTarget = 14,
    GetContainingOutput = 15,
    GetFrameStatistics = 16,
    GetLastPresentCount = 17,

    // IDXGISwapChain1
    GetDesc1 = 18,
    GetFullscreenDesc = 19,
    GetHwnd = 20,
    GetCoreWindow = 21,
    Present1 = 22,
    IsTemporaryMonoSupported = 23,
    GetRestrictToOutput = 24,
    SetBackgroundColor = 25,
    GetBackgroundColor = 26,
    SetRotation = 27,
    GetRotation = 28,

    // IDXGISwapChain2
    SetSourceSize = 29,
    GetSourceSize = 30,
    SetMaximumFrameLatency = 31,
    GetMaximumFrameLatency = 32,
    GetFrameLatencyWaitableObject = 33,
    SetMatrixTransform = 34,
    GetMatrixTransform = 35,

    // IDXGISwapChain3
    GetCurrentBackBufferIndex = 36,
    CheckColorSpaceSupport = 37,
    SetColorSpace1 = 38,
    ResizeBuffers1 = 39,

    // IDXGISwapChain4
    SetHDRMetaData = 40,
}