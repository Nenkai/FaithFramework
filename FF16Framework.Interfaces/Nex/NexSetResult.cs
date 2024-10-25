using FF16Framework.Interfaces.Nex.Structures;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex;

public unsafe struct NexSetResult
{
    public NexRowInstance* Rows;
    public long Count;
}
