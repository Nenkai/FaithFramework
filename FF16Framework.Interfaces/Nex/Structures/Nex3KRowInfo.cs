using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

/// <summary>
/// Info for a triple-keyed row.
/// </summary>
public struct Nex3KRowInfo
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
    /// Key 3.
    /// </summary>
    public uint Key3;

    /// <summary>
    /// Unused.
    /// </summary>
    public uint field_0x0C;

    /// <summary>
    /// Row data, relative to this struct.
    /// </summary>
    public int RowDataOffsetRelative;
}
