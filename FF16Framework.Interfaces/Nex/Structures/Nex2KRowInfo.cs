using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

/// <summary>
/// Info for a double-keyed row.
/// </summary>
public struct Nex2KRowInfo
{
    /// <summary>
    /// Key 1.
    /// </summary>
    public uint Key1;

    /// <summary>
    /// Key 2.
    /// </summary>
    public uint Key2;

    /// <summary>
    /// Offset to the row data, starting from this struct.
    /// </summary>
    public int RowDataOffsetRelative;
}
