using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Abstractions;

namespace FF16Framework.ImGuiManager.Windows.Framework;

public class DocumentationComponent : IImGuiComponent
{
    public bool IsOverlay => false;

    private readonly IImGui _imGui;

    public DocumentationComponent(IImGui imGui)
    {
        _imGui = imGui;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        _imGui.SeparatorText("📖 Documentation"u8);
        _imGui.TextLinkOpenURLEx("ImGui API"u8, "https://nenkai.github.io/ffxvi-modding/modding/framework/imgui_api/"u8);
        _imGui.TextLinkOpenURLEx("Nex API"u8, "https://nenkai.github.io/ffxvi-modding/modding/framework/nex_api/"u8);
    }

    public void Render(IImGuiShell imGuiShell)
    {

    }
}
