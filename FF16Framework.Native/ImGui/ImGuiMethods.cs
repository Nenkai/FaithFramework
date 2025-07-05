using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Numerics;
using FF16Framework.Interfaces.ImGui;

namespace FF16Framework.Native.ImGui;
#pragma warning disable CA1401 // P/Invokes should not be visible

#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time

#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments

public unsafe class ImGuiMethods
{
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ulong ImTextureRef_GetTexID(ImTextureRef self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_CreateContext", ExactSpelling = true)]
    public static extern ImGuiContextStruct* CreateContext(ImFontAtlasStruct* shared_font_atlas);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DestroyContext", ExactSpelling = true)]
    public static extern void DestroyContext(ImGuiContextStruct* ctx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetCurrentContext", ExactSpelling = true)]
    public static extern ImGuiContextStruct* GetCurrentContext();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetCurrentContext", ExactSpelling = true)]
    public static extern void SetCurrentContext(ImGuiContextStruct* ctx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetIO", ExactSpelling = true)]
    public static extern ImGuiIOStruct* GetIO();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetPlatformIO", ExactSpelling = true)]
    public static extern ImGuiPlatformIOStruct* GetPlatformIO();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetStyle", ExactSpelling = true)]
    public static extern ImGuiStyleStruct* GetStyle();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_NewFrame", ExactSpelling = true)]
    public static extern void NewFrame();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndFrame", ExactSpelling = true)]
    public static extern void EndFrame();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Render", ExactSpelling = true)]
    public static extern void Render();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetDrawData", ExactSpelling = true)]
    public static extern ImDrawDataStruct* GetDrawData();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowDemoWindow", ExactSpelling = true)]
    public static extern void ShowDemoWindow(ref bool p_open);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowMetricsWindow", ExactSpelling = true)]
    public static extern void ShowMetricsWindow(ref bool p_open);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowDebugLogWindow", ExactSpelling = true)]
    public static extern void ShowDebugLogWindow(ref bool p_open);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowIDStackToolWindow", ExactSpelling = true)]
    public static extern void ShowIDStackToolWindow();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowIDStackToolWindowEx", ExactSpelling = true)]
    public static extern void ShowIDStackToolWindowEx(ref bool p_open);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowAboutWindow", ExactSpelling = true)]
    public static extern void ShowAboutWindow(ref bool p_open);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowStyleEditor", ExactSpelling = true)]
    public static extern void ShowStyleEditor(ImGuiStyleStruct* @ref);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowStyleSelector", ExactSpelling = true)]
    public static extern bool ShowStyleSelector([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowFontSelector", ExactSpelling = true)]
    public static extern void ShowFontSelector([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowUserGuide", ExactSpelling = true)]
    public static extern void ShowUserGuide();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetVersion", ExactSpelling = true)]
    public static extern sbyte* GetVersion();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_StyleColorsDark", ExactSpelling = true)]
    public static extern void StyleColorsDark(ImGuiStyleStruct* dst);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_StyleColorsLight", ExactSpelling = true)]
    public static extern void StyleColorsLight(ImGuiStyleStruct* dst);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_StyleColorsClassic", ExactSpelling = true)]
    public static extern void StyleColorsClassic(ImGuiStyleStruct* dst);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Begin", ExactSpelling = true)]
    public static extern bool Begin([MarshalAs(UnmanagedType.LPUTF8Str)] string name, ref bool p_open, ImGuiWindowFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_End", ExactSpelling = true)]
    public static extern void End();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginChild", ExactSpelling = true)]
    public static extern bool BeginChild([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginChildID", ExactSpelling = true)]
    public static extern bool BeginChildID(uint id, Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndChild", ExactSpelling = true)]
    public static extern void EndChild();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsWindowAppearing", ExactSpelling = true)]
    public static extern bool IsWindowAppearing();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsWindowCollapsed", ExactSpelling = true)]
    public static extern bool IsWindowCollapsed();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsWindowFocused", ExactSpelling = true)]
    public static extern bool IsWindowFocused(ImGuiFocusedFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsWindowHovered", ExactSpelling = true)]
    public static extern bool IsWindowHovered(ImGuiHoveredFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowDrawList", ExactSpelling = true)]
    public static extern ImDrawListStruct* GetWindowDrawList();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowDpiScale", ExactSpelling = true)]
    public static extern float GetWindowDpiScale();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowPos", ExactSpelling = true)]
    public static extern Vector2 GetWindowPos();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowSize", ExactSpelling = true)]
    public static extern Vector2 GetWindowSize();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowWidth", ExactSpelling = true)]
    public static extern float GetWindowWidth();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowHeight", ExactSpelling = true)]
    public static extern float GetWindowHeight();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowViewport", ExactSpelling = true)]
    public static extern ImGuiViewportStruct* GetWindowViewport();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowPos", ExactSpelling = true)]
    public static extern void SetNextWindowPos(Vector2 pos, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowPosEx", ExactSpelling = true)]
    public static extern void SetNextWindowPosEx(Vector2 pos, ImGuiCond cond, Vector2 pivot);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowSize", ExactSpelling = true)]
    public static extern void SetNextWindowSize(Vector2 size, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowSizeConstraints", ExactSpelling = true)]
    public static extern void SetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, delegate* unmanaged[Cdecl]<nint, void> custom_callback, void* custom_callback_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowContentSize", ExactSpelling = true)]
    public static extern void SetNextWindowContentSize(Vector2 size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowCollapsed", ExactSpelling = true)]
    public static extern void SetNextWindowCollapsed(bool collapsed, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowFocus", ExactSpelling = true)]
    public static extern void SetNextWindowFocus();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowScroll", ExactSpelling = true)]
    public static extern void SetNextWindowScroll(Vector2 scroll);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowBgAlpha", ExactSpelling = true)]
    public static extern void SetNextWindowBgAlpha(float alpha);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowViewport", ExactSpelling = true)]
    public static extern void SetNextWindowViewport(uint viewport_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetWindowPos", ExactSpelling = true)]
    public static extern void SetWindowPos(Vector2 pos, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetWindowSize", ExactSpelling = true)]
    public static extern void SetWindowSize(Vector2 size, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetWindowCollapsed", ExactSpelling = true)]
    public static extern void SetWindowCollapsed(bool collapsed, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetWindowFocus", ExactSpelling = true)]
    public static extern void SetWindowFocus();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetWindowPosStr", ExactSpelling = true)]
    public static extern void SetWindowPosStr([MarshalAs(UnmanagedType.LPUTF8Str)] string name, Vector2 pos, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetWindowSizeStr", ExactSpelling = true)]
    public static extern void SetWindowSizeStr([MarshalAs(UnmanagedType.LPUTF8Str)] string name, Vector2 size, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetWindowCollapsedStr", ExactSpelling = true)]
    public static extern void SetWindowCollapsedStr([MarshalAs(UnmanagedType.LPUTF8Str)] string name, bool collapsed, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetWindowFocusStr", ExactSpelling = true)]
    public static extern void SetWindowFocusStr([MarshalAs(UnmanagedType.LPUTF8Str)] string name);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetScrollX", ExactSpelling = true)]
    public static extern float GetScrollX();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetScrollY", ExactSpelling = true)]
    public static extern float GetScrollY();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetScrollX", ExactSpelling = true)]
    public static extern void SetScrollX(float scroll_x);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetScrollY", ExactSpelling = true)]
    public static extern void SetScrollY(float scroll_y);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetScrollMaxX", ExactSpelling = true)]
    public static extern float GetScrollMaxX();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetScrollMaxY", ExactSpelling = true)]
    public static extern float GetScrollMaxY();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetScrollHereX", ExactSpelling = true)]
    public static extern void SetScrollHereX(float center_x_ratio);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetScrollHereY", ExactSpelling = true)]
    public static extern void SetScrollHereY(float center_y_ratio);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetScrollFromPosX", ExactSpelling = true)]
    public static extern void SetScrollFromPosX(float local_x, float center_x_ratio);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetScrollFromPosY", ExactSpelling = true)]
    public static extern void SetScrollFromPosY(float local_y, float center_y_ratio);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushFontFloat", ExactSpelling = true)]
    public static extern void PushFontFloat(ImFontStruct* font, float font_size_base_unscaled);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopFont", ExactSpelling = true)]
    public static extern void PopFont();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetFont", ExactSpelling = true)]
    public static extern ImFontStruct* GetFont();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetFontSize", ExactSpelling = true)]
    public static extern float GetFontSize();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetFontBaked", ExactSpelling = true)]
    public static extern ImFontBakedStruct* GetFontBaked();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushStyleColor", ExactSpelling = true)]
    public static extern void PushStyleColor(ImGuiCol idx, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushStyleColorImVec4", ExactSpelling = true)]
    public static extern void PushStyleColorImVec4(ImGuiCol idx, Vector4 col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopStyleColor", ExactSpelling = true)]
    public static extern void PopStyleColor();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopStyleColorEx", ExactSpelling = true)]
    public static extern void PopStyleColorEx(int count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushStyleVar", ExactSpelling = true)]
    public static extern void PushStyleVar(ImGuiStyleVar idx, float val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushStyleVarImVec2", ExactSpelling = true)]
    public static extern void PushStyleVarImVec2(ImGuiStyleVar idx, Vector2 val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushStyleVarX", ExactSpelling = true)]
    public static extern void PushStyleVarX(ImGuiStyleVar idx, float val_x);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushStyleVarY", ExactSpelling = true)]
    public static extern void PushStyleVarY(ImGuiStyleVar idx, float val_y);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopStyleVar", ExactSpelling = true)]
    public static extern void PopStyleVar();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopStyleVarEx", ExactSpelling = true)]
    public static extern void PopStyleVarEx(int count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushItemFlag", ExactSpelling = true)]
    public static extern void PushItemFlag(ImGuiItemFlags option, bool enabled);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopItemFlag", ExactSpelling = true)]
    public static extern void PopItemFlag();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushItemWidth", ExactSpelling = true)]
    public static extern void PushItemWidth(float item_width);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopItemWidth", ExactSpelling = true)]
    public static extern void PopItemWidth();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextItemWidth", ExactSpelling = true)]
    public static extern void SetNextItemWidth(float item_width);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_CalcItemWidth", ExactSpelling = true)]
    public static extern float CalcItemWidth();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushTextWrapPos", ExactSpelling = true)]
    public static extern void PushTextWrapPos(float wrap_local_pos_x);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopTextWrapPos", ExactSpelling = true)]
    public static extern void PopTextWrapPos();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetFontTexUvWhitePixel", ExactSpelling = true)]
    public static extern Vector2 GetFontTexUvWhitePixel();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetColorU32", ExactSpelling = true)]
    public static extern uint GetColorU32(ImGuiCol idx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetColorU32Ex", ExactSpelling = true)]
    public static extern uint GetColorU32Ex(ImGuiCol idx, float alpha_mul);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetColorU32ImVec4", ExactSpelling = true)]
    public static extern uint GetColorU32ImVec4(Vector4 col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetColorU32ImU32", ExactSpelling = true)]
    public static extern uint GetColorU32ImU32(uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetColorU32ImU32Ex", ExactSpelling = true)]
    public static extern uint GetColorU32ImU32Ex(uint col, float alpha_mul);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetStyleColorVec4", ExactSpelling = true)]
    public static extern Vector4 GetStyleColorVec4(ImGuiCol idx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetCursorScreenPos", ExactSpelling = true)]
    public static extern Vector2 GetCursorScreenPos();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetCursorScreenPos", ExactSpelling = true)]
    public static extern void SetCursorScreenPos(Vector2 pos);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetContentRegionAvail", ExactSpelling = true)]
    public static extern Vector2 GetContentRegionAvail();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetCursorPos", ExactSpelling = true)]
    public static extern Vector2 GetCursorPos();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetCursorPosX", ExactSpelling = true)]
    public static extern float GetCursorPosX();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetCursorPosY", ExactSpelling = true)]
    public static extern float GetCursorPosY();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetCursorPos", ExactSpelling = true)]
    public static extern void SetCursorPos(Vector2 local_pos);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetCursorPosX", ExactSpelling = true)]
    public static extern void SetCursorPosX(float local_x);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetCursorPosY", ExactSpelling = true)]
    public static extern void SetCursorPosY(float local_y);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetCursorStartPos", ExactSpelling = true)]
    public static extern Vector2 GetCursorStartPos();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Separator", ExactSpelling = true)]
    public static extern void Separator();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SameLine", ExactSpelling = true)]
    public static extern void SameLine();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SameLineEx", ExactSpelling = true)]
    public static extern void SameLineEx(float offset_from_start_x, float spacing);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_NewLine", ExactSpelling = true)]
    public static extern void NewLine();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Spacing", ExactSpelling = true)]
    public static extern void Spacing();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Dummy", ExactSpelling = true)]
    public static extern void Dummy(Vector2 size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Indent", ExactSpelling = true)]
    public static extern void Indent();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IndentEx", ExactSpelling = true)]
    public static extern void IndentEx(float indent_w);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Unindent", ExactSpelling = true)]
    public static extern void Unindent();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_UnindentEx", ExactSpelling = true)]
    public static extern void UnindentEx(float indent_w);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginGroup", ExactSpelling = true)]
    public static extern void BeginGroup();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndGroup", ExactSpelling = true)]
    public static extern void EndGroup();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_AlignTextToFramePadding", ExactSpelling = true)]
    public static extern void AlignTextToFramePadding();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetTextLineHeight", ExactSpelling = true)]
    public static extern float GetTextLineHeight();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetTextLineHeightWithSpacing", ExactSpelling = true)]
    public static extern float GetTextLineHeightWithSpacing();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetFrameHeight", ExactSpelling = true)]
    public static extern float GetFrameHeight();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetFrameHeightWithSpacing", ExactSpelling = true)]
    public static extern float GetFrameHeightWithSpacing();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushID", ExactSpelling = true)]
    public static extern void PushID([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushIDStr", ExactSpelling = true)]
    public static extern void PushIDStr([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id_begin, [MarshalAs(UnmanagedType.LPUTF8Str)] string str_id_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushIDPtr", ExactSpelling = true)]
    public static extern void PushIDPtr(void* ptr_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushIDInt", ExactSpelling = true)]
    public static extern void PushIDInt(int int_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopID", ExactSpelling = true)]
    public static extern void PopID();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetID", ExactSpelling = true)]
    public static extern uint GetID([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetIDStr", ExactSpelling = true)]
    public static extern uint GetIDStr([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id_begin, [MarshalAs(UnmanagedType.LPUTF8Str)] string str_id_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetIDPtr", ExactSpelling = true)]
    public static extern uint GetIDPtr(void* ptr_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetIDInt", ExactSpelling = true)]
    public static extern uint GetIDInt(int int_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextUnformatted", ExactSpelling = true)]
    public static extern void TextUnformatted([MarshalAs(UnmanagedType.LPUTF8Str)] string text);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextUnformattedEx", ExactSpelling = true)]
    public static extern void TextUnformattedEx([MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Text", ExactSpelling = true)]
    public static extern void Text([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextV", ExactSpelling = true)]
    public static extern void TextV([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextColored", ExactSpelling = true)]
    public static extern void TextColored(Vector4 col, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextColoredV", ExactSpelling = true)]
    public static extern void TextColoredV(Vector4 col, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextDisabled", ExactSpelling = true)]
    public static extern void TextDisabled([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextDisabledV", ExactSpelling = true)]
    public static extern void TextDisabledV([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextWrapped", ExactSpelling = true)]
    public static extern void TextWrapped([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextWrappedV", ExactSpelling = true)]
    public static extern void TextWrappedV([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LabelText", ExactSpelling = true)]
    public static extern void LabelText([MarshalAs(UnmanagedType.LPUTF8Str)] string label, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LabelTextV", ExactSpelling = true)]
    public static extern void LabelTextV([MarshalAs(UnmanagedType.LPUTF8Str)] string label, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BulletText", ExactSpelling = true)]
    public static extern void BulletText([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BulletTextV", ExactSpelling = true)]
    public static extern void BulletTextV([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SeparatorText", ExactSpelling = true)]
    public static extern void SeparatorText([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Button", ExactSpelling = true)]
    public static extern bool Button([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ButtonEx", ExactSpelling = true)]
    public static extern bool ButtonEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, Vector2 size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SmallButton", ExactSpelling = true)]
    public static extern bool SmallButton([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InvisibleButton", ExactSpelling = true)]
    public static extern bool InvisibleButton([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, Vector2 size, ImGuiButtonFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ArrowButton", ExactSpelling = true)]
    public static extern bool ArrowButton([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, int dir);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Checkbox", ExactSpelling = true)]
    public static extern bool Checkbox([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref bool v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_CheckboxFlagsIntPtr", ExactSpelling = true)]
    public static extern bool CheckboxFlagsIntPtr([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int flags, int flags_value);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_CheckboxFlagsUintPtr", ExactSpelling = true)]
    public static extern bool CheckboxFlagsUintPtr([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref uint flags, uint flags_value);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_RadioButton", ExactSpelling = true)]
    public static extern bool RadioButton([MarshalAs(UnmanagedType.LPUTF8Str)] string label, bool active);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_RadioButtonIntPtr", ExactSpelling = true)]
    public static extern bool RadioButtonIntPtr([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int v_button);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ProgressBar", ExactSpelling = true)]
    public static extern void ProgressBar(float fraction, Vector2 size_arg, [MarshalAs(UnmanagedType.LPUTF8Str)] string overlay);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Bullet", ExactSpelling = true)]
    public static extern void Bullet();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextLink", ExactSpelling = true)]
    public static extern bool TextLink([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextLinkOpenURL", ExactSpelling = true)]
    public static extern bool TextLinkOpenURL([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TextLinkOpenURLEx", ExactSpelling = true)]
    public static extern bool TextLinkOpenURLEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, [MarshalAs(UnmanagedType.LPUTF8Str)] string url);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Image", ExactSpelling = true)]
    public static extern void Image(ImTextureRef tex_ref, Vector2 image_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ImageEx", ExactSpelling = true)]
    public static extern void ImageEx(ImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ImageWithBg", ExactSpelling = true)]
    public static extern void ImageWithBg(ImTextureRef tex_ref, Vector2 image_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ImageWithBgEx", ExactSpelling = true)]
    public static extern void ImageWithBgEx(ImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1, Vector4 bg_col, Vector4 tint_col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ImageButton", ExactSpelling = true)]
    public static extern bool ImageButton([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImTextureRef tex_ref, Vector2 image_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ImageButtonEx", ExactSpelling = true)]
    public static extern bool ImageButtonEx([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1, Vector4 bg_col, Vector4 tint_col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginCombo", ExactSpelling = true)]
    public static extern bool BeginCombo([MarshalAs(UnmanagedType.LPUTF8Str)] string label, [MarshalAs(UnmanagedType.LPUTF8Str)] string preview_value, ImGuiComboFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndCombo", ExactSpelling = true)]
    public static extern void EndCombo();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ComboChar", ExactSpelling = true)]
    public static extern bool ComboChar([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, sbyte** items, int items_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ComboCharEx", ExactSpelling = true)]
    public static extern bool ComboCharEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, sbyte** items, int items_count, int popup_max_height_in_items);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Combo", ExactSpelling = true)]
    public static extern bool Combo([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, [MarshalAs(UnmanagedType.LPUTF8Str)] string items_separated_by_zeros);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ComboEx", ExactSpelling = true)]
    public static extern bool ComboEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, [MarshalAs(UnmanagedType.LPUTF8Str)] string items_separated_by_zeros, int popup_max_height_in_items);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ComboCallback", ExactSpelling = true)]
    public static extern bool ComboCallback([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ComboCallbackEx", ExactSpelling = true)]
    public static extern bool ComboCallbackEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count, int popup_max_height_in_items);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloat", ExactSpelling = true)]
    public static extern bool DragFloat([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloatEx", ExactSpelling = true)]
    public static extern bool DragFloatEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_speed, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloat2", ExactSpelling = true)]
    public static extern bool DragFloat2([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloat2Ex", ExactSpelling = true)]
    public static extern bool DragFloat2Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_speed, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloat3", ExactSpelling = true)]
    public static extern bool DragFloat3([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloat3Ex", ExactSpelling = true)]
    public static extern bool DragFloat3Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_speed, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloat4", ExactSpelling = true)]
    public static extern bool DragFloat4([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloat4Ex", ExactSpelling = true)]
    public static extern bool DragFloat4Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_speed, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloatRange2", ExactSpelling = true)]
    public static extern bool DragFloatRange2([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v_current_min, ref float v_current_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragFloatRange2Ex", ExactSpelling = true)]
    public static extern bool DragFloatRange2Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v_current_min, ref float v_current_max, float v_speed, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, [MarshalAs(UnmanagedType.LPUTF8Str)] string format_max, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragInt", ExactSpelling = true)]
    public static extern bool DragInt([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragIntEx", ExactSpelling = true)]
    public static extern bool DragIntEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, float v_speed, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragInt2", ExactSpelling = true)]
    public static extern bool DragInt2([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragInt2Ex", ExactSpelling = true)]
    public static extern bool DragInt2Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, float v_speed, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragInt3", ExactSpelling = true)]
    public static extern bool DragInt3([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragInt3Ex", ExactSpelling = true)]
    public static extern bool DragInt3Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, float v_speed, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragInt4", ExactSpelling = true)]
    public static extern bool DragInt4([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragInt4Ex", ExactSpelling = true)]
    public static extern bool DragInt4Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, float v_speed, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragIntRange2", ExactSpelling = true)]
    public static extern bool DragIntRange2([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v_current_min, ref int v_current_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragIntRange2Ex", ExactSpelling = true)]
    public static extern bool DragIntRange2Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v_current_min, ref int v_current_max, float v_speed, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, [MarshalAs(UnmanagedType.LPUTF8Str)] string format_max, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragScalar", ExactSpelling = true)]
    public static extern bool DragScalar([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragScalarEx", ExactSpelling = true)]
    public static extern bool DragScalarEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, float v_speed, void* p_min, void* p_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragScalarN", ExactSpelling = true)]
    public static extern bool DragScalarN([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, int components);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DragScalarNEx", ExactSpelling = true)]
    public static extern bool DragScalarNEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, int components, float v_speed, void* p_min, void* p_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderFloat", ExactSpelling = true)]
    public static extern bool SliderFloat([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_min, float v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderFloatEx", ExactSpelling = true)]
    public static extern bool SliderFloatEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderFloat2", ExactSpelling = true)]
    public static extern bool SliderFloat2([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_min, float v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderFloat2Ex", ExactSpelling = true)]
    public static extern bool SliderFloat2Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderFloat3", ExactSpelling = true)]
    public static extern bool SliderFloat3([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_min, float v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderFloat3Ex", ExactSpelling = true)]
    public static extern bool SliderFloat3Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderFloat4", ExactSpelling = true)]
    public static extern bool SliderFloat4([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_min, float v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderFloat4Ex", ExactSpelling = true)]
    public static extern bool SliderFloat4Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderAngle", ExactSpelling = true)]
    public static extern bool SliderAngle([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v_rad);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderAngleEx", ExactSpelling = true)]
    public static extern bool SliderAngleEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v_rad, float v_degrees_min, float v_degrees_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderInt", ExactSpelling = true)]
    public static extern bool SliderInt([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int v_min, int v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderIntEx", ExactSpelling = true)]
    public static extern bool SliderIntEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderInt2", ExactSpelling = true)]
    public static extern bool SliderInt2([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int v_min, int v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderInt2Ex", ExactSpelling = true)]
    public static extern bool SliderInt2Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderInt3", ExactSpelling = true)]
    public static extern bool SliderInt3([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int v_min, int v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderInt3Ex", ExactSpelling = true)]
    public static extern bool SliderInt3Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderInt4", ExactSpelling = true)]
    public static extern bool SliderInt4([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int v_min, int v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderInt4Ex", ExactSpelling = true)]
    public static extern bool SliderInt4Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderScalar", ExactSpelling = true)]
    public static extern bool SliderScalar([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderScalarEx", ExactSpelling = true)]
    public static extern bool SliderScalarEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderScalarN", ExactSpelling = true)]
    public static extern bool SliderScalarN([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, int components, void* p_min, void* p_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SliderScalarNEx", ExactSpelling = true)]
    public static extern bool SliderScalarNEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, int components, void* p_min, void* p_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_VSliderFloat", ExactSpelling = true)]
    public static extern bool VSliderFloat([MarshalAs(UnmanagedType.LPUTF8Str)] string label, Vector2 size, ref float v, float v_min, float v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_VSliderFloatEx", ExactSpelling = true)]
    public static extern bool VSliderFloatEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, Vector2 size, ref float v, float v_min, float v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_VSliderInt", ExactSpelling = true)]
    public static extern bool VSliderInt([MarshalAs(UnmanagedType.LPUTF8Str)] string label, Vector2 size, ref int v, int v_min, int v_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_VSliderIntEx", ExactSpelling = true)]
    public static extern bool VSliderIntEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, Vector2 size, ref int v, int v_min, int v_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_VSliderScalar", ExactSpelling = true)]
    public static extern bool VSliderScalar([MarshalAs(UnmanagedType.LPUTF8Str)] string label, Vector2 size, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_VSliderScalarEx", ExactSpelling = true)]
    public static extern bool VSliderScalarEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, Vector2 size, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiSliderFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputText", ExactSpelling = true)]
    public static extern bool InputText([MarshalAs(UnmanagedType.LPUTF8Str)] string label, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputTextEx", ExactSpelling = true)]
    public static extern bool InputTextEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags, delegate* unmanaged[Cdecl]<nint, int> callback, void* user_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputTextMultiline", ExactSpelling = true)]
    public static extern bool InputTextMultiline([MarshalAs(UnmanagedType.LPUTF8Str)] string label, sbyte* buf, nuint buf_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputTextMultilineEx", ExactSpelling = true)]
    public static extern bool InputTextMultilineEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, sbyte* buf, nuint buf_size, Vector2 size, ImGuiInputTextFlags flags, delegate* unmanaged[Cdecl]<nint, int> callback, void* user_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputTextWithHint", ExactSpelling = true)]
    public static extern bool InputTextWithHint([MarshalAs(UnmanagedType.LPUTF8Str)] string label, [MarshalAs(UnmanagedType.LPUTF8Str)] string hint, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputTextWithHintEx", ExactSpelling = true)]
    public static extern bool InputTextWithHintEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, [MarshalAs(UnmanagedType.LPUTF8Str)] string hint, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags, delegate* unmanaged[Cdecl]<nint, int> callback, void* user_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputFloat", ExactSpelling = true)]
    public static extern bool InputFloat([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputFloatEx", ExactSpelling = true)]
    public static extern bool InputFloatEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, float step, float step_fast, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputFloat2", ExactSpelling = true)]
    public static extern bool InputFloat2([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputFloat2Ex", ExactSpelling = true)]
    public static extern bool InputFloat2Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputFloat3", ExactSpelling = true)]
    public static extern bool InputFloat3([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputFloat3Ex", ExactSpelling = true)]
    public static extern bool InputFloat3Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputFloat4", ExactSpelling = true)]
    public static extern bool InputFloat4([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputFloat4Ex", ExactSpelling = true)]
    public static extern bool InputFloat4Ex([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float v, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputInt", ExactSpelling = true)]
    public static extern bool InputInt([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputIntEx", ExactSpelling = true)]
    public static extern bool InputIntEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, int step, int step_fast, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputInt2", ExactSpelling = true)]
    public static extern bool InputInt2([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputInt3", ExactSpelling = true)]
    public static extern bool InputInt3([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputInt4", ExactSpelling = true)]
    public static extern bool InputInt4([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int v, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputDouble", ExactSpelling = true)]
    public static extern bool InputDouble([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref double v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputDoubleEx", ExactSpelling = true)]
    public static extern bool InputDoubleEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref double v, double step, double step_fast, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputScalar", ExactSpelling = true)]
    public static extern bool InputScalar([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputScalarEx", ExactSpelling = true)]
    public static extern bool InputScalarEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, void* p_step, void* p_step_fast, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputScalarN", ExactSpelling = true)]
    public static extern bool InputScalarN([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, int components);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_InputScalarNEx", ExactSpelling = true)]
    public static extern bool InputScalarNEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiDataType data_type, void* p_data, int components, void* p_step, void* p_step_fast, [MarshalAs(UnmanagedType.LPUTF8Str)] string format, ImGuiInputTextFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorEdit3", ExactSpelling = true)]
    public static extern bool ColorEdit3([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float col, ImGuiColorEditFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorEdit4", ExactSpelling = true)]
    public static extern bool ColorEdit4([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float col, ImGuiColorEditFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorPicker3", ExactSpelling = true)]
    public static extern bool ColorPicker3([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float col, ImGuiColorEditFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorPicker4", ExactSpelling = true)]
    public static extern bool ColorPicker4([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float col, ImGuiColorEditFlags flags, ref float ref_col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorButton", ExactSpelling = true)]
    public static extern bool ColorButton([MarshalAs(UnmanagedType.LPUTF8Str)] string desc_id, Vector4 col, ImGuiColorEditFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorButtonEx", ExactSpelling = true)]
    public static extern bool ColorButtonEx([MarshalAs(UnmanagedType.LPUTF8Str)] string desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetColorEditOptions", ExactSpelling = true)]
    public static extern void SetColorEditOptions(ImGuiColorEditFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNode", ExactSpelling = true)]
    public static extern bool TreeNode([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNodeStr", ExactSpelling = true)]
    public static extern bool TreeNodeStr([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNodePtr", ExactSpelling = true)]
    public static extern bool TreeNodePtr(void* ptr_id, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNodeV", ExactSpelling = true)]
    public static extern bool TreeNodeV([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNodeVPtr", ExactSpelling = true)]
    public static extern bool TreeNodeVPtr(void* ptr_id, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNodeEx", ExactSpelling = true)]
    public static extern bool TreeNodeEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiTreeNodeFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNodeExStr", ExactSpelling = true)]
    public static extern bool TreeNodeExStr([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiTreeNodeFlags flags, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNodeExPtr", ExactSpelling = true)]
    public static extern bool TreeNodeExPtr(void* ptr_id, ImGuiTreeNodeFlags flags, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNodeExV", ExactSpelling = true)]
    public static extern bool TreeNodeExV([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiTreeNodeFlags flags, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreeNodeExVPtr", ExactSpelling = true)]
    public static extern bool TreeNodeExVPtr(void* ptr_id, ImGuiTreeNodeFlags flags, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreePush", ExactSpelling = true)]
    public static extern void TreePush([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreePushPtr", ExactSpelling = true)]
    public static extern void TreePushPtr(void* ptr_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TreePop", ExactSpelling = true)]
    public static extern void TreePop();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetTreeNodeToLabelSpacing", ExactSpelling = true)]
    public static extern float GetTreeNodeToLabelSpacing();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_CollapsingHeader", ExactSpelling = true)]
    public static extern bool CollapsingHeader([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiTreeNodeFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_CollapsingHeaderBoolPtr", ExactSpelling = true)]
    public static extern bool CollapsingHeaderBoolPtr([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref bool p_visible, ImGuiTreeNodeFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextItemOpen", ExactSpelling = true)]
    public static extern void SetNextItemOpen(bool is_open, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextItemStorageID", ExactSpelling = true)]
    public static extern void SetNextItemStorageID(uint storage_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Selectable", ExactSpelling = true)]
    public static extern bool Selectable([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SelectableEx", ExactSpelling = true)]
    public static extern bool SelectableEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, bool selected, ImGuiSelectableFlags flags, Vector2 size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SelectableBoolPtr", ExactSpelling = true)]
    public static extern bool SelectableBoolPtr([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref bool p_selected, ImGuiSelectableFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SelectableBoolPtrEx", ExactSpelling = true)]
    public static extern bool SelectableBoolPtrEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref bool p_selected, ImGuiSelectableFlags flags, Vector2 size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginMultiSelect", ExactSpelling = true)]
    public static extern ImGuiMultiSelectIOStruct* BeginMultiSelect(ImGuiMultiSelectFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginMultiSelectEx", ExactSpelling = true)]
    public static extern ImGuiMultiSelectIOStruct* BeginMultiSelectEx(ImGuiMultiSelectFlags flags, int selection_size, int items_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndMultiSelect", ExactSpelling = true)]
    public static extern ImGuiMultiSelectIOStruct* EndMultiSelect();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextItemSelectionUserData", ExactSpelling = true)]
    public static extern void SetNextItemSelectionUserData(long selection_user_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemToggledSelection", ExactSpelling = true)]
    public static extern bool IsItemToggledSelection();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginListBox", ExactSpelling = true)]
    public static extern bool BeginListBox([MarshalAs(UnmanagedType.LPUTF8Str)] string label, Vector2 size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndListBox", ExactSpelling = true)]
    public static extern void EndListBox();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ListBox", ExactSpelling = true)]
    public static extern bool ListBox([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, sbyte** items, int items_count, int height_in_items);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ListBoxCallback", ExactSpelling = true)]
    public static extern bool ListBoxCallback([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ListBoxCallbackEx", ExactSpelling = true)]
    public static extern bool ListBoxCallbackEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count, int height_in_items);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PlotLines", ExactSpelling = true)]
    public static extern void PlotLines([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float values, int values_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PlotLinesEx", ExactSpelling = true)]
    public static extern void PlotLinesEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float values, int values_count, int values_offset, [MarshalAs(UnmanagedType.LPUTF8Str)] string overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PlotLinesCallback", ExactSpelling = true)]
    public static extern void PlotLinesCallback([MarshalAs(UnmanagedType.LPUTF8Str)] string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PlotLinesCallbackEx", ExactSpelling = true)]
    public static extern void PlotLinesCallbackEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count, int values_offset, [MarshalAs(UnmanagedType.LPUTF8Str)] string overlay_text, float scale_min, float scale_max, Vector2 graph_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PlotHistogram", ExactSpelling = true)]
    public static extern void PlotHistogram([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float values, int values_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PlotHistogramEx", ExactSpelling = true)]
    public static extern void PlotHistogramEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref float values, int values_count, int values_offset, [MarshalAs(UnmanagedType.LPUTF8Str)] string overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PlotHistogramCallback", ExactSpelling = true)]
    public static extern void PlotHistogramCallback([MarshalAs(UnmanagedType.LPUTF8Str)] string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PlotHistogramCallbackEx", ExactSpelling = true)]
    public static extern void PlotHistogramCallbackEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count, int values_offset, [MarshalAs(UnmanagedType.LPUTF8Str)] string overlay_text, float scale_min, float scale_max, Vector2 graph_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginMenuBar", ExactSpelling = true)]
    public static extern bool BeginMenuBar();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndMenuBar", ExactSpelling = true)]
    public static extern void EndMenuBar();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginMainMenuBar", ExactSpelling = true)]
    public static extern bool BeginMainMenuBar();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndMainMenuBar", ExactSpelling = true)]
    public static extern void EndMainMenuBar();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginMenu", ExactSpelling = true)]
    public static extern bool BeginMenu([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginMenuEx", ExactSpelling = true)]
    public static extern bool BeginMenuEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, bool enabled);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndMenu", ExactSpelling = true)]
    public static extern void EndMenu();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_MenuItem", ExactSpelling = true)]
    public static extern bool MenuItem([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_MenuItemEx", ExactSpelling = true)]
    public static extern bool MenuItemEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, [MarshalAs(UnmanagedType.LPUTF8Str)] string shortcut, bool selected, bool enabled);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_MenuItemBoolPtr", ExactSpelling = true)]
    public static extern bool MenuItemBoolPtr([MarshalAs(UnmanagedType.LPUTF8Str)] string label, [MarshalAs(UnmanagedType.LPUTF8Str)] string shortcut, ref bool p_selected, bool enabled);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginTooltip", ExactSpelling = true)]
    public static extern bool BeginTooltip();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndTooltip", ExactSpelling = true)]
    public static extern void EndTooltip();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetTooltip", ExactSpelling = true)]
    public static extern void SetTooltip([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetTooltipV", ExactSpelling = true)]
    public static extern void SetTooltipV([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginItemTooltip", ExactSpelling = true)]
    public static extern bool BeginItemTooltip();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetItemTooltip", ExactSpelling = true)]
    public static extern void SetItemTooltip([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetItemTooltipV", ExactSpelling = true)]
    public static extern void SetItemTooltipV([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginPopup", ExactSpelling = true)]
    public static extern bool BeginPopup([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiWindowFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginPopupModal", ExactSpelling = true)]
    public static extern bool BeginPopupModal([MarshalAs(UnmanagedType.LPUTF8Str)] string name, ref bool p_open, ImGuiWindowFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndPopup", ExactSpelling = true)]
    public static extern void EndPopup();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_OpenPopup", ExactSpelling = true)]
    public static extern void OpenPopup([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiPopupFlags popup_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_OpenPopupID", ExactSpelling = true)]
    public static extern void OpenPopupID(uint id, ImGuiPopupFlags popup_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_OpenPopupOnItemClick", ExactSpelling = true)]
    public static extern void OpenPopupOnItemClick([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiPopupFlags popup_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_CloseCurrentPopup", ExactSpelling = true)]
    public static extern void CloseCurrentPopup();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginPopupContextItem", ExactSpelling = true)]
    public static extern bool BeginPopupContextItem();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginPopupContextItemEx", ExactSpelling = true)]
    public static extern bool BeginPopupContextItemEx([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiPopupFlags popup_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginPopupContextWindow", ExactSpelling = true)]
    public static extern bool BeginPopupContextWindow();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginPopupContextWindowEx", ExactSpelling = true)]
    public static extern bool BeginPopupContextWindowEx([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiPopupFlags popup_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginPopupContextVoid", ExactSpelling = true)]
    public static extern bool BeginPopupContextVoid();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginPopupContextVoidEx", ExactSpelling = true)]
    public static extern bool BeginPopupContextVoidEx([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiPopupFlags popup_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsPopupOpen", ExactSpelling = true)]
    public static extern bool IsPopupOpen([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiPopupFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginTable", ExactSpelling = true)]
    public static extern bool BeginTable([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, int columns, ImGuiTableFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginTableEx", ExactSpelling = true)]
    public static extern bool BeginTableEx([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, int columns, ImGuiTableFlags flags, Vector2 outer_size, float inner_width);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndTable", ExactSpelling = true)]
    public static extern void EndTable();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableNextRow", ExactSpelling = true)]
    public static extern void TableNextRow();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableNextRowEx", ExactSpelling = true)]
    public static extern void TableNextRowEx(ImGuiTableRowFlags row_flags, float min_row_height);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableNextColumn", ExactSpelling = true)]
    public static extern bool TableNextColumn();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableSetColumnIndex", ExactSpelling = true)]
    public static extern bool TableSetColumnIndex(int column_n);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableSetupColumn", ExactSpelling = true)]
    public static extern void TableSetupColumn([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiTableColumnFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableSetupColumnEx", ExactSpelling = true)]
    public static extern void TableSetupColumnEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiTableColumnFlags flags, float init_width_or_weight, uint user_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableSetupScrollFreeze", ExactSpelling = true)]
    public static extern void TableSetupScrollFreeze(int cols, int rows);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableHeader", ExactSpelling = true)]
    public static extern void TableHeader([MarshalAs(UnmanagedType.LPUTF8Str)] string label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableHeadersRow", ExactSpelling = true)]
    public static extern void TableHeadersRow();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableAngledHeadersRow", ExactSpelling = true)]
    public static extern void TableAngledHeadersRow();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableGetSortSpecs", ExactSpelling = true)]
    public static extern ImGuiTableSortSpecsStruct* TableGetSortSpecs();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableGetColumnCount", ExactSpelling = true)]
    public static extern int TableGetColumnCount();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableGetColumnIndex", ExactSpelling = true)]
    public static extern int TableGetColumnIndex();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableGetRowIndex", ExactSpelling = true)]
    public static extern int TableGetRowIndex();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableGetColumnName", ExactSpelling = true)]
    public static extern sbyte* TableGetColumnName(int column_n);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableGetColumnFlags", ExactSpelling = true)]
    public static extern ImGuiTableColumnFlags TableGetColumnFlags(int column_n);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableSetColumnEnabled", ExactSpelling = true)]
    public static extern void TableSetColumnEnabled(int column_n, bool v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableGetHoveredColumn", ExactSpelling = true)]
    public static extern int TableGetHoveredColumn();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TableSetBgColor", ExactSpelling = true)]
    public static extern void TableSetBgColor(ImGuiTableBgTarget target, uint color, int column_n);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Columns", ExactSpelling = true)]
    public static extern void Columns();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColumnsEx", ExactSpelling = true)]
    public static extern void ColumnsEx(int count, [MarshalAs(UnmanagedType.LPUTF8Str)] string id, bool borders);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_NextColumn", ExactSpelling = true)]
    public static extern void NextColumn();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetColumnIndex", ExactSpelling = true)]
    public static extern int GetColumnIndex();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetColumnWidth", ExactSpelling = true)]
    public static extern float GetColumnWidth(int column_index);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetColumnWidth", ExactSpelling = true)]
    public static extern void SetColumnWidth(int column_index, float width);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetColumnOffset", ExactSpelling = true)]
    public static extern float GetColumnOffset(int column_index);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetColumnOffset", ExactSpelling = true)]
    public static extern void SetColumnOffset(int column_index, float offset_x);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetColumnsCount", ExactSpelling = true)]
    public static extern int GetColumnsCount();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginTabBar", ExactSpelling = true)]
    public static extern bool BeginTabBar([MarshalAs(UnmanagedType.LPUTF8Str)] string str_id, ImGuiTabBarFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndTabBar", ExactSpelling = true)]
    public static extern void EndTabBar();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginTabItem", ExactSpelling = true)]
    public static extern bool BeginTabItem([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref bool p_open, ImGuiTabItemFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndTabItem", ExactSpelling = true)]
    public static extern void EndTabItem();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_TabItemButton", ExactSpelling = true)]
    public static extern bool TabItemButton([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ImGuiTabItemFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetTabItemClosed", ExactSpelling = true)]
    public static extern void SetTabItemClosed([MarshalAs(UnmanagedType.LPUTF8Str)] string tab_or_docked_window_label);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DockSpace", ExactSpelling = true)]
    public static extern uint DockSpace(uint dockspace_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DockSpaceEx", ExactSpelling = true)]
    public static extern uint DockSpaceEx(uint dockspace_id, Vector2 size, ImGuiDockNodeFlags flags, ImGuiWindowClassStruct* window_class);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DockSpaceOverViewport", ExactSpelling = true)]
    public static extern uint DockSpaceOverViewport();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DockSpaceOverViewportEx", ExactSpelling = true)]
    public static extern uint DockSpaceOverViewportEx(uint dockspace_id, ImGuiViewportStruct* viewport, ImGuiDockNodeFlags flags, ImGuiWindowClassStruct* window_class);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowDockID", ExactSpelling = true)]
    public static extern void SetNextWindowDockID(uint dock_id, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextWindowClass", ExactSpelling = true)]
    public static extern void SetNextWindowClass(ImGuiWindowClassStruct* window_class);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowDockID", ExactSpelling = true)]
    public static extern uint GetWindowDockID();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsWindowDocked", ExactSpelling = true)]
    public static extern bool IsWindowDocked();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LogToTTY", ExactSpelling = true)]
    public static extern void LogToTTY(int auto_open_depth);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LogToFile", ExactSpelling = true)]
    public static extern void LogToFile(int auto_open_depth, [MarshalAs(UnmanagedType.LPUTF8Str)] string filename);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LogToClipboard", ExactSpelling = true)]
    public static extern void LogToClipboard(int auto_open_depth);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LogFinish", ExactSpelling = true)]
    public static extern void LogFinish();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LogButtons", ExactSpelling = true)]
    public static extern void LogButtons();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LogText", ExactSpelling = true)]
    public static extern void LogText([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LogTextV", ExactSpelling = true)]
    public static extern void LogTextV([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginDragDropSource", ExactSpelling = true)]
    public static extern bool BeginDragDropSource(ImGuiDragDropFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetDragDropPayload", ExactSpelling = true)]
    public static extern bool SetDragDropPayload([MarshalAs(UnmanagedType.LPUTF8Str)] string type, void* data, nuint sz, ImGuiCond cond);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndDragDropSource", ExactSpelling = true)]
    public static extern void EndDragDropSource();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginDragDropTarget", ExactSpelling = true)]
    public static extern bool BeginDragDropTarget();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_AcceptDragDropPayload", ExactSpelling = true)]
    public static extern ImGuiPayloadStruct* AcceptDragDropPayload([MarshalAs(UnmanagedType.LPUTF8Str)] string type, ImGuiDragDropFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndDragDropTarget", ExactSpelling = true)]
    public static extern void EndDragDropTarget();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetDragDropPayload", ExactSpelling = true)]
    public static extern ImGuiPayloadStruct* GetDragDropPayload();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginDisabled", ExactSpelling = true)]
    public static extern void BeginDisabled(bool disabled);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndDisabled", ExactSpelling = true)]
    public static extern void EndDisabled();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushClipRect", ExactSpelling = true)]
    public static extern void PushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, bool intersect_with_current_clip_rect);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopClipRect", ExactSpelling = true)]
    public static extern void PopClipRect();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetItemDefaultFocus", ExactSpelling = true)]
    public static extern void SetItemDefaultFocus();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetKeyboardFocusHere", ExactSpelling = true)]
    public static extern void SetKeyboardFocusHere();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetKeyboardFocusHereEx", ExactSpelling = true)]
    public static extern void SetKeyboardFocusHereEx(int offset);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNavCursorVisible", ExactSpelling = true)]
    public static extern void SetNavCursorVisible(bool visible);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextItemAllowOverlap", ExactSpelling = true)]
    public static extern void SetNextItemAllowOverlap();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemHovered", ExactSpelling = true)]
    public static extern bool IsItemHovered(ImGuiHoveredFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemActive", ExactSpelling = true)]
    public static extern bool IsItemActive();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemFocused", ExactSpelling = true)]
    public static extern bool IsItemFocused();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemClicked", ExactSpelling = true)]
    public static extern bool IsItemClicked();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemClickedEx", ExactSpelling = true)]
    public static extern bool IsItemClickedEx(ImGuiMouseButton mouse_button);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemVisible", ExactSpelling = true)]
    public static extern bool IsItemVisible();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemEdited", ExactSpelling = true)]
    public static extern bool IsItemEdited();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemActivated", ExactSpelling = true)]
    public static extern bool IsItemActivated();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemDeactivated", ExactSpelling = true)]
    public static extern bool IsItemDeactivated();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemDeactivatedAfterEdit", ExactSpelling = true)]
    public static extern bool IsItemDeactivatedAfterEdit();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsItemToggledOpen", ExactSpelling = true)]
    public static extern bool IsItemToggledOpen();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsAnyItemHovered", ExactSpelling = true)]
    public static extern bool IsAnyItemHovered();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsAnyItemActive", ExactSpelling = true)]
    public static extern bool IsAnyItemActive();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsAnyItemFocused", ExactSpelling = true)]
    public static extern bool IsAnyItemFocused();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetItemID", ExactSpelling = true)]
    public static extern uint GetItemID();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetItemRectMin", ExactSpelling = true)]
    public static extern Vector2 GetItemRectMin();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetItemRectMax", ExactSpelling = true)]
    public static extern Vector2 GetItemRectMax();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetItemRectSize", ExactSpelling = true)]
    public static extern Vector2 GetItemRectSize();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetMainViewport", ExactSpelling = true)]
    public static extern ImGuiViewportStruct* GetMainViewport();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetBackgroundDrawList", ExactSpelling = true)]
    public static extern ImDrawListStruct* GetBackgroundDrawList();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetBackgroundDrawListEx", ExactSpelling = true)]
    public static extern ImDrawListStruct* GetBackgroundDrawListEx(ImGuiViewportStruct* viewport);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetForegroundDrawList", ExactSpelling = true)]
    public static extern ImDrawListStruct* GetForegroundDrawList();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetForegroundDrawListEx", ExactSpelling = true)]
    public static extern ImDrawListStruct* GetForegroundDrawListEx(ImGuiViewportStruct* viewport);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsRectVisibleBySize", ExactSpelling = true)]
    public static extern bool IsRectVisibleBySize(Vector2 size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsRectVisible", ExactSpelling = true)]
    public static extern bool IsRectVisible(Vector2 rect_min, Vector2 rect_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetTime", ExactSpelling = true)]
    public static extern double GetTime();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetFrameCount", ExactSpelling = true)]
    public static extern int GetFrameCount();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetDrawListSharedData", ExactSpelling = true)]
    public static extern nint GetDrawListSharedData();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetStyleColorName", ExactSpelling = true)]
    public static extern sbyte* GetStyleColorName(ImGuiCol idx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetStateStorage", ExactSpelling = true)]
    public static extern void SetStateStorage(ImGuiStorageStruct* storage);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetStateStorage", ExactSpelling = true)]
    public static extern ImGuiStorageStruct* GetStateStorage();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_CalcTextSize", ExactSpelling = true)]
    public static extern Vector2 CalcTextSize([MarshalAs(UnmanagedType.LPUTF8Str)] string text);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_CalcTextSizeEx", ExactSpelling = true)]
    public static extern Vector2 CalcTextSizeEx([MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end, bool hide_text_after_double_hash, float wrap_width);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorConvertU32ToFloat4", ExactSpelling = true)]
    public static extern Vector4 ColorConvertU32ToFloat4(uint @in);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorConvertFloat4ToU32", ExactSpelling = true)]
    public static extern uint ColorConvertFloat4ToU32(Vector4 @in);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorConvertRGBtoHSV", ExactSpelling = true)]
    public static extern void ColorConvertRGBtoHSV(float r, float g, float b, ref float out_h, ref float out_s, ref float out_v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ColorConvertHSVtoRGB", ExactSpelling = true)]
    public static extern void ColorConvertHSVtoRGB(float h, float s, float v, ref float out_r, ref float out_g, ref float out_b);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsKeyDown", ExactSpelling = true)]
    public static extern bool IsKeyDown(int key);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsKeyPressed", ExactSpelling = true)]
    public static extern bool IsKeyPressed(int key);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsKeyPressedEx", ExactSpelling = true)]
    public static extern bool IsKeyPressedEx(int key, bool repeat);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsKeyReleased", ExactSpelling = true)]
    public static extern bool IsKeyReleased(int key);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsKeyChordPressed", ExactSpelling = true)]
    public static extern bool IsKeyChordPressed(int key_chord);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetKeyPressedAmount", ExactSpelling = true)]
    public static extern int GetKeyPressedAmount(int key, float repeat_delay, float rate);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetKeyName", ExactSpelling = true)]
    public static extern sbyte* GetKeyName(int key);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextFrameWantCaptureKeyboard", ExactSpelling = true)]
    public static extern void SetNextFrameWantCaptureKeyboard(bool want_capture_keyboard);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_Shortcut", ExactSpelling = true)]
    public static extern bool Shortcut(int key_chord, ImGuiInputFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextItemShortcut", ExactSpelling = true)]
    public static extern void SetNextItemShortcut(int key_chord, ImGuiInputFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetItemKeyOwner", ExactSpelling = true)]
    public static extern void SetItemKeyOwner(int key);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMouseDown", ExactSpelling = true)]
    public static extern bool IsMouseDown(ImGuiMouseButton button);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMouseClicked", ExactSpelling = true)]
    public static extern bool IsMouseClicked(ImGuiMouseButton button);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMouseClickedEx", ExactSpelling = true)]
    public static extern bool IsMouseClickedEx(ImGuiMouseButton button, bool repeat);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMouseReleased", ExactSpelling = true)]
    public static extern bool IsMouseReleased(ImGuiMouseButton button);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMouseDoubleClicked", ExactSpelling = true)]
    public static extern bool IsMouseDoubleClicked(ImGuiMouseButton button);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMouseReleasedWithDelay", ExactSpelling = true)]
    public static extern bool IsMouseReleasedWithDelay(ImGuiMouseButton button, float delay);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetMouseClickedCount", ExactSpelling = true)]
    public static extern int GetMouseClickedCount(ImGuiMouseButton button);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMouseHoveringRect", ExactSpelling = true)]
    public static extern bool IsMouseHoveringRect(Vector2 r_min, Vector2 r_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMouseHoveringRectEx", ExactSpelling = true)]
    public static extern bool IsMouseHoveringRectEx(Vector2 r_min, Vector2 r_max, bool clip);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMousePosValid", ExactSpelling = true)]
    public static extern bool IsMousePosValid(Vector2 mouse_pos);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsAnyMouseDown", ExactSpelling = true)]
    public static extern bool IsAnyMouseDown();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetMousePos", ExactSpelling = true)]
    public static extern Vector2 GetMousePos();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetMousePosOnOpeningCurrentPopup", ExactSpelling = true)]
    public static extern Vector2 GetMousePosOnOpeningCurrentPopup();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_IsMouseDragging", ExactSpelling = true)]
    public static extern bool IsMouseDragging(ImGuiMouseButton button, float lock_threshold);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetMouseDragDelta", ExactSpelling = true)]
    public static extern Vector2 GetMouseDragDelta(ImGuiMouseButton button, float lock_threshold);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ResetMouseDragDelta", ExactSpelling = true)]
    public static extern void ResetMouseDragDelta();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ResetMouseDragDeltaEx", ExactSpelling = true)]
    public static extern void ResetMouseDragDeltaEx(ImGuiMouseButton button);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetMouseCursor", ExactSpelling = true)]
    public static extern ImGuiMouseCursor GetMouseCursor();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetMouseCursor", ExactSpelling = true)]
    public static extern void SetMouseCursor(ImGuiMouseCursor cursor_type);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetNextFrameWantCaptureMouse", ExactSpelling = true)]
    public static extern void SetNextFrameWantCaptureMouse(bool want_capture_mouse);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetClipboardText", ExactSpelling = true)]
    public static extern sbyte* GetClipboardText();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetClipboardText", ExactSpelling = true)]
    public static extern void SetClipboardText([MarshalAs(UnmanagedType.LPUTF8Str)] string text);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LoadIniSettingsFromDisk", ExactSpelling = true)]
    public static extern void LoadIniSettingsFromDisk([MarshalAs(UnmanagedType.LPUTF8Str)] string ini_filename);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_LoadIniSettingsFromMemory", ExactSpelling = true)]
    public static extern void LoadIniSettingsFromMemory([MarshalAs(UnmanagedType.LPUTF8Str)] string ini_data, nuint ini_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SaveIniSettingsToDisk", ExactSpelling = true)]
    public static extern void SaveIniSettingsToDisk([MarshalAs(UnmanagedType.LPUTF8Str)] string ini_filename);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SaveIniSettingsToMemory", ExactSpelling = true)]
    public static extern sbyte* SaveIniSettingsToMemory(nuint out_ini_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DebugTextEncoding", ExactSpelling = true)]
    public static extern void DebugTextEncoding([MarshalAs(UnmanagedType.LPUTF8Str)] string text);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DebugFlashStyleColor", ExactSpelling = true)]
    public static extern void DebugFlashStyleColor(ImGuiCol idx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DebugStartItemPicker", ExactSpelling = true)]
    public static extern void DebugStartItemPicker();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DebugCheckVersionAndDataLayout", ExactSpelling = true)]
    public static extern bool DebugCheckVersionAndDataLayout([MarshalAs(UnmanagedType.LPUTF8Str)] string version_str, nuint sz_io, nuint sz_style, nuint sz_vec2, nuint sz_vec4, nuint sz_drawvert, nuint sz_drawidx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DebugLog", ExactSpelling = true)]
    public static extern void DebugLog([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DebugLogV", ExactSpelling = true)]
    public static extern void DebugLogV([MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetAllocatorFunctions", ExactSpelling = true)]
    public static extern void SetAllocatorFunctions(delegate* unmanaged[Cdecl]<nuint, nint, nint> alloc_func, delegate* unmanaged[Cdecl]<nint, nint, void> free_func, void* user_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetAllocatorFunctions", ExactSpelling = true)]
    public static extern void GetAllocatorFunctions(delegate* unmanaged[Cdecl]<nuint, void*, void*> * p_alloc_func, delegate* unmanaged[Cdecl]<void*, void*, void> * p_free_func, void** p_user_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_MemAlloc", ExactSpelling = true)]
    public static extern void* MemAlloc(nuint size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_MemFree", ExactSpelling = true)]
    public static extern void MemFree(void* ptr);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_UpdatePlatformWindows", ExactSpelling = true)]
    public static extern void UpdatePlatformWindows();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_RenderPlatformWindowsDefault", ExactSpelling = true)]
    public static extern void RenderPlatformWindowsDefault();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_RenderPlatformWindowsDefaultEx", ExactSpelling = true)]
    public static extern void RenderPlatformWindowsDefaultEx(void* platform_render_arg, void* renderer_render_arg);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_DestroyPlatformWindows", ExactSpelling = true)]
    public static extern void DestroyPlatformWindows();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_FindViewportByID", ExactSpelling = true)]
    public static extern ImGuiViewportStruct* FindViewportByID(uint id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_FindViewportByPlatformHandle", ExactSpelling = true)]
    public static extern ImGuiViewportStruct* FindViewportByPlatformHandle(void* platform_handle);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImVector_Construct(void* vector);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImVector_Destruct(void* vector);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiPlatformIO_SetPlatform_GetWindowWorkAreaInsets(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowWorkAreaInsetsFunc);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiPlatformIO_SetPlatform_GetWindowFramebufferScale(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowFramebufferScaleFunc);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiPlatformIO_SetPlatform_GetWindowPos(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowPosFunc);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiPlatformIO_SetPlatform_GetWindowSize(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowSizeFunc);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiStyle_ScaleAllSizes(ImGuiStyleStruct* self, float scale_factor);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddKeyEvent(ImGuiIOStruct* self, int key, bool down);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddKeyAnalogEvent(ImGuiIOStruct* self, int key, bool down, float v);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddMousePosEvent(ImGuiIOStruct* self, float x, float y);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddMouseButtonEvent(ImGuiIOStruct* self, int button, bool down);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddMouseWheelEvent(ImGuiIOStruct* self, float wheel_x, float wheel_y);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddMouseSourceEvent(ImGuiIOStruct* self, int source);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddMouseViewportEvent(ImGuiIOStruct* self, uint id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddFocusEvent(ImGuiIOStruct* self, bool focused);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddInputCharacter(ImGuiIOStruct* self, uint c);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddInputCharacterUTF16(ImGuiIOStruct* self, ushort c);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_AddInputCharactersUTF8(ImGuiIOStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string str);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_SetKeyEventNativeData(ImGuiIOStruct* self, int key, int native_keycode, int native_scancode);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_SetKeyEventNativeDataEx(ImGuiIOStruct* self, int key, int native_keycode, int native_scancode, int native_legacy_index);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_SetAppAcceptingEvents(ImGuiIOStruct* self, bool accepting_events);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_ClearEventsQueue(ImGuiIOStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_ClearInputKeys(ImGuiIOStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_ClearInputMouse(ImGuiIOStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiIO_ClearInputCharacters(ImGuiIOStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiInputTextCallbackData_DeleteChars(ImGuiInputTextCallbackDataStruct* self, int pos, int bytes_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiInputTextCallbackData_InsertChars(ImGuiInputTextCallbackDataStruct* self, int pos, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiInputTextCallbackData_SelectAll(ImGuiInputTextCallbackDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiInputTextCallbackData_ClearSelection(ImGuiInputTextCallbackDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiInputTextCallbackData_HasSelection(ImGuiInputTextCallbackDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiPayload_Clear(ImGuiPayloadStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiPayload_IsDataType(ImGuiPayloadStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string type);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiPayload_IsPreview(ImGuiPayloadStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiPayload_IsDelivery(ImGuiPayloadStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiTextFilter_ImGuiTextRange_empty(ImGuiTextFilter_ImGuiTextRangeStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiTextFilter_ImGuiTextRange_split(ImGuiTextFilter_ImGuiTextRangeStruct* self, sbyte separator, ref ImVector<ImGuiTextFilter_ImGuiTextRangeStruct> @out);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiTextFilter_Draw(ImGuiTextFilterStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string label, float width);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiTextFilter_PassFilter(ImGuiTextFilterStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiTextFilter_Build(ImGuiTextFilterStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiTextFilter_Clear(ImGuiTextFilterStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiTextFilter_IsActive(ImGuiTextFilterStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern sbyte* ImGuiTextBuffer_begin(ImGuiTextBufferStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern sbyte* ImGuiTextBuffer_end(ImGuiTextBufferStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int ImGuiTextBuffer_size(ImGuiTextBufferStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiTextBuffer_empty(ImGuiTextBufferStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiTextBuffer_clear(ImGuiTextBufferStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiTextBuffer_resize(ImGuiTextBufferStruct* self, int size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiTextBuffer_reserve(ImGuiTextBufferStruct* self, int capacity);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern sbyte* ImGuiTextBuffer_c_str(ImGuiTextBufferStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiTextBuffer_append(ImGuiTextBufferStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string str, [MarshalAs(UnmanagedType.LPUTF8Str)] string str_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiTextBuffer_appendf(ImGuiTextBufferStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiTextBuffer_appendfv(ImGuiTextBufferStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, sbyte* args);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiStorage_Clear(ImGuiStorageStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int ImGuiStorage_GetInt(ImGuiStorageStruct* self, uint key, int default_val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiStorage_SetInt(ImGuiStorageStruct* self, uint key, int val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiStorage_GetBool(ImGuiStorageStruct* self, uint key, bool default_val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiStorage_SetBool(ImGuiStorageStruct* self, uint key, bool val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern float ImGuiStorage_GetFloat(ImGuiStorageStruct* self, uint key, float default_val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiStorage_SetFloat(ImGuiStorageStruct* self, uint key, float val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void* ImGuiStorage_GetVoidPtr(ImGuiStorageStruct* self, uint key);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiStorage_SetVoidPtr(ImGuiStorageStruct* self, uint key, void* val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int* ImGuiStorage_GetIntRef(ImGuiStorageStruct* self, uint key, int default_val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool* ImGuiStorage_GetBoolRef(ImGuiStorageStruct* self, uint key, bool default_val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern float* ImGuiStorage_GetFloatRef(ImGuiStorageStruct* self, uint key, float default_val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void** ImGuiStorage_GetVoidPtrRef(ImGuiStorageStruct* self, uint key, void* default_val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiStorage_BuildSortByKey(ImGuiStorageStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiStorage_SetAllInt(ImGuiStorageStruct* self, int val);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiListClipper_Begin(ImGuiListClipperStruct* self, int items_count, float items_height);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiListClipper_End(ImGuiListClipperStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiListClipper_Step(ImGuiListClipperStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiListClipper_IncludeItemByIndex(ImGuiListClipperStruct* self, int item_index);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiListClipper_IncludeItemsByIndex(ImGuiListClipperStruct* self, int item_begin, int item_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiListClipper_SeekCursorForItem(ImGuiListClipperStruct* self, int item_index);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiListClipper_IncludeRangeByIndices(ImGuiListClipperStruct* self, int item_begin, int item_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImColor_SetHSV(Vector4 self, float h, float s, float v, float a);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern Vector4 ImColor_HSV(float h, float s, float v, float a);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiSelectionBasicStorage_ApplyRequests(ImGuiSelectionBasicStorageStruct* self, ImGuiMultiSelectIOStruct* ms_io);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiSelectionBasicStorage_Contains(ImGuiSelectionBasicStorageStruct* self, uint id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiSelectionBasicStorage_Clear(ImGuiSelectionBasicStorageStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiSelectionBasicStorage_Swap(ImGuiSelectionBasicStorageStruct* self, ImGuiSelectionBasicStorageStruct* r);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiSelectionBasicStorage_SetItemSelected(ImGuiSelectionBasicStorageStruct* self, uint id, bool selected);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiSelectionBasicStorage_GetNextSelectedItem(ImGuiSelectionBasicStorageStruct* self, void** opaque_it, ref uint out_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint ImGuiSelectionBasicStorage_GetStorageIdFromIndex(ImGuiSelectionBasicStorageStruct* self, int idx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiSelectionExternalStorage_ApplyRequests(ImGuiSelectionExternalStorageStruct* self, ImGuiMultiSelectIOStruct* ms_io);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ulong ImDrawCmd_GetTexID(ImDrawCmdStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawListSplitter_Clear(ImDrawListSplitterStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawListSplitter_ClearFreeMemory(ImDrawListSplitterStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawListSplitter_Split(ImDrawListSplitterStruct* self, ImDrawListStruct* draw_list, int count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawListSplitter_Merge(ImDrawListSplitterStruct* self, ImDrawListStruct* draw_list);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawListSplitter_SetCurrentChannel(ImDrawListSplitterStruct* self, ImDrawListStruct* draw_list, int channel_idx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PushClipRect(ImDrawListStruct* self, Vector2 clip_rect_min, Vector2 clip_rect_max, bool intersect_with_current_clip_rect);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PushClipRectFullScreen(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PopClipRect(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PushTexture(ImDrawListStruct* self, ImTextureRef tex_ref);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PopTexture(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern Vector2 ImDrawList_GetClipRectMin(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern Vector2 ImDrawList_GetClipRectMax(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddLine(ImDrawListStruct* self, Vector2 p1, Vector2 p2, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddLineEx(ImDrawListStruct* self, Vector2 p1, Vector2 p2, uint col, float thickness);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddRect(ImDrawListStruct* self, Vector2 p_min, Vector2 p_max, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddRectEx(ImDrawListStruct* self, Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags, float thickness);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddRectFilled(ImDrawListStruct* self, Vector2 p_min, Vector2 p_max, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddRectFilledEx(ImDrawListStruct* self, Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddRectFilledMultiColor(ImDrawListStruct* self, Vector2 p_min, Vector2 p_max, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddQuad(ImDrawListStruct* self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddQuadEx(ImDrawListStruct* self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddQuadFilled(ImDrawListStruct* self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddTriangle(ImDrawListStruct* self, Vector2 p1, Vector2 p2, Vector2 p3, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddTriangleEx(ImDrawListStruct* self, Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddTriangleFilled(ImDrawListStruct* self, Vector2 p1, Vector2 p2, Vector2 p3, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddCircle(ImDrawListStruct* self, Vector2 center, float radius, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddCircleEx(ImDrawListStruct* self, Vector2 center, float radius, uint col, int num_segments, float thickness);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddCircleFilled(ImDrawListStruct* self, Vector2 center, float radius, uint col, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddNgon(ImDrawListStruct* self, Vector2 center, float radius, uint col, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddNgonEx(ImDrawListStruct* self, Vector2 center, float radius, uint col, int num_segments, float thickness);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddNgonFilled(ImDrawListStruct* self, Vector2 center, float radius, uint col, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddEllipse(ImDrawListStruct* self, Vector2 center, Vector2 radius, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddEllipseEx(ImDrawListStruct* self, Vector2 center, Vector2 radius, uint col, float rot, int num_segments, float thickness);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddEllipseFilled(ImDrawListStruct* self, Vector2 center, Vector2 radius, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddEllipseFilledEx(ImDrawListStruct* self, Vector2 center, Vector2 radius, uint col, float rot, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddText(ImDrawListStruct* self, Vector2 pos, uint col, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_begin);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddTextEx(ImDrawListStruct* self, Vector2 pos, uint col, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_begin, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddTextImFontPtr(ImDrawListStruct* self, ImFontStruct* font, float font_size, Vector2 pos, uint col, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_begin);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddTextImFontPtrEx(ImDrawListStruct* self, ImFontStruct* font, float font_size, Vector2 pos, uint col, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_begin, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end, float wrap_width, Vector4 cpu_fine_clip_rect);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddBezierCubic(ImDrawListStruct* self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddBezierQuadratic(ImDrawListStruct* self, Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddPolyline(ImDrawListStruct* self, Vector2 points, int num_points, uint col, ImDrawFlags flags, float thickness);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddConvexPolyFilled(ImDrawListStruct* self, Vector2 points, int num_points, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddConcavePolyFilled(ImDrawListStruct* self, Vector2 points, int num_points, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddImage(ImDrawListStruct* self, ImTextureRef tex_ref, Vector2 p_min, Vector2 p_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddImageEx(ImDrawListStruct* self, ImTextureRef tex_ref, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddImageQuad(ImDrawListStruct* self, ImTextureRef tex_ref, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddImageQuadEx(ImDrawListStruct* self, ImTextureRef tex_ref, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddImageRounded(ImDrawListStruct* self, ImTextureRef tex_ref, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col, float rounding, ImDrawFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathClear(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathLineTo(ImDrawListStruct* self, Vector2 pos);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathLineToMergeDuplicate(ImDrawListStruct* self, Vector2 pos);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathFillConvex(ImDrawListStruct* self, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathFillConcave(ImDrawListStruct* self, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathStroke(ImDrawListStruct* self, uint col, ImDrawFlags flags, float thickness);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathArcTo(ImDrawListStruct* self, Vector2 center, float radius, float a_min, float a_max, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathArcToFast(ImDrawListStruct* self, Vector2 center, float radius, int a_min_of_12, int a_max_of_12);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathEllipticalArcTo(ImDrawListStruct* self, Vector2 center, Vector2 radius, float rot, float a_min, float a_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathEllipticalArcToEx(ImDrawListStruct* self, Vector2 center, Vector2 radius, float rot, float a_min, float a_max, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathBezierCubicCurveTo(ImDrawListStruct* self, Vector2 p2, Vector2 p3, Vector2 p4, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathBezierQuadraticCurveTo(ImDrawListStruct* self, Vector2 p2, Vector2 p3, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PathRect(ImDrawListStruct* self, Vector2 rect_min, Vector2 rect_max, float rounding, ImDrawFlags flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddCallback(ImDrawListStruct* self, delegate* unmanaged[Cdecl]<nint, nint, void> callback, void* userdata);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddCallbackEx(ImDrawListStruct* self, delegate* unmanaged[Cdecl]<nint, nint, void> callback, void* userdata, nuint userdata_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_AddDrawCmd(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImDrawListStruct* ImDrawList_CloneOutput(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_ChannelsSplit(ImDrawListStruct* self, int count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_ChannelsMerge(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_ChannelsSetCurrent(ImDrawListStruct* self, int n);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PrimReserve(ImDrawListStruct* self, int idx_count, int vtx_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PrimUnreserve(ImDrawListStruct* self, int idx_count, int vtx_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PrimRect(ImDrawListStruct* self, Vector2 a, Vector2 b, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PrimRectUV(ImDrawListStruct* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PrimQuadUV(ImDrawListStruct* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PrimWriteVtx(ImDrawListStruct* self, Vector2 pos, Vector2 uv, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PrimWriteIdx(ImDrawListStruct* self, ushort idx);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PrimVtx(ImDrawListStruct* self, Vector2 pos, Vector2 uv, uint col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PushTextureID(ImDrawListStruct* self, ImTextureRef tex_ref);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList_PopTextureID(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__SetDrawListSharedData(ImDrawListStruct* self, nint data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__ResetForNewFrame(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__ClearFreeMemory(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__PopUnusedDrawCmd(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__TryMergeDrawCmds(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__OnChangedClipRect(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__OnChangedTexture(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__OnChangedVtxOffset(ImDrawListStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__SetTexture(ImDrawListStruct* self, ImTextureRef tex_ref);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int ImDrawList__CalcCircleAutoSegmentCount(ImDrawListStruct* self, float radius);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__PathArcToFastEx(ImDrawListStruct* self, Vector2 center, float radius, int a_min_sample, int a_max_sample, int a_step);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawList__PathArcToN(ImDrawListStruct* self, Vector2 center, float radius, float a_min, float a_max, int num_segments);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawData_Clear(ImDrawDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawData_AddDrawList(ImDrawDataStruct* self, ImDrawListStruct* draw_list);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawData_DeIndexAllBuffers(ImDrawDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImDrawData_ScaleClipRects(ImDrawDataStruct* self, Vector2 fb_scale);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImTextureData_Create(ImTextureDataStruct* self, ImTextureFormat format, int w, int h);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImTextureData_DestroyPixels(ImTextureDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void* ImTextureData_GetPixels(ImTextureDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void* ImTextureData_GetPixelsAt(ImTextureDataStruct* self, int x, int y);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int ImTextureData_GetSizeInBytes(ImTextureDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int ImTextureData_GetPitch(ImTextureDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImTextureRef ImTextureData_GetTexRef(ImTextureDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ulong ImTextureData_GetTexID(ImTextureDataStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImTextureData_SetTexID(ImTextureDataStruct* self, ulong tex_id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImTextureData_SetStatus(ImTextureDataStruct* self, ImTextureStatus status);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontGlyphRangesBuilder_Clear(ImFontGlyphRangesBuilderStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImFontGlyphRangesBuilder_GetBit(ImFontGlyphRangesBuilderStruct* self, nuint n);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontGlyphRangesBuilder_SetBit(ImFontGlyphRangesBuilderStruct* self, nuint n);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontGlyphRangesBuilder_AddChar(ImFontGlyphRangesBuilderStruct* self, uint c);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontGlyphRangesBuilder_AddText(ImFontGlyphRangesBuilderStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontGlyphRangesBuilder_AddRanges(ImFontGlyphRangesBuilderStruct* self, ref uint ranges);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontGlyphRangesBuilder_BuildRanges(ImFontGlyphRangesBuilderStruct* self, ref ImVector<uint> out_ranges);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontStruct* ImFontAtlas_AddFont(ImFontAtlasStruct* self, ImFontConfigStruct* font_cfg);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontStruct* ImFontAtlas_AddFontDefault(ImFontAtlasStruct* self, ImFontConfigStruct* font_cfg);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontStruct* ImFontAtlas_AddFontFromFileTTF(ImFontAtlasStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string filename, float size_pixels, ImFontConfigStruct* font_cfg, ref uint glyph_ranges);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontStruct* ImFontAtlas_AddFontFromMemoryTTF(ImFontAtlasStruct* self, void* font_data, int font_data_size, float size_pixels, ImFontConfigStruct* font_cfg, ref uint glyph_ranges);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontStruct* ImFontAtlas_AddFontFromMemoryCompressedTTF(ImFontAtlasStruct* self, void* compressed_font_data, int compressed_font_data_size, float size_pixels, ImFontConfigStruct* font_cfg, ref uint glyph_ranges);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontStruct* ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(ImFontAtlasStruct* self, [MarshalAs(UnmanagedType.LPUTF8Str)] string compressed_font_data_base85, float size_pixels, ImFontConfigStruct* font_cfg, ref uint glyph_ranges);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_RemoveFont(ImFontAtlasStruct* self, ImFontStruct* font);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_Clear(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_CompactCache(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_ClearInputData(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_ClearFonts(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_ClearTexData(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImFontAtlas_Build(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_GetTexDataAsAlpha8(ImFontAtlasStruct* self, byte** out_pixels, ref int out_width, ref int out_height, ref int out_bytes_per_pixel);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_GetTexDataAsRGBA32(ImFontAtlasStruct* self, byte** out_pixels, ref int out_width, ref int out_height, ref int out_bytes_per_pixel);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_SetTexID(ImFontAtlasStruct* self, ulong id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_SetTexIDImTextureRef(ImFontAtlasStruct* self, ImTextureRef id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImFontAtlas_IsBuilt(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint* ImFontAtlas_GetGlyphRangesDefault(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint* ImFontAtlas_GetGlyphRangesGreek(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint* ImFontAtlas_GetGlyphRangesKorean(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint* ImFontAtlas_GetGlyphRangesJapanese(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint* ImFontAtlas_GetGlyphRangesChineseFull(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint* ImFontAtlas_GetGlyphRangesCyrillic(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint* ImFontAtlas_GetGlyphRangesThai(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern uint* ImFontAtlas_GetGlyphRangesVietnamese(ImFontAtlasStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int ImFontAtlas_AddCustomRect(ImFontAtlasStruct* self, int width, int height, ImFontAtlasRectStruct* out_r);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_RemoveCustomRect(ImFontAtlasStruct* self, int id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImFontAtlas_GetCustomRect(ImFontAtlasStruct* self, int id, ImFontAtlasRectStruct* out_r);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int ImFontAtlas_AddCustomRectRegular(ImFontAtlasStruct* self, int w, int h);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontAtlasRectStruct* ImFontAtlas_GetCustomRectByIndex(ImFontAtlasStruct* self, int id);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontAtlas_CalcCustomRectUV(ImFontAtlasStruct* self, ImFontAtlasRectStruct* r, Vector2 out_uv_min, Vector2 out_uv_max);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int ImFontAtlas_AddCustomRectFontGlyph(ImFontAtlasStruct* self, ImFontStruct* font, uint codepoint, int w, int h, float advance_x, Vector2 offset);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern int ImFontAtlas_AddCustomRectFontGlyphForSize(ImFontAtlasStruct* self, ImFontStruct* font, float font_size, uint codepoint, int w, int h, float advance_x, Vector2 offset);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFontBaked_ClearOutputData(ImFontBakedStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontGlyphStruct* ImFontBaked_FindGlyph(ImFontBakedStruct* self, uint c);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontGlyphStruct* ImFontBaked_FindGlyphNoFallback(ImFontBakedStruct* self, uint c);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern float ImFontBaked_GetCharAdvance(ImFontBakedStruct* self, uint c);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImFontBaked_IsGlyphLoaded(ImFontBakedStruct* self, uint c);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImFont_IsGlyphInFont(ImFontStruct* self, uint c);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImFont_IsLoaded(ImFontStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern sbyte* ImFont_GetDebugName(ImFontStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontBakedStruct* ImFont_GetFontBaked(ImFontStruct* self, float font_size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontBakedStruct* ImFont_GetFontBakedEx(ImFontStruct* self, float font_size, float density);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern Vector2 ImFont_CalcTextSizeA(ImFontStruct* self, float size, float max_width, float wrap_width, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_begin);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern Vector2 ImFont_CalcTextSizeAEx(ImFontStruct* self, float size, float max_width, float wrap_width, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_begin, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end, sbyte** remaining);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern sbyte* ImFont_CalcWordWrapPosition(ImFontStruct* self, float size, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end, float wrap_width);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFont_RenderChar(ImFontStruct* self, ImDrawListStruct* draw_list, float size, Vector2 pos, uint col, uint c);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFont_RenderCharEx(ImFontStruct* self, ImDrawListStruct* draw_list, float size, Vector2 pos, uint col, uint c, Vector4 cpu_fine_clip);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFont_RenderText(ImFontStruct* self, ImDrawListStruct* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_begin, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end, float wrap_width, bool cpu_fine_clip);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern sbyte* ImFont_CalcWordWrapPositionA(ImFontStruct* self, float scale, [MarshalAs(UnmanagedType.LPUTF8Str)] string text, [MarshalAs(UnmanagedType.LPUTF8Str)] string text_end, float wrap_width);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFont_ClearOutputData(ImFontStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImFont_AddRemapChar(ImFontStruct* self, uint from_codepoint, uint to_codepoint);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImFont_IsGlyphRangeUnused(ImFontStruct* self, uint c_begin, uint c_last);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern Vector2 ImGuiViewport_GetCenter(ImGuiViewportStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern Vector2 ImGuiViewport_GetWorkCenter(ImGuiViewportStruct* self);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushFont", ExactSpelling = true)]
    public static extern void PushFont(ImFontStruct* font);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetWindowFontScale", ExactSpelling = true)]
    public static extern void SetWindowFontScale(float scale);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ImageImVec4", ExactSpelling = true)]
    public static extern void ImageImVec4(ImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushButtonRepeat", ExactSpelling = true)]
    public static extern void PushButtonRepeat(bool repeat);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopButtonRepeat", ExactSpelling = true)]
    public static extern void PopButtonRepeat();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PushTabStop", ExactSpelling = true)]
    public static extern void PushTabStop(bool tab_stop);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_PopTabStop", ExactSpelling = true)]
    public static extern void PopTabStop();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetContentRegionMax", ExactSpelling = true)]
    public static extern Vector2 GetContentRegionMax();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowContentRegionMin", ExactSpelling = true)]
    public static extern Vector2 GetWindowContentRegionMin();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_GetWindowContentRegionMax", ExactSpelling = true)]
    public static extern Vector2 GetWindowContentRegionMax();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginChildFrame", ExactSpelling = true)]
    public static extern bool BeginChildFrame(uint id, Vector2 size);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_BeginChildFrameEx", ExactSpelling = true)]
    public static extern bool BeginChildFrameEx(uint id, Vector2 size, ImGuiWindowFlags window_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_EndChildFrame", ExactSpelling = true)]
    public static extern void EndChildFrame();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ShowStackToolWindow", ExactSpelling = true)]
    public static extern void ShowStackToolWindow(ref bool p_open);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ComboObsolete", ExactSpelling = true)]
    public static extern bool ComboObsolete([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ComboObsoleteEx", ExactSpelling = true)]
    public static extern bool ComboObsoleteEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count, int popup_max_height_in_items);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ListBoxObsolete", ExactSpelling = true)]
    public static extern bool ListBoxObsolete([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_ListBoxObsoleteEx", ExactSpelling = true)]
    public static extern bool ListBoxObsoleteEx([MarshalAs(UnmanagedType.LPUTF8Str)] string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count, int height_in_items);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImGui_SetItemAllowOverlap", ExactSpelling = true)]
    public static extern void SetItemAllowOverlap();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern ImFontLoaderStruct* ImGuiFreeTypeGetFontLoader();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiFreeTypeSetAllocatorFunctions(delegate* unmanaged[Cdecl]<nuint, nint, nint> alloc_func, delegate* unmanaged[Cdecl]<nint, nint, void> free_func);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void ImGuiFreeTypeSetAllocatorFunctionsEx(delegate* unmanaged[Cdecl]<nuint, nint, nint> alloc_func, delegate* unmanaged[Cdecl]<nint, nint, void> free_func, void* user_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool ImGuiFreeTypeDebugEditFontLoaderFlags(ref uint p_font_loader_flags);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool cImGui_ImplDX9_Init(nint device);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX9_Shutdown();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX9_NewFrame();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX9_RenderDrawData(nint draw_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool cImGui_ImplDX9_CreateDeviceObjects();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX9_InvalidateDeviceObjects();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX9_UpdateTexture(nint tex);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool cImGui_ImplDX11_Init(nint device, nint device_context);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX11_Shutdown();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX11_NewFrame();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX11_RenderDrawData(nint draw_data);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool cImGui_ImplDX11_CreateDeviceObjects();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX11_InvalidateDeviceObjects();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX11_UpdateTexture(nint tex);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool cImGui_ImplDX12_Init(nint info);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX12_Shutdown();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX12_NewFrame();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX12_RenderDrawData(nint draw_data, nint graphics_command_list);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool cImGui_ImplDX12_InitID3D12DevicePtr(nint device, int num_frames_in_flight, nint rtv_format, nint srv_descriptor_heap, nint font_srv_cpu_desc_handle, nint font_srv_gpu_desc_handle);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool cImGui_ImplDX12_CreateDeviceObjects();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX12_InvalidateDeviceObjects();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplDX12_UpdateTexture(nint tex);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool cImGui_ImplWin32_Init(nint hwnd);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern bool cImGui_ImplWin32_InitForOpenGL(nint hwnd);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplWin32_Shutdown();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplWin32_NewFrame();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern long cImGui_ImplWin32_WndProcHandler(nint hWnd, uint msg, ulong wParam, long lParam);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplWin32_EnableDpiAwareness();
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern float cImGui_ImplWin32_GetDpiScaleForHwnd(nint hwnd);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern float cImGui_ImplWin32_GetDpiScaleForMonitor(nint monitor);
    [DllImport("ImGui/Binaries/ImGuiLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
    public static extern void cImGui_ImplWin32_EnableAlphaCompositing(nint hwnd);
}

public unsafe struct ImDrawListSharedDataStruct
{
}

public unsafe struct ImFontAtlasBuilderStruct
{
}

public unsafe struct ImFontLoaderStruct
{
}

public unsafe struct ImGuiContextStruct
{
}

public unsafe struct ImGuiTableSortSpecsStruct
{
    public ImGuiTableColumnSortSpecsStruct* Specs;
    public int SpecsCount;
    [MarshalAs(UnmanagedType.I1)]
    public bool SpecsDirty;
}

public unsafe struct ImGuiTableColumnSortSpecsStruct
{
    public uint ColumnUserID;
    public short ColumnIndex;
    public short SortOrder;
    public byte SortDirection;
}

public unsafe struct ImGuiStyleStruct
{
    public float FontSizeBase;
    public float FontScaleMain;
    public float FontScaleDpi;
    public float Alpha;
    public float DisabledAlpha;
    public Vector2 WindowPadding;
    public float WindowRounding;
    public float WindowBorderSize;
    public float WindowBorderHoverPadding;
    public Vector2 WindowMinSize;
    public Vector2 WindowTitleAlign;
    public int WindowMenuButtonPosition;
    public float ChildRounding;
    public float ChildBorderSize;
    public float PopupRounding;
    public float PopupBorderSize;
    public Vector2 FramePadding;
    public float FrameRounding;
    public float FrameBorderSize;
    public Vector2 ItemSpacing;
    public Vector2 ItemInnerSpacing;
    public Vector2 CellPadding;
    public Vector2 TouchExtraPadding;
    public float IndentSpacing;
    public float ColumnsMinSpacing;
    public float ScrollbarSize;
    public float ScrollbarRounding;
    public float GrabMinSize;
    public float GrabRounding;
    public float LogSliderDeadzone;
    public float ImageBorderSize;
    public float TabRounding;
    public float TabBorderSize;
    public float TabCloseButtonMinWidthSelected;
    public float TabCloseButtonMinWidthUnselected;
    public float TabBarBorderSize;
    public float TabBarOverlineSize;
    public float TableAngledHeadersAngle;
    public Vector2 TableAngledHeadersTextAlign;
    public ImGuiTreeNodeFlags TreeLinesFlags;
    public float TreeLinesSize;
    public float TreeLinesRounding;
    public int ColorButtonPosition;
    public Vector2 ButtonTextAlign;
    public Vector2 SelectableTextAlign;
    public float SeparatorTextBorderSize;
    public Vector2 SeparatorTextAlign;
    public Vector2 SeparatorTextPadding;
    public Vector2 DisplayWindowPadding;
    public Vector2 DisplaySafeAreaPadding;
    public float DockingSeparatorSize;
    public float MouseCursorScale;
    [MarshalAs(UnmanagedType.I1)]
    public bool AntiAliasedLines;
    [MarshalAs(UnmanagedType.I1)]
    public bool AntiAliasedLinesUseTex;
    [MarshalAs(UnmanagedType.I1)]
    public bool AntiAliasedFill;
    public float CurveTessellationTol;
    public float CircleTessellationMaxError;
    public _Colors_e__FixedBuffer Colors;
    public float HoverStationaryDelay;
    public float HoverDelayShort;
    public float HoverDelayNormal;
    public ImGuiHoveredFlags HoverFlagsForTooltipMouse;
    public ImGuiHoveredFlags HoverFlagsForTooltipNav;
    public float _MainScale;
    public float _NextFrameFontSizeBase;
    public partial struct _Colors_e__FixedBuffer
    {
        public Vector4 e0;
        public Vector4 e1;
        public Vector4 e2;
        public Vector4 e3;
        public Vector4 e4;
        public Vector4 e5;
        public Vector4 e6;
        public Vector4 e7;
        public Vector4 e8;
        public Vector4 e9;
        public Vector4 e10;
        public Vector4 e11;
        public Vector4 e12;
        public Vector4 e13;
        public Vector4 e14;
        public Vector4 e15;
        public Vector4 e16;
        public Vector4 e17;
        public Vector4 e18;
        public Vector4 e19;
        public Vector4 e20;
        public Vector4 e21;
        public Vector4 e22;
        public Vector4 e23;
        public Vector4 e24;
        public Vector4 e25;
        public Vector4 e26;
        public Vector4 e27;
        public Vector4 e28;
        public Vector4 e29;
        public Vector4 e30;
        public Vector4 e31;
        public Vector4 e32;
        public Vector4 e33;
        public Vector4 e34;
        public Vector4 e35;
        public Vector4 e36;
        public Vector4 e37;
        public Vector4 e38;
        public Vector4 e39;
        public Vector4 e40;
        public Vector4 e41;
        public Vector4 e42;
        public Vector4 e43;
        public Vector4 e44;
        public Vector4 e45;
        public Vector4 e46;
        public Vector4 e47;
        public Vector4 e48;
        public Vector4 e49;
        public Vector4 e50;
        public Vector4 e51;
        public Vector4 e52;
        public Vector4 e53;
        public Vector4 e54;
        public Vector4 e55;
        public Vector4 e56;
        public Vector4 e57;
        public Vector4 e58;
        public Vector4 e59;
    }
}

public unsafe struct ImGuiKeyDataStruct
{
    [MarshalAs(UnmanagedType.I1)]
    public bool Down;
    public float DownDuration;
    public float DownDurationPrev;
    public float AnalogValue;
}

public unsafe struct ImGuiIOStruct
{
    public ImGuiConfigFlags ConfigFlags;
    public ImGuiBackendFlags BackendFlags;
    public Vector2 DisplaySize;
    public Vector2 DisplayFramebufferScale;
    public float DeltaTime;
    public float IniSavingRate;
    public sbyte* IniFilename;
    public sbyte* LogFilename;
    public void* UserData;
    public ImFontAtlasStruct* Fonts;
    public ImFontStruct* FontDefault;
    [MarshalAs(UnmanagedType.I1)]
    public bool FontAllowUserScaling;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigNavSwapGamepadButtons;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigNavMoveSetMousePos;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigNavCaptureKeyboard;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigNavEscapeClearFocusItem;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigNavEscapeClearFocusWindow;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigNavCursorVisibleAuto;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigNavCursorVisibleAlways;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDockingNoSplit;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDockingWithShift;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDockingAlwaysTabBar;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDockingTransparentPayload;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigViewportsNoAutoMerge;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigViewportsNoTaskBarIcon;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigViewportsNoDecoration;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigViewportsNoDefaultParent;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDpiScaleFonts;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDpiScaleViewports;
    [MarshalAs(UnmanagedType.I1)]
    public bool MouseDrawCursor;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigMacOSXBehaviors;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigInputTrickleEventQueue;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigInputTextCursorBlink;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigInputTextEnterKeepActive;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDragClickToInputText;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigWindowsResizeFromEdges;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigWindowsMoveFromTitleBarOnly;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigWindowsCopyContentsWithCtrlC;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigScrollbarScrollByPage;
    public float ConfigMemoryCompactTimer;
    public float MouseDoubleClickTime;
    public float MouseDoubleClickMaxDist;
    public float MouseDragThreshold;
    public float KeyRepeatDelay;
    public float KeyRepeatRate;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigErrorRecovery;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigErrorRecoveryEnableAssert;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigErrorRecoveryEnableDebugLog;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigErrorRecoveryEnableTooltip;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDebugIsDebuggerPresent;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDebugHighlightIdConflicts;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDebugHighlightIdConflictsShowItemPicker;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDebugBeginReturnValueOnce;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDebugBeginReturnValueLoop;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDebugIgnoreFocusLoss;
    [MarshalAs(UnmanagedType.I1)]
    public bool ConfigDebugIniSettings;
    public sbyte* BackendPlatformName;
    public sbyte* BackendRendererName;
    public void* BackendPlatformUserData;
    public void* BackendRendererUserData;
    public void* BackendLanguageUserData;
    [MarshalAs(UnmanagedType.I1)]
    public bool WantCaptureMouse;
    [MarshalAs(UnmanagedType.I1)]
    public bool WantCaptureKeyboard;
    [MarshalAs(UnmanagedType.I1)]
    public bool WantTextInput;
    [MarshalAs(UnmanagedType.I1)]
    public bool WantSetMousePos;
    [MarshalAs(UnmanagedType.I1)]
    public bool WantSaveIniSettings;
    [MarshalAs(UnmanagedType.I1)]
    public bool NavActive;
    [MarshalAs(UnmanagedType.I1)]
    public bool NavVisible;
    public float Framerate;
    public int MetricsRenderVertices;
    public int MetricsRenderIndices;
    public int MetricsRenderWindows;
    public int MetricsActiveWindows;
    public Vector2 MouseDelta;
    public ImGuiContextStruct* Ctx;
    public Vector2 MousePos;
    public fixed byte MouseDown[5];
    public float MouseWheel;
    public float MouseWheelH;
    public int MouseSource;
    public uint MouseHoveredViewport;
    [MarshalAs(UnmanagedType.I1)]
    public bool KeyCtrl;
    [MarshalAs(UnmanagedType.I1)]
    public bool KeyShift;
    [MarshalAs(UnmanagedType.I1)]
    public bool KeyAlt;
    [MarshalAs(UnmanagedType.I1)]
    public bool KeySuper;
    public int KeyMods;
    public _KeysData_e__FixedBuffer KeysData;
    [MarshalAs(UnmanagedType.I1)]
    public bool WantCaptureMouseUnlessPopupClose;
    public Vector2 MousePosPrev;
    public _MouseClickedPos_e__FixedBuffer MouseClickedPos;
    public fixed double MouseClickedTime[5];
    public fixed byte MouseClicked[5];
    public fixed byte MouseDoubleClicked[5];
    public fixed ushort MouseClickedCount[5];
    public fixed ushort MouseClickedLastCount[5];
    public fixed byte MouseReleased[5];
    public fixed double MouseReleasedTime[5];
    public fixed byte MouseDownOwned[5];
    public fixed byte MouseDownOwnedUnlessPopupClose[5];
    [MarshalAs(UnmanagedType.I1)]
    public bool MouseWheelRequestAxisSwap;
    [MarshalAs(UnmanagedType.I1)]
    public bool MouseCtrlLeftAsRightClick;
    public fixed float MouseDownDuration[5];
    public fixed float MouseDownDurationPrev[5];
    public _MouseDragMaxDistanceAbs_e__FixedBuffer MouseDragMaxDistanceAbs;
    public fixed float MouseDragMaxDistanceSqr[5];
    public float PenPressure;
    [MarshalAs(UnmanagedType.I1)]
    public bool AppFocusLost;
    [MarshalAs(UnmanagedType.I1)]
    public bool AppAcceptingEvents;
    public ushort InputQueueSurrogate;
    public ImVector<uint> InputQueueCharacters;
    public float FontGlobalScale;
    public delegate* unmanaged[Cdecl]<nint, nint> GetClipboardTextFn;
    public delegate* unmanaged[Cdecl]<nint, nint, void> SetClipboardTextFn;
    public void* ClipboardUserData;
    public partial struct _KeysData_e__FixedBuffer
    {
        public ImGuiKeyDataStruct e0;
        public ImGuiKeyDataStruct e1;
        public ImGuiKeyDataStruct e2;
        public ImGuiKeyDataStruct e3;
        public ImGuiKeyDataStruct e4;
        public ImGuiKeyDataStruct e5;
        public ImGuiKeyDataStruct e6;
        public ImGuiKeyDataStruct e7;
        public ImGuiKeyDataStruct e8;
        public ImGuiKeyDataStruct e9;
        public ImGuiKeyDataStruct e10;
        public ImGuiKeyDataStruct e11;
        public ImGuiKeyDataStruct e12;
        public ImGuiKeyDataStruct e13;
        public ImGuiKeyDataStruct e14;
        public ImGuiKeyDataStruct e15;
        public ImGuiKeyDataStruct e16;
        public ImGuiKeyDataStruct e17;
        public ImGuiKeyDataStruct e18;
        public ImGuiKeyDataStruct e19;
        public ImGuiKeyDataStruct e20;
        public ImGuiKeyDataStruct e21;
        public ImGuiKeyDataStruct e22;
        public ImGuiKeyDataStruct e23;
        public ImGuiKeyDataStruct e24;
        public ImGuiKeyDataStruct e25;
        public ImGuiKeyDataStruct e26;
        public ImGuiKeyDataStruct e27;
        public ImGuiKeyDataStruct e28;
        public ImGuiKeyDataStruct e29;
        public ImGuiKeyDataStruct e30;
        public ImGuiKeyDataStruct e31;
        public ImGuiKeyDataStruct e32;
        public ImGuiKeyDataStruct e33;
        public ImGuiKeyDataStruct e34;
        public ImGuiKeyDataStruct e35;
        public ImGuiKeyDataStruct e36;
        public ImGuiKeyDataStruct e37;
        public ImGuiKeyDataStruct e38;
        public ImGuiKeyDataStruct e39;
        public ImGuiKeyDataStruct e40;
        public ImGuiKeyDataStruct e41;
        public ImGuiKeyDataStruct e42;
        public ImGuiKeyDataStruct e43;
        public ImGuiKeyDataStruct e44;
        public ImGuiKeyDataStruct e45;
        public ImGuiKeyDataStruct e46;
        public ImGuiKeyDataStruct e47;
        public ImGuiKeyDataStruct e48;
        public ImGuiKeyDataStruct e49;
        public ImGuiKeyDataStruct e50;
        public ImGuiKeyDataStruct e51;
        public ImGuiKeyDataStruct e52;
        public ImGuiKeyDataStruct e53;
        public ImGuiKeyDataStruct e54;
        public ImGuiKeyDataStruct e55;
        public ImGuiKeyDataStruct e56;
        public ImGuiKeyDataStruct e57;
        public ImGuiKeyDataStruct e58;
        public ImGuiKeyDataStruct e59;
        public ImGuiKeyDataStruct e60;
        public ImGuiKeyDataStruct e61;
        public ImGuiKeyDataStruct e62;
        public ImGuiKeyDataStruct e63;
        public ImGuiKeyDataStruct e64;
        public ImGuiKeyDataStruct e65;
        public ImGuiKeyDataStruct e66;
        public ImGuiKeyDataStruct e67;
        public ImGuiKeyDataStruct e68;
        public ImGuiKeyDataStruct e69;
        public ImGuiKeyDataStruct e70;
        public ImGuiKeyDataStruct e71;
        public ImGuiKeyDataStruct e72;
        public ImGuiKeyDataStruct e73;
        public ImGuiKeyDataStruct e74;
        public ImGuiKeyDataStruct e75;
        public ImGuiKeyDataStruct e76;
        public ImGuiKeyDataStruct e77;
        public ImGuiKeyDataStruct e78;
        public ImGuiKeyDataStruct e79;
        public ImGuiKeyDataStruct e80;
        public ImGuiKeyDataStruct e81;
        public ImGuiKeyDataStruct e82;
        public ImGuiKeyDataStruct e83;
        public ImGuiKeyDataStruct e84;
        public ImGuiKeyDataStruct e85;
        public ImGuiKeyDataStruct e86;
        public ImGuiKeyDataStruct e87;
        public ImGuiKeyDataStruct e88;
        public ImGuiKeyDataStruct e89;
        public ImGuiKeyDataStruct e90;
        public ImGuiKeyDataStruct e91;
        public ImGuiKeyDataStruct e92;
        public ImGuiKeyDataStruct e93;
        public ImGuiKeyDataStruct e94;
        public ImGuiKeyDataStruct e95;
        public ImGuiKeyDataStruct e96;
        public ImGuiKeyDataStruct e97;
        public ImGuiKeyDataStruct e98;
        public ImGuiKeyDataStruct e99;
        public ImGuiKeyDataStruct e100;
        public ImGuiKeyDataStruct e101;
        public ImGuiKeyDataStruct e102;
        public ImGuiKeyDataStruct e103;
        public ImGuiKeyDataStruct e104;
        public ImGuiKeyDataStruct e105;
        public ImGuiKeyDataStruct e106;
        public ImGuiKeyDataStruct e107;
        public ImGuiKeyDataStruct e108;
        public ImGuiKeyDataStruct e109;
        public ImGuiKeyDataStruct e110;
        public ImGuiKeyDataStruct e111;
        public ImGuiKeyDataStruct e112;
        public ImGuiKeyDataStruct e113;
        public ImGuiKeyDataStruct e114;
        public ImGuiKeyDataStruct e115;
        public ImGuiKeyDataStruct e116;
        public ImGuiKeyDataStruct e117;
        public ImGuiKeyDataStruct e118;
        public ImGuiKeyDataStruct e119;
        public ImGuiKeyDataStruct e120;
        public ImGuiKeyDataStruct e121;
        public ImGuiKeyDataStruct e122;
        public ImGuiKeyDataStruct e123;
        public ImGuiKeyDataStruct e124;
        public ImGuiKeyDataStruct e125;
        public ImGuiKeyDataStruct e126;
        public ImGuiKeyDataStruct e127;
        public ImGuiKeyDataStruct e128;
        public ImGuiKeyDataStruct e129;
        public ImGuiKeyDataStruct e130;
        public ImGuiKeyDataStruct e131;
        public ImGuiKeyDataStruct e132;
        public ImGuiKeyDataStruct e133;
        public ImGuiKeyDataStruct e134;
        public ImGuiKeyDataStruct e135;
        public ImGuiKeyDataStruct e136;
        public ImGuiKeyDataStruct e137;
        public ImGuiKeyDataStruct e138;
        public ImGuiKeyDataStruct e139;
        public ImGuiKeyDataStruct e140;
        public ImGuiKeyDataStruct e141;
        public ImGuiKeyDataStruct e142;
        public ImGuiKeyDataStruct e143;
        public ImGuiKeyDataStruct e144;
        public ImGuiKeyDataStruct e145;
        public ImGuiKeyDataStruct e146;
        public ImGuiKeyDataStruct e147;
        public ImGuiKeyDataStruct e148;
        public ImGuiKeyDataStruct e149;
        public ImGuiKeyDataStruct e150;
        public ImGuiKeyDataStruct e151;
        public ImGuiKeyDataStruct e152;
        public ImGuiKeyDataStruct e153;
        public ImGuiKeyDataStruct e154;
    }

    public partial struct _MouseClickedPos_e__FixedBuffer
    {
        public Vector2 e0;
        public Vector2 e1;
        public Vector2 e2;
        public Vector2 e3;
        public Vector2 e4;
    }

    public partial struct _MouseDragMaxDistanceAbs_e__FixedBuffer
    {
        public Vector2 e0;
        public Vector2 e1;
        public Vector2 e2;
        public Vector2 e3;
        public Vector2 e4;
    }
}

public unsafe struct ImGuiInputTextCallbackDataStruct
{
    public ImGuiContextStruct* Ctx;
    public ImGuiInputTextFlags EventFlag;
    public ImGuiInputTextFlags Flags;
    public void* UserData;
    public uint EventChar;
    public int EventKey;
    public sbyte* Buf;
    public int BufTextLen;
    public int BufSize;
    [MarshalAs(UnmanagedType.I1)]
    public bool BufDirty;
    public int CursorPos;
    public int SelectionStart;
    public int SelectionEnd;
}

public unsafe struct ImGuiSizeCallbackDataStruct
{
    public void* UserData;
    public Vector2 Pos;
    public Vector2 CurrentSize;
    public Vector2 DesiredSize;
}

public unsafe struct ImGuiWindowClassStruct
{
    public uint ClassId;
    public uint ParentViewportId;
    public uint FocusRouteParentWindowId;
    public ImGuiViewportFlags ViewportFlagsOverrideSet;
    public ImGuiViewportFlags ViewportFlagsOverrideClear;
    public ImGuiTabItemFlags TabItemFlagsOverrideSet;
    public ImGuiDockNodeFlags DockNodeFlagsOverrideSet;
    [MarshalAs(UnmanagedType.I1)]
    public bool DockingAlwaysTabBar;
    [MarshalAs(UnmanagedType.I1)]
    public bool DockingAllowUnclassed;
}

public unsafe struct ImGuiPayloadStruct
{
    public void* Data;
    public int DataSize;
    public uint SourceId;
    public uint SourceParentId;
    public int DataFrameCount;
    public fixed sbyte DataType[33];
    [MarshalAs(UnmanagedType.I1)]
    public bool Preview;
    [MarshalAs(UnmanagedType.I1)]
    public bool Delivery;
}

public unsafe struct ImGuiTextFilter_ImGuiTextRangeStruct
{
    public sbyte* b;
    public sbyte* e;
}

public unsafe struct ImGuiTextFilterStruct
{
    public fixed sbyte InputBuf[256];
    public ImVector<ImGuiTextFilter_ImGuiTextRangeStruct> Filters;
    public int CountGrep;
}

public unsafe struct ImGuiTextBufferStruct
{
    public ImVector<byte> Buf;
}

public unsafe struct ImGuiStorageStruct
{
    public ImVector<ImGuiStoragePairStruct> Data;
}

public unsafe struct ImGuiListClipperStruct
{
    public ImGuiContextStruct* Ctx;
    public int DisplayStart;
    public int DisplayEnd;
    public int ItemsCount;
    public float ItemsHeight;
    public double StartPosY;
    public double StartSeekOffsetY;
    public void* TempData;
}

public unsafe struct ImGuiMultiSelectIOStruct
{
    public ImVector<ImGuiSelectionRequestStruct> Requests;
    public long RangeSrcItem;
    public long NavIdItem;
    [MarshalAs(UnmanagedType.I1)]
    public bool NavIdSelected;
    [MarshalAs(UnmanagedType.I1)]
    public bool RangeSrcReset;
    public int ItemsCount;
}

public unsafe struct ImGuiSelectionRequestStruct
{
    public ImGuiSelectionRequestType Type;
    [MarshalAs(UnmanagedType.I1)]
    public bool Selected;
    public sbyte RangeDirection;
    public long RangeFirstItem;
    public long RangeLastItem;
}

public unsafe struct ImGuiSelectionBasicStorageStruct
{
    public int Size;
    [MarshalAs(UnmanagedType.I1)]
    public bool PreserveOrder;
    public void* UserData;
    public delegate* unmanaged[Cdecl]<nint, int, uint> AdapterIndexToStorageId;
    public int _SelectionOrder;
    public ImGuiStorageStruct _Storage;
}

public unsafe struct ImGuiSelectionExternalStorageStruct
{
    public void* UserData;
    public delegate* unmanaged[Cdecl]<nint, int, byte, void> AdapterSetItemSelected;
}

public unsafe struct ImDrawCmdStruct
{
    public Vector4 ClipRect;
    public ImTextureRef TexRef;
    public uint VtxOffset;
    public uint IdxOffset;
    public uint ElemCount;
    public delegate* unmanaged[Cdecl]<nint, nint, void> UserCallback;
    public void* UserCallbackData;
    public int UserCallbackDataSize;
    public int UserCallbackDataOffset;
}

public unsafe struct ImDrawVertStruct
{
    public Vector2 pos;
    public Vector2 uv;
    public uint col;
}

public unsafe struct ImDrawCmdHeaderStruct
{
    public Vector4 ClipRect;
    public ImTextureRef TexRef;
    public uint VtxOffset;
}

public unsafe struct ImDrawChannelStruct
{
    public ImVector<ImDrawCmdStruct> _CmdBuffer;
    public ImVector<ushort> _IdxBuffer;
}

public unsafe struct ImDrawListSplitterStruct
{
    public int _Current;
    public int _Count;
    public ImVector<ImDrawChannelStruct> _Channels;
}

public unsafe struct ImDrawListStruct
{
    public ImVector<ImDrawCmdStruct> CmdBuffer;
    public ImVector<ushort> IdxBuffer;
    public ImVector<ImDrawVertStruct> VtxBuffer;
    public ImDrawListFlags Flags;
    public uint _VtxCurrentIdx;
    public nint _Data;
    public ImDrawVertStruct* _VtxWritePtr;
    public ushort* _IdxWritePtr;
    public ImVector<Vector2> _Path;
    public ImDrawCmdHeaderStruct _CmdHeader;
    public ImDrawListSplitterStruct _Splitter;
    public ImVector<Vector4> _ClipRectStack;
    public ImVector<ImTextureRef> _TextureStack;
    public ImVector<byte> _CallbacksDataBuf;
    public float _FringeScale;
    public sbyte* _OwnerName;
}

public unsafe struct ImDrawDataStruct
{
    [MarshalAs(UnmanagedType.I1)]
    public bool Valid;
    public int CmdListsCount;
    public int TotalIdxCount;
    public int TotalVtxCount;
    public ImStructPtrVector<ImDrawListStruct> CmdLists;
    public Vector2 DisplayPos;
    public Vector2 DisplaySize;
    public Vector2 FramebufferScale;
    public ImGuiViewportStruct* OwnerViewport;
    public ImStructPtrVectorPtr<ImTextureDataStruct> Textures;
}

public unsafe struct ImTextureRectStruct
{
    public ushort x;
    public ushort y;
    public ushort w;
    public ushort h;
}

public unsafe struct ImTextureDataStruct
{
    public int UniqueID;
    public ImTextureStatus Status;
    public void* BackendUserData;
    public ulong TexID;
    public ImTextureFormat Format;
    public int Width;
    public int Height;
    public int BytesPerPixel;
    public byte* Pixels;
    public ImTextureRectStruct UsedRect;
    public ImTextureRectStruct UpdateRect;
    public ImVector<ImTextureRectStruct> Updates;
    public int UnusedFrames;
    public ushort RefCount;
    [MarshalAs(UnmanagedType.I1)]
    public bool UseColors;
    [MarshalAs(UnmanagedType.I1)]
    public bool WantDestroyNextFrame;
}

public unsafe struct ImFontConfigStruct
{
    public fixed sbyte Name[40];
    public void* FontData;
    public int FontDataSize;
    [MarshalAs(UnmanagedType.I1)]
    public bool FontDataOwnedByAtlas;
    [MarshalAs(UnmanagedType.I1)]
    public bool MergeMode;
    [MarshalAs(UnmanagedType.I1)]
    public bool PixelSnapH;
    [MarshalAs(UnmanagedType.I1)]
    public bool PixelSnapV;
    public sbyte FontNo;
    public sbyte OversampleH;
    public sbyte OversampleV;
    public float SizePixels;
    public uint* GlyphRanges;
    public uint* GlyphExcludeRanges;
    public Vector2 GlyphOffset;
    public float GlyphMinAdvanceX;
    public float GlyphMaxAdvanceX;
    public float GlyphExtraAdvanceX;
    public uint FontLoaderFlags;
    public float RasterizerMultiply;
    public float RasterizerDensity;
    public uint EllipsisChar;
    public int Flags;
    public ImFontStruct* DstFont;
    public ImFontLoaderStruct* FontLoader;
    public void* FontLoaderData;
}

public unsafe struct ImFontGlyphStruct
{
    public uint _bitfield;
    public uint Colored
    {
        readonly get
        {
            return _bitfield & 0x1u;
        }

        set
        {
            _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
        }
    }

    public uint Visible
    {
        readonly get
        {
            return (_bitfield >> 1) & 0x1u;
        }

        set
        {
            _bitfield = (_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1);
        }
    }

    public uint SourceIdx
    {
        readonly get
        {
            return (_bitfield >> 2) & 0xFu;
        }

        set
        {
            _bitfield = (_bitfield & ~(0xFu << 2)) | ((value & 0xFu) << 2);
        }
    }

    public uint Codepoint
    {
        readonly get
        {
            return (_bitfield >> 6) & 0x3FFFFFFu;
        }

        set
        {
            _bitfield = (_bitfield & ~(0x3FFFFFFu << 6)) | ((value & 0x3FFFFFFu) << 6);
        }
    }

    public float AdvanceX;
    public float X0;
    public float Y0;
    public float X1;
    public float Y1;
    public float U0;
    public float V0;
    public float U1;
    public float V1;
    public int PackId;
}

public unsafe struct ImFontGlyphRangesBuilderStruct
{
    public ImVector<uint> UsedChars;
}

public unsafe struct ImFontAtlasRectStruct
{
    public ushort x;
    public ushort y;
    public ushort w;
    public ushort h;
    public Vector2 uv0;
    public Vector2 uv1;
}

public unsafe struct ImFontAtlasStruct
{
    public ImFontAtlasFlags Flags;
    public ImTextureFormat TexDesiredFormat;
    public int TexGlyphPadding;
    public int TexMinWidth;
    public int TexMinHeight;
    public int TexMaxWidth;
    public int TexMaxHeight;
    public void* UserData;
    public _Anonymous_e__Union Anonymous;
    public ImTextureDataStruct* TexData;
    public ImStructPtrVector<ImTextureDataStruct> TexList;
    [MarshalAs(UnmanagedType.I1)]
    public bool Locked;
    [MarshalAs(UnmanagedType.I1)]
    public bool RendererHasTextures;
    [MarshalAs(UnmanagedType.I1)]
    public bool TexIsBuilt;
    [MarshalAs(UnmanagedType.I1)]
    public bool TexPixelsUseColors;
    public Vector2 TexUvScale;
    public Vector2 TexUvWhitePixel;
    public ImStructPtrVector<ImFontStruct> Fonts;
    public ImVector<ImFontConfigStruct> Sources;
    public _TexUvLines_e__FixedBuffer TexUvLines;
    public int TexNextUniqueID;
    public int FontNextUniqueID;
    public ImStructPtrVector<ImDrawListSharedDataStruct> DrawListSharedDatas;
    public ImFontAtlasBuilderStruct* Builder;
    public ImFontLoaderStruct* FontLoader;
    public sbyte* FontLoaderName;
    public void* FontLoaderData;
    public uint FontLoaderFlags;
    public int RefCount;
    public ImGuiContextStruct* OwnerContext;
    public ImFontAtlasRectStruct TempRect;
    public ref ImTextureRef TexRef
    {
        get
        {
            return ref MemoryMarshal.GetReference(MemoryMarshal.CreateSpan(ref Anonymous.TexRef, 1));
        }
    }

    public ref ImTextureRef TexID
    {
        get
        {
            return ref MemoryMarshal.GetReference(MemoryMarshal.CreateSpan(ref Anonymous.TexID, 1));
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public partial struct _Anonymous_e__Union
    {
        [FieldOffset(0)]
        public ImTextureRef TexRef;
        [FieldOffset(0)]
        public ImTextureRef TexID;
    }

    public partial struct _TexUvLines_e__FixedBuffer
    {
        public Vector4 e0;
        public Vector4 e1;
        public Vector4 e2;
        public Vector4 e3;
        public Vector4 e4;
        public Vector4 e5;
        public Vector4 e6;
        public Vector4 e7;
        public Vector4 e8;
        public Vector4 e9;
        public Vector4 e10;
        public Vector4 e11;
        public Vector4 e12;
        public Vector4 e13;
        public Vector4 e14;
        public Vector4 e15;
        public Vector4 e16;
        public Vector4 e17;
        public Vector4 e18;
        public Vector4 e19;
        public Vector4 e20;
        public Vector4 e21;
        public Vector4 e22;
        public Vector4 e23;
        public Vector4 e24;
        public Vector4 e25;
        public Vector4 e26;
        public Vector4 e27;
        public Vector4 e28;
        public Vector4 e29;
        public Vector4 e30;
        public Vector4 e31;
        public Vector4 e32;
    }
}

public unsafe struct ImFontBakedStruct
{
    public ImVector<float> IndexAdvanceX;
    public float FallbackAdvanceX;
    public float Size;
    public float RasterizerDensity;
    public ImVector<ushort> IndexLookup;
    public ImVector<ImFontGlyphStruct> Glyphs;
    public int FallbackGlyphIndex;
    public float Ascent;
    public float Descent;
    public uint _bitfield;
    public uint MetricsTotalSurface
    {
        readonly get
        {
            return _bitfield & 0x3FFFFFFu;
        }

        set
        {
            _bitfield = (_bitfield & ~0x3FFFFFFu) | (value & 0x3FFFFFFu);
        }
    }

    public uint WantDestroy
    {
        readonly get
        {
            return (_bitfield >> 26) & 0x1u;
        }

        set
        {
            _bitfield = (_bitfield & ~(0x1u << 26)) | ((value & 0x1u) << 26);
        }
    }

    public uint LockLoadingFallback
    {
        readonly get
        {
            return (_bitfield >> 27) & 0x1u;
        }

        set
        {
            _bitfield = (_bitfield & ~(0x1u << 27)) | ((value & 0x1u) << 27);
        }
    }

    public int LastUsedFrame;
    public uint BakedId;
    public ImFontStruct* ContainerFont;
    public void* FontLoaderDatas;
}

public unsafe struct ImFontStruct
{
    public ImFontBakedStruct* LastBaked;
    public ImFontAtlasStruct* ContainerAtlas;
    public int Flags;
    public float CurrentRasterizerDensity;
    public uint FontId;
    public float LegacySize;
    public ImStructPtrVector<ImFontConfigStruct> Sources;
    public uint EllipsisChar;
    public uint FallbackChar;
    public fixed byte Used8kPagesMap[17];
    [MarshalAs(UnmanagedType.I1)]
    public bool EllipsisAutoBake;
    public ImGuiStorageStruct RemapPairs;
    public float Scale;
}

public unsafe struct ImGuiViewportStruct
{
    public uint ID;
    public ImGuiViewportFlags Flags;
    public Vector2 Pos;
    public Vector2 Size;
    public Vector2 FramebufferScale;
    public Vector2 WorkPos;
    public Vector2 WorkSize;
    public float DpiScale;
    public uint ParentViewportId;
    public ImDrawDataStruct* DrawData;
    public void* RendererUserData;
    public void* PlatformUserData;
    public void* PlatformHandle;
    public void* PlatformHandleRaw;
    [MarshalAs(UnmanagedType.I1)]
    public bool PlatformWindowCreated;
    [MarshalAs(UnmanagedType.I1)]
    public bool PlatformRequestMove;
    [MarshalAs(UnmanagedType.I1)]
    public bool PlatformRequestResize;
    [MarshalAs(UnmanagedType.I1)]
    public bool PlatformRequestClose;
}

public unsafe struct ImGuiPlatformIOStruct
{
    public delegate* unmanaged[Cdecl]<nint, nint> Platform_GetClipboardTextFn;
    public delegate* unmanaged[Cdecl]<nint, nint, void> Platform_SetClipboardTextFn;
    public void* Platform_ClipboardUserData;
    public delegate* unmanaged[Cdecl]<nint, nint, byte> Platform_OpenInShellFn;
    public void* Platform_OpenInShellUserData;
    public delegate* unmanaged[Cdecl]<nint, nint, nint, void> Platform_SetImeDataFn;
    public void* Platform_ImeUserData;
    public uint Platform_LocaleDecimalPoint;
    public int Renderer_TextureMaxWidth;
    public int Renderer_TextureMaxHeight;
    public void* Renderer_RenderState;
    public delegate* unmanaged[Cdecl]<nint, void> Platform_CreateWindow;
    public delegate* unmanaged[Cdecl]<nint, void> Platform_DestroyWindow;
    public delegate* unmanaged[Cdecl]<nint, void> Platform_ShowWindow;
    public delegate* unmanaged[Cdecl]<nint, Vector2, void> Platform_SetWindowPos;
    public delegate* unmanaged[Cdecl]<nint, Vector2> Platform_GetWindowPos;
    public delegate* unmanaged[Cdecl]<nint, Vector2, void> Platform_SetWindowSize;
    public delegate* unmanaged[Cdecl]<nint, Vector2> Platform_GetWindowSize;
    public delegate* unmanaged[Cdecl]<nint, Vector2> Platform_GetWindowFramebufferScale;
    public delegate* unmanaged[Cdecl]<nint, void> Platform_SetWindowFocus;
    public delegate* unmanaged[Cdecl]<nint, byte> Platform_GetWindowFocus;
    public delegate* unmanaged[Cdecl]<nint, byte> Platform_GetWindowMinimized;
    public delegate* unmanaged[Cdecl]<nint, nint, void> Platform_SetWindowTitle;
    public delegate* unmanaged[Cdecl]<nint, float, void> Platform_SetWindowAlpha;
    public delegate* unmanaged[Cdecl]<nint, void> Platform_UpdateWindow;
    public delegate* unmanaged[Cdecl]<nint, nint, void> Platform_RenderWindow;
    public delegate* unmanaged[Cdecl]<nint, nint, void> Platform_SwapBuffers;
    public delegate* unmanaged[Cdecl]<nint, float> Platform_GetWindowDpiScale;
    public delegate* unmanaged[Cdecl]<nint, void> Platform_OnChangedViewport;
    public delegate* unmanaged[Cdecl]<nint, Vector4> Platform_GetWindowWorkAreaInsets;
    public delegate* unmanaged[Cdecl]<nint, ulong, nint, nint, int> Platform_CreateVkSurface;
    public delegate* unmanaged[Cdecl]<nint, void> Renderer_CreateWindow;
    public delegate* unmanaged[Cdecl]<nint, void> Renderer_DestroyWindow;
    public delegate* unmanaged[Cdecl]<nint, Vector2, void> Renderer_SetWindowSize;
    public delegate* unmanaged[Cdecl]<nint, nint, void> Renderer_RenderWindow;
    public delegate* unmanaged[Cdecl]<nint, nint, void> Renderer_SwapBuffers;
    public ImVector<ImGuiPlatformMonitorStruct> Monitors;
    public ImStructPtrVector<ImTextureDataStruct> Textures;
    public ImStructPtrVector<ImGuiViewportStruct> Viewports;
}

public unsafe struct ImGuiPlatformMonitorStruct
{
    public Vector2 MainPos;
    public Vector2 MainSize;
    public Vector2 WorkPos;
    public Vector2 WorkSize;
    public float DpiScale;
    public void* PlatformHandle;
}

public unsafe struct ImGuiPlatformImeDataStruct
{
    [MarshalAs(UnmanagedType.I1)]
    public bool WantVisible;
    [MarshalAs(UnmanagedType.I1)]
    public bool WantTextInput;
    public Vector2 InputPos;
    public float InputLineHeight;
    public uint ViewportId;
}

public unsafe struct ImGuiFreeTypeImDrawDataStruct
{
}