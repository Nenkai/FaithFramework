﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGui.Hooks.Definitions;

public enum ID3D12CommandQueueVTable
{
    // IUnknown
    QueryInterface,
    AddRef,
    Release,

    // ID3D12Object
    GetPrivateData,
    SetPrivateData,
    SetPrivateDataInterface,
    SetName,

    // ID3D12DeviceChild
    GetDevice,

    // ID3D12Pageable

    // ID3D12CommandQueue
    UpdateTileMappings,
    CopyTileMappings,
    ExecuteCommandLists,
    SetMarker,
    BeginEvent,
    EndEvent,
    Signal,
    Wait,
    GetTimestampFrequency,
    GetClockCalibration,
    GetDesc,
};