using FF16Framework.Interfaces.ImGui;
using FF16Framework.Interfaces.ImGuiManager;
using FF16Framework.Native.ImGui;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGuiManager;

public class ImGuiSeparator : IImGuiComponent
{
    public bool IsOverlay => false;
    private readonly string? _name;

    public ImGuiSeparator(string? name)
    {
        _name = name;
    }

    public void RenderMenu(IImGuiSupport imGuiSupport, IImGui imGui)
    {
        if (!string.IsNullOrEmpty(_name))
            imGui.SeparatorText(_name);
        else
            imGui.Separator();
    }

    public void Render(IImGuiSupport imGuiSupport, IImGui imGui)
    {
        
    }
}