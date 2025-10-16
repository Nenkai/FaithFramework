using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

/// <summary>
/// Info for a single-keyed row.
/// </summary>
public struct Nex1KRowInfo
{
    /// <summary>
    /// Key.
    /// </summary>
    public uint Key1;

    /// <summary>
    /// Offset to the raw data, starting from this struct.
    /// </summary>
    public int RowDataOffsetRelative;
}
