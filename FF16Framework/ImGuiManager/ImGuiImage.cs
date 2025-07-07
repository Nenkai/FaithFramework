using FF16Framework.Interfaces.ImGuiManager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGuiManager;

public class ImGuiImage : IImGuiImage
{
    public ulong TexId { get; set; }
    public uint Width { get; set; }
    public uint Height { get; set; }

    internal ImGuiTextureManager TextureManager { get; set; }
    internal bool Disposed { get; set; }

    public ImGuiImage(ImGuiTextureManager textureManager, ulong texId, uint width, uint height)
    {
        TextureManager = textureManager;

        TexId = texId;
        Width = width;
        Height = height;
    }

    public void Dispose()
    {
        TextureManager.FreeImage(this);
    }
}
