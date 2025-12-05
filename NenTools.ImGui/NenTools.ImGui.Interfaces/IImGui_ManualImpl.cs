// This file was generated with ImGuiInterfaceGenerator
using System;
using System.Runtime.InteropServices;
using System.Numerics;

namespace NenTools.ImGui.Interfaces;

///<summary>
///Interface to the ImGui library.<br/>
///</summary>
public unsafe partial interface IImGui
{
    public ulong ImTextureRef_GetTexID(IImTextureRef self);
    public IImTextureRef CreateTextureRef(ulong texId);

    /// <summary>
    /// This is needed as AddFontFromFileTTF has sanity checks (and will assert/error if some properties are off for a default structure) <br/>
    /// Refer to ImFontConfig constructor - https://github.com/ocornut/imgui/blob/842837e35b421a4c85ca30f6840321f0a3c5a029/imgui_draw.cpp#L2404
    /// </summary>
    /// <returns></returns>
    public IDisposableHandle<IImFontConfig> CreateFontConfig();

    public void ImGuiTextFilter_ImGuiTextRange_split(IImGuiTextFilter_ImGuiTextRange self, sbyte separator, out IImVectorWrapper<IImGuiTextFilter_ImGuiTextRange> @out);

    ///<summary>
    /// Output new ranges (ImVector_Construct()/ImVector_Destruct() can be used to safely construct out_ranges)<br/>
    ///</summary>
    public void ImFontGlyphRangesBuilder_BuildRanges(IImFontGlyphRangesBuilder self, out IImVectorWrapper<uint> out_ranges);
}