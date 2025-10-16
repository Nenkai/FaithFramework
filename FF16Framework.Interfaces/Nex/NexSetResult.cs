using FF16Framework.Interfaces.Nex.Structures;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex;

/// <summary>
/// Result for a row set query.
/// </summary>
public unsafe struct NexSetResult
{
    /// <summary>
    /// Rows pointer.
    /// </summary>
    public NexRowInstance* Rows;

    /// <summary>
    /// Number of rows.
    /// </summary>
    public long Count;
}
