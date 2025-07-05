using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.ImGuiManager;

public class ImGuiImage
{
    public ulong TexId { get; set; }
    public uint Width { get; set; }
    public uint Height { get; set; }

    public ImGuiImage(ulong texId, uint width, uint height)
    {
        TexId = texId;
        Width = width;
        Height = height;
    }
}
