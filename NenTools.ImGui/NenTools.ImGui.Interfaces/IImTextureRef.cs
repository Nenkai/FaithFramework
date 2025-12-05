using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Interfaces;

public interface IImTextureRef
{
    public nint TexData { get; }
    public ulong TexID { get; }
}
