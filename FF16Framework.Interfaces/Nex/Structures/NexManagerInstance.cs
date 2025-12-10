using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0169 // The field is never used

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member