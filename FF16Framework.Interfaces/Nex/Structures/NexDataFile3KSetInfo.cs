using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public struct NexDataFile3KSetInfo
{
    public uint Key1;
    public uint RowSubSetOffset;
    public uint NumSubRows;
    public uint UnkOffset;
    public uint UnkAlways0;
}
#pragma warning restore CS1591
