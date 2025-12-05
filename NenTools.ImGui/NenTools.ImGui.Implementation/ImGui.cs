// This file was generated with ImGuiInterfaceGenerator
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Native;

namespace NenTools.ImGui.Implementation;
public unsafe partial class ImGui : IImGui
{
    public IImGuiContext CreateContext(IImFontAtlas shared_font_atlas)
    {
        var ret = ImGuiMethods.CreateContext(shared_font_atlas is not null ? (ImFontAtlasStruct*)shared_font_atlas.NativePointer : null);
        if (ret is null)
        return null !;
        return new ImGuiContext(ret);
    }

    public void DestroyContext(IImGuiContext ctx) => ImGuiMethods.DestroyContext(ctx is not null ? (ImGuiContextStruct*)ctx.NativePointer : null);
    public IImGuiContext GetCurrentContext()
    {
        var ret = ImGuiMethods.GetCurrentContext();
        if (ret is null)
        return null !;
        return new ImGuiContext(ret);
    }

    public void SetCurrentContext(IImGuiContext ctx) => ImGuiMethods.SetCurrentContext(ctx is not null ? (ImGuiContextStruct*)ctx.NativePointer : null);
    public IImGuiIO GetIO()
    {
        var ret = ImGuiMethods.GetIO();
        if (ret is null)
        return null !;
        return new ImGuiIO(ret);
    }

    public IImGuiPlatformIO GetPlatformIO()
    {
        var ret = ImGuiMethods.GetPlatformIO();
        if (ret is null)
        return null !;
        return new ImGuiPlatformIO(ret);
    }

    public IImGuiStyle GetStyle()
    {
        var ret = ImGuiMethods.GetStyle();
        if (ret is null)
        return null !;
        return new ImGuiStyle(ret);
    }

    public void NewFrame() => ImGuiMethods.NewFrame();
    public void EndFrame() => ImGuiMethods.EndFrame();
    public void Render() => ImGuiMethods.Render();
    public IImDrawData GetDrawData()
    {
        var ret = ImGuiMethods.GetDrawData();
        if (ret is null)
        return null !;
        return new ImDrawData(ret);
    }

    public void ShowDemoWindow(ref bool p_open) => ImGuiMethods.ShowDemoWindow(ref p_open);
    public void ShowMetricsWindow(ref bool p_open) => ImGuiMethods.ShowMetricsWindow(ref p_open);
    public void ShowDebugLogWindow(ref bool p_open) => ImGuiMethods.ShowDebugLogWindow(ref p_open);
    public void ShowIDStackToolWindow() => ImGuiMethods.ShowIDStackToolWindow();
    public void ShowIDStackToolWindowEx(ref bool p_open) => ImGuiMethods.ShowIDStackToolWindowEx(ref p_open);
    public void ShowAboutWindow(ref bool p_open) => ImGuiMethods.ShowAboutWindow(ref p_open);
    public void ShowStyleEditor(IImGuiStyle @ref) => ImGuiMethods.ShowStyleEditor(@ref is not null ? (ImGuiStyleStruct*)@ref.NativePointer : null);
    public bool ShowStyleSelector(string label) => ImGuiMethods.ShowStyleSelector(label);
    public void ShowFontSelector(string label) => ImGuiMethods.ShowFontSelector(label);
    public void ShowUserGuide() => ImGuiMethods.ShowUserGuide();
    public string GetVersion()
    {
        sbyte* retStrPtr = ImGuiMethods.GetVersion();
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public void StyleColorsDark(IImGuiStyle dst) => ImGuiMethods.StyleColorsDark(dst is not null ? (ImGuiStyleStruct*)dst.NativePointer : null);
    public void StyleColorsLight(IImGuiStyle dst) => ImGuiMethods.StyleColorsLight(dst is not null ? (ImGuiStyleStruct*)dst.NativePointer : null);
    public void StyleColorsClassic(IImGuiStyle dst) => ImGuiMethods.StyleColorsClassic(dst is not null ? (ImGuiStyleStruct*)dst.NativePointer : null);
    public bool Begin(string name, ref bool p_open, ImGuiWindowFlags flags) => ImGuiMethods.Begin(name, ref p_open, (int)flags);
    public void End() => ImGuiMethods.End();
    public bool BeginChild(string str_id, Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags) => ImGuiMethods.BeginChild(str_id, size, (int)child_flags, (int)window_flags);
    public bool BeginChildID(uint id, Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags) => ImGuiMethods.BeginChildID(id, size, (int)child_flags, (int)window_flags);
    public void EndChild() => ImGuiMethods.EndChild();
    public bool IsWindowAppearing() => ImGuiMethods.IsWindowAppearing();
    public bool IsWindowCollapsed() => ImGuiMethods.IsWindowCollapsed();
    public bool IsWindowFocused(ImGuiFocusedFlags flags) => ImGuiMethods.IsWindowFocused((int)flags);
    public bool IsWindowHovered(ImGuiHoveredFlags flags) => ImGuiMethods.IsWindowHovered((int)flags);
    public IImDrawList GetWindowDrawList()
    {
        var ret = ImGuiMethods.GetWindowDrawList();
        if (ret is null)
        return null !;
        return new ImDrawList(ret);
    }

    public float GetWindowDpiScale() => ImGuiMethods.GetWindowDpiScale();
    public Vector2 GetWindowPos() => ImGuiMethods.GetWindowPos();
    public Vector2 GetWindowSize() => ImGuiMethods.GetWindowSize();
    public float GetWindowWidth() => ImGuiMethods.GetWindowWidth();
    public float GetWindowHeight() => ImGuiMethods.GetWindowHeight();
    public IImGuiViewport GetWindowViewport()
    {
        var ret = ImGuiMethods.GetWindowViewport();
        if (ret is null)
        return null !;
        return new ImGuiViewport(ret);
    }

    public void SetNextWindowPos(Vector2 pos, ImGuiCond cond) => ImGuiMethods.SetNextWindowPos(pos, (int)cond);
    public void SetNextWindowPosEx(Vector2 pos, ImGuiCond cond, Vector2 pivot) => ImGuiMethods.SetNextWindowPosEx(pos, (int)cond, pivot);
    public void SetNextWindowSize(Vector2 size, ImGuiCond cond) => ImGuiMethods.SetNextWindowSize(size, (int)cond);
    public void SetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, delegate* unmanaged[Cdecl]<nint, void> custom_callback, void* custom_callback_data) => ImGuiMethods.SetNextWindowSizeConstraints(size_min, size_max, custom_callback, custom_callback_data);
    public void SetNextWindowContentSize(Vector2 size) => ImGuiMethods.SetNextWindowContentSize(size);
    public void SetNextWindowCollapsed(bool collapsed, ImGuiCond cond) => ImGuiMethods.SetNextWindowCollapsed(collapsed, (int)cond);
    public void SetNextWindowFocus() => ImGuiMethods.SetNextWindowFocus();
    public void SetNextWindowScroll(Vector2 scroll) => ImGuiMethods.SetNextWindowScroll(scroll);
    public void SetNextWindowBgAlpha(float alpha) => ImGuiMethods.SetNextWindowBgAlpha(alpha);
    public void SetNextWindowViewport(uint viewport_id) => ImGuiMethods.SetNextWindowViewport(viewport_id);
    public void SetWindowPos(Vector2 pos, ImGuiCond cond) => ImGuiMethods.SetWindowPos(pos, (int)cond);
    public void SetWindowSize(Vector2 size, ImGuiCond cond) => ImGuiMethods.SetWindowSize(size, (int)cond);
    public void SetWindowCollapsed(bool collapsed, ImGuiCond cond) => ImGuiMethods.SetWindowCollapsed(collapsed, (int)cond);
    public void SetWindowFocus() => ImGuiMethods.SetWindowFocus();
    public void SetWindowPosStr(string name, Vector2 pos, ImGuiCond cond) => ImGuiMethods.SetWindowPosStr(name, pos, (int)cond);
    public void SetWindowSizeStr(string name, Vector2 size, ImGuiCond cond) => ImGuiMethods.SetWindowSizeStr(name, size, (int)cond);
    public void SetWindowCollapsedStr(string name, bool collapsed, ImGuiCond cond) => ImGuiMethods.SetWindowCollapsedStr(name, collapsed, (int)cond);
    public void SetWindowFocusStr(string name) => ImGuiMethods.SetWindowFocusStr(name);
    public float GetScrollX() => ImGuiMethods.GetScrollX();
    public float GetScrollY() => ImGuiMethods.GetScrollY();
    public void SetScrollX(float scroll_x) => ImGuiMethods.SetScrollX(scroll_x);
    public void SetScrollY(float scroll_y) => ImGuiMethods.SetScrollY(scroll_y);
    public float GetScrollMaxX() => ImGuiMethods.GetScrollMaxX();
    public float GetScrollMaxY() => ImGuiMethods.GetScrollMaxY();
    public void SetScrollHereX(float center_x_ratio) => ImGuiMethods.SetScrollHereX(center_x_ratio);
    public void SetScrollHereY(float center_y_ratio) => ImGuiMethods.SetScrollHereY(center_y_ratio);
    public void SetScrollFromPosX(float local_x, float center_x_ratio) => ImGuiMethods.SetScrollFromPosX(local_x, center_x_ratio);
    public void SetScrollFromPosY(float local_y, float center_y_ratio) => ImGuiMethods.SetScrollFromPosY(local_y, center_y_ratio);
    public void PushFontFloat(IImFont font, float font_size_base_unscaled) => ImGuiMethods.PushFontFloat(font is not null ? (ImFontStruct*)font.NativePointer : null, font_size_base_unscaled);
    public void PopFont() => ImGuiMethods.PopFont();
    public IImFont GetFont()
    {
        var ret = ImGuiMethods.GetFont();
        if (ret is null)
        return null !;
        return new ImFont(ret);
    }

    public float GetFontSize() => ImGuiMethods.GetFontSize();
    public IImFontBaked GetFontBaked()
    {
        var ret = ImGuiMethods.GetFontBaked();
        if (ret is null)
        return null !;
        return new ImFontBaked(ret);
    }

