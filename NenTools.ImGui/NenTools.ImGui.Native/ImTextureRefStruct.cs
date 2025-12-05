using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Native;

public struct ImTextureRefStruct
{
    public nint TexData;
    public ulong TexID;

    public ImTextureRefStruct(ulong texId)
    {
        TexID = texId;
    }
}
