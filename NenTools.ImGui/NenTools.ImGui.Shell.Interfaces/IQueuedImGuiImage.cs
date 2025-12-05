using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Shell.Interfaces;

/// <summary>
/// Represents a ImGui image queued for load.<br/>
/// Ensure to dispose this when unloading menus/windows.
/// </summary>
public interface IQueuedImGuiImage : IDisposable
{
    /// <summary>
    /// Whether the image has been loaded.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Image))]
    public bool IsLoaded { get; set; }

    /// <summary>
    /// Image instance when loaded.
    /// </summary>
    public IImGuiImage? Image { get; set; }
}
