using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Interfaces.ImGuiManager;

/// <summary>
/// Represents a ImGui image.<br/>
/// Ensure to dispose this when unloading menus/windows.
/// </summary>
public interface IImGuiImage : IDisposable
{
    /// <summary>
    /// Texture Id, intended to be passed to ImGui.
    /// </summary>
    public ulong TexId { get; set; }

    /// <summary>
    /// Original width of the texture.
    /// </summary>
    public uint Width { get; set; }

    /// <summary>
    /// Original height of the texture.
    /// </summary>
    public uint Height { get; set; }
}
