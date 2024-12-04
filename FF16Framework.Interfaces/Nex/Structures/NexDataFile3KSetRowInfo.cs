using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

public struct NexDataFile3KSubsetInfo
{
    public uint Key2;
    public uint UnkOffset;
    public uint UnkAlways0;
    public uint RowOffsets;
    public uint NumRows;
}
