using FF16Framework.Interfaces.Nex.Structures;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex;

/// <summary>
/// Returns a new row set result.
/// </summary>
public unsafe struct NexSetResult
{
    /// <summary>
    /// Row instances.
    /// </summary>
    public NexRowInstance* Rows;

    /// <summary>
    /// Number of rows returned.
    /// </summary>
    public long Count;
}
