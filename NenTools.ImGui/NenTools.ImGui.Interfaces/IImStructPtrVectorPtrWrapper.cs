using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Interfaces;

public interface IImStructPtrVectorPtrWrapper<T> : IEnumerable<T>
{
    public nint NativePointer { get; }

    /// <summary>
    /// Returns an element by index from this vector.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IImStructPtrVectorWrapper<T> this[int index] { get; }
}
