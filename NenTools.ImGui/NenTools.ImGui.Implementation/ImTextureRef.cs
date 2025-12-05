using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Native;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Implementation;

public class ImTextureRef : IImTextureRef
{
    public nint TexData { get; }
    public ulong TexID { get; }

    public ImTextureRef(ulong texId)
    {
        TexID = texId;
    }

    public unsafe ImTextureRef(ImTextureRefStruct* @struct)
    {
        TexData = @struct->TexData;
        TexID = @struct->TexID;
    }

    public ImTextureRef(ImTextureRefStruct @struct)
    {
        TexData = @struct.TexData;
        TexID = @struct.TexID;
    }

    public ImTextureRefStruct ToStruct() => new ImTextureRefStruct() { TexData =  TexData, TexID = TexID };
}
