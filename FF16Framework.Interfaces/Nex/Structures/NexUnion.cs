using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

public struct NexUnionElement
{
    public ushort Type { get; set; }
    public int Value { get; set; }
}
