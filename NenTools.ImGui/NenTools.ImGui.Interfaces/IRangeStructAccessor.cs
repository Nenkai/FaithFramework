using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Interfaces;

/// <summary>
/// Used to wrap custom structs for interfacing.
/// </summary>
/// <typeparam name="T"></typeparam>
public unsafe interface IRangeStructAccessor<T> : IReadOnlyList<T>
{
    public void* Data { get; }
    public int Stride { get; }
}