    public void PushStyleColor(ImGuiCol idx, uint col) => ImGuiMethods.PushStyleColor((int)idx, col);
    public void PushStyleColorImVec4(ImGuiCol idx, Vector4 col) => ImGuiMethods.PushStyleColorImVec4((int)idx, col);
    public void PopStyleColor() => ImGuiMethods.PopStyleColor();
    public void PopStyleColorEx(int count) => ImGuiMethods.PopStyleColorEx(count);
    public void PushStyleVar(ImGuiStyleVar idx, float val) => ImGuiMethods.PushStyleVar((int)idx, val);
    public void PushStyleVarImVec2(ImGuiStyleVar idx, Vector2 val) => ImGuiMethods.PushStyleVarImVec2((int)idx, val);
    public void PushStyleVarX(ImGuiStyleVar idx, float val_x) => ImGuiMethods.PushStyleVarX((int)idx, val_x);
    public void PushStyleVarY(ImGuiStyleVar idx, float val_y) => ImGuiMethods.PushStyleVarY((int)idx, val_y);
    public void PopStyleVar() => ImGuiMethods.PopStyleVar();
    public void PopStyleVarEx(int count) => ImGuiMethods.PopStyleVarEx(count);
    public void PushItemFlag(ImGuiItemFlags option, bool enabled) => ImGuiMethods.PushItemFlag((int)option, enabled);
    public void PopItemFlag() => ImGuiMethods.PopItemFlag();
    public void PushItemWidth(float item_width) => ImGuiMethods.PushItemWidth(item_width);
    public void PopItemWidth() => ImGuiMethods.PopItemWidth();
    public void SetNextItemWidth(float item_width) => ImGuiMethods.SetNextItemWidth(item_width);
    public float CalcItemWidth() => ImGuiMethods.CalcItemWidth();
    public void PushTextWrapPos(float wrap_local_pos_x) => ImGuiMethods.PushTextWrapPos(wrap_local_pos_x);
    public void PopTextWrapPos() => ImGuiMethods.PopTextWrapPos();
    public Vector2 GetFontTexUvWhitePixel() => ImGuiMethods.GetFontTexUvWhitePixel();
    public uint GetColorU32(ImGuiCol idx) => ImGuiMethods.GetColorU32((int)idx);
    public uint GetColorU32Ex(ImGuiCol idx, float alpha_mul) => ImGuiMethods.GetColorU32Ex((int)idx, alpha_mul);
    public uint GetColorU32ImVec4(Vector4 col) => ImGuiMethods.GetColorU32ImVec4(col);
    public uint GetColorU32ImU32(uint col) => ImGuiMethods.GetColorU32ImU32(col);
    public uint GetColorU32ImU32Ex(uint col, float alpha_mul) => ImGuiMethods.GetColorU32ImU32Ex(col, alpha_mul);
    public Vector4 GetStyleColorVec4(ImGuiCol idx) => ImGuiMethods.GetStyleColorVec4((int)idx);
    public Vector2 GetCursorScreenPos() => ImGuiMethods.GetCursorScreenPos();
    public void SetCursorScreenPos(Vector2 pos) => ImGuiMethods.SetCursorScreenPos(pos);
    public Vector2 GetContentRegionAvail() => ImGuiMethods.GetContentRegionAvail();
    public Vector2 GetCursorPos() => ImGuiMethods.GetCursorPos();
    public float GetCursorPosX() => ImGuiMethods.GetCursorPosX();
    public float GetCursorPosY() => ImGuiMethods.GetCursorPosY();
    public void SetCursorPos(Vector2 local_pos) => ImGuiMethods.SetCursorPos(local_pos);
    public void SetCursorPosX(float local_x) => ImGuiMethods.SetCursorPosX(local_x);
    public void SetCursorPosY(float local_y) => ImGuiMethods.SetCursorPosY(local_y);
    public Vector2 GetCursorStartPos() => ImGuiMethods.GetCursorStartPos();
    public void Separator() => ImGuiMethods.Separator();
    public void SameLine() => ImGuiMethods.SameLine();
    public void SameLineEx(float offset_from_start_x, float spacing) => ImGuiMethods.SameLineEx(offset_from_start_x, spacing);
    public void NewLine() => ImGuiMethods.NewLine();
    public void Spacing() => ImGuiMethods.Spacing();
    public void Dummy(Vector2 size) => ImGuiMethods.Dummy(size);
    public void Indent() => ImGuiMethods.Indent();
    public void IndentEx(float indent_w) => ImGuiMethods.IndentEx(indent_w);
    public void Unindent() => ImGuiMethods.Unindent();
    public void UnindentEx(float indent_w) => ImGuiMethods.UnindentEx(indent_w);
    public void BeginGroup() => ImGuiMethods.BeginGroup();
    public void EndGroup() => ImGuiMethods.EndGroup();
    public void AlignTextToFramePadding() => ImGuiMethods.AlignTextToFramePadding();
    public float GetTextLineHeight() => ImGuiMethods.GetTextLineHeight();
    public float GetTextLineHeightWithSpacing() => ImGuiMethods.GetTextLineHeightWithSpacing();
    public float GetFrameHeight() => ImGuiMethods.GetFrameHeight();
    public float GetFrameHeightWithSpacing() => ImGuiMethods.GetFrameHeightWithSpacing();
    public void PushID(string str_id) => ImGuiMethods.PushID(str_id);
    public void PushIDStr(string str_id_begin, string str_id_end) => ImGuiMethods.PushIDStr(str_id_begin, str_id_end);
    public void PushIDPtr(void* ptr_id) => ImGuiMethods.PushIDPtr(ptr_id);
    public void PushIDInt(int int_id) => ImGuiMethods.PushIDInt(int_id);
    public void PopID() => ImGuiMethods.PopID();
    public uint GetID(string str_id) => ImGuiMethods.GetID(str_id);
    public uint GetIDStr(string str_id_begin, string str_id_end) => ImGuiMethods.GetIDStr(str_id_begin, str_id_end);
    public uint GetIDPtr(void* ptr_id) => ImGuiMethods.GetIDPtr(ptr_id);
    public uint GetIDInt(int int_id) => ImGuiMethods.GetIDInt(int_id);
    public void TextUnformatted(string text) => ImGuiMethods.TextUnformatted(text);
    public void TextUnformattedEx(string text, string text_end) => ImGuiMethods.TextUnformattedEx(text, text_end);
    public void Text(string fmt) => ImGuiMethods.Text(fmt);
    public void TextV(string fmt, sbyte* args) => ImGuiMethods.TextV(fmt, args);
    public void TextColored(Vector4 col, string fmt) => ImGuiMethods.TextColored(col, fmt);
    public void TextColoredV(Vector4 col, string fmt, sbyte* args) => ImGuiMethods.TextColoredV(col, fmt, args);
    public void TextDisabled(string fmt) => ImGuiMethods.TextDisabled(fmt);
    public void TextDisabledV(string fmt, sbyte* args) => ImGuiMethods.TextDisabledV(fmt, args);
    public void TextWrapped(string fmt) => ImGuiMethods.TextWrapped(fmt);
    public void TextWrappedV(string fmt, sbyte* args) => ImGuiMethods.TextWrappedV(fmt, args);
    public void LabelText(string label, string fmt) => ImGuiMethods.LabelText(label, fmt);
    public void LabelTextV(string label, string fmt, sbyte* args) => ImGuiMethods.LabelTextV(label, fmt, args);
    public void BulletText(string fmt) => ImGuiMethods.BulletText(fmt);
    public void BulletTextV(string fmt, sbyte* args) => ImGuiMethods.BulletTextV(fmt, args);
    public void SeparatorText(string label) => ImGuiMethods.SeparatorText(label);
    public bool Button(string label) => ImGuiMethods.Button(label);
    public bool ButtonEx(string label, Vector2 size) => ImGuiMethods.ButtonEx(label, size);
    public bool SmallButton(string label) => ImGuiMethods.SmallButton(label);
    public bool InvisibleButton(string str_id, Vector2 size, ImGuiButtonFlags flags) => ImGuiMethods.InvisibleButton(str_id, size, (int)flags);
    public bool ArrowButton(string str_id, int dir) => ImGuiMethods.ArrowButton(str_id, dir);
    public bool Checkbox(string label, ref bool v) => ImGuiMethods.Checkbox(label, ref v);
    public bool CheckboxFlagsIntPtr(string label, ref int flags, int flags_value) => ImGuiMethods.CheckboxFlagsIntPtr(label, ref flags, flags_value);
    public bool CheckboxFlagsUintPtr(string label, ref uint flags, uint flags_value) => ImGuiMethods.CheckboxFlagsUintPtr(label, ref flags, flags_value);
    public bool RadioButton(string label, bool active) => ImGuiMethods.RadioButton(label, active);
    public bool RadioButtonIntPtr(string label, ref int v, int v_button) => ImGuiMethods.RadioButtonIntPtr(label, ref v, v_button);
    public void ProgressBar(float fraction, Vector2 size_arg, string overlay) => ImGuiMethods.ProgressBar(fraction, size_arg, overlay);
    public void Bullet() => ImGuiMethods.Bullet();
    public bool TextLink(string label) => ImGuiMethods.TextLink(label);
    public bool TextLinkOpenURL(string label) => ImGuiMethods.TextLinkOpenURL(label);
    public bool TextLinkOpenURLEx(string label, string url) => ImGuiMethods.TextLinkOpenURLEx(label, url);
    public void Image(IImTextureRef tex_ref, Vector2 image_size) => ImGuiMethods.Image(((ImTextureRef)tex_ref).ToStruct(), image_size);
    public void ImageEx(IImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1) => ImGuiMethods.ImageEx(((ImTextureRef)tex_ref).ToStruct(), image_size, uv0, uv1);
    public void ImageWithBg(IImTextureRef tex_ref, Vector2 image_size) => ImGuiMethods.ImageWithBg(((ImTextureRef)tex_ref).ToStruct(), image_size);
    public void ImageWithBgEx(IImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1, Vector4 bg_col, Vector4 tint_col) => ImGuiMethods.ImageWithBgEx(((ImTextureRef)tex_ref).ToStruct(), image_size, uv0, uv1, bg_col, tint_col);
    public bool ImageButton(string str_id, IImTextureRef tex_ref, Vector2 image_size) => ImGuiMethods.ImageButton(str_id, ((ImTextureRef)tex_ref).ToStruct(), image_size);
    public bool ImageButtonEx(string str_id, IImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1, Vector4 bg_col, Vector4 tint_col) => ImGuiMethods.ImageButtonEx(str_id, ((ImTextureRef)tex_ref).ToStruct(), image_size, uv0, uv1, bg_col, tint_col);
    public bool BeginCombo(string label, string preview_value, ImGuiComboFlags flags) => ImGuiMethods.BeginCombo(label, preview_value, (int)flags);
    public void EndCombo() => ImGuiMethods.EndCombo();
    public bool ComboChar(string label, ref int current_item, sbyte** items, int items_count) => ImGuiMethods.ComboChar(label, ref current_item, items, items_count);
    public bool ComboCharEx(string label, ref int current_item, sbyte** items, int items_count, int popup_max_height_in_items) => ImGuiMethods.ComboCharEx(label, ref current_item, items, items_count, popup_max_height_in_items);
    public bool Combo(string label, ref int current_item, string items_separated_by_zeros) => ImGuiMethods.Combo(label, ref current_item, items_separated_by_zeros);
    public bool ComboEx(string label, ref int current_item, string items_separated_by_zeros, int popup_max_height_in_items) => ImGuiMethods.ComboEx(label, ref current_item, items_separated_by_zeros, popup_max_height_in_items);
    public bool ComboCallback(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count) => ImGuiMethods.ComboCallback(label, ref current_item, getter, user_data, items_count);
    public bool ComboCallbackEx(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count, int popup_max_height_in_items) => ImGuiMethods.ComboCallbackEx(label, ref current_item, getter, user_data, items_count, popup_max_height_in_items);
    public bool DragFloat(string label, ref float v) => ImGuiMethods.DragFloat(label, ref v);
    public bool DragFloatEx(string label, ref float v, float v_speed, float v_min, float v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragFloatEx(label, ref v, v_speed, v_min, v_max, format, (int)flags);
    public bool DragFloat2(string label, ref float v) => ImGuiMethods.DragFloat2(label, ref v);
    public bool DragFloat2Ex(string label, ref float v, float v_speed, float v_min, float v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragFloat2Ex(label, ref v, v_speed, v_min, v_max, format, (int)flags);
    public bool DragFloat3(string label, ref float v) => ImGuiMethods.DragFloat3(label, ref v);
    public bool DragFloat3Ex(string label, ref float v, float v_speed, float v_min, float v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragFloat3Ex(label, ref v, v_speed, v_min, v_max, format, (int)flags);
    public bool DragFloat4(string label, ref float v) => ImGuiMethods.DragFloat4(label, ref v);
    public bool DragFloat4Ex(string label, ref float v, float v_speed, float v_min, float v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragFloat4Ex(label, ref v, v_speed, v_min, v_max, format, (int)flags);
    public bool DragFloatRange2(string label, ref float v_current_min, ref float v_current_max) => ImGuiMethods.DragFloatRange2(label, ref v_current_min, ref v_current_max);
    public bool DragFloatRange2Ex(string label, ref float v_current_min, ref float v_current_max, float v_speed, float v_min, float v_max, string format, string format_max, ImGuiSliderFlags flags) => ImGuiMethods.DragFloatRange2Ex(label, ref v_current_min, ref v_current_max, v_speed, v_min, v_max, format, format_max, (int)flags);
    public bool DragInt(string label, ref int v) => ImGuiMethods.DragInt(label, ref v);
    public bool DragIntEx(string label, ref int v, float v_speed, int v_min, int v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragIntEx(label, ref v, v_speed, v_min, v_max, format, (int)flags);
    public bool DragInt2(string label, ref int v) => ImGuiMethods.DragInt2(label, ref v);
    public bool DragInt2Ex(string label, ref int v, float v_speed, int v_min, int v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragInt2Ex(label, ref v, v_speed, v_min, v_max, format, (int)flags);
    public bool DragInt3(string label, ref int v) => ImGuiMethods.DragInt3(label, ref v);
    public bool DragInt3Ex(string label, ref int v, float v_speed, int v_min, int v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragInt3Ex(label, ref v, v_speed, v_min, v_max, format, (int)flags);
    public bool DragInt4(string label, ref int v) => ImGuiMethods.DragInt4(label, ref v);
    public bool DragInt4Ex(string label, ref int v, float v_speed, int v_min, int v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragInt4Ex(label, ref v, v_speed, v_min, v_max, format, (int)flags);
    public bool DragIntRange2(string label, ref int v_current_min, ref int v_current_max) => ImGuiMethods.DragIntRange2(label, ref v_current_min, ref v_current_max);
    public bool DragIntRange2Ex(string label, ref int v_current_min, ref int v_current_max, float v_speed, int v_min, int v_max, string format, string format_max, ImGuiSliderFlags flags) => ImGuiMethods.DragIntRange2Ex(label, ref v_current_min, ref v_current_max, v_speed, v_min, v_max, format, format_max, (int)flags);
    public bool DragScalar(string label, ImGuiDataType data_type, void* p_data) => ImGuiMethods.DragScalar(label, (int)data_type, p_data);
    public bool DragScalarEx(string label, ImGuiDataType data_type, void* p_data, float v_speed, void* p_min, void* p_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragScalarEx(label, (int)data_type, p_data, v_speed, p_min, p_max, format, (int)flags);
    public bool DragScalarN(string label, ImGuiDataType data_type, void* p_data, int components) => ImGuiMethods.DragScalarN(label, (int)data_type, p_data, components);
    public bool DragScalarNEx(string label, ImGuiDataType data_type, void* p_data, int components, float v_speed, void* p_min, void* p_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.DragScalarNEx(label, (int)data_type, p_data, components, v_speed, p_min, p_max, format, (int)flags);
    public bool SliderFloat(string label, ref float v, float v_min, float v_max) => ImGuiMethods.SliderFloat(label, ref v, v_min, v_max);
    public bool SliderFloatEx(string label, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderFloatEx(label, ref v, v_min, v_max, format, (int)flags);
    public bool SliderFloat2(string label, ref float v, float v_min, float v_max) => ImGuiMethods.SliderFloat2(label, ref v, v_min, v_max);
    public bool SliderFloat2Ex(string label, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderFloat2Ex(label, ref v, v_min, v_max, format, (int)flags);
    public bool SliderFloat3(string label, ref float v, float v_min, float v_max) => ImGuiMethods.SliderFloat3(label, ref v, v_min, v_max);
    public bool SliderFloat3Ex(string label, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderFloat3Ex(label, ref v, v_min, v_max, format, (int)flags);
    public bool SliderFloat4(string label, ref float v, float v_min, float v_max) => ImGuiMethods.SliderFloat4(label, ref v, v_min, v_max);
    public bool SliderFloat4Ex(string label, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderFloat4Ex(label, ref v, v_min, v_max, format, (int)flags);
    public bool SliderAngle(string label, ref float v_rad) => ImGuiMethods.SliderAngle(label, ref v_rad);
    public bool SliderAngleEx(string label, ref float v_rad, float v_degrees_min, float v_degrees_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderAngleEx(label, ref v_rad, v_degrees_min, v_degrees_max, format, (int)flags);
    public bool SliderInt(string label, ref int v, int v_min, int v_max) => ImGuiMethods.SliderInt(label, ref v, v_min, v_max);
    public bool SliderIntEx(string label, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderIntEx(label, ref v, v_min, v_max, format, (int)flags);
    public bool SliderInt2(string label, ref int v, int v_min, int v_max) => ImGuiMethods.SliderInt2(label, ref v, v_min, v_max);
    public bool SliderInt2Ex(string label, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderInt2Ex(label, ref v, v_min, v_max, format, (int)flags);
    public bool SliderInt3(string label, ref int v, int v_min, int v_max) => ImGuiMethods.SliderInt3(label, ref v, v_min, v_max);
    public bool SliderInt3Ex(string label, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderInt3Ex(label, ref v, v_min, v_max, format, (int)flags);
    public bool SliderInt4(string label, ref int v, int v_min, int v_max) => ImGuiMethods.SliderInt4(label, ref v, v_min, v_max);
    public bool SliderInt4Ex(string label, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderInt4Ex(label, ref v, v_min, v_max, format, (int)flags);
    public bool SliderScalar(string label, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max) => ImGuiMethods.SliderScalar(label, (int)data_type, p_data, p_min, p_max);
    public bool SliderScalarEx(string label, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderScalarEx(label, (int)data_type, p_data, p_min, p_max, format, (int)flags);
    public bool SliderScalarN(string label, ImGuiDataType data_type, void* p_data, int components, void* p_min, void* p_max) => ImGuiMethods.SliderScalarN(label, (int)data_type, p_data, components, p_min, p_max);
    public bool SliderScalarNEx(string label, ImGuiDataType data_type, void* p_data, int components, void* p_min, void* p_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.SliderScalarNEx(label, (int)data_type, p_data, components, p_min, p_max, format, (int)flags);
    public bool VSliderFloat(string label, Vector2 size, ref float v, float v_min, float v_max) => ImGuiMethods.VSliderFloat(label, size, ref v, v_min, v_max);
    public bool VSliderFloatEx(string label, Vector2 size, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.VSliderFloatEx(label, size, ref v, v_min, v_max, format, (int)flags);
    public bool VSliderInt(string label, Vector2 size, ref int v, int v_min, int v_max) => ImGuiMethods.VSliderInt(label, size, ref v, v_min, v_max);
    public bool VSliderIntEx(string label, Vector2 size, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.VSliderIntEx(label, size, ref v, v_min, v_max, format, (int)flags);
    public bool VSliderScalar(string label, Vector2 size, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max) => ImGuiMethods.VSliderScalar(label, size, (int)data_type, p_data, p_min, p_max);
    public bool VSliderScalarEx(string label, Vector2 size, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max, string format, ImGuiSliderFlags flags) => ImGuiMethods.VSliderScalarEx(label, size, (int)data_type, p_data, p_min, p_max, format, (int)flags);
    public bool InputText(string label, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags) => ImGuiMethods.InputText(label, buf, buf_size, (int)flags);
    public bool InputTextEx(string label, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags, delegate* unmanaged[Cdecl]<nint, int> callback, void* user_data) => ImGuiMethods.InputTextEx(label, buf, buf_size, (int)flags, callback, user_data);
    public bool InputTextMultiline(string label, sbyte* buf, nuint buf_size) => ImGuiMethods.InputTextMultiline(label, buf, buf_size);
    public bool InputTextMultilineEx(string label, sbyte* buf, nuint buf_size, Vector2 size, ImGuiInputTextFlags flags, delegate* unmanaged[Cdecl]<nint, int> callback, void* user_data) => ImGuiMethods.InputTextMultilineEx(label, buf, buf_size, size, (int)flags, callback, user_data);
    public bool InputTextWithHint(string label, string hint, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags) => ImGuiMethods.InputTextWithHint(label, hint, buf, buf_size, (int)flags);
    public bool InputTextWithHintEx(string label, string hint, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags, delegate* unmanaged[Cdecl]<nint, int> callback, void* user_data) => ImGuiMethods.InputTextWithHintEx(label, hint, buf, buf_size, (int)flags, callback, user_data);
    public bool InputFloat(string label, ref float v) => ImGuiMethods.InputFloat(label, ref v);
    public bool InputFloatEx(string label, ref float v, float step, float step_fast, string format, ImGuiInputTextFlags flags) => ImGuiMethods.InputFloatEx(label, ref v, step, step_fast, format, (int)flags);
    public bool InputFloat2(string label, ref float v) => ImGuiMethods.InputFloat2(label, ref v);
    public bool InputFloat2Ex(string label, ref float v, string format, ImGuiInputTextFlags flags) => ImGuiMethods.InputFloat2Ex(label, ref v, format, (int)flags);
    public bool InputFloat3(string label, ref float v) => ImGuiMethods.InputFloat3(label, ref v);
    public bool InputFloat3Ex(string label, ref float v, string format, ImGuiInputTextFlags flags) => ImGuiMethods.InputFloat3Ex(label, ref v, format, (int)flags);
    public bool InputFloat4(string label, ref float v) => ImGuiMethods.InputFloat4(label, ref v);
    public bool InputFloat4Ex(string label, ref float v, string format, ImGuiInputTextFlags flags) => ImGuiMethods.InputFloat4Ex(label, ref v, format, (int)flags);
    public bool InputInt(string label, ref int v) => ImGuiMethods.InputInt(label, ref v);
    public bool InputIntEx(string label, ref int v, int step, int step_fast, ImGuiInputTextFlags flags) => ImGuiMethods.InputIntEx(label, ref v, step, step_fast, (int)flags);
    public bool InputInt2(string label, ref int v, ImGuiInputTextFlags flags) => ImGuiMethods.InputInt2(label, ref v, (int)flags);
    public bool InputInt3(string label, ref int v, ImGuiInputTextFlags flags) => ImGuiMethods.InputInt3(label, ref v, (int)flags);
    public bool InputInt4(string label, ref int v, ImGuiInputTextFlags flags) => ImGuiMethods.InputInt4(label, ref v, (int)flags);
    public bool InputDouble(string label, ref double v) => ImGuiMethods.InputDouble(label, ref v);
    public bool InputDoubleEx(string label, ref double v, double step, double step_fast, string format, ImGuiInputTextFlags flags) => ImGuiMethods.InputDoubleEx(label, ref v, step, step_fast, format, (int)flags);
    public bool InputScalar(string label, ImGuiDataType data_type, void* p_data) => ImGuiMethods.InputScalar(label, (int)data_type, p_data);
    public bool InputScalarEx(string label, ImGuiDataType data_type, void* p_data, void* p_step, void* p_step_fast, string format, ImGuiInputTextFlags flags) => ImGuiMethods.InputScalarEx(label, (int)data_type, p_data, p_step, p_step_fast, format, (int)flags);
    public bool InputScalarN(string label, ImGuiDataType data_type, void* p_data, int components) => ImGuiMethods.InputScalarN(label, (int)data_type, p_data, components);
    public bool InputScalarNEx(string label, ImGuiDataType data_type, void* p_data, int components, void* p_step, void* p_step_fast, string format, ImGuiInputTextFlags flags) => ImGuiMethods.InputScalarNEx(label, (int)data_type, p_data, components, p_step, p_step_fast, format, (int)flags);
    public bool ColorEdit3(string label, ref float col, ImGuiColorEditFlags flags) => ImGuiMethods.ColorEdit3(label, ref col, (int)flags);
    public bool ColorEdit4(string label, ref float col, ImGuiColorEditFlags flags) => ImGuiMethods.ColorEdit4(label, ref col, (int)flags);
    public bool ColorPicker3(string label, ref float col, ImGuiColorEditFlags flags) => ImGuiMethods.ColorPicker3(label, ref col, (int)flags);
    public bool ColorPicker4(string label, ref float col, ImGuiColorEditFlags flags, ref float ref_col) => ImGuiMethods.ColorPicker4(label, ref col, (int)flags, ref ref_col);
    public bool ColorButton(string desc_id, Vector4 col, ImGuiColorEditFlags flags) => ImGuiMethods.ColorButton(desc_id, col, (int)flags);
    public bool ColorButtonEx(string desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size) => ImGuiMethods.ColorButtonEx(desc_id, col, (int)flags, size);
    public void SetColorEditOptions(ImGuiColorEditFlags flags) => ImGuiMethods.SetColorEditOptions((int)flags);
    public bool TreeNode(string label) => ImGuiMethods.TreeNode(label);
    public bool TreeNodeStr(string str_id, string fmt) => ImGuiMethods.TreeNodeStr(str_id, fmt);
    public bool TreeNodePtr(void* ptr_id, string fmt) => ImGuiMethods.TreeNodePtr(ptr_id, fmt);
    public bool TreeNodeV(string str_id, string fmt, sbyte* args) => ImGuiMethods.TreeNodeV(str_id, fmt, args);
    public bool TreeNodeVPtr(void* ptr_id, string fmt, sbyte* args) => ImGuiMethods.TreeNodeVPtr(ptr_id, fmt, args);
    public bool TreeNodeEx(string label, ImGuiTreeNodeFlags flags) => ImGuiMethods.TreeNodeEx(label, (int)flags);
    public bool TreeNodeExStr(string str_id, ImGuiTreeNodeFlags flags, string fmt) => ImGuiMethods.TreeNodeExStr(str_id, (int)flags, fmt);
    public bool TreeNodeExPtr(void* ptr_id, ImGuiTreeNodeFlags flags, string fmt) => ImGuiMethods.TreeNodeExPtr(ptr_id, (int)flags, fmt);
    public bool TreeNodeExV(string str_id, ImGuiTreeNodeFlags flags, string fmt, sbyte* args) => ImGuiMethods.TreeNodeExV(str_id, (int)flags, fmt, args);
    public bool TreeNodeExVPtr(void* ptr_id, ImGuiTreeNodeFlags flags, string fmt, sbyte* args) => ImGuiMethods.TreeNodeExVPtr(ptr_id, (int)flags, fmt, args);
    public void TreePush(string str_id) => ImGuiMethods.TreePush(str_id);
    public void TreePushPtr(void* ptr_id) => ImGuiMethods.TreePushPtr(ptr_id);
    public void TreePop() => ImGuiMethods.TreePop();
    public float GetTreeNodeToLabelSpacing() => ImGuiMethods.GetTreeNodeToLabelSpacing();
    public bool CollapsingHeader(string label, ImGuiTreeNodeFlags flags) => ImGuiMethods.CollapsingHeader(label, (int)flags);
    public bool CollapsingHeaderBoolPtr(string label, ref bool p_visible, ImGuiTreeNodeFlags flags) => ImGuiMethods.CollapsingHeaderBoolPtr(label, ref p_visible, (int)flags);
    public void SetNextItemOpen(bool is_open, ImGuiCond cond) => ImGuiMethods.SetNextItemOpen(is_open, (int)cond);
    public void SetNextItemStorageID(uint storage_id) => ImGuiMethods.SetNextItemStorageID(storage_id);
    public bool Selectable(string label) => ImGuiMethods.Selectable(label);
    public bool SelectableEx(string label, bool selected, ImGuiSelectableFlags flags, Vector2 size) => ImGuiMethods.SelectableEx(label, selected, (int)flags, size);
    public bool SelectableBoolPtr(string label, ref bool p_selected, ImGuiSelectableFlags flags) => ImGuiMethods.SelectableBoolPtr(label, ref p_selected, (int)flags);
    public bool SelectableBoolPtrEx(string label, ref bool p_selected, ImGuiSelectableFlags flags, Vector2 size) => ImGuiMethods.SelectableBoolPtrEx(label, ref p_selected, (int)flags, size);
    public IImGuiMultiSelectIO BeginMultiSelect(ImGuiMultiSelectFlags flags)
    {
        var ret = ImGuiMethods.BeginMultiSelect((int)flags);
        if (ret is null)
        return null !;
        return new ImGuiMultiSelectIO(ret);
    }

    public IImGuiMultiSelectIO BeginMultiSelectEx(ImGuiMultiSelectFlags flags, int selection_size, int items_count)
    {
        var ret = ImGuiMethods.BeginMultiSelectEx((int)flags, selection_size, items_count);
        if (ret is null)
        return null !;
        return new ImGuiMultiSelectIO(ret);
    }

    public IImGuiMultiSelectIO EndMultiSelect()
    {
        var ret = ImGuiMethods.EndMultiSelect();
        if (ret is null)
        return null !;
        return new ImGuiMultiSelectIO(ret);
    }

    public void SetNextItemSelectionUserData(long selection_user_data) => ImGuiMethods.SetNextItemSelectionUserData(selection_user_data);
    public bool IsItemToggledSelection() => ImGuiMethods.IsItemToggledSelection();
    public bool BeginListBox(string label, Vector2 size) => ImGuiMethods.BeginListBox(label, size);
    public void EndListBox() => ImGuiMethods.EndListBox();
    public bool ListBox(string label, ref int current_item, sbyte** items, int items_count, int height_in_items) => ImGuiMethods.ListBox(label, ref current_item, items, items_count, height_in_items);
    public bool ListBoxCallback(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count) => ImGuiMethods.ListBoxCallback(label, ref current_item, getter, user_data, items_count);
    public bool ListBoxCallbackEx(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count, int height_in_items) => ImGuiMethods.ListBoxCallbackEx(label, ref current_item, getter, user_data, items_count, height_in_items);
    public void PlotLines(string label, ref float values, int values_count) => ImGuiMethods.PlotLines(label, ref values, values_count);
    public void PlotLinesEx(string label, ref float values, int values_count, int values_offset, string overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => ImGuiMethods.PlotLinesEx(label, ref values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
    public void PlotLinesCallback(string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count) => ImGuiMethods.PlotLinesCallback(label, values_getter, data, values_count);
    public void PlotLinesCallbackEx(string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count, int values_offset, string overlay_text, float scale_min, float scale_max, Vector2 graph_size) => ImGuiMethods.PlotLinesCallbackEx(label, values_getter, data, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size);
    public void PlotHistogram(string label, ref float values, int values_count) => ImGuiMethods.PlotHistogram(label, ref values, values_count);
    public void PlotHistogramEx(string label, ref float values, int values_count, int values_offset, string overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => ImGuiMethods.PlotHistogramEx(label, ref values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
    public void PlotHistogramCallback(string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count) => ImGuiMethods.PlotHistogramCallback(label, values_getter, data, values_count);
    public void PlotHistogramCallbackEx(string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count, int values_offset, string overlay_text, float scale_min, float scale_max, Vector2 graph_size) => ImGuiMethods.PlotHistogramCallbackEx(label, values_getter, data, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size);
    public bool BeginMenuBar() => ImGuiMethods.BeginMenuBar();
    public void EndMenuBar() => ImGuiMethods.EndMenuBar();
    public bool BeginMainMenuBar() => ImGuiMethods.BeginMainMenuBar();
    public void EndMainMenuBar() => ImGuiMethods.EndMainMenuBar();
    public bool BeginMenu(string label) => ImGuiMethods.BeginMenu(label);
    public bool BeginMenuEx(string label, bool enabled) => ImGuiMethods.BeginMenuEx(label, enabled);
    public void EndMenu() => ImGuiMethods.EndMenu();
    public bool MenuItem(string label) => ImGuiMethods.MenuItem(label);
    public bool MenuItemEx(string label, string shortcut, bool selected, bool enabled) => ImGuiMethods.MenuItemEx(label, shortcut, selected, enabled);
    public bool MenuItemBoolPtr(string label, string shortcut, ref bool p_selected, bool enabled) => ImGuiMethods.MenuItemBoolPtr(label, shortcut, ref p_selected, enabled);
    public bool BeginTooltip() => ImGuiMethods.BeginTooltip();
    public void EndTooltip() => ImGuiMethods.EndTooltip();
    public void SetTooltip(string fmt) => ImGuiMethods.SetTooltip(fmt);
    public void SetTooltipV(string fmt, sbyte* args) => ImGuiMethods.SetTooltipV(fmt, args);
    public bool BeginItemTooltip() => ImGuiMethods.BeginItemTooltip();
    public void SetItemTooltip(string fmt) => ImGuiMethods.SetItemTooltip(fmt);
    public void SetItemTooltipV(string fmt, sbyte* args) => ImGuiMethods.SetItemTooltipV(fmt, args);
    public bool BeginPopup(string str_id, ImGuiWindowFlags flags) => ImGuiMethods.BeginPopup(str_id, (int)flags);
    public bool BeginPopupModal(string name, ref bool p_open, ImGuiWindowFlags flags) => ImGuiMethods.BeginPopupModal(name, ref p_open, (int)flags);
    public void EndPopup() => ImGuiMethods.EndPopup();
    public void OpenPopup(string str_id, ImGuiPopupFlags popup_flags) => ImGuiMethods.OpenPopup(str_id, (int)popup_flags);
    public void OpenPopupID(uint id, ImGuiPopupFlags popup_flags) => ImGuiMethods.OpenPopupID(id, (int)popup_flags);
    public void OpenPopupOnItemClick(string str_id, ImGuiPopupFlags popup_flags) => ImGuiMethods.OpenPopupOnItemClick(str_id, (int)popup_flags);
    public void CloseCurrentPopup() => ImGuiMethods.CloseCurrentPopup();
    public bool BeginPopupContextItem() => ImGuiMethods.BeginPopupContextItem();
    public bool BeginPopupContextItemEx(string str_id, ImGuiPopupFlags popup_flags) => ImGuiMethods.BeginPopupContextItemEx(str_id, (int)popup_flags);
    public bool BeginPopupContextWindow() => ImGuiMethods.BeginPopupContextWindow();
    public bool BeginPopupContextWindowEx(string str_id, ImGuiPopupFlags popup_flags) => ImGuiMethods.BeginPopupContextWindowEx(str_id, (int)popup_flags);
    public bool BeginPopupContextVoid() => ImGuiMethods.BeginPopupContextVoid();
    public bool BeginPopupContextVoidEx(string str_id, ImGuiPopupFlags popup_flags) => ImGuiMethods.BeginPopupContextVoidEx(str_id, (int)popup_flags);
    public bool IsPopupOpen(string str_id, ImGuiPopupFlags flags) => ImGuiMethods.IsPopupOpen(str_id, (int)flags);
    public bool BeginTable(string str_id, int columns, ImGuiTableFlags flags) => ImGuiMethods.BeginTable(str_id, columns, (int)flags);
    public bool BeginTableEx(string str_id, int columns, ImGuiTableFlags flags, Vector2 outer_size, float inner_width) => ImGuiMethods.BeginTableEx(str_id, columns, (int)flags, outer_size, inner_width);
    public void EndTable() => ImGuiMethods.EndTable();
    public void TableNextRow() => ImGuiMethods.TableNextRow();
    public void TableNextRowEx(ImGuiTableRowFlags row_flags, float min_row_height) => ImGuiMethods.TableNextRowEx((int)row_flags, min_row_height);
    public bool TableNextColumn() => ImGuiMethods.TableNextColumn();
    public bool TableSetColumnIndex(int column_n) => ImGuiMethods.TableSetColumnIndex(column_n);
    public void TableSetupColumn(string label, ImGuiTableColumnFlags flags) => ImGuiMethods.TableSetupColumn(label, (int)flags);
    public void TableSetupColumnEx(string label, ImGuiTableColumnFlags flags, float init_width_or_weight, uint user_id) => ImGuiMethods.TableSetupColumnEx(label, (int)flags, init_width_or_weight, user_id);
    public void TableSetupScrollFreeze(int cols, int rows) => ImGuiMethods.TableSetupScrollFreeze(cols, rows);
    public void TableHeader(string label) => ImGuiMethods.TableHeader(label);
    public void TableHeadersRow() => ImGuiMethods.TableHeadersRow();
    public void TableAngledHeadersRow() => ImGuiMethods.TableAngledHeadersRow();
    public IImGuiTableSortSpecs TableGetSortSpecs()
    {
        var ret = ImGuiMethods.TableGetSortSpecs();
        if (ret is null)
        return null !;
        return new ImGuiTableSortSpecs(ret);
    }

    public int TableGetColumnCount() => ImGuiMethods.TableGetColumnCount();
    public int TableGetColumnIndex() => ImGuiMethods.TableGetColumnIndex();
    public int TableGetRowIndex() => ImGuiMethods.TableGetRowIndex();
    public string TableGetColumnName(int column_n)
    {
        sbyte* retStrPtr = ImGuiMethods.TableGetColumnName(column_n);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public ImGuiTableColumnFlags TableGetColumnFlags(int column_n) => (ImGuiTableColumnFlags)ImGuiMethods.TableGetColumnFlags(column_n);
    public void TableSetColumnEnabled(int column_n, bool v) => ImGuiMethods.TableSetColumnEnabled(column_n, v);
    public int TableGetHoveredColumn() => ImGuiMethods.TableGetHoveredColumn();
    public void TableSetBgColor(ImGuiTableBgTarget target, uint color, int column_n) => ImGuiMethods.TableSetBgColor((int)target, color, column_n);
    public void Columns() => ImGuiMethods.Columns();
    public void ColumnsEx(int count, string id, bool borders) => ImGuiMethods.ColumnsEx(count, id, borders);
    public void NextColumn() => ImGuiMethods.NextColumn();
    public int GetColumnIndex() => ImGuiMethods.GetColumnIndex();
    public float GetColumnWidth(int column_index) => ImGuiMethods.GetColumnWidth(column_index);
    public void SetColumnWidth(int column_index, float width) => ImGuiMethods.SetColumnWidth(column_index, width);
    public float GetColumnOffset(int column_index) => ImGuiMethods.GetColumnOffset(column_index);
    public void SetColumnOffset(int column_index, float offset_x) => ImGuiMethods.SetColumnOffset(column_index, offset_x);
    public int GetColumnsCount() => ImGuiMethods.GetColumnsCount();
    public bool BeginTabBar(string str_id, ImGuiTabBarFlags flags) => ImGuiMethods.BeginTabBar(str_id, (int)flags);
    public void EndTabBar() => ImGuiMethods.EndTabBar();
    public bool BeginTabItem(string label, ref bool p_open, ImGuiTabItemFlags flags) => ImGuiMethods.BeginTabItem(label, ref p_open, (int)flags);
    public void EndTabItem() => ImGuiMethods.EndTabItem();
    public bool TabItemButton(string label, ImGuiTabItemFlags flags) => ImGuiMethods.TabItemButton(label, (int)flags);
    public void SetTabItemClosed(string tab_or_docked_window_label) => ImGuiMethods.SetTabItemClosed(tab_or_docked_window_label);
    public uint DockSpace(uint dockspace_id) => ImGuiMethods.DockSpace(dockspace_id);
    public uint DockSpaceEx(uint dockspace_id, Vector2 size, ImGuiDockNodeFlags flags, IImGuiWindowClass window_class) => ImGuiMethods.DockSpaceEx(dockspace_id, size, (int)flags, window_class is not null ? (ImGuiWindowClassStruct*)window_class.NativePointer : null);
    public uint DockSpaceOverViewport() => ImGuiMethods.DockSpaceOverViewport();
    public uint DockSpaceOverViewportEx(uint dockspace_id, IImGuiViewport viewport, ImGuiDockNodeFlags flags, IImGuiWindowClass window_class) => ImGuiMethods.DockSpaceOverViewportEx(dockspace_id, viewport is not null ? (ImGuiViewportStruct*)viewport.NativePointer : null, (int)flags, window_class is not null ? (ImGuiWindowClassStruct*)window_class.NativePointer : null);
    public void SetNextWindowDockID(uint dock_id, ImGuiCond cond) => ImGuiMethods.SetNextWindowDockID(dock_id, (int)cond);
    public void SetNextWindowClass(IImGuiWindowClass window_class) => ImGuiMethods.SetNextWindowClass(window_class is not null ? (ImGuiWindowClassStruct*)window_class.NativePointer : null);
    public uint GetWindowDockID() => ImGuiMethods.GetWindowDockID();
    public bool IsWindowDocked() => ImGuiMethods.IsWindowDocked();
    public void LogToTTY(int auto_open_depth) => ImGuiMethods.LogToTTY(auto_open_depth);
    public void LogToFile(int auto_open_depth, string filename) => ImGuiMethods.LogToFile(auto_open_depth, filename);
    public void LogToClipboard(int auto_open_depth) => ImGuiMethods.LogToClipboard(auto_open_depth);
    public void LogFinish() => ImGuiMethods.LogFinish();
    public void LogButtons() => ImGuiMethods.LogButtons();
    public void LogText(string fmt) => ImGuiMethods.LogText(fmt);
    public void LogTextV(string fmt, sbyte* args) => ImGuiMethods.LogTextV(fmt, args);
    public bool BeginDragDropSource(ImGuiDragDropFlags flags) => ImGuiMethods.BeginDragDropSource((int)flags);
    public bool SetDragDropPayload(string type, void* data, nuint sz, ImGuiCond cond) => ImGuiMethods.SetDragDropPayload(type, data, sz, (int)cond);
    public void EndDragDropSource() => ImGuiMethods.EndDragDropSource();
    public bool BeginDragDropTarget() => ImGuiMethods.BeginDragDropTarget();
    public IImGuiPayload AcceptDragDropPayload(string type, ImGuiDragDropFlags flags)
    {
        var ret = ImGuiMethods.AcceptDragDropPayload(type, (int)flags);
        if (ret is null)
        return null !;
        return new ImGuiPayload(ret);
    }

    public void EndDragDropTarget() => ImGuiMethods.EndDragDropTarget();
    public IImGuiPayload GetDragDropPayload()
    {
        var ret = ImGuiMethods.GetDragDropPayload();
        if (ret is null)
        return null !;
        return new ImGuiPayload(ret);
    }

    public void BeginDisabled(bool disabled) => ImGuiMethods.BeginDisabled(disabled);
    public void EndDisabled() => ImGuiMethods.EndDisabled();
    public void PushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, bool intersect_with_current_clip_rect) => ImGuiMethods.PushClipRect(clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
    public void PopClipRect() => ImGuiMethods.PopClipRect();
    public void SetItemDefaultFocus() => ImGuiMethods.SetItemDefaultFocus();
    public void SetKeyboardFocusHere() => ImGuiMethods.SetKeyboardFocusHere();
    public void SetKeyboardFocusHereEx(int offset) => ImGuiMethods.SetKeyboardFocusHereEx(offset);
    public void SetNavCursorVisible(bool visible) => ImGuiMethods.SetNavCursorVisible(visible);
    public void SetNextItemAllowOverlap() => ImGuiMethods.SetNextItemAllowOverlap();
    public bool IsItemHovered(ImGuiHoveredFlags flags) => ImGuiMethods.IsItemHovered((int)flags);
    public bool IsItemActive() => ImGuiMethods.IsItemActive();
    public bool IsItemFocused() => ImGuiMethods.IsItemFocused();
    public bool IsItemClicked() => ImGuiMethods.IsItemClicked();
    public bool IsItemClickedEx(ImGuiMouseButton mouse_button) => ImGuiMethods.IsItemClickedEx((int)mouse_button);
    public bool IsItemVisible() => ImGuiMethods.IsItemVisible();
    public bool IsItemEdited() => ImGuiMethods.IsItemEdited();
    public bool IsItemActivated() => ImGuiMethods.IsItemActivated();
    public bool IsItemDeactivated() => ImGuiMethods.IsItemDeactivated();
    public bool IsItemDeactivatedAfterEdit() => ImGuiMethods.IsItemDeactivatedAfterEdit();
    public bool IsItemToggledOpen() => ImGuiMethods.IsItemToggledOpen();
    public bool IsAnyItemHovered() => ImGuiMethods.IsAnyItemHovered();
    public bool IsAnyItemActive() => ImGuiMethods.IsAnyItemActive();
    public bool IsAnyItemFocused() => ImGuiMethods.IsAnyItemFocused();
    public uint GetItemID() => ImGuiMethods.GetItemID();
    public Vector2 GetItemRectMin() => ImGuiMethods.GetItemRectMin();
    public Vector2 GetItemRectMax() => ImGuiMethods.GetItemRectMax();
    public Vector2 GetItemRectSize() => ImGuiMethods.GetItemRectSize();
    public IImGuiViewport GetMainViewport()
    {
        var ret = ImGuiMethods.GetMainViewport();
        if (ret is null)
        return null !;
        return new ImGuiViewport(ret);
    }

    public IImDrawList GetBackgroundDrawList()
    {
        var ret = ImGuiMethods.GetBackgroundDrawList();
        if (ret is null)
        return null !;
        return new ImDrawList(ret);
    }

    public IImDrawList GetBackgroundDrawListEx(IImGuiViewport viewport)
    {
        var ret = ImGuiMethods.GetBackgroundDrawListEx(viewport is not null ? (ImGuiViewportStruct*)viewport.NativePointer : null);
        if (ret is null)
        return null !;
        return new ImDrawList(ret);
    }

    public IImDrawList GetForegroundDrawList()
    {
        var ret = ImGuiMethods.GetForegroundDrawList();
        if (ret is null)
        return null !;
        return new ImDrawList(ret);
    }

    public IImDrawList GetForegroundDrawListEx(IImGuiViewport viewport)
    {
        var ret = ImGuiMethods.GetForegroundDrawListEx(viewport is not null ? (ImGuiViewportStruct*)viewport.NativePointer : null);
        if (ret is null)
        return null !;
        return new ImDrawList(ret);
    }

    public bool IsRectVisibleBySize(Vector2 size) => ImGuiMethods.IsRectVisibleBySize(size);
    public bool IsRectVisible(Vector2 rect_min, Vector2 rect_max) => ImGuiMethods.IsRectVisible(rect_min, rect_max);
    public double GetTime() => ImGuiMethods.GetTime();
    public int GetFrameCount() => ImGuiMethods.GetFrameCount();
    public nint GetDrawListSharedData() => ImGuiMethods.GetDrawListSharedData();
    public string GetStyleColorName(ImGuiCol idx)
    {
        sbyte* retStrPtr = ImGuiMethods.GetStyleColorName((int)idx);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public void SetStateStorage(IImGuiStorage storage) => ImGuiMethods.SetStateStorage(storage is not null ? (ImGuiStorageStruct*)storage.NativePointer : null);
    public IImGuiStorage GetStateStorage()
    {
        var ret = ImGuiMethods.GetStateStorage();
        if (ret is null)
        return null !;
        return new ImGuiStorage(ret);
    }

    public Vector2 CalcTextSize(string text) => ImGuiMethods.CalcTextSize(text);
    public Vector2 CalcTextSizeEx(string text, string text_end, bool hide_text_after_double_hash, float wrap_width) => ImGuiMethods.CalcTextSizeEx(text, text_end, hide_text_after_double_hash, wrap_width);
    public Vector4 ColorConvertU32ToFloat4(uint @in) => ImGuiMethods.ColorConvertU32ToFloat4(@in);
    public uint ColorConvertFloat4ToU32(Vector4 @in) => ImGuiMethods.ColorConvertFloat4ToU32(@in);
    public void ColorConvertRGBtoHSV(float r, float g, float b, ref float out_h, ref float out_s, ref float out_v) => ImGuiMethods.ColorConvertRGBtoHSV(r, g, b, ref out_h, ref out_s, ref out_v);
    public void ColorConvertHSVtoRGB(float h, float s, float v, ref float out_r, ref float out_g, ref float out_b) => ImGuiMethods.ColorConvertHSVtoRGB(h, s, v, ref out_r, ref out_g, ref out_b);
    public bool IsKeyDown(int key) => ImGuiMethods.IsKeyDown(key);
    public bool IsKeyPressed(int key) => ImGuiMethods.IsKeyPressed(key);
    public bool IsKeyPressedEx(int key, bool repeat) => ImGuiMethods.IsKeyPressedEx(key, repeat);
    public bool IsKeyReleased(int key) => ImGuiMethods.IsKeyReleased(key);
    public bool IsKeyChordPressed(int key_chord) => ImGuiMethods.IsKeyChordPressed(key_chord);
    public int GetKeyPressedAmount(int key, float repeat_delay, float rate) => ImGuiMethods.GetKeyPressedAmount(key, repeat_delay, rate);
    public string GetKeyName(int key)
    {
        sbyte* retStrPtr = ImGuiMethods.GetKeyName(key);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public void SetNextFrameWantCaptureKeyboard(bool want_capture_keyboard) => ImGuiMethods.SetNextFrameWantCaptureKeyboard(want_capture_keyboard);
    public bool Shortcut(int key_chord, ImGuiInputFlags flags) => ImGuiMethods.Shortcut(key_chord, (int)flags);
    public void SetNextItemShortcut(int key_chord, ImGuiInputFlags flags) => ImGuiMethods.SetNextItemShortcut(key_chord, (int)flags);
    public void SetItemKeyOwner(int key) => ImGuiMethods.SetItemKeyOwner(key);
    public bool IsMouseDown(ImGuiMouseButton button) => ImGuiMethods.IsMouseDown((int)button);
    public bool IsMouseClicked(ImGuiMouseButton button) => ImGuiMethods.IsMouseClicked((int)button);
    public bool IsMouseClickedEx(ImGuiMouseButton button, bool repeat) => ImGuiMethods.IsMouseClickedEx((int)button, repeat);
    public bool IsMouseReleased(ImGuiMouseButton button) => ImGuiMethods.IsMouseReleased((int)button);
    public bool IsMouseDoubleClicked(ImGuiMouseButton button) => ImGuiMethods.IsMouseDoubleClicked((int)button);
    public bool IsMouseReleasedWithDelay(ImGuiMouseButton button, float delay) => ImGuiMethods.IsMouseReleasedWithDelay((int)button, delay);
    public int GetMouseClickedCount(ImGuiMouseButton button) => ImGuiMethods.GetMouseClickedCount((int)button);
    public bool IsMouseHoveringRect(Vector2 r_min, Vector2 r_max) => ImGuiMethods.IsMouseHoveringRect(r_min, r_max);
    public bool IsMouseHoveringRectEx(Vector2 r_min, Vector2 r_max, bool clip) => ImGuiMethods.IsMouseHoveringRectEx(r_min, r_max, clip);
    public bool IsMousePosValid(Vector2 mouse_pos) => ImGuiMethods.IsMousePosValid(mouse_pos);
    public bool IsAnyMouseDown() => ImGuiMethods.IsAnyMouseDown();
    public Vector2 GetMousePos() => ImGuiMethods.GetMousePos();
    public Vector2 GetMousePosOnOpeningCurrentPopup() => ImGuiMethods.GetMousePosOnOpeningCurrentPopup();
    public bool IsMouseDragging(ImGuiMouseButton button, float lock_threshold) => ImGuiMethods.IsMouseDragging((int)button, lock_threshold);
    public Vector2 GetMouseDragDelta(ImGuiMouseButton button, float lock_threshold) => ImGuiMethods.GetMouseDragDelta((int)button, lock_threshold);
    public void ResetMouseDragDelta() => ImGuiMethods.ResetMouseDragDelta();
    public void ResetMouseDragDeltaEx(ImGuiMouseButton button) => ImGuiMethods.ResetMouseDragDeltaEx((int)button);
    public ImGuiMouseCursor GetMouseCursor() => (ImGuiMouseCursor)ImGuiMethods.GetMouseCursor();
    public void SetMouseCursor(ImGuiMouseCursor cursor_type) => ImGuiMethods.SetMouseCursor((int)cursor_type);
    public void SetNextFrameWantCaptureMouse(bool want_capture_mouse) => ImGuiMethods.SetNextFrameWantCaptureMouse(want_capture_mouse);
    public string GetClipboardText()
    {
        sbyte* retStrPtr = ImGuiMethods.GetClipboardText();
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public void SetClipboardText(string text) => ImGuiMethods.SetClipboardText(text);
    public void LoadIniSettingsFromDisk(string ini_filename) => ImGuiMethods.LoadIniSettingsFromDisk(ini_filename);
    public void LoadIniSettingsFromMemory(string ini_data, nuint ini_size) => ImGuiMethods.LoadIniSettingsFromMemory(ini_data, ini_size);
    public void SaveIniSettingsToDisk(string ini_filename) => ImGuiMethods.SaveIniSettingsToDisk(ini_filename);
    public string SaveIniSettingsToMemory(nuint out_ini_size)
    {
        sbyte* retStrPtr = ImGuiMethods.SaveIniSettingsToMemory(out_ini_size);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public void DebugTextEncoding(string text) => ImGuiMethods.DebugTextEncoding(text);
    public void DebugFlashStyleColor(ImGuiCol idx) => ImGuiMethods.DebugFlashStyleColor((int)idx);
    public void DebugStartItemPicker() => ImGuiMethods.DebugStartItemPicker();
    public bool DebugCheckVersionAndDataLayout(string version_str, nuint sz_io, nuint sz_style, nuint sz_vec2, nuint sz_vec4, nuint sz_drawvert, nuint sz_drawidx) => ImGuiMethods.DebugCheckVersionAndDataLayout(version_str, sz_io, sz_style, sz_vec2, sz_vec4, sz_drawvert, sz_drawidx);
    public void DebugLog(string fmt) => ImGuiMethods.DebugLog(fmt);
    public void DebugLogV(string fmt, sbyte* args) => ImGuiMethods.DebugLogV(fmt, args);
    public void SetAllocatorFunctions(delegate* unmanaged[Cdecl]<nuint, nint, nint> alloc_func, delegate* unmanaged[Cdecl]<nint, nint, void> free_func, void* user_data) => ImGuiMethods.SetAllocatorFunctions(alloc_func, free_func, user_data);
    public void GetAllocatorFunctions(delegate* unmanaged[Cdecl]<nuint, void*, void*> * p_alloc_func, delegate* unmanaged[Cdecl]<void*, void*, void> * p_free_func, void** p_user_data) => ImGuiMethods.GetAllocatorFunctions(p_alloc_func, p_free_func, p_user_data);
    public void* MemAlloc(nuint size) => ImGuiMethods.MemAlloc(size);
    public void MemFree(void* ptr) => ImGuiMethods.MemFree(ptr);
    public void UpdatePlatformWindows() => ImGuiMethods.UpdatePlatformWindows();
    public void RenderPlatformWindowsDefault() => ImGuiMethods.RenderPlatformWindowsDefault();
    public void RenderPlatformWindowsDefaultEx(void* platform_render_arg, void* renderer_render_arg) => ImGuiMethods.RenderPlatformWindowsDefaultEx(platform_render_arg, renderer_render_arg);
    public void DestroyPlatformWindows() => ImGuiMethods.DestroyPlatformWindows();
    public IImGuiViewport FindViewportByID(uint viewport_id)
    {
        var ret = ImGuiMethods.FindViewportByID(viewport_id);
        if (ret is null)
        return null !;
        return new ImGuiViewport(ret);
    }

    public IImGuiViewport FindViewportByPlatformHandle(void* platform_handle)
    {
        var ret = ImGuiMethods.FindViewportByPlatformHandle(platform_handle);
        if (ret is null)
        return null !;
        return new ImGuiViewport(ret);
    }

    public void ImVector_Construct(void* vector) => ImGuiMethods.ImVector_Construct(vector);
    public void ImVector_Destruct(void* vector) => ImGuiMethods.ImVector_Destruct(vector);
    public void ImGuiPlatformIO_SetPlatform_GetWindowWorkAreaInsets(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowWorkAreaInsetsFunc) => ImGuiMethods.ImGuiPlatformIO_SetPlatform_GetWindowWorkAreaInsets(getWindowWorkAreaInsetsFunc);
    public void ImGuiPlatformIO_SetPlatform_GetWindowFramebufferScale(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowFramebufferScaleFunc) => ImGuiMethods.ImGuiPlatformIO_SetPlatform_GetWindowFramebufferScale(getWindowFramebufferScaleFunc);
    public void ImGuiPlatformIO_SetPlatform_GetWindowPos(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowPosFunc) => ImGuiMethods.ImGuiPlatformIO_SetPlatform_GetWindowPos(getWindowPosFunc);
    public void ImGuiPlatformIO_SetPlatform_GetWindowSize(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowSizeFunc) => ImGuiMethods.ImGuiPlatformIO_SetPlatform_GetWindowSize(getWindowSizeFunc);
    public void ImGuiStyle_ScaleAllSizes(IImGuiStyle self, float scale_factor) => ImGuiMethods.ImGuiStyle_ScaleAllSizes(self is not null ? (ImGuiStyleStruct*)self.NativePointer : null, scale_factor);
    public void ImGuiIO_AddKeyEvent(IImGuiIO self, int key, bool down) => ImGuiMethods.ImGuiIO_AddKeyEvent(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, key, down);
    public void ImGuiIO_AddKeyAnalogEvent(IImGuiIO self, int key, bool down, float v) => ImGuiMethods.ImGuiIO_AddKeyAnalogEvent(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, key, down, v);
    public void ImGuiIO_AddMousePosEvent(IImGuiIO self, float x, float y) => ImGuiMethods.ImGuiIO_AddMousePosEvent(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, x, y);
    public void ImGuiIO_AddMouseButtonEvent(IImGuiIO self, int button, bool down) => ImGuiMethods.ImGuiIO_AddMouseButtonEvent(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, button, down);
    public void ImGuiIO_AddMouseWheelEvent(IImGuiIO self, float wheel_x, float wheel_y) => ImGuiMethods.ImGuiIO_AddMouseWheelEvent(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, wheel_x, wheel_y);
    public void ImGuiIO_AddMouseSourceEvent(IImGuiIO self, int source) => ImGuiMethods.ImGuiIO_AddMouseSourceEvent(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, source);
    public void ImGuiIO_AddMouseViewportEvent(IImGuiIO self, uint id) => ImGuiMethods.ImGuiIO_AddMouseViewportEvent(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, id);
    public void ImGuiIO_AddFocusEvent(IImGuiIO self, bool focused) => ImGuiMethods.ImGuiIO_AddFocusEvent(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, focused);
    public void ImGuiIO_AddInputCharacter(IImGuiIO self, uint c) => ImGuiMethods.ImGuiIO_AddInputCharacter(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, c);
    public void ImGuiIO_AddInputCharacterUTF16(IImGuiIO self, ushort c) => ImGuiMethods.ImGuiIO_AddInputCharacterUTF16(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, c);
    public void ImGuiIO_AddInputCharactersUTF8(IImGuiIO self, string str) => ImGuiMethods.ImGuiIO_AddInputCharactersUTF8(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, str);
    public void ImGuiIO_SetKeyEventNativeData(IImGuiIO self, int key, int native_keycode, int native_scancode) => ImGuiMethods.ImGuiIO_SetKeyEventNativeData(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, key, native_keycode, native_scancode);
    public void ImGuiIO_SetKeyEventNativeDataEx(IImGuiIO self, int key, int native_keycode, int native_scancode, int native_legacy_index) => ImGuiMethods.ImGuiIO_SetKeyEventNativeDataEx(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, key, native_keycode, native_scancode, native_legacy_index);
    public void ImGuiIO_SetAppAcceptingEvents(IImGuiIO self, bool accepting_events) => ImGuiMethods.ImGuiIO_SetAppAcceptingEvents(self is not null ? (ImGuiIOStruct*)self.NativePointer : null, accepting_events);
    public void ImGuiIO_ClearEventsQueue(IImGuiIO self) => ImGuiMethods.ImGuiIO_ClearEventsQueue(self is not null ? (ImGuiIOStruct*)self.NativePointer : null);
    public void ImGuiIO_ClearInputKeys(IImGuiIO self) => ImGuiMethods.ImGuiIO_ClearInputKeys(self is not null ? (ImGuiIOStruct*)self.NativePointer : null);
    public void ImGuiIO_ClearInputMouse(IImGuiIO self) => ImGuiMethods.ImGuiIO_ClearInputMouse(self is not null ? (ImGuiIOStruct*)self.NativePointer : null);
    public void ImGuiInputTextCallbackData_DeleteChars(IImGuiInputTextCallbackData self, int pos, int bytes_count) => ImGuiMethods.ImGuiInputTextCallbackData_DeleteChars(self is not null ? (ImGuiInputTextCallbackDataStruct*)self.NativePointer : null, pos, bytes_count);
    public void ImGuiInputTextCallbackData_InsertChars(IImGuiInputTextCallbackData self, int pos, string text, string text_end) => ImGuiMethods.ImGuiInputTextCallbackData_InsertChars(self is not null ? (ImGuiInputTextCallbackDataStruct*)self.NativePointer : null, pos, text, text_end);
    public void ImGuiInputTextCallbackData_SelectAll(IImGuiInputTextCallbackData self) => ImGuiMethods.ImGuiInputTextCallbackData_SelectAll(self is not null ? (ImGuiInputTextCallbackDataStruct*)self.NativePointer : null);
    public void ImGuiInputTextCallbackData_ClearSelection(IImGuiInputTextCallbackData self) => ImGuiMethods.ImGuiInputTextCallbackData_ClearSelection(self is not null ? (ImGuiInputTextCallbackDataStruct*)self.NativePointer : null);
    public bool ImGuiInputTextCallbackData_HasSelection(IImGuiInputTextCallbackData self) => ImGuiMethods.ImGuiInputTextCallbackData_HasSelection(self is not null ? (ImGuiInputTextCallbackDataStruct*)self.NativePointer : null);
    public void ImGuiPayload_Clear(IImGuiPayload self) => ImGuiMethods.ImGuiPayload_Clear(self is not null ? (ImGuiPayloadStruct*)self.NativePointer : null);
    public bool ImGuiPayload_IsDataType(IImGuiPayload self, string type) => ImGuiMethods.ImGuiPayload_IsDataType(self is not null ? (ImGuiPayloadStruct*)self.NativePointer : null, type);
    public bool ImGuiPayload_IsPreview(IImGuiPayload self) => ImGuiMethods.ImGuiPayload_IsPreview(self is not null ? (ImGuiPayloadStruct*)self.NativePointer : null);
    public bool ImGuiPayload_IsDelivery(IImGuiPayload self) => ImGuiMethods.ImGuiPayload_IsDelivery(self is not null ? (ImGuiPayloadStruct*)self.NativePointer : null);
    public bool ImGuiTextFilter_ImGuiTextRange_empty(IImGuiTextFilter_ImGuiTextRange self) => ImGuiMethods.ImGuiTextFilter_ImGuiTextRange_empty(self is not null ? (ImGuiTextFilter_ImGuiTextRangeStruct*)self.NativePointer : null);
    public bool ImGuiTextFilter_Draw(IImGuiTextFilter self, string label, float width) => ImGuiMethods.ImGuiTextFilter_Draw(self is not null ? (ImGuiTextFilterStruct*)self.NativePointer : null, label, width);
    public bool ImGuiTextFilter_PassFilter(IImGuiTextFilter self, string text, string text_end) => ImGuiMethods.ImGuiTextFilter_PassFilter(self is not null ? (ImGuiTextFilterStruct*)self.NativePointer : null, text, text_end);
    public void ImGuiTextFilter_Build(IImGuiTextFilter self) => ImGuiMethods.ImGuiTextFilter_Build(self is not null ? (ImGuiTextFilterStruct*)self.NativePointer : null);
    public void ImGuiTextFilter_Clear(IImGuiTextFilter self) => ImGuiMethods.ImGuiTextFilter_Clear(self is not null ? (ImGuiTextFilterStruct*)self.NativePointer : null);
    public bool ImGuiTextFilter_IsActive(IImGuiTextFilter self) => ImGuiMethods.ImGuiTextFilter_IsActive(self is not null ? (ImGuiTextFilterStruct*)self.NativePointer : null);
    public string ImGuiTextBuffer_begin(IImGuiTextBuffer self)
    {
        sbyte* retStrPtr = ImGuiMethods.ImGuiTextBuffer_begin(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public string ImGuiTextBuffer_end(IImGuiTextBuffer self)
    {
        sbyte* retStrPtr = ImGuiMethods.ImGuiTextBuffer_end(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public int ImGuiTextBuffer_size(IImGuiTextBuffer self) => ImGuiMethods.ImGuiTextBuffer_size(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null);
    public bool ImGuiTextBuffer_empty(IImGuiTextBuffer self) => ImGuiMethods.ImGuiTextBuffer_empty(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null);
    public void ImGuiTextBuffer_clear(IImGuiTextBuffer self) => ImGuiMethods.ImGuiTextBuffer_clear(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null);
    public void ImGuiTextBuffer_resize(IImGuiTextBuffer self, int size) => ImGuiMethods.ImGuiTextBuffer_resize(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null, size);
    public void ImGuiTextBuffer_reserve(IImGuiTextBuffer self, int capacity) => ImGuiMethods.ImGuiTextBuffer_reserve(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null, capacity);
    public string ImGuiTextBuffer_c_str(IImGuiTextBuffer self)
    {
        sbyte* retStrPtr = ImGuiMethods.ImGuiTextBuffer_c_str(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public void ImGuiTextBuffer_append(IImGuiTextBuffer self, string str, string str_end) => ImGuiMethods.ImGuiTextBuffer_append(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null, str, str_end);
    public void ImGuiTextBuffer_appendf(IImGuiTextBuffer self, string fmt) => ImGuiMethods.ImGuiTextBuffer_appendf(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null, fmt);
    public void ImGuiTextBuffer_appendfv(IImGuiTextBuffer self, string fmt, sbyte* args) => ImGuiMethods.ImGuiTextBuffer_appendfv(self is not null ? (ImGuiTextBufferStruct*)self.NativePointer : null, fmt, args);
    public void ImGuiStorage_Clear(IImGuiStorage self) => ImGuiMethods.ImGuiStorage_Clear(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null);
    public int ImGuiStorage_GetInt(IImGuiStorage self, uint key, int default_val) => ImGuiMethods.ImGuiStorage_GetInt(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, default_val);
    public void ImGuiStorage_SetInt(IImGuiStorage self, uint key, int val) => ImGuiMethods.ImGuiStorage_SetInt(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, val);
    public bool ImGuiStorage_GetBool(IImGuiStorage self, uint key, bool default_val) => ImGuiMethods.ImGuiStorage_GetBool(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, default_val);
    public void ImGuiStorage_SetBool(IImGuiStorage self, uint key, bool val) => ImGuiMethods.ImGuiStorage_SetBool(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, val);
    public float ImGuiStorage_GetFloat(IImGuiStorage self, uint key, float default_val) => ImGuiMethods.ImGuiStorage_GetFloat(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, default_val);
    public void ImGuiStorage_SetFloat(IImGuiStorage self, uint key, float val) => ImGuiMethods.ImGuiStorage_SetFloat(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, val);
    public void* ImGuiStorage_GetVoidPtr(IImGuiStorage self, uint key) => ImGuiMethods.ImGuiStorage_GetVoidPtr(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key);
    public void ImGuiStorage_SetVoidPtr(IImGuiStorage self, uint key, void* val) => ImGuiMethods.ImGuiStorage_SetVoidPtr(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, val);
    public int* ImGuiStorage_GetIntRef(IImGuiStorage self, uint key, int default_val) => ImGuiMethods.ImGuiStorage_GetIntRef(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, default_val);
    public bool* ImGuiStorage_GetBoolRef(IImGuiStorage self, uint key, bool default_val) => ImGuiMethods.ImGuiStorage_GetBoolRef(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, default_val);
    public float* ImGuiStorage_GetFloatRef(IImGuiStorage self, uint key, float default_val) => ImGuiMethods.ImGuiStorage_GetFloatRef(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, default_val);
    public void** ImGuiStorage_GetVoidPtrRef(IImGuiStorage self, uint key, void* default_val) => ImGuiMethods.ImGuiStorage_GetVoidPtrRef(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, key, default_val);
    public void ImGuiStorage_BuildSortByKey(IImGuiStorage self) => ImGuiMethods.ImGuiStorage_BuildSortByKey(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null);
    public void ImGuiStorage_SetAllInt(IImGuiStorage self, int val) => ImGuiMethods.ImGuiStorage_SetAllInt(self is not null ? (ImGuiStorageStruct*)self.NativePointer : null, val);
    public void ImGuiListClipper_Begin(IImGuiListClipper self, int items_count, float items_height) => ImGuiMethods.ImGuiListClipper_Begin(self is not null ? (ImGuiListClipperStruct*)self.NativePointer : null, items_count, items_height);
    public void ImGuiListClipper_End(IImGuiListClipper self) => ImGuiMethods.ImGuiListClipper_End(self is not null ? (ImGuiListClipperStruct*)self.NativePointer : null);
    public bool ImGuiListClipper_Step(IImGuiListClipper self) => ImGuiMethods.ImGuiListClipper_Step(self is not null ? (ImGuiListClipperStruct*)self.NativePointer : null);
    public void ImGuiListClipper_IncludeItemByIndex(IImGuiListClipper self, int item_index) => ImGuiMethods.ImGuiListClipper_IncludeItemByIndex(self is not null ? (ImGuiListClipperStruct*)self.NativePointer : null, item_index);
    public void ImGuiListClipper_IncludeItemsByIndex(IImGuiListClipper self, int item_begin, int item_end) => ImGuiMethods.ImGuiListClipper_IncludeItemsByIndex(self is not null ? (ImGuiListClipperStruct*)self.NativePointer : null, item_begin, item_end);
    public void ImGuiListClipper_SeekCursorForItem(IImGuiListClipper self, int item_index) => ImGuiMethods.ImGuiListClipper_SeekCursorForItem(self is not null ? (ImGuiListClipperStruct*)self.NativePointer : null, item_index);
    public void ImColor_SetHSV(Vector4 self, float h, float s, float v, float a) => ImGuiMethods.ImColor_SetHSV(self, h, s, v, a);
    public Vector4 ImColor_HSV(float h, float s, float v, float a) => ImGuiMethods.ImColor_HSV(h, s, v, a);
    public void ImGuiSelectionBasicStorage_ApplyRequests(IImGuiSelectionBasicStorage self, IImGuiMultiSelectIO ms_io) => ImGuiMethods.ImGuiSelectionBasicStorage_ApplyRequests(self is not null ? (ImGuiSelectionBasicStorageStruct*)self.NativePointer : null, ms_io is not null ? (ImGuiMultiSelectIOStruct*)ms_io.NativePointer : null);
    public bool ImGuiSelectionBasicStorage_Contains(IImGuiSelectionBasicStorage self, uint id) => ImGuiMethods.ImGuiSelectionBasicStorage_Contains(self is not null ? (ImGuiSelectionBasicStorageStruct*)self.NativePointer : null, id);
    public void ImGuiSelectionBasicStorage_Clear(IImGuiSelectionBasicStorage self) => ImGuiMethods.ImGuiSelectionBasicStorage_Clear(self is not null ? (ImGuiSelectionBasicStorageStruct*)self.NativePointer : null);
    public void ImGuiSelectionBasicStorage_Swap(IImGuiSelectionBasicStorage self, IImGuiSelectionBasicStorage r) => ImGuiMethods.ImGuiSelectionBasicStorage_Swap(self is not null ? (ImGuiSelectionBasicStorageStruct*)self.NativePointer : null, r is not null ? (ImGuiSelectionBasicStorageStruct*)r.NativePointer : null);
    public void ImGuiSelectionBasicStorage_SetItemSelected(IImGuiSelectionBasicStorage self, uint id, bool selected) => ImGuiMethods.ImGuiSelectionBasicStorage_SetItemSelected(self is not null ? (ImGuiSelectionBasicStorageStruct*)self.NativePointer : null, id, selected);
    public bool ImGuiSelectionBasicStorage_GetNextSelectedItem(IImGuiSelectionBasicStorage self, void** opaque_it, ref uint out_id) => ImGuiMethods.ImGuiSelectionBasicStorage_GetNextSelectedItem(self is not null ? (ImGuiSelectionBasicStorageStruct*)self.NativePointer : null, opaque_it, ref out_id);
    public uint ImGuiSelectionBasicStorage_GetStorageIdFromIndex(IImGuiSelectionBasicStorage self, int idx) => ImGuiMethods.ImGuiSelectionBasicStorage_GetStorageIdFromIndex(self is not null ? (ImGuiSelectionBasicStorageStruct*)self.NativePointer : null, idx);
    public void ImGuiSelectionExternalStorage_ApplyRequests(IImGuiSelectionExternalStorage self, IImGuiMultiSelectIO ms_io) => ImGuiMethods.ImGuiSelectionExternalStorage_ApplyRequests(self is not null ? (ImGuiSelectionExternalStorageStruct*)self.NativePointer : null, ms_io is not null ? (ImGuiMultiSelectIOStruct*)ms_io.NativePointer : null);
    public ulong ImDrawCmd_GetTexID(IImDrawCmd self) => ImGuiMethods.ImDrawCmd_GetTexID(self is not null ? (ImDrawCmdStruct*)self.NativePointer : null);
    public void ImDrawListSplitter_Clear(IImDrawListSplitter self) => ImGuiMethods.ImDrawListSplitter_Clear(self is not null ? (ImDrawListSplitterStruct*)self.NativePointer : null);
    public void ImDrawListSplitter_ClearFreeMemory(IImDrawListSplitter self) => ImGuiMethods.ImDrawListSplitter_ClearFreeMemory(self is not null ? (ImDrawListSplitterStruct*)self.NativePointer : null);
    public void ImDrawListSplitter_Split(IImDrawListSplitter self, IImDrawList draw_list, int count) => ImGuiMethods.ImDrawListSplitter_Split(self is not null ? (ImDrawListSplitterStruct*)self.NativePointer : null, draw_list is not null ? (ImDrawListStruct*)draw_list.NativePointer : null, count);
    public void ImDrawListSplitter_Merge(IImDrawListSplitter self, IImDrawList draw_list) => ImGuiMethods.ImDrawListSplitter_Merge(self is not null ? (ImDrawListSplitterStruct*)self.NativePointer : null, draw_list is not null ? (ImDrawListStruct*)draw_list.NativePointer : null);
    public void ImDrawListSplitter_SetCurrentChannel(IImDrawListSplitter self, IImDrawList draw_list, int channel_idx) => ImGuiMethods.ImDrawListSplitter_SetCurrentChannel(self is not null ? (ImDrawListSplitterStruct*)self.NativePointer : null, draw_list is not null ? (ImDrawListStruct*)draw_list.NativePointer : null, channel_idx);
    public void ImDrawList_PushClipRect(IImDrawList self, Vector2 clip_rect_min, Vector2 clip_rect_max, bool intersect_with_current_clip_rect) => ImGuiMethods.ImDrawList_PushClipRect(self is not null ? (ImDrawListStruct*)self.NativePointer : null, clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
    public void ImDrawList_PushClipRectFullScreen(IImDrawList self) => ImGuiMethods.ImDrawList_PushClipRectFullScreen(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList_PopClipRect(IImDrawList self) => ImGuiMethods.ImDrawList_PopClipRect(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList_PushTexture(IImDrawList self, IImTextureRef tex_ref) => ImGuiMethods.ImDrawList_PushTexture(self is not null ? (ImDrawListStruct*)self.NativePointer : null, ((ImTextureRef)tex_ref).ToStruct());
    public void ImDrawList_PopTexture(IImDrawList self) => ImGuiMethods.ImDrawList_PopTexture(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public Vector2 ImDrawList_GetClipRectMin(IImDrawList self) => ImGuiMethods.ImDrawList_GetClipRectMin(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public Vector2 ImDrawList_GetClipRectMax(IImDrawList self) => ImGuiMethods.ImDrawList_GetClipRectMax(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList_AddLine(IImDrawList self, Vector2 p1, Vector2 p2, uint col) => ImGuiMethods.ImDrawList_AddLine(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, col);
    public void ImDrawList_AddLineEx(IImDrawList self, Vector2 p1, Vector2 p2, uint col, float thickness) => ImGuiMethods.ImDrawList_AddLineEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, col, thickness);
    public void ImDrawList_AddRect(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col) => ImGuiMethods.ImDrawList_AddRect(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p_min, p_max, col);
    public void ImDrawList_AddRectEx(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags, float thickness) => ImGuiMethods.ImDrawList_AddRectEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p_min, p_max, col, rounding, (int)flags, thickness);
    public void ImDrawList_AddRectFilled(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col) => ImGuiMethods.ImDrawList_AddRectFilled(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p_min, p_max, col);
    public void ImDrawList_AddRectFilledEx(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags) => ImGuiMethods.ImDrawList_AddRectFilledEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p_min, p_max, col, rounding, (int)flags);
    public void ImDrawList_AddRectFilledMultiColor(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left) => ImGuiMethods.ImDrawList_AddRectFilledMultiColor(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p_min, p_max, col_upr_left, col_upr_right, col_bot_right, col_bot_left);
    public void ImDrawList_AddQuad(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col) => ImGuiMethods.ImDrawList_AddQuad(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, p3, p4, col);
    public void ImDrawList_AddQuadEx(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness) => ImGuiMethods.ImDrawList_AddQuadEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, p3, p4, col, thickness);
    public void ImDrawList_AddQuadFilled(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col) => ImGuiMethods.ImDrawList_AddQuadFilled(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, p3, p4, col);
    public void ImDrawList_AddTriangle(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, uint col) => ImGuiMethods.ImDrawList_AddTriangle(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, p3, col);
    public void ImDrawList_AddTriangleEx(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness) => ImGuiMethods.ImDrawList_AddTriangleEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, p3, col, thickness);
    public void ImDrawList_AddTriangleFilled(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, uint col) => ImGuiMethods.ImDrawList_AddTriangleFilled(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, p3, col);
    public void ImDrawList_AddCircle(IImDrawList self, Vector2 center, float radius, uint col) => ImGuiMethods.ImDrawList_AddCircle(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col);
    public void ImDrawList_AddCircleEx(IImDrawList self, Vector2 center, float radius, uint col, int num_segments, float thickness) => ImGuiMethods.ImDrawList_AddCircleEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col, num_segments, thickness);
    public void ImDrawList_AddCircleFilled(IImDrawList self, Vector2 center, float radius, uint col, int num_segments) => ImGuiMethods.ImDrawList_AddCircleFilled(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col, num_segments);
    public void ImDrawList_AddNgon(IImDrawList self, Vector2 center, float radius, uint col, int num_segments) => ImGuiMethods.ImDrawList_AddNgon(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col, num_segments);
    public void ImDrawList_AddNgonEx(IImDrawList self, Vector2 center, float radius, uint col, int num_segments, float thickness) => ImGuiMethods.ImDrawList_AddNgonEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col, num_segments, thickness);
    public void ImDrawList_AddNgonFilled(IImDrawList self, Vector2 center, float radius, uint col, int num_segments) => ImGuiMethods.ImDrawList_AddNgonFilled(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col, num_segments);
    public void ImDrawList_AddEllipse(IImDrawList self, Vector2 center, Vector2 radius, uint col) => ImGuiMethods.ImDrawList_AddEllipse(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col);
    public void ImDrawList_AddEllipseEx(IImDrawList self, Vector2 center, Vector2 radius, uint col, float rot, int num_segments, float thickness) => ImGuiMethods.ImDrawList_AddEllipseEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col, rot, num_segments, thickness);
    public void ImDrawList_AddEllipseFilled(IImDrawList self, Vector2 center, Vector2 radius, uint col) => ImGuiMethods.ImDrawList_AddEllipseFilled(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col);
    public void ImDrawList_AddEllipseFilledEx(IImDrawList self, Vector2 center, Vector2 radius, uint col, float rot, int num_segments) => ImGuiMethods.ImDrawList_AddEllipseFilledEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, col, rot, num_segments);
    public void ImDrawList_AddText(IImDrawList self, Vector2 pos, uint col, string text_begin) => ImGuiMethods.ImDrawList_AddText(self is not null ? (ImDrawListStruct*)self.NativePointer : null, pos, col, text_begin);
    public void ImDrawList_AddTextEx(IImDrawList self, Vector2 pos, uint col, string text_begin, string text_end) => ImGuiMethods.ImDrawList_AddTextEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, pos, col, text_begin, text_end);
    public void ImDrawList_AddTextImFontPtr(IImDrawList self, IImFont font, float font_size, Vector2 pos, uint col, string text_begin) => ImGuiMethods.ImDrawList_AddTextImFontPtr(self is not null ? (ImDrawListStruct*)self.NativePointer : null, font is not null ? (ImFontStruct*)font.NativePointer : null, font_size, pos, col, text_begin);
    public void ImDrawList_AddTextImFontPtrEx(IImDrawList self, IImFont font, float font_size, Vector2 pos, uint col, string text_begin, string text_end, float wrap_width, Vector4 cpu_fine_clip_rect) => ImGuiMethods.ImDrawList_AddTextImFontPtrEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, font is not null ? (ImFontStruct*)font.NativePointer : null, font_size, pos, col, text_begin, text_end, wrap_width, cpu_fine_clip_rect);
    public void ImDrawList_AddBezierCubic(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness, int num_segments) => ImGuiMethods.ImDrawList_AddBezierCubic(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, p3, p4, col, thickness, num_segments);
    public void ImDrawList_AddBezierQuadratic(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness, int num_segments) => ImGuiMethods.ImDrawList_AddBezierQuadratic(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p1, p2, p3, col, thickness, num_segments);
    public void ImDrawList_AddPolyline(IImDrawList self, Vector2 points, int num_points, uint col, ImDrawFlags flags, float thickness) => ImGuiMethods.ImDrawList_AddPolyline(self is not null ? (ImDrawListStruct*)self.NativePointer : null, points, num_points, col, (int)flags, thickness);
    public void ImDrawList_AddConvexPolyFilled(IImDrawList self, Vector2 points, int num_points, uint col) => ImGuiMethods.ImDrawList_AddConvexPolyFilled(self is not null ? (ImDrawListStruct*)self.NativePointer : null, points, num_points, col);
    public void ImDrawList_AddConcavePolyFilled(IImDrawList self, Vector2 points, int num_points, uint col) => ImGuiMethods.ImDrawList_AddConcavePolyFilled(self is not null ? (ImDrawListStruct*)self.NativePointer : null, points, num_points, col);
    public void ImDrawList_AddImage(IImDrawList self, IImTextureRef tex_ref, Vector2 p_min, Vector2 p_max) => ImGuiMethods.ImDrawList_AddImage(self is not null ? (ImDrawListStruct*)self.NativePointer : null, ((ImTextureRef)tex_ref).ToStruct(), p_min, p_max);
    public void ImDrawList_AddImageEx(IImDrawList self, IImTextureRef tex_ref, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col) => ImGuiMethods.ImDrawList_AddImageEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, ((ImTextureRef)tex_ref).ToStruct(), p_min, p_max, uv_min, uv_max, col);
    public void ImDrawList_AddImageQuad(IImDrawList self, IImTextureRef tex_ref, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) => ImGuiMethods.ImDrawList_AddImageQuad(self is not null ? (ImDrawListStruct*)self.NativePointer : null, ((ImTextureRef)tex_ref).ToStruct(), p1, p2, p3, p4);
    public void ImDrawList_AddImageQuadEx(IImDrawList self, IImTextureRef tex_ref, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, uint col) => ImGuiMethods.ImDrawList_AddImageQuadEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, ((ImTextureRef)tex_ref).ToStruct(), p1, p2, p3, p4, uv1, uv2, uv3, uv4, col);
    public void ImDrawList_AddImageRounded(IImDrawList self, IImTextureRef tex_ref, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col, float rounding, ImDrawFlags flags) => ImGuiMethods.ImDrawList_AddImageRounded(self is not null ? (ImDrawListStruct*)self.NativePointer : null, ((ImTextureRef)tex_ref).ToStruct(), p_min, p_max, uv_min, uv_max, col, rounding, (int)flags);
    public void ImDrawList_PathClear(IImDrawList self) => ImGuiMethods.ImDrawList_PathClear(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList_PathLineTo(IImDrawList self, Vector2 pos) => ImGuiMethods.ImDrawList_PathLineTo(self is not null ? (ImDrawListStruct*)self.NativePointer : null, pos);
    public void ImDrawList_PathLineToMergeDuplicate(IImDrawList self, Vector2 pos) => ImGuiMethods.ImDrawList_PathLineToMergeDuplicate(self is not null ? (ImDrawListStruct*)self.NativePointer : null, pos);
    public void ImDrawList_PathFillConvex(IImDrawList self, uint col) => ImGuiMethods.ImDrawList_PathFillConvex(self is not null ? (ImDrawListStruct*)self.NativePointer : null, col);
    public void ImDrawList_PathFillConcave(IImDrawList self, uint col) => ImGuiMethods.ImDrawList_PathFillConcave(self is not null ? (ImDrawListStruct*)self.NativePointer : null, col);
    public void ImDrawList_PathStroke(IImDrawList self, uint col, ImDrawFlags flags, float thickness) => ImGuiMethods.ImDrawList_PathStroke(self is not null ? (ImDrawListStruct*)self.NativePointer : null, col, (int)flags, thickness);
    public void ImDrawList_PathArcTo(IImDrawList self, Vector2 center, float radius, float a_min, float a_max, int num_segments) => ImGuiMethods.ImDrawList_PathArcTo(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, a_min, a_max, num_segments);
    public void ImDrawList_PathArcToFast(IImDrawList self, Vector2 center, float radius, int a_min_of_12, int a_max_of_12) => ImGuiMethods.ImDrawList_PathArcToFast(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, a_min_of_12, a_max_of_12);
    public void ImDrawList_PathEllipticalArcTo(IImDrawList self, Vector2 center, Vector2 radius, float rot, float a_min, float a_max) => ImGuiMethods.ImDrawList_PathEllipticalArcTo(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, rot, a_min, a_max);
    public void ImDrawList_PathEllipticalArcToEx(IImDrawList self, Vector2 center, Vector2 radius, float rot, float a_min, float a_max, int num_segments) => ImGuiMethods.ImDrawList_PathEllipticalArcToEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, rot, a_min, a_max, num_segments);
    public void ImDrawList_PathBezierCubicCurveTo(IImDrawList self, Vector2 p2, Vector2 p3, Vector2 p4, int num_segments) => ImGuiMethods.ImDrawList_PathBezierCubicCurveTo(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p2, p3, p4, num_segments);
    public void ImDrawList_PathBezierQuadraticCurveTo(IImDrawList self, Vector2 p2, Vector2 p3, int num_segments) => ImGuiMethods.ImDrawList_PathBezierQuadraticCurveTo(self is not null ? (ImDrawListStruct*)self.NativePointer : null, p2, p3, num_segments);
    public void ImDrawList_PathRect(IImDrawList self, Vector2 rect_min, Vector2 rect_max, float rounding, ImDrawFlags flags) => ImGuiMethods.ImDrawList_PathRect(self is not null ? (ImDrawListStruct*)self.NativePointer : null, rect_min, rect_max, rounding, (int)flags);
    public void ImDrawList_AddCallback(IImDrawList self, delegate* unmanaged[Cdecl]<nint, nint, void> callback, void* userdata) => ImGuiMethods.ImDrawList_AddCallback(self is not null ? (ImDrawListStruct*)self.NativePointer : null, callback, userdata);
    public void ImDrawList_AddCallbackEx(IImDrawList self, delegate* unmanaged[Cdecl]<nint, nint, void> callback, void* userdata, nuint userdata_size) => ImGuiMethods.ImDrawList_AddCallbackEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, callback, userdata, userdata_size);
    public void ImDrawList_AddDrawCmd(IImDrawList self) => ImGuiMethods.ImDrawList_AddDrawCmd(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public IImDrawList ImDrawList_CloneOutput(IImDrawList self)
    {
        var ret = ImGuiMethods.ImDrawList_CloneOutput(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
        if (ret is null)
        return null !;
        return new ImDrawList(ret);
    }

    public void ImDrawList_ChannelsSplit(IImDrawList self, int count) => ImGuiMethods.ImDrawList_ChannelsSplit(self is not null ? (ImDrawListStruct*)self.NativePointer : null, count);
    public void ImDrawList_ChannelsMerge(IImDrawList self) => ImGuiMethods.ImDrawList_ChannelsMerge(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList_ChannelsSetCurrent(IImDrawList self, int n) => ImGuiMethods.ImDrawList_ChannelsSetCurrent(self is not null ? (ImDrawListStruct*)self.NativePointer : null, n);
    public void ImDrawList_PrimReserve(IImDrawList self, int idx_count, int vtx_count) => ImGuiMethods.ImDrawList_PrimReserve(self is not null ? (ImDrawListStruct*)self.NativePointer : null, idx_count, vtx_count);
    public void ImDrawList_PrimUnreserve(IImDrawList self, int idx_count, int vtx_count) => ImGuiMethods.ImDrawList_PrimUnreserve(self is not null ? (ImDrawListStruct*)self.NativePointer : null, idx_count, vtx_count);
    public void ImDrawList_PrimRect(IImDrawList self, Vector2 a, Vector2 b, uint col) => ImGuiMethods.ImDrawList_PrimRect(self is not null ? (ImDrawListStruct*)self.NativePointer : null, a, b, col);
    public void ImDrawList_PrimRectUV(IImDrawList self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col) => ImGuiMethods.ImDrawList_PrimRectUV(self is not null ? (ImDrawListStruct*)self.NativePointer : null, a, b, uv_a, uv_b, col);
    public void ImDrawList_PrimQuadUV(IImDrawList self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col) => ImGuiMethods.ImDrawList_PrimQuadUV(self is not null ? (ImDrawListStruct*)self.NativePointer : null, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
    public void ImDrawList_PrimWriteVtx(IImDrawList self, Vector2 pos, Vector2 uv, uint col) => ImGuiMethods.ImDrawList_PrimWriteVtx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, pos, uv, col);
    public void ImDrawList_PrimWriteIdx(IImDrawList self, ushort idx) => ImGuiMethods.ImDrawList_PrimWriteIdx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, idx);
    public void ImDrawList_PrimVtx(IImDrawList self, Vector2 pos, Vector2 uv, uint col) => ImGuiMethods.ImDrawList_PrimVtx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, pos, uv, col);
    public void ImDrawList_PushTextureID(IImDrawList self, IImTextureRef tex_ref) => ImGuiMethods.ImDrawList_PushTextureID(self is not null ? (ImDrawListStruct*)self.NativePointer : null, ((ImTextureRef)tex_ref).ToStruct());
    public void ImDrawList_PopTextureID(IImDrawList self) => ImGuiMethods.ImDrawList_PopTextureID(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList__SetDrawListSharedData(IImDrawList self, nint data) => ImGuiMethods.ImDrawList__SetDrawListSharedData(self is not null ? (ImDrawListStruct*)self.NativePointer : null, data);
    public void ImDrawList__ResetForNewFrame(IImDrawList self) => ImGuiMethods.ImDrawList__ResetForNewFrame(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList__ClearFreeMemory(IImDrawList self) => ImGuiMethods.ImDrawList__ClearFreeMemory(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList__PopUnusedDrawCmd(IImDrawList self) => ImGuiMethods.ImDrawList__PopUnusedDrawCmd(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList__TryMergeDrawCmds(IImDrawList self) => ImGuiMethods.ImDrawList__TryMergeDrawCmds(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList__OnChangedClipRect(IImDrawList self) => ImGuiMethods.ImDrawList__OnChangedClipRect(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList__OnChangedTexture(IImDrawList self) => ImGuiMethods.ImDrawList__OnChangedTexture(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList__OnChangedVtxOffset(IImDrawList self) => ImGuiMethods.ImDrawList__OnChangedVtxOffset(self is not null ? (ImDrawListStruct*)self.NativePointer : null);
    public void ImDrawList__SetTexture(IImDrawList self, IImTextureRef tex_ref) => ImGuiMethods.ImDrawList__SetTexture(self is not null ? (ImDrawListStruct*)self.NativePointer : null, ((ImTextureRef)tex_ref).ToStruct());
    public int ImDrawList__CalcCircleAutoSegmentCount(IImDrawList self, float radius) => ImGuiMethods.ImDrawList__CalcCircleAutoSegmentCount(self is not null ? (ImDrawListStruct*)self.NativePointer : null, radius);
    public void ImDrawList__PathArcToFastEx(IImDrawList self, Vector2 center, float radius, int a_min_sample, int a_max_sample, int a_step) => ImGuiMethods.ImDrawList__PathArcToFastEx(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, a_min_sample, a_max_sample, a_step);
    public void ImDrawList__PathArcToN(IImDrawList self, Vector2 center, float radius, float a_min, float a_max, int num_segments) => ImGuiMethods.ImDrawList__PathArcToN(self is not null ? (ImDrawListStruct*)self.NativePointer : null, center, radius, a_min, a_max, num_segments);
    public void ImDrawData_Clear(IImDrawData self) => ImGuiMethods.ImDrawData_Clear(self is not null ? (ImDrawDataStruct*)self.NativePointer : null);
    public void ImDrawData_AddDrawList(IImDrawData self, IImDrawList draw_list) => ImGuiMethods.ImDrawData_AddDrawList(self is not null ? (ImDrawDataStruct*)self.NativePointer : null, draw_list is not null ? (ImDrawListStruct*)draw_list.NativePointer : null);
    public void ImDrawData_DeIndexAllBuffers(IImDrawData self) => ImGuiMethods.ImDrawData_DeIndexAllBuffers(self is not null ? (ImDrawDataStruct*)self.NativePointer : null);
    public void ImDrawData_ScaleClipRects(IImDrawData self, Vector2 fb_scale) => ImGuiMethods.ImDrawData_ScaleClipRects(self is not null ? (ImDrawDataStruct*)self.NativePointer : null, fb_scale);
    public void ImTextureData_Create(IImTextureData self, ImTextureFormat format, int w, int h) => ImGuiMethods.ImTextureData_Create(self is not null ? (ImTextureDataStruct*)self.NativePointer : null, (int)format, w, h);
    public void ImTextureData_DestroyPixels(IImTextureData self) => ImGuiMethods.ImTextureData_DestroyPixels(self is not null ? (ImTextureDataStruct*)self.NativePointer : null);
    public void* ImTextureData_GetPixels(IImTextureData self) => ImGuiMethods.ImTextureData_GetPixels(self is not null ? (ImTextureDataStruct*)self.NativePointer : null);
    public void* ImTextureData_GetPixelsAt(IImTextureData self, int x, int y) => ImGuiMethods.ImTextureData_GetPixelsAt(self is not null ? (ImTextureDataStruct*)self.NativePointer : null, x, y);
    public int ImTextureData_GetSizeInBytes(IImTextureData self) => ImGuiMethods.ImTextureData_GetSizeInBytes(self is not null ? (ImTextureDataStruct*)self.NativePointer : null);
    public int ImTextureData_GetPitch(IImTextureData self) => ImGuiMethods.ImTextureData_GetPitch(self is not null ? (ImTextureDataStruct*)self.NativePointer : null);
    public IImTextureRef ImTextureData_GetTexRef(IImTextureData self)
    {
        var ret = ImGuiMethods.ImTextureData_GetTexRef(self is not null ? (ImTextureDataStruct*)self.NativePointer : null);
        return new ImTextureRef(ret);
    }

    public ulong ImTextureData_GetTexID(IImTextureData self) => ImGuiMethods.ImTextureData_GetTexID(self is not null ? (ImTextureDataStruct*)self.NativePointer : null);
    public void ImTextureData_SetTexID(IImTextureData self, ulong tex_id) => ImGuiMethods.ImTextureData_SetTexID(self is not null ? (ImTextureDataStruct*)self.NativePointer : null, tex_id);
    public void ImTextureData_SetStatus(IImTextureData self, ImTextureStatus status) => ImGuiMethods.ImTextureData_SetStatus(self is not null ? (ImTextureDataStruct*)self.NativePointer : null, (int)status);
    public void ImFontGlyphRangesBuilder_Clear(IImFontGlyphRangesBuilder self) => ImGuiMethods.ImFontGlyphRangesBuilder_Clear(self is not null ? (ImFontGlyphRangesBuilderStruct*)self.NativePointer : null);
    public bool ImFontGlyphRangesBuilder_GetBit(IImFontGlyphRangesBuilder self, nuint n) => ImGuiMethods.ImFontGlyphRangesBuilder_GetBit(self is not null ? (ImFontGlyphRangesBuilderStruct*)self.NativePointer : null, n);
    public void ImFontGlyphRangesBuilder_SetBit(IImFontGlyphRangesBuilder self, nuint n) => ImGuiMethods.ImFontGlyphRangesBuilder_SetBit(self is not null ? (ImFontGlyphRangesBuilderStruct*)self.NativePointer : null, n);
    public void ImFontGlyphRangesBuilder_AddChar(IImFontGlyphRangesBuilder self, uint c) => ImGuiMethods.ImFontGlyphRangesBuilder_AddChar(self is not null ? (ImFontGlyphRangesBuilderStruct*)self.NativePointer : null, c);
    public void ImFontGlyphRangesBuilder_AddText(IImFontGlyphRangesBuilder self, string text, string text_end) => ImGuiMethods.ImFontGlyphRangesBuilder_AddText(self is not null ? (ImFontGlyphRangesBuilderStruct*)self.NativePointer : null, text, text_end);
    public void ImFontGlyphRangesBuilder_AddRanges(IImFontGlyphRangesBuilder self, ref uint ranges) => ImGuiMethods.ImFontGlyphRangesBuilder_AddRanges(self is not null ? (ImFontGlyphRangesBuilderStruct*)self.NativePointer : null, ref ranges);
    public IImFont ImFontAtlas_AddFont(IImFontAtlas self, IImFontConfig font_cfg)
    {
        var ret = ImGuiMethods.ImFontAtlas_AddFont(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, font_cfg is not null ? (ImFontConfigStruct*)font_cfg.NativePointer : null);
        if (ret is null)
        return null !;
        return new ImFont(ret);
    }

    public IImFont ImFontAtlas_AddFontDefault(IImFontAtlas self, IImFontConfig font_cfg)
    {
        var ret = ImGuiMethods.ImFontAtlas_AddFontDefault(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, font_cfg is not null ? (ImFontConfigStruct*)font_cfg.NativePointer : null);
        if (ret is null)
        return null !;
        return new ImFont(ret);
    }

    public IImFont ImFontAtlas_AddFontFromFileTTF(IImFontAtlas self, string filename, float size_pixels, IImFontConfig font_cfg, ref uint glyph_ranges)
    {
        var ret = ImGuiMethods.ImFontAtlas_AddFontFromFileTTF(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, filename, size_pixels, font_cfg is not null ? (ImFontConfigStruct*)font_cfg.NativePointer : null, ref glyph_ranges);
        if (ret is null)
        return null !;
        return new ImFont(ret);
    }

    public IImFont ImFontAtlas_AddFontFromMemoryTTF(IImFontAtlas self, void* font_data, int font_data_size, float size_pixels, IImFontConfig font_cfg, ref uint glyph_ranges)
    {
        var ret = ImGuiMethods.ImFontAtlas_AddFontFromMemoryTTF(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, font_data, font_data_size, size_pixels, font_cfg is not null ? (ImFontConfigStruct*)font_cfg.NativePointer : null, ref glyph_ranges);
        if (ret is null)
        return null !;
        return new ImFont(ret);
    }

    public IImFont ImFontAtlas_AddFontFromMemoryCompressedTTF(IImFontAtlas self, void* compressed_font_data, int compressed_font_data_size, float size_pixels, IImFontConfig font_cfg, ref uint glyph_ranges)
    {
        var ret = ImGuiMethods.ImFontAtlas_AddFontFromMemoryCompressedTTF(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, compressed_font_data, compressed_font_data_size, size_pixels, font_cfg is not null ? (ImFontConfigStruct*)font_cfg.NativePointer : null, ref glyph_ranges);
        if (ret is null)
        return null !;
        return new ImFont(ret);
    }

    public IImFont ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(IImFontAtlas self, string compressed_font_data_base85, float size_pixels, IImFontConfig font_cfg, ref uint glyph_ranges)
    {
        var ret = ImGuiMethods.ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, compressed_font_data_base85, size_pixels, font_cfg is not null ? (ImFontConfigStruct*)font_cfg.NativePointer : null, ref glyph_ranges);
        if (ret is null)
        return null !;
        return new ImFont(ret);
    }

    public void ImFontAtlas_RemoveFont(IImFontAtlas self, IImFont font) => ImGuiMethods.ImFontAtlas_RemoveFont(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, font is not null ? (ImFontStruct*)font.NativePointer : null);
    public void ImFontAtlas_Clear(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_Clear(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public void ImFontAtlas_CompactCache(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_CompactCache(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public void ImFontAtlas_SetFontLoader(IImFontAtlas self, IImFontLoader font_loader) => ImGuiMethods.ImFontAtlas_SetFontLoader(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, font_loader is not null ? (ImFontLoaderStruct*)font_loader.NativePointer : null);
    public void ImFontAtlas_ClearInputData(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_ClearInputData(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public void ImFontAtlas_ClearFonts(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_ClearFonts(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public void ImFontAtlas_ClearTexData(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_ClearTexData(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public bool ImFontAtlas_Build(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_Build(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public void ImFontAtlas_GetTexDataAsAlpha8(IImFontAtlas self, byte** out_pixels, ref int out_width, ref int out_height, ref int out_bytes_per_pixel) => ImGuiMethods.ImFontAtlas_GetTexDataAsAlpha8(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, out_pixels, ref out_width, ref out_height, ref out_bytes_per_pixel);
    public void ImFontAtlas_GetTexDataAsRGBA32(IImFontAtlas self, byte** out_pixels, ref int out_width, ref int out_height, ref int out_bytes_per_pixel) => ImGuiMethods.ImFontAtlas_GetTexDataAsRGBA32(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, out_pixels, ref out_width, ref out_height, ref out_bytes_per_pixel);
    public void ImFontAtlas_SetTexID(IImFontAtlas self, ulong id) => ImGuiMethods.ImFontAtlas_SetTexID(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, id);
    public void ImFontAtlas_SetTexIDImTextureRef(IImFontAtlas self, IImTextureRef id) => ImGuiMethods.ImFontAtlas_SetTexIDImTextureRef(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, ((ImTextureRef)id).ToStruct());
    public bool ImFontAtlas_IsBuilt(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_IsBuilt(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public uint* ImFontAtlas_GetGlyphRangesDefault(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_GetGlyphRangesDefault(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public uint* ImFontAtlas_GetGlyphRangesGreek(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_GetGlyphRangesGreek(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public uint* ImFontAtlas_GetGlyphRangesKorean(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_GetGlyphRangesKorean(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public uint* ImFontAtlas_GetGlyphRangesJapanese(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_GetGlyphRangesJapanese(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public uint* ImFontAtlas_GetGlyphRangesChineseFull(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_GetGlyphRangesChineseFull(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public uint* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public uint* ImFontAtlas_GetGlyphRangesCyrillic(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_GetGlyphRangesCyrillic(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public uint* ImFontAtlas_GetGlyphRangesThai(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_GetGlyphRangesThai(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public uint* ImFontAtlas_GetGlyphRangesVietnamese(IImFontAtlas self) => ImGuiMethods.ImFontAtlas_GetGlyphRangesVietnamese(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null);
    public int ImFontAtlas_AddCustomRect(IImFontAtlas self, int width, int height, IImFontAtlasRect out_r) => ImGuiMethods.ImFontAtlas_AddCustomRect(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, width, height, out_r is not null ? (ImFontAtlasRectStruct*)out_r.NativePointer : null);
    public void ImFontAtlas_RemoveCustomRect(IImFontAtlas self, int id) => ImGuiMethods.ImFontAtlas_RemoveCustomRect(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, id);
    public bool ImFontAtlas_GetCustomRect(IImFontAtlas self, int id, IImFontAtlasRect out_r) => ImGuiMethods.ImFontAtlas_GetCustomRect(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, id, out_r is not null ? (ImFontAtlasRectStruct*)out_r.NativePointer : null);
    public int ImFontAtlas_AddCustomRectRegular(IImFontAtlas self, int w, int h) => ImGuiMethods.ImFontAtlas_AddCustomRectRegular(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, w, h);
    public IImFontAtlasRect ImFontAtlas_GetCustomRectByIndex(IImFontAtlas self, int id)
    {
        var ret = ImGuiMethods.ImFontAtlas_GetCustomRectByIndex(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, id);
        if (ret is null)
        return null !;
        return new ImFontAtlasRect(ret);
    }

    public void ImFontAtlas_CalcCustomRectUV(IImFontAtlas self, IImFontAtlasRect r, Vector2 out_uv_min, Vector2 out_uv_max) => ImGuiMethods.ImFontAtlas_CalcCustomRectUV(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, r is not null ? (ImFontAtlasRectStruct*)r.NativePointer : null, out_uv_min, out_uv_max);
    public int ImFontAtlas_AddCustomRectFontGlyph(IImFontAtlas self, IImFont font, uint codepoint, int w, int h, float advance_x, Vector2 offset) => ImGuiMethods.ImFontAtlas_AddCustomRectFontGlyph(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, font is not null ? (ImFontStruct*)font.NativePointer : null, codepoint, w, h, advance_x, offset);
    public int ImFontAtlas_AddCustomRectFontGlyphForSize(IImFontAtlas self, IImFont font, float font_size, uint codepoint, int w, int h, float advance_x, Vector2 offset) => ImGuiMethods.ImFontAtlas_AddCustomRectFontGlyphForSize(self is not null ? (ImFontAtlasStruct*)self.NativePointer : null, font is not null ? (ImFontStruct*)font.NativePointer : null, font_size, codepoint, w, h, advance_x, offset);
    public void ImFontBaked_ClearOutputData(IImFontBaked self) => ImGuiMethods.ImFontBaked_ClearOutputData(self is not null ? (ImFontBakedStruct*)self.NativePointer : null);
    public IImFontGlyph ImFontBaked_FindGlyph(IImFontBaked self, uint c)
    {
        var ret = ImGuiMethods.ImFontBaked_FindGlyph(self is not null ? (ImFontBakedStruct*)self.NativePointer : null, c);
        if (ret is null)
        return null !;
        return new ImFontGlyph(ret);
    }

    public IImFontGlyph ImFontBaked_FindGlyphNoFallback(IImFontBaked self, uint c)
    {
        var ret = ImGuiMethods.ImFontBaked_FindGlyphNoFallback(self is not null ? (ImFontBakedStruct*)self.NativePointer : null, c);
        if (ret is null)
        return null !;
        return new ImFontGlyph(ret);
    }

    public float ImFontBaked_GetCharAdvance(IImFontBaked self, uint c) => ImGuiMethods.ImFontBaked_GetCharAdvance(self is not null ? (ImFontBakedStruct*)self.NativePointer : null, c);
    public bool ImFontBaked_IsGlyphLoaded(IImFontBaked self, uint c) => ImGuiMethods.ImFontBaked_IsGlyphLoaded(self is not null ? (ImFontBakedStruct*)self.NativePointer : null, c);
    public bool ImFont_IsGlyphInFont(IImFont self, uint c) => ImGuiMethods.ImFont_IsGlyphInFont(self is not null ? (ImFontStruct*)self.NativePointer : null, c);
    public bool ImFont_IsLoaded(IImFont self) => ImGuiMethods.ImFont_IsLoaded(self is not null ? (ImFontStruct*)self.NativePointer : null);
    public string ImFont_GetDebugName(IImFont self)
    {
        sbyte* retStrPtr = ImGuiMethods.ImFont_GetDebugName(self is not null ? (ImFontStruct*)self.NativePointer : null);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public IImFontBaked ImFont_GetFontBaked(IImFont self, float font_size)
    {
        var ret = ImGuiMethods.ImFont_GetFontBaked(self is not null ? (ImFontStruct*)self.NativePointer : null, font_size);
        if (ret is null)
        return null !;
        return new ImFontBaked(ret);
    }

    public IImFontBaked ImFont_GetFontBakedEx(IImFont self, float font_size, float density)
    {
        var ret = ImGuiMethods.ImFont_GetFontBakedEx(self is not null ? (ImFontStruct*)self.NativePointer : null, font_size, density);
        if (ret is null)
        return null !;
        return new ImFontBaked(ret);
    }

    public Vector2 ImFont_CalcTextSizeA(IImFont self, float size, float max_width, float wrap_width, string text_begin) => ImGuiMethods.ImFont_CalcTextSizeA(self is not null ? (ImFontStruct*)self.NativePointer : null, size, max_width, wrap_width, text_begin);
    public Vector2 ImFont_CalcTextSizeAEx(IImFont self, float size, float max_width, float wrap_width, string text_begin, string text_end, sbyte** out_remaining) => ImGuiMethods.ImFont_CalcTextSizeAEx(self is not null ? (ImFontStruct*)self.NativePointer : null, size, max_width, wrap_width, text_begin, text_end, out_remaining);
    public string ImFont_CalcWordWrapPosition(IImFont self, float size, string text, string text_end, float wrap_width)
    {
        sbyte* retStrPtr = ImGuiMethods.ImFont_CalcWordWrapPosition(self is not null ? (ImFontStruct*)self.NativePointer : null, size, text, text_end, wrap_width);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public void ImFont_RenderChar(IImFont self, IImDrawList draw_list, float size, Vector2 pos, uint col, uint c) => ImGuiMethods.ImFont_RenderChar(self is not null ? (ImFontStruct*)self.NativePointer : null, draw_list is not null ? (ImDrawListStruct*)draw_list.NativePointer : null, size, pos, col, c);
    public void ImFont_RenderCharEx(IImFont self, IImDrawList draw_list, float size, Vector2 pos, uint col, uint c, Vector4 cpu_fine_clip) => ImGuiMethods.ImFont_RenderCharEx(self is not null ? (ImFontStruct*)self.NativePointer : null, draw_list is not null ? (ImDrawListStruct*)draw_list.NativePointer : null, size, pos, col, c, cpu_fine_clip);
    public void ImFont_RenderText(IImFont self, IImDrawList draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, string text_begin, string text_end, float wrap_width, int flags) => ImGuiMethods.ImFont_RenderText(self is not null ? (ImFontStruct*)self.NativePointer : null, draw_list is not null ? (ImDrawListStruct*)draw_list.NativePointer : null, size, pos, col, clip_rect, text_begin, text_end, wrap_width, flags);
    public string ImFont_CalcWordWrapPositionA(IImFont self, float scale, string text, string text_end, float wrap_width)
    {
        sbyte* retStrPtr = ImGuiMethods.ImFont_CalcWordWrapPositionA(self is not null ? (ImFontStruct*)self.NativePointer : null, scale, text, text_end, wrap_width);
        if (retStrPtr is null)
        return null !;
        string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;
        return retStr;
    }

    public void ImFont_ClearOutputData(IImFont self) => ImGuiMethods.ImFont_ClearOutputData(self is not null ? (ImFontStruct*)self.NativePointer : null);
    public void ImFont_AddRemapChar(IImFont self, uint from_codepoint, uint to_codepoint) => ImGuiMethods.ImFont_AddRemapChar(self is not null ? (ImFontStruct*)self.NativePointer : null, from_codepoint, to_codepoint);
    public bool ImFont_IsGlyphRangeUnused(IImFont self, uint c_begin, uint c_last) => ImGuiMethods.ImFont_IsGlyphRangeUnused(self is not null ? (ImFontStruct*)self.NativePointer : null, c_begin, c_last);
    public Vector2 ImGuiViewport_GetCenter(IImGuiViewport self) => ImGuiMethods.ImGuiViewport_GetCenter(self is not null ? (ImGuiViewportStruct*)self.NativePointer : null);
    public Vector2 ImGuiViewport_GetWorkCenter(IImGuiViewport self) => ImGuiMethods.ImGuiViewport_GetWorkCenter(self is not null ? (ImGuiViewportStruct*)self.NativePointer : null);
    public void ImGuiPlatformIO_ClearPlatformHandlers(IImGuiPlatformIO self) => ImGuiMethods.ImGuiPlatformIO_ClearPlatformHandlers(self is not null ? (ImGuiPlatformIOStruct*)self.NativePointer : null);
    public void ImGuiPlatformIO_ClearRendererHandlers(IImGuiPlatformIO self) => ImGuiMethods.ImGuiPlatformIO_ClearRendererHandlers(self is not null ? (ImGuiPlatformIOStruct*)self.NativePointer : null);
    public void PushFont(IImFont font) => ImGuiMethods.PushFont(font is not null ? (ImFontStruct*)font.NativePointer : null);
    public void SetWindowFontScale(float scale) => ImGuiMethods.SetWindowFontScale(scale);
    public void ImageImVec4(IImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col) => ImGuiMethods.ImageImVec4(((ImTextureRef)tex_ref).ToStruct(), image_size, uv0, uv1, tint_col, border_col);
    public void PushButtonRepeat(bool repeat) => ImGuiMethods.PushButtonRepeat(repeat);
    public void PopButtonRepeat() => ImGuiMethods.PopButtonRepeat();
    public void PushTabStop(bool tab_stop) => ImGuiMethods.PushTabStop(tab_stop);
    public void PopTabStop() => ImGuiMethods.PopTabStop();
    public Vector2 GetContentRegionMax() => ImGuiMethods.GetContentRegionMax();
    public Vector2 GetWindowContentRegionMin() => ImGuiMethods.GetWindowContentRegionMin();
    public Vector2 GetWindowContentRegionMax() => ImGuiMethods.GetWindowContentRegionMax();
    public bool BeginChildFrame(uint id, Vector2 size) => ImGuiMethods.BeginChildFrame(id, size);
    public bool BeginChildFrameEx(uint id, Vector2 size, ImGuiWindowFlags window_flags) => ImGuiMethods.BeginChildFrameEx(id, size, (int)window_flags);
    public void EndChildFrame() => ImGuiMethods.EndChildFrame();
    public void ShowStackToolWindow(ref bool p_open) => ImGuiMethods.ShowStackToolWindow(ref p_open);
    public bool ComboObsolete(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count) => ImGuiMethods.ComboObsolete(label, ref current_item, old_callback, user_data, items_count);
    public bool ComboObsoleteEx(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count, int popup_max_height_in_items) => ImGuiMethods.ComboObsoleteEx(label, ref current_item, old_callback, user_data, items_count, popup_max_height_in_items);
    public bool ListBoxObsolete(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count) => ImGuiMethods.ListBoxObsolete(label, ref current_item, old_callback, user_data, items_count);
    public bool ListBoxObsoleteEx(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count, int height_in_items) => ImGuiMethods.ListBoxObsoleteEx(label, ref current_item, old_callback, user_data, items_count, height_in_items);
    public unsafe partial struct ImDrawListSharedData : IImDrawListSharedData
    {
        public nint NativePointer { get; set; }

        public ImDrawListSharedData(ImDrawListSharedDataStruct* nativePtr) => NativePointer = (nint)nativePtr;
    }

    public unsafe partial struct ImFontAtlasBuilder : IImFontAtlasBuilder
    {
        public nint NativePointer { get; set; }

        public ImFontAtlasBuilder(ImFontAtlasBuilderStruct* nativePtr) => NativePointer = (nint)nativePtr;
    }

    public unsafe partial struct ImFontLoader : IImFontLoader
    {
        public nint NativePointer { get; set; }

        public ImFontLoader(ImFontLoaderStruct* nativePtr) => NativePointer = (nint)nativePtr;
    }

    public unsafe partial struct ImGuiContext : IImGuiContext
    {
        public nint NativePointer { get; set; }

        public ImGuiContext(ImGuiContextStruct* nativePtr) => NativePointer = (nint)nativePtr;
    }

    public unsafe partial struct ImGuiTableSortSpecs : IImGuiTableSortSpecs
    {
        public nint NativePointer { get; set; }

        public ImGuiTableSortSpecs(ImGuiTableSortSpecsStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImGuiTableColumnSortSpecs Specs => new ImGuiTableColumnSortSpecs(((ImGuiTableSortSpecsStruct*)NativePointer)->Specs);
        public readonly ref int SpecsCount => ref Unsafe.AsRef<int>(&((ImGuiTableSortSpecsStruct*)NativePointer)->SpecsCount);
        public readonly ref bool SpecsDirty => ref Unsafe.AsRef<bool>(&((ImGuiTableSortSpecsStruct*)NativePointer)->SpecsDirty);
    }

    public unsafe partial struct ImGuiTableColumnSortSpecs : IImGuiTableColumnSortSpecs
    {
        public nint NativePointer { get; set; }

        public ImGuiTableColumnSortSpecs(ImGuiTableColumnSortSpecsStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref uint ColumnUserID => ref Unsafe.AsRef<uint>(&((ImGuiTableColumnSortSpecsStruct*)NativePointer)->ColumnUserID);
        public readonly ref short ColumnIndex => ref Unsafe.AsRef<short>(&((ImGuiTableColumnSortSpecsStruct*)NativePointer)->ColumnIndex);
        public readonly ref short SortOrder => ref Unsafe.AsRef<short>(&((ImGuiTableColumnSortSpecsStruct*)NativePointer)->SortOrder);
        public readonly ref byte SortDirection => ref Unsafe.AsRef<byte>(&((ImGuiTableColumnSortSpecsStruct*)NativePointer)->SortDirection);
    }

    public unsafe partial struct ImGuiStyle : IImGuiStyle
    {
        public nint NativePointer { get; set; }

        public ImGuiStyle(ImGuiStyleStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref float FontSizeBase => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->FontSizeBase);
        public readonly ref float FontScaleMain => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->FontScaleMain);
        public readonly ref float FontScaleDpi => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->FontScaleDpi);
        public readonly ref float Alpha => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->Alpha);
        public readonly ref float DisabledAlpha => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->DisabledAlpha);
        public readonly ref Vector2 WindowPadding => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->WindowPadding);
        public readonly ref float WindowRounding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->WindowRounding);
        public readonly ref float WindowBorderSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->WindowBorderSize);
        public readonly ref float WindowBorderHoverPadding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->WindowBorderHoverPadding);
        public readonly ref Vector2 WindowMinSize => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->WindowMinSize);
        public readonly ref Vector2 WindowTitleAlign => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->WindowTitleAlign);
        public readonly ref int WindowMenuButtonPosition => ref Unsafe.AsRef<int>(&((ImGuiStyleStruct*)NativePointer)->WindowMenuButtonPosition);
        public readonly ref float ChildRounding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->ChildRounding);
        public readonly ref float ChildBorderSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->ChildBorderSize);
        public readonly ref float PopupRounding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->PopupRounding);
        public readonly ref float PopupBorderSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->PopupBorderSize);
        public readonly ref Vector2 FramePadding => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->FramePadding);
        public readonly ref float FrameRounding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->FrameRounding);
        public readonly ref float FrameBorderSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->FrameBorderSize);
        public readonly ref Vector2 ItemSpacing => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->ItemSpacing);
        public readonly ref Vector2 ItemInnerSpacing => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->ItemInnerSpacing);
        public readonly ref Vector2 CellPadding => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->CellPadding);
        public readonly ref Vector2 TouchExtraPadding => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->TouchExtraPadding);
        public readonly ref float IndentSpacing => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->IndentSpacing);
        public readonly ref float ColumnsMinSpacing => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->ColumnsMinSpacing);
        public readonly ref float ScrollbarSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->ScrollbarSize);
        public readonly ref float ScrollbarRounding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->ScrollbarRounding);
        public readonly ref float ScrollbarPadding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->ScrollbarPadding);
        public readonly ref float GrabMinSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->GrabMinSize);
        public readonly ref float GrabRounding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->GrabRounding);
        public readonly ref float LogSliderDeadzone => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->LogSliderDeadzone);
        public readonly ref float ImageBorderSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->ImageBorderSize);
        public readonly ref float TabRounding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TabRounding);
        public readonly ref float TabBorderSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TabBorderSize);
        public readonly ref float TabMinWidthBase => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TabMinWidthBase);
        public readonly ref float TabMinWidthShrink => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TabMinWidthShrink);
        public readonly ref float TabCloseButtonMinWidthSelected => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TabCloseButtonMinWidthSelected);
        public readonly ref float TabCloseButtonMinWidthUnselected => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TabCloseButtonMinWidthUnselected);
        public readonly ref float TabBarBorderSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TabBarBorderSize);
        public readonly ref float TabBarOverlineSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TabBarOverlineSize);
        public readonly ref float TableAngledHeadersAngle => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TableAngledHeadersAngle);
        public readonly ref Vector2 TableAngledHeadersTextAlign => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->TableAngledHeadersTextAlign);
        public readonly ref ImGuiTreeNodeFlags TreeLinesFlags => ref Unsafe.AsRef<ImGuiTreeNodeFlags>(&((ImGuiStyleStruct*)NativePointer)->TreeLinesFlags);
        public readonly ref float TreeLinesSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TreeLinesSize);
        public readonly ref float TreeLinesRounding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->TreeLinesRounding);
        public readonly ref float DragDropTargetRounding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->DragDropTargetRounding);
        public readonly ref float DragDropTargetBorderSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->DragDropTargetBorderSize);
        public readonly ref float DragDropTargetPadding => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->DragDropTargetPadding);
        public readonly ref int ColorButtonPosition => ref Unsafe.AsRef<int>(&((ImGuiStyleStruct*)NativePointer)->ColorButtonPosition);
        public readonly ref Vector2 ButtonTextAlign => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->ButtonTextAlign);
        public readonly ref Vector2 SelectableTextAlign => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->SelectableTextAlign);
        public readonly ref float SeparatorTextBorderSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->SeparatorTextBorderSize);
        public readonly ref Vector2 SeparatorTextAlign => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->SeparatorTextAlign);
        public readonly ref Vector2 SeparatorTextPadding => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->SeparatorTextPadding);
        public readonly ref Vector2 DisplayWindowPadding => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->DisplayWindowPadding);
        public readonly ref Vector2 DisplaySafeAreaPadding => ref Unsafe.AsRef<Vector2>(&((ImGuiStyleStruct*)NativePointer)->DisplaySafeAreaPadding);
        public readonly ref bool DockingNodeHasCloseButton => ref Unsafe.AsRef<bool>(&((ImGuiStyleStruct*)NativePointer)->DockingNodeHasCloseButton);
        public readonly ref float DockingSeparatorSize => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->DockingSeparatorSize);
        public readonly ref float MouseCursorScale => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->MouseCursorScale);
        public readonly ref bool AntiAliasedLines => ref Unsafe.AsRef<bool>(&((ImGuiStyleStruct*)NativePointer)->AntiAliasedLines);
        public readonly ref bool AntiAliasedLinesUseTex => ref Unsafe.AsRef<bool>(&((ImGuiStyleStruct*)NativePointer)->AntiAliasedLinesUseTex);
        public readonly ref bool AntiAliasedFill => ref Unsafe.AsRef<bool>(&((ImGuiStyleStruct*)NativePointer)->AntiAliasedFill);
        public readonly ref float CurveTessellationTol => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->CurveTessellationTol);
        public readonly ref float CircleTessellationMaxError => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->CircleTessellationMaxError);
        public readonly IRangeAccessor<Vector4> Colors => new RangeAccessor<Vector4>(&((ImGuiStyleStruct*)NativePointer)->Colors, 62);
        public readonly ref float HoverStationaryDelay => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->HoverStationaryDelay);
        public readonly ref float HoverDelayShort => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->HoverDelayShort);
        public readonly ref float HoverDelayNormal => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->HoverDelayNormal);
        public readonly ref ImGuiHoveredFlags HoverFlagsForTooltipMouse => ref Unsafe.AsRef<ImGuiHoveredFlags>(&((ImGuiStyleStruct*)NativePointer)->HoverFlagsForTooltipMouse);
        public readonly ref ImGuiHoveredFlags HoverFlagsForTooltipNav => ref Unsafe.AsRef<ImGuiHoveredFlags>(&((ImGuiStyleStruct*)NativePointer)->HoverFlagsForTooltipNav);
        public readonly ref float _MainScale => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->_MainScale);
        public readonly ref float _NextFrameFontSizeBase => ref Unsafe.AsRef<float>(&((ImGuiStyleStruct*)NativePointer)->_NextFrameFontSizeBase);
    }

    public unsafe partial struct ImGuiKeyData : IImGuiKeyData
    {
        public nint NativePointer { get; set; }

        public ImGuiKeyData(ImGuiKeyDataStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref bool Down => ref Unsafe.AsRef<bool>(&((ImGuiKeyDataStruct*)NativePointer)->Down);
        public readonly ref float DownDuration => ref Unsafe.AsRef<float>(&((ImGuiKeyDataStruct*)NativePointer)->DownDuration);
        public readonly ref float DownDurationPrev => ref Unsafe.AsRef<float>(&((ImGuiKeyDataStruct*)NativePointer)->DownDurationPrev);
        public readonly ref float AnalogValue => ref Unsafe.AsRef<float>(&((ImGuiKeyDataStruct*)NativePointer)->AnalogValue);
    }

    public unsafe partial struct ImGuiIO : IImGuiIO
    {
        public nint NativePointer { get; set; }

        public ImGuiIO(ImGuiIOStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref ImGuiConfigFlags ConfigFlags => ref Unsafe.AsRef<ImGuiConfigFlags>(&((ImGuiIOStruct*)NativePointer)->ConfigFlags);
        public readonly ref ImGuiBackendFlags BackendFlags => ref Unsafe.AsRef<ImGuiBackendFlags>(&((ImGuiIOStruct*)NativePointer)->BackendFlags);
        public readonly ref Vector2 DisplaySize => ref Unsafe.AsRef<Vector2>(&((ImGuiIOStruct*)NativePointer)->DisplaySize);
        public readonly ref Vector2 DisplayFramebufferScale => ref Unsafe.AsRef<Vector2>(&((ImGuiIOStruct*)NativePointer)->DisplayFramebufferScale);
        public readonly ref float DeltaTime => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->DeltaTime);
        public readonly ref float IniSavingRate => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->IniSavingRate);
        public readonly sbyte* IniFilename { get => ((ImGuiIOStruct*)NativePointer)->IniFilename; set => ((ImGuiIOStruct*)NativePointer)->IniFilename = value; }
        public readonly sbyte* LogFilename { get => ((ImGuiIOStruct*)NativePointer)->LogFilename; set => ((ImGuiIOStruct*)NativePointer)->LogFilename = value; }
        public readonly void* UserData { get => ((ImGuiIOStruct*)NativePointer)->UserData; set => ((ImGuiIOStruct*)NativePointer)->UserData = value; }
        public readonly IImFontAtlas Fonts => new ImFontAtlas(((ImGuiIOStruct*)NativePointer)->Fonts);
        public readonly IImFont FontDefault => new ImFont(((ImGuiIOStruct*)NativePointer)->FontDefault);
        public readonly ref bool FontAllowUserScaling => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->FontAllowUserScaling);
        public readonly ref bool ConfigNavSwapGamepadButtons => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigNavSwapGamepadButtons);
        public readonly ref bool ConfigNavMoveSetMousePos => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigNavMoveSetMousePos);
        public readonly ref bool ConfigNavCaptureKeyboard => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigNavCaptureKeyboard);
        public readonly ref bool ConfigNavEscapeClearFocusItem => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigNavEscapeClearFocusItem);
        public readonly ref bool ConfigNavEscapeClearFocusWindow => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigNavEscapeClearFocusWindow);
        public readonly ref bool ConfigNavCursorVisibleAuto => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigNavCursorVisibleAuto);
        public readonly ref bool ConfigNavCursorVisibleAlways => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigNavCursorVisibleAlways);
        public readonly ref bool ConfigDockingNoSplit => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDockingNoSplit);
        public readonly ref bool ConfigDockingNoDockingOver => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDockingNoDockingOver);
        public readonly ref bool ConfigDockingWithShift => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDockingWithShift);
        public readonly ref bool ConfigDockingAlwaysTabBar => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDockingAlwaysTabBar);
        public readonly ref bool ConfigDockingTransparentPayload => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDockingTransparentPayload);
        public readonly ref bool ConfigViewportsNoAutoMerge => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigViewportsNoAutoMerge);
        public readonly ref bool ConfigViewportsNoTaskBarIcon => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigViewportsNoTaskBarIcon);
        public readonly ref bool ConfigViewportsNoDecoration => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigViewportsNoDecoration);
        public readonly ref bool ConfigViewportsNoDefaultParent => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigViewportsNoDefaultParent);
        public readonly ref bool ConfigViewportsPlatformFocusSetsImGuiFocus => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigViewportsPlatformFocusSetsImGuiFocus);
        public readonly ref bool ConfigDpiScaleFonts => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDpiScaleFonts);
        public readonly ref bool ConfigDpiScaleViewports => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDpiScaleViewports);
        public readonly ref bool MouseDrawCursor => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->MouseDrawCursor);
        public readonly ref bool ConfigMacOSXBehaviors => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigMacOSXBehaviors);
        public readonly ref bool ConfigInputTrickleEventQueue => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigInputTrickleEventQueue);
        public readonly ref bool ConfigInputTextCursorBlink => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigInputTextCursorBlink);
        public readonly ref bool ConfigInputTextEnterKeepActive => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigInputTextEnterKeepActive);
        public readonly ref bool ConfigDragClickToInputText => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDragClickToInputText);
        public readonly ref bool ConfigWindowsResizeFromEdges => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigWindowsResizeFromEdges);
        public readonly ref bool ConfigWindowsMoveFromTitleBarOnly => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigWindowsMoveFromTitleBarOnly);
        public readonly ref bool ConfigWindowsCopyContentsWithCtrlC => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigWindowsCopyContentsWithCtrlC);
        public readonly ref bool ConfigScrollbarScrollByPage => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigScrollbarScrollByPage);
        public readonly ref float ConfigMemoryCompactTimer => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->ConfigMemoryCompactTimer);
        public readonly ref float MouseDoubleClickTime => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->MouseDoubleClickTime);
        public readonly ref float MouseDoubleClickMaxDist => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->MouseDoubleClickMaxDist);
        public readonly ref float MouseDragThreshold => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->MouseDragThreshold);
        public readonly ref float KeyRepeatDelay => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->KeyRepeatDelay);
        public readonly ref float KeyRepeatRate => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->KeyRepeatRate);
        public readonly ref bool ConfigErrorRecovery => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigErrorRecovery);
        public readonly ref bool ConfigErrorRecoveryEnableAssert => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigErrorRecoveryEnableAssert);
        public readonly ref bool ConfigErrorRecoveryEnableDebugLog => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigErrorRecoveryEnableDebugLog);
        public readonly ref bool ConfigErrorRecoveryEnableTooltip => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigErrorRecoveryEnableTooltip);
        public readonly ref bool ConfigDebugIsDebuggerPresent => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDebugIsDebuggerPresent);
        public readonly ref bool ConfigDebugHighlightIdConflicts => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDebugHighlightIdConflicts);
        public readonly ref bool ConfigDebugHighlightIdConflictsShowItemPicker => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDebugHighlightIdConflictsShowItemPicker);
        public readonly ref bool ConfigDebugBeginReturnValueOnce => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDebugBeginReturnValueOnce);
        public readonly ref bool ConfigDebugBeginReturnValueLoop => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDebugBeginReturnValueLoop);
        public readonly ref bool ConfigDebugIgnoreFocusLoss => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDebugIgnoreFocusLoss);
        public readonly ref bool ConfigDebugIniSettings => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->ConfigDebugIniSettings);
        public readonly sbyte* BackendPlatformName { get => ((ImGuiIOStruct*)NativePointer)->BackendPlatformName; set => ((ImGuiIOStruct*)NativePointer)->BackendPlatformName = value; }
        public readonly sbyte* BackendRendererName { get => ((ImGuiIOStruct*)NativePointer)->BackendRendererName; set => ((ImGuiIOStruct*)NativePointer)->BackendRendererName = value; }
        public readonly void* BackendPlatformUserData { get => ((ImGuiIOStruct*)NativePointer)->BackendPlatformUserData; set => ((ImGuiIOStruct*)NativePointer)->BackendPlatformUserData = value; }
        public readonly void* BackendRendererUserData { get => ((ImGuiIOStruct*)NativePointer)->BackendRendererUserData; set => ((ImGuiIOStruct*)NativePointer)->BackendRendererUserData = value; }
        public readonly void* BackendLanguageUserData { get => ((ImGuiIOStruct*)NativePointer)->BackendLanguageUserData; set => ((ImGuiIOStruct*)NativePointer)->BackendLanguageUserData = value; }
        public readonly ref bool WantCaptureMouse => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->WantCaptureMouse);
        public readonly ref bool WantCaptureKeyboard => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->WantCaptureKeyboard);
        public readonly ref bool WantTextInput => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->WantTextInput);
        public readonly ref bool WantSetMousePos => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->WantSetMousePos);
        public readonly ref bool WantSaveIniSettings => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->WantSaveIniSettings);
        public readonly ref bool NavActive => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->NavActive);
        public readonly ref bool NavVisible => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->NavVisible);
        public readonly ref float Framerate => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->Framerate);
        public readonly ref int MetricsRenderVertices => ref Unsafe.AsRef<int>(&((ImGuiIOStruct*)NativePointer)->MetricsRenderVertices);
        public readonly ref int MetricsRenderIndices => ref Unsafe.AsRef<int>(&((ImGuiIOStruct*)NativePointer)->MetricsRenderIndices);
        public readonly ref int MetricsRenderWindows => ref Unsafe.AsRef<int>(&((ImGuiIOStruct*)NativePointer)->MetricsRenderWindows);
        public readonly ref int MetricsActiveWindows => ref Unsafe.AsRef<int>(&((ImGuiIOStruct*)NativePointer)->MetricsActiveWindows);
        public readonly ref Vector2 MouseDelta => ref Unsafe.AsRef<Vector2>(&((ImGuiIOStruct*)NativePointer)->MouseDelta);
        public readonly IImGuiContext Ctx => new ImGuiContext(((ImGuiIOStruct*)NativePointer)->Ctx);
        public readonly ref Vector2 MousePos => ref Unsafe.AsRef<Vector2>(&((ImGuiIOStruct*)NativePointer)->MousePos);
        public readonly IRangeAccessor<bool> MouseDown => new RangeAccessor<bool>(&((ImGuiIOStruct*)NativePointer)->MouseDown, 5);
        public readonly ref float MouseWheel => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->MouseWheel);
        public readonly ref float MouseWheelH => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->MouseWheelH);
        public readonly ref int MouseSource => ref Unsafe.AsRef<int>(&((ImGuiIOStruct*)NativePointer)->MouseSource);
        public readonly ref uint MouseHoveredViewport => ref Unsafe.AsRef<uint>(&((ImGuiIOStruct*)NativePointer)->MouseHoveredViewport);
        public readonly ref bool KeyCtrl => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->KeyCtrl);
        public readonly ref bool KeyShift => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->KeyShift);
        public readonly ref bool KeyAlt => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->KeyAlt);
        public readonly ref bool KeySuper => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->KeySuper);
        public readonly ref int KeyMods => ref Unsafe.AsRef<int>(&((ImGuiIOStruct*)NativePointer)->KeyMods);
        public readonly IRangeStructAccessor<IImGuiKeyData> KeysData => new RangeStructAccessor<IImGuiKeyData>(&((ImGuiIOStruct*)NativePointer)->KeysData, 155, Unsafe.SizeOf<ImGuiKeyDataStruct>(), (addr) => new ImGuiKeyData((ImGuiKeyDataStruct*)addr));
        public readonly ref bool WantCaptureMouseUnlessPopupClose => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->WantCaptureMouseUnlessPopupClose);
        public readonly ref Vector2 MousePosPrev => ref Unsafe.AsRef<Vector2>(&((ImGuiIOStruct*)NativePointer)->MousePosPrev);
        public readonly IRangeAccessor<Vector2> MouseClickedPos => new RangeAccessor<Vector2>(&((ImGuiIOStruct*)NativePointer)->MouseClickedPos, 5);
        public readonly IRangeAccessor<double> MouseClickedTime => new RangeAccessor<double>(&((ImGuiIOStruct*)NativePointer)->MouseClickedTime, 5);
        public readonly IRangeAccessor<bool> MouseClicked => new RangeAccessor<bool>(&((ImGuiIOStruct*)NativePointer)->MouseClicked, 5);
        public readonly IRangeAccessor<bool> MouseDoubleClicked => new RangeAccessor<bool>(&((ImGuiIOStruct*)NativePointer)->MouseDoubleClicked, 5);
        public readonly IRangeAccessor<ushort> MouseClickedCount => new RangeAccessor<ushort>(&((ImGuiIOStruct*)NativePointer)->MouseClickedCount, 5);
        public readonly IRangeAccessor<ushort> MouseClickedLastCount => new RangeAccessor<ushort>(&((ImGuiIOStruct*)NativePointer)->MouseClickedLastCount, 5);
        public readonly IRangeAccessor<bool> MouseReleased => new RangeAccessor<bool>(&((ImGuiIOStruct*)NativePointer)->MouseReleased, 5);
        public readonly IRangeAccessor<double> MouseReleasedTime => new RangeAccessor<double>(&((ImGuiIOStruct*)NativePointer)->MouseReleasedTime, 5);
        public readonly IRangeAccessor<bool> MouseDownOwned => new RangeAccessor<bool>(&((ImGuiIOStruct*)NativePointer)->MouseDownOwned, 5);
        public readonly IRangeAccessor<bool> MouseDownOwnedUnlessPopupClose => new RangeAccessor<bool>(&((ImGuiIOStruct*)NativePointer)->MouseDownOwnedUnlessPopupClose, 5);
        public readonly ref bool MouseWheelRequestAxisSwap => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->MouseWheelRequestAxisSwap);
        public readonly ref bool MouseCtrlLeftAsRightClick => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->MouseCtrlLeftAsRightClick);
        public readonly IRangeAccessor<float> MouseDownDuration => new RangeAccessor<float>(&((ImGuiIOStruct*)NativePointer)->MouseDownDuration, 5);
        public readonly IRangeAccessor<float> MouseDownDurationPrev => new RangeAccessor<float>(&((ImGuiIOStruct*)NativePointer)->MouseDownDurationPrev, 5);
        public readonly IRangeAccessor<Vector2> MouseDragMaxDistanceAbs => new RangeAccessor<Vector2>(&((ImGuiIOStruct*)NativePointer)->MouseDragMaxDistanceAbs, 5);
        public readonly IRangeAccessor<float> MouseDragMaxDistanceSqr => new RangeAccessor<float>(&((ImGuiIOStruct*)NativePointer)->MouseDragMaxDistanceSqr, 5);
        public readonly ref float PenPressure => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->PenPressure);
        public readonly ref bool AppFocusLost => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->AppFocusLost);
        public readonly ref bool AppAcceptingEvents => ref Unsafe.AsRef<bool>(&((ImGuiIOStruct*)NativePointer)->AppAcceptingEvents);
        public readonly ref ushort InputQueueSurrogate => ref Unsafe.AsRef<ushort>(&((ImGuiIOStruct*)NativePointer)->InputQueueSurrogate);
        public readonly IImVectorWrapper<uint> InputQueueCharacters => new ImVectorWrapper<uint>(Unsafe.Read<ImVector>(&((ImGuiIOStruct*)NativePointer)->InputQueueCharacters), Unsafe.SizeOf<uint>(), (addr) => *(uint*)addr);
        public readonly ref float FontGlobalScale => ref Unsafe.AsRef<float>(&((ImGuiIOStruct*)NativePointer)->FontGlobalScale);
        public readonly delegate* unmanaged[Cdecl]<nint, nint> GetClipboardTextFn { get => ((ImGuiIOStruct*)NativePointer)->GetClipboardTextFn; set => ((ImGuiIOStruct*)NativePointer)->GetClipboardTextFn = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> SetClipboardTextFn { get => ((ImGuiIOStruct*)NativePointer)->SetClipboardTextFn; set => ((ImGuiIOStruct*)NativePointer)->SetClipboardTextFn = value; }
        public readonly void* ClipboardUserData { get => ((ImGuiIOStruct*)NativePointer)->ClipboardUserData; set => ((ImGuiIOStruct*)NativePointer)->ClipboardUserData = value; }
    }

    public unsafe partial struct ImGuiInputTextCallbackData : IImGuiInputTextCallbackData
    {
        public nint NativePointer { get; set; }

        public ImGuiInputTextCallbackData(ImGuiInputTextCallbackDataStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImGuiContext Ctx => new ImGuiContext(((ImGuiInputTextCallbackDataStruct*)NativePointer)->Ctx);
        public readonly ref ImGuiInputTextFlags EventFlag => ref Unsafe.AsRef<ImGuiInputTextFlags>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->EventFlag);
        public readonly ref ImGuiInputTextFlags Flags => ref Unsafe.AsRef<ImGuiInputTextFlags>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->Flags);
        public readonly void* UserData { get => ((ImGuiInputTextCallbackDataStruct*)NativePointer)->UserData; set => ((ImGuiInputTextCallbackDataStruct*)NativePointer)->UserData = value; }
        public readonly ref uint EventChar => ref Unsafe.AsRef<uint>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->EventChar);
        public readonly ref int EventKey => ref Unsafe.AsRef<int>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->EventKey);
        public readonly sbyte* Buf { get => ((ImGuiInputTextCallbackDataStruct*)NativePointer)->Buf; set => ((ImGuiInputTextCallbackDataStruct*)NativePointer)->Buf = value; }
        public readonly ref int BufTextLen => ref Unsafe.AsRef<int>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->BufTextLen);
        public readonly ref int BufSize => ref Unsafe.AsRef<int>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->BufSize);
        public readonly ref bool BufDirty => ref Unsafe.AsRef<bool>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->BufDirty);
        public readonly ref int CursorPos => ref Unsafe.AsRef<int>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->CursorPos);
        public readonly ref int SelectionStart => ref Unsafe.AsRef<int>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->SelectionStart);
        public readonly ref int SelectionEnd => ref Unsafe.AsRef<int>(&((ImGuiInputTextCallbackDataStruct*)NativePointer)->SelectionEnd);
    }

    public unsafe partial struct ImGuiSizeCallbackData : IImGuiSizeCallbackData
    {
        public nint NativePointer { get; set; }

        public ImGuiSizeCallbackData(ImGuiSizeCallbackDataStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly void* UserData { get => ((ImGuiSizeCallbackDataStruct*)NativePointer)->UserData; set => ((ImGuiSizeCallbackDataStruct*)NativePointer)->UserData = value; }
        public readonly ref Vector2 Pos => ref Unsafe.AsRef<Vector2>(&((ImGuiSizeCallbackDataStruct*)NativePointer)->Pos);
        public readonly ref Vector2 CurrentSize => ref Unsafe.AsRef<Vector2>(&((ImGuiSizeCallbackDataStruct*)NativePointer)->CurrentSize);
        public readonly ref Vector2 DesiredSize => ref Unsafe.AsRef<Vector2>(&((ImGuiSizeCallbackDataStruct*)NativePointer)->DesiredSize);
    }

    public unsafe partial struct ImGuiWindowClass : IImGuiWindowClass
    {
        public nint NativePointer { get; set; }

        public ImGuiWindowClass(ImGuiWindowClassStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref uint ClassId => ref Unsafe.AsRef<uint>(&((ImGuiWindowClassStruct*)NativePointer)->ClassId);
        public readonly ref uint ParentViewportId => ref Unsafe.AsRef<uint>(&((ImGuiWindowClassStruct*)NativePointer)->ParentViewportId);
        public readonly ref uint FocusRouteParentWindowId => ref Unsafe.AsRef<uint>(&((ImGuiWindowClassStruct*)NativePointer)->FocusRouteParentWindowId);
        public readonly ref ImGuiViewportFlags ViewportFlagsOverrideSet => ref Unsafe.AsRef<ImGuiViewportFlags>(&((ImGuiWindowClassStruct*)NativePointer)->ViewportFlagsOverrideSet);
        public readonly ref ImGuiViewportFlags ViewportFlagsOverrideClear => ref Unsafe.AsRef<ImGuiViewportFlags>(&((ImGuiWindowClassStruct*)NativePointer)->ViewportFlagsOverrideClear);
        public readonly ref ImGuiTabItemFlags TabItemFlagsOverrideSet => ref Unsafe.AsRef<ImGuiTabItemFlags>(&((ImGuiWindowClassStruct*)NativePointer)->TabItemFlagsOverrideSet);
        public readonly ref ImGuiDockNodeFlags DockNodeFlagsOverrideSet => ref Unsafe.AsRef<ImGuiDockNodeFlags>(&((ImGuiWindowClassStruct*)NativePointer)->DockNodeFlagsOverrideSet);
        public readonly ref bool DockingAlwaysTabBar => ref Unsafe.AsRef<bool>(&((ImGuiWindowClassStruct*)NativePointer)->DockingAlwaysTabBar);
        public readonly ref bool DockingAllowUnclassed => ref Unsafe.AsRef<bool>(&((ImGuiWindowClassStruct*)NativePointer)->DockingAllowUnclassed);
    }

    public unsafe partial struct ImGuiPayload : IImGuiPayload
    {
        public nint NativePointer { get; set; }

        public ImGuiPayload(ImGuiPayloadStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly void* Data { get => ((ImGuiPayloadStruct*)NativePointer)->Data; set => ((ImGuiPayloadStruct*)NativePointer)->Data = value; }
        public readonly ref int DataSize => ref Unsafe.AsRef<int>(&((ImGuiPayloadStruct*)NativePointer)->DataSize);
        public readonly ref uint SourceId => ref Unsafe.AsRef<uint>(&((ImGuiPayloadStruct*)NativePointer)->SourceId);
        public readonly ref uint SourceParentId => ref Unsafe.AsRef<uint>(&((ImGuiPayloadStruct*)NativePointer)->SourceParentId);
        public readonly ref int DataFrameCount => ref Unsafe.AsRef<int>(&((ImGuiPayloadStruct*)NativePointer)->DataFrameCount);
        public readonly IRangeAccessor<byte> DataType => new RangeAccessor<byte>(&((ImGuiPayloadStruct*)NativePointer)->DataType, 33);
        public readonly ref bool Preview => ref Unsafe.AsRef<bool>(&((ImGuiPayloadStruct*)NativePointer)->Preview);
        public readonly ref bool Delivery => ref Unsafe.AsRef<bool>(&((ImGuiPayloadStruct*)NativePointer)->Delivery);
    }

    public unsafe partial struct ImGuiTextFilter_ImGuiTextRange : IImGuiTextFilter_ImGuiTextRange
    {
        public nint NativePointer { get; set; }

        public ImGuiTextFilter_ImGuiTextRange(ImGuiTextFilter_ImGuiTextRangeStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly sbyte* b { get => ((ImGuiTextFilter_ImGuiTextRangeStruct*)NativePointer)->b; set => ((ImGuiTextFilter_ImGuiTextRangeStruct*)NativePointer)->b = value; }
        public readonly sbyte* e { get => ((ImGuiTextFilter_ImGuiTextRangeStruct*)NativePointer)->e; set => ((ImGuiTextFilter_ImGuiTextRangeStruct*)NativePointer)->e = value; }
    }

    public unsafe partial struct ImGuiTextFilter : IImGuiTextFilter
    {
        public nint NativePointer { get; set; }

        public ImGuiTextFilter(ImGuiTextFilterStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IRangeAccessor<byte> InputBuf => new RangeAccessor<byte>(&((ImGuiTextFilterStruct*)NativePointer)->InputBuf, 256);
        public readonly IImVectorWrapper<IImGuiTextFilter_ImGuiTextRange> Filters => new ImVectorWrapper<IImGuiTextFilter_ImGuiTextRange>(Unsafe.Read<ImVector>(&((ImGuiTextFilterStruct*)NativePointer)->Filters), Unsafe.SizeOf<ImGuiTextFilter_ImGuiTextRangeStruct>(), (addr) => new ImGuiTextFilter_ImGuiTextRange((ImGuiTextFilter_ImGuiTextRangeStruct*)addr));
        public readonly ref int CountGrep => ref Unsafe.AsRef<int>(&((ImGuiTextFilterStruct*)NativePointer)->CountGrep);
    }

    public unsafe partial struct ImGuiTextBuffer : IImGuiTextBuffer
    {
        public nint NativePointer { get; set; }

        public ImGuiTextBuffer(ImGuiTextBufferStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImVectorWrapper<byte> Buf => new ImVectorWrapper<byte>(Unsafe.Read<ImVector>(&((ImGuiTextBufferStruct*)NativePointer)->Buf), Unsafe.SizeOf<byte>(), (addr) => *(byte*)addr);
    }

    public unsafe partial struct ImGuiStorage : IImGuiStorage
    {
        public nint NativePointer { get; set; }

        public ImGuiStorage(ImGuiStorageStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImVectorWrapper<IImGuiStoragePair> Data => new ImVectorWrapper<IImGuiStoragePair>(Unsafe.Read<ImVector>(&((ImGuiStorageStruct*)NativePointer)->Data), Unsafe.SizeOf<ImGuiStoragePairStruct>(), (addr) => new ImGuiStoragePair((ImGuiStoragePairStruct*)addr));
    }

    public unsafe partial struct ImGuiListClipper : IImGuiListClipper
    {
        public nint NativePointer { get; set; }

        public ImGuiListClipper(ImGuiListClipperStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImGuiContext Ctx => new ImGuiContext(((ImGuiListClipperStruct*)NativePointer)->Ctx);
        public readonly ref int DisplayStart => ref Unsafe.AsRef<int>(&((ImGuiListClipperStruct*)NativePointer)->DisplayStart);
        public readonly ref int DisplayEnd => ref Unsafe.AsRef<int>(&((ImGuiListClipperStruct*)NativePointer)->DisplayEnd);
        public readonly ref int ItemsCount => ref Unsafe.AsRef<int>(&((ImGuiListClipperStruct*)NativePointer)->ItemsCount);
        public readonly ref float ItemsHeight => ref Unsafe.AsRef<float>(&((ImGuiListClipperStruct*)NativePointer)->ItemsHeight);
        public readonly ref double StartPosY => ref Unsafe.AsRef<double>(&((ImGuiListClipperStruct*)NativePointer)->StartPosY);
        public readonly ref double StartSeekOffsetY => ref Unsafe.AsRef<double>(&((ImGuiListClipperStruct*)NativePointer)->StartSeekOffsetY);
        public readonly void* TempData { get => ((ImGuiListClipperStruct*)NativePointer)->TempData; set => ((ImGuiListClipperStruct*)NativePointer)->TempData = value; }
        public readonly ref int Flags => ref Unsafe.AsRef<int>(&((ImGuiListClipperStruct*)NativePointer)->Flags);
    }

    public unsafe partial struct ImGuiMultiSelectIO : IImGuiMultiSelectIO
    {
        public nint NativePointer { get; set; }

        public ImGuiMultiSelectIO(ImGuiMultiSelectIOStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImVectorWrapper<IImGuiSelectionRequest> Requests => new ImVectorWrapper<IImGuiSelectionRequest>(Unsafe.Read<ImVector>(&((ImGuiMultiSelectIOStruct*)NativePointer)->Requests), Unsafe.SizeOf<ImGuiSelectionRequestStruct>(), (addr) => new ImGuiSelectionRequest((ImGuiSelectionRequestStruct*)addr));
        public readonly ref long RangeSrcItem => ref Unsafe.AsRef<long>(&((ImGuiMultiSelectIOStruct*)NativePointer)->RangeSrcItem);
        public readonly ref long NavIdItem => ref Unsafe.AsRef<long>(&((ImGuiMultiSelectIOStruct*)NativePointer)->NavIdItem);
        public readonly ref bool NavIdSelected => ref Unsafe.AsRef<bool>(&((ImGuiMultiSelectIOStruct*)NativePointer)->NavIdSelected);
        public readonly ref bool RangeSrcReset => ref Unsafe.AsRef<bool>(&((ImGuiMultiSelectIOStruct*)NativePointer)->RangeSrcReset);
        public readonly ref int ItemsCount => ref Unsafe.AsRef<int>(&((ImGuiMultiSelectIOStruct*)NativePointer)->ItemsCount);
    }

    public unsafe partial struct ImGuiSelectionRequest : IImGuiSelectionRequest
    {
        public nint NativePointer { get; set; }

        public ImGuiSelectionRequest(ImGuiSelectionRequestStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref ImGuiSelectionRequestType Type => ref Unsafe.AsRef<ImGuiSelectionRequestType>(&((ImGuiSelectionRequestStruct*)NativePointer)->Type);
        public readonly ref bool Selected => ref Unsafe.AsRef<bool>(&((ImGuiSelectionRequestStruct*)NativePointer)->Selected);
        public readonly ref sbyte RangeDirection => ref Unsafe.AsRef<sbyte>(&((ImGuiSelectionRequestStruct*)NativePointer)->RangeDirection);
        public readonly ref long RangeFirstItem => ref Unsafe.AsRef<long>(&((ImGuiSelectionRequestStruct*)NativePointer)->RangeFirstItem);
        public readonly ref long RangeLastItem => ref Unsafe.AsRef<long>(&((ImGuiSelectionRequestStruct*)NativePointer)->RangeLastItem);
    }

    public unsafe partial struct ImGuiSelectionBasicStorage : IImGuiSelectionBasicStorage
    {
        public nint NativePointer { get; set; }

        public ImGuiSelectionBasicStorage(ImGuiSelectionBasicStorageStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref int Size => ref Unsafe.AsRef<int>(&((ImGuiSelectionBasicStorageStruct*)NativePointer)->Size);
        public readonly ref bool PreserveOrder => ref Unsafe.AsRef<bool>(&((ImGuiSelectionBasicStorageStruct*)NativePointer)->PreserveOrder);
        public readonly void* UserData { get => ((ImGuiSelectionBasicStorageStruct*)NativePointer)->UserData; set => ((ImGuiSelectionBasicStorageStruct*)NativePointer)->UserData = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, int, uint> AdapterIndexToStorageId { get => ((ImGuiSelectionBasicStorageStruct*)NativePointer)->AdapterIndexToStorageId; set => ((ImGuiSelectionBasicStorageStruct*)NativePointer)->AdapterIndexToStorageId = value; }
        public readonly ref int _SelectionOrder => ref Unsafe.AsRef<int>(&((ImGuiSelectionBasicStorageStruct*)NativePointer)->_SelectionOrder);
        public readonly IImGuiStorage _Storage => new ImGuiStorage(&((ImGuiSelectionBasicStorageStruct*)NativePointer)->_Storage);
    }

    public unsafe partial struct ImGuiSelectionExternalStorage : IImGuiSelectionExternalStorage
    {
        public nint NativePointer { get; set; }

        public ImGuiSelectionExternalStorage(ImGuiSelectionExternalStorageStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly void* UserData { get => ((ImGuiSelectionExternalStorageStruct*)NativePointer)->UserData; set => ((ImGuiSelectionExternalStorageStruct*)NativePointer)->UserData = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, int, byte, void> AdapterSetItemSelected { get => ((ImGuiSelectionExternalStorageStruct*)NativePointer)->AdapterSetItemSelected; set => ((ImGuiSelectionExternalStorageStruct*)NativePointer)->AdapterSetItemSelected = value; }
    }

    public unsafe partial struct ImDrawCmd : IImDrawCmd
    {
        public nint NativePointer { get; set; }

        public ImDrawCmd(ImDrawCmdStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref Vector4 ClipRect => ref Unsafe.AsRef<Vector4>(&((ImDrawCmdStruct*)NativePointer)->ClipRect);
        public readonly IImTextureRef TexRef => new ImTextureRef(&((ImDrawCmdStruct*)NativePointer)->TexRef);
        public readonly ref uint VtxOffset => ref Unsafe.AsRef<uint>(&((ImDrawCmdStruct*)NativePointer)->VtxOffset);
        public readonly ref uint IdxOffset => ref Unsafe.AsRef<uint>(&((ImDrawCmdStruct*)NativePointer)->IdxOffset);
        public readonly ref uint ElemCount => ref Unsafe.AsRef<uint>(&((ImDrawCmdStruct*)NativePointer)->ElemCount);
        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> UserCallback { get => ((ImDrawCmdStruct*)NativePointer)->UserCallback; set => ((ImDrawCmdStruct*)NativePointer)->UserCallback = value; }
        public readonly void* UserCallbackData { get => ((ImDrawCmdStruct*)NativePointer)->UserCallbackData; set => ((ImDrawCmdStruct*)NativePointer)->UserCallbackData = value; }
        public readonly ref int UserCallbackDataSize => ref Unsafe.AsRef<int>(&((ImDrawCmdStruct*)NativePointer)->UserCallbackDataSize);
        public readonly ref int UserCallbackDataOffset => ref Unsafe.AsRef<int>(&((ImDrawCmdStruct*)NativePointer)->UserCallbackDataOffset);
    }

    public unsafe partial struct ImDrawVert : IImDrawVert
    {
        public nint NativePointer { get; set; }

        public ImDrawVert(ImDrawVertStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref Vector2 pos => ref Unsafe.AsRef<Vector2>(&((ImDrawVertStruct*)NativePointer)->pos);
        public readonly ref Vector2 uv => ref Unsafe.AsRef<Vector2>(&((ImDrawVertStruct*)NativePointer)->uv);
        public readonly ref uint col => ref Unsafe.AsRef<uint>(&((ImDrawVertStruct*)NativePointer)->col);
    }

    public unsafe partial struct ImDrawCmdHeader : IImDrawCmdHeader
    {
        public nint NativePointer { get; set; }

        public ImDrawCmdHeader(ImDrawCmdHeaderStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref Vector4 ClipRect => ref Unsafe.AsRef<Vector4>(&((ImDrawCmdHeaderStruct*)NativePointer)->ClipRect);
        public readonly IImTextureRef TexRef => new ImTextureRef(&((ImDrawCmdHeaderStruct*)NativePointer)->TexRef);
        public readonly ref uint VtxOffset => ref Unsafe.AsRef<uint>(&((ImDrawCmdHeaderStruct*)NativePointer)->VtxOffset);
    }

    public unsafe partial struct ImDrawChannel : IImDrawChannel
    {
        public nint NativePointer { get; set; }

        public ImDrawChannel(ImDrawChannelStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImVectorWrapper<IImDrawCmd> _CmdBuffer => new ImVectorWrapper<IImDrawCmd>(Unsafe.Read<ImVector>(&((ImDrawChannelStruct*)NativePointer)->_CmdBuffer), Unsafe.SizeOf<ImDrawCmdStruct>(), (addr) => new ImDrawCmd((ImDrawCmdStruct*)addr));
        public readonly IImVectorWrapper<ushort> _IdxBuffer => new ImVectorWrapper<ushort>(Unsafe.Read<ImVector>(&((ImDrawChannelStruct*)NativePointer)->_IdxBuffer), Unsafe.SizeOf<ushort>(), (addr) => *(ushort*)addr);
    }

    public unsafe partial struct ImDrawListSplitter : IImDrawListSplitter
    {
        public nint NativePointer { get; set; }

        public ImDrawListSplitter(ImDrawListSplitterStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref int _Current => ref Unsafe.AsRef<int>(&((ImDrawListSplitterStruct*)NativePointer)->_Current);
        public readonly ref int _Count => ref Unsafe.AsRef<int>(&((ImDrawListSplitterStruct*)NativePointer)->_Count);
        public readonly IImVectorWrapper<IImDrawChannel> _Channels => new ImVectorWrapper<IImDrawChannel>(Unsafe.Read<ImVector>(&((ImDrawListSplitterStruct*)NativePointer)->_Channels), Unsafe.SizeOf<ImDrawChannelStruct>(), (addr) => new ImDrawChannel((ImDrawChannelStruct*)addr));
    }

    public unsafe partial struct ImDrawList : IImDrawList
    {
        public nint NativePointer { get; set; }

        public ImDrawList(ImDrawListStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImVectorWrapper<IImDrawCmd> CmdBuffer => new ImVectorWrapper<IImDrawCmd>(Unsafe.Read<ImVector>(&((ImDrawListStruct*)NativePointer)->CmdBuffer), Unsafe.SizeOf<ImDrawCmdStruct>(), (addr) => new ImDrawCmd((ImDrawCmdStruct*)addr));
        public readonly IImVectorWrapper<ushort> IdxBuffer => new ImVectorWrapper<ushort>(Unsafe.Read<ImVector>(&((ImDrawListStruct*)NativePointer)->IdxBuffer), Unsafe.SizeOf<ushort>(), (addr) => *(ushort*)addr);
        public readonly IImVectorWrapper<IImDrawVert> VtxBuffer => new ImVectorWrapper<IImDrawVert>(Unsafe.Read<ImVector>(&((ImDrawListStruct*)NativePointer)->VtxBuffer), Unsafe.SizeOf<ImDrawVertStruct>(), (addr) => new ImDrawVert((ImDrawVertStruct*)addr));
        public readonly ref ImDrawListFlags Flags => ref Unsafe.AsRef<ImDrawListFlags>(&((ImDrawListStruct*)NativePointer)->Flags);
        public readonly ref uint _VtxCurrentIdx => ref Unsafe.AsRef<uint>(&((ImDrawListStruct*)NativePointer)->_VtxCurrentIdx);
        public readonly nint _Data { get => ((ImDrawListStruct*)NativePointer)->_Data; set => ((ImDrawListStruct*)NativePointer)->_Data = value; }
        public readonly IImDrawVert _VtxWritePtr => new ImDrawVert(((ImDrawListStruct*)NativePointer)->_VtxWritePtr);
        public readonly ushort* _IdxWritePtr { get => ((ImDrawListStruct*)NativePointer)->_IdxWritePtr; set => ((ImDrawListStruct*)NativePointer)->_IdxWritePtr = value; }
        public readonly IImVectorWrapper<Vector2> _Path => new ImVectorWrapper<Vector2>(Unsafe.Read<ImVector>(&((ImDrawListStruct*)NativePointer)->_Path), Unsafe.SizeOf<Vector2>(), (addr) => *(Vector2*)addr);
        public readonly IImDrawCmdHeader _CmdHeader => new ImDrawCmdHeader(&((ImDrawListStruct*)NativePointer)->_CmdHeader);
        public readonly IImDrawListSplitter _Splitter => new ImDrawListSplitter(&((ImDrawListStruct*)NativePointer)->_Splitter);
        public readonly IImVectorWrapper<Vector4> _ClipRectStack => new ImVectorWrapper<Vector4>(Unsafe.Read<ImVector>(&((ImDrawListStruct*)NativePointer)->_ClipRectStack), Unsafe.SizeOf<Vector4>(), (addr) => *(Vector4*)addr);
        public readonly IImVectorWrapper<IImTextureRef> _TextureStack => new ImVectorWrapper<IImTextureRef>(Unsafe.Read<ImVector>(&((ImDrawListStruct*)NativePointer)->_TextureStack), Unsafe.SizeOf<ImTextureRefStruct>(), (addr) => new ImTextureRef((ImTextureRefStruct*)addr));
        public readonly IImVectorWrapper<byte> _CallbacksDataBuf => new ImVectorWrapper<byte>(Unsafe.Read<ImVector>(&((ImDrawListStruct*)NativePointer)->_CallbacksDataBuf), Unsafe.SizeOf<byte>(), (addr) => *(byte*)addr);
        public readonly ref float _FringeScale => ref Unsafe.AsRef<float>(&((ImDrawListStruct*)NativePointer)->_FringeScale);
        public readonly sbyte* _OwnerName { get => ((ImDrawListStruct*)NativePointer)->_OwnerName; set => ((ImDrawListStruct*)NativePointer)->_OwnerName = value; }
    }

    public unsafe partial struct ImDrawData : IImDrawData
    {
        public nint NativePointer { get; set; }

        public ImDrawData(ImDrawDataStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref bool Valid => ref Unsafe.AsRef<bool>(&((ImDrawDataStruct*)NativePointer)->Valid);
        public readonly ref int CmdListsCount => ref Unsafe.AsRef<int>(&((ImDrawDataStruct*)NativePointer)->CmdListsCount);
        public readonly ref int TotalIdxCount => ref Unsafe.AsRef<int>(&((ImDrawDataStruct*)NativePointer)->TotalIdxCount);
        public readonly ref int TotalVtxCount => ref Unsafe.AsRef<int>(&((ImDrawDataStruct*)NativePointer)->TotalVtxCount);
        public readonly IImStructPtrVectorWrapper<IImDrawList> CmdLists => new ImStructPtrVectorWrapper<IImDrawList>(Unsafe.Read<ImVector>(&((ImDrawDataStruct*)NativePointer)->CmdLists), (addr) => new ImDrawList((ImDrawListStruct*)addr));
        public readonly ref Vector2 DisplayPos => ref Unsafe.AsRef<Vector2>(&((ImDrawDataStruct*)NativePointer)->DisplayPos);
        public readonly ref Vector2 DisplaySize => ref Unsafe.AsRef<Vector2>(&((ImDrawDataStruct*)NativePointer)->DisplaySize);
        public readonly ref Vector2 FramebufferScale => ref Unsafe.AsRef<Vector2>(&((ImDrawDataStruct*)NativePointer)->FramebufferScale);
        public readonly IImGuiViewport OwnerViewport => new ImGuiViewport(((ImDrawDataStruct*)NativePointer)->OwnerViewport);
        public readonly IImStructPtrVectorPtrWrapper<IImTextureData> Textures => new ImStructPtrVectorPtrWrapper<IImTextureData>((nint)(&(((ImDrawDataStruct*)NativePointer)->Textures)), (addr) => new ImTextureData((ImTextureDataStruct*)addr));
    }

    public unsafe partial struct ImTextureRect : IImTextureRect
    {
        public nint NativePointer { get; set; }

        public ImTextureRect(ImTextureRectStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref ushort x => ref Unsafe.AsRef<ushort>(&((ImTextureRectStruct*)NativePointer)->x);
        public readonly ref ushort y => ref Unsafe.AsRef<ushort>(&((ImTextureRectStruct*)NativePointer)->y);
        public readonly ref ushort w => ref Unsafe.AsRef<ushort>(&((ImTextureRectStruct*)NativePointer)->w);
        public readonly ref ushort h => ref Unsafe.AsRef<ushort>(&((ImTextureRectStruct*)NativePointer)->h);
    }

    public unsafe partial struct ImTextureData : IImTextureData
    {
        public nint NativePointer { get; set; }

        public ImTextureData(ImTextureDataStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref int UniqueID => ref Unsafe.AsRef<int>(&((ImTextureDataStruct*)NativePointer)->UniqueID);
        public readonly ref ImTextureStatus Status => ref Unsafe.AsRef<ImTextureStatus>(&((ImTextureDataStruct*)NativePointer)->Status);
        public readonly void* BackendUserData { get => ((ImTextureDataStruct*)NativePointer)->BackendUserData; set => ((ImTextureDataStruct*)NativePointer)->BackendUserData = value; }
        public readonly ref ulong TexID => ref Unsafe.AsRef<ulong>(&((ImTextureDataStruct*)NativePointer)->TexID);
        public readonly ref ImTextureFormat Format => ref Unsafe.AsRef<ImTextureFormat>(&((ImTextureDataStruct*)NativePointer)->Format);
        public readonly ref int Width => ref Unsafe.AsRef<int>(&((ImTextureDataStruct*)NativePointer)->Width);
        public readonly ref int Height => ref Unsafe.AsRef<int>(&((ImTextureDataStruct*)NativePointer)->Height);
        public readonly ref int BytesPerPixel => ref Unsafe.AsRef<int>(&((ImTextureDataStruct*)NativePointer)->BytesPerPixel);
        public readonly byte* Pixels { get => ((ImTextureDataStruct*)NativePointer)->Pixels; set => ((ImTextureDataStruct*)NativePointer)->Pixels = value; }
        public readonly IImTextureRect UsedRect => new ImTextureRect(&((ImTextureDataStruct*)NativePointer)->UsedRect);
        public readonly IImTextureRect UpdateRect => new ImTextureRect(&((ImTextureDataStruct*)NativePointer)->UpdateRect);
        public readonly IImVectorWrapper<IImTextureRect> Updates => new ImVectorWrapper<IImTextureRect>(Unsafe.Read<ImVector>(&((ImTextureDataStruct*)NativePointer)->Updates), Unsafe.SizeOf<ImTextureRectStruct>(), (addr) => new ImTextureRect((ImTextureRectStruct*)addr));
        public readonly ref int UnusedFrames => ref Unsafe.AsRef<int>(&((ImTextureDataStruct*)NativePointer)->UnusedFrames);
        public readonly ref ushort RefCount => ref Unsafe.AsRef<ushort>(&((ImTextureDataStruct*)NativePointer)->RefCount);
        public readonly ref bool UseColors => ref Unsafe.AsRef<bool>(&((ImTextureDataStruct*)NativePointer)->UseColors);
        public readonly ref bool WantDestroyNextFrame => ref Unsafe.AsRef<bool>(&((ImTextureDataStruct*)NativePointer)->WantDestroyNextFrame);
    }

    public unsafe partial struct ImFontConfig : IImFontConfig
    {
        public nint NativePointer { get; set; }

        public ImFontConfig(ImFontConfigStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IRangeAccessor<byte> Name => new RangeAccessor<byte>(&((ImFontConfigStruct*)NativePointer)->Name, 40);
        public readonly void* FontData { get => ((ImFontConfigStruct*)NativePointer)->FontData; set => ((ImFontConfigStruct*)NativePointer)->FontData = value; }
        public readonly ref int FontDataSize => ref Unsafe.AsRef<int>(&((ImFontConfigStruct*)NativePointer)->FontDataSize);
        public readonly ref bool FontDataOwnedByAtlas => ref Unsafe.AsRef<bool>(&((ImFontConfigStruct*)NativePointer)->FontDataOwnedByAtlas);
        public readonly ref bool MergeMode => ref Unsafe.AsRef<bool>(&((ImFontConfigStruct*)NativePointer)->MergeMode);
        public readonly ref bool PixelSnapH => ref Unsafe.AsRef<bool>(&((ImFontConfigStruct*)NativePointer)->PixelSnapH);
        public readonly ref bool PixelSnapV => ref Unsafe.AsRef<bool>(&((ImFontConfigStruct*)NativePointer)->PixelSnapV);
        public readonly ref sbyte OversampleH => ref Unsafe.AsRef<sbyte>(&((ImFontConfigStruct*)NativePointer)->OversampleH);
        public readonly ref sbyte OversampleV => ref Unsafe.AsRef<sbyte>(&((ImFontConfigStruct*)NativePointer)->OversampleV);
        public readonly ref uint EllipsisChar => ref Unsafe.AsRef<uint>(&((ImFontConfigStruct*)NativePointer)->EllipsisChar);
        public readonly ref float SizePixels => ref Unsafe.AsRef<float>(&((ImFontConfigStruct*)NativePointer)->SizePixels);
        public readonly uint* GlyphRanges { get => ((ImFontConfigStruct*)NativePointer)->GlyphRanges; set => ((ImFontConfigStruct*)NativePointer)->GlyphRanges = value; }
        public readonly uint* GlyphExcludeRanges { get => ((ImFontConfigStruct*)NativePointer)->GlyphExcludeRanges; set => ((ImFontConfigStruct*)NativePointer)->GlyphExcludeRanges = value; }
        public readonly ref Vector2 GlyphOffset => ref Unsafe.AsRef<Vector2>(&((ImFontConfigStruct*)NativePointer)->GlyphOffset);
        public readonly ref float GlyphMinAdvanceX => ref Unsafe.AsRef<float>(&((ImFontConfigStruct*)NativePointer)->GlyphMinAdvanceX);
        public readonly ref float GlyphMaxAdvanceX => ref Unsafe.AsRef<float>(&((ImFontConfigStruct*)NativePointer)->GlyphMaxAdvanceX);
        public readonly ref float GlyphExtraAdvanceX => ref Unsafe.AsRef<float>(&((ImFontConfigStruct*)NativePointer)->GlyphExtraAdvanceX);
        public readonly ref uint FontNo => ref Unsafe.AsRef<uint>(&((ImFontConfigStruct*)NativePointer)->FontNo);
        public readonly ref uint FontLoaderFlags => ref Unsafe.AsRef<uint>(&((ImFontConfigStruct*)NativePointer)->FontLoaderFlags);
        public readonly ref float RasterizerMultiply => ref Unsafe.AsRef<float>(&((ImFontConfigStruct*)NativePointer)->RasterizerMultiply);
        public readonly ref float RasterizerDensity => ref Unsafe.AsRef<float>(&((ImFontConfigStruct*)NativePointer)->RasterizerDensity);
        public readonly ref int Flags => ref Unsafe.AsRef<int>(&((ImFontConfigStruct*)NativePointer)->Flags);
        public readonly IImFont DstFont => new ImFont(((ImFontConfigStruct*)NativePointer)->DstFont);
        public readonly IImFontLoader FontLoader => new ImFontLoader(((ImFontConfigStruct*)NativePointer)->FontLoader);
        public readonly void* FontLoaderData { get => ((ImFontConfigStruct*)NativePointer)->FontLoaderData; set => ((ImFontConfigStruct*)NativePointer)->FontLoaderData = value; }
    }

    public unsafe partial struct ImFontGlyph : IImFontGlyph
    {
        public nint NativePointer { get; set; }

        public ImFontGlyph(ImFontGlyphStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref uint _bitfield => ref Unsafe.AsRef<uint>(&((ImFontGlyphStruct*)NativePointer)->_bitfield);
        public readonly uint Colored { get => ((ImFontGlyphStruct*)NativePointer)->Colored; set => ((ImFontGlyphStruct*)NativePointer)->Colored = value; }
        public readonly uint Visible { get => ((ImFontGlyphStruct*)NativePointer)->Visible; set => ((ImFontGlyphStruct*)NativePointer)->Visible = value; }
        public readonly uint SourceIdx { get => ((ImFontGlyphStruct*)NativePointer)->SourceIdx; set => ((ImFontGlyphStruct*)NativePointer)->SourceIdx = value; }
        public readonly uint Codepoint { get => ((ImFontGlyphStruct*)NativePointer)->Codepoint; set => ((ImFontGlyphStruct*)NativePointer)->Codepoint = value; }
        public readonly ref float AdvanceX => ref Unsafe.AsRef<float>(&((ImFontGlyphStruct*)NativePointer)->AdvanceX);
        public readonly ref float X0 => ref Unsafe.AsRef<float>(&((ImFontGlyphStruct*)NativePointer)->X0);
        public readonly ref float Y0 => ref Unsafe.AsRef<float>(&((ImFontGlyphStruct*)NativePointer)->Y0);
        public readonly ref float X1 => ref Unsafe.AsRef<float>(&((ImFontGlyphStruct*)NativePointer)->X1);
        public readonly ref float Y1 => ref Unsafe.AsRef<float>(&((ImFontGlyphStruct*)NativePointer)->Y1);
        public readonly ref float U0 => ref Unsafe.AsRef<float>(&((ImFontGlyphStruct*)NativePointer)->U0);
        public readonly ref float V0 => ref Unsafe.AsRef<float>(&((ImFontGlyphStruct*)NativePointer)->V0);
        public readonly ref float U1 => ref Unsafe.AsRef<float>(&((ImFontGlyphStruct*)NativePointer)->U1);
        public readonly ref float V1 => ref Unsafe.AsRef<float>(&((ImFontGlyphStruct*)NativePointer)->V1);
        public readonly ref int PackId => ref Unsafe.AsRef<int>(&((ImFontGlyphStruct*)NativePointer)->PackId);
    }

    public unsafe partial struct ImFontGlyphRangesBuilder : IImFontGlyphRangesBuilder
    {
        public nint NativePointer { get; set; }

        public ImFontGlyphRangesBuilder(ImFontGlyphRangesBuilderStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImVectorWrapper<uint> UsedChars => new ImVectorWrapper<uint>(Unsafe.Read<ImVector>(&((ImFontGlyphRangesBuilderStruct*)NativePointer)->UsedChars), Unsafe.SizeOf<uint>(), (addr) => *(uint*)addr);
    }

    public unsafe partial struct ImFontAtlasRect : IImFontAtlasRect
    {
        public nint NativePointer { get; set; }

        public ImFontAtlasRect(ImFontAtlasRectStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref ushort x => ref Unsafe.AsRef<ushort>(&((ImFontAtlasRectStruct*)NativePointer)->x);
        public readonly ref ushort y => ref Unsafe.AsRef<ushort>(&((ImFontAtlasRectStruct*)NativePointer)->y);
        public readonly ref ushort w => ref Unsafe.AsRef<ushort>(&((ImFontAtlasRectStruct*)NativePointer)->w);
        public readonly ref ushort h => ref Unsafe.AsRef<ushort>(&((ImFontAtlasRectStruct*)NativePointer)->h);
        public readonly ref Vector2 uv0 => ref Unsafe.AsRef<Vector2>(&((ImFontAtlasRectStruct*)NativePointer)->uv0);
        public readonly ref Vector2 uv1 => ref Unsafe.AsRef<Vector2>(&((ImFontAtlasRectStruct*)NativePointer)->uv1);
    }

    public unsafe partial struct ImFontAtlas : IImFontAtlas
    {
        public nint NativePointer { get; set; }

        public ImFontAtlas(ImFontAtlasStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref ImFontAtlasFlags Flags => ref Unsafe.AsRef<ImFontAtlasFlags>(&((ImFontAtlasStruct*)NativePointer)->Flags);
        public readonly ref ImTextureFormat TexDesiredFormat => ref Unsafe.AsRef<ImTextureFormat>(&((ImFontAtlasStruct*)NativePointer)->TexDesiredFormat);
        public readonly ref int TexGlyphPadding => ref Unsafe.AsRef<int>(&((ImFontAtlasStruct*)NativePointer)->TexGlyphPadding);
        public readonly ref int TexMinWidth => ref Unsafe.AsRef<int>(&((ImFontAtlasStruct*)NativePointer)->TexMinWidth);
        public readonly ref int TexMinHeight => ref Unsafe.AsRef<int>(&((ImFontAtlasStruct*)NativePointer)->TexMinHeight);
        public readonly ref int TexMaxWidth => ref Unsafe.AsRef<int>(&((ImFontAtlasStruct*)NativePointer)->TexMaxWidth);
        public readonly ref int TexMaxHeight => ref Unsafe.AsRef<int>(&((ImFontAtlasStruct*)NativePointer)->TexMaxHeight);
        public readonly void* UserData { get => ((ImFontAtlasStruct*)NativePointer)->UserData; set => ((ImFontAtlasStruct*)NativePointer)->UserData = value; }
        public readonly IImTextureData TexData => new ImTextureData(((ImFontAtlasStruct*)NativePointer)->TexData);
        public readonly IImStructPtrVectorWrapper<IImTextureData> TexList => new ImStructPtrVectorWrapper<IImTextureData>(Unsafe.Read<ImVector>(&((ImFontAtlasStruct*)NativePointer)->TexList), (addr) => new ImTextureData((ImTextureDataStruct*)addr));
        public readonly ref bool Locked => ref Unsafe.AsRef<bool>(&((ImFontAtlasStruct*)NativePointer)->Locked);
        public readonly ref bool RendererHasTextures => ref Unsafe.AsRef<bool>(&((ImFontAtlasStruct*)NativePointer)->RendererHasTextures);
        public readonly ref bool TexIsBuilt => ref Unsafe.AsRef<bool>(&((ImFontAtlasStruct*)NativePointer)->TexIsBuilt);
        public readonly ref bool TexPixelsUseColors => ref Unsafe.AsRef<bool>(&((ImFontAtlasStruct*)NativePointer)->TexPixelsUseColors);
        public readonly ref Vector2 TexUvScale => ref Unsafe.AsRef<Vector2>(&((ImFontAtlasStruct*)NativePointer)->TexUvScale);
        public readonly ref Vector2 TexUvWhitePixel => ref Unsafe.AsRef<Vector2>(&((ImFontAtlasStruct*)NativePointer)->TexUvWhitePixel);
        public readonly IImStructPtrVectorWrapper<IImFont> Fonts => new ImStructPtrVectorWrapper<IImFont>(Unsafe.Read<ImVector>(&((ImFontAtlasStruct*)NativePointer)->Fonts), (addr) => new ImFont((ImFontStruct*)addr));
        public readonly IImVectorWrapper<IImFontConfig> Sources => new ImVectorWrapper<IImFontConfig>(Unsafe.Read<ImVector>(&((ImFontAtlasStruct*)NativePointer)->Sources), Unsafe.SizeOf<ImFontConfigStruct>(), (addr) => new ImFontConfig((ImFontConfigStruct*)addr));
        public readonly IRangeAccessor<Vector4> TexUvLines => new RangeAccessor<Vector4>(&((ImFontAtlasStruct*)NativePointer)->TexUvLines, 33);
        public readonly ref int TexNextUniqueID => ref Unsafe.AsRef<int>(&((ImFontAtlasStruct*)NativePointer)->TexNextUniqueID);
        public readonly ref int FontNextUniqueID => ref Unsafe.AsRef<int>(&((ImFontAtlasStruct*)NativePointer)->FontNextUniqueID);
        public readonly IImStructPtrVectorWrapper<IImDrawListSharedData> DrawListSharedDatas => new ImStructPtrVectorWrapper<IImDrawListSharedData>(Unsafe.Read<ImVector>(&((ImFontAtlasStruct*)NativePointer)->DrawListSharedDatas), (addr) => new ImDrawListSharedData((ImDrawListSharedDataStruct*)addr));
        public readonly IImFontAtlasBuilder Builder => new ImFontAtlasBuilder(((ImFontAtlasStruct*)NativePointer)->Builder);
        public readonly IImFontLoader FontLoader => new ImFontLoader(((ImFontAtlasStruct*)NativePointer)->FontLoader);
        public readonly sbyte* FontLoaderName { get => ((ImFontAtlasStruct*)NativePointer)->FontLoaderName; set => ((ImFontAtlasStruct*)NativePointer)->FontLoaderName = value; }
        public readonly void* FontLoaderData { get => ((ImFontAtlasStruct*)NativePointer)->FontLoaderData; set => ((ImFontAtlasStruct*)NativePointer)->FontLoaderData = value; }
        public readonly ref uint FontLoaderFlags => ref Unsafe.AsRef<uint>(&((ImFontAtlasStruct*)NativePointer)->FontLoaderFlags);
        public readonly ref int RefCount => ref Unsafe.AsRef<int>(&((ImFontAtlasStruct*)NativePointer)->RefCount);
        public readonly IImGuiContext OwnerContext => new ImGuiContext(((ImFontAtlasStruct*)NativePointer)->OwnerContext);
        public readonly IImFontAtlasRect TempRect => new ImFontAtlasRect(&((ImFontAtlasStruct*)NativePointer)->TempRect);
        public readonly IImTextureRef TexRef { get => new ImTextureRef(((ImFontAtlasStruct*)NativePointer)->TexRef); set => ((ImFontAtlasStruct*)NativePointer)->TexRef = ((ImTextureRef)value).ToStruct(); }
        public readonly IImTextureRef TexID { get => new ImTextureRef(((ImFontAtlasStruct*)NativePointer)->TexID); set => ((ImFontAtlasStruct*)NativePointer)->TexID = ((ImTextureRef)value).ToStruct(); }
    }

    public unsafe partial struct ImFontBaked : IImFontBaked
    {
        public nint NativePointer { get; set; }

        public ImFontBaked(ImFontBakedStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImVectorWrapper<float> IndexAdvanceX => new ImVectorWrapper<float>(Unsafe.Read<ImVector>(&((ImFontBakedStruct*)NativePointer)->IndexAdvanceX), Unsafe.SizeOf<float>(), (addr) => *(float*)addr);
        public readonly ref float FallbackAdvanceX => ref Unsafe.AsRef<float>(&((ImFontBakedStruct*)NativePointer)->FallbackAdvanceX);
        public readonly ref float Size => ref Unsafe.AsRef<float>(&((ImFontBakedStruct*)NativePointer)->Size);
        public readonly ref float RasterizerDensity => ref Unsafe.AsRef<float>(&((ImFontBakedStruct*)NativePointer)->RasterizerDensity);
        public readonly IImVectorWrapper<ushort> IndexLookup => new ImVectorWrapper<ushort>(Unsafe.Read<ImVector>(&((ImFontBakedStruct*)NativePointer)->IndexLookup), Unsafe.SizeOf<ushort>(), (addr) => *(ushort*)addr);
        public readonly IImVectorWrapper<IImFontGlyph> Glyphs => new ImVectorWrapper<IImFontGlyph>(Unsafe.Read<ImVector>(&((ImFontBakedStruct*)NativePointer)->Glyphs), Unsafe.SizeOf<ImFontGlyphStruct>(), (addr) => new ImFontGlyph((ImFontGlyphStruct*)addr));
        public readonly ref int FallbackGlyphIndex => ref Unsafe.AsRef<int>(&((ImFontBakedStruct*)NativePointer)->FallbackGlyphIndex);
        public readonly ref float Ascent => ref Unsafe.AsRef<float>(&((ImFontBakedStruct*)NativePointer)->Ascent);
        public readonly ref float Descent => ref Unsafe.AsRef<float>(&((ImFontBakedStruct*)NativePointer)->Descent);
        public readonly ref uint _bitfield => ref Unsafe.AsRef<uint>(&((ImFontBakedStruct*)NativePointer)->_bitfield);
        public readonly uint MetricsTotalSurface { get => ((ImFontBakedStruct*)NativePointer)->MetricsTotalSurface; set => ((ImFontBakedStruct*)NativePointer)->MetricsTotalSurface = value; }
        public readonly uint WantDestroy { get => ((ImFontBakedStruct*)NativePointer)->WantDestroy; set => ((ImFontBakedStruct*)NativePointer)->WantDestroy = value; }
        public readonly uint LoadNoFallback { get => ((ImFontBakedStruct*)NativePointer)->LoadNoFallback; set => ((ImFontBakedStruct*)NativePointer)->LoadNoFallback = value; }
        public readonly uint LoadNoRenderOnLayout { get => ((ImFontBakedStruct*)NativePointer)->LoadNoRenderOnLayout; set => ((ImFontBakedStruct*)NativePointer)->LoadNoRenderOnLayout = value; }
        public readonly ref int LastUsedFrame => ref Unsafe.AsRef<int>(&((ImFontBakedStruct*)NativePointer)->LastUsedFrame);
        public readonly ref uint BakedId => ref Unsafe.AsRef<uint>(&((ImFontBakedStruct*)NativePointer)->BakedId);
        public readonly IImFont OwnerFont => new ImFont(((ImFontBakedStruct*)NativePointer)->OwnerFont);
        public readonly void* FontLoaderDatas { get => ((ImFontBakedStruct*)NativePointer)->FontLoaderDatas; set => ((ImFontBakedStruct*)NativePointer)->FontLoaderDatas = value; }
    }

    public unsafe partial struct ImFont : IImFont
    {
        public nint NativePointer { get; set; }

        public ImFont(ImFontStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly IImFontBaked LastBaked => new ImFontBaked(((ImFontStruct*)NativePointer)->LastBaked);
        public readonly IImFontAtlas OwnerAtlas => new ImFontAtlas(((ImFontStruct*)NativePointer)->OwnerAtlas);
        public readonly ref int Flags => ref Unsafe.AsRef<int>(&((ImFontStruct*)NativePointer)->Flags);
        public readonly ref float CurrentRasterizerDensity => ref Unsafe.AsRef<float>(&((ImFontStruct*)NativePointer)->CurrentRasterizerDensity);
        public readonly ref uint FontId => ref Unsafe.AsRef<uint>(&((ImFontStruct*)NativePointer)->FontId);
        public readonly ref float LegacySize => ref Unsafe.AsRef<float>(&((ImFontStruct*)NativePointer)->LegacySize);
        public readonly IImStructPtrVectorWrapper<IImFontConfig> Sources => new ImStructPtrVectorWrapper<IImFontConfig>(Unsafe.Read<ImVector>(&((ImFontStruct*)NativePointer)->Sources), (addr) => new ImFontConfig((ImFontConfigStruct*)addr));
        public readonly ref uint EllipsisChar => ref Unsafe.AsRef<uint>(&((ImFontStruct*)NativePointer)->EllipsisChar);
        public readonly ref uint FallbackChar => ref Unsafe.AsRef<uint>(&((ImFontStruct*)NativePointer)->FallbackChar);
        public readonly IRangeAccessor<byte> Used8kPagesMap => new RangeAccessor<byte>(&((ImFontStruct*)NativePointer)->Used8kPagesMap, 17);
        public readonly ref bool EllipsisAutoBake => ref Unsafe.AsRef<bool>(&((ImFontStruct*)NativePointer)->EllipsisAutoBake);
        public readonly IImGuiStorage RemapPairs => new ImGuiStorage(&((ImFontStruct*)NativePointer)->RemapPairs);
        public readonly ref float Scale => ref Unsafe.AsRef<float>(&((ImFontStruct*)NativePointer)->Scale);
    }

    public unsafe partial struct ImGuiViewport : IImGuiViewport
    {
        public nint NativePointer { get; set; }

        public ImGuiViewport(ImGuiViewportStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref uint ID => ref Unsafe.AsRef<uint>(&((ImGuiViewportStruct*)NativePointer)->ID);
        public readonly ref ImGuiViewportFlags Flags => ref Unsafe.AsRef<ImGuiViewportFlags>(&((ImGuiViewportStruct*)NativePointer)->Flags);
        public readonly ref Vector2 Pos => ref Unsafe.AsRef<Vector2>(&((ImGuiViewportStruct*)NativePointer)->Pos);
        public readonly ref Vector2 Size => ref Unsafe.AsRef<Vector2>(&((ImGuiViewportStruct*)NativePointer)->Size);
        public readonly ref Vector2 FramebufferScale => ref Unsafe.AsRef<Vector2>(&((ImGuiViewportStruct*)NativePointer)->FramebufferScale);
        public readonly ref Vector2 WorkPos => ref Unsafe.AsRef<Vector2>(&((ImGuiViewportStruct*)NativePointer)->WorkPos);
        public readonly ref Vector2 WorkSize => ref Unsafe.AsRef<Vector2>(&((ImGuiViewportStruct*)NativePointer)->WorkSize);
        public readonly ref float DpiScale => ref Unsafe.AsRef<float>(&((ImGuiViewportStruct*)NativePointer)->DpiScale);
        public readonly ref uint ParentViewportId => ref Unsafe.AsRef<uint>(&((ImGuiViewportStruct*)NativePointer)->ParentViewportId);
        public readonly IImGuiViewport ParentViewport => new ImGuiViewport(((ImGuiViewportStruct*)NativePointer)->ParentViewport);
        public readonly IImDrawData DrawData => new ImDrawData(((ImGuiViewportStruct*)NativePointer)->DrawData);
        public readonly void* RendererUserData { get => ((ImGuiViewportStruct*)NativePointer)->RendererUserData; set => ((ImGuiViewportStruct*)NativePointer)->RendererUserData = value; }
        public readonly void* PlatformUserData { get => ((ImGuiViewportStruct*)NativePointer)->PlatformUserData; set => ((ImGuiViewportStruct*)NativePointer)->PlatformUserData = value; }
        public readonly void* PlatformHandle { get => ((ImGuiViewportStruct*)NativePointer)->PlatformHandle; set => ((ImGuiViewportStruct*)NativePointer)->PlatformHandle = value; }
        public readonly void* PlatformHandleRaw { get => ((ImGuiViewportStruct*)NativePointer)->PlatformHandleRaw; set => ((ImGuiViewportStruct*)NativePointer)->PlatformHandleRaw = value; }
        public readonly ref bool PlatformWindowCreated => ref Unsafe.AsRef<bool>(&((ImGuiViewportStruct*)NativePointer)->PlatformWindowCreated);
        public readonly ref bool PlatformRequestMove => ref Unsafe.AsRef<bool>(&((ImGuiViewportStruct*)NativePointer)->PlatformRequestMove);
        public readonly ref bool PlatformRequestResize => ref Unsafe.AsRef<bool>(&((ImGuiViewportStruct*)NativePointer)->PlatformRequestResize);
        public readonly ref bool PlatformRequestClose => ref Unsafe.AsRef<bool>(&((ImGuiViewportStruct*)NativePointer)->PlatformRequestClose);
    }

    public unsafe partial struct ImGuiPlatformIO : IImGuiPlatformIO
    {
        public nint NativePointer { get; set; }

        public ImGuiPlatformIO(ImGuiPlatformIOStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly delegate* unmanaged[Cdecl]<nint, nint> Platform_GetClipboardTextFn { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetClipboardTextFn; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetClipboardTextFn = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> Platform_SetClipboardTextFn { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetClipboardTextFn; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetClipboardTextFn = value; }
        public readonly void* Platform_ClipboardUserData { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_ClipboardUserData; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_ClipboardUserData = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, nint, byte> Platform_OpenInShellFn { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_OpenInShellFn; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_OpenInShellFn = value; }
        public readonly void* Platform_OpenInShellUserData { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_OpenInShellUserData; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_OpenInShellUserData = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, nint, nint, void> Platform_SetImeDataFn { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetImeDataFn; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetImeDataFn = value; }
        public readonly void* Platform_ImeUserData { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_ImeUserData; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_ImeUserData = value; }
        public readonly ref uint Platform_LocaleDecimalPoint => ref Unsafe.AsRef<uint>(&((ImGuiPlatformIOStruct*)NativePointer)->Platform_LocaleDecimalPoint);
        public readonly ref int Renderer_TextureMaxWidth => ref Unsafe.AsRef<int>(&((ImGuiPlatformIOStruct*)NativePointer)->Renderer_TextureMaxWidth);
        public readonly ref int Renderer_TextureMaxHeight => ref Unsafe.AsRef<int>(&((ImGuiPlatformIOStruct*)NativePointer)->Renderer_TextureMaxHeight);
        public readonly void* Renderer_RenderState { get => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_RenderState; set => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_RenderState = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, void> Platform_CreateWindow { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_CreateWindow; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_CreateWindow = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, void> Platform_DestroyWindow { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_DestroyWindow; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_DestroyWindow = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, void> Platform_ShowWindow { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_ShowWindow; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_ShowWindow = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, Vector2, void> Platform_SetWindowPos { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowPos; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowPos = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, Vector2> Platform_GetWindowPos { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowPos; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowPos = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, Vector2, void> Platform_SetWindowSize { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowSize; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowSize = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, Vector2> Platform_GetWindowSize { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowSize; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowSize = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, Vector2> Platform_GetWindowFramebufferScale { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowFramebufferScale; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowFramebufferScale = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, void> Platform_SetWindowFocus { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowFocus; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowFocus = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, byte> Platform_GetWindowFocus { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowFocus; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowFocus = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, byte> Platform_GetWindowMinimized { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowMinimized; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowMinimized = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> Platform_SetWindowTitle { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowTitle; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowTitle = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, float, void> Platform_SetWindowAlpha { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowAlpha; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SetWindowAlpha = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, void> Platform_UpdateWindow { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_UpdateWindow; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_UpdateWindow = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> Platform_RenderWindow { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_RenderWindow; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_RenderWindow = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> Platform_SwapBuffers { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SwapBuffers; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_SwapBuffers = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, float> Platform_GetWindowDpiScale { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowDpiScale; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowDpiScale = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, void> Platform_OnChangedViewport { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_OnChangedViewport; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_OnChangedViewport = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, Vector4> Platform_GetWindowWorkAreaInsets { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowWorkAreaInsets; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_GetWindowWorkAreaInsets = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, ulong, nint, nint, int> Platform_CreateVkSurface { get => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_CreateVkSurface; set => ((ImGuiPlatformIOStruct*)NativePointer)->Platform_CreateVkSurface = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, void> Renderer_CreateWindow { get => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_CreateWindow; set => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_CreateWindow = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, void> Renderer_DestroyWindow { get => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_DestroyWindow; set => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_DestroyWindow = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, Vector2, void> Renderer_SetWindowSize { get => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_SetWindowSize; set => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_SetWindowSize = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> Renderer_RenderWindow { get => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_RenderWindow; set => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_RenderWindow = value; }
        public readonly delegate* unmanaged[Cdecl]<nint, nint, void> Renderer_SwapBuffers { get => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_SwapBuffers; set => ((ImGuiPlatformIOStruct*)NativePointer)->Renderer_SwapBuffers = value; }
        public readonly IImVectorWrapper<IImGuiPlatformMonitor> Monitors => new ImVectorWrapper<IImGuiPlatformMonitor>(Unsafe.Read<ImVector>(&((ImGuiPlatformIOStruct*)NativePointer)->Monitors), Unsafe.SizeOf<ImGuiPlatformMonitorStruct>(), (addr) => new ImGuiPlatformMonitor((ImGuiPlatformMonitorStruct*)addr));
        public readonly IImStructPtrVectorWrapper<IImTextureData> Textures => new ImStructPtrVectorWrapper<IImTextureData>(Unsafe.Read<ImVector>(&((ImGuiPlatformIOStruct*)NativePointer)->Textures), (addr) => new ImTextureData((ImTextureDataStruct*)addr));
        public readonly IImStructPtrVectorWrapper<IImGuiViewport> Viewports => new ImStructPtrVectorWrapper<IImGuiViewport>(Unsafe.Read<ImVector>(&((ImGuiPlatformIOStruct*)NativePointer)->Viewports), (addr) => new ImGuiViewport((ImGuiViewportStruct*)addr));
    }

    public unsafe partial struct ImGuiPlatformMonitor : IImGuiPlatformMonitor
    {
        public nint NativePointer { get; set; }

        public ImGuiPlatformMonitor(ImGuiPlatformMonitorStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref Vector2 MainPos => ref Unsafe.AsRef<Vector2>(&((ImGuiPlatformMonitorStruct*)NativePointer)->MainPos);
        public readonly ref Vector2 MainSize => ref Unsafe.AsRef<Vector2>(&((ImGuiPlatformMonitorStruct*)NativePointer)->MainSize);
        public readonly ref Vector2 WorkPos => ref Unsafe.AsRef<Vector2>(&((ImGuiPlatformMonitorStruct*)NativePointer)->WorkPos);
        public readonly ref Vector2 WorkSize => ref Unsafe.AsRef<Vector2>(&((ImGuiPlatformMonitorStruct*)NativePointer)->WorkSize);
        public readonly ref float DpiScale => ref Unsafe.AsRef<float>(&((ImGuiPlatformMonitorStruct*)NativePointer)->DpiScale);
        public readonly void* PlatformHandle { get => ((ImGuiPlatformMonitorStruct*)NativePointer)->PlatformHandle; set => ((ImGuiPlatformMonitorStruct*)NativePointer)->PlatformHandle = value; }
    }

    public unsafe partial struct ImGuiPlatformImeData : IImGuiPlatformImeData
    {
        public nint NativePointer { get; set; }

        public ImGuiPlatformImeData(ImGuiPlatformImeDataStruct* nativePtr) => NativePointer = (nint)nativePtr;
        public readonly ref bool WantVisible => ref Unsafe.AsRef<bool>(&((ImGuiPlatformImeDataStruct*)NativePointer)->WantVisible);
        public readonly ref bool WantTextInput => ref Unsafe.AsRef<bool>(&((ImGuiPlatformImeDataStruct*)NativePointer)->WantTextInput);
        public readonly ref Vector2 InputPos => ref Unsafe.AsRef<Vector2>(&((ImGuiPlatformImeDataStruct*)NativePointer)->InputPos);
        public readonly ref float InputLineHeight => ref Unsafe.AsRef<float>(&((ImGuiPlatformImeDataStruct*)NativePointer)->InputLineHeight);
        public readonly ref uint ViewportId => ref Unsafe.AsRef<uint>(&((ImGuiPlatformImeDataStruct*)NativePointer)->ViewportId);
    }

    public IImFontLoader ImGuiFreeTypeGetFontLoader()
    {
        var ret = ImGuiMethods.ImGuiFreeTypeGetFontLoader();
        if (ret is null)
        return null !;
        return new ImFontLoader(ret);
    }

    public void ImGuiFreeTypeSetAllocatorFunctions(delegate* unmanaged[Cdecl]<nuint, nint, nint> alloc_func, delegate* unmanaged[Cdecl]<nint, nint, void> free_func) => ImGuiMethods.ImGuiFreeTypeSetAllocatorFunctions(alloc_func, free_func);
    public void ImGuiFreeTypeSetAllocatorFunctionsEx(delegate* unmanaged[Cdecl]<nuint, nint, nint> alloc_func, delegate* unmanaged[Cdecl]<nint, nint, void> free_func, void* user_data) => ImGuiMethods.ImGuiFreeTypeSetAllocatorFunctionsEx(alloc_func, free_func, user_data);
    public bool ImGuiFreeTypeDebugEditFontLoaderFlags(ref uint p_font_loader_flags) => ImGuiMethods.ImGuiFreeTypeDebugEditFontLoaderFlags(ref p_font_loader_flags);
    public unsafe partial struct ImGuiFreeTypeImDrawData : IImGuiFreeTypeImDrawData
    {
        public nint NativePointer { get; set; }

        public ImGuiFreeTypeImDrawData(ImGuiFreeTypeImDrawDataStruct* nativePtr) => NativePointer = (nint)nativePtr;
    }
}