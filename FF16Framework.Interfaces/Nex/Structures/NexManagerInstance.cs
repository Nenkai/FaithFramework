using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

// Size: 0x80
public struct NexManagerInstance
{
    ulong vtable;
    ulong qword8;
    ulong qword10;
    uint dword18;
    // pad
    ulong qword20;
    ulong NumTablesLoaded;
    StdUnorderedMap Tables;
};

