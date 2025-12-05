using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Interfaces;

/// <summary>
/// Used to wrap ImVectors for vectors of regular structures<br/>
/// Supports <see cref="IEnumerable"/> and indexing.<br/>
/// <br/>
/// Essentially this is a wrapper around <b>ImVector&lt;T&gt;</b>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IImVectorWrapper<T> : IReadOnlyList<T>
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

    /// <summary>
    /// Size of one element/struct, used to seek between elements in the vector.
    /// </summary>
    public int Stride { get; }

    public Func<nint, T> Wrapper { get; }
}
