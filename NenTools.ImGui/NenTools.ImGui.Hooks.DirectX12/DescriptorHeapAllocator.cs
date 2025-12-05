using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D12;

namespace NenTools.ImGui.Hooks.DirectX12;

// Taken from imgui examples
public class DescriptorHeapAllocator
{
    private DescriptorHeap Heap;
    private DescriptorHeapType HeapType = DescriptorHeapType.NumTypes;
    private CpuDescriptorHandle HeapStartCpu;
    private GpuDescriptorHandle HeapStartGpu;
    private uint HeapHandleIncrement;
    private Stack<int> FreeIndices = [];

    public void Create(Device device, DescriptorHeap heap)
    {
        ArgumentNullException.ThrowIfNull(device, nameof(device));
        ArgumentNullException.ThrowIfNull(heap, nameof(heap));

        Heap = heap;
        DescriptorHeapDescription desc = heap.Description;
        HeapType = desc.Type;
        HeapStartCpu = Heap.CPUDescriptorHandleForHeapStart;
        HeapStartGpu = Heap.GPUDescriptorHandleForHeapStart;
        HeapHandleIncrement = (uint)device.GetDescriptorHandleIncrementSize(HeapType);
        FreeIndices = new Stack<int>(desc.DescriptorCount);
        for (int n = desc.DescriptorCount; n > 0; n--)
            FreeIndices.Push(n - 1);
    }

    public void Destroy()
    {
        Heap?.Dispose();
        FreeIndices.Clear();
    }

    public void Alloc(ref CpuDescriptorHandle outCpuDescHandle, ref GpuDescriptorHandle outGpuDescHandle)
    {
        int idx = FreeIndices.Peek();
        FreeIndices.Pop();
        outCpuDescHandle.Ptr = HeapStartCpu.Ptr + idx * HeapHandleIncrement;
        outGpuDescHandle.Ptr = HeapStartGpu.Ptr + idx * HeapHandleIncrement;
    }

    public void Free(CpuDescriptorHandle outCpuDescHandle, GpuDescriptorHandle outGpuDescHandle)
    {
        int cpuIdx = (int)((outCpuDescHandle.Ptr - HeapStartCpu.Ptr) / HeapHandleIncrement);
        int gpuIdx = (int)((outGpuDescHandle.Ptr - HeapStartGpu.Ptr) / HeapHandleIncrement);
        FreeIndices.Push(cpuIdx);
    }
};
