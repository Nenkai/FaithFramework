using SharpDX.Direct3D12;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Hooks.DirectX12;

public class TextureResource : IDisposable
{
    public required Resource Resource { get; set; }
    public required CpuDescriptorHandle CpuDescHandle { get; set; }
    public required GpuDescriptorHandle GpuDescHandle { get; set; }

    public void Dispose()
    {
        ((IDisposable)Resource).Dispose();
        GC.SuppressFinalize(this);
    }
}
