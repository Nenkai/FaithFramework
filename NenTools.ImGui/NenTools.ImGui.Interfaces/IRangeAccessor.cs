using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Interfaces;

/// <summary>
/// Represents a fixed size array/range accessor.
/// </summary>
/// <typeparam name="T"></typeparam>
public unsafe interface IRangeAccessor<T> : IEnumerable<T> where T : struct
{
    void* Data { get; }
    int Count { get; }

    ref T this[int index] { get; }
}
