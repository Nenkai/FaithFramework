using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell.Interfaces;

namespace NenTools.ImGui.Shell;

public class ImGuiSeparator : IImGuiComponent
{
    public bool IsOverlay => false;
    private readonly string? _name;

    private readonly IImGui _imGui;
    public ImGuiSeparator(IImGui imGui, string? name)
    {
        _imGui = imGui;
        _name = name;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (!string.IsNullOrEmpty(_name))
            _imGui.SeparatorText(_name);
        else
            _imGui.Separator();
    }

    public void Render(IImGuiShell imGuiShell)
    {
        
    }
}