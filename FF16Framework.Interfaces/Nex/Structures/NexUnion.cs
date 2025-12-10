using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.Nex.Structures;

/// <summary>
/// Represents a cell that is a union and may refer to a specific table or type of value.
/// </summary>
public struct NexUnionElement
{
    /// <summary>
    /// Union type.
    /// </summary>
    public ushort Type { get; set; }

    /// <summary>
    /// Union id/value.
    /// </summary>
    public int Value { get; set; }
}
