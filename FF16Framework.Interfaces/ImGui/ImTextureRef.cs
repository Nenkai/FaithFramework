using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.ImGui;

public struct ImTextureRef
{
    public nint TexData;
    public ulong TexID;

    public ImTextureRef(ulong texId)
    {
        TexID = texId;
    }
}
