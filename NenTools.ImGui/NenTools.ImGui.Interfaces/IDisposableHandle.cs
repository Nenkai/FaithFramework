using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Interfaces;

/// <summary>
/// Represents a handle of a structure that was allocated which will be freed on disposal.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDisposableHandle<T> : IDisposable
{
    public T Value { get; }
}
