using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Interfaces;

/// <summary>
/// Used to wrap ImVectors for vectors of structure pointers<br/>
/// Supports <see cref="IEnumerable"/> and indexing.<br/>
/// <br/>
/// Essentially this is a wrapper around <b>ImVector&lt;T*&gt;</b>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IImStructPtrVectorWrapper<T> : IReadOnlyList<T>
{
    /// <summary>
    /// Size/Length of the vector.
    /// </summary>
    public int Size { get; }

    /// <summary>
    /// Capacity for this vector.
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// Pointer to the first element/data for this vector.
    /// </summary>
    public nint Data { get; }
}
