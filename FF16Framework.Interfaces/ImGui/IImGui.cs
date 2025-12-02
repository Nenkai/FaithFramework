// This file was generated with ImGuiInterfaceGenerator
using System;
using System.Runtime.InteropServices;
using System.Numerics;

namespace FF16Framework.Interfaces.ImGui;
///<summary>
///Interface to the ImGui library.<br/>
///</summary>
unsafe public interface IImGui
{
    ///<summary>
    /// == (_TexData ? _TexData-&gt;TexID : _TexID)  Implemented below in the file.<br/>
    ///</summary>
    ulong ImTextureRef_GetTexID(ImTextureRef self);
    ///<summary>
    /// Context creation and access<br/>
    /// - Each context create its own ImFontAtlas by default. You may instance one yourself and pass it to CreateContext() to share a font atlas between contexts.<br/>
    /// - DLL users: heaps and globals are not shared across DLL boundaries! You will need to call SetCurrentContext() + SetAllocatorFunctions()<br/>
    ///   for each static/DLL boundary you are calling from. Read "Context and Memory Allocators" section of imgui.cpp for details.<br/>
    ///</summary>
    IImGuiContext CreateContext(IImFontAtlas shared_font_atlas);
    ///<summary>
    /// NULL = destroy current context<br/>
    ///</summary>
    void DestroyContext(IImGuiContext ctx);
    IImGuiContext GetCurrentContext();
    void SetCurrentContext(IImGuiContext ctx);
    ///<summary>
    /// access the ImGuiIO structure (mouse/keyboard/gamepad inputs, time, various configuration options/flags)<br/>
    ///<br/>
    /// Main<br/>
    ///</summary>
    IImGuiIO GetIO();
    ///<summary>
    /// access the ImGuiPlatformIO structure (mostly hooks/functions to connect to platform/renderer and OS Clipboard, IME etc.)<br/>
    ///</summary>
    IImGuiPlatformIO GetPlatformIO();
    ///<summary>
    /// access the Style structure (colors, sizes). Always use PushStyleColor(), PushStyleVar() to modify style mid-frame!<br/>
    ///</summary>
    IImGuiStyle GetStyle();
    ///<summary>
    /// start a new Dear ImGui frame, you can submit any command from this point until Render()/EndFrame().<br/>
    ///</summary>
    void NewFrame();
    ///<summary>
    /// ends the Dear ImGui frame. automatically called by Render(). If you don't need to render data (skipping rendering) you may call EndFrame() without Render()... but you'll have wasted CPU already! If you don't need to render, better to not create any windows and not call NewFrame() at all!<br/>
    ///</summary>
    void EndFrame();
    ///<summary>
    /// ends the Dear ImGui frame, finalize the draw data. You can then get call GetDrawData().<br/>
    ///</summary>
    void Render();
    ///<summary>
    /// valid after Render() and until the next call to NewFrame(). Call ImGui_ImplXXXX_RenderDrawData() function in your Renderer Backend to render.<br/>
    ///</summary>
    IImDrawData GetDrawData();
    ///<summary>
    /// create Demo window. demonstrate most ImGui features. call this to learn about the library! try to make it always available in your application!<br/>
    ///<br/>
    /// Demo, Debug, Information<br/>
    ///</summary>
    void ShowDemoWindow(ref bool p_open);
    ///<summary>
    /// create Metrics/Debugger window. display Dear ImGui internals: windows, draw commands, various internal state, etc.<br/>
    ///</summary>
    void ShowMetricsWindow(ref bool p_open);
    ///<summary>
    /// create Debug Log window. display a simplified log of important dear imgui events.<br/>
    ///</summary>
    void ShowDebugLogWindow(ref bool p_open);
    ///<summary>
    /// Implied p_open = NULL<br/>
    ///</summary>
    void ShowIDStackToolWindow();
    ///<summary>
    /// create Stack Tool window. hover items with mouse to query information about the source of their unique ID.<br/>
    ///</summary>
    void ShowIDStackToolWindowEx(ref bool p_open);
    ///<summary>
    /// create About window. display Dear ImGui version, credits and build/system information.<br/>
    ///</summary>
    void ShowAboutWindow(ref bool p_open);
    ///<summary>
    /// add style editor block (not a window). you can pass in a reference ImGuiStyle structure to compare to, revert to and save to (else it uses the default style)<br/>
    ///</summary>
    void ShowStyleEditor(IImGuiStyle @ref);
    ///<summary>
    /// add style selector block (not a window), essentially a combo listing the default styles.<br/>
    ///</summary>
    bool ShowStyleSelector(string label);
    ///<summary>
    /// add font selector block (not a window), essentially a combo listing the loaded fonts.<br/>
    ///</summary>
    void ShowFontSelector(string label);
    ///<summary>
    /// add basic help/info block (not a window): how to manipulate ImGui as an end-user (mouse/keyboard controls).<br/>
    ///</summary>
    void ShowUserGuide();
    ///<summary>
    /// get the compiled version string e.g. "1.80 WIP" (essentially the value for IMGUI_VERSION from the compiled version of imgui.cpp)<br/>
    ///</summary>
    string GetVersion();
    ///<summary>
    /// new, recommended style (default)<br/>
    ///<br/>
    /// Styles<br/>
    ///</summary>
    void StyleColorsDark(IImGuiStyle dst);
    ///<summary>
    /// best used with borders and a custom, thicker font<br/>
    ///</summary>
    void StyleColorsLight(IImGuiStyle dst);
    ///<summary>
    /// classic imgui style<br/>
    ///</summary>
    void StyleColorsClassic(IImGuiStyle dst);
    ///<summary>
    /// Windows<br/>
    /// - Begin() = push window to the stack and start appending to it. End() = pop window from the stack.<br/>
    /// - Passing 'bool* p_open != NULL' shows a window-closing widget in the upper-right corner of the window,<br/>
    ///   which clicking will set the boolean to false when clicked.<br/>
    /// - You may append multiple times to the same window during the same frame by calling Begin()/End() pairs multiple times.<br/>
    ///   Some information such as 'flags' or 'p_open' will only be considered by the first call to Begin().<br/>
    /// - Begin() return false to indicate the window is collapsed or fully clipped, so you may early out and omit submitting<br/>
    ///   anything to the window. Always call a matching End() for each Begin() call, regardless of its return value!<br/>
    ///   [Important: due to legacy reason, Begin/End and BeginChild/EndChild are inconsistent with all other functions<br/>
    ///    such as BeginMenu/EndMenu, BeginPopup/EndPopup, etc. where the EndXXX call should only be called if the corresponding<br/>
    ///    BeginXXX function returned true. Begin and BeginChild are the only odd ones out. Will be fixed in a future update.]<br/>
    /// - Note that the bottom of window stack always contains a window called "Debug".<br/>
    ///</summary>
    bool Begin(string name, ref bool p_open, ImGuiWindowFlags flags);
    void End();
    ///<summary>
    /// Child Windows<br/>
    /// - Use child windows to begin into a self-contained independent scrolling/clipping regions within a host window. Child windows can embed their own child.<br/>
    /// - Before 1.90 (November 2023), the "ImGuiChildFlags child_flags = 0" parameter was "bool border = false".<br/>
    ///   This API is backward compatible with old code, as we guarantee that ImGuiChildFlags_Borders == true.<br/>
    ///   Consider updating your old code:<br/>
    ///      BeginChild("Name", size, false)   -&gt; Begin("Name", size, 0); or Begin("Name", size, ImGuiChildFlags_None);<br/>
    ///      BeginChild("Name", size, true)    -&gt; Begin("Name", size, ImGuiChildFlags_Borders);<br/>
    /// - Manual sizing (each axis can use a different setting e.g. ImVec2(0.0f, 400.0f)):<br/>
    ///     == 0.0f: use remaining parent window size for this axis.<br/>
    ///      &gt; 0.0f: use specified size for this axis.<br/>
    ///      &lt; 0.0f: right/bottom-align to specified distance from available content boundaries.<br/>
    /// - Specifying ImGuiChildFlags_AutoResizeX or ImGuiChildFlags_AutoResizeY makes the sizing automatic based on child contents.<br/>
    ///   Combining both ImGuiChildFlags_AutoResizeX _and_ ImGuiChildFlags_AutoResizeY defeats purpose of a scrolling region and is NOT recommended.<br/>
    /// - BeginChild() returns false to indicate the window is collapsed or fully clipped, so you may early out and omit submitting<br/>
    ///   anything to the window. Always call a matching EndChild() for each BeginChild() call, regardless of its return value.<br/>
    ///   [Important: due to legacy reason, Begin/End and BeginChild/EndChild are inconsistent with all other functions<br/>
    ///    such as BeginMenu/EndMenu, BeginPopup/EndPopup, etc. where the EndXXX call should only be called if the corresponding<br/>
    ///    BeginXXX function returned true. Begin and BeginChild are the only odd ones out. Will be fixed in a future update.]<br/>
    ///</summary>
    bool BeginChild(string str_id, Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags);
    bool BeginChildID(uint id, Vector2 size, ImGuiChildFlags child_flags, ImGuiWindowFlags window_flags);
    void EndChild();
    ///<summary>
    /// Windows Utilities<br/>
    /// - 'current window' = the window we are appending into while inside a Begin()/End() block. 'next window' = next window we will Begin() into.<br/>
    ///</summary>
    bool IsWindowAppearing();
    bool IsWindowCollapsed();
    ///<summary>
    /// is current window focused? or its root/child, depending on flags. see flags for options.<br/>
    ///</summary>
    bool IsWindowFocused(ImGuiFocusedFlags flags);
    ///<summary>
    /// is current window hovered and hoverable (e.g. not blocked by a popup/modal)? See ImGuiHoveredFlags_ for options. IMPORTANT: If you are trying to check whether your mouse should be dispatched to Dear ImGui or to your underlying app, you should not use this function! Use the 'io.WantCaptureMouse' boolean for that! Refer to FAQ entry "How can I tell whether to dispatch mouse/keyboard to Dear ImGui or my application?" for details.<br/>
    ///</summary>
    bool IsWindowHovered(ImGuiHoveredFlags flags);
    ///<summary>
    /// get draw list associated to the current window, to append your own drawing primitives<br/>
    ///</summary>
    IImDrawList GetWindowDrawList();
    ///<summary>
    /// get DPI scale currently associated to the current window's viewport.<br/>
    ///</summary>
    float GetWindowDpiScale();
    ///<summary>
    /// get current window position in screen space (IT IS UNLIKELY YOU EVER NEED TO USE THIS. Consider always using GetCursorScreenPos() and GetContentRegionAvail() instead)<br/>
    ///</summary>
    Vector2 GetWindowPos();
    ///<summary>
    /// get current window size (IT IS UNLIKELY YOU EVER NEED TO USE THIS. Consider always using GetCursorScreenPos() and GetContentRegionAvail() instead)<br/>
    ///</summary>
    Vector2 GetWindowSize();
    ///<summary>
    /// get current window width (IT IS UNLIKELY YOU EVER NEED TO USE THIS). Shortcut for GetWindowSize().x.<br/>
    ///</summary>
    float GetWindowWidth();
    ///<summary>
    /// get current window height (IT IS UNLIKELY YOU EVER NEED TO USE THIS). Shortcut for GetWindowSize().y.<br/>
    ///</summary>
    float GetWindowHeight();
    ///<summary>
    /// get viewport currently associated to the current window.<br/>
    ///</summary>
    IImGuiViewport GetWindowViewport();
    ///<summary>
    /// Implied pivot = ImVec2(0, 0)<br/>
    ///<br/>
    /// Window manipulation<br/>
    /// - Prefer using SetNextXXX functions (before Begin) rather that SetXXX functions (after Begin).<br/>
    ///</summary>
    void SetNextWindowPos(Vector2 pos, ImGuiCond cond);
    ///<summary>
    /// set next window position. call before Begin(). use pivot=(0.5f,0.5f) to center on given point, etc.<br/>
    ///</summary>
    void SetNextWindowPosEx(Vector2 pos, ImGuiCond cond, Vector2 pivot);
    ///<summary>
    /// set next window size. set axis to 0.0f to force an auto-fit on this axis. call before Begin()<br/>
    ///</summary>
    void SetNextWindowSize(Vector2 size, ImGuiCond cond);
    ///<summary>
    /// set next window size limits. use 0.0f or FLT_MAX if you don't want limits. Use -1 for both min and max of same axis to preserve current size (which itself is a constraint). Use callback to apply non-trivial programmatic constraints.<br/>
    ///</summary>
    void SetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, delegate* unmanaged[Cdecl]<nint, void> custom_callback, void* custom_callback_data);
    ///<summary>
    /// set next window content size (~ scrollable client area, which enforce the range of scrollbars). Not including window decorations (title bar, menu bar, etc.) nor WindowPadding. set an axis to 0.0f to leave it automatic. call before Begin()<br/>
    ///</summary>
    void SetNextWindowContentSize(Vector2 size);
    ///<summary>
    /// set next window collapsed state. call before Begin()<br/>
    ///</summary>
    void SetNextWindowCollapsed(bool collapsed, ImGuiCond cond);
    ///<summary>
    /// set next window to be focused / top-most. call before Begin()<br/>
    ///</summary>
    void SetNextWindowFocus();
    ///<summary>
    /// set next window scrolling value (use &lt; 0.0f to not affect a given axis).<br/>
    ///</summary>
    void SetNextWindowScroll(Vector2 scroll);
    ///<summary>
    /// set next window background color alpha. helper to easily override the Alpha component of ImGuiCol_WindowBg/ChildBg/PopupBg. you may also use ImGuiWindowFlags_NoBackground.<br/>
    ///</summary>
    void SetNextWindowBgAlpha(float alpha);
    ///<summary>
    /// set next window viewport<br/>
    ///</summary>
    void SetNextWindowViewport(uint viewport_id);
    ///<summary>
    /// (not recommended) set current window position - call within Begin()/End(). prefer using SetNextWindowPos(), as this may incur tearing and side-effects.<br/>
    ///</summary>
    void SetWindowPos(Vector2 pos, ImGuiCond cond);
    ///<summary>
    /// (not recommended) set current window size - call within Begin()/End(). set to ImVec2(0, 0) to force an auto-fit. prefer using SetNextWindowSize(), as this may incur tearing and minor side-effects.<br/>
    ///</summary>
    void SetWindowSize(Vector2 size, ImGuiCond cond);
    ///<summary>
    /// (not recommended) set current window collapsed state. prefer using SetNextWindowCollapsed().<br/>
    ///</summary>
    void SetWindowCollapsed(bool collapsed, ImGuiCond cond);
    ///<summary>
    /// (not recommended) set current window to be focused / top-most. prefer using SetNextWindowFocus().<br/>
    ///</summary>
    void SetWindowFocus();
    ///<summary>
    /// set named window position.<br/>
    ///</summary>
    void SetWindowPosStr(string name, Vector2 pos, ImGuiCond cond);
    ///<summary>
    /// set named window size. set axis to 0.0f to force an auto-fit on this axis.<br/>
    ///</summary>
    void SetWindowSizeStr(string name, Vector2 size, ImGuiCond cond);
    ///<summary>
    /// set named window collapsed state<br/>
    ///</summary>
    void SetWindowCollapsedStr(string name, bool collapsed, ImGuiCond cond);
    ///<summary>
    /// set named window to be focused / top-most. use NULL to remove focus.<br/>
    ///</summary>
    void SetWindowFocusStr(string name);
    ///<summary>
    /// get scrolling amount [0 .. GetScrollMaxX()]<br/>
    ///<br/>
    /// Windows Scrolling<br/>
    /// - Any change of Scroll will be applied at the beginning of next frame in the first call to Begin().<br/>
    /// - You may instead use SetNextWindowScroll() prior to calling Begin() to avoid this delay, as an alternative to using SetScrollX()/SetScrollY().<br/>
    ///</summary>
    float GetScrollX();
    ///<summary>
    /// get scrolling amount [0 .. GetScrollMaxY()]<br/>
    ///</summary>
    float GetScrollY();
    ///<summary>
    /// set scrolling amount [0 .. GetScrollMaxX()]<br/>
    ///</summary>
    void SetScrollX(float scroll_x);
    ///<summary>
    /// set scrolling amount [0 .. GetScrollMaxY()]<br/>
    ///</summary>
    void SetScrollY(float scroll_y);
    ///<summary>
    /// get maximum scrolling amount ~~ ContentSize.x - WindowSize.x - DecorationsSize.x<br/>
    ///</summary>
    float GetScrollMaxX();
    ///<summary>
    /// get maximum scrolling amount ~~ ContentSize.y - WindowSize.y - DecorationsSize.y<br/>
    ///</summary>
    float GetScrollMaxY();
    ///<summary>
    /// adjust scrolling amount to make current cursor position visible. center_x_ratio=0.0: left, 0.5: center, 1.0: right. When using to make a "default/current item" visible, consider using SetItemDefaultFocus() instead.<br/>
    ///</summary>
    void SetScrollHereX(float center_x_ratio);
    ///<summary>
    /// adjust scrolling amount to make current cursor position visible. center_y_ratio=0.0: top, 0.5: center, 1.0: bottom. When using to make a "default/current item" visible, consider using SetItemDefaultFocus() instead.<br/>
    ///</summary>
    void SetScrollHereY(float center_y_ratio);
    ///<summary>
    /// adjust scrolling amount to make given position visible. Generally GetCursorStartPos() + offset to compute a valid position.<br/>
    ///</summary>
    void SetScrollFromPosX(float local_x, float center_x_ratio);
    ///<summary>
    /// adjust scrolling amount to make given position visible. Generally GetCursorStartPos() + offset to compute a valid position.<br/>
    ///</summary>
    void SetScrollFromPosY(float local_y, float center_y_ratio);
    ///<summary>
    /// Use NULL as a shortcut to keep current font. Use 0.0f to keep current size.<br/>
    ///<br/>
    /// Parameters stacks (font)<br/>
    ///  - PushFont(font, 0.0f)                        Change font and keep current size<br/>
    ///  - PushFont(NULL, 20.0f)                       Keep font and change current size<br/>
    ///  - PushFont(font, 20.0f)                       Change font and set size to 20.0f<br/>
    ///  - PushFont(font, style.FontSizeBase * 2.0f)   Change font and set size to be twice bigger than current size.<br/>
    ///  - PushFont(font, font-&gt;LegacySize)            Change font and set size to size passed to AddFontXXX() function. Same as pre-1.92 behavior.<br/>
    /// *IMPORTANT* before 1.92, fonts had a single size. They can now be dynamically be adjusted.<br/>
    ///  - In 1.92 we have REMOVED the single parameter version of PushFont() because it seems like the easiest way to provide an error-proof transition.<br/>
    ///  - PushFont(font) before 1.92 = PushFont(font, font-&gt;LegacySize) after 1.92           Use default font size as passed to AddFontXXX() function.<br/>
    /// *IMPORTANT* global scale factors are applied over the provided size.<br/>
    ///  - Global scale factors are: 'style.FontScaleMain', 'style.FontScaleDpi' and maybe more.<br/>
    /// -  If you want to apply a factor to the _current_ font size:<br/>
    ///  - CORRECT:   PushFont(NULL, style.FontSizeBase)          use current unscaled size    == does nothing<br/>
    ///  - CORRECT:   PushFont(NULL, style.FontSizeBase * 2.0f)   use current unscaled size x2 == make text twice bigger<br/>
    ///  - INCORRECT: PushFont(NULL, GetFontSize())               INCORRECT! using size after global factors already applied == GLOBAL SCALING FACTORS WILL APPLY TWICE!<br/>
    ///  - INCORRECT: PushFont(NULL, GetFontSize() * 2.0f)        INCORRECT! using size after global factors already applied == GLOBAL SCALING FACTORS WILL APPLY TWICE!<br/>
    ///</summary>
    void PushFontFloat(IImFont font, float font_size_base_unscaled);
    void PopFont();
    ///<summary>
    /// get current font<br/>
    ///</summary>
    IImFont GetFont();
    ///<summary>
    /// get current scaled font size (= height in pixels). AFTER global scale factors applied. *IMPORTANT* DO NOT PASS THIS VALUE TO PushFont()! Use ImGui::GetStyle().FontSizeBase to get value before global scale factors.<br/>
    ///</summary>
    float GetFontSize();
    ///<summary>
    /// get current font bound at current size  == GetFont()-&gt;GetFontBaked(GetFontSize())<br/>
    ///</summary>
    IImFontBaked GetFontBaked();
    ///<summary>
    /// modify a style color. always use this if you modify the style after NewFrame().<br/>
    ///<br/>
    /// Parameters stacks (shared)<br/>
    ///</summary>
    void PushStyleColor(ImGuiCol idx, uint col);
    void PushStyleColorImVec4(ImGuiCol idx, Vector4 col);
    ///<summary>
    /// Implied count = 1<br/>
    ///</summary>
    void PopStyleColor();
    void PopStyleColorEx(int count);
    ///<summary>
    /// modify a style float variable. always use this if you modify the style after NewFrame()!<br/>
    ///</summary>
    void PushStyleVar(ImGuiStyleVar idx, float val);
    ///<summary>
    /// modify a style ImVec2 variable. "<br/>
    ///</summary>
    void PushStyleVarImVec2(ImGuiStyleVar idx, Vector2 val);
    ///<summary>
    /// modify X component of a style ImVec2 variable. "<br/>
    ///</summary>
    void PushStyleVarX(ImGuiStyleVar idx, float val_x);
    ///<summary>
    /// modify Y component of a style ImVec2 variable. "<br/>
    ///</summary>
    void PushStyleVarY(ImGuiStyleVar idx, float val_y);
    ///<summary>
    /// Implied count = 1<br/>
    ///</summary>
    void PopStyleVar();
    void PopStyleVarEx(int count);
    ///<summary>
    /// modify specified shared item flag, e.g. PushItemFlag(ImGuiItemFlags_NoTabStop, true)<br/>
    ///</summary>
    void PushItemFlag(ImGuiItemFlags option, bool enabled);
    void PopItemFlag();
    ///<summary>
    /// push width of items for common large "item+label" widgets. &gt;0.0f: width in pixels, &lt;0.0f align xx pixels to the right of window (so -FLT_MIN always align width to the right side).<br/>
    ///<br/>
    /// Parameters stacks (current window)<br/>
    ///</summary>
    void PushItemWidth(float item_width);
    void PopItemWidth();
    ///<summary>
    /// set width of the _next_ common large "item+label" widget. &gt;0.0f: width in pixels, &lt;0.0f align xx pixels to the right of window (so -FLT_MIN always align width to the right side)<br/>
    ///</summary>
    void SetNextItemWidth(float item_width);
    ///<summary>
    /// width of item given pushed settings and current cursor position. NOT necessarily the width of last item unlike most 'Item' functions.<br/>
    ///</summary>
    float CalcItemWidth();
    ///<summary>
    /// push word-wrapping position for Text*() commands. &lt; 0.0f: no wrapping; 0.0f: wrap to end of window (or column); &gt; 0.0f: wrap at 'wrap_pos_x' position in window local space<br/>
    ///</summary>
    void PushTextWrapPos(float wrap_local_pos_x);
    void PopTextWrapPos();
    ///<summary>
    /// get UV coordinate for a white pixel, useful to draw custom shapes via the ImDrawList API<br/>
    ///<br/>
    /// Style read access<br/>
    /// - Use the ShowStyleEditor() function to interactively see/edit the colors.<br/>
    ///</summary>
    Vector2 GetFontTexUvWhitePixel();
    ///<summary>
    /// Implied alpha_mul = 1.0f<br/>
    ///</summary>
    uint GetColorU32(ImGuiCol idx);
    ///<summary>
    /// retrieve given style color with style alpha applied and optional extra alpha multiplier, packed as a 32-bit value suitable for ImDrawList<br/>
    ///</summary>
    uint GetColorU32Ex(ImGuiCol idx, float alpha_mul);
    ///<summary>
    /// retrieve given color with style alpha applied, packed as a 32-bit value suitable for ImDrawList<br/>
    ///</summary>
    uint GetColorU32ImVec4(Vector4 col);
    ///<summary>
    /// Implied alpha_mul = 1.0f<br/>
    ///</summary>
    uint GetColorU32ImU32(uint col);
    ///<summary>
    /// retrieve given color with style alpha applied, packed as a 32-bit value suitable for ImDrawList<br/>
    ///</summary>
    uint GetColorU32ImU32Ex(uint col, float alpha_mul);
    ///<summary>
    /// retrieve style color as stored in ImGuiStyle structure. use to feed back into PushStyleColor(), otherwise use GetColorU32() to get style color with style alpha baked in.<br/>
    ///</summary>
    Vector4 GetStyleColorVec4(ImGuiCol idx);
    ///<summary>
    /// cursor position, absolute coordinates. THIS IS YOUR BEST FRIEND (prefer using this rather than GetCursorPos(), also more useful to work with ImDrawList API).<br/>
    ///<br/>
    /// Layout cursor positioning<br/>
    /// - By "cursor" we mean the current output position.<br/>
    /// - The typical widget behavior is to output themselves at the current cursor position, then move the cursor one line down.<br/>
    /// - You can call SameLine() between widgets to undo the last carriage return and output at the right of the preceding widget.<br/>
    /// - YOU CAN DO 99% OF WHAT YOU NEED WITH ONLY GetCursorScreenPos() and GetContentRegionAvail().<br/>
    /// - Attention! We currently have inconsistencies between window-local and absolute positions we will aim to fix with future API:<br/>
    ///    - Absolute coordinate:        GetCursorScreenPos(), SetCursorScreenPos(), all ImDrawList:: functions. -&gt; this is the preferred way forward.<br/>
    ///    - Window-local coordinates:   SameLine(offset), GetCursorPos(), SetCursorPos(), GetCursorStartPos(), PushTextWrapPos()<br/>
    ///    - Window-local coordinates:   GetContentRegionMax(), GetWindowContentRegionMin(), GetWindowContentRegionMax() --&gt; all obsoleted. YOU DON'T NEED THEM.<br/>
    /// - GetCursorScreenPos() = GetCursorPos() + GetWindowPos(). GetWindowPos() is almost only ever useful to convert from window-local to absolute coordinates. Try not to use it.<br/>
    ///</summary>
    Vector2 GetCursorScreenPos();
    ///<summary>
    /// cursor position, absolute coordinates. THIS IS YOUR BEST FRIEND.<br/>
    ///</summary>
    void SetCursorScreenPos(Vector2 pos);
    ///<summary>
    /// available space from current position. THIS IS YOUR BEST FRIEND.<br/>
    ///</summary>
    Vector2 GetContentRegionAvail();
    ///<summary>
    /// [window-local] cursor position in window-local coordinates. This is not your best friend.<br/>
    ///</summary>
    Vector2 GetCursorPos();
    ///<summary>
    /// [window-local] "<br/>
    ///</summary>
    float GetCursorPosX();
    ///<summary>
    /// [window-local] "<br/>
    ///</summary>
    float GetCursorPosY();
    ///<summary>
    /// [window-local] "<br/>
    ///</summary>
    void SetCursorPos(Vector2 local_pos);
    ///<summary>
    /// [window-local] "<br/>
    ///</summary>
    void SetCursorPosX(float local_x);
    ///<summary>
    /// [window-local] "<br/>
    ///</summary>
    void SetCursorPosY(float local_y);
    ///<summary>
    /// [window-local] initial cursor position, in window-local coordinates. Call GetCursorScreenPos() after Begin() to get the absolute coordinates version.<br/>
    ///</summary>
    Vector2 GetCursorStartPos();
    ///<summary>
    /// separator, generally horizontal. inside a menu bar or in horizontal layout mode, this becomes a vertical separator.<br/>
    ///<br/>
    /// Other layout functions<br/>
    ///</summary>
    void Separator();
    ///<summary>
    /// Implied offset_from_start_x = 0.0f, spacing = -1.0f<br/>
    ///</summary>
    void SameLine();
    ///<summary>
    /// call between widgets or groups to layout them horizontally. X position given in window coordinates.<br/>
    ///</summary>
    void SameLineEx(float offset_from_start_x, float spacing);
    ///<summary>
    /// undo a SameLine() or force a new line when in a horizontal-layout context.<br/>
    ///</summary>
    void NewLine();
    ///<summary>
    /// add vertical spacing.<br/>
    ///</summary>
    void Spacing();
    ///<summary>
    /// add a dummy item of given size. unlike InvisibleButton(), Dummy() won't take the mouse click or be navigable into.<br/>
    ///</summary>
    void Dummy(Vector2 size);
    ///<summary>
    /// Implied indent_w = 0.0f<br/>
    ///</summary>
    void Indent();
    ///<summary>
    /// move content position toward the right, by indent_w, or style.IndentSpacing if indent_w &lt;= 0<br/>
    ///</summary>
    void IndentEx(float indent_w);
    ///<summary>
    /// Implied indent_w = 0.0f<br/>
    ///</summary>
    void Unindent();
    ///<summary>
    /// move content position back to the left, by indent_w, or style.IndentSpacing if indent_w &lt;= 0<br/>
    ///</summary>
    void UnindentEx(float indent_w);
    ///<summary>
    /// lock horizontal starting position<br/>
    ///</summary>
    void BeginGroup();
    ///<summary>
    /// unlock horizontal starting position + capture the whole group bounding box into one "item" (so you can use IsItemHovered() or layout primitives such as SameLine() on whole group, etc.)<br/>
    ///</summary>
    void EndGroup();
    ///<summary>
    /// vertically align upcoming text baseline to FramePadding.y so that it will align properly to regularly framed items (call if you have text on a line before a framed item)<br/>
    ///</summary>
    void AlignTextToFramePadding();
    ///<summary>
    /// ~ FontSize<br/>
    ///</summary>
    float GetTextLineHeight();
    ///<summary>
    /// ~ FontSize + style.ItemSpacing.y (distance in pixels between 2 consecutive lines of text)<br/>
    ///</summary>
    float GetTextLineHeightWithSpacing();
    ///<summary>
    /// ~ FontSize + style.FramePadding.y * 2<br/>
    ///</summary>
    float GetFrameHeight();
    ///<summary>
    /// ~ FontSize + style.FramePadding.y * 2 + style.ItemSpacing.y (distance in pixels between 2 consecutive lines of framed widgets)<br/>
    ///</summary>
    float GetFrameHeightWithSpacing();
    ///<summary>
    /// push string into the ID stack (will hash string).<br/>
    ///<br/>
    /// ID stack/scopes<br/>
    /// Read the FAQ (docs/FAQ.md or http:dearimgui.com/faq) for more details about how ID are handled in dear imgui.<br/>
    /// - Those questions are answered and impacted by understanding of the ID stack system:<br/>
    ///   - "Q: Why is my widget not reacting when I click on it?"<br/>
    ///   - "Q: How can I have widgets with an empty label?"<br/>
    ///   - "Q: How can I have multiple widgets with the same label?"<br/>
    /// - Short version: ID are hashes of the entire ID stack. If you are creating widgets in a loop you most likely<br/>
    ///   want to push a unique identifier (e.g. object pointer, loop index) to uniquely differentiate them.<br/>
    /// - You can also use the "Label##foobar" syntax within widget label to distinguish them from each others.<br/>
    /// - In this header file we use the "label"/"name" terminology to denote a string that will be displayed + used as an ID,<br/>
    ///   whereas "str_id" denote a string that is only used as an ID and not normally displayed.<br/>
    ///</summary>
    void PushID(string str_id);
    ///<summary>
    /// push string into the ID stack (will hash string).<br/>
    ///</summary>
    void PushIDStr(string str_id_begin, string str_id_end);
    ///<summary>
    /// push pointer into the ID stack (will hash pointer).<br/>
    ///</summary>
    void PushIDPtr(void* ptr_id);
    ///<summary>
    /// push integer into the ID stack (will hash integer).<br/>
    ///</summary>
    void PushIDInt(int int_id);
    ///<summary>
    /// pop from the ID stack.<br/>
    ///</summary>
    void PopID();
    ///<summary>
    /// calculate unique ID (hash of whole ID stack + given parameter). e.g. if you want to query into ImGuiStorage yourself<br/>
    ///</summary>
    uint GetID(string str_id);
    uint GetIDStr(string str_id_begin, string str_id_end);
    uint GetIDPtr(void* ptr_id);
    uint GetIDInt(int int_id);
    ///<summary>
    /// Implied text_end = NULL<br/>
    ///<br/>
    /// Widgets: Text<br/>
    ///</summary>
    void TextUnformatted(string text);
    ///<summary>
    /// raw text without formatting. Roughly equivalent to Text("%s", text) but: A) doesn't require null terminated string if 'text_end' is specified, B) it's faster, no memory copy is done, no buffer size limits, recommended for long chunks of text.<br/>
    ///</summary>
    void TextUnformattedEx(string text, string text_end);
    ///<summary>
    /// formatted text<br/>
    ///</summary>
    void Text(string fmt);
    void TextV(string fmt, sbyte* args);
    ///<summary>
    /// shortcut for PushStyleColor(ImGuiCol_Text, col); Text(fmt, ...); PopStyleColor();<br/>
    ///</summary>
    void TextColored(Vector4 col, string fmt);
    void TextColoredV(Vector4 col, string fmt, sbyte* args);
    ///<summary>
    /// shortcut for PushStyleColor(ImGuiCol_Text, style.Colors[ImGuiCol_TextDisabled]); Text(fmt, ...); PopStyleColor();<br/>
    ///</summary>
    void TextDisabled(string fmt);
    void TextDisabledV(string fmt, sbyte* args);
    ///<summary>
    /// shortcut for PushTextWrapPos(0.0f); Text(fmt, ...); PopTextWrapPos();. Note that this won't work on an auto-resizing window if there's no other widgets to extend the window width, yoy may need to set a size using SetNextWindowSize().<br/>
    ///</summary>
    void TextWrapped(string fmt);
    void TextWrappedV(string fmt, sbyte* args);
    ///<summary>
    /// display text+label aligned the same way as value+label widgets<br/>
    ///</summary>
    void LabelText(string label, string fmt);
    void LabelTextV(string label, string fmt, sbyte* args);
    ///<summary>
    /// shortcut for Bullet()+Text()<br/>
    ///</summary>
    void BulletText(string fmt);
    void BulletTextV(string fmt, sbyte* args);
    ///<summary>
    /// currently: formatted text with a horizontal line<br/>
    ///</summary>
    void SeparatorText(string label);
    ///<summary>
    /// Implied size = ImVec2(0, 0)<br/>
    ///<br/>
    /// Widgets: Main<br/>
    /// - Most widgets return true when the value has been changed or when pressed/selected<br/>
    /// - You may also use one of the many IsItemXXX functions (e.g. IsItemActive, IsItemHovered, etc.) to query widget state.<br/>
    ///</summary>
    bool Button(string label);
    ///<summary>
    /// button<br/>
    ///</summary>
    bool ButtonEx(string label, Vector2 size);
    ///<summary>
    /// button with (FramePadding.y == 0) to easily embed within text<br/>
    ///</summary>
    bool SmallButton(string label);
    ///<summary>
    /// flexible button behavior without the visuals, frequently useful to build custom behaviors using the public api (along with IsItemActive, IsItemHovered, etc.)<br/>
    ///</summary>
    bool InvisibleButton(string str_id, Vector2 size, ImGuiButtonFlags flags);
    ///<summary>
    /// square button with an arrow shape<br/>
    ///</summary>
    bool ArrowButton(string str_id, int dir);
    bool Checkbox(string label, ref bool v);
    bool CheckboxFlagsIntPtr(string label, ref int flags, int flags_value);
    bool CheckboxFlagsUintPtr(string label, ref uint flags, uint flags_value);
    ///<summary>
    /// use with e.g. if (RadioButton("one", my_value==1)) { my_value = 1; }<br/>
    ///</summary>
    bool RadioButton(string label, bool active);
    ///<summary>
    /// shortcut to handle the above pattern when value is an integer<br/>
    ///</summary>
    bool RadioButtonIntPtr(string label, ref int v, int v_button);
    void ProgressBar(float fraction, Vector2 size_arg, string overlay);
    ///<summary>
    /// draw a small circle + keep the cursor on the same line. advance cursor x position by GetTreeNodeToLabelSpacing(), same distance that TreeNode() uses<br/>
    ///</summary>
    void Bullet();
    ///<summary>
    /// hyperlink text button, return true when clicked<br/>
    ///</summary>
    bool TextLink(string label);
    ///<summary>
    /// Implied url = NULL<br/>
    ///</summary>
    bool TextLinkOpenURL(string label);
    ///<summary>
    /// hyperlink text button, automatically open file/url when clicked<br/>
    ///</summary>
    bool TextLinkOpenURLEx(string label, string url);
    ///<summary>
    /// Implied uv0 = ImVec2(0, 0), uv1 = ImVec2(1, 1)<br/>
    ///<br/>
    /// Widgets: Images<br/>
    /// - Read about ImTextureID/ImTextureRef  here: https:github.com/ocornut/imgui/wiki/Image-Loading-and-Displaying-Examples<br/>
    /// - 'uv0' and 'uv1' are texture coordinates. Read about them from the same link above.<br/>
    /// - Image() pads adds style.ImageBorderSize on each side, ImageButton() adds style.FramePadding on each side.<br/>
    /// - ImageButton() draws a background based on regular Button() color + optionally an inner background if specified.<br/>
    /// - An obsolete version of Image(), before 1.91.9 (March 2025), had a 'tint_col' parameter which is now supported by the ImageWithBg() function.<br/>
    ///</summary>
    void Image(ImTextureRef tex_ref, Vector2 image_size);
    void ImageEx(ImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1);
    ///<summary>
    /// Implied uv0 = ImVec2(0, 0), uv1 = ImVec2(1, 1), bg_col = ImVec4(0, 0, 0, 0), tint_col = ImVec4(1, 1, 1, 1)<br/>
    ///</summary>
    void ImageWithBg(ImTextureRef tex_ref, Vector2 image_size);
    void ImageWithBgEx(ImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1, Vector4 bg_col, Vector4 tint_col);
    ///<summary>
    /// Implied uv0 = ImVec2(0, 0), uv1 = ImVec2(1, 1), bg_col = ImVec4(0, 0, 0, 0), tint_col = ImVec4(1, 1, 1, 1)<br/>
    ///</summary>
    bool ImageButton(string str_id, ImTextureRef tex_ref, Vector2 image_size);
    bool ImageButtonEx(string str_id, ImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1, Vector4 bg_col, Vector4 tint_col);
    ///<summary>
    /// Widgets: Combo Box (Dropdown)<br/>
    /// - The BeginCombo()/EndCombo() api allows you to manage your contents and selection state however you want it, by creating e.g. Selectable() items.<br/>
    /// - The old Combo() api are helpers over BeginCombo()/EndCombo() which are kept available for convenience purpose. This is analogous to how ListBox are created.<br/>
    ///</summary>
    bool BeginCombo(string label, string preview_value, ImGuiComboFlags flags);
    ///<summary>
    /// only call EndCombo() if BeginCombo() returns true!<br/>
    ///</summary>
    void EndCombo();
    ///<summary>
    /// Implied popup_max_height_in_items = -1<br/>
    ///</summary>
    bool ComboChar(string label, ref int current_item, sbyte** items, int items_count);
    bool ComboCharEx(string label, ref int current_item, sbyte** items, int items_count, int popup_max_height_in_items);
    ///<summary>
    /// Implied popup_max_height_in_items = -1<br/>
    ///</summary>
    bool Combo(string label, ref int current_item, string items_separated_by_zeros);
    ///<summary>
    /// Separate items with \0 within a string, end item-list with \0\0. e.g. "One\0Two\0Three\0"<br/>
    ///</summary>
    bool ComboEx(string label, ref int current_item, string items_separated_by_zeros, int popup_max_height_in_items);
    ///<summary>
    /// Implied popup_max_height_in_items = -1<br/>
    ///</summary>
    bool ComboCallback(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count);
    bool ComboCallbackEx(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count, int popup_max_height_in_items);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0.0f, v_max = 0.0f, format = "%.3f", flags = 0<br/>
    ///<br/>
    /// Widgets: Drag Sliders<br/>
    /// - Ctrl+Click on any drag box to turn them into an input box. Manually input values aren't clamped by default and can go off-bounds. Use ImGuiSliderFlags_AlwaysClamp to always clamp.<br/>
    /// - For all the Float2/Float3/Float4/Int2/Int3/Int4 versions of every function, note that a 'float v[X]' function argument is the same as 'float* v',<br/>
    ///   the array syntax is just a way to document the number of elements that are expected to be accessible. You can pass address of your first element out of a contiguous set, e.g. &amp;myvector.x<br/>
    /// - Adjust format string to decorate the value with a prefix, a suffix, or adapt the editing and display precision e.g. "%.3f" -&gt; 1.234; "%5.2f secs" -&gt; 01.23 secs; "Biscuit: %.0f" -&gt; Biscuit: 1; etc.<br/>
    /// - Format string may also be set to NULL or use the default format ("%f" or "%d").<br/>
    /// - Speed are per-pixel of mouse movement (v_speed=0.2f: mouse needs to move by 5 pixels to increase value by 1). For keyboard/gamepad navigation, minimum speed is Max(v_speed, minimum_step_at_given_precision).<br/>
    /// - Use v_min &lt; v_max to clamp edits to given limits. Note that Ctrl+Click manual input can override those limits if ImGuiSliderFlags_AlwaysClamp is not used.<br/>
    /// - Use v_max = FLT_MAX / INT_MAX etc to avoid clamping to a maximum, same with v_min = -FLT_MAX / INT_MIN to avoid clamping to a minimum.<br/>
    /// - We use the same sets of flags for DragXXX() and SliderXXX() functions as the features are the same and it makes it easier to swap them.<br/>
    /// - Legacy: Pre-1.78 there are DragXXX() function signatures that take a final `float power=1.0f' argument instead of the `ImGuiSliderFlags flags=0' argument.<br/>
    ///   If you get a warning converting a float to ImGuiSliderFlags, read https:github.com/ocornut/imgui/issues/3361<br/>
    ///</summary>
    bool DragFloat(string label, ref float v);
    ///<summary>
    /// If v_min &gt;= v_max we have no bound<br/>
    ///</summary>
    bool DragFloatEx(string label, ref float v, float v_speed, float v_min, float v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0.0f, v_max = 0.0f, format = "%.3f", flags = 0<br/>
    ///</summary>
    bool DragFloat2(string label, ref float v);
    bool DragFloat2Ex(string label, ref float v, float v_speed, float v_min, float v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0.0f, v_max = 0.0f, format = "%.3f", flags = 0<br/>
    ///</summary>
    bool DragFloat3(string label, ref float v);
    bool DragFloat3Ex(string label, ref float v, float v_speed, float v_min, float v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0.0f, v_max = 0.0f, format = "%.3f", flags = 0<br/>
    ///</summary>
    bool DragFloat4(string label, ref float v);
    bool DragFloat4Ex(string label, ref float v, float v_speed, float v_min, float v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0.0f, v_max = 0.0f, format = "%.3f", format_max = NULL, flags = 0<br/>
    ///</summary>
    bool DragFloatRange2(string label, ref float v_current_min, ref float v_current_max);
    bool DragFloatRange2Ex(string label, ref float v_current_min, ref float v_current_max, float v_speed, float v_min, float v_max, string format, string format_max, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0, v_max = 0, format = "%d", flags = 0<br/>
    ///</summary>
    bool DragInt(string label, ref int v);
    ///<summary>
    /// If v_min &gt;= v_max we have no bound<br/>
    ///</summary>
    bool DragIntEx(string label, ref int v, float v_speed, int v_min, int v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0, v_max = 0, format = "%d", flags = 0<br/>
    ///</summary>
    bool DragInt2(string label, ref int v);
    bool DragInt2Ex(string label, ref int v, float v_speed, int v_min, int v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0, v_max = 0, format = "%d", flags = 0<br/>
    ///</summary>
    bool DragInt3(string label, ref int v);
    bool DragInt3Ex(string label, ref int v, float v_speed, int v_min, int v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0, v_max = 0, format = "%d", flags = 0<br/>
    ///</summary>
    bool DragInt4(string label, ref int v);
    bool DragInt4Ex(string label, ref int v, float v_speed, int v_min, int v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, v_min = 0, v_max = 0, format = "%d", format_max = NULL, flags = 0<br/>
    ///</summary>
    bool DragIntRange2(string label, ref int v_current_min, ref int v_current_max);
    bool DragIntRange2Ex(string label, ref int v_current_min, ref int v_current_max, float v_speed, int v_min, int v_max, string format, string format_max, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, p_min = NULL, p_max = NULL, format = NULL, flags = 0<br/>
    ///</summary>
    bool DragScalar(string label, ImGuiDataType data_type, void* p_data);
    bool DragScalarEx(string label, ImGuiDataType data_type, void* p_data, float v_speed, void* p_min, void* p_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_speed = 1.0f, p_min = NULL, p_max = NULL, format = NULL, flags = 0<br/>
    ///</summary>
    bool DragScalarN(string label, ImGuiDataType data_type, void* p_data, int components);
    bool DragScalarNEx(string label, ImGuiDataType data_type, void* p_data, int components, float v_speed, void* p_min, void* p_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%.3f", flags = 0<br/>
    ///<br/>
    /// Widgets: Regular Sliders<br/>
    /// - Ctrl+Click on any slider to turn them into an input box. Manually input values aren't clamped by default and can go off-bounds. Use ImGuiSliderFlags_AlwaysClamp to always clamp.<br/>
    /// - Adjust format string to decorate the value with a prefix, a suffix, or adapt the editing and display precision e.g. "%.3f" -&gt; 1.234; "%5.2f secs" -&gt; 01.23 secs; "Biscuit: %.0f" -&gt; Biscuit: 1; etc.<br/>
    /// - Format string may also be set to NULL or use the default format ("%f" or "%d").<br/>
    /// - Legacy: Pre-1.78 there are SliderXXX() function signatures that take a final `float power=1.0f' argument instead of the `ImGuiSliderFlags flags=0' argument.<br/>
    ///   If you get a warning converting a float to ImGuiSliderFlags, read https:github.com/ocornut/imgui/issues/3361<br/>
    ///</summary>
    bool SliderFloat(string label, ref float v, float v_min, float v_max);
    ///<summary>
    /// adjust format to decorate the value with a prefix or a suffix for in-slider labels or unit display.<br/>
    ///</summary>
    bool SliderFloatEx(string label, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%.3f", flags = 0<br/>
    ///</summary>
    bool SliderFloat2(string label, ref float v, float v_min, float v_max);
    bool SliderFloat2Ex(string label, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%.3f", flags = 0<br/>
    ///</summary>
    bool SliderFloat3(string label, ref float v, float v_min, float v_max);
    bool SliderFloat3Ex(string label, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%.3f", flags = 0<br/>
    ///</summary>
    bool SliderFloat4(string label, ref float v, float v_min, float v_max);
    bool SliderFloat4Ex(string label, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied v_degrees_min = -360.0f, v_degrees_max = +360.0f, format = "%.0f deg", flags = 0<br/>
    ///</summary>
    bool SliderAngle(string label, ref float v_rad);
    bool SliderAngleEx(string label, ref float v_rad, float v_degrees_min, float v_degrees_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%d", flags = 0<br/>
    ///</summary>
    bool SliderInt(string label, ref int v, int v_min, int v_max);
    bool SliderIntEx(string label, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%d", flags = 0<br/>
    ///</summary>
    bool SliderInt2(string label, ref int v, int v_min, int v_max);
    bool SliderInt2Ex(string label, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%d", flags = 0<br/>
    ///</summary>
    bool SliderInt3(string label, ref int v, int v_min, int v_max);
    bool SliderInt3Ex(string label, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%d", flags = 0<br/>
    ///</summary>
    bool SliderInt4(string label, ref int v, int v_min, int v_max);
    bool SliderInt4Ex(string label, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = NULL, flags = 0<br/>
    ///</summary>
    bool SliderScalar(string label, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max);
    bool SliderScalarEx(string label, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = NULL, flags = 0<br/>
    ///</summary>
    bool SliderScalarN(string label, ImGuiDataType data_type, void* p_data, int components, void* p_min, void* p_max);
    bool SliderScalarNEx(string label, ImGuiDataType data_type, void* p_data, int components, void* p_min, void* p_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%.3f", flags = 0<br/>
    ///</summary>
    bool VSliderFloat(string label, Vector2 size, ref float v, float v_min, float v_max);
    bool VSliderFloatEx(string label, Vector2 size, ref float v, float v_min, float v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = "%d", flags = 0<br/>
    ///</summary>
    bool VSliderInt(string label, Vector2 size, ref int v, int v_min, int v_max);
    bool VSliderIntEx(string label, Vector2 size, ref int v, int v_min, int v_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied format = NULL, flags = 0<br/>
    ///</summary>
    bool VSliderScalar(string label, Vector2 size, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max);
    bool VSliderScalarEx(string label, Vector2 size, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max, string format, ImGuiSliderFlags flags);
    ///<summary>
    /// Implied callback = NULL, user_data = NULL<br/>
    ///<br/>
    /// Widgets: Input with Keyboard<br/>
    /// - If you want to use InputText() with std::string or any custom dynamic string type, use the wrapper in misc/cpp/imgui_stdlib.h/.cpp!<br/>
    /// - Most of the ImGuiInputTextFlags flags are only useful for InputText() and not for InputFloatX, InputIntX, InputDouble etc.<br/>
    ///</summary>
    bool InputText(string label, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags);
    bool InputTextEx(string label, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags, delegate* unmanaged[Cdecl]<nint, int> callback, void* user_data);
    ///<summary>
    /// Implied size = ImVec2(0, 0), flags = 0, callback = NULL, user_data = NULL<br/>
    ///</summary>
    bool InputTextMultiline(string label, sbyte* buf, nuint buf_size);
    bool InputTextMultilineEx(string label, sbyte* buf, nuint buf_size, Vector2 size, ImGuiInputTextFlags flags, delegate* unmanaged[Cdecl]<nint, int> callback, void* user_data);
    ///<summary>
    /// Implied callback = NULL, user_data = NULL<br/>
    ///</summary>
    bool InputTextWithHint(string label, string hint, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags);
    bool InputTextWithHintEx(string label, string hint, sbyte* buf, nuint buf_size, ImGuiInputTextFlags flags, delegate* unmanaged[Cdecl]<nint, int> callback, void* user_data);
    ///<summary>
    /// Implied step = 0.0f, step_fast = 0.0f, format = "%.3f", flags = 0<br/>
    ///</summary>
    bool InputFloat(string label, ref float v);
    bool InputFloatEx(string label, ref float v, float step, float step_fast, string format, ImGuiInputTextFlags flags);
    ///<summary>
    /// Implied format = "%.3f", flags = 0<br/>
    ///</summary>
    bool InputFloat2(string label, ref float v);
    bool InputFloat2Ex(string label, ref float v, string format, ImGuiInputTextFlags flags);
    ///<summary>
    /// Implied format = "%.3f", flags = 0<br/>
    ///</summary>
    bool InputFloat3(string label, ref float v);
    bool InputFloat3Ex(string label, ref float v, string format, ImGuiInputTextFlags flags);
    ///<summary>
    /// Implied format = "%.3f", flags = 0<br/>
    ///</summary>
    bool InputFloat4(string label, ref float v);
    bool InputFloat4Ex(string label, ref float v, string format, ImGuiInputTextFlags flags);
    ///<summary>
    /// Implied step = 1, step_fast = 100, flags = 0<br/>
    ///</summary>
    bool InputInt(string label, ref int v);
    bool InputIntEx(string label, ref int v, int step, int step_fast, ImGuiInputTextFlags flags);
    bool InputInt2(string label, ref int v, ImGuiInputTextFlags flags);
    bool InputInt3(string label, ref int v, ImGuiInputTextFlags flags);
    bool InputInt4(string label, ref int v, ImGuiInputTextFlags flags);
    ///<summary>
    /// Implied step = 0.0, step_fast = 0.0, format = "%.6f", flags = 0<br/>
    ///</summary>
    bool InputDouble(string label, ref double v);
    bool InputDoubleEx(string label, ref double v, double step, double step_fast, string format, ImGuiInputTextFlags flags);
    ///<summary>
    /// Implied p_step = NULL, p_step_fast = NULL, format = NULL, flags = 0<br/>
    ///</summary>
    bool InputScalar(string label, ImGuiDataType data_type, void* p_data);
    bool InputScalarEx(string label, ImGuiDataType data_type, void* p_data, void* p_step, void* p_step_fast, string format, ImGuiInputTextFlags flags);
    ///<summary>
    /// Implied p_step = NULL, p_step_fast = NULL, format = NULL, flags = 0<br/>
    ///</summary>
    bool InputScalarN(string label, ImGuiDataType data_type, void* p_data, int components);
    bool InputScalarNEx(string label, ImGuiDataType data_type, void* p_data, int components, void* p_step, void* p_step_fast, string format, ImGuiInputTextFlags flags);
    ///<summary>
    /// Widgets: Color Editor/Picker (tip: the ColorEdit* functions have a little color square that can be left-clicked to open a picker, and right-clicked to open an option menu.)<br/>
    /// - Note that in C++ a 'float v[X]' function argument is the _same_ as 'float* v', the array syntax is just a way to document the number of elements that are expected to be accessible.<br/>
    /// - You can pass the address of a first float element out of a contiguous structure, e.g. &amp;myvector.x<br/>
    ///</summary>
    bool ColorEdit3(string label, ref float col, ImGuiColorEditFlags flags);
    bool ColorEdit4(string label, ref float col, ImGuiColorEditFlags flags);
    bool ColorPicker3(string label, ref float col, ImGuiColorEditFlags flags);
    bool ColorPicker4(string label, ref float col, ImGuiColorEditFlags flags, ref float ref_col);
    ///<summary>
    /// Implied size = ImVec2(0, 0)<br/>
    ///</summary>
    bool ColorButton(string desc_id, Vector4 col, ImGuiColorEditFlags flags);
    ///<summary>
    /// display a color square/button, hover for details, return true when pressed.<br/>
    ///</summary>
    bool ColorButtonEx(string desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size);
    ///<summary>
    /// initialize current options (generally on application startup) if you want to select a default format, picker type, etc. User will be able to change many settings, unless you pass the _NoOptions flag to your calls.<br/>
    ///</summary>
    void SetColorEditOptions(ImGuiColorEditFlags flags);
    ///<summary>
    /// Widgets: Trees<br/>
    /// - TreeNode functions return true when the node is open, in which case you need to also call TreePop() when you are finished displaying the tree node contents.<br/>
    ///</summary>
    bool TreeNode(string label);
    ///<summary>
    /// helper variation to easily decorrelate the id from the displayed string. Read the FAQ about why and how to use ID. to align arbitrary text at the same level as a TreeNode() you can use Bullet().<br/>
    ///</summary>
    bool TreeNodeStr(string str_id, string fmt);
    ///<summary>
    /// "<br/>
    ///</summary>
    bool TreeNodePtr(void* ptr_id, string fmt);
    bool TreeNodeV(string str_id, string fmt, sbyte* args);
    bool TreeNodeVPtr(void* ptr_id, string fmt, sbyte* args);
    bool TreeNodeEx(string label, ImGuiTreeNodeFlags flags);
    bool TreeNodeExStr(string str_id, ImGuiTreeNodeFlags flags, string fmt);
    bool TreeNodeExPtr(void* ptr_id, ImGuiTreeNodeFlags flags, string fmt);
    bool TreeNodeExV(string str_id, ImGuiTreeNodeFlags flags, string fmt, sbyte* args);
    bool TreeNodeExVPtr(void* ptr_id, ImGuiTreeNodeFlags flags, string fmt, sbyte* args);
    ///<summary>
    /// ~ Indent()+PushID(). Already called by TreeNode() when returning true, but you can call TreePush/TreePop yourself if desired.<br/>
    ///</summary>
    void TreePush(string str_id);
    ///<summary>
    /// "<br/>
    ///</summary>
    void TreePushPtr(void* ptr_id);
    ///<summary>
    /// ~ Unindent()+PopID()<br/>
    ///</summary>
    void TreePop();
    ///<summary>
    /// horizontal distance preceding label when using TreeNode*() or Bullet() == (g.FontSize + style.FramePadding.x*2) for a regular unframed TreeNode<br/>
    ///</summary>
    float GetTreeNodeToLabelSpacing();
    ///<summary>
    /// if returning 'true' the header is open. doesn't indent nor push on ID stack. user doesn't have to call TreePop().<br/>
    ///</summary>
    bool CollapsingHeader(string label, ImGuiTreeNodeFlags flags);
    ///<summary>
    /// when 'p_visible != NULL': if '*p_visible==true' display an additional small close button on upper right of the header which will set the bool to false when clicked, if '*p_visible==false' don't display the header.<br/>
    ///</summary>
    bool CollapsingHeaderBoolPtr(string label, ref bool p_visible, ImGuiTreeNodeFlags flags);
    ///<summary>
    /// set next TreeNode/CollapsingHeader open state.<br/>
    ///</summary>
    void SetNextItemOpen(bool is_open, ImGuiCond cond);
    ///<summary>
    /// set id to use for open/close storage (default to same as item id).<br/>
    ///</summary>
    void SetNextItemStorageID(uint storage_id);
    ///<summary>
    /// Implied selected = false, flags = 0, size = ImVec2(0, 0)<br/>
    ///<br/>
    /// Widgets: Selectables<br/>
    /// - A selectable highlights when hovered, and can display another color when selected.<br/>
    /// - Neighbors selectable extend their highlight bounds in order to leave no gap between them. This is so a series of selected Selectable appear contiguous.<br/>
    ///</summary>
    bool Selectable(string label);
    ///<summary>
    /// "bool selected" carry the selection state (read-only). Selectable() is clicked is returns true so you can modify your selection state. size.x==0.0: use remaining width, size.x&gt;0.0: specify width. size.y==0.0: use label height, size.y&gt;0.0: specify height<br/>
    ///</summary>
    bool SelectableEx(string label, bool selected, ImGuiSelectableFlags flags, Vector2 size);
    ///<summary>
    /// Implied size = ImVec2(0, 0)<br/>
    ///</summary>
    bool SelectableBoolPtr(string label, ref bool p_selected, ImGuiSelectableFlags flags);
    ///<summary>
    /// "bool* p_selected" point to the selection state (read-write), as a convenient helper.<br/>
    ///</summary>
    bool SelectableBoolPtrEx(string label, ref bool p_selected, ImGuiSelectableFlags flags, Vector2 size);
    ///<summary>
    /// Implied selection_size = -1, items_count = -1<br/>
    ///<br/>
    /// Multi-selection system for Selectable(), Checkbox(), TreeNode() functions [BETA]<br/>
    /// - This enables standard multi-selection/range-selection idioms (Ctrl+Mouse/Keyboard, Shift+Mouse/Keyboard, etc.) in a way that also allow a clipper to be used.<br/>
    /// - ImGuiSelectionUserData is often used to store your item index within the current view (but may store something else).<br/>
    /// - Read comments near ImGuiMultiSelectIO for instructions/details and see 'Demo-&gt;Widgets-&gt;Selection State &amp; Multi-Select' for demo.<br/>
    /// - TreeNode() is technically supported but... using this correctly is more complicated. You need some sort of linear/random access to your tree,<br/>
    ///   which is suited to advanced trees setups already implementing filters and clipper. We will work simplifying the current demo.<br/>
    /// - 'selection_size' and 'items_count' parameters are optional and used by a few features. If they are costly for you to compute, you may avoid them.<br/>
    ///</summary>
    IImGuiMultiSelectIO BeginMultiSelect(ImGuiMultiSelectFlags flags);
    IImGuiMultiSelectIO BeginMultiSelectEx(ImGuiMultiSelectFlags flags, int selection_size, int items_count);
    IImGuiMultiSelectIO EndMultiSelect();
    void SetNextItemSelectionUserData(long selection_user_data);
    ///<summary>
    /// Was the last item selection state toggled? Useful if you need the per-item information _before_ reaching EndMultiSelect(). We only returns toggle _event_ in order to handle clipping correctly.<br/>
    ///</summary>
    bool IsItemToggledSelection();
    ///<summary>
    /// open a framed scrolling region<br/>
    ///<br/>
    /// Widgets: List Boxes<br/>
    /// - This is essentially a thin wrapper to using BeginChild/EndChild with the ImGuiChildFlags_FrameStyle flag for stylistic changes + displaying a label.<br/>
    /// - If you don't need a label you can probably simply use BeginChild() with the ImGuiChildFlags_FrameStyle flag for the same result.<br/>
    /// - You can submit contents and manage your selection state however you want it, by creating e.g. Selectable() or any other items.<br/>
    /// - The simplified/old ListBox() api are helpers over BeginListBox()/EndListBox() which are kept available for convenience purpose. This is analogous to how Combos are created.<br/>
    /// - Choose frame width:   size.x &gt; 0.0f: custom  /  size.x &lt; 0.0f or -FLT_MIN: right-align   /  size.x = 0.0f (default): use current ItemWidth<br/>
    /// - Choose frame height:  size.y &gt; 0.0f: custom  /  size.y &lt; 0.0f or -FLT_MIN: bottom-align  /  size.y = 0.0f (default): arbitrary default height which can fit ~7 items<br/>
    ///</summary>
    bool BeginListBox(string label, Vector2 size);
    ///<summary>
    /// only call EndListBox() if BeginListBox() returned true!<br/>
    ///</summary>
    void EndListBox();
    bool ListBox(string label, ref int current_item, sbyte** items, int items_count, int height_in_items);
    ///<summary>
    /// Implied height_in_items = -1<br/>
    ///</summary>
    bool ListBoxCallback(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count);
    bool ListBoxCallbackEx(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint> getter, void* user_data, int items_count, int height_in_items);
    ///<summary>
    /// Implied values_offset = 0, overlay_text = NULL, scale_min = FLT_MAX, scale_max = FLT_MAX, graph_size = ImVec2(0, 0), stride = sizeof(float)<br/>
    ///<br/>
    /// Widgets: Data Plotting<br/>
    /// - Consider using ImPlot (https:github.com/epezent/implot) which is much better!<br/>
    ///</summary>
    void PlotLines(string label, ref float values, int values_count);
    void PlotLinesEx(string label, ref float values, int values_count, int values_offset, string overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
    ///<summary>
    /// Implied values_offset = 0, overlay_text = NULL, scale_min = FLT_MAX, scale_max = FLT_MAX, graph_size = ImVec2(0, 0)<br/>
    ///</summary>
    void PlotLinesCallback(string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count);
    void PlotLinesCallbackEx(string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count, int values_offset, string overlay_text, float scale_min, float scale_max, Vector2 graph_size);
    ///<summary>
    /// Implied values_offset = 0, overlay_text = NULL, scale_min = FLT_MAX, scale_max = FLT_MAX, graph_size = ImVec2(0, 0), stride = sizeof(float)<br/>
    ///</summary>
    void PlotHistogram(string label, ref float values, int values_count);
    void PlotHistogramEx(string label, ref float values, int values_count, int values_offset, string overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
    ///<summary>
    /// Implied values_offset = 0, overlay_text = NULL, scale_min = FLT_MAX, scale_max = FLT_MAX, graph_size = ImVec2(0, 0)<br/>
    ///</summary>
    void PlotHistogramCallback(string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count);
    void PlotHistogramCallbackEx(string label, delegate* unmanaged[Cdecl]<nint, int, float> values_getter, void* data, int values_count, int values_offset, string overlay_text, float scale_min, float scale_max, Vector2 graph_size);
    ///<summary>
    /// append to menu-bar of current window (requires ImGuiWindowFlags_MenuBar flag set on parent window).<br/>
    ///<br/>
    /// Widgets: Menus<br/>
    /// - Use BeginMenuBar() on a window ImGuiWindowFlags_MenuBar to append to its menu bar.<br/>
    /// - Use BeginMainMenuBar() to create a menu bar at the top of the screen and append to it.<br/>
    /// - Use BeginMenu() to create a menu. You can call BeginMenu() multiple time with the same identifier to append more items to it.<br/>
    /// - Not that MenuItem() keyboardshortcuts are displayed as a convenience but _not processed_ by Dear ImGui at the moment.<br/>
    ///</summary>
    bool BeginMenuBar();
    ///<summary>
    /// only call EndMenuBar() if BeginMenuBar() returns true!<br/>
    ///</summary>
    void EndMenuBar();
    ///<summary>
    /// create and append to a full screen menu-bar.<br/>
    ///</summary>
    bool BeginMainMenuBar();
    ///<summary>
    /// only call EndMainMenuBar() if BeginMainMenuBar() returns true!<br/>
    ///</summary>
    void EndMainMenuBar();
    ///<summary>
    /// Implied enabled = true<br/>
    ///</summary>
    bool BeginMenu(string label);
    ///<summary>
    /// create a sub-menu entry. only call EndMenu() if this returns true!<br/>
    ///</summary>
    bool BeginMenuEx(string label, bool enabled);
    ///<summary>
    /// only call EndMenu() if BeginMenu() returns true!<br/>
    ///</summary>
    void EndMenu();
    ///<summary>
    /// Implied shortcut = NULL, selected = false, enabled = true<br/>
    ///</summary>
    bool MenuItem(string label);
    ///<summary>
    /// return true when activated.<br/>
    ///</summary>
    bool MenuItemEx(string label, string shortcut, bool selected, bool enabled);
    ///<summary>
    /// return true when activated + toggle (*p_selected) if p_selected != NULL<br/>
    ///</summary>
    bool MenuItemBoolPtr(string label, string shortcut, ref bool p_selected, bool enabled);
    ///<summary>
    /// begin/append a tooltip window.<br/>
    ///<br/>
    /// Tooltips<br/>
    /// - Tooltips are windows following the mouse. They do not take focus away.<br/>
    /// - A tooltip window can contain items of any types.<br/>
    /// - SetTooltip() is more or less a shortcut for the 'if (BeginTooltip()) { Text(...); EndTooltip(); }' idiom (with a subtlety that it discard any previously submitted tooltip)<br/>
    ///</summary>
    bool BeginTooltip();
    ///<summary>
    /// only call EndTooltip() if BeginTooltip()/BeginItemTooltip() returns true!<br/>
    ///</summary>
    void EndTooltip();
    ///<summary>
    /// set a text-only tooltip. Often used after a ImGui::IsItemHovered() check. Override any previous call to SetTooltip().<br/>
    ///</summary>
    void SetTooltip(string fmt);
    void SetTooltipV(string fmt, sbyte* args);
    ///<summary>
    /// begin/append a tooltip window if preceding item was hovered.<br/>
    ///<br/>
    /// Tooltips: helpers for showing a tooltip when hovering an item<br/>
    /// - BeginItemTooltip() is a shortcut for the 'if (IsItemHovered(ImGuiHoveredFlags_ForTooltip) &amp;&amp; BeginTooltip())' idiom.<br/>
    /// - SetItemTooltip() is a shortcut for the 'if (IsItemHovered(ImGuiHoveredFlags_ForTooltip)) { SetTooltip(...); }' idiom.<br/>
    /// - Where 'ImGuiHoveredFlags_ForTooltip' itself is a shortcut to use 'style.HoverFlagsForTooltipMouse' or 'style.HoverFlagsForTooltipNav' depending on active input type. For mouse it defaults to 'ImGuiHoveredFlags_Stationary | ImGuiHoveredFlags_DelayShort'.<br/>
    ///</summary>
    bool BeginItemTooltip();
    ///<summary>
    /// set a text-only tooltip if preceding item was hovered. override any previous call to SetTooltip().<br/>
    ///</summary>
    void SetItemTooltip(string fmt);
    void SetItemTooltipV(string fmt, sbyte* args);
    ///<summary>
    /// return true if the popup is open, and you can start outputting to it.<br/>
    ///<br/>
    /// Popups, Modals<br/>
    ///  - They block normal mouse hovering detection (and therefore most mouse interactions) behind them.<br/>
    ///  - If not modal: they can be closed by clicking anywhere outside them, or by pressing ESCAPE.<br/>
    ///  - Their visibility state (~bool) is held internally instead of being held by the programmer as we are used to with regular Begin*() calls.<br/>
    ///  - The 3 properties above are related: we need to retain popup visibility state in the library because popups may be closed as any time.<br/>
    ///  - You can bypass the hovering restriction by using ImGuiHoveredFlags_AllowWhenBlockedByPopup when calling IsItemHovered() or IsWindowHovered().<br/>
    ///  - IMPORTANT: Popup identifiers are relative to the current ID stack, so OpenPopup and BeginPopup generally needs to be at the same level of the stack.<br/>
    ///    This is sometimes leading to confusing mistakes. May rework this in the future.<br/>
    ///  - BeginPopup(): query popup state, if open start appending into the window. Call EndPopup() afterwards if returned true. ImGuiWindowFlags are forwarded to the window.<br/>
    ///  - BeginPopupModal(): block every interaction behind the window, cannot be closed by user, add a dimming background, has a title bar.<br/>
    ///</summary>
    bool BeginPopup(string str_id, ImGuiWindowFlags flags);
    ///<summary>
    /// return true if the modal is open, and you can start outputting to it.<br/>
    ///</summary>
    bool BeginPopupModal(string name, ref bool p_open, ImGuiWindowFlags flags);
    ///<summary>
    /// only call EndPopup() if BeginPopupXXX() returns true!<br/>
    ///</summary>
    void EndPopup();
    ///<summary>
    /// call to mark popup as open (don't call every frame!).<br/>
    ///<br/>
    /// Popups: open/close functions<br/>
    ///  - OpenPopup(): set popup state to open. ImGuiPopupFlags are available for opening options.<br/>
    ///  - If not modal: they can be closed by clicking anywhere outside them, or by pressing ESCAPE.<br/>
    ///  - CloseCurrentPopup(): use inside the BeginPopup()/EndPopup() scope to close manually.<br/>
    ///  - CloseCurrentPopup() is called by default by Selectable()/MenuItem() when activated (FIXME: need some options).<br/>
    ///  - Use ImGuiPopupFlags_NoOpenOverExistingPopup to avoid opening a popup if there's already one at the same level. This is equivalent to e.g. testing for !IsAnyPopupOpen() prior to OpenPopup().<br/>
    ///  - Use IsWindowAppearing() after BeginPopup() to tell if a window just opened.<br/>
    ///  - IMPORTANT: Notice that for OpenPopupOnItemClick() we exceptionally default flags to 1 (== ImGuiPopupFlags_MouseButtonRight) for backward compatibility with older API taking 'int mouse_button = 1' parameter<br/>
    ///</summary>
    void OpenPopup(string str_id, ImGuiPopupFlags popup_flags);
    ///<summary>
    /// id overload to facilitate calling from nested stacks<br/>
    ///</summary>
    void OpenPopupID(uint id, ImGuiPopupFlags popup_flags);
    ///<summary>
    /// helper to open popup when clicked on last item. Default to ImGuiPopupFlags_MouseButtonRight == 1. (note: actually triggers on the mouse _released_ event to be consistent with popup behaviors)<br/>
    ///</summary>
    void OpenPopupOnItemClick(string str_id, ImGuiPopupFlags popup_flags);
    ///<summary>
    /// manually close the popup we have begin-ed into.<br/>
    ///</summary>
    void CloseCurrentPopup();
    ///<summary>
    /// Implied str_id = NULL, popup_flags = 1<br/>
    ///<br/>
    /// Popups: open+begin combined functions helpers<br/>
    ///  - Helpers to do OpenPopup+BeginPopup where the Open action is triggered by e.g. hovering an item and right-clicking.<br/>
    ///  - They are convenient to easily create context menus, hence the name.<br/>
    ///  - IMPORTANT: Notice that BeginPopupContextXXX takes ImGuiPopupFlags just like OpenPopup() and unlike BeginPopup(). For full consistency, we may add ImGuiWindowFlags to the BeginPopupContextXXX functions in the future.<br/>
    ///  - IMPORTANT: Notice that we exceptionally default their flags to 1 (== ImGuiPopupFlags_MouseButtonRight) for backward compatibility with older API taking 'int mouse_button = 1' parameter, so if you add other flags remember to re-add the ImGuiPopupFlags_MouseButtonRight.<br/>
    ///</summary>
    bool BeginPopupContextItem();
    ///<summary>
    /// open+begin popup when clicked on last item. Use str_id==NULL to associate the popup to previous item. If you want to use that on a non-interactive item such as Text() you need to pass in an explicit ID here. read comments in .cpp!<br/>
    ///</summary>
    bool BeginPopupContextItemEx(string str_id, ImGuiPopupFlags popup_flags);
    ///<summary>
    /// Implied str_id = NULL, popup_flags = 1<br/>
    ///</summary>
    bool BeginPopupContextWindow();
    ///<summary>
    /// open+begin popup when clicked on current window.<br/>
    ///</summary>
    bool BeginPopupContextWindowEx(string str_id, ImGuiPopupFlags popup_flags);
    ///<summary>
    /// Implied str_id = NULL, popup_flags = 1<br/>
    ///</summary>
    bool BeginPopupContextVoid();
    ///<summary>
    /// open+begin popup when clicked in void (where there are no windows).<br/>
    ///</summary>
    bool BeginPopupContextVoidEx(string str_id, ImGuiPopupFlags popup_flags);
    ///<summary>
    /// return true if the popup is open.<br/>
    ///<br/>
    /// Popups: query functions<br/>
    ///  - IsPopupOpen(): return true if the popup is open at the current BeginPopup() level of the popup stack.<br/>
    ///  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId: return true if any popup is open at the current BeginPopup() level of the popup stack.<br/>
    ///  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId + ImGuiPopupFlags_AnyPopupLevel: return true if any popup is open.<br/>
    ///</summary>
    bool IsPopupOpen(string str_id, ImGuiPopupFlags flags);
    ///<summary>
    /// Implied outer_size = ImVec2(0.0f, 0.0f), inner_width = 0.0f<br/>
    ///<br/>
    /// Tables<br/>
    /// - Full-featured replacement for old Columns API.<br/>
    /// - See Demo-&gt;Tables for demo code. See top of imgui_tables.cpp for general commentary.<br/>
    /// - See ImGuiTableFlags_ and ImGuiTableColumnFlags_ enums for a description of available flags.<br/>
    /// The typical call flow is:<br/>
    /// - 1. Call BeginTable(), early out if returning false.<br/>
    /// - 2. Optionally call TableSetupColumn() to submit column name/flags/defaults.<br/>
    /// - 3. Optionally call TableSetupScrollFreeze() to request scroll freezing of columns/rows.<br/>
    /// - 4. Optionally call TableHeadersRow() to submit a header row. Names are pulled from TableSetupColumn() data.<br/>
    /// - 5. Populate contents:<br/>
    ///    - In most situations you can use TableNextRow() + TableSetColumnIndex(N) to start appending into a column.<br/>
    ///    - If you are using tables as a sort of grid, where every column is holding the same type of contents,<br/>
    ///      you may prefer using TableNextColumn() instead of TableNextRow() + TableSetColumnIndex().<br/>
    ///      TableNextColumn() will automatically wrap-around into the next row if needed.<br/>
    ///    - IMPORTANT: Comparatively to the old Columns() API, we need to call TableNextColumn() for the first column!<br/>
    ///    - Summary of possible call flow:<br/>
    ///        - TableNextRow() -&gt; TableSetColumnIndex(0) -&gt; Text("Hello 0") -&gt; TableSetColumnIndex(1) -&gt; Text("Hello 1")   OK<br/>
    ///        - TableNextRow() -&gt; TableNextColumn()      -&gt; Text("Hello 0") -&gt; TableNextColumn()      -&gt; Text("Hello 1")   OK<br/>
    ///        -                   TableNextColumn()      -&gt; Text("Hello 0") -&gt; TableNextColumn()      -&gt; Text("Hello 1")   OK: TableNextColumn() automatically gets to next row!<br/>
    ///        - TableNextRow()                           -&gt; Text("Hello 0")                                                Not OK! Missing TableSetColumnIndex() or TableNextColumn()! Text will not appear!<br/>
    /// - 5. Call EndTable()<br/>
    ///</summary>
    bool BeginTable(string str_id, int columns, ImGuiTableFlags flags);
    bool BeginTableEx(string str_id, int columns, ImGuiTableFlags flags, Vector2 outer_size, float inner_width);
    ///<summary>
    /// only call EndTable() if BeginTable() returns true!<br/>
    ///</summary>
    void EndTable();
    ///<summary>
    /// Implied row_flags = 0, min_row_height = 0.0f<br/>
    ///</summary>
    void TableNextRow();
    ///<summary>
    /// append into the first cell of a new row.<br/>
    ///</summary>
    void TableNextRowEx(ImGuiTableRowFlags row_flags, float min_row_height);
    ///<summary>
    /// append into the next column (or first column of next row if currently in last column). Return true when column is visible.<br/>
    ///</summary>
    bool TableNextColumn();
    ///<summary>
    /// append into the specified column. Return true when column is visible.<br/>
    ///</summary>
    bool TableSetColumnIndex(int column_n);
    ///<summary>
    /// Implied init_width_or_weight = 0.0f, user_id = 0<br/>
    ///<br/>
    /// Tables: Headers &amp; Columns declaration<br/>
    /// - Use TableSetupColumn() to specify label, resizing policy, default width/weight, id, various other flags etc.<br/>
    /// - Use TableHeadersRow() to create a header row and automatically submit a TableHeader() for each column.<br/>
    ///   Headers are required to perform: reordering, sorting, and opening the context menu.<br/>
    ///   The context menu can also be made available in columns body using ImGuiTableFlags_ContextMenuInBody.<br/>
    /// - You may manually submit headers using TableNextRow() + TableHeader() calls, but this is only useful in<br/>
    ///   some advanced use cases (e.g. adding custom widgets in header row).<br/>
    /// - Use TableSetupScrollFreeze() to lock columns/rows so they stay visible when scrolled.<br/>
    ///</summary>
    void TableSetupColumn(string label, ImGuiTableColumnFlags flags);
    void TableSetupColumnEx(string label, ImGuiTableColumnFlags flags, float init_width_or_weight, uint user_id);
    ///<summary>
    /// lock columns/rows so they stay visible when scrolled.<br/>
    ///</summary>
    void TableSetupScrollFreeze(int cols, int rows);
    ///<summary>
    /// submit one header cell manually (rarely used)<br/>
    ///</summary>
    void TableHeader(string label);
    ///<summary>
    /// submit a row with headers cells based on data provided to TableSetupColumn() + submit context menu<br/>
    ///</summary>
    void TableHeadersRow();
    ///<summary>
    /// submit a row with angled headers for every column with the ImGuiTableColumnFlags_AngledHeader flag. MUST BE FIRST ROW.<br/>
    ///</summary>
    void TableAngledHeadersRow();
    ///<summary>
    /// get latest sort specs for the table (NULL if not sorting).  Lifetime: don't hold on this pointer over multiple frames or past any subsequent call to BeginTable().<br/>
    ///<br/>
    /// Tables: Sorting &amp; Miscellaneous functions<br/>
    /// - Sorting: call TableGetSortSpecs() to retrieve latest sort specs for the table. NULL when not sorting.<br/>
    ///   When 'sort_specs-&gt;SpecsDirty == true' you should sort your data. It will be true when sorting specs have<br/>
    ///   changed since last call, or the first time. Make sure to set 'SpecsDirty = false' after sorting,<br/>
    ///   else you may wastefully sort your data every frame!<br/>
    /// - Functions args 'int column_n' treat the default value of -1 as the same as passing the current column index.<br/>
    ///</summary>
    IImGuiTableSortSpecs TableGetSortSpecs();
    ///<summary>
    /// return number of columns (value passed to BeginTable)<br/>
    ///</summary>
    int TableGetColumnCount();
    ///<summary>
    /// return current column index.<br/>
    ///</summary>
    int TableGetColumnIndex();
    ///<summary>
    /// return current row index (header rows are accounted for)<br/>
    ///</summary>
    int TableGetRowIndex();
    ///<summary>
    /// return "" if column didn't have a name declared by TableSetupColumn(). Pass -1 to use current column.<br/>
    ///</summary>
    string TableGetColumnName(int column_n);
    ///<summary>
    /// return column flags so you can query their Enabled/Visible/Sorted/Hovered status flags. Pass -1 to use current column.<br/>
    ///</summary>
    ImGuiTableColumnFlags TableGetColumnFlags(int column_n);
    ///<summary>
    /// change user accessible enabled/disabled state of a column. Set to false to hide the column. User can use the context menu to change this themselves (right-click in headers, or right-click in columns body with ImGuiTableFlags_ContextMenuInBody)<br/>
    ///</summary>
    void TableSetColumnEnabled(int column_n, bool v);
    ///<summary>
    /// return hovered column. return -1 when table is not hovered. return columns_count if the unused space at the right of visible columns is hovered. Can also use (TableGetColumnFlags() &amp; ImGuiTableColumnFlags_IsHovered) instead.<br/>
    ///</summary>
    int TableGetHoveredColumn();
    ///<summary>
    /// change the color of a cell, row, or column. See ImGuiTableBgTarget_ flags for details.<br/>
    ///</summary>
    void TableSetBgColor(ImGuiTableBgTarget target, uint color, int column_n);
    ///<summary>
    /// Implied count = 1, id = NULL, borders = true<br/>
    ///<br/>
    /// Legacy Columns API (prefer using Tables!)<br/>
    /// - You can also use SameLine(pos_x) to mimic simplified columns.<br/>
    ///</summary>
    void Columns();
    void ColumnsEx(int count, string id, bool borders);
    ///<summary>
    /// next column, defaults to current row or next row if the current row is finished<br/>
    ///</summary>
    void NextColumn();
    ///<summary>
    /// get current column index<br/>
    ///</summary>
    int GetColumnIndex();
    ///<summary>
    /// get column width (in pixels). pass -1 to use current column<br/>
    ///</summary>
    float GetColumnWidth(int column_index);
    ///<summary>
    /// set column width (in pixels). pass -1 to use current column<br/>
    ///</summary>
    void SetColumnWidth(int column_index, float width);
    ///<summary>
    /// get position of column line (in pixels, from the left side of the contents region). pass -1 to use current column, otherwise 0..GetColumnsCount() inclusive. column 0 is typically 0.0f<br/>
    ///</summary>
    float GetColumnOffset(int column_index);
    ///<summary>
    /// set position of column line (in pixels, from the left side of the contents region). pass -1 to use current column<br/>
    ///</summary>
    void SetColumnOffset(int column_index, float offset_x);
    int GetColumnsCount();
    ///<summary>
    /// create and append into a TabBar<br/>
    ///<br/>
    /// Tab Bars, Tabs<br/>
    /// - Note: Tabs are automatically created by the docking system (when in 'docking' branch). Use this to create tab bars/tabs yourself.<br/>
    ///</summary>
    bool BeginTabBar(string str_id, ImGuiTabBarFlags flags);
    ///<summary>
    /// only call EndTabBar() if BeginTabBar() returns true!<br/>
    ///</summary>
    void EndTabBar();
    ///<summary>
    /// create a Tab. Returns true if the Tab is selected.<br/>
    ///</summary>
    bool BeginTabItem(string label, ref bool p_open, ImGuiTabItemFlags flags);
    ///<summary>
    /// only call EndTabItem() if BeginTabItem() returns true!<br/>
    ///</summary>
    void EndTabItem();
    ///<summary>
    /// create a Tab behaving like a button. return true when clicked. cannot be selected in the tab bar.<br/>
    ///</summary>
    bool TabItemButton(string label, ImGuiTabItemFlags flags);
    ///<summary>
    /// notify TabBar or Docking system of a closed tab/window ahead (useful to reduce visual flicker on reorderable tab bars). For tab-bar: call after BeginTabBar() and before Tab submissions. Otherwise call with a window name.<br/>
    ///</summary>
    void SetTabItemClosed(string tab_or_docked_window_label);
    ///<summary>
    /// Implied size = ImVec2(0, 0), flags = 0, window_class = NULL<br/>
    ///<br/>
    /// Docking<br/>
    /// - Read https:github.com/ocornut/imgui/wiki/Docking for details.<br/>
    /// - Enable with io.ConfigFlags |= ImGuiConfigFlags_DockingEnable.<br/>
    /// - You can use most Docking facilities without calling any API. You don't necessarily need to call a DockSpaceXXX function to use Docking!<br/>
    ///   - Drag from window title bar or their tab to dock/undock. Hold SHIFT to disable docking.<br/>
    ///   - Drag from window menu button (upper-left button) to undock an entire node (all windows).<br/>
    ///   - When io.ConfigDockingWithShift == true, you instead need to hold SHIFT to enable docking.<br/>
    /// - Dockspaces:<br/>
    ///   - If you want to dock windows into the edge of your screen, most application can simply call DockSpaceOverViewport():<br/>
    ///     e.g. ImGui::NewFrame(); then ImGui::DockSpaceOverViewport();   Create a dockspace in main viewport.<br/>
    ///      or: ImGui::NewFrame(); then ImGui::DockSpaceOverViewport(0, nullptr, ImGuiDockNodeFlags_PassthruCentralNode);   Create a dockspace in main viewport, where central node is transparent.<br/>
    ///   - A dockspace is an explicit dock node within an existing window.<br/>
    ///   - DockSpaceOverViewport() basically creates an invisible window covering a viewport, and submit a DockSpace() into it.<br/>
    ///   - IMPORTANT: Dockspaces need to be submitted _before_ any window they can host. Submit them early in your frame!<br/>
    ///   - IMPORTANT: Dockspaces need to be kept alive if hidden, otherwise windows docked into it will be undocked.<br/>
    ///     If you have e.g. multiple tabs with a dockspace inside each tab: submit the non-visible dockspaces with ImGuiDockNodeFlags_KeepAliveOnly.<br/>
    /// - Programmatic docking:<br/>
    ///   - There is no public API yet other than the very limited SetNextWindowDockID() function. Sorry for that!<br/>
    ///   - Read https:github.com/ocornut/imgui/wiki/Docking for examples of how to use current internal API.<br/>
    ///</summary>
    uint DockSpace(uint dockspace_id);
    uint DockSpaceEx(uint dockspace_id, Vector2 size, ImGuiDockNodeFlags flags, IImGuiWindowClass window_class);
    ///<summary>
    /// Implied dockspace_id = 0, viewport = NULL, flags = 0, window_class = NULL<br/>
    ///</summary>
    uint DockSpaceOverViewport();
    uint DockSpaceOverViewportEx(uint dockspace_id, IImGuiViewport viewport, ImGuiDockNodeFlags flags, IImGuiWindowClass window_class);
    ///<summary>
    /// set next window dock id<br/>
    ///</summary>
    void SetNextWindowDockID(uint dock_id, ImGuiCond cond);
    ///<summary>
    /// set next window class (control docking compatibility + provide hints to platform backend via custom viewport flags and platform parent/child relationship)<br/>
    ///</summary>
    void SetNextWindowClass(IImGuiWindowClass window_class);
    uint GetWindowDockID();
    ///<summary>
    /// is current window docked into another window?<br/>
    ///</summary>
    bool IsWindowDocked();
    ///<summary>
    /// start logging to tty (stdout)<br/>
    ///<br/>
    /// Logging/Capture<br/>
    /// - All text output from the interface can be captured into tty/file/clipboard. By default, tree nodes are automatically opened during logging.<br/>
    ///</summary>
    void LogToTTY(int auto_open_depth);
    ///<summary>
    /// start logging to file<br/>
    ///</summary>
    void LogToFile(int auto_open_depth, string filename);
    ///<summary>
    /// start logging to OS clipboard<br/>
    ///</summary>
    void LogToClipboard(int auto_open_depth);
    ///<summary>
    /// stop logging (close file, etc.)<br/>
    ///</summary>
    void LogFinish();
    ///<summary>
    /// helper to display buttons for logging to tty/file/clipboard<br/>
    ///</summary>
    void LogButtons();
    ///<summary>
    /// pass text data straight to log (without being displayed)<br/>
    ///</summary>
    void LogText(string fmt);
    void LogTextV(string fmt, sbyte* args);
    ///<summary>
    /// call after submitting an item which may be dragged. when this return true, you can call SetDragDropPayload() + EndDragDropSource()<br/>
    ///<br/>
    /// Drag and Drop<br/>
    /// - On source items, call BeginDragDropSource(), if it returns true also call SetDragDropPayload() + EndDragDropSource().<br/>
    /// - On target candidates, call BeginDragDropTarget(), if it returns true also call AcceptDragDropPayload() + EndDragDropTarget().<br/>
    /// - If you stop calling BeginDragDropSource() the payload is preserved however it won't have a preview tooltip (we currently display a fallback "..." tooltip, see #1725)<br/>
    /// - An item can be both drag source and drop target.<br/>
    ///</summary>
    bool BeginDragDropSource(ImGuiDragDropFlags flags);
    ///<summary>
    /// type is a user defined string of maximum 32 characters. Strings starting with '_' are reserved for dear imgui internal types. Data is copied and held by imgui. Return true when payload has been accepted.<br/>
    ///</summary>
    bool SetDragDropPayload(string type, void* data, nuint sz, ImGuiCond cond);
    ///<summary>
    /// only call EndDragDropSource() if BeginDragDropSource() returns true!<br/>
    ///</summary>
    void EndDragDropSource();
    ///<summary>
    /// call after submitting an item that may receive a payload. If this returns true, you can call AcceptDragDropPayload() + EndDragDropTarget()<br/>
    ///</summary>
    bool BeginDragDropTarget();
    ///<summary>
    /// accept contents of a given type. If ImGuiDragDropFlags_AcceptBeforeDelivery is set you can peek into the payload before the mouse button is released.<br/>
    ///</summary>
    IImGuiPayload AcceptDragDropPayload(string type, ImGuiDragDropFlags flags);
    ///<summary>
    /// only call EndDragDropTarget() if BeginDragDropTarget() returns true!<br/>
    ///</summary>
    void EndDragDropTarget();
    ///<summary>
    /// peek directly into the current payload from anywhere. returns NULL when drag and drop is finished or inactive. use ImGuiPayload::IsDataType() to test for the payload type.<br/>
    ///</summary>
    IImGuiPayload GetDragDropPayload();
    ///<summary>
    /// Disabling [BETA API]<br/>
    /// - Disable all user interactions and dim items visuals (applying style.DisabledAlpha over current colors)<br/>
    /// - Those can be nested but it cannot be used to enable an already disabled section (a single BeginDisabled(true) in the stack is enough to keep everything disabled)<br/>
    /// - Tooltips windows are automatically opted out of disabling. Note that IsItemHovered() by default returns false on disabled items, unless using ImGuiHoveredFlags_AllowWhenDisabled. <br/>
    /// - BeginDisabled(false)/EndDisabled() essentially does nothing but is provided to facilitate use of boolean expressions (as a micro-optimization: if you have tens of thousands of BeginDisabled(false)/EndDisabled() pairs, you might want to reformulate your code to avoid making those calls)<br/>
    ///</summary>
    void BeginDisabled(bool disabled);
    void EndDisabled();
    ///<summary>
    /// Clipping<br/>
    /// - Mouse hovering is affected by ImGui::PushClipRect() calls, unlike direct calls to ImDrawList::PushClipRect() which are render only.<br/>
    ///</summary>
    void PushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, bool intersect_with_current_clip_rect);
    void PopClipRect();
    ///<summary>
    /// make last item the default focused item of a newly appearing window.<br/>
    ///<br/>
    /// Focus, Activation<br/>
    ///</summary>
    void SetItemDefaultFocus();
    ///<summary>
    /// Implied offset = 0<br/>
    ///</summary>
    void SetKeyboardFocusHere();
    ///<summary>
    /// focus keyboard on the next widget. Use positive 'offset' to access sub components of a multiple component widget. Use -1 to access previous widget.<br/>
    ///</summary>
    void SetKeyboardFocusHereEx(int offset);
    ///<summary>
    /// alter visibility of keyboard/gamepad cursor. by default: show when using an arrow key, hide when clicking with mouse.<br/>
    ///<br/>
    /// Keyboard/Gamepad Navigation<br/>
    ///</summary>
    void SetNavCursorVisible(bool visible);
    ///<summary>
    /// allow next item to be overlapped by a subsequent item. Useful with invisible buttons, selectable, treenode covering an area where subsequent items may need to be added. Note that both Selectable() and TreeNode() have dedicated flags doing this.<br/>
    ///<br/>
    /// Overlapping mode<br/>
    ///</summary>
    void SetNextItemAllowOverlap();
    ///<summary>
    /// is the last item hovered? (and usable, aka not blocked by a popup, etc.). See ImGuiHoveredFlags for more options.<br/>
    ///<br/>
    /// Item/Widgets Utilities and Query Functions<br/>
    /// - Most of the functions are referring to the previous Item that has been submitted.<br/>
    /// - See Demo Window under "Widgets-&gt;Querying Status" for an interactive visualization of most of those functions.<br/>
    ///</summary>
    bool IsItemHovered(ImGuiHoveredFlags flags);
    ///<summary>
    /// is the last item active? (e.g. button being held, text field being edited. This will continuously return true while holding mouse button on an item. Items that don't interact will always return false)<br/>
    ///</summary>
    bool IsItemActive();
    ///<summary>
    /// is the last item focused for keyboard/gamepad navigation?<br/>
    ///</summary>
    bool IsItemFocused();
    ///<summary>
    /// Implied mouse_button = 0<br/>
    ///</summary>
    bool IsItemClicked();
    ///<summary>
    /// is the last item hovered and mouse clicked on? (**)  == IsMouseClicked(mouse_button) &amp;&amp; IsItemHovered()Important. (**) this is NOT equivalent to the behavior of e.g. Button(). Read comments in function definition.<br/>
    ///</summary>
    bool IsItemClickedEx(ImGuiMouseButton mouse_button);
    ///<summary>
    /// is the last item visible? (items may be out of sight because of clipping/scrolling)<br/>
    ///</summary>
    bool IsItemVisible();
    ///<summary>
    /// did the last item modify its underlying value this frame? or was pressed? This is generally the same as the "bool" return value of many widgets.<br/>
    ///</summary>
    bool IsItemEdited();
    ///<summary>
    /// was the last item just made active (item was previously inactive).<br/>
    ///</summary>
    bool IsItemActivated();
    ///<summary>
    /// was the last item just made inactive (item was previously active). Useful for Undo/Redo patterns with widgets that require continuous editing.<br/>
    ///</summary>
    bool IsItemDeactivated();
    ///<summary>
    /// was the last item just made inactive and made a value change when it was active? (e.g. Slider/Drag moved). Useful for Undo/Redo patterns with widgets that require continuous editing. Note that you may get false positives (some widgets such as Combo()/ListBox()/Selectable() will return true even when clicking an already selected item).<br/>
    ///</summary>
    bool IsItemDeactivatedAfterEdit();
    ///<summary>
    /// was the last item open state toggled? set by TreeNode().<br/>
    ///</summary>
    bool IsItemToggledOpen();
    ///<summary>
    /// is any item hovered?<br/>
    ///</summary>
    bool IsAnyItemHovered();
    ///<summary>
    /// is any item active?<br/>
    ///</summary>
    bool IsAnyItemActive();
    ///<summary>
    /// is any item focused?<br/>
    ///</summary>
    bool IsAnyItemFocused();
    ///<summary>
    /// get ID of last item (~~ often same ImGui::GetID(label) beforehand)<br/>
    ///</summary>
    uint GetItemID();
    ///<summary>
    /// get upper-left bounding rectangle of the last item (screen space)<br/>
    ///</summary>
    Vector2 GetItemRectMin();
    ///<summary>
    /// get lower-right bounding rectangle of the last item (screen space)<br/>
    ///</summary>
    Vector2 GetItemRectMax();
    ///<summary>
    /// get size of last item<br/>
    ///</summary>
    Vector2 GetItemRectSize();
    ///<summary>
    /// return primary/default viewport. This can never be NULL.<br/>
    ///<br/>
    /// Viewports<br/>
    /// - Currently represents the Platform Window created by the application which is hosting our Dear ImGui windows.<br/>
    /// - In 'docking' branch with multi-viewport enabled, we extend this concept to have multiple active viewports.<br/>
    /// - In the future we will extend this concept further to also represent Platform Monitor and support a "no main platform window" operation mode.<br/>
    ///</summary>
    IImGuiViewport GetMainViewport();
    ///<summary>
    /// Implied viewport = NULL<br/>
    ///<br/>
    /// Background/Foreground Draw Lists<br/>
    ///</summary>
    IImDrawList GetBackgroundDrawList();
    ///<summary>
    /// get background draw list for the given viewport or viewport associated to the current window. this draw list will be the first rendering one. Useful to quickly draw shapes/text behind dear imgui contents.<br/>
    ///</summary>
    IImDrawList GetBackgroundDrawListEx(IImGuiViewport viewport);
    ///<summary>
    /// Implied viewport = NULL<br/>
    ///</summary>
    IImDrawList GetForegroundDrawList();
    ///<summary>
    /// get foreground draw list for the given viewport or viewport associated to the current window. this draw list will be the top-most rendered one. Useful to quickly draw shapes/text over dear imgui contents.<br/>
    ///</summary>
    IImDrawList GetForegroundDrawListEx(IImGuiViewport viewport);
    ///<summary>
    /// test if rectangle (of given size, starting from cursor position) is visible / not clipped.<br/>
    ///<br/>
    /// Miscellaneous Utilities<br/>
    ///</summary>
    bool IsRectVisibleBySize(Vector2 size);
    ///<summary>
    /// test if rectangle (in screen space) is visible / not clipped. to perform coarse clipping on user's side.<br/>
    ///</summary>
    bool IsRectVisible(Vector2 rect_min, Vector2 rect_max);
    ///<summary>
    /// get global imgui time. incremented by io.DeltaTime every frame.<br/>
    ///</summary>
    double GetTime();
    ///<summary>
    /// get global imgui frame count. incremented by 1 every frame.<br/>
    ///</summary>
    int GetFrameCount();
    ///<summary>
    /// you may use this when creating your own ImDrawList instances.<br/>
    ///</summary>
    nint GetDrawListSharedData();
    ///<summary>
    /// get a string corresponding to the enum value (for display, saving, etc.).<br/>
    ///</summary>
    string GetStyleColorName(ImGuiCol idx);
    ///<summary>
    /// replace current window storage with our own (if you want to manipulate it yourself, typically clear subsection of it)<br/>
    ///</summary>
    void SetStateStorage(IImGuiStorage storage);
    IImGuiStorage GetStateStorage();
    ///<summary>
    /// Implied text_end = NULL, hide_text_after_double_hash = false, wrap_width = -1.0f<br/>
    ///<br/>
    /// Text Utilities<br/>
    ///</summary>
    Vector2 CalcTextSize(string text);
    Vector2 CalcTextSizeEx(string text, string text_end, bool hide_text_after_double_hash, float wrap_width);
    ///<summary>
    /// Color Utilities<br/>
    ///</summary>
    Vector4 ColorConvertU32ToFloat4(uint @in);
    uint ColorConvertFloat4ToU32(Vector4 @in);
    void ColorConvertRGBtoHSV(float r, float g, float b, ref float out_h, ref float out_s, ref float out_v);
    void ColorConvertHSVtoRGB(float h, float s, float v, ref float out_r, ref float out_g, ref float out_b);
    ///<summary>
    /// is key being held.<br/>
    ///<br/>
    /// Inputs Utilities: Keyboard/Mouse/Gamepad<br/>
    /// - the ImGuiKey enum contains all possible keyboard, mouse and gamepad inputs (e.g. ImGuiKey_A, ImGuiKey_MouseLeft, ImGuiKey_GamepadDpadUp...).<br/>
    /// - (legacy: before v1.87, we used ImGuiKey to carry native/user indices as defined by each backends. This was obsoleted in 1.87 (2022-02) and completely removed in 1.91.5 (2024-11). See https:github.com/ocornut/imgui/issues/4921)<br/>
    /// - (legacy: any use of ImGuiKey will assert when key &lt; 512 to detect passing legacy native/user indices)<br/>
    ///</summary>
    bool IsKeyDown(int key);
    ///<summary>
    /// Implied repeat = true<br/>
    ///</summary>
    bool IsKeyPressed(int key);
    ///<summary>
    /// was key pressed (went from !Down to Down)? if repeat=true, uses io.KeyRepeatDelay / KeyRepeatRate<br/>
    ///</summary>
    bool IsKeyPressedEx(int key, bool repeat);
    ///<summary>
    /// was key released (went from Down to !Down)?<br/>
    ///</summary>
    bool IsKeyReleased(int key);
    ///<summary>
    /// was key chord (mods + key) pressed, e.g. you can pass 'ImGuiMod_Ctrl | ImGuiKey_S' as a key-chord. This doesn't do any routing or focus check, please consider using Shortcut() function instead.<br/>
    ///</summary>
    bool IsKeyChordPressed(int key_chord);
    ///<summary>
    /// uses provided repeat rate/delay. return a count, most often 0 or 1 but might be &gt;1 if RepeatRate is small enough that DeltaTime &gt; RepeatRate<br/>
    ///</summary>
    int GetKeyPressedAmount(int key, float repeat_delay, float rate);
    ///<summary>
    /// [DEBUG] returns English name of the key. Those names are provided for debugging purpose and are not meant to be saved persistently nor compared.<br/>
    ///</summary>
    string GetKeyName(int key);
    ///<summary>
    /// Override io.WantCaptureKeyboard flag next frame (said flag is left for your application to handle, typically when true it instructs your app to ignore inputs). e.g. force capture keyboard when your widget is being hovered. This is equivalent to setting "io.WantCaptureKeyboard = want_capture_keyboard"; after the next NewFrame() call.<br/>
    ///</summary>
    void SetNextFrameWantCaptureKeyboard(bool want_capture_keyboard);
    ///<summary>
    /// Inputs Utilities: Shortcut Testing &amp; Routing [BETA]<br/>
    /// - ImGuiKeyChord = a ImGuiKey + optional ImGuiMod_Alt/ImGuiMod_Ctrl/ImGuiMod_Shift/ImGuiMod_Super.<br/>
    ///       ImGuiKey_C                           Accepted by functions taking ImGuiKey or ImGuiKeyChord arguments<br/>
    ///       ImGuiMod_Ctrl | ImGuiKey_C           Accepted by functions taking ImGuiKeyChord arguments<br/>
    ///   only ImGuiMod_XXX values are legal to combine with an ImGuiKey. You CANNOT combine two ImGuiKey values.<br/>
    /// - The general idea is that several callers may register interest in a shortcut, and only one owner gets it.<br/>
    ///      Parent   -&gt; call Shortcut(Ctrl+S)     When Parent is focused, Parent gets the shortcut.<br/>
    ///        Child1 -&gt; call Shortcut(Ctrl+S)     When Child1 is focused, Child1 gets the shortcut (Child1 overrides Parent shortcuts)<br/>
    ///        Child2 -&gt; no call                   When Child2 is focused, Parent gets the shortcut.<br/>
    ///   The whole system is order independent, so if Child1 makes its calls before Parent, results will be identical.<br/>
    ///   This is an important property as it facilitate working with foreign code or larger codebase.<br/>
    /// - To understand the difference:<br/>
    ///   - IsKeyChordPressed() compares mods and call IsKeyPressed() -&gt; function has no side-effect.<br/>
    ///   - Shortcut() submits a route, routes are resolved, if it currently can be routed it calls IsKeyChordPressed() -&gt; function has (desirable) side-effects as it can prevents another call from getting the route.<br/>
    /// - Visualize registered routes in 'Metrics/Debugger-&gt;Inputs'.<br/>
    ///</summary>
    bool Shortcut(int key_chord, ImGuiInputFlags flags);
    void SetNextItemShortcut(int key_chord, ImGuiInputFlags flags);
    ///<summary>
    /// Set key owner to last item ID if it is hovered or active. Equivalent to 'if (IsItemHovered() || IsItemActive()) { SetKeyOwner(key, GetItemID());'.<br/>
    ///<br/>
    /// Inputs Utilities: Key/Input Ownership [BETA]<br/>
    /// - One common use case would be to allow your items to disable standard inputs behaviors such<br/>
    ///   as Tab or Alt key handling, Mouse Wheel scrolling, etc.<br/>
    ///   e.g. Button(...); SetItemKeyOwner(ImGuiKey_MouseWheelY); to make hovering/activating a button disable wheel for scrolling.<br/>
    /// - Reminder ImGuiKey enum include access to mouse buttons and gamepad, so key ownership can apply to them.<br/>
    /// - Many related features are still in imgui_internal.h. For instance, most IsKeyXXX()/IsMouseXXX() functions have an owner-id-aware version.<br/>
    ///</summary>
    void SetItemKeyOwner(int key);
    ///<summary>
    /// is mouse button held?<br/>
    ///<br/>
    /// Inputs Utilities: Mouse<br/>
    /// - To refer to a mouse button, you may use named enums in your code e.g. ImGuiMouseButton_Left, ImGuiMouseButton_Right.<br/>
    /// - You can also use regular integer: it is forever guaranteed that 0=Left, 1=Right, 2=Middle.<br/>
    /// - Dragging operations are only reported after mouse has moved a certain distance away from the initial clicking position (see 'lock_threshold' and 'io.MouseDraggingThreshold')<br/>
    ///</summary>
    bool IsMouseDown(ImGuiMouseButton button);
    ///<summary>
    /// Implied repeat = false<br/>
    ///</summary>
    bool IsMouseClicked(ImGuiMouseButton button);
    ///<summary>
    /// did mouse button clicked? (went from !Down to Down). Same as GetMouseClickedCount() == 1.<br/>
    ///</summary>
    bool IsMouseClickedEx(ImGuiMouseButton button, bool repeat);
    ///<summary>
    /// did mouse button released? (went from Down to !Down)<br/>
    ///</summary>
    bool IsMouseReleased(ImGuiMouseButton button);
    ///<summary>
    /// did mouse button double-clicked? Same as GetMouseClickedCount() == 2. (note that a double-click will also report IsMouseClicked() == true)<br/>
    ///</summary>
    bool IsMouseDoubleClicked(ImGuiMouseButton button);
    ///<summary>
    /// delayed mouse release (use very sparingly!). Generally used with 'delay &gt;= io.MouseDoubleClickTime' + combined with a 'io.MouseClickedLastCount==1' test. This is a very rarely used UI idiom, but some apps use this: e.g. MS Explorer single click on an icon to rename.<br/>
    ///</summary>
    bool IsMouseReleasedWithDelay(ImGuiMouseButton button, float delay);
    ///<summary>
    /// return the number of successive mouse-clicks at the time where a click happen (otherwise 0).<br/>
    ///</summary>
    int GetMouseClickedCount(ImGuiMouseButton button);
    ///<summary>
    /// Implied clip = true<br/>
    ///</summary>
    bool IsMouseHoveringRect(Vector2 r_min, Vector2 r_max);
    ///<summary>
    /// is mouse hovering given bounding rect (in screen space). clipped by current clipping settings, but disregarding of other consideration of focus/window ordering/popup-block.<br/>
    ///</summary>
    bool IsMouseHoveringRectEx(Vector2 r_min, Vector2 r_max, bool clip);
    ///<summary>
    /// by convention we use (-FLT_MAX,-FLT_MAX) to denote that there is no mouse available<br/>
    ///</summary>
    bool IsMousePosValid(Vector2 mouse_pos);
    ///<summary>
    /// [WILL OBSOLETE] is any mouse button held? This was designed for backends, but prefer having backend maintain a mask of held mouse buttons, because upcoming input queue system will make this invalid.<br/>
    ///</summary>
    bool IsAnyMouseDown();
    ///<summary>
    /// shortcut to ImGui::GetIO().MousePos provided by user, to be consistent with other calls<br/>
    ///</summary>
    Vector2 GetMousePos();
    ///<summary>
    /// retrieve mouse position at the time of opening popup we have BeginPopup() into (helper to avoid user backing that value themselves)<br/>
    ///</summary>
    Vector2 GetMousePosOnOpeningCurrentPopup();
    ///<summary>
    /// is mouse dragging? (uses io.MouseDraggingThreshold if lock_threshold &lt; 0.0f)<br/>
    ///</summary>
    bool IsMouseDragging(ImGuiMouseButton button, float lock_threshold);
    ///<summary>
    /// return the delta from the initial clicking position while the mouse button is pressed or was just released. This is locked and return 0.0f until the mouse moves past a distance threshold at least once (uses io.MouseDraggingThreshold if lock_threshold &lt; 0.0f)<br/>
    ///</summary>
    Vector2 GetMouseDragDelta(ImGuiMouseButton button, float lock_threshold);
    ///<summary>
    /// Implied button = 0<br/>
    ///</summary>
    void ResetMouseDragDelta();
    ///<summary>
    ///<br/>
    ///</summary>
    void ResetMouseDragDeltaEx(ImGuiMouseButton button);
    ///<summary>
    /// get desired mouse cursor shape. Important: reset in ImGui::NewFrame(), this is updated during the frame. valid before Render(). If you use software rendering by setting io.MouseDrawCursor ImGui will render those for you<br/>
    ///</summary>
    ImGuiMouseCursor GetMouseCursor();
    ///<summary>
    /// set desired mouse cursor shape<br/>
    ///</summary>
    void SetMouseCursor(ImGuiMouseCursor cursor_type);
    ///<summary>
    /// Override io.WantCaptureMouse flag next frame (said flag is left for your application to handle, typical when true it instructs your app to ignore inputs). This is equivalent to setting "io.WantCaptureMouse = want_capture_mouse;" after the next NewFrame() call.<br/>
    ///</summary>
    void SetNextFrameWantCaptureMouse(bool want_capture_mouse);
    ///<summary>
    /// Clipboard Utilities<br/>
    /// - Also see the LogToClipboard() function to capture GUI into clipboard, or easily output text data to the clipboard.<br/>
    ///</summary>
    string GetClipboardText();
    void SetClipboardText(string text);
    ///<summary>
    /// call after CreateContext() and before the first call to NewFrame(). NewFrame() automatically calls LoadIniSettingsFromDisk(io.IniFilename).<br/>
    ///<br/>
    /// Settings/.Ini Utilities<br/>
    /// - The disk functions are automatically called if io.IniFilename != NULL (default is "imgui.ini").<br/>
    /// - Set io.IniFilename to NULL to load/save manually. Read io.WantSaveIniSettings description about handling .ini saving manually.<br/>
    /// - Important: default value "imgui.ini" is relative to current working dir! Most apps will want to lock this to an absolute path (e.g. same path as executables).<br/>
    ///</summary>
    void LoadIniSettingsFromDisk(string ini_filename);
    ///<summary>
    /// call after CreateContext() and before the first call to NewFrame() to provide .ini data from your own data source.<br/>
    ///</summary>
    void LoadIniSettingsFromMemory(string ini_data, nuint ini_size);
    ///<summary>
    /// this is automatically called (if io.IniFilename is not empty) a few seconds after any modification that should be reflected in the .ini file (and also by DestroyContext).<br/>
    ///</summary>
    void SaveIniSettingsToDisk(string ini_filename);
    ///<summary>
    /// return a zero-terminated string with the .ini data which you can save by your own mean. call when io.WantSaveIniSettings is set, then save data by your own mean and clear io.WantSaveIniSettings.<br/>
    ///</summary>
    string SaveIniSettingsToMemory(nuint out_ini_size);
    ///<summary>
    /// Debug Utilities<br/>
    /// - Your main debugging friend is the ShowMetricsWindow() function, which is also accessible from Demo-&gt;Tools-&gt;Metrics Debugger<br/>
    ///</summary>
    void DebugTextEncoding(string text);
    void DebugFlashStyleColor(ImGuiCol idx);
    void DebugStartItemPicker();
    ///<summary>
    /// This is called by IMGUI_CHECKVERSION() macro.<br/>
    ///</summary>
    bool DebugCheckVersionAndDataLayout(string version_str, nuint sz_io, nuint sz_style, nuint sz_vec2, nuint sz_vec4, nuint sz_drawvert, nuint sz_drawidx);
    ///<summary>
    /// Call via IMGUI_DEBUG_LOG() for maximum stripping in caller code!<br/>
    ///</summary>
    void DebugLog(string fmt);
    void DebugLogV(string fmt, sbyte* args);
    ///<summary>
    /// Memory Allocators<br/>
    /// - Those functions are not reliant on the current context.<br/>
    /// - DLL users: heaps and globals are not shared across DLL boundaries! You will need to call SetCurrentContext() + SetAllocatorFunctions()<br/>
    ///   for each static/DLL boundary you are calling from. Read "Context and Memory Allocators" section of imgui.cpp for more details.<br/>
    ///</summary>
    void SetAllocatorFunctions(delegate* unmanaged[Cdecl]<nuint, nint, nint> alloc_func, delegate* unmanaged[Cdecl]<nint, nint, void> free_func, void* user_data);
    void GetAllocatorFunctions(delegate* unmanaged[Cdecl]<nuint, void*, void*> * p_alloc_func, delegate* unmanaged[Cdecl]<void*, void*, void> * p_free_func, void** p_user_data);
    void* MemAlloc(nuint size);
    void MemFree(void* ptr);
    ///<summary>
    /// call in main loop. will call CreateWindow/ResizeWindow/etc. platform functions for each secondary viewport, and DestroyWindow for each inactive viewport.<br/>
    ///<br/>
    /// (Optional) Platform/OS interface for multi-viewport support<br/>
    /// Read comments around the ImGuiPlatformIO structure for more details.<br/>
    /// Note: You may use GetWindowViewport() to get the current viewport of the current window.<br/>
    ///</summary>
    void UpdatePlatformWindows();
    ///<summary>
    /// Implied platform_render_arg = NULL, renderer_render_arg = NULL<br/>
    ///</summary>
    void RenderPlatformWindowsDefault();
    ///<summary>
    /// call in main loop. will call RenderWindow/SwapBuffers platform functions for each secondary viewport which doesn't have the ImGuiViewportFlags_Minimized flag set. May be reimplemented by user for custom rendering needs.<br/>
    ///</summary>
    void RenderPlatformWindowsDefaultEx(void* platform_render_arg, void* renderer_render_arg);
    ///<summary>
    /// call DestroyWindow platform functions for all viewports. call from backend Shutdown() if you need to close platform windows before imgui shutdown. otherwise will be called by DestroyContext().<br/>
    ///</summary>
    void DestroyPlatformWindows();
    ///<summary>
    /// this is a helper for backends.<br/>
    ///</summary>
    IImGuiViewport FindViewportByID(uint viewport_id);
    ///<summary>
    /// this is a helper for backends. the type platform_handle is decided by the backend (e.g. HWND, MyWindow*, GLFWwindow* etc.)<br/>
    ///</summary>
    IImGuiViewport FindViewportByPlatformHandle(void* platform_handle);
    ///<summary>
    /// Construct a zero-size ImVector&lt;&gt; (of any type). This is primarily useful when calling ImFontGlyphRangesBuilder_BuildRanges()<br/>
    ///</summary>
    void ImVector_Construct(void* vector);
    ///<summary>
    /// Destruct an ImVector&lt;&gt; (of any type). Important: Frees the vector memory but does not call destructors on contained objects (if they have them)<br/>
    ///</summary>
    void ImVector_Destruct(void* vector);
    ///<summary>
    /// Set ImGuiPlatformIO::Platform_GetWindowWorkAreaInsets in a C-compatible mannner<br/>
    ///</summary>
    void ImGuiPlatformIO_SetPlatform_GetWindowWorkAreaInsets(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowWorkAreaInsetsFunc);
    ///<summary>
    /// Set ImGuiPlatformIO::Platform_GetWindowFramebufferScale in a C-compatible mannner<br/>
    ///</summary>
    void ImGuiPlatformIO_SetPlatform_GetWindowFramebufferScale(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowFramebufferScaleFunc);
    ///<summary>
    /// Set ImGuiPlatformIO::Platform_GetWindowPos in a C-compatible mannner<br/>
    ///</summary>
    void ImGuiPlatformIO_SetPlatform_GetWindowPos(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowPosFunc);
    ///<summary>
    /// Set ImGuiPlatformIO::Platform_GetWindowSize in a C-compatible mannner<br/>
    ///</summary>
    void ImGuiPlatformIO_SetPlatform_GetWindowSize(delegate* unmanaged[Cdecl]<nint, nint, void> getWindowSizeFunc);
    ///<summary>
    /// Scale all spacing/padding/thickness values. Do not scale fonts.<br/>
    ///</summary>
    void ImGuiStyle_ScaleAllSizes(IImGuiStyle self, float scale_factor);
    ///<summary>
    /// Queue a new key down/up event. Key should be "translated" (as in, generally ImGuiKey_A matches the key end-user would use to emit an 'A' character)<br/>
    ///<br/>
    /// Input Functions<br/>
    ///</summary>
    void ImGuiIO_AddKeyEvent(IImGuiIO self, int key, bool down);
    ///<summary>
    /// Queue a new key down/up event for analog values (e.g. ImGuiKey_Gamepad_ values). Dead-zones should be handled by the backend.<br/>
    ///</summary>
    void ImGuiIO_AddKeyAnalogEvent(IImGuiIO self, int key, bool down, float v);
    ///<summary>
    /// Queue a mouse position update. Use -FLT_MAX,-FLT_MAX to signify no mouse (e.g. app not focused and not hovered)<br/>
    ///</summary>
    void ImGuiIO_AddMousePosEvent(IImGuiIO self, float x, float y);
    ///<summary>
    /// Queue a mouse button change<br/>
    ///</summary>
    void ImGuiIO_AddMouseButtonEvent(IImGuiIO self, int button, bool down);
    ///<summary>
    /// Queue a mouse wheel update. wheel_y&lt;0: scroll down, wheel_y&gt;0: scroll up, wheel_x&lt;0: scroll right, wheel_x&gt;0: scroll left.<br/>
    ///</summary>
    void ImGuiIO_AddMouseWheelEvent(IImGuiIO self, float wheel_x, float wheel_y);
    ///<summary>
    /// Queue a mouse source change (Mouse/TouchScreen/Pen)<br/>
    ///</summary>
    void ImGuiIO_AddMouseSourceEvent(IImGuiIO self, int source);
    ///<summary>
    /// Queue a mouse hovered viewport. Requires backend to set ImGuiBackendFlags_HasMouseHoveredViewport to call this (for multi-viewport support).<br/>
    ///</summary>
    void ImGuiIO_AddMouseViewportEvent(IImGuiIO self, uint id);
    ///<summary>
    /// Queue a gain/loss of focus for the application (generally based on OS/platform focus of your window)<br/>
    ///</summary>
    void ImGuiIO_AddFocusEvent(IImGuiIO self, bool focused);
    ///<summary>
    /// Queue a new character input<br/>
    ///</summary>
    void ImGuiIO_AddInputCharacter(IImGuiIO self, uint c);
    ///<summary>
    /// Queue a new character input from a UTF-16 character, it can be a surrogate<br/>
    ///</summary>
    void ImGuiIO_AddInputCharacterUTF16(IImGuiIO self, ushort c);
    ///<summary>
    /// Queue a new characters input from a UTF-8 string<br/>
    ///</summary>
    void ImGuiIO_AddInputCharactersUTF8(IImGuiIO self, string str);
    ///<summary>
    /// Implied native_legacy_index = -1<br/>
    ///</summary>
    void ImGuiIO_SetKeyEventNativeData(IImGuiIO self, int key, int native_keycode, int native_scancode);
    ///<summary>
    /// [Optional] Specify index for legacy &lt;1.87 IsKeyXXX() functions with native indices + specify native keycode, scancode.<br/>
    ///</summary>
    void ImGuiIO_SetKeyEventNativeDataEx(IImGuiIO self, int key, int native_keycode, int native_scancode, int native_legacy_index);
    ///<summary>
    /// Set master flag for accepting key/mouse/text events (default to true). Useful if you have native dialog boxes that are interrupting your application loop/refresh, and you want to disable events being queued while your app is frozen.<br/>
    ///</summary>
    void ImGuiIO_SetAppAcceptingEvents(IImGuiIO self, bool accepting_events);
    ///<summary>
    /// Clear all incoming events.<br/>
    ///</summary>
    void ImGuiIO_ClearEventsQueue(IImGuiIO self);
    ///<summary>
    /// Clear current keyboard/gamepad state + current frame text input buffer. Equivalent to releasing all keys/buttons.<br/>
    ///</summary>
    void ImGuiIO_ClearInputKeys(IImGuiIO self);
    ///<summary>
    /// Clear current mouse state.<br/>
    ///</summary>
    void ImGuiIO_ClearInputMouse(IImGuiIO self);
    void ImGuiInputTextCallbackData_DeleteChars(IImGuiInputTextCallbackData self, int pos, int bytes_count);
    void ImGuiInputTextCallbackData_InsertChars(IImGuiInputTextCallbackData self, int pos, string text, string text_end);
    void ImGuiInputTextCallbackData_SelectAll(IImGuiInputTextCallbackData self);
    void ImGuiInputTextCallbackData_ClearSelection(IImGuiInputTextCallbackData self);
    bool ImGuiInputTextCallbackData_HasSelection(IImGuiInputTextCallbackData self);
    void ImGuiPayload_Clear(IImGuiPayload self);
    bool ImGuiPayload_IsDataType(IImGuiPayload self, string type);
    bool ImGuiPayload_IsPreview(IImGuiPayload self);
    bool ImGuiPayload_IsDelivery(IImGuiPayload self);
    bool ImGuiTextFilter_ImGuiTextRange_empty(IImGuiTextFilter_ImGuiTextRange self);
    void ImGuiTextFilter_ImGuiTextRange_split(IImGuiTextFilter_ImGuiTextRange self, sbyte separator, ImStructVectorWrapper<IImGuiTextFilter_ImGuiTextRange> @out);
    ///<summary>
    /// Helper calling InputText+Build<br/>
    ///</summary>
    bool ImGuiTextFilter_Draw(IImGuiTextFilter self, string label, float width);
    bool ImGuiTextFilter_PassFilter(IImGuiTextFilter self, string text, string text_end);
    void ImGuiTextFilter_Build(IImGuiTextFilter self);
    void ImGuiTextFilter_Clear(IImGuiTextFilter self);
    bool ImGuiTextFilter_IsActive(IImGuiTextFilter self);
    string ImGuiTextBuffer_begin(IImGuiTextBuffer self);
    ///<summary>
    /// Buf is zero-terminated, so end() will point on the zero-terminator<br/>
    ///</summary>
    string ImGuiTextBuffer_end(IImGuiTextBuffer self);
    int ImGuiTextBuffer_size(IImGuiTextBuffer self);
    bool ImGuiTextBuffer_empty(IImGuiTextBuffer self);
    void ImGuiTextBuffer_clear(IImGuiTextBuffer self);
    ///<summary>
    /// Similar to resize(0) on ImVector: empty string but don't free buffer.<br/>
    ///</summary>
    void ImGuiTextBuffer_resize(IImGuiTextBuffer self, int size);
    void ImGuiTextBuffer_reserve(IImGuiTextBuffer self, int capacity);
    string ImGuiTextBuffer_c_str(IImGuiTextBuffer self);
    void ImGuiTextBuffer_append(IImGuiTextBuffer self, string str, string str_end);
    void ImGuiTextBuffer_appendf(IImGuiTextBuffer self, string fmt);
    void ImGuiTextBuffer_appendfv(IImGuiTextBuffer self, string fmt, sbyte* args);
    ///<summary>
    /// - Get***() functions find pair, never add/allocate. Pairs are sorted so a query is O(log N)<br/>
    /// - Set***() functions find pair, insertion on demand if missing.<br/>
    /// - Sorted insertion is costly, paid once. A typical frame shouldn't need to insert any new pair.<br/>
    ///</summary>
    void ImGuiStorage_Clear(IImGuiStorage self);
    int ImGuiStorage_GetInt(IImGuiStorage self, uint key, int default_val);
    void ImGuiStorage_SetInt(IImGuiStorage self, uint key, int val);
    bool ImGuiStorage_GetBool(IImGuiStorage self, uint key, bool default_val);
    void ImGuiStorage_SetBool(IImGuiStorage self, uint key, bool val);
    float ImGuiStorage_GetFloat(IImGuiStorage self, uint key, float default_val);
    void ImGuiStorage_SetFloat(IImGuiStorage self, uint key, float val);
    ///<summary>
    /// default_val is NULL<br/>
    ///</summary>
    void* ImGuiStorage_GetVoidPtr(IImGuiStorage self, uint key);
    void ImGuiStorage_SetVoidPtr(IImGuiStorage self, uint key, void* val);
    ///<summary>
    /// - Get***Ref() functions finds pair, insert on demand if missing, return pointer. Useful if you intend to do Get+Set.<br/>
    /// - References are only valid until a new value is added to the storage. Calling a Set***() function or a Get***Ref() function invalidates the pointer.<br/>
    /// - A typical use case where this is convenient for quick hacking (e.g. add storage during a live Edit&amp;Continue session if you can't modify existing struct)<br/>
    ///      float* pvar = ImGui::GetFloatRef(key); ImGui::SliderFloat("var", pvar, 0, 100.0f); some_var += *pvar;<br/>
    ///</summary>
    int* ImGuiStorage_GetIntRef(IImGuiStorage self, uint key, int default_val);
    bool* ImGuiStorage_GetBoolRef(IImGuiStorage self, uint key, bool default_val);
    float* ImGuiStorage_GetFloatRef(IImGuiStorage self, uint key, float default_val);
    void** ImGuiStorage_GetVoidPtrRef(IImGuiStorage self, uint key, void* default_val);
    ///<summary>
    /// Advanced: for quicker full rebuild of a storage (instead of an incremental one), you may add all your contents and then sort once.<br/>
    ///</summary>
    void ImGuiStorage_BuildSortByKey(IImGuiStorage self);
    ///<summary>
    /// Obsolete: use on your own storage if you know only integer are being stored (open/close all tree nodes)<br/>
    ///</summary>
    void ImGuiStorage_SetAllInt(IImGuiStorage self, int val);
    void ImGuiListClipper_Begin(IImGuiListClipper self, int items_count, float items_height);
    ///<summary>
    /// Automatically called on the last call of Step() that returns false.<br/>
    ///</summary>
    void ImGuiListClipper_End(IImGuiListClipper self);
    ///<summary>
    /// Call until it returns false. The DisplayStart/DisplayEnd fields will be set and you can process/draw those items.<br/>
    ///</summary>
    bool ImGuiListClipper_Step(IImGuiListClipper self);
    ///<summary>
    /// Call IncludeItemByIndex() or IncludeItemsByIndex() *BEFORE* first call to Step() if you need a range of items to not be clipped, regardless of their visibility.<br/>
    /// (Due to alignment / padding of certain items it is possible that an extra item may be included on either end of the display range).<br/>
    ///</summary>
    void ImGuiListClipper_IncludeItemByIndex(IImGuiListClipper self, int item_index);
    ///<summary>
    /// item_end is exclusive e.g. use (42, 42+1) to make item 42 never clipped.<br/>
    ///</summary>
    void ImGuiListClipper_IncludeItemsByIndex(IImGuiListClipper self, int item_begin, int item_end);
    ///<summary>
    /// Seek cursor toward given item. This is automatically called while stepping.<br/>
    /// - The only reason to call this is: you can use ImGuiListClipper::Begin(INT_MAX) if you don't know item count ahead of time.<br/>
    /// - In this case, after all steps are done, you'll want to call SeekCursorForItem(item_count).<br/>
    ///</summary>
    void ImGuiListClipper_SeekCursorForItem(IImGuiListClipper self, int item_index);
    ///<summary>
    /// FIXME-OBSOLETE: May need to obsolete/cleanup those helpers.<br/>
    ///</summary>
    void ImColor_SetHSV(Vector4 self, float h, float s, float v, float a);
    Vector4 ImColor_HSV(float h, float s, float v, float a);
    ///<summary>
    /// Apply selection requests coming from BeginMultiSelect() and EndMultiSelect() functions. It uses 'items_count' passed to BeginMultiSelect()<br/>
    ///</summary>
    void ImGuiSelectionBasicStorage_ApplyRequests(IImGuiSelectionBasicStorage self, IImGuiMultiSelectIO ms_io);
    ///<summary>
    /// Query if an item id is in selection.<br/>
    ///</summary>
    bool ImGuiSelectionBasicStorage_Contains(IImGuiSelectionBasicStorage self, uint id);
    ///<summary>
    /// Clear selection<br/>
    ///</summary>
    void ImGuiSelectionBasicStorage_Clear(IImGuiSelectionBasicStorage self);
    ///<summary>
    /// Swap two selections<br/>
    ///</summary>
    void ImGuiSelectionBasicStorage_Swap(IImGuiSelectionBasicStorage self, IImGuiSelectionBasicStorage r);
    ///<summary>
    /// Add/remove an item from selection (generally done by ApplyRequests() function)<br/>
    ///</summary>
    void ImGuiSelectionBasicStorage_SetItemSelected(IImGuiSelectionBasicStorage self, uint id, bool selected);
    ///<summary>
    /// Iterate selection with 'void* it = NULL; ImGuiID id; while (selection.GetNextSelectedItem(&amp;it, &amp;id)) { ... }'<br/>
    ///</summary>
    bool ImGuiSelectionBasicStorage_GetNextSelectedItem(IImGuiSelectionBasicStorage self, void** opaque_it, ref uint out_id);
    ///<summary>
    /// Convert index to item id based on provided adapter.<br/>
    ///</summary>
    uint ImGuiSelectionBasicStorage_GetStorageIdFromIndex(IImGuiSelectionBasicStorage self, int idx);
    ///<summary>
    /// Apply selection requests by using AdapterSetItemSelected() calls<br/>
    ///</summary>
    void ImGuiSelectionExternalStorage_ApplyRequests(IImGuiSelectionExternalStorage self, IImGuiMultiSelectIO ms_io);
    ///<summary>
    /// == (TexRef._TexData ? TexRef._TexData-&gt;TexID : TexRef._TexID)<br/>
    ///<br/>
    /// Since 1.83: returns ImTextureID associated with this draw call. Warning: DO NOT assume this is always same as 'TextureId' (we will change this function for an upcoming feature)<br/>
    /// Since 1.92: removed ImDrawCmd::TextureId field, the getter function must be used!<br/>
    ///</summary>
    ulong ImDrawCmd_GetTexID(IImDrawCmd self);
    ///<summary>
    /// Do not clear Channels[] so our allocations are reused next frame<br/>
    ///</summary>
    void ImDrawListSplitter_Clear(IImDrawListSplitter self);
    void ImDrawListSplitter_ClearFreeMemory(IImDrawListSplitter self);
    void ImDrawListSplitter_Split(IImDrawListSplitter self, IImDrawList draw_list, int count);
    void ImDrawListSplitter_Merge(IImDrawListSplitter self, IImDrawList draw_list);
    void ImDrawListSplitter_SetCurrentChannel(IImDrawListSplitter self, IImDrawList draw_list, int channel_idx);
    ///<summary>
    /// Render-level scissoring. This is passed down to your render function but not used for CPU-side coarse clipping. Prefer using higher-level ImGui::PushClipRect() to affect logic (hit-testing and widget culling)<br/>
    ///</summary>
    void ImDrawList_PushClipRect(IImDrawList self, Vector2 clip_rect_min, Vector2 clip_rect_max, bool intersect_with_current_clip_rect);
    void ImDrawList_PushClipRectFullScreen(IImDrawList self);
    void ImDrawList_PopClipRect(IImDrawList self);
    void ImDrawList_PushTexture(IImDrawList self, ImTextureRef tex_ref);
    void ImDrawList_PopTexture(IImDrawList self);
    Vector2 ImDrawList_GetClipRectMin(IImDrawList self);
    Vector2 ImDrawList_GetClipRectMax(IImDrawList self);
    ///<summary>
    /// Implied thickness = 1.0f<br/>
    ///<br/>
    /// Primitives<br/>
    /// - Filled shapes must always use clockwise winding order. The anti-aliasing fringe depends on it. Counter-clockwise shapes will have "inward" anti-aliasing.<br/>
    /// - For rectangular primitives, "p_min" and "p_max" represent the upper-left and lower-right corners.<br/>
    /// - For circle primitives, use "num_segments == 0" to automatically calculate tessellation (preferred).<br/>
    ///   In older versions (until Dear ImGui 1.77) the AddCircle functions defaulted to num_segments == 12.<br/>
    ///   In future versions we will use textures to provide cheaper and higher-quality circles.<br/>
    ///   Use AddNgon() and AddNgonFilled() functions if you need to guarantee a specific number of sides.<br/>
    ///</summary>
    void ImDrawList_AddLine(IImDrawList self, Vector2 p1, Vector2 p2, uint col);
    void ImDrawList_AddLineEx(IImDrawList self, Vector2 p1, Vector2 p2, uint col, float thickness);
    ///<summary>
    /// Implied rounding = 0.0f, flags = 0, thickness = 1.0f<br/>
    ///</summary>
    void ImDrawList_AddRect(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col);
    ///<summary>
    /// a: upper-left, b: lower-right (== upper-left + size)<br/>
    ///</summary>
    void ImDrawList_AddRectEx(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags, float thickness);
    ///<summary>
    /// Implied rounding = 0.0f, flags = 0<br/>
    ///</summary>
    void ImDrawList_AddRectFilled(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col);
    ///<summary>
    /// a: upper-left, b: lower-right (== upper-left + size)<br/>
    ///</summary>
    void ImDrawList_AddRectFilledEx(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags);
    void ImDrawList_AddRectFilledMultiColor(IImDrawList self, Vector2 p_min, Vector2 p_max, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left);
    ///<summary>
    /// Implied thickness = 1.0f<br/>
    ///</summary>
    void ImDrawList_AddQuad(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col);
    void ImDrawList_AddQuadEx(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness);
    void ImDrawList_AddQuadFilled(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col);
    ///<summary>
    /// Implied thickness = 1.0f<br/>
    ///</summary>
    void ImDrawList_AddTriangle(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, uint col);
    void ImDrawList_AddTriangleEx(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness);
    void ImDrawList_AddTriangleFilled(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, uint col);
    ///<summary>
    /// Implied num_segments = 0, thickness = 1.0f<br/>
    ///</summary>
    void ImDrawList_AddCircle(IImDrawList self, Vector2 center, float radius, uint col);
    void ImDrawList_AddCircleEx(IImDrawList self, Vector2 center, float radius, uint col, int num_segments, float thickness);
    void ImDrawList_AddCircleFilled(IImDrawList self, Vector2 center, float radius, uint col, int num_segments);
    ///<summary>
    /// Implied thickness = 1.0f<br/>
    ///</summary>
    void ImDrawList_AddNgon(IImDrawList self, Vector2 center, float radius, uint col, int num_segments);
    void ImDrawList_AddNgonEx(IImDrawList self, Vector2 center, float radius, uint col, int num_segments, float thickness);
    void ImDrawList_AddNgonFilled(IImDrawList self, Vector2 center, float radius, uint col, int num_segments);
    ///<summary>
    /// Implied rot = 0.0f, num_segments = 0, thickness = 1.0f<br/>
    ///</summary>
    void ImDrawList_AddEllipse(IImDrawList self, Vector2 center, Vector2 radius, uint col);
    void ImDrawList_AddEllipseEx(IImDrawList self, Vector2 center, Vector2 radius, uint col, float rot, int num_segments, float thickness);
    ///<summary>
    /// Implied rot = 0.0f, num_segments = 0<br/>
    ///</summary>
    void ImDrawList_AddEllipseFilled(IImDrawList self, Vector2 center, Vector2 radius, uint col);
    void ImDrawList_AddEllipseFilledEx(IImDrawList self, Vector2 center, Vector2 radius, uint col, float rot, int num_segments);
    ///<summary>
    /// Implied text_end = NULL<br/>
    ///</summary>
    void ImDrawList_AddText(IImDrawList self, Vector2 pos, uint col, string text_begin);
    void ImDrawList_AddTextEx(IImDrawList self, Vector2 pos, uint col, string text_begin, string text_end);
    ///<summary>
    /// Implied text_end = NULL, wrap_width = 0.0f, cpu_fine_clip_rect = NULL<br/>
    ///</summary>
    void ImDrawList_AddTextImFontPtr(IImDrawList self, IImFont font, float font_size, Vector2 pos, uint col, string text_begin);
    void ImDrawList_AddTextImFontPtrEx(IImDrawList self, IImFont font, float font_size, Vector2 pos, uint col, string text_begin, string text_end, float wrap_width, Vector4 cpu_fine_clip_rect);
    ///<summary>
    /// Cubic Bezier (4 control points)<br/>
    ///</summary>
    void ImDrawList_AddBezierCubic(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness, int num_segments);
    ///<summary>
    /// Quadratic Bezier (3 control points)<br/>
    ///</summary>
    void ImDrawList_AddBezierQuadratic(IImDrawList self, Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness, int num_segments);
    ///<summary>
    /// General polygon<br/>
    /// - Only simple polygons are supported by filling functions (no self-intersections, no holes).<br/>
    /// - Concave polygon fill is more expensive than convex one: it has O(N^2) complexity. Provided as a convenience for the user but not used by the main library.<br/>
    ///</summary>
    void ImDrawList_AddPolyline(IImDrawList self, Vector2 points, int num_points, uint col, ImDrawFlags flags, float thickness);
    void ImDrawList_AddConvexPolyFilled(IImDrawList self, Vector2 points, int num_points, uint col);
    void ImDrawList_AddConcavePolyFilled(IImDrawList self, Vector2 points, int num_points, uint col);
    ///<summary>
    /// Implied uv_min = ImVec2(0, 0), uv_max = ImVec2(1, 1), col = IM_COL32_WHITE<br/>
    ///<br/>
    /// Image primitives<br/>
    /// - Read FAQ to understand what ImTextureID/ImTextureRef are.<br/>
    /// - "p_min" and "p_max" represent the upper-left and lower-right corners of the rectangle.<br/>
    /// - "uv_min" and "uv_max" represent the normalized texture coordinates to use for those corners. Using (0,0)-&gt;(1,1) texture coordinates will generally display the entire texture.<br/>
    ///</summary>
    void ImDrawList_AddImage(IImDrawList self, ImTextureRef tex_ref, Vector2 p_min, Vector2 p_max);
    void ImDrawList_AddImageEx(IImDrawList self, ImTextureRef tex_ref, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col);
    ///<summary>
    /// Implied uv1 = ImVec2(0, 0), uv2 = ImVec2(1, 0), uv3 = ImVec2(1, 1), uv4 = ImVec2(0, 1), col = IM_COL32_WHITE<br/>
    ///</summary>
    void ImDrawList_AddImageQuad(IImDrawList self, ImTextureRef tex_ref, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4);
    void ImDrawList_AddImageQuadEx(IImDrawList self, ImTextureRef tex_ref, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, uint col);
    void ImDrawList_AddImageRounded(IImDrawList self, ImTextureRef tex_ref, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col, float rounding, ImDrawFlags flags);
    ///<summary>
    /// Stateful path API, add points then finish with PathFillConvex() or PathStroke()<br/>
    /// - Important: filled shapes must always use clockwise winding order! The anti-aliasing fringe depends on it. Counter-clockwise shapes will have "inward" anti-aliasing.<br/>
    ///   so e.g. 'PathArcTo(center, radius, PI * -0.5f, PI)' is ok, whereas 'PathArcTo(center, radius, PI, PI * -0.5f)' won't have correct anti-aliasing when followed by PathFillConvex().<br/>
    ///</summary>
    void ImDrawList_PathClear(IImDrawList self);
    void ImDrawList_PathLineTo(IImDrawList self, Vector2 pos);
    void ImDrawList_PathLineToMergeDuplicate(IImDrawList self, Vector2 pos);
    void ImDrawList_PathFillConvex(IImDrawList self, uint col);
    void ImDrawList_PathFillConcave(IImDrawList self, uint col);
    void ImDrawList_PathStroke(IImDrawList self, uint col, ImDrawFlags flags, float thickness);
    void ImDrawList_PathArcTo(IImDrawList self, Vector2 center, float radius, float a_min, float a_max, int num_segments);
    ///<summary>
    /// Use precomputed angles for a 12 steps circle<br/>
    ///</summary>
    void ImDrawList_PathArcToFast(IImDrawList self, Vector2 center, float radius, int a_min_of_12, int a_max_of_12);
    ///<summary>
    /// Implied num_segments = 0<br/>
    ///</summary>
    void ImDrawList_PathEllipticalArcTo(IImDrawList self, Vector2 center, Vector2 radius, float rot, float a_min, float a_max);
    ///<summary>
    /// Ellipse<br/>
    ///</summary>
    void ImDrawList_PathEllipticalArcToEx(IImDrawList self, Vector2 center, Vector2 radius, float rot, float a_min, float a_max, int num_segments);
    ///<summary>
    /// Cubic Bezier (4 control points)<br/>
    ///</summary>
    void ImDrawList_PathBezierCubicCurveTo(IImDrawList self, Vector2 p2, Vector2 p3, Vector2 p4, int num_segments);
    ///<summary>
    /// Quadratic Bezier (3 control points)<br/>
    ///</summary>
    void ImDrawList_PathBezierQuadraticCurveTo(IImDrawList self, Vector2 p2, Vector2 p3, int num_segments);
    void ImDrawList_PathRect(IImDrawList self, Vector2 rect_min, Vector2 rect_max, float rounding, ImDrawFlags flags);
    ///<summary>
    /// Implied userdata_size = 0<br/>
    ///<br/>
    /// Advanced: Draw Callbacks<br/>
    /// - May be used to alter render state (change sampler, blending, current shader). May be used to emit custom rendering commands (difficult to do correctly, but possible).<br/>
    /// - Use special ImDrawCallback_ResetRenderState callback to instruct backend to reset its render state to the default.<br/>
    /// - Your rendering loop must check for 'UserCallback' in ImDrawCmd and call the function instead of rendering triangles. All standard backends are honoring this.<br/>
    /// - For some backends, the callback may access selected render-states exposed by the backend in a ImGui_ImplXXXX_RenderState structure pointed to by platform_io.Renderer_RenderState.<br/>
    /// - IMPORTANT: please be mindful of the different level of indirection between using size==0 (copying argument) and using size&gt;0 (copying pointed data into a buffer).<br/>
    ///   - If userdata_size == 0: we copy/store the 'userdata' argument as-is. It will be available unmodified in ImDrawCmd::UserCallbackData during render.<br/>
    ///   - If userdata_size &gt; 0,  we copy/store 'userdata_size' bytes pointed to by 'userdata'. We store them in a buffer stored inside the drawlist. ImDrawCmd::UserCallbackData will point inside that buffer so you have to retrieve data from there. Your callback may need to use ImDrawCmd::UserCallbackDataSize if you expect dynamically-sized data.<br/>
    ///   - Support for userdata_size &gt; 0 was added in v1.91.4, October 2024. So earlier code always only allowed to copy/store a simple void*.<br/>
    ///</summary>
    void ImDrawList_AddCallback(IImDrawList self, delegate* unmanaged[Cdecl]<nint, nint, void> callback, void* userdata);
    void ImDrawList_AddCallbackEx(IImDrawList self, delegate* unmanaged[Cdecl]<nint, nint, void> callback, void* userdata, nuint userdata_size);
    ///<summary>
    /// This is useful if you need to forcefully create a new draw call (to allow for dependent rendering / blending). Otherwise primitives are merged into the same draw-call as much as possible<br/>
    ///<br/>
    /// Advanced: Miscellaneous<br/>
    ///</summary>
    void ImDrawList_AddDrawCmd(IImDrawList self);
    ///<summary>
    /// Create a clone of the CmdBuffer/IdxBuffer/VtxBuffer. For multi-threaded rendering, consider using `imgui_threaded_rendering` from https:github.com/ocornut/imgui_club instead.<br/>
    ///</summary>
    IImDrawList ImDrawList_CloneOutput(IImDrawList self);
    ///<summary>
    /// Advanced: Channels<br/>
    /// - Use to split render into layers. By switching channels to can render out-of-order (e.g. submit FG primitives before BG primitives)<br/>
    /// - Use to minimize draw calls (e.g. if going back-and-forth between multiple clipping rectangles, prefer to append into separate channels then merge at the end)<br/>
    /// - This API shouldn't have been in ImDrawList in the first place!<br/>
    ///   Prefer using your own persistent instance of ImDrawListSplitter as you can stack them.<br/>
    ///   Using the ImDrawList::ChannelsXXXX you cannot stack a split over another.<br/>
    ///</summary>
    void ImDrawList_ChannelsSplit(IImDrawList self, int count);
    void ImDrawList_ChannelsMerge(IImDrawList self);
    void ImDrawList_ChannelsSetCurrent(IImDrawList self, int n);
    ///<summary>
    /// Advanced: Primitives allocations<br/>
    /// - We render triangles (three vertices)<br/>
    /// - All primitives needs to be reserved via PrimReserve() beforehand.<br/>
    ///</summary>
    void ImDrawList_PrimReserve(IImDrawList self, int idx_count, int vtx_count);
    void ImDrawList_PrimUnreserve(IImDrawList self, int idx_count, int vtx_count);
    ///<summary>
    /// Axis aligned rectangle (composed of two triangles)<br/>
    ///</summary>
    void ImDrawList_PrimRect(IImDrawList self, Vector2 a, Vector2 b, uint col);
    void ImDrawList_PrimRectUV(IImDrawList self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
    void ImDrawList_PrimQuadUV(IImDrawList self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
    void ImDrawList_PrimWriteVtx(IImDrawList self, Vector2 pos, Vector2 uv, uint col);
    void ImDrawList_PrimWriteIdx(IImDrawList self, ushort idx);
    ///<summary>
    /// Write vertex with unique index<br/>
    ///</summary>
    void ImDrawList_PrimVtx(IImDrawList self, Vector2 pos, Vector2 uv, uint col);
    ///<summary>
    /// RENAMED in 1.92.0<br/>
    ///</summary>
    void ImDrawList_PushTextureID(IImDrawList self, ImTextureRef tex_ref);
    ///<summary>
    /// RENAMED in 1.92.0<br/>
    ///</summary>
    void ImDrawList_PopTextureID(IImDrawList self);
    ///<summary>
    /// [Internal helpers]<br/>
    ///</summary>
    void ImDrawList__SetDrawListSharedData(IImDrawList self, nint data);
    void ImDrawList__ResetForNewFrame(IImDrawList self);
    void ImDrawList__ClearFreeMemory(IImDrawList self);
    void ImDrawList__PopUnusedDrawCmd(IImDrawList self);
    void ImDrawList__TryMergeDrawCmds(IImDrawList self);
    void ImDrawList__OnChangedClipRect(IImDrawList self);
    void ImDrawList__OnChangedTexture(IImDrawList self);
    void ImDrawList__OnChangedVtxOffset(IImDrawList self);
    void ImDrawList__SetTexture(IImDrawList self, ImTextureRef tex_ref);
    int ImDrawList__CalcCircleAutoSegmentCount(IImDrawList self, float radius);
    void ImDrawList__PathArcToFastEx(IImDrawList self, Vector2 center, float radius, int a_min_sample, int a_max_sample, int a_step);
    void ImDrawList__PathArcToN(IImDrawList self, Vector2 center, float radius, float a_min, float a_max, int num_segments);
    void ImDrawData_Clear(IImDrawData self);
    ///<summary>
    /// Helper to add an external draw list into an existing ImDrawData.<br/>
    ///</summary>
    void ImDrawData_AddDrawList(IImDrawData self, IImDrawList draw_list);
    ///<summary>
    /// Helper to convert all buffers from indexed to non-indexed, in case you cannot render indexed. Note: this is slow and most likely a waste of resources. Always prefer indexed rendering!<br/>
    ///</summary>
    void ImDrawData_DeIndexAllBuffers(IImDrawData self);
    ///<summary>
    /// Helper to scale the ClipRect field of each ImDrawCmd. Use if your final output buffer is at a different scale than Dear ImGui expects, or if there is a difference between your window resolution and framebuffer resolution.<br/>
    ///</summary>
    void ImDrawData_ScaleClipRects(IImDrawData self, Vector2 fb_scale);
    void ImTextureData_Create(IImTextureData self, ImTextureFormat format, int w, int h);
    void ImTextureData_DestroyPixels(IImTextureData self);
    void* ImTextureData_GetPixels(IImTextureData self);
    void* ImTextureData_GetPixelsAt(IImTextureData self, int x, int y);
    int ImTextureData_GetSizeInBytes(IImTextureData self);
    int ImTextureData_GetPitch(IImTextureData self);
    ImTextureRef ImTextureData_GetTexRef(IImTextureData self);
    ulong ImTextureData_GetTexID(IImTextureData self);
    ///<summary>
    /// Called by Renderer backend<br/>
    /// - Call SetTexID() and SetStatus() after honoring texture requests. Never modify TexID and Status directly!<br/>
    /// - A backend may decide to destroy a texture that we did not request to destroy, which is fine (e.g. freeing resources), but we immediately set the texture back in _WantCreate mode.<br/>
    ///</summary>
    void ImTextureData_SetTexID(IImTextureData self, ulong tex_id);
    void ImTextureData_SetStatus(IImTextureData self, ImTextureStatus status);
    void ImFontGlyphRangesBuilder_Clear(IImFontGlyphRangesBuilder self);
    ///<summary>
    /// Get bit n in the array<br/>
    ///</summary>
    bool ImFontGlyphRangesBuilder_GetBit(IImFontGlyphRangesBuilder self, nuint n);
    ///<summary>
    /// Set bit n in the array<br/>
    ///</summary>
    void ImFontGlyphRangesBuilder_SetBit(IImFontGlyphRangesBuilder self, nuint n);
    ///<summary>
    /// Add character<br/>
    ///</summary>
    void ImFontGlyphRangesBuilder_AddChar(IImFontGlyphRangesBuilder self, uint c);
    ///<summary>
    /// Add string (each character of the UTF-8 string are added)<br/>
    ///</summary>
    void ImFontGlyphRangesBuilder_AddText(IImFontGlyphRangesBuilder self, string text, string text_end);
    ///<summary>
    /// Add ranges, e.g. builder.AddRanges(ImFontAtlas::GetGlyphRangesDefault()) to force add all of ASCII/Latin+Ext<br/>
    ///</summary>
    void ImFontGlyphRangesBuilder_AddRanges(IImFontGlyphRangesBuilder self, ref uint ranges);
    ///<summary>
    /// Output new ranges (ImVector_Construct()/ImVector_Destruct() can be used to safely construct out_ranges)<br/>
    ///</summary>
    void ImFontGlyphRangesBuilder_BuildRanges(IImFontGlyphRangesBuilder self, ref ImVector<uint> out_ranges);
    IImFont ImFontAtlas_AddFont(IImFontAtlas self, IImFontConfig font_cfg);
    IImFont ImFontAtlas_AddFontDefault(IImFontAtlas self, IImFontConfig font_cfg);
    IImFont ImFontAtlas_AddFontFromFileTTF(IImFontAtlas self, string filename, float size_pixels, IImFontConfig font_cfg, ref uint glyph_ranges);
    ///<summary>
    /// Note: Transfer ownership of 'ttf_data' to ImFontAtlas! Will be deleted after destruction of the atlas. Set font_cfg-&gt;FontDataOwnedByAtlas=false to keep ownership of your data and it won't be freed.<br/>
    ///</summary>
    IImFont ImFontAtlas_AddFontFromMemoryTTF(IImFontAtlas self, void* font_data, int font_data_size, float size_pixels, IImFontConfig font_cfg, ref uint glyph_ranges);
    ///<summary>
    /// 'compressed_font_data' still owned by caller. Compress with binary_to_compressed_c.cpp.<br/>
    ///</summary>
    IImFont ImFontAtlas_AddFontFromMemoryCompressedTTF(IImFontAtlas self, void* compressed_font_data, int compressed_font_data_size, float size_pixels, IImFontConfig font_cfg, ref uint glyph_ranges);
    ///<summary>
    /// 'compressed_font_data_base85' still owned by caller. Compress with binary_to_compressed_c.cpp with -base85 parameter.<br/>
    ///</summary>
    IImFont ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(IImFontAtlas self, string compressed_font_data_base85, float size_pixels, IImFontConfig font_cfg, ref uint glyph_ranges);
    void ImFontAtlas_RemoveFont(IImFontAtlas self, IImFont font);
    ///<summary>
    /// Clear everything (input fonts, output glyphs/textures).<br/>
    ///</summary>
    void ImFontAtlas_Clear(IImFontAtlas self);
    ///<summary>
    /// Compact cached glyphs and texture.<br/>
    ///</summary>
    void ImFontAtlas_CompactCache(IImFontAtlas self);
    ///<summary>
    /// Change font loader at runtime.<br/>
    ///</summary>
    void ImFontAtlas_SetFontLoader(IImFontAtlas self, IImFontLoader font_loader);
    ///<summary>
    /// [OBSOLETE] Clear input data (all ImFontConfig structures including sizes, TTF data, glyph ranges, etc.) = all the data used to build the texture and fonts.<br/>
    ///<br/>
    /// As we are transitioning toward a new font system, we expect to obsolete those soon:<br/>
    ///</summary>
    void ImFontAtlas_ClearInputData(IImFontAtlas self);
    ///<summary>
    /// [OBSOLETE] Clear input+output font data (same as ClearInputData() + glyphs storage, UV coordinates).<br/>
    ///</summary>
    void ImFontAtlas_ClearFonts(IImFontAtlas self);
    ///<summary>
    /// [OBSOLETE] Clear CPU-side copy of the texture data. Saves RAM once the texture has been copied to graphics memory.<br/>
    ///</summary>
    void ImFontAtlas_ClearTexData(IImFontAtlas self);
    ///<summary>
    /// Build pixels data. This is called automatically for you by the GetTexData*** functions.<br/>
    ///<br/>
    /// Legacy path for build atlas + retrieving pixel data.<br/>
    /// - User is in charge of copying the pixels into graphics memory (e.g. create a texture with your engine). Then store your texture handle with SetTexID().<br/>
    /// - The pitch is always = Width * BytesPerPixels (1 or 4)<br/>
    /// - Building in RGBA32 format is provided for convenience and compatibility, but note that unless you manually manipulate or copy color data into<br/>
    ///   the texture (e.g. when using the AddCustomRect*** api), then the RGB pixels emitted will always be white (~75% of memory/bandwidth waste).<br/>
    /// - From 1.92 with backends supporting ImGuiBackendFlags_RendererHasTextures:<br/>
    ///   - Calling Build(), GetTexDataAsAlpha8(), GetTexDataAsRGBA32() is not needed.<br/>
    ///   - In backend: replace calls to ImFontAtlas::SetTexID() with calls to ImTextureData::SetTexID() after honoring texture creation.<br/>
    ///</summary>
    bool ImFontAtlas_Build(IImFontAtlas self);
    ///<summary>
    /// 1 byte per-pixel<br/>
    ///</summary>
    void ImFontAtlas_GetTexDataAsAlpha8(IImFontAtlas self, byte** out_pixels, ref int out_width, ref int out_height, ref int out_bytes_per_pixel);
    ///<summary>
    /// 4 bytes-per-pixel<br/>
    ///</summary>
    void ImFontAtlas_GetTexDataAsRGBA32(IImFontAtlas self, byte** out_pixels, ref int out_width, ref int out_height, ref int out_bytes_per_pixel);
    ///<summary>
    /// Called by legacy backends. May be called before texture creation.<br/>
    ///</summary>
    void ImFontAtlas_SetTexID(IImFontAtlas self, ulong id);
    ///<summary>
    /// Called by legacy backends.<br/>
    ///</summary>
    void ImFontAtlas_SetTexIDImTextureRef(IImFontAtlas self, ImTextureRef id);
    ///<summary>
    /// Bit ambiguous: used to detect when user didn't build texture but effectively we should check TexID != 0 except that would be backend dependent..<br/>
    ///</summary>
    bool ImFontAtlas_IsBuilt(IImFontAtlas self);
    ///<summary>
    /// Basic Latin, Extended Latin<br/>
    ///<br/>
    /// Since 1.92: specifying glyph ranges is only useful/necessary if your backend doesn't support ImGuiBackendFlags_RendererHasTextures!<br/>
    ///</summary>
    uint* ImFontAtlas_GetGlyphRangesDefault(IImFontAtlas self);
    ///<summary>
    /// Default + Greek and Coptic<br/>
    ///<br/>
    /// Helpers to retrieve list of common Unicode ranges (2 value per range, values are inclusive, zero-terminated list)<br/>
    /// NB: Make sure that your string are UTF-8 and NOT in your local code page.<br/>
    /// Read https:github.com/ocornut/imgui/blob/master/docs/FONTS.md/#about-utf-8-encoding for details.<br/>
    /// NB: Consider using ImFontGlyphRangesBuilder to build glyph ranges from textual data.<br/>
    ///</summary>
    uint* ImFontAtlas_GetGlyphRangesGreek(IImFontAtlas self);
    ///<summary>
    /// Default + Korean characters<br/>
    ///</summary>
    uint* ImFontAtlas_GetGlyphRangesKorean(IImFontAtlas self);
    ///<summary>
    /// Default + Hiragana, Katakana, Half-Width, Selection of 2999 Ideographs<br/>
    ///</summary>
    uint* ImFontAtlas_GetGlyphRangesJapanese(IImFontAtlas self);
    ///<summary>
    /// Default + Half-Width + Japanese Hiragana/Katakana + full set of about 21000 CJK Unified Ideographs<br/>
    ///</summary>
    uint* ImFontAtlas_GetGlyphRangesChineseFull(IImFontAtlas self);
    ///<summary>
    /// Default + Half-Width + Japanese Hiragana/Katakana + set of 2500 CJK Unified Ideographs for common simplified Chinese<br/>
    ///</summary>
    uint* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(IImFontAtlas self);
    ///<summary>
    /// Default + about 400 Cyrillic characters<br/>
    ///</summary>
    uint* ImFontAtlas_GetGlyphRangesCyrillic(IImFontAtlas self);
    ///<summary>
    /// Default + Thai characters<br/>
    ///</summary>
    uint* ImFontAtlas_GetGlyphRangesThai(IImFontAtlas self);
    ///<summary>
    /// Default + Vietnamese characters<br/>
    ///</summary>
    uint* ImFontAtlas_GetGlyphRangesVietnamese(IImFontAtlas self);
    ///<summary>
    /// Register a rectangle. Return -1 (ImFontAtlasRectId_Invalid) on error.<br/>
    ///<br/>
    /// Register and retrieve custom rectangles<br/>
    /// - You can request arbitrary rectangles to be packed into the atlas, for your own purpose.<br/>
    /// - Since 1.92.0, packing is done immediately in the function call (previously packing was done during the Build call)<br/>
    /// - You can render your pixels into the texture right after calling the AddCustomRect() functions.<br/>
    /// - VERY IMPORTANT:<br/>
    ///   - Texture may be created/resized at any time when calling ImGui or ImFontAtlas functions.<br/>
    ///   - IT WILL INVALIDATE RECTANGLE DATA SUCH AS UV COORDINATES. Always use latest values from GetCustomRect().<br/>
    ///   - UV coordinates are associated to the current texture identifier aka 'atlas-&gt;TexRef'. Both TexRef and UV coordinates are typically changed at the same time.<br/>
    /// - If you render colored output into your custom rectangles: set 'atlas-&gt;TexPixelsUseColors = true' as this may help some backends decide of preferred texture format.<br/>
    /// - Read docs/FONTS.md for more details about using colorful icons.<br/>
    /// - Note: this API may be reworked further in order to facilitate supporting e.g. multi-monitor, varying DPI settings.<br/>
    /// - (Pre-1.92 names) ------------&gt; (1.92 names)<br/>
    ///   - GetCustomRectByIndex()   --&gt; Use GetCustomRect()<br/>
    ///   - CalcCustomRectUV()       --&gt; Use GetCustomRect() and read uv0, uv1 fields.<br/>
    ///   - AddCustomRectRegular()   --&gt; Renamed to AddCustomRect()<br/>
    ///   - AddCustomRectFontGlyph() --&gt; Prefer using custom ImFontLoader inside ImFontConfig<br/>
    ///   - ImFontAtlasCustomRect    --&gt; Renamed to ImFontAtlasRect<br/>
    ///</summary>
    int ImFontAtlas_AddCustomRect(IImFontAtlas self, int width, int height, IImFontAtlasRect out_r);
    ///<summary>
    /// Unregister a rectangle. Existing pixels will stay in texture until resized / garbage collected.<br/>
    ///</summary>
    void ImFontAtlas_RemoveCustomRect(IImFontAtlas self, int id);
    ///<summary>
    /// Get rectangle coordinates for current texture. Valid immediately, never store this (read above)!<br/>
    ///</summary>
    bool ImFontAtlas_GetCustomRect(IImFontAtlas self, int id, IImFontAtlasRect out_r);
    ///<summary>
    /// RENAMED in 1.92.0<br/>
    ///</summary>
    int ImFontAtlas_AddCustomRectRegular(IImFontAtlas self, int w, int h);
    ///<summary>
    /// OBSOLETED in 1.92.0<br/>
    ///</summary>
    IImFontAtlasRect ImFontAtlas_GetCustomRectByIndex(IImFontAtlas self, int id);
    ///<summary>
    /// OBSOLETED in 1.92.0<br/>
    ///</summary>
    void ImFontAtlas_CalcCustomRectUV(IImFontAtlas self, IImFontAtlasRect r, Vector2 out_uv_min, Vector2 out_uv_max);
    ///<summary>
    /// OBSOLETED in 1.92.0: Use custom ImFontLoader in ImFontConfig<br/>
    ///</summary>
    int ImFontAtlas_AddCustomRectFontGlyph(IImFontAtlas self, IImFont font, uint codepoint, int w, int h, float advance_x, Vector2 offset);
    ///<summary>
    /// ADDED AND OBSOLETED in 1.92.0<br/>
    ///</summary>
    int ImFontAtlas_AddCustomRectFontGlyphForSize(IImFontAtlas self, IImFont font, float font_size, uint codepoint, int w, int h, float advance_x, Vector2 offset);
    void ImFontBaked_ClearOutputData(IImFontBaked self);
    ///<summary>
    /// Return U+FFFD glyph if requested glyph doesn't exists.<br/>
    ///</summary>
    IImFontGlyph ImFontBaked_FindGlyph(IImFontBaked self, uint c);
    ///<summary>
    /// Return NULL if glyph doesn't exist<br/>
    ///</summary>
    IImFontGlyph ImFontBaked_FindGlyphNoFallback(IImFontBaked self, uint c);
    float ImFontBaked_GetCharAdvance(IImFontBaked self, uint c);
    bool ImFontBaked_IsGlyphLoaded(IImFontBaked self, uint c);
    bool ImFont_IsGlyphInFont(IImFont self, uint c);
    bool ImFont_IsLoaded(IImFont self);
    ///<summary>
    /// Fill ImFontConfig::Name.<br/>
    ///</summary>
    string ImFont_GetDebugName(IImFont self);
    ///<summary>
    /// Implied density = -1.0f<br/>
    ///<br/>
    /// [Internal] Don't use!<br/>
    /// 'max_width' stops rendering after a certain width (could be turned into a 2d size). FLT_MAX to disable.<br/>
    /// 'wrap_width' enable automatic word-wrapping across multiple lines to fit into given width. 0.0f to disable.<br/>
    ///</summary>
    IImFontBaked ImFont_GetFontBaked(IImFont self, float font_size);
    ///<summary>
    /// Get or create baked data for given size<br/>
    ///</summary>
    IImFontBaked ImFont_GetFontBakedEx(IImFont self, float font_size, float density);
    ///<summary>
    /// Implied text_end = NULL, out_remaining = NULL<br/>
    ///</summary>
    Vector2 ImFont_CalcTextSizeA(IImFont self, float size, float max_width, float wrap_width, string text_begin);
    Vector2 ImFont_CalcTextSizeAEx(IImFont self, float size, float max_width, float wrap_width, string text_begin, string text_end, sbyte** out_remaining);
    string ImFont_CalcWordWrapPosition(IImFont self, float size, string text, string text_end, float wrap_width);
    ///<summary>
    /// Implied cpu_fine_clip = NULL<br/>
    ///</summary>
    void ImFont_RenderChar(IImFont self, IImDrawList draw_list, float size, Vector2 pos, uint col, uint c);
    void ImFont_RenderCharEx(IImFont self, IImDrawList draw_list, float size, Vector2 pos, uint col, uint c, Vector4 cpu_fine_clip);
    void ImFont_RenderText(IImFont self, IImDrawList draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, string text_begin, string text_end, float wrap_width, int flags);
    string ImFont_CalcWordWrapPositionA(IImFont self, float scale, string text, string text_end, float wrap_width);
    ///<summary>
    /// [Internal] Don't use!<br/>
    ///</summary>
    void ImFont_ClearOutputData(IImFont self);
    ///<summary>
    /// Makes 'from_codepoint' character points to 'to_codepoint' glyph.<br/>
    ///</summary>
    void ImFont_AddRemapChar(IImFont self, uint from_codepoint, uint to_codepoint);
    bool ImFont_IsGlyphRangeUnused(IImFont self, uint c_begin, uint c_last);
    ///<summary>
    /// Helpers<br/>
    ///</summary>
    Vector2 ImGuiViewport_GetCenter(IImGuiViewport self);
    Vector2 ImGuiViewport_GetWorkCenter(IImGuiViewport self);
    ///<summary>
    /// Clear all Platform_XXX fields. Typically called on Platform Backend shutdown.<br/>
    ///</summary>
    void ImGuiPlatformIO_ClearPlatformHandlers(IImGuiPlatformIO self);
    ///<summary>
    /// Clear all Renderer_XXX fields. Typically called on Renderer Backend shutdown.<br/>
    ///</summary>
    void ImGuiPlatformIO_ClearRendererHandlers(IImGuiPlatformIO self);
    ///<summary>
    /// OBSOLETED in 1.92.0 (from June 2025)<br/>
    ///</summary>
    void PushFont(IImFont font);
    ///<summary>
    /// Set font scale factor for current window. Prefer using PushFont(NULL, style.FontSizeBase * factor) or use style.FontScaleMain to scale all windows.<br/>
    ///</summary>
    void SetWindowFontScale(float scale);
    ///<summary>
    /// &lt;-- 'border_col' was removed in favor of ImGuiCol_ImageBorder. If you use 'tint_col', use ImageWithBg() instead.<br/>
    ///<br/>
    /// OBSOLETED in 1.91.9 (from February 2025)<br/>
    ///</summary>
    void ImageImVec4(ImTextureRef tex_ref, Vector2 image_size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col);
    ///<summary>
    /// OBSOLETED in 1.91.0 (from July 2024)<br/>
    ///</summary>
    void PushButtonRepeat(bool repeat);
    void PopButtonRepeat();
    void PushTabStop(bool tab_stop);
    void PopTabStop();
    ///<summary>
    /// Content boundaries max (e.g. window boundaries including scrolling, or current column boundaries). You should never need this. Always use GetCursorScreenPos() and GetContentRegionAvail()!<br/>
    ///</summary>
    Vector2 GetContentRegionMax();
    ///<summary>
    /// Content boundaries min for the window (roughly (0,0)-Scroll), in window-local coordinates. You should never need this. Always use GetCursorScreenPos() and GetContentRegionAvail()!<br/>
    ///</summary>
    Vector2 GetWindowContentRegionMin();
    ///<summary>
    /// Content boundaries max for the window (roughly (0,0)+Size-Scroll), in window-local coordinates. You should never need this. Always use GetCursorScreenPos() and GetContentRegionAvail()!<br/>
    ///</summary>
    Vector2 GetWindowContentRegionMax();
    ///<summary>
    /// Implied window_flags = 0<br/>
    ///<br/>
    /// OBSOLETED in 1.90.0 (from September 2023)<br/>
    ///</summary>
    bool BeginChildFrame(uint id, Vector2 size);
    bool BeginChildFrameEx(uint id, Vector2 size, ImGuiWindowFlags window_flags);
    void EndChildFrame();
    ///<summary>
    ///inline bool       BeginChild(const char* str_id, const ImVec2&amp; size_arg, bool borders, ImGuiWindowFlags window_flags){ return BeginChild(str_id, size_arg, borders ? ImGuiChildFlags_Borders : ImGuiChildFlags_None, window_flags); }  Unnecessary as true == ImGuiChildFlags_Borders<br/>
    ///inline bool       BeginChild(ImGuiID id, const ImVec2&amp; size_arg, bool borders, ImGuiWindowFlags window_flags)        { return BeginChild(id, size_arg, borders ? ImGuiChildFlags_Borders : ImGuiChildFlags_None, window_flags);     }  Unnecessary as true == ImGuiChildFlags_Borders<br/>
    ///</summary>
    void ShowStackToolWindow(ref bool p_open);
    ///<summary>
    /// Implied popup_max_height_in_items = -1<br/>
    ///</summary>
    bool ComboObsolete(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count);
    bool ComboObsoleteEx(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count, int popup_max_height_in_items);
    ///<summary>
    /// Implied height_in_items = -1<br/>
    ///</summary>
    bool ListBoxObsolete(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count);
    bool ListBoxObsoleteEx(string label, ref int current_item, delegate* unmanaged[Cdecl]<nint, int, nint, byte> old_callback, void* user_data, int items_count, int height_in_items);
    ///<summary>
    /// - prefer deep-copying this into your own ImFontLoader instance if you use hot-reloading that messes up static data.<br/>
    ///</summary>
    IImFontLoader ImGuiFreeTypeGetFontLoader();
    ///<summary>
    /// Implied user_data = nullptr<br/>
    ///<br/>
    /// Override allocators. By default ImGuiFreeType will use IM_ALLOC()/IM_FREE()<br/>
    /// However, as FreeType does lots of allocations we provide a way for the user to redirect it to a separate memory heap if desired.<br/>
    ///</summary>
    void ImGuiFreeTypeSetAllocatorFunctions(delegate* unmanaged[Cdecl]<nuint, nint, nint> alloc_func, delegate* unmanaged[Cdecl]<nint, nint, void> free_func);
    void ImGuiFreeTypeSetAllocatorFunctionsEx(delegate* unmanaged[Cdecl]<nuint, nint, nint> alloc_func, delegate* unmanaged[Cdecl]<nint, nint, void> free_func, void* user_data);
    ///<summary>
    /// Display UI to edit ImFontAtlas::FontLoaderFlags (shared) or ImFontConfig::FontLoaderFlags (single source)<br/>
    ///</summary>
    bool ImGuiFreeTypeDebugEditFontLoaderFlags(ref uint p_font_loader_flags);
}

///<summary>
/// Data shared among multiple draw lists (typically owned by parent ImGui context, but you may create one yourself)<br/>
///</summary>
public unsafe interface IImDrawListSharedData
{
    public nint NativePointer { get; }
}

///<summary>
/// Opaque storage for building a ImFontAtlas<br/>
///</summary>
public unsafe interface IImFontAtlasBuilder
{
    public nint NativePointer { get; }
}

///<summary>
/// Opaque interface to a font loading backend (stb_truetype, FreeType etc.).<br/>
///</summary>
public unsafe interface IImFontLoader
{
    public nint NativePointer { get; }
}

///<summary>
/// Dear ImGui context (opaque structure, unless including imgui_internal.h)<br/>
///<br/>
/// Forward declarations: ImGui layer<br/>
///</summary>
public unsafe interface IImGuiContext
{
    public nint NativePointer { get; }
}

///<summary>
/// Flags for ImGui::Begin()<br/>
/// (Those are per-window flags. There are shared flags in ImGuiIO: io.ConfigWindowsResizeFromEdges and io.ConfigWindowsMoveFromTitleBarOnly)<br/>
///</summary>
public enum ImGuiWindowFlags
{
    ImGuiWindowFlags_None = 0,
    ///<summary>
    /// Disable title-bar<br/>
    ///</summary>
    ImGuiWindowFlags_NoTitleBar = 1 << 0,
    ///<summary>
    /// Disable user resizing with the lower-right grip<br/>
    ///</summary>
    ImGuiWindowFlags_NoResize = 1 << 1,
    ///<summary>
    /// Disable user moving the window<br/>
    ///</summary>
    ImGuiWindowFlags_NoMove = 1 << 2,
    ///<summary>
    /// Disable scrollbars (window can still scroll with mouse or programmatically)<br/>
    ///</summary>
    ImGuiWindowFlags_NoScrollbar = 1 << 3,
    ///<summary>
    /// Disable user vertically scrolling with mouse wheel. On child window, mouse wheel will be forwarded to the parent unless NoScrollbar is also set.<br/>
    ///</summary>
    ImGuiWindowFlags_NoScrollWithMouse = 1 << 4,
    ///<summary>
    /// Disable user collapsing window by double-clicking on it. Also referred to as Window Menu Button (e.g. within a docking node).<br/>
    ///</summary>
    ImGuiWindowFlags_NoCollapse = 1 << 5,
    ///<summary>
    /// Resize every window to its content every frame<br/>
    ///</summary>
    ImGuiWindowFlags_AlwaysAutoResize = 1 << 6,
    ///<summary>
    /// Disable drawing background color (WindowBg, etc.) and outside border. Similar as using SetNextWindowBgAlpha(0.0f).<br/>
    ///</summary>
    ImGuiWindowFlags_NoBackground = 1 << 7,
    ///<summary>
    /// Never load/save settings in .ini file<br/>
    ///</summary>
    ImGuiWindowFlags_NoSavedSettings = 1 << 8,
    ///<summary>
    /// Disable catching mouse, hovering test with pass through.<br/>
    ///</summary>
    ImGuiWindowFlags_NoMouseInputs = 1 << 9,
    ///<summary>
    /// Has a menu-bar<br/>
    ///</summary>
    ImGuiWindowFlags_MenuBar = 1 << 10,
    ///<summary>
    /// Allow horizontal scrollbar to appear (off by default). You may use SetNextWindowContentSize(ImVec2(width,0.0f)); prior to calling Begin() to specify width. Read code in imgui_demo in the "Horizontal Scrolling" section.<br/>
    ///</summary>
    ImGuiWindowFlags_HorizontalScrollbar = 1 << 11,
    ///<summary>
    /// Disable taking focus when transitioning from hidden to visible state<br/>
    ///</summary>
    ImGuiWindowFlags_NoFocusOnAppearing = 1 << 12,
    ///<summary>
    /// Disable bringing window to front when taking focus (e.g. clicking on it or programmatically giving it focus)<br/>
    ///</summary>
    ImGuiWindowFlags_NoBringToFrontOnFocus = 1 << 13,
    ///<summary>
    /// Always show vertical scrollbar (even if ContentSize.y &lt; Size.y)<br/>
    ///</summary>
    ImGuiWindowFlags_AlwaysVerticalScrollbar = 1 << 14,
    ///<summary>
    /// Always show horizontal scrollbar (even if ContentSize.x &lt; Size.x)<br/>
    ///</summary>
    ImGuiWindowFlags_AlwaysHorizontalScrollbar = 1 << 15,
    ///<summary>
    /// No keyboard/gamepad navigation within the window<br/>
    ///</summary>
    ImGuiWindowFlags_NoNavInputs = 1 << 16,
    ///<summary>
    /// No focusing toward this window with keyboard/gamepad navigation (e.g. skipped by Ctrl+Tab)<br/>
    ///</summary>
    ImGuiWindowFlags_NoNavFocus = 1 << 17,
    ///<summary>
    /// Display a dot next to the title. When used in a tab/docking context, tab is selected when clicking the X + closure is not assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.<br/>
    ///</summary>
    ImGuiWindowFlags_UnsavedDocument = 1 << 18,
    ///<summary>
    /// Disable docking of this window<br/>
    ///</summary>
    ImGuiWindowFlags_NoDocking = 1 << 19,
    ImGuiWindowFlags_NoNav = ImGuiWindowFlags_NoNavInputs | ImGuiWindowFlags_NoNavFocus,
    ImGuiWindowFlags_NoDecoration = ImGuiWindowFlags_NoTitleBar | ImGuiWindowFlags_NoResize | ImGuiWindowFlags_NoScrollbar | ImGuiWindowFlags_NoCollapse,
    ImGuiWindowFlags_NoInputs = ImGuiWindowFlags_NoMouseInputs | ImGuiWindowFlags_NoNavInputs | ImGuiWindowFlags_NoNavFocus,
    ///<summary>
    /// Don't use! For internal use by Begin()/NewFrame()<br/>
    ///<br/>
    /// [Internal]<br/>
    ///</summary>
    ImGuiWindowFlags_DockNodeHost = 1 << 23,
    ///<summary>
    /// Don't use! For internal use by BeginChild()<br/>
    ///</summary>
    ImGuiWindowFlags_ChildWindow = 1 << 24,
    ///<summary>
    /// Don't use! For internal use by BeginTooltip()<br/>
    ///</summary>
    ImGuiWindowFlags_Tooltip = 1 << 25,
    ///<summary>
    /// Don't use! For internal use by BeginPopup()<br/>
    ///</summary>
    ImGuiWindowFlags_Popup = 1 << 26,
    ///<summary>
    /// Don't use! For internal use by BeginPopupModal()<br/>
    ///</summary>
    ImGuiWindowFlags_Modal = 1 << 27,
    ///<summary>
    /// Don't use! For internal use by BeginMenu()<br/>
    ///</summary>
    ImGuiWindowFlags_ChildMenu = 1 << 28
}

///<summary>
/// Flags for ImGui::BeginChild()<br/>
/// (Legacy: bit 0 must always correspond to ImGuiChildFlags_Borders to be backward compatible with old API using 'bool border = false'.)<br/>
/// About using AutoResizeX/AutoResizeY flags:<br/>
/// - May be combined with SetNextWindowSizeConstraints() to set a min/max size for each axis (see "Demo-&gt;Child-&gt;Auto-resize with Constraints").<br/>
/// - Size measurement for a given axis is only performed when the child window is within visible boundaries, or is just appearing.<br/>
///   - This allows BeginChild() to return false when not within boundaries (e.g. when scrolling), which is more optimal. BUT it won't update its auto-size while clipped.<br/>
///     While not perfect, it is a better default behavior as the always-on performance gain is more valuable than the occasional "resizing after becoming visible again" glitch.<br/>
///   - You may also use ImGuiChildFlags_AlwaysAutoResize to force an update even when child window is not in view.<br/>
///     HOWEVER PLEASE UNDERSTAND THAT DOING SO WILL PREVENT BeginChild() FROM EVER RETURNING FALSE, disabling benefits of coarse clipping.<br/>
///</summary>
public enum ImGuiChildFlags
{
    ImGuiChildFlags_None = 0,
    ///<summary>
    /// Show an outer border and enable WindowPadding. (IMPORTANT: this is always == 1 == true for legacy reason)<br/>
    ///</summary>
    ImGuiChildFlags_Borders = 1 << 0,
    ///<summary>
    /// Pad with style.WindowPadding even if no border are drawn (no padding by default for non-bordered child windows because it makes more sense)<br/>
    ///</summary>
    ImGuiChildFlags_AlwaysUseWindowPadding = 1 << 1,
    ///<summary>
    /// Allow resize from right border (layout direction). Enable .ini saving (unless ImGuiWindowFlags_NoSavedSettings passed to window flags)<br/>
    ///</summary>
    ImGuiChildFlags_ResizeX = 1 << 2,
    ///<summary>
    /// Allow resize from bottom border (layout direction). "<br/>
    ///</summary>
    ImGuiChildFlags_ResizeY = 1 << 3,
    ///<summary>
    /// Enable auto-resizing width. Read "IMPORTANT: Size measurement" details above.<br/>
    ///</summary>
    ImGuiChildFlags_AutoResizeX = 1 << 4,
    ///<summary>
    /// Enable auto-resizing height. Read "IMPORTANT: Size measurement" details above.<br/>
    ///</summary>
    ImGuiChildFlags_AutoResizeY = 1 << 5,
    ///<summary>
    /// Combined with AutoResizeX/AutoResizeY. Always measure size even when child is hidden, always return true, always disable clipping optimization! NOT RECOMMENDED.<br/>
    ///</summary>
    ImGuiChildFlags_AlwaysAutoResize = 1 << 6,
    ///<summary>
    /// Style the child window like a framed item: use FrameBg, FrameRounding, FrameBorderSize, FramePadding instead of ChildBg, ChildRounding, ChildBorderSize, WindowPadding.<br/>
    ///</summary>
    ImGuiChildFlags_FrameStyle = 1 << 7,
    ///<summary>
    /// [BETA] Share focus scope, allow keyboard/gamepad navigation to cross over parent border to this child or between sibling child windows.<br/>
    ///</summary>
    ImGuiChildFlags_NavFlattened = 1 << 8
}

///<summary>
/// Flags for ImGui::PushItemFlag()<br/>
/// (Those are shared by all items)<br/>
///</summary>
public enum ImGuiItemFlags
{
    ///<summary>
    /// (Default)<br/>
    ///</summary>
    ImGuiItemFlags_None = 0,
    ///<summary>
    /// false     Disable keyboard tabbing. This is a "lighter" version of ImGuiItemFlags_NoNav.<br/>
    ///</summary>
    ImGuiItemFlags_NoTabStop = 1 << 0,
    ///<summary>
    /// false     Disable any form of focusing (keyboard/gamepad directional navigation and SetKeyboardFocusHere() calls).<br/>
    ///</summary>
    ImGuiItemFlags_NoNav = 1 << 1,
    ///<summary>
    /// false     Disable item being a candidate for default focus (e.g. used by title bar items).<br/>
    ///</summary>
    ImGuiItemFlags_NoNavDefaultFocus = 1 << 2,
    ///<summary>
    /// false     Any button-like behavior will have repeat mode enabled (based on io.KeyRepeatDelay and io.KeyRepeatRate values). Note that you can also call IsItemActive() after any button to tell if it is being held.<br/>
    ///</summary>
    ImGuiItemFlags_ButtonRepeat = 1 << 3,
    ///<summary>
    /// true      MenuItem()/Selectable() automatically close their parent popup window.<br/>
    ///</summary>
    ImGuiItemFlags_AutoClosePopups = 1 << 4,
    ///<summary>
    /// false     Allow submitting an item with the same identifier as an item already submitted this frame without triggering a warning tooltip if io.ConfigDebugHighlightIdConflicts is set.<br/>
    ///</summary>
    ImGuiItemFlags_AllowDuplicateId = 1 << 5
}

///<summary>
/// Flags for ImGui::InputText()<br/>
/// (Those are per-item flags. There are shared flags in ImGuiIO: io.ConfigInputTextCursorBlink and io.ConfigInputTextEnterKeepActive)<br/>
///</summary>
public enum ImGuiInputTextFlags
{
    ///<summary>
    /// Basic filters (also see ImGuiInputTextFlags_CallbackCharFilter)<br/>
    ///</summary>
    ImGuiInputTextFlags_None = 0,
    ///<summary>
    /// Allow 0123456789.+-*/<br/>
    ///</summary>
    ImGuiInputTextFlags_CharsDecimal = 1 << 0,
    ///<summary>
    /// Allow 0123456789ABCDEFabcdef<br/>
    ///</summary>
    ImGuiInputTextFlags_CharsHexadecimal = 1 << 1,
    ///<summary>
    /// Allow 0123456789.+-*/eE (Scientific notation input)<br/>
    ///</summary>
    ImGuiInputTextFlags_CharsScientific = 1 << 2,
    ///<summary>
    /// Turn a..z into A..Z<br/>
    ///</summary>
    ImGuiInputTextFlags_CharsUppercase = 1 << 3,
    ///<summary>
    /// Filter out spaces, tabs<br/>
    ///</summary>
    ImGuiInputTextFlags_CharsNoBlank = 1 << 4,
    ///<summary>
    /// Pressing TAB input a '\t' character into the text field<br/>
    ///<br/>
    /// Inputs<br/>
    ///</summary>
    ImGuiInputTextFlags_AllowTabInput = 1 << 5,
    ///<summary>
    /// Return 'true' when Enter is pressed (as opposed to every time the value was modified). Consider using IsItemDeactivatedAfterEdit() instead!<br/>
    ///</summary>
    ImGuiInputTextFlags_EnterReturnsTrue = 1 << 6,
    ///<summary>
    /// Escape key clears content if not empty, and deactivate otherwise (contrast to default behavior of Escape to revert)<br/>
    ///</summary>
    ImGuiInputTextFlags_EscapeClearsAll = 1 << 7,
    ///<summary>
    /// In multi-line mode, validate with Enter, add new line with Ctrl+Enter (default is opposite: validate with Ctrl+Enter, add line with Enter).<br/>
    ///</summary>
    ImGuiInputTextFlags_CtrlEnterForNewLine = 1 << 8,
    ///<summary>
    /// Read-only mode<br/>
    ///<br/>
    /// Other options<br/>
    ///</summary>
    ImGuiInputTextFlags_ReadOnly = 1 << 9,
    ///<summary>
    /// Password mode, display all characters as '*', disable copy<br/>
    ///</summary>
    ImGuiInputTextFlags_Password = 1 << 10,
    ///<summary>
    /// Overwrite mode<br/>
    ///</summary>
    ImGuiInputTextFlags_AlwaysOverwrite = 1 << 11,
    ///<summary>
    /// Select entire text when first taking mouse focus<br/>
    ///</summary>
    ImGuiInputTextFlags_AutoSelectAll = 1 << 12,
    ///<summary>
    /// InputFloat(), InputInt(), InputScalar() etc. only: parse empty string as zero value.<br/>
    ///</summary>
    ImGuiInputTextFlags_ParseEmptyRefVal = 1 << 13,
    ///<summary>
    /// InputFloat(), InputInt(), InputScalar() etc. only: when value is zero, do not display it. Generally used with ImGuiInputTextFlags_ParseEmptyRefVal.<br/>
    ///</summary>
    ImGuiInputTextFlags_DisplayEmptyRefVal = 1 << 14,
    ///<summary>
    /// Disable following the cursor horizontally<br/>
    ///</summary>
    ImGuiInputTextFlags_NoHorizontalScroll = 1 << 15,
    ///<summary>
    /// Disable undo/redo. Note that input text owns the text data while active, if you want to provide your own undo/redo stack you need e.g. to call ClearActiveID().<br/>
    ///</summary>
    ImGuiInputTextFlags_NoUndoRedo = 1 << 16,
    ///<summary>
    /// When text doesn't fit, elide left side to ensure right side stays visible. Useful for path/filenames. Single-line only!<br/>
    ///<br/>
    /// Elide display / Alignment<br/>
    ///</summary>
    ImGuiInputTextFlags_ElideLeft = 1 << 17,
    ///<summary>
    /// Callback on pressing TAB (for completion handling)<br/>
    ///<br/>
    /// Callback features<br/>
    ///</summary>
    ImGuiInputTextFlags_CallbackCompletion = 1 << 18,
    ///<summary>
    /// Callback on pressing Up/Down arrows (for history handling)<br/>
    ///</summary>
    ImGuiInputTextFlags_CallbackHistory = 1 << 19,
    ///<summary>
    /// Callback on each iteration. User code may query cursor position, modify text buffer.<br/>
    ///</summary>
    ImGuiInputTextFlags_CallbackAlways = 1 << 20,
    ///<summary>
    /// Callback on character inputs to replace or discard them. Modify 'EventChar' to replace or discard, or return 1 in callback to discard.<br/>
    ///</summary>
    ImGuiInputTextFlags_CallbackCharFilter = 1 << 21,
    ///<summary>
    /// Callback on buffer capacity changes request (beyond 'buf_size' parameter value), allowing the string to grow. Notify when the string wants to be resized (for string types which hold a cache of their Size). You will be provided a new BufSize in the callback and NEED to honor it. (see misc/cpp/imgui_stdlib.h for an example of using this)<br/>
    ///</summary>
    ImGuiInputTextFlags_CallbackResize = 1 << 22,
    ///<summary>
    /// Callback on any edit. Note that InputText() already returns true on edit + you can always use IsItemEdited(). The callback is useful to manipulate the underlying buffer while focus is active.<br/>
    ///</summary>
    ImGuiInputTextFlags_CallbackEdit = 1 << 23,
    ///<summary>
    /// InputTextMultiline(): word-wrap lines that are too long.<br/>
    ///<br/>
    /// Multi-line Word-Wrapping [BETA]<br/>
    /// - Not well tested yet. Please report any incorrect cursor movement, selection behavior etc. bug to https:github.com/ocornut/imgui/issues/3237.<br/>
    /// - Wrapping style is not ideal. Wrapping of long words/sections (e.g. words larger than total available width) may be particularly unpleasing.<br/>
    /// - Wrapping width needs to always account for the possibility of a vertical scrollbar.<br/>
    /// - It is much slower than regular text fields.<br/>
    ///   Ballpark estimate of cost on my 2019 desktop PC: for a 100 KB text buffer: +~0.3 ms (Optimized) / +~1.0 ms (Debug build).<br/>
    ///   The CPU cost is very roughly proportional to text length, so a 10 KB buffer should cost about ten times less.<br/>
    ///</summary>
    ImGuiInputTextFlags_WordWrap = 1 << 24
}

///<summary>
/// Flags for ImGui::TreeNodeEx(), ImGui::CollapsingHeader*()<br/>
///</summary>
public enum ImGuiTreeNodeFlags
{
    ImGuiTreeNodeFlags_None = 0,
    ///<summary>
    /// Draw as selected<br/>
    ///</summary>
    ImGuiTreeNodeFlags_Selected = 1 << 0,
    ///<summary>
    /// Draw frame with background (e.g. for CollapsingHeader)<br/>
    ///</summary>
    ImGuiTreeNodeFlags_Framed = 1 << 1,
    ///<summary>
    /// Hit testing to allow subsequent widgets to overlap this one<br/>
    ///</summary>
    ImGuiTreeNodeFlags_AllowOverlap = 1 << 2,
    ///<summary>
    /// Don't do a TreePush() when open (e.g. for CollapsingHeader) = no extra indent nor pushing on ID stack<br/>
    ///</summary>
    ImGuiTreeNodeFlags_NoTreePushOnOpen = 1 << 3,
    ///<summary>
    /// Don't automatically and temporarily open node when Logging is active (by default logging will automatically open tree nodes)<br/>
    ///</summary>
    ImGuiTreeNodeFlags_NoAutoOpenOnLog = 1 << 4,
    ///<summary>
    /// Default node to be open<br/>
    ///</summary>
    ImGuiTreeNodeFlags_DefaultOpen = 1 << 5,
    ///<summary>
    /// Open on double-click instead of simple click (default for multi-select unless any _OpenOnXXX behavior is set explicitly). Both behaviors may be combined.<br/>
    ///</summary>
    ImGuiTreeNodeFlags_OpenOnDoubleClick = 1 << 6,
    ///<summary>
    /// Open when clicking on the arrow part (default for multi-select unless any _OpenOnXXX behavior is set explicitly). Both behaviors may be combined.<br/>
    ///</summary>
    ImGuiTreeNodeFlags_OpenOnArrow = 1 << 7,
    ///<summary>
    /// No collapsing, no arrow (use as a convenience for leaf nodes).<br/>
    ///</summary>
    ImGuiTreeNodeFlags_Leaf = 1 << 8,
    ///<summary>
    /// Display a bullet instead of arrow. IMPORTANT: node can still be marked open/close if you don't set the _Leaf flag!<br/>
    ///</summary>
    ImGuiTreeNodeFlags_Bullet = 1 << 9,
    ///<summary>
    /// Use FramePadding (even for an unframed text node) to vertically align text baseline to regular widget height. Equivalent to calling AlignTextToFramePadding() before the node.<br/>
    ///</summary>
    ImGuiTreeNodeFlags_FramePadding = 1 << 10,
    ///<summary>
    /// Extend hit box to the right-most edge, even if not framed. This is not the default in order to allow adding other items on the same line without using AllowOverlap mode.<br/>
    ///</summary>
    ImGuiTreeNodeFlags_SpanAvailWidth = 1 << 11,
    ///<summary>
    /// Extend hit box to the left-most and right-most edges (cover the indent area).<br/>
    ///</summary>
    ImGuiTreeNodeFlags_SpanFullWidth = 1 << 12,
    ///<summary>
    /// Narrow hit box + narrow hovering highlight, will only cover the label text.<br/>
    ///</summary>
    ImGuiTreeNodeFlags_SpanLabelWidth = 1 << 13,
    ///<summary>
    /// Frame will span all columns of its container table (label will still fit in current column)<br/>
    ///</summary>
    ImGuiTreeNodeFlags_SpanAllColumns = 1 << 14,
    ///<summary>
    /// Label will span all columns of its container table<br/>
    ///</summary>
    ImGuiTreeNodeFlags_LabelSpanAllColumns = 1 << 15,
    ///<summary>
    /// Nav: left arrow moves back to parent. This is processed in TreePop() when there's an unfulfilled Left nav request remaining.<br/>
    ///<br/>
    ///ImGuiTreeNodeFlags_NoScrollOnOpen     = 1 &lt;&lt; 16,   FIXME: TODO: Disable automatic scroll on TreePop() if node got just open and contents is not visible<br/>
    ///</summary>
    ImGuiTreeNodeFlags_NavLeftJumpsToParent = 1 << 17,
    ImGuiTreeNodeFlags_CollapsingHeader = ImGuiTreeNodeFlags_Framed | ImGuiTreeNodeFlags_NoTreePushOnOpen | ImGuiTreeNodeFlags_NoAutoOpenOnLog,
    ///<summary>
    /// No lines drawn<br/>
    ///<br/>
    /// [EXPERIMENTAL] Draw lines connecting TreeNode hierarchy. Discuss in GitHub issue #2920.<br/>
    /// Default value is pulled from style.TreeLinesFlags. May be overridden in TreeNode calls.<br/>
    ///</summary>
    ImGuiTreeNodeFlags_DrawLinesNone = 1 << 18,
    ///<summary>
    /// Horizontal lines to child nodes. Vertical line drawn down to TreePop() position: cover full contents. Faster (for large trees).<br/>
    ///</summary>
    ImGuiTreeNodeFlags_DrawLinesFull = 1 << 19,
    ///<summary>
    /// Horizontal lines to child nodes. Vertical line drawn down to bottom-most child node. Slower (for large trees).<br/>
    ///</summary>
    ImGuiTreeNodeFlags_DrawLinesToNodes = 1 << 20,
    ///<summary>
    /// Renamed in 1.92.0<br/>
    ///</summary>
    ImGuiTreeNodeFlags_NavLeftJumpsBackHere = ImGuiTreeNodeFlags_NavLeftJumpsToParent,
    ///<summary>
    /// Renamed in 1.90.7<br/>
    ///</summary>
    ImGuiTreeNodeFlags_SpanTextWidth = ImGuiTreeNodeFlags_SpanLabelWidth
}

///<summary>
/// Flags for OpenPopup*(), BeginPopupContext*(), IsPopupOpen() functions.<br/>
/// - To be backward compatible with older API which took an 'int mouse_button = 1' argument instead of 'ImGuiPopupFlags flags',<br/>
///   we need to treat small flags values as a mouse button index, so we encode the mouse button in the first few bits of the flags.<br/>
///   It is therefore guaranteed to be legal to pass a mouse button index in ImGuiPopupFlags.<br/>
/// - For the same reason, we exceptionally default the ImGuiPopupFlags argument of BeginPopupContextXXX functions to 1 instead of 0.<br/>
///   IMPORTANT: because the default parameter is 1 (==ImGuiPopupFlags_MouseButtonRight), if you rely on the default parameter<br/>
///   and want to use another flag, you need to pass in the ImGuiPopupFlags_MouseButtonRight flag explicitly.<br/>
/// - Multiple buttons currently cannot be combined/or-ed in those functions (we could allow it later).<br/>
///</summary>
public enum ImGuiPopupFlags
{
    ImGuiPopupFlags_None = 0,
    ///<summary>
    /// For BeginPopupContext*(): open on Left Mouse release. Guaranteed to always be == 0 (same as ImGuiMouseButton_Left)<br/>
    ///</summary>
    ImGuiPopupFlags_MouseButtonLeft = 0,
    ///<summary>
    /// For BeginPopupContext*(): open on Right Mouse release. Guaranteed to always be == 1 (same as ImGuiMouseButton_Right)<br/>
    ///</summary>
    ImGuiPopupFlags_MouseButtonRight = 1,
    ///<summary>
    /// For BeginPopupContext*(): open on Middle Mouse release. Guaranteed to always be == 2 (same as ImGuiMouseButton_Middle)<br/>
    ///</summary>
    ImGuiPopupFlags_MouseButtonMiddle = 2,
    ImGuiPopupFlags_MouseButtonMask_ = 0x1F,
    ImGuiPopupFlags_MouseButtonDefault_ = 1,
    ///<summary>
    /// For OpenPopup*(), BeginPopupContext*(): don't reopen same popup if already open (won't reposition, won't reinitialize navigation)<br/>
    ///</summary>
    ImGuiPopupFlags_NoReopen = 1 << 5,
    ///<summary>
    /// For OpenPopup*(), BeginPopupContext*(): don't open if there's already a popup at the same level of the popup stack<br/>
    ///<br/>
    ///ImGuiPopupFlags_NoReopenAlwaysNavInit = 1 &lt;&lt; 6,    For OpenPopup*(), BeginPopupContext*(): focus and initialize navigation even when not reopening.<br/>
    ///</summary>
    ImGuiPopupFlags_NoOpenOverExistingPopup = 1 << 7,
    ///<summary>
    /// For BeginPopupContextWindow(): don't return true when hovering items, only when hovering empty space<br/>
    ///</summary>
    ImGuiPopupFlags_NoOpenOverItems = 1 << 8,
    ///<summary>
    /// For IsPopupOpen(): ignore the ImGuiID parameter and test for any popup.<br/>
    ///</summary>
    ImGuiPopupFlags_AnyPopupId = 1 << 10,
    ///<summary>
    /// For IsPopupOpen(): search/test at any level of the popup stack (default test in the current level)<br/>
    ///</summary>
    ImGuiPopupFlags_AnyPopupLevel = 1 << 11,
    ImGuiPopupFlags_AnyPopup = ImGuiPopupFlags_AnyPopupId | ImGuiPopupFlags_AnyPopupLevel
}

///<summary>
/// Flags for ImGui::Selectable()<br/>
///</summary>
public enum ImGuiSelectableFlags
{
    ImGuiSelectableFlags_None = 0,
    ///<summary>
    /// Clicking this doesn't close parent popup window (overrides ImGuiItemFlags_AutoClosePopups)<br/>
    ///</summary>
    ImGuiSelectableFlags_NoAutoClosePopups = 1 << 0,
    ///<summary>
    /// Frame will span all columns of its container table (text will still fit in current column)<br/>
    ///</summary>
    ImGuiSelectableFlags_SpanAllColumns = 1 << 1,
    ///<summary>
    /// Generate press events on double clicks too<br/>
    ///</summary>
    ImGuiSelectableFlags_AllowDoubleClick = 1 << 2,
    ///<summary>
    /// Cannot be selected, display grayed out text<br/>
    ///</summary>
    ImGuiSelectableFlags_Disabled = 1 << 3,
    ///<summary>
    /// (WIP) Hit testing to allow subsequent widgets to overlap this one<br/>
    ///</summary>
    ImGuiSelectableFlags_AllowOverlap = 1 << 4,
    ///<summary>
    /// Make the item be displayed as if it is hovered<br/>
    ///</summary>
    ImGuiSelectableFlags_Highlight = 1 << 5,
    ///<summary>
    /// Auto-select when moved into, unless Ctrl is held. Automatic when in a BeginMultiSelect() block.<br/>
    ///</summary>
    ImGuiSelectableFlags_SelectOnNav = 1 << 6,
    ///<summary>
    /// Renamed in 1.91.0<br/>
    ///</summary>
    ImGuiSelectableFlags_DontClosePopups = ImGuiSelectableFlags_NoAutoClosePopups
}

///<summary>
/// Flags for ImGui::BeginCombo()<br/>
///</summary>
public enum ImGuiComboFlags
{
    ImGuiComboFlags_None = 0,
    ///<summary>
    /// Align the popup toward the left by default<br/>
    ///</summary>
    ImGuiComboFlags_PopupAlignLeft = 1 << 0,
    ///<summary>
    /// Max ~4 items visible. Tip: If you want your combo popup to be a specific size you can use SetNextWindowSizeConstraints() prior to calling BeginCombo()<br/>
    ///</summary>
    ImGuiComboFlags_HeightSmall = 1 << 1,
    ///<summary>
    /// Max ~8 items visible (default)<br/>
    ///</summary>
    ImGuiComboFlags_HeightRegular = 1 << 2,
    ///<summary>
    /// Max ~20 items visible<br/>
    ///</summary>
    ImGuiComboFlags_HeightLarge = 1 << 3,
    ///<summary>
    /// As many fitting items as possible<br/>
    ///</summary>
    ImGuiComboFlags_HeightLargest = 1 << 4,
    ///<summary>
    /// Display on the preview box without the square arrow button<br/>
    ///</summary>
    ImGuiComboFlags_NoArrowButton = 1 << 5,
    ///<summary>
    /// Display only a square arrow button<br/>
    ///</summary>
    ImGuiComboFlags_NoPreview = 1 << 6,
    ///<summary>
    /// Width dynamically calculated from preview contents<br/>
    ///</summary>
    ImGuiComboFlags_WidthFitPreview = 1 << 7,
    ImGuiComboFlags_HeightMask_ = ImGuiComboFlags_HeightSmall | ImGuiComboFlags_HeightRegular | ImGuiComboFlags_HeightLarge | ImGuiComboFlags_HeightLargest
}

///<summary>
/// Flags for ImGui::BeginTabBar()<br/>
///</summary>
public enum ImGuiTabBarFlags
{
    ImGuiTabBarFlags_None = 0,
    ///<summary>
    /// Allow manually dragging tabs to re-order them + New tabs are appended at the end of list<br/>
    ///</summary>
    ImGuiTabBarFlags_Reorderable = 1 << 0,
    ///<summary>
    /// Automatically select new tabs when they appear<br/>
    ///</summary>
    ImGuiTabBarFlags_AutoSelectNewTabs = 1 << 1,
    ///<summary>
    /// Disable buttons to open the tab list popup<br/>
    ///</summary>
    ImGuiTabBarFlags_TabListPopupButton = 1 << 2,
    ///<summary>
    /// Disable behavior of closing tabs (that are submitted with p_open != NULL) with middle mouse button. You may handle this behavior manually on user's side with if (IsItemHovered() &amp;&amp; IsMouseClicked(2)) *p_open = false.<br/>
    ///</summary>
    ImGuiTabBarFlags_NoCloseWithMiddleMouseButton = 1 << 3,
    ///<summary>
    /// Disable scrolling buttons (apply when fitting policy is ImGuiTabBarFlags_FittingPolicyScroll)<br/>
    ///</summary>
    ImGuiTabBarFlags_NoTabListScrollingButtons = 1 << 4,
    ///<summary>
    /// Disable tooltips when hovering a tab<br/>
    ///</summary>
    ImGuiTabBarFlags_NoTooltip = 1 << 5,
    ///<summary>
    /// Draw selected overline markers over selected tab<br/>
    ///</summary>
    ImGuiTabBarFlags_DrawSelectedOverline = 1 << 6,
    ///<summary>
    /// Shrink down tabs when they don't fit, until width is style.TabMinWidthShrink, then enable scrolling buttons.<br/>
    ///<br/>
    /// Fitting/Resize policy<br/>
    ///</summary>
    ImGuiTabBarFlags_FittingPolicyMixed = 1 << 7,
    ///<summary>
    /// Shrink down tabs when they don't fit<br/>
    ///</summary>
    ImGuiTabBarFlags_FittingPolicyShrink = 1 << 8,
    ///<summary>
    /// Enable scrolling buttons when tabs don't fit<br/>
    ///</summary>
    ImGuiTabBarFlags_FittingPolicyScroll = 1 << 9,
    ImGuiTabBarFlags_FittingPolicyMask_ = ImGuiTabBarFlags_FittingPolicyMixed | ImGuiTabBarFlags_FittingPolicyShrink | ImGuiTabBarFlags_FittingPolicyScroll,
    ImGuiTabBarFlags_FittingPolicyDefault_ = ImGuiTabBarFlags_FittingPolicyMixed,
    ///<summary>
    /// Renamed in 1.92.2<br/>
    ///</summary>
    ImGuiTabBarFlags_FittingPolicyResizeDown = ImGuiTabBarFlags_FittingPolicyShrink
}

///<summary>
/// Flags for ImGui::BeginTabItem()<br/>
///</summary>
public enum ImGuiTabItemFlags
{
    ImGuiTabItemFlags_None = 0,
    ///<summary>
    /// Display a dot next to the title + set ImGuiTabItemFlags_NoAssumedClosure.<br/>
    ///</summary>
    ImGuiTabItemFlags_UnsavedDocument = 1 << 0,
    ///<summary>
    /// Trigger flag to programmatically make the tab selected when calling BeginTabItem()<br/>
    ///</summary>
    ImGuiTabItemFlags_SetSelected = 1 << 1,
    ///<summary>
    /// Disable behavior of closing tabs (that are submitted with p_open != NULL) with middle mouse button. You may handle this behavior manually on user's side with if (IsItemHovered() &amp;&amp; IsMouseClicked(2)) *p_open = false.<br/>
    ///</summary>
    ImGuiTabItemFlags_NoCloseWithMiddleMouseButton = 1 << 2,
    ///<summary>
    /// Don't call PushID()/PopID() on BeginTabItem()/EndTabItem()<br/>
    ///</summary>
    ImGuiTabItemFlags_NoPushId = 1 << 3,
    ///<summary>
    /// Disable tooltip for the given tab<br/>
    ///</summary>
    ImGuiTabItemFlags_NoTooltip = 1 << 4,
    ///<summary>
    /// Disable reordering this tab or having another tab cross over this tab<br/>
    ///</summary>
    ImGuiTabItemFlags_NoReorder = 1 << 5,
    ///<summary>
    /// Enforce the tab position to the left of the tab bar (after the tab list popup button)<br/>
    ///</summary>
    ImGuiTabItemFlags_Leading = 1 << 6,
    ///<summary>
    /// Enforce the tab position to the right of the tab bar (before the scrolling buttons)<br/>
    ///</summary>
    ImGuiTabItemFlags_Trailing = 1 << 7,
    ///<summary>
    /// Tab is selected when trying to close + closure is not immediately assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.<br/>
    ///</summary>
    ImGuiTabItemFlags_NoAssumedClosure = 1 << 8
}

///<summary>
/// Flags for ImGui::IsWindowFocused()<br/>
///</summary>
public enum ImGuiFocusedFlags
{
    ImGuiFocusedFlags_None = 0,
    ///<summary>
    /// Return true if any children of the window is focused<br/>
    ///</summary>
    ImGuiFocusedFlags_ChildWindows = 1 << 0,
    ///<summary>
    /// Test from root window (top most parent of the current hierarchy)<br/>
    ///</summary>
    ImGuiFocusedFlags_RootWindow = 1 << 1,
    ///<summary>
    /// Return true if any window is focused. Important: If you are trying to tell how to dispatch your low-level inputs, do NOT use this. Use 'io.WantCaptureMouse' instead! Please read the FAQ!<br/>
    ///</summary>
    ImGuiFocusedFlags_AnyWindow = 1 << 2,
    ///<summary>
    /// Do not consider popup hierarchy (do not treat popup emitter as parent of popup) (when used with _ChildWindows or _RootWindow)<br/>
    ///</summary>
    ImGuiFocusedFlags_NoPopupHierarchy = 1 << 3,
    ///<summary>
    /// Consider docking hierarchy (treat dockspace host as parent of docked window) (when used with _ChildWindows or _RootWindow)<br/>
    ///</summary>
    ImGuiFocusedFlags_DockHierarchy = 1 << 4,
    ImGuiFocusedFlags_RootAndChildWindows = ImGuiFocusedFlags_RootWindow | ImGuiFocusedFlags_ChildWindows
}

///<summary>
/// Flags for ImGui::IsItemHovered(), ImGui::IsWindowHovered()<br/>
/// Note: if you are trying to check whether your mouse should be dispatched to Dear ImGui or to your app, you should use 'io.WantCaptureMouse' instead! Please read the FAQ!<br/>
/// Note: windows with the ImGuiWindowFlags_NoInputs flag are ignored by IsWindowHovered() calls.<br/>
///</summary>
public enum ImGuiHoveredFlags
{
    ///<summary>
    /// Return true if directly over the item/window, not obstructed by another window, not obstructed by an active popup or modal blocking inputs under them.<br/>
    ///</summary>
    ImGuiHoveredFlags_None = 0,
    ///<summary>
    /// IsWindowHovered() only: Return true if any children of the window is hovered<br/>
    ///</summary>
    ImGuiHoveredFlags_ChildWindows = 1 << 0,
    ///<summary>
    /// IsWindowHovered() only: Test from root window (top most parent of the current hierarchy)<br/>
    ///</summary>
    ImGuiHoveredFlags_RootWindow = 1 << 1,
    ///<summary>
    /// IsWindowHovered() only: Return true if any window is hovered<br/>
    ///</summary>
    ImGuiHoveredFlags_AnyWindow = 1 << 2,
    ///<summary>
    /// IsWindowHovered() only: Do not consider popup hierarchy (do not treat popup emitter as parent of popup) (when used with _ChildWindows or _RootWindow)<br/>
    ///</summary>
    ImGuiHoveredFlags_NoPopupHierarchy = 1 << 3,
    ///<summary>
    /// IsWindowHovered() only: Consider docking hierarchy (treat dockspace host as parent of docked window) (when used with _ChildWindows or _RootWindow)<br/>
    ///</summary>
    ImGuiHoveredFlags_DockHierarchy = 1 << 4,
    ///<summary>
    /// Return true even if a popup window is normally blocking access to this item/window<br/>
    ///</summary>
    ImGuiHoveredFlags_AllowWhenBlockedByPopup = 1 << 5,
    ///<summary>
    /// Return true even if an active item is blocking access to this item/window. Useful for Drag and Drop patterns.<br/>
    ///<br/>
    ///ImGuiHoveredFlags_AllowWhenBlockedByModal     = 1 &lt;&lt; 6,    Return true even if a modal popup window is normally blocking access to this item/window. FIXME-TODO: Unavailable yet.<br/>
    ///</summary>
    ImGuiHoveredFlags_AllowWhenBlockedByActiveItem = 1 << 7,
    ///<summary>
    /// IsItemHovered() only: Return true even if the item uses AllowOverlap mode and is overlapped by another hoverable item.<br/>
    ///</summary>
    ImGuiHoveredFlags_AllowWhenOverlappedByItem = 1 << 8,
    ///<summary>
    /// IsItemHovered() only: Return true even if the position is obstructed or overlapped by another window.<br/>
    ///</summary>
    ImGuiHoveredFlags_AllowWhenOverlappedByWindow = 1 << 9,
    ///<summary>
    /// IsItemHovered() only: Return true even if the item is disabled<br/>
    ///</summary>
    ImGuiHoveredFlags_AllowWhenDisabled = 1 << 10,
    ///<summary>
    /// IsItemHovered() only: Disable using keyboard/gamepad navigation state when active, always query mouse<br/>
    ///</summary>
    ImGuiHoveredFlags_NoNavOverride = 1 << 11,
    ImGuiHoveredFlags_AllowWhenOverlapped = ImGuiHoveredFlags_AllowWhenOverlappedByItem | ImGuiHoveredFlags_AllowWhenOverlappedByWindow,
    ImGuiHoveredFlags_RectOnly = ImGuiHoveredFlags_AllowWhenBlockedByPopup | ImGuiHoveredFlags_AllowWhenBlockedByActiveItem | ImGuiHoveredFlags_AllowWhenOverlapped,
    ImGuiHoveredFlags_RootAndChildWindows = ImGuiHoveredFlags_RootWindow | ImGuiHoveredFlags_ChildWindows,
    ///<summary>
    /// Shortcut for standard flags when using IsItemHovered() + SetTooltip() sequence.<br/>
    ///<br/>
    /// Tooltips mode<br/>
    /// - typically used in IsItemHovered() + SetTooltip() sequence.<br/>
    /// - this is a shortcut to pull flags from 'style.HoverFlagsForTooltipMouse' or 'style.HoverFlagsForTooltipNav' where you can reconfigure desired behavior.<br/>
    ///   e.g. 'HoverFlagsForTooltipMouse' defaults to 'ImGuiHoveredFlags_Stationary | ImGuiHoveredFlags_DelayShort | ImGuiHoveredFlags_AllowWhenDisabled'.<br/>
    /// - for frequently actioned or hovered items providing a tooltip, you want may to use ImGuiHoveredFlags_ForTooltip (stationary + delay) so the tooltip doesn't show too often.<br/>
    /// - for items which main purpose is to be hovered, or items with low affordance, or in less consistent apps, prefer no delay or shorter delay.<br/>
    ///</summary>
    ImGuiHoveredFlags_ForTooltip = 1 << 12,
    ///<summary>
    /// Require mouse to be stationary for style.HoverStationaryDelay (~0.15 sec) _at least one time_. After this, can move on same item/window. Using the stationary test tends to reduces the need for a long delay.<br/>
    ///<br/>
    /// (Advanced) Mouse Hovering delays.<br/>
    /// - generally you can use ImGuiHoveredFlags_ForTooltip to use application-standardized flags.<br/>
    /// - use those if you need specific overrides.<br/>
    ///</summary>
    ImGuiHoveredFlags_Stationary = 1 << 13,
    ///<summary>
    /// IsItemHovered() only: Return true immediately (default). As this is the default you generally ignore this.<br/>
    ///</summary>
    ImGuiHoveredFlags_DelayNone = 1 << 14,
    ///<summary>
    /// IsItemHovered() only: Return true after style.HoverDelayShort elapsed (~0.15 sec) (shared between items) + requires mouse to be stationary for style.HoverStationaryDelay (once per item).<br/>
    ///</summary>
    ImGuiHoveredFlags_DelayShort = 1 << 15,
    ///<summary>
    /// IsItemHovered() only: Return true after style.HoverDelayNormal elapsed (~0.40 sec) (shared between items) + requires mouse to be stationary for style.HoverStationaryDelay (once per item).<br/>
    ///</summary>
    ImGuiHoveredFlags_DelayNormal = 1 << 16,
    ///<summary>
    /// IsItemHovered() only: Disable shared delay system where moving from one item to the next keeps the previous timer for a short time (standard for tooltips with long delays)<br/>
    ///</summary>
    ImGuiHoveredFlags_NoSharedDelay = 1 << 17
}

///<summary>
/// Flags for ImGui::DockSpace(), shared/inherited by child nodes.<br/>
/// (Some flags can be applied to individual nodes directly)<br/>
/// FIXME-DOCK: Also see ImGuiDockNodeFlagsPrivate_ which may involve using the WIP and internal DockBuilder api.<br/>
///</summary>
public enum ImGuiDockNodeFlags
{
    ImGuiDockNodeFlags_None = 0,
    ///<summary>
    ///        Don't display the dockspace node but keep it alive. Windows docked into this dockspace node won't be undocked.<br/>
    ///</summary>
    ImGuiDockNodeFlags_KeepAliveOnly = 1 << 0,
    ///<summary>
    ///        Disable docking over the Central Node, which will be always kept empty.<br/>
    ///<br/>
    ///ImGuiDockNodeFlags_NoCentralNode              = 1 &lt;&lt; 1,           Disable Central Node (the node which can stay empty)<br/>
    ///</summary>
    ImGuiDockNodeFlags_NoDockingOverCentralNode = 1 << 2,
    ///<summary>
    ///        Enable passthru dockspace: 1) DockSpace() will render a ImGuiCol_WindowBg background covering everything excepted the Central Node when empty. Meaning the host window should probably use SetNextWindowBgAlpha(0.0f) prior to Begin() when using this. 2) When Central Node is empty: let inputs pass-through + won't display a DockingEmptyBg background. See demo for details.<br/>
    ///</summary>
    ImGuiDockNodeFlags_PassthruCentralNode = 1 << 3,
    ///<summary>
    ///        Disable other windows/nodes from splitting this node.<br/>
    ///</summary>
    ImGuiDockNodeFlags_NoDockingSplit = 1 << 4,
    ///<summary>
    /// Saved  Disable resizing node using the splitter/separators. Useful with programmatically setup dockspaces.<br/>
    ///</summary>
    ImGuiDockNodeFlags_NoResize = 1 << 5,
    ///<summary>
    ///        Tab bar will automatically hide when there is a single window in the dock node.<br/>
    ///</summary>
    ImGuiDockNodeFlags_AutoHideTabBar = 1 << 6,
    ///<summary>
    ///        Disable undocking this node.<br/>
    ///</summary>
    ImGuiDockNodeFlags_NoUndocking = 1 << 7,
    ///<summary>
    /// Renamed in 1.90<br/>
    ///</summary>
    ImGuiDockNodeFlags_NoSplit = ImGuiDockNodeFlags_NoDockingSplit,
    ///<summary>
    /// Renamed in 1.90<br/>
    ///</summary>
    ImGuiDockNodeFlags_NoDockingInCentralNode = ImGuiDockNodeFlags_NoDockingOverCentralNode
}

///<summary>
/// Flags for ImGui::BeginDragDropSource(), ImGui::AcceptDragDropPayload()<br/>
///</summary>
public enum ImGuiDragDropFlags
{
    ImGuiDragDropFlags_None = 0,
    ///<summary>
    /// Disable preview tooltip. By default, a successful call to BeginDragDropSource opens a tooltip so you can display a preview or description of the source contents. This flag disables this behavior.<br/>
    ///<br/>
    /// BeginDragDropSource() flags<br/>
    ///</summary>
    ImGuiDragDropFlags_SourceNoPreviewTooltip = 1 << 0,
    ///<summary>
    /// By default, when dragging we clear data so that IsItemHovered() will return false, to avoid subsequent user code submitting tooltips. This flag disables this behavior so you can still call IsItemHovered() on the source item.<br/>
    ///</summary>
    ImGuiDragDropFlags_SourceNoDisableHover = 1 << 1,
    ///<summary>
    /// Disable the behavior that allows to open tree nodes and collapsing header by holding over them while dragging a source item.<br/>
    ///</summary>
    ImGuiDragDropFlags_SourceNoHoldToOpenOthers = 1 << 2,
    ///<summary>
    /// Allow items such as Text(), Image() that have no unique identifier to be used as drag source, by manufacturing a temporary identifier based on their window-relative position. This is extremely unusual within the dear imgui ecosystem and so we made it explicit.<br/>
    ///</summary>
    ImGuiDragDropFlags_SourceAllowNullID = 1 << 3,
    ///<summary>
    /// External source (from outside of dear imgui), won't attempt to read current item/window info. Will always return true. Only one Extern source can be active simultaneously.<br/>
    ///</summary>
    ImGuiDragDropFlags_SourceExtern = 1 << 4,
    ///<summary>
    /// Automatically expire the payload if the source cease to be submitted (otherwise payloads are persisting while being dragged)<br/>
    ///</summary>
    ImGuiDragDropFlags_PayloadAutoExpire = 1 << 5,
    ///<summary>
    /// Hint to specify that the payload may not be copied outside current dear imgui context.<br/>
    ///</summary>
    ImGuiDragDropFlags_PayloadNoCrossContext = 1 << 6,
    ///<summary>
    /// Hint to specify that the payload may not be copied outside current process.<br/>
    ///</summary>
    ImGuiDragDropFlags_PayloadNoCrossProcess = 1 << 7,
    ///<summary>
    /// AcceptDragDropPayload() will returns true even before the mouse button is released. You can then call IsDelivery() to test if the payload needs to be delivered.<br/>
    ///<br/>
    /// AcceptDragDropPayload() flags<br/>
    ///</summary>
    ImGuiDragDropFlags_AcceptBeforeDelivery = 1 << 10,
    ///<summary>
    /// Do not draw the default highlight rectangle when hovering over target.<br/>
    ///</summary>
    ImGuiDragDropFlags_AcceptNoDrawDefaultRect = 1 << 11,
    ///<summary>
    /// Request hiding the BeginDragDropSource tooltip from the BeginDragDropTarget site.<br/>
    ///</summary>
    ImGuiDragDropFlags_AcceptNoPreviewTooltip = 1 << 12,
    ///<summary>
    /// Accepting item will render as if hovered. Useful for e.g. a Button() used as a drop target.<br/>
    ///</summary>
    ImGuiDragDropFlags_AcceptDrawAsHovered = 1 << 13,
    ///<summary>
    /// For peeking ahead and inspecting the payload before delivery.<br/>
    ///</summary>
    ImGuiDragDropFlags_AcceptPeekOnly = ImGuiDragDropFlags_AcceptBeforeDelivery | ImGuiDragDropFlags_AcceptNoDrawDefaultRect,
    ///<summary>
    /// Renamed in 1.90.9<br/>
    ///</summary>
    ImGuiDragDropFlags_SourceAutoExpirePayload = ImGuiDragDropFlags_PayloadAutoExpire
}

///<summary>
/// A primary data type<br/>
///</summary>
public enum ImGuiDataType
{
    ///<summary>
    /// signed char / char (with sensible compilers)<br/>
    ///</summary>
    ImGuiDataType_S8,
    ///<summary>
    /// unsigned char<br/>
    ///</summary>
    ImGuiDataType_U8,
    ///<summary>
    /// short<br/>
    ///</summary>
    ImGuiDataType_S16,
    ///<summary>
    /// unsigned short<br/>
    ///</summary>
    ImGuiDataType_U16,
    ///<summary>
    /// int<br/>
    ///</summary>
    ImGuiDataType_S32,
    ///<summary>
    /// unsigned int<br/>
    ///</summary>
    ImGuiDataType_U32,
    ///<summary>
    /// long long / __int64<br/>
    ///</summary>
    ImGuiDataType_S64,
    ///<summary>
    /// unsigned long long / unsigned __int64<br/>
    ///</summary>
    ImGuiDataType_U64,
    ///<summary>
    /// float<br/>
    ///</summary>
    ImGuiDataType_Float,
    ///<summary>
    /// double<br/>
    ///</summary>
    ImGuiDataType_Double,
    ///<summary>
    /// bool (provided for user convenience, not supported by scalar widgets)<br/>
    ///</summary>
    ImGuiDataType_Bool,
    ///<summary>
    /// char* (provided for user convenience, not supported by scalar widgets)<br/>
    ///</summary>
    ImGuiDataType_String,
    ImGuiDataType_COUNT
}

///<summary>
/// Flags for Shortcut(), SetNextItemShortcut(),<br/>
/// (and for upcoming extended versions of IsKeyPressed(), IsMouseClicked(), Shortcut(), SetKeyOwner(), SetItemKeyOwner() that are still in imgui_internal.h)<br/>
/// Don't mistake with ImGuiInputTextFlags! (which is for ImGui::InputText() function)<br/>
///</summary>
public enum ImGuiInputFlags
{
    ImGuiInputFlags_None = 0,
    ///<summary>
    /// Enable repeat. Return true on successive repeats. Default for legacy IsKeyPressed(). NOT Default for legacy IsMouseClicked(). MUST BE == 1.<br/>
    ///</summary>
    ImGuiInputFlags_Repeat = 1 << 0,
    ///<summary>
    /// Route to active item only.<br/>
    ///<br/>
    /// Flags for Shortcut(), SetNextItemShortcut()<br/>
    /// - Routing policies: RouteGlobal+OverActive &gt;&gt; RouteActive or RouteFocused (if owner is active item) &gt;&gt; RouteGlobal+OverFocused &gt;&gt; RouteFocused (if in focused window stack) &gt;&gt; RouteGlobal.<br/>
    /// - Default policy is RouteFocused. Can select only 1 policy among all available.<br/>
    ///</summary>
    ImGuiInputFlags_RouteActive = 1 << 10,
    ///<summary>
    /// Route to windows in the focus stack (DEFAULT). Deep-most focused window takes inputs. Active item takes inputs over deep-most focused window.<br/>
    ///</summary>
    ImGuiInputFlags_RouteFocused = 1 << 11,
    ///<summary>
    /// Global route (unless a focused window or active item registered the route).<br/>
    ///</summary>
    ImGuiInputFlags_RouteGlobal = 1 << 12,
    ///<summary>
    /// Do not register route, poll keys directly.<br/>
    ///</summary>
    ImGuiInputFlags_RouteAlways = 1 << 13,
    ///<summary>
    /// Option: global route: higher priority than focused route (unless active item in focused route).<br/>
    ///<br/>
    /// - Routing options<br/>
    ///</summary>
    ImGuiInputFlags_RouteOverFocused = 1 << 14,
    ///<summary>
    /// Option: global route: higher priority than active item. Unlikely you need to use that: will interfere with every active items, e.g. Ctrl+A registered by InputText will be overridden by this. May not be fully honored as user/internal code is likely to always assume they can access keys when active.<br/>
    ///</summary>
    ImGuiInputFlags_RouteOverActive = 1 << 15,
    ///<summary>
    /// Option: global route: will not be applied if underlying background/void is focused (== no Dear ImGui windows are focused). Useful for overlay applications.<br/>
    ///</summary>
    ImGuiInputFlags_RouteUnlessBgFocused = 1 << 16,
    ///<summary>
    /// Option: route evaluated from the point of view of root window rather than current window.<br/>
    ///</summary>
    ImGuiInputFlags_RouteFromRootWindow = 1 << 17,
    ///<summary>
    /// Automatically display a tooltip when hovering item [BETA] Unsure of right api (opt-in/opt-out)<br/>
    ///<br/>
    /// Flags for SetNextItemShortcut()<br/>
    ///</summary>
    ImGuiInputFlags_Tooltip = 1 << 18
}

///<summary>
/// Configuration flags stored in io.ConfigFlags. Set by user/application.<br/>
///</summary>
public enum ImGuiConfigFlags
{
    ImGuiConfigFlags_None = 0,
    ///<summary>
    /// Master keyboard navigation enable flag. Enable full Tabbing + directional arrows + space/enter to activate.<br/>
    ///</summary>
    ImGuiConfigFlags_NavEnableKeyboard = 1 << 0,
    ///<summary>
    /// Master gamepad navigation enable flag. Backend also needs to set ImGuiBackendFlags_HasGamepad.<br/>
    ///</summary>
    ImGuiConfigFlags_NavEnableGamepad = 1 << 1,
    ///<summary>
    /// Instruct dear imgui to disable mouse inputs and interactions.<br/>
    ///</summary>
    ImGuiConfigFlags_NoMouse = 1 << 4,
    ///<summary>
    /// Instruct backend to not alter mouse cursor shape and visibility. Use if the backend cursor changes are interfering with yours and you don't want to use SetMouseCursor() to change mouse cursor. You may want to honor requests from imgui by reading GetMouseCursor() yourself instead.<br/>
    ///</summary>
    ImGuiConfigFlags_NoMouseCursorChange = 1 << 5,
    ///<summary>
    /// Instruct dear imgui to disable keyboard inputs and interactions. This is done by ignoring keyboard events and clearing existing states.<br/>
    ///</summary>
    ImGuiConfigFlags_NoKeyboard = 1 << 6,
    ///<summary>
    /// Docking enable flags.<br/>
    ///<br/>
    /// [BETA] Docking<br/>
    ///</summary>
    ImGuiConfigFlags_DockingEnable = 1 << 7,
    ///<summary>
    /// Viewport enable flags (require both ImGuiBackendFlags_PlatformHasViewports + ImGuiBackendFlags_RendererHasViewports set by the respective backends)<br/>
    ///<br/>
    /// [BETA] Viewports<br/>
    /// When using viewports it is recommended that your default value for ImGuiCol_WindowBg is opaque (Alpha=1.0) so transition to a viewport won't be noticeable.<br/>
    ///</summary>
    ImGuiConfigFlags_ViewportsEnable = 1 << 10,
    ///<summary>
    /// Application is SRGB-aware.<br/>
    ///<br/>
    /// User storage (to allow your backend/engine to communicate to code that may be shared between multiple projects. Those flags are NOT used by core Dear ImGui)<br/>
    ///</summary>
    ImGuiConfigFlags_IsSRGB = 1 << 20,
    ///<summary>
    /// Application is using a touch screen instead of a mouse.<br/>
    ///</summary>
    ImGuiConfigFlags_IsTouchScreen = 1 << 21,
    ///<summary>
    /// [moved/renamed in 1.91.4] -&gt; use bool io.ConfigNavMoveSetMousePos<br/>
    ///</summary>
    ImGuiConfigFlags_NavEnableSetMousePos = 1 << 2,
    ///<summary>
    /// [moved/renamed in 1.91.4] -&gt; use bool io.ConfigNavCaptureKeyboard<br/>
    ///</summary>
    ImGuiConfigFlags_NavNoCaptureKeyboard = 1 << 3,
    ///<summary>
    /// [moved/renamed in 1.92.0] -&gt; use bool io.ConfigDpiScaleFonts<br/>
    ///</summary>
    ImGuiConfigFlags_DpiEnableScaleFonts = 1 << 14,
    ///<summary>
    /// [moved/renamed in 1.92.0] -&gt; use bool io.ConfigDpiScaleViewports<br/>
    ///</summary>
    ImGuiConfigFlags_DpiEnableScaleViewports = 1 << 15
}

///<summary>
/// Backend capabilities flags stored in io.BackendFlags. Set by imgui_impl_xxx or custom backend.<br/>
///</summary>
public enum ImGuiBackendFlags
{
    ImGuiBackendFlags_None = 0,
    ///<summary>
    /// Backend Platform supports gamepad and currently has one connected.<br/>
    ///</summary>
    ImGuiBackendFlags_HasGamepad = 1 << 0,
    ///<summary>
    /// Backend Platform supports honoring GetMouseCursor() value to change the OS cursor shape.<br/>
    ///</summary>
    ImGuiBackendFlags_HasMouseCursors = 1 << 1,
    ///<summary>
    /// Backend Platform supports io.WantSetMousePos requests to reposition the OS mouse position (only used if io.ConfigNavMoveSetMousePos is set).<br/>
    ///</summary>
    ImGuiBackendFlags_HasSetMousePos = 1 << 2,
    ///<summary>
    /// Backend Renderer supports ImDrawCmd::VtxOffset. This enables output of large meshes (64K+ vertices) while still using 16-bit indices.<br/>
    ///</summary>
    ImGuiBackendFlags_RendererHasVtxOffset = 1 << 3,
    ///<summary>
    /// Backend Renderer supports ImTextureData requests to create/update/destroy textures. This enables incremental texture updates and texture reloads. See https:github.com/ocornut/imgui/blob/master/docs/BACKENDS.md for instructions on how to upgrade your custom backend.<br/>
    ///</summary>
    ImGuiBackendFlags_RendererHasTextures = 1 << 4,
    ///<summary>
    /// Backend Renderer supports multiple viewports.<br/>
    ///<br/>
    /// [BETA] Multi-Viewports<br/>
    ///</summary>
    ImGuiBackendFlags_RendererHasViewports = 1 << 10,
    ///<summary>
    /// Backend Platform supports multiple viewports.<br/>
    ///</summary>
    ImGuiBackendFlags_PlatformHasViewports = 1 << 11,
    ///<summary>
    /// Backend Platform supports calling io.AddMouseViewportEvent() with the viewport under the mouse. IF POSSIBLE, ignore viewports with the ImGuiViewportFlags_NoInputs flag (Win32 backend, GLFW 3.30+ backend can do this, SDL backend cannot). If this cannot be done, Dear ImGui needs to use a flawed heuristic to find the viewport under.<br/>
    ///</summary>
    ImGuiBackendFlags_HasMouseHoveredViewport = 1 << 12,
    ///<summary>
    /// Backend Platform supports honoring viewport-&gt;ParentViewport/ParentViewportId value, by applying the corresponding parent/child relation at the Platform level.<br/>
    ///</summary>
    ImGuiBackendFlags_HasParentViewport = 1 << 13
}

///<summary>
/// Enumeration for PushStyleColor() / PopStyleColor()<br/>
///</summary>
public enum ImGuiCol
{
    ImGuiCol_Text,
    ImGuiCol_TextDisabled,
    ///<summary>
    /// Background of normal windows<br/>
    ///</summary>
    ImGuiCol_WindowBg,
    ///<summary>
    /// Background of child windows<br/>
    ///</summary>
    ImGuiCol_ChildBg,
    ///<summary>
    /// Background of popups, menus, tooltips windows<br/>
    ///</summary>
    ImGuiCol_PopupBg,
    ImGuiCol_Border,
    ImGuiCol_BorderShadow,
    ///<summary>
    /// Background of checkbox, radio button, plot, slider, text input<br/>
    ///</summary>
    ImGuiCol_FrameBg,
    ImGuiCol_FrameBgHovered,
    ImGuiCol_FrameBgActive,
    ///<summary>
    /// Title bar<br/>
    ///</summary>
    ImGuiCol_TitleBg,
    ///<summary>
    /// Title bar when focused<br/>
    ///</summary>
    ImGuiCol_TitleBgActive,
    ///<summary>
    /// Title bar when collapsed<br/>
    ///</summary>
    ImGuiCol_TitleBgCollapsed,
    ImGuiCol_MenuBarBg,
    ImGuiCol_ScrollbarBg,
    ImGuiCol_ScrollbarGrab,
    ImGuiCol_ScrollbarGrabHovered,
    ImGuiCol_ScrollbarGrabActive,
    ///<summary>
    /// Checkbox tick and RadioButton circle<br/>
    ///</summary>
    ImGuiCol_CheckMark,
    ImGuiCol_SliderGrab,
    ImGuiCol_SliderGrabActive,
    ImGuiCol_Button,
    ImGuiCol_ButtonHovered,
    ImGuiCol_ButtonActive,
    ///<summary>
    /// Header* colors are used for CollapsingHeader, TreeNode, Selectable, MenuItem<br/>
    ///</summary>
    ImGuiCol_Header,
    ImGuiCol_HeaderHovered,
    ImGuiCol_HeaderActive,
    ImGuiCol_Separator,
    ImGuiCol_SeparatorHovered,
    ImGuiCol_SeparatorActive,
    ///<summary>
    /// Resize grip in lower-right and lower-left corners of windows.<br/>
    ///</summary>
    ImGuiCol_ResizeGrip,
    ImGuiCol_ResizeGripHovered,
    ImGuiCol_ResizeGripActive,
    ///<summary>
    /// InputText cursor/caret<br/>
    ///</summary>
    ImGuiCol_InputTextCursor,
    ///<summary>
    /// Tab background, when hovered<br/>
    ///</summary>
    ImGuiCol_TabHovered,
    ///<summary>
    /// Tab background, when tab-bar is focused &amp; tab is unselected<br/>
    ///</summary>
    ImGuiCol_Tab,
    ///<summary>
    /// Tab background, when tab-bar is focused &amp; tab is selected<br/>
    ///</summary>
    ImGuiCol_TabSelected,
    ///<summary>
    /// Tab horizontal overline, when tab-bar is focused &amp; tab is selected<br/>
    ///</summary>
    ImGuiCol_TabSelectedOverline,
    ///<summary>
    /// Tab background, when tab-bar is unfocused &amp; tab is unselected<br/>
    ///</summary>
    ImGuiCol_TabDimmed,
    ///<summary>
    /// Tab background, when tab-bar is unfocused &amp; tab is selected<br/>
    ///</summary>
    ImGuiCol_TabDimmedSelected,
    ///<summary>
    ///..horizontal overline, when tab-bar is unfocused &amp; tab is selected<br/>
    ///</summary>
    ImGuiCol_TabDimmedSelectedOverline,
    ///<summary>
    /// Preview overlay color when about to docking something<br/>
    ///</summary>
    ImGuiCol_DockingPreview,
    ///<summary>
    /// Background color for empty node (e.g. CentralNode with no window docked into it)<br/>
    ///</summary>
    ImGuiCol_DockingEmptyBg,
    ImGuiCol_PlotLines,
    ImGuiCol_PlotLinesHovered,
    ImGuiCol_PlotHistogram,
    ImGuiCol_PlotHistogramHovered,
    ///<summary>
    /// Table header background<br/>
    ///</summary>
    ImGuiCol_TableHeaderBg,
    ///<summary>
    /// Table outer and header borders (prefer using Alpha=1.0 here)<br/>
    ///</summary>
    ImGuiCol_TableBorderStrong,
    ///<summary>
    /// Table inner borders (prefer using Alpha=1.0 here)<br/>
    ///</summary>
    ImGuiCol_TableBorderLight,
    ///<summary>
    /// Table row background (even rows)<br/>
    ///</summary>
    ImGuiCol_TableRowBg,
    ///<summary>
    /// Table row background (odd rows)<br/>
    ///</summary>
    ImGuiCol_TableRowBgAlt,
    ///<summary>
    /// Hyperlink color<br/>
    ///</summary>
    ImGuiCol_TextLink,
    ///<summary>
    /// Selected text inside an InputText<br/>
    ///</summary>
    ImGuiCol_TextSelectedBg,
    ///<summary>
    /// Tree node hierarchy outlines when using ImGuiTreeNodeFlags_DrawLines<br/>
    ///</summary>
    ImGuiCol_TreeLines,
    ///<summary>
    /// Rectangle border highlighting a drop target<br/>
    ///</summary>
    ImGuiCol_DragDropTarget,
    ///<summary>
    /// Rectangle background highlighting a drop target<br/>
    ///</summary>
    ImGuiCol_DragDropTargetBg,
    ///<summary>
    /// Unsaved Document marker (in window title and tabs)<br/>
    ///</summary>
    ImGuiCol_UnsavedMarker,
    ///<summary>
    /// Color of keyboard/gamepad navigation cursor/rectangle, when visible<br/>
    ///</summary>
    ImGuiCol_NavCursor,
    ///<summary>
    /// Highlight window when using Ctrl+Tab<br/>
    ///</summary>
    ImGuiCol_NavWindowingHighlight,
    ///<summary>
    /// Darken/colorize entire screen behind the Ctrl+Tab window list, when active<br/>
    ///</summary>
    ImGuiCol_NavWindowingDimBg,
    ///<summary>
    /// Darken/colorize entire screen behind a modal window, when one is active<br/>
    ///</summary>
    ImGuiCol_ModalWindowDimBg,
    ImGuiCol_COUNT,
    ///<summary>
    /// [renamed in 1.90.9]<br/>
    ///</summary>
    ImGuiCol_TabActive = ImGuiCol_TabSelected,
    ///<summary>
    /// [renamed in 1.90.9]<br/>
    ///</summary>
    ImGuiCol_TabUnfocused = ImGuiCol_TabDimmed,
    ///<summary>
    /// [renamed in 1.90.9]<br/>
    ///</summary>
    ImGuiCol_TabUnfocusedActive = ImGuiCol_TabDimmedSelected,
    ///<summary>
    /// [renamed in 1.91.4]<br/>
    ///</summary>
    ImGuiCol_NavHighlight = ImGuiCol_NavCursor
}

///<summary>
/// Enumeration for PushStyleVar() / PopStyleVar() to temporarily modify the ImGuiStyle structure.<br/>
/// - The enum only refers to fields of ImGuiStyle which makes sense to be pushed/popped inside UI code.<br/>
///   During initialization or between frames, feel free to just poke into ImGuiStyle directly.<br/>
/// - Tip: Use your programming IDE navigation facilities on the names in the _second column_ below to find the actual members and their description.<br/>
///   - In Visual Studio: Ctrl+Comma ("Edit.GoToAll") can follow symbols inside comments, whereas Ctrl+F12 ("Edit.GoToImplementation") cannot.<br/>
///   - In Visual Studio w/ Visual Assist installed: Alt+G ("VAssistX.GoToImplementation") can also follow symbols inside comments.<br/>
///   - In VS Code, CLion, etc.: Ctrl+Click can follow symbols inside comments.<br/>
/// - When changing this enum, you need to update the associated internal table GStyleVarInfo[] accordingly. This is where we link enum values to members offset/type.<br/>
///</summary>
public enum ImGuiStyleVar
{
    ///<summary>
    /// float     Alpha<br/>
    ///<br/>
    /// Enum name --------------------------  Member in ImGuiStyle structure (see ImGuiStyle for descriptions)<br/>
    ///</summary>
    ImGuiStyleVar_Alpha,
    ///<summary>
    /// float     DisabledAlpha<br/>
    ///</summary>
    ImGuiStyleVar_DisabledAlpha,
    ///<summary>
    /// ImVec2    WindowPadding<br/>
    ///</summary>
    ImGuiStyleVar_WindowPadding,
    ///<summary>
    /// float     WindowRounding<br/>
    ///</summary>
    ImGuiStyleVar_WindowRounding,
    ///<summary>
    /// float     WindowBorderSize<br/>
    ///</summary>
    ImGuiStyleVar_WindowBorderSize,
    ///<summary>
    /// ImVec2    WindowMinSize<br/>
    ///</summary>
    ImGuiStyleVar_WindowMinSize,
    ///<summary>
    /// ImVec2    WindowTitleAlign<br/>
    ///</summary>
    ImGuiStyleVar_WindowTitleAlign,
    ///<summary>
    /// float     ChildRounding<br/>
    ///</summary>
    ImGuiStyleVar_ChildRounding,
    ///<summary>
    /// float     ChildBorderSize<br/>
    ///</summary>
    ImGuiStyleVar_ChildBorderSize,
    ///<summary>
    /// float     PopupRounding<br/>
    ///</summary>
    ImGuiStyleVar_PopupRounding,
    ///<summary>
    /// float     PopupBorderSize<br/>
    ///</summary>
    ImGuiStyleVar_PopupBorderSize,
    ///<summary>
    /// ImVec2    FramePadding<br/>
    ///</summary>
    ImGuiStyleVar_FramePadding,
    ///<summary>
    /// float     FrameRounding<br/>
    ///</summary>
    ImGuiStyleVar_FrameRounding,
    ///<summary>
    /// float     FrameBorderSize<br/>
    ///</summary>
    ImGuiStyleVar_FrameBorderSize,
    ///<summary>
    /// ImVec2    ItemSpacing<br/>
    ///</summary>
    ImGuiStyleVar_ItemSpacing,
    ///<summary>
    /// ImVec2    ItemInnerSpacing<br/>
    ///</summary>
    ImGuiStyleVar_ItemInnerSpacing,
    ///<summary>
    /// float     IndentSpacing<br/>
    ///</summary>
    ImGuiStyleVar_IndentSpacing,
    ///<summary>
    /// ImVec2    CellPadding<br/>
    ///</summary>
    ImGuiStyleVar_CellPadding,
    ///<summary>
    /// float     ScrollbarSize<br/>
    ///</summary>
    ImGuiStyleVar_ScrollbarSize,
    ///<summary>
    /// float     ScrollbarRounding<br/>
    ///</summary>
    ImGuiStyleVar_ScrollbarRounding,
    ///<summary>
    /// float     ScrollbarPadding<br/>
    ///</summary>
    ImGuiStyleVar_ScrollbarPadding,
    ///<summary>
    /// float     GrabMinSize<br/>
    ///</summary>
    ImGuiStyleVar_GrabMinSize,
    ///<summary>
    /// float     GrabRounding<br/>
    ///</summary>
    ImGuiStyleVar_GrabRounding,
    ///<summary>
    /// float     ImageBorderSize<br/>
    ///</summary>
    ImGuiStyleVar_ImageBorderSize,
    ///<summary>
    /// float     TabRounding<br/>
    ///</summary>
    ImGuiStyleVar_TabRounding,
    ///<summary>
    /// float     TabBorderSize<br/>
    ///</summary>
    ImGuiStyleVar_TabBorderSize,
    ///<summary>
    /// float     TabMinWidthBase<br/>
    ///</summary>
    ImGuiStyleVar_TabMinWidthBase,
    ///<summary>
    /// float     TabMinWidthShrink<br/>
    ///</summary>
    ImGuiStyleVar_TabMinWidthShrink,
    ///<summary>
    /// float     TabBarBorderSize<br/>
    ///</summary>
    ImGuiStyleVar_TabBarBorderSize,
    ///<summary>
    /// float     TabBarOverlineSize<br/>
    ///</summary>
    ImGuiStyleVar_TabBarOverlineSize,
    ///<summary>
    /// float     TableAngledHeadersAngle<br/>
    ///</summary>
    ImGuiStyleVar_TableAngledHeadersAngle,
    ///<summary>
    /// ImVec2  TableAngledHeadersTextAlign<br/>
    ///</summary>
    ImGuiStyleVar_TableAngledHeadersTextAlign,
    ///<summary>
    /// float     TreeLinesSize<br/>
    ///</summary>
    ImGuiStyleVar_TreeLinesSize,
    ///<summary>
    /// float     TreeLinesRounding<br/>
    ///</summary>
    ImGuiStyleVar_TreeLinesRounding,
    ///<summary>
    /// ImVec2    ButtonTextAlign<br/>
    ///</summary>
    ImGuiStyleVar_ButtonTextAlign,
    ///<summary>
    /// ImVec2    SelectableTextAlign<br/>
    ///</summary>
    ImGuiStyleVar_SelectableTextAlign,
    ///<summary>
    /// float     SeparatorTextBorderSize<br/>
    ///</summary>
    ImGuiStyleVar_SeparatorTextBorderSize,
    ///<summary>
    /// ImVec2    SeparatorTextAlign<br/>
    ///</summary>
    ImGuiStyleVar_SeparatorTextAlign,
    ///<summary>
    /// ImVec2    SeparatorTextPadding<br/>
    ///</summary>
    ImGuiStyleVar_SeparatorTextPadding,
    ///<summary>
    /// float     DockingSeparatorSize<br/>
    ///</summary>
    ImGuiStyleVar_DockingSeparatorSize,
    ImGuiStyleVar_COUNT
}

///<summary>
/// Flags for InvisibleButton() [extended in imgui_internal.h]<br/>
///</summary>
public enum ImGuiButtonFlags
{
    ImGuiButtonFlags_None = 0,
    ///<summary>
    /// React on left mouse button (default)<br/>
    ///</summary>
    ImGuiButtonFlags_MouseButtonLeft = 1 << 0,
    ///<summary>
    /// React on right mouse button<br/>
    ///</summary>
    ImGuiButtonFlags_MouseButtonRight = 1 << 1,
    ///<summary>
    /// React on center mouse button<br/>
    ///</summary>
    ImGuiButtonFlags_MouseButtonMiddle = 1 << 2,
    ///<summary>
    /// [Internal]<br/>
    ///</summary>
    ImGuiButtonFlags_MouseButtonMask_ = ImGuiButtonFlags_MouseButtonLeft | ImGuiButtonFlags_MouseButtonRight | ImGuiButtonFlags_MouseButtonMiddle,
    ///<summary>
    /// InvisibleButton(): do not disable navigation/tabbing. Otherwise disabled by default.<br/>
    ///</summary>
    ImGuiButtonFlags_EnableNav = 1 << 3
}

///<summary>
/// Flags for ColorEdit3() / ColorEdit4() / ColorPicker3() / ColorPicker4() / ColorButton()<br/>
///</summary>
public enum ImGuiColorEditFlags
{
    ImGuiColorEditFlags_None = 0,
    ///<summary>
    ///               ColorEdit, ColorPicker, ColorButton: ignore Alpha component (will only read 3 components from the input pointer).<br/>
    ///</summary>
    ImGuiColorEditFlags_NoAlpha = 1 << 1,
    ///<summary>
    ///               ColorEdit: disable picker when clicking on color square.<br/>
    ///</summary>
    ImGuiColorEditFlags_NoPicker = 1 << 2,
    ///<summary>
    ///               ColorEdit: disable toggling options menu when right-clicking on inputs/small preview.<br/>
    ///</summary>
    ImGuiColorEditFlags_NoOptions = 1 << 3,
    ///<summary>
    ///               ColorEdit, ColorPicker: disable color square preview next to the inputs. (e.g. to show only the inputs)<br/>
    ///</summary>
    ImGuiColorEditFlags_NoSmallPreview = 1 << 4,
    ///<summary>
    ///               ColorEdit, ColorPicker: disable inputs sliders/text widgets (e.g. to show only the small preview color square).<br/>
    ///</summary>
    ImGuiColorEditFlags_NoInputs = 1 << 5,
    ///<summary>
    ///               ColorEdit, ColorPicker, ColorButton: disable tooltip when hovering the preview.<br/>
    ///</summary>
    ImGuiColorEditFlags_NoTooltip = 1 << 6,
    ///<summary>
    ///               ColorEdit, ColorPicker: disable display of inline text label (the label is still forwarded to the tooltip and picker).<br/>
    ///</summary>
    ImGuiColorEditFlags_NoLabel = 1 << 7,
    ///<summary>
    ///               ColorPicker: disable bigger color preview on right side of the picker, use small color square preview instead.<br/>
    ///</summary>
    ImGuiColorEditFlags_NoSidePreview = 1 << 8,
    ///<summary>
    ///               ColorEdit: disable drag and drop target. ColorButton: disable drag and drop source.<br/>
    ///</summary>
    ImGuiColorEditFlags_NoDragDrop = 1 << 9,
    ///<summary>
    ///               ColorButton: disable border (which is enforced by default)<br/>
    ///</summary>
    ImGuiColorEditFlags_NoBorder = 1 << 10,
    ///<summary>
    ///               ColorEdit, ColorPicker, ColorButton: disable alpha in the preview,. Contrary to _NoAlpha it may still be edited when calling ColorEdit4()/ColorPicker4(). For ColorButton() this does the same as _NoAlpha.<br/>
    ///<br/>
    /// Alpha preview<br/>
    /// - Prior to 1.91.8 (2025/01/21): alpha was made opaque in the preview by default using old name ImGuiColorEditFlags_AlphaPreview.<br/>
    /// - We now display the preview as transparent by default. You can use ImGuiColorEditFlags_AlphaOpaque to use old behavior.<br/>
    /// - The new flags may be combined better and allow finer controls.<br/>
    ///</summary>
    ImGuiColorEditFlags_AlphaOpaque = 1 << 11,
    ///<summary>
    ///               ColorEdit, ColorPicker, ColorButton: disable rendering a checkerboard background behind transparent color.<br/>
    ///</summary>
    ImGuiColorEditFlags_AlphaNoBg = 1 << 12,
    ///<summary>
    ///               ColorEdit, ColorPicker, ColorButton: display half opaque / half transparent preview.<br/>
    ///</summary>
    ImGuiColorEditFlags_AlphaPreviewHalf = 1 << 13,
    ///<summary>
    ///               ColorEdit, ColorPicker: show vertical alpha bar/gradient in picker.<br/>
    ///<br/>
    /// User Options (right-click on widget to change some of them).<br/>
    ///</summary>
    ImGuiColorEditFlags_AlphaBar = 1 << 16,
    ///<summary>
    ///               (WIP) ColorEdit: Currently only disable 0.0f..1.0f limits in RGBA edition (note: you probably want to use ImGuiColorEditFlags_Float flag as well).<br/>
    ///</summary>
    ImGuiColorEditFlags_HDR = 1 << 19,
    ///<summary>
    /// [Display]     ColorEdit: override _display_ type among RGB/HSV/Hex. ColorPicker: select any combination using one or more of RGB/HSV/Hex.<br/>
    ///</summary>
    ImGuiColorEditFlags_DisplayRGB = 1 << 20,
    ///<summary>
    /// [Display]     "<br/>
    ///</summary>
    ImGuiColorEditFlags_DisplayHSV = 1 << 21,
    ///<summary>
    /// [Display]     "<br/>
    ///</summary>
    ImGuiColorEditFlags_DisplayHex = 1 << 22,
    ///<summary>
    /// [DataType]    ColorEdit, ColorPicker, ColorButton: _display_ values formatted as 0..255.<br/>
    ///</summary>
    ImGuiColorEditFlags_Uint8 = 1 << 23,
    ///<summary>
    /// [DataType]    ColorEdit, ColorPicker, ColorButton: _display_ values formatted as 0.0f..1.0f floats instead of 0..255 integers. No round-trip of value via integers.<br/>
    ///</summary>
    ImGuiColorEditFlags_Float = 1 << 24,
    ///<summary>
    /// [Picker]      ColorPicker: bar for Hue, rectangle for Sat/Value.<br/>
    ///</summary>
    ImGuiColorEditFlags_PickerHueBar = 1 << 25,
    ///<summary>
    /// [Picker]      ColorPicker: wheel for Hue, triangle for Sat/Value.<br/>
    ///</summary>
    ImGuiColorEditFlags_PickerHueWheel = 1 << 26,
    ///<summary>
    /// [Input]       ColorEdit, ColorPicker: input and output data in RGB format.<br/>
    ///</summary>
    ImGuiColorEditFlags_InputRGB = 1 << 27,
    ///<summary>
    /// [Input]       ColorEdit, ColorPicker: input and output data in HSV format.<br/>
    ///</summary>
    ImGuiColorEditFlags_InputHSV = 1 << 28,
    ///<summary>
    /// Defaults Options. You can set application defaults using SetColorEditOptions(). The intent is that you probably don't want to<br/>
    /// override them in most of your calls. Let the user choose via the option menu and/or call SetColorEditOptions() once during startup.<br/>
    ///</summary>
    ImGuiColorEditFlags_DefaultOptions_ = ImGuiColorEditFlags_Uint8 | ImGuiColorEditFlags_DisplayRGB | ImGuiColorEditFlags_InputRGB | ImGuiColorEditFlags_PickerHueBar,
    ///<summary>
    /// [Internal] Masks<br/>
    ///</summary>
    ImGuiColorEditFlags_AlphaMask_ = ImGuiColorEditFlags_NoAlpha | ImGuiColorEditFlags_AlphaOpaque | ImGuiColorEditFlags_AlphaNoBg | ImGuiColorEditFlags_AlphaPreviewHalf,
    ImGuiColorEditFlags_DisplayMask_ = ImGuiColorEditFlags_DisplayRGB | ImGuiColorEditFlags_DisplayHSV | ImGuiColorEditFlags_DisplayHex,
    ImGuiColorEditFlags_DataTypeMask_ = ImGuiColorEditFlags_Uint8 | ImGuiColorEditFlags_Float,
    ImGuiColorEditFlags_PickerMask_ = ImGuiColorEditFlags_PickerHueWheel | ImGuiColorEditFlags_PickerHueBar,
    ImGuiColorEditFlags_InputMask_ = ImGuiColorEditFlags_InputRGB | ImGuiColorEditFlags_InputHSV,
    ///<summary>
    /// Removed in 1.91.8. This is the default now. Will display a checkerboard unless ImGuiColorEditFlags_AlphaNoBg is set.<br/>
    ///</summary>
    ImGuiColorEditFlags_AlphaPreview = 0
}

///<summary>
/// Flags for DragFloat(), DragInt(), SliderFloat(), SliderInt() etc.<br/>
/// We use the same sets of flags for DragXXX() and SliderXXX() functions as the features are the same and it makes it easier to swap them.<br/>
/// (Those are per-item flags. There is shared behavior flag too: ImGuiIO: io.ConfigDragClickToInputText)<br/>
///</summary>
public enum ImGuiSliderFlags
{
    ImGuiSliderFlags_None = 0,
    ///<summary>
    /// Make the widget logarithmic (linear otherwise). Consider using ImGuiSliderFlags_NoRoundToFormat with this if using a format-string with small amount of digits.<br/>
    ///</summary>
    ImGuiSliderFlags_Logarithmic = 1 << 5,
    ///<summary>
    /// Disable rounding underlying value to match precision of the display format string (e.g. %.3f values are rounded to those 3 digits).<br/>
    ///</summary>
    ImGuiSliderFlags_NoRoundToFormat = 1 << 6,
    ///<summary>
    /// Disable Ctrl+Click or Enter key allowing to input text directly into the widget.<br/>
    ///</summary>
    ImGuiSliderFlags_NoInput = 1 << 7,
    ///<summary>
    /// Enable wrapping around from max to min and from min to max. Only supported by DragXXX() functions for now.<br/>
    ///</summary>
    ImGuiSliderFlags_WrapAround = 1 << 8,
    ///<summary>
    /// Clamp value to min/max bounds when input manually with Ctrl+Click. By default Ctrl+Click allows going out of bounds.<br/>
    ///</summary>
    ImGuiSliderFlags_ClampOnInput = 1 << 9,
    ///<summary>
    /// Clamp even if min==max==0.0f. Otherwise due to legacy reason DragXXX functions don't clamp with those values. When your clamping limits are dynamic you almost always want to use it.<br/>
    ///</summary>
    ImGuiSliderFlags_ClampZeroRange = 1 << 10,
    ///<summary>
    /// Disable keyboard modifiers altering tweak speed. Useful if you want to alter tweak speed yourself based on your own logic.<br/>
    ///</summary>
    ImGuiSliderFlags_NoSpeedTweaks = 1 << 11,
    ImGuiSliderFlags_AlwaysClamp = ImGuiSliderFlags_ClampOnInput | ImGuiSliderFlags_ClampZeroRange,
    ///<summary>
    /// [Internal] We treat using those bits as being potentially a 'float power' argument from the previous API that has got miscast to this enum, and will trigger an assert if needed.<br/>
    ///</summary>
    ImGuiSliderFlags_InvalidMask_ = 0x7000000F
}

///<summary>
/// Identify a mouse button.<br/>
/// Those values are guaranteed to be stable and we frequently use 0/1 directly. Named enums provided for convenience.<br/>
///</summary>
public enum ImGuiMouseButton
{
    ImGuiMouseButton_Left = 0,
    ImGuiMouseButton_Right = 1,
    ImGuiMouseButton_Middle = 2,
    ImGuiMouseButton_COUNT = 5
}

///<summary>
/// Enumeration for GetMouseCursor()<br/>
/// User code may request backend to display given cursor by calling SetMouseCursor(), which is why we have some cursors that are marked unused here<br/>
///</summary>
public enum ImGuiMouseCursor
{
    ImGuiMouseCursor_None = -1,
    ImGuiMouseCursor_Arrow = 0,
    ///<summary>
    /// When hovering over InputText, etc.<br/>
    ///</summary>
    ImGuiMouseCursor_TextInput,
    ///<summary>
    /// (Unused by Dear ImGui functions)<br/>
    ///</summary>
    ImGuiMouseCursor_ResizeAll,
    ///<summary>
    /// When hovering over a horizontal border<br/>
    ///</summary>
    ImGuiMouseCursor_ResizeNS,
    ///<summary>
    /// When hovering over a vertical border or a column<br/>
    ///</summary>
    ImGuiMouseCursor_ResizeEW,
    ///<summary>
    /// When hovering over the bottom-left corner of a window<br/>
    ///</summary>
    ImGuiMouseCursor_ResizeNESW,
    ///<summary>
    /// When hovering over the bottom-right corner of a window<br/>
    ///</summary>
    ImGuiMouseCursor_ResizeNWSE,
    ///<summary>
    /// (Unused by Dear ImGui functions. Use for e.g. hyperlinks)<br/>
    ///</summary>
    ImGuiMouseCursor_Hand,
    ///<summary>
    /// When waiting for something to process/load.<br/>
    ///</summary>
    ImGuiMouseCursor_Wait,
    ///<summary>
    /// When waiting for something to process/load, but application is still interactive.<br/>
    ///</summary>
    ImGuiMouseCursor_Progress,
    ///<summary>
    /// When hovering something with disallowed interaction. Usually a crossed circle.<br/>
    ///</summary>
    ImGuiMouseCursor_NotAllowed,
    ImGuiMouseCursor_COUNT
}

///<summary>
/// Enumeration for ImGui::SetNextWindow***(), SetWindow***(), SetNextItem***() functions<br/>
/// Represent a condition.<br/>
/// Important: Treat as a regular enum! Do NOT combine multiple values using binary operators! All the functions above treat 0 as a shortcut to ImGuiCond_Always.<br/>
///</summary>
public enum ImGuiCond
{
    ///<summary>
    /// No condition (always set the variable), same as _Always<br/>
    ///</summary>
    ImGuiCond_None = 0,
    ///<summary>
    /// No condition (always set the variable), same as _None<br/>
    ///</summary>
    ImGuiCond_Always = 1 << 0,
    ///<summary>
    /// Set the variable once per runtime session (only the first call will succeed)<br/>
    ///</summary>
    ImGuiCond_Once = 1 << 1,
    ///<summary>
    /// Set the variable if the object/window has no persistently saved data (no entry in .ini file)<br/>
    ///</summary>
    ImGuiCond_FirstUseEver = 1 << 2,
    ///<summary>
    /// Set the variable if the object/window is appearing after being hidden/inactive (or the first time)<br/>
    ///</summary>
    ImGuiCond_Appearing = 1 << 3
}

///<summary>
/// Flags for ImGui::BeginTable()<br/>
/// - Important! Sizing policies have complex and subtle side effects, much more so than you would expect.<br/>
///   Read comments/demos carefully + experiment with live demos to get acquainted with them.<br/>
/// - The DEFAULT sizing policies are:<br/>
///    - Default to ImGuiTableFlags_SizingFixedFit    if ScrollX is on, or if host window has ImGuiWindowFlags_AlwaysAutoResize.<br/>
///    - Default to ImGuiTableFlags_SizingStretchSame if ScrollX is off.<br/>
/// - When ScrollX is off:<br/>
///    - Table defaults to ImGuiTableFlags_SizingStretchSame -&gt; all Columns defaults to ImGuiTableColumnFlags_WidthStretch with same weight.<br/>
///    - Columns sizing policy allowed: Stretch (default), Fixed/Auto.<br/>
///    - Fixed Columns (if any) will generally obtain their requested width (unless the table cannot fit them all).<br/>
///    - Stretch Columns will share the remaining width according to their respective weight.<br/>
///    - Mixed Fixed/Stretch columns is possible but has various side-effects on resizing behaviors.<br/>
///      The typical use of mixing sizing policies is: any number of LEADING Fixed columns, followed by one or two TRAILING Stretch columns.<br/>
///      (this is because the visible order of columns have subtle but necessary effects on how they react to manual resizing).<br/>
/// - When ScrollX is on:<br/>
///    - Table defaults to ImGuiTableFlags_SizingFixedFit -&gt; all Columns defaults to ImGuiTableColumnFlags_WidthFixed<br/>
///    - Columns sizing policy allowed: Fixed/Auto mostly.<br/>
///    - Fixed Columns can be enlarged as needed. Table will show a horizontal scrollbar if needed.<br/>
///    - When using auto-resizing (non-resizable) fixed columns, querying the content width to use item right-alignment e.g. SetNextItemWidth(-FLT_MIN) doesn't make sense, would create a feedback loop.<br/>
///    - Using Stretch columns OFTEN DOES NOT MAKE SENSE if ScrollX is on, UNLESS you have specified a value for 'inner_width' in BeginTable().<br/>
///      If you specify a value for 'inner_width' then effectively the scrolling space is known and Stretch or mixed Fixed/Stretch columns become meaningful again.<br/>
/// - Read on documentation at the top of imgui_tables.cpp for details.<br/>
///</summary>
public enum ImGuiTableFlags
{
    ///<summary>
    /// Features<br/>
    ///</summary>
    ImGuiTableFlags_None = 0,
    ///<summary>
    /// Enable resizing columns.<br/>
    ///</summary>
    ImGuiTableFlags_Resizable = 1 << 0,
    ///<summary>
    /// Enable reordering columns in header row (need calling TableSetupColumn() + TableHeadersRow() to display headers)<br/>
    ///</summary>
    ImGuiTableFlags_Reorderable = 1 << 1,
    ///<summary>
    /// Enable hiding/disabling columns in context menu.<br/>
    ///</summary>
    ImGuiTableFlags_Hideable = 1 << 2,
    ///<summary>
    /// Enable sorting. Call TableGetSortSpecs() to obtain sort specs. Also see ImGuiTableFlags_SortMulti and ImGuiTableFlags_SortTristate.<br/>
    ///</summary>
    ImGuiTableFlags_Sortable = 1 << 3,
    ///<summary>
    /// Disable persisting columns order, width and sort settings in the .ini file.<br/>
    ///</summary>
    ImGuiTableFlags_NoSavedSettings = 1 << 4,
    ///<summary>
    /// Right-click on columns body/contents will display table context menu. By default it is available in TableHeadersRow().<br/>
    ///</summary>
    ImGuiTableFlags_ContextMenuInBody = 1 << 5,
    ///<summary>
    /// Set each RowBg color with ImGuiCol_TableRowBg or ImGuiCol_TableRowBgAlt (equivalent of calling TableSetBgColor with ImGuiTableBgFlags_RowBg0 on each row manually)<br/>
    ///<br/>
    /// Decorations<br/>
    ///</summary>
    ImGuiTableFlags_RowBg = 1 << 6,
    ///<summary>
    /// Draw horizontal borders between rows.<br/>
    ///</summary>
    ImGuiTableFlags_BordersInnerH = 1 << 7,
    ///<summary>
    /// Draw horizontal borders at the top and bottom.<br/>
    ///</summary>
    ImGuiTableFlags_BordersOuterH = 1 << 8,
    ///<summary>
    /// Draw vertical borders between columns.<br/>
    ///</summary>
    ImGuiTableFlags_BordersInnerV = 1 << 9,
    ///<summary>
    /// Draw vertical borders on the left and right sides.<br/>
    ///</summary>
    ImGuiTableFlags_BordersOuterV = 1 << 10,
    ///<summary>
    /// Draw horizontal borders.<br/>
    ///</summary>
    ImGuiTableFlags_BordersH = ImGuiTableFlags_BordersInnerH | ImGuiTableFlags_BordersOuterH,
    ///<summary>
    /// Draw vertical borders.<br/>
    ///</summary>
    ImGuiTableFlags_BordersV = ImGuiTableFlags_BordersInnerV | ImGuiTableFlags_BordersOuterV,
    ///<summary>
    /// Draw inner borders.<br/>
    ///</summary>
    ImGuiTableFlags_BordersInner = ImGuiTableFlags_BordersInnerV | ImGuiTableFlags_BordersInnerH,
    ///<summary>
    /// Draw outer borders.<br/>
    ///</summary>
    ImGuiTableFlags_BordersOuter = ImGuiTableFlags_BordersOuterV | ImGuiTableFlags_BordersOuterH,
    ///<summary>
    /// Draw all borders.<br/>
    ///</summary>
    ImGuiTableFlags_Borders = ImGuiTableFlags_BordersInner | ImGuiTableFlags_BordersOuter,
    ///<summary>
    /// [ALPHA] Disable vertical borders in columns Body (borders will always appear in Headers). -&gt; May move to style<br/>
    ///</summary>
    ImGuiTableFlags_NoBordersInBody = 1 << 11,
    ///<summary>
    /// [ALPHA] Disable vertical borders in columns Body until hovered for resize (borders will always appear in Headers). -&gt; May move to style<br/>
    ///</summary>
    ImGuiTableFlags_NoBordersInBodyUntilResize = 1 << 12,
    ///<summary>
    /// Columns default to _WidthFixed or _WidthAuto (if resizable or not resizable), matching contents width.<br/>
    ///<br/>
    /// Sizing Policy (read above for defaults)<br/>
    ///</summary>
    ImGuiTableFlags_SizingFixedFit = 1 << 13,
    ///<summary>
    /// Columns default to _WidthFixed or _WidthAuto (if resizable or not resizable), matching the maximum contents width of all columns. Implicitly enable ImGuiTableFlags_NoKeepColumnsVisible.<br/>
    ///</summary>
    ImGuiTableFlags_SizingFixedSame = 2 << 13,
    ///<summary>
    /// Columns default to _WidthStretch with default weights proportional to each columns contents widths.<br/>
    ///</summary>
    ImGuiTableFlags_SizingStretchProp = 3 << 13,
    ///<summary>
    /// Columns default to _WidthStretch with default weights all equal, unless overridden by TableSetupColumn().<br/>
    ///</summary>
    ImGuiTableFlags_SizingStretchSame = 4 << 13,
    ///<summary>
    /// Make outer width auto-fit to columns, overriding outer_size.x value. Only available when ScrollX/ScrollY are disabled and Stretch columns are not used.<br/>
    ///<br/>
    /// Sizing Extra Options<br/>
    ///</summary>
    ImGuiTableFlags_NoHostExtendX = 1 << 16,
    ///<summary>
    /// Make outer height stop exactly at outer_size.y (prevent auto-extending table past the limit). Only available when ScrollX/ScrollY are disabled. Data below the limit will be clipped and not visible.<br/>
    ///</summary>
    ImGuiTableFlags_NoHostExtendY = 1 << 17,
    ///<summary>
    /// Disable keeping column always minimally visible when ScrollX is off and table gets too small. Not recommended if columns are resizable.<br/>
    ///</summary>
    ImGuiTableFlags_NoKeepColumnsVisible = 1 << 18,
    ///<summary>
    /// Disable distributing remainder width to stretched columns (width allocation on a 100-wide table with 3 columns: Without this flag: 33,33,34. With this flag: 33,33,33). With larger number of columns, resizing will appear to be less smooth.<br/>
    ///</summary>
    ImGuiTableFlags_PreciseWidths = 1 << 19,
    ///<summary>
    /// Disable clipping rectangle for every individual columns (reduce draw command count, items will be able to overflow into other columns). Generally incompatible with TableSetupScrollFreeze().<br/>
    ///<br/>
    /// Clipping<br/>
    ///</summary>
    ImGuiTableFlags_NoClip = 1 << 20,
    ///<summary>
    /// Default if BordersOuterV is on. Enable outermost padding. Generally desirable if you have headers.<br/>
    ///<br/>
    /// Padding<br/>
    ///</summary>
    ImGuiTableFlags_PadOuterX = 1 << 21,
    ///<summary>
    /// Default if BordersOuterV is off. Disable outermost padding.<br/>
    ///</summary>
    ImGuiTableFlags_NoPadOuterX = 1 << 22,
    ///<summary>
    /// Disable inner padding between columns (double inner padding if BordersOuterV is on, single inner padding if BordersOuterV is off).<br/>
    ///</summary>
    ImGuiTableFlags_NoPadInnerX = 1 << 23,
    ///<summary>
    /// Enable horizontal scrolling. Require 'outer_size' parameter of BeginTable() to specify the container size. Changes default sizing policy. Because this creates a child window, ScrollY is currently generally recommended when using ScrollX.<br/>
    ///<br/>
    /// Scrolling<br/>
    ///</summary>
    ImGuiTableFlags_ScrollX = 1 << 24,
    ///<summary>
    /// Enable vertical scrolling. Require 'outer_size' parameter of BeginTable() to specify the container size.<br/>
    ///</summary>
    ImGuiTableFlags_ScrollY = 1 << 25,
    ///<summary>
    /// Hold shift when clicking headers to sort on multiple column. TableGetSortSpecs() may return specs where (SpecsCount &gt; 1).<br/>
    ///<br/>
    /// Sorting<br/>
    ///</summary>
    ImGuiTableFlags_SortMulti = 1 << 26,
    ///<summary>
    /// Allow no sorting, disable default sorting. TableGetSortSpecs() may return specs where (SpecsCount == 0).<br/>
    ///</summary>
    ImGuiTableFlags_SortTristate = 1 << 27,
    ///<summary>
    /// Highlight column headers when hovered (may evolve into a fuller highlight)<br/>
    ///<br/>
    /// Miscellaneous<br/>
    ///</summary>
    ImGuiTableFlags_HighlightHoveredColumn = 1 << 28,
    ///<summary>
    /// [Internal] Combinations and masks<br/>
    ///</summary>
    ImGuiTableFlags_SizingMask_ = ImGuiTableFlags_SizingFixedFit | ImGuiTableFlags_SizingFixedSame | ImGuiTableFlags_SizingStretchProp | ImGuiTableFlags_SizingStretchSame
}

///<summary>
/// Flags for ImGui::TableSetupColumn()<br/>
///</summary>
public enum ImGuiTableColumnFlags
{
    ///<summary>
    /// Input configuration flags<br/>
    ///</summary>
    ImGuiTableColumnFlags_None = 0,
    ///<summary>
    /// Overriding/master disable flag: hide column, won't show in context menu (unlike calling TableSetColumnEnabled() which manipulates the user accessible state)<br/>
    ///</summary>
    ImGuiTableColumnFlags_Disabled = 1 << 0,
    ///<summary>
    /// Default as a hidden/disabled column.<br/>
    ///</summary>
    ImGuiTableColumnFlags_DefaultHide = 1 << 1,
    ///<summary>
    /// Default as a sorting column.<br/>
    ///</summary>
    ImGuiTableColumnFlags_DefaultSort = 1 << 2,
    ///<summary>
    /// Column will stretch. Preferable with horizontal scrolling disabled (default if table sizing policy is _SizingStretchSame or _SizingStretchProp).<br/>
    ///</summary>
    ImGuiTableColumnFlags_WidthStretch = 1 << 3,
    ///<summary>
    /// Column will not stretch. Preferable with horizontal scrolling enabled (default if table sizing policy is _SizingFixedFit and table is resizable).<br/>
    ///</summary>
    ImGuiTableColumnFlags_WidthFixed = 1 << 4,
    ///<summary>
    /// Disable manual resizing.<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoResize = 1 << 5,
    ///<summary>
    /// Disable manual reordering this column, this will also prevent other columns from crossing over this column.<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoReorder = 1 << 6,
    ///<summary>
    /// Disable ability to hide/disable this column.<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoHide = 1 << 7,
    ///<summary>
    /// Disable clipping for this column (all NoClip columns will render in a same draw command).<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoClip = 1 << 8,
    ///<summary>
    /// Disable ability to sort on this field (even if ImGuiTableFlags_Sortable is set on the table).<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoSort = 1 << 9,
    ///<summary>
    /// Disable ability to sort in the ascending direction.<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoSortAscending = 1 << 10,
    ///<summary>
    /// Disable ability to sort in the descending direction.<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoSortDescending = 1 << 11,
    ///<summary>
    /// TableHeadersRow() will submit an empty label for this column. Convenient for some small columns. Name will still appear in context menu or in angled headers. You may append into this cell by calling TableSetColumnIndex() right after the TableHeadersRow() call.<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoHeaderLabel = 1 << 12,
    ///<summary>
    /// Disable header text width contribution to automatic column width.<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoHeaderWidth = 1 << 13,
    ///<summary>
    /// Make the initial sort direction Ascending when first sorting on this column (default).<br/>
    ///</summary>
    ImGuiTableColumnFlags_PreferSortAscending = 1 << 14,
    ///<summary>
    /// Make the initial sort direction Descending when first sorting on this column.<br/>
    ///</summary>
    ImGuiTableColumnFlags_PreferSortDescending = 1 << 15,
    ///<summary>
    /// Use current Indent value when entering cell (default for column 0).<br/>
    ///</summary>
    ImGuiTableColumnFlags_IndentEnable = 1 << 16,
    ///<summary>
    /// Ignore current Indent value when entering cell (default for columns &gt; 0). Indentation changes _within_ the cell will still be honored.<br/>
    ///</summary>
    ImGuiTableColumnFlags_IndentDisable = 1 << 17,
    ///<summary>
    /// TableHeadersRow() will submit an angled header row for this column. Note this will add an extra row.<br/>
    ///</summary>
    ImGuiTableColumnFlags_AngledHeader = 1 << 18,
    ///<summary>
    /// Status: is enabled == not hidden by user/api (referred to as "Hide" in _DefaultHide and _NoHide) flags.<br/>
    ///<br/>
    /// Output status flags, read-only via TableGetColumnFlags()<br/>
    ///</summary>
    ImGuiTableColumnFlags_IsEnabled = 1 << 24,
    ///<summary>
    /// Status: is visible == is enabled AND not clipped by scrolling.<br/>
    ///</summary>
    ImGuiTableColumnFlags_IsVisible = 1 << 25,
    ///<summary>
    /// Status: is currently part of the sort specs<br/>
    ///</summary>
    ImGuiTableColumnFlags_IsSorted = 1 << 26,
    ///<summary>
    /// Status: is hovered by mouse<br/>
    ///</summary>
    ImGuiTableColumnFlags_IsHovered = 1 << 27,
    ///<summary>
    /// [Internal] Combinations and masks<br/>
    ///</summary>
    ImGuiTableColumnFlags_WidthMask_ = ImGuiTableColumnFlags_WidthStretch | ImGuiTableColumnFlags_WidthFixed,
    ImGuiTableColumnFlags_IndentMask_ = ImGuiTableColumnFlags_IndentEnable | ImGuiTableColumnFlags_IndentDisable,
    ImGuiTableColumnFlags_StatusMask_ = ImGuiTableColumnFlags_IsEnabled | ImGuiTableColumnFlags_IsVisible | ImGuiTableColumnFlags_IsSorted | ImGuiTableColumnFlags_IsHovered,
    ///<summary>
    /// [Internal] Disable user resizing this column directly (it may however we resized indirectly from its left edge)<br/>
    ///</summary>
    ImGuiTableColumnFlags_NoDirectResize_ = 1 << 30
}

///<summary>
/// Flags for ImGui::TableNextRow()<br/>
///</summary>
public enum ImGuiTableRowFlags
{
    ImGuiTableRowFlags_None = 0,
    ///<summary>
    /// Identify header row (set default background color + width of its contents accounted differently for auto column width)<br/>
    ///</summary>
    ImGuiTableRowFlags_Headers = 1 << 0
}

///<summary>
/// Enum for ImGui::TableSetBgColor()<br/>
/// Background colors are rendering in 3 layers:<br/>
///  - Layer 0: draw with RowBg0 color if set, otherwise draw with ColumnBg0 if set.<br/>
///  - Layer 1: draw with RowBg1 color if set, otherwise draw with ColumnBg1 if set.<br/>
///  - Layer 2: draw with CellBg color if set.<br/>
/// The purpose of the two row/columns layers is to let you decide if a background color change should override or blend with the existing color.<br/>
/// When using ImGuiTableFlags_RowBg on the table, each row has the RowBg0 color automatically set for odd/even rows.<br/>
/// If you set the color of RowBg0 target, your color will override the existing RowBg0 color.<br/>
/// If you set the color of RowBg1 or ColumnBg1 target, your color will blend over the RowBg0 color.<br/>
///</summary>
public enum ImGuiTableBgTarget
{
    ImGuiTableBgTarget_None = 0,
    ///<summary>
    /// Set row background color 0 (generally used for background, automatically set when ImGuiTableFlags_RowBg is used)<br/>
    ///</summary>
    ImGuiTableBgTarget_RowBg0 = 1,
    ///<summary>
    /// Set row background color 1 (generally used for selection marking)<br/>
    ///</summary>
    ImGuiTableBgTarget_RowBg1 = 2,
    ///<summary>
    /// Set cell background color (top-most color)<br/>
    ///</summary>
    ImGuiTableBgTarget_CellBg = 3
}

///<summary>
/// Sorting specifications for a table (often handling sort specs for a single column, occasionally more)<br/>
/// Obtained by calling TableGetSortSpecs().<br/>
/// When 'SpecsDirty == true' you can sort your data. It will be true with sorting specs have changed since last call, or the first time.<br/>
/// Make sure to set 'SpecsDirty = false' after sorting, else you may wastefully sort your data every frame!<br/>
///</summary>
public unsafe interface IImGuiTableSortSpecs
{
    public nint NativePointer { get; }

    ///<summary>
    /// Pointer to sort spec array.<br/>
    ///</summary>
    IImGuiTableColumnSortSpecs Specs { get; }

    ///<summary>
    /// Sort spec count. Most often 1. May be &gt; 1 when ImGuiTableFlags_SortMulti is enabled. May be == 0 when ImGuiTableFlags_SortTristate is enabled.<br/>
    ///</summary>
    ref int SpecsCount { get; }

    ///<summary>
    /// Set to true when specs have changed since last time! Use this to sort again, then clear the flag.<br/>
    ///</summary>
    ref bool SpecsDirty { get; }
}

///<summary>
/// Sorting specification for one column of a table (sizeof == 12 bytes)<br/>
///</summary>
public unsafe interface IImGuiTableColumnSortSpecs
{
    public nint NativePointer { get; }

    ///<summary>
    /// User id of the column (if specified by a TableSetupColumn() call)<br/>
    ///</summary>
    ref uint ColumnUserID { get; }

    ///<summary>
    /// Index of the column<br/>
    ///</summary>
    ref short ColumnIndex { get; }

    ///<summary>
    /// Index within parent ImGuiTableSortSpecs (always stored in order starting from 0, tables sorted on a single criteria will always have a 0 here)<br/>
    ///</summary>
    ref short SortOrder { get; }

    ///<summary>
    /// ImGuiSortDirection_Ascending or ImGuiSortDirection_Descending<br/>
    ///</summary>
    ref byte SortDirection { get; }
}

public unsafe interface IImGuiStyle
{
    public nint NativePointer { get; }

    ///<summary>
    /// Current base font size before external global factors are applied. Use PushFont(NULL, size) to modify. Use ImGui::GetFontSize() to obtain scaled value.<br/>
    ///<br/>
    /// Font scaling<br/>
    /// - recap: ImGui::GetFontSize() == FontSizeBase * (FontScaleMain * FontScaleDpi * other_scaling_factors)<br/>
    ///</summary>
    ref float FontSizeBase { get; }

    ///<summary>
    /// Main global scale factor. May be set by application once, or exposed to end-user.<br/>
    ///</summary>
    ref float FontScaleMain { get; }

    ///<summary>
    /// Additional global scale factor from viewport/monitor contents scale. When io.ConfigDpiScaleFonts is enabled, this is automatically overwritten when changing monitor DPI.<br/>
    ///</summary>
    ref float FontScaleDpi { get; }

    ///<summary>
    /// Global alpha applies to everything in Dear ImGui.<br/>
    ///</summary>
    ref float Alpha { get; }

    ///<summary>
    /// Additional alpha multiplier applied by BeginDisabled(). Multiply over current value of Alpha.<br/>
    ///</summary>
    ref float DisabledAlpha { get; }

    ///<summary>
    /// Padding within a window.<br/>
    ///</summary>
    ref Vector2 WindowPadding { get; }

    ///<summary>
    /// Radius of window corners rounding. Set to 0.0f to have rectangular windows. Large values tend to lead to variety of artifacts and are not recommended.<br/>
    ///</summary>
    ref float WindowRounding { get; }

    ///<summary>
    /// Thickness of border around windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).<br/>
    ///</summary>
    ref float WindowBorderSize { get; }

    ///<summary>
    /// Hit-testing extent outside/inside resizing border. Also extend determination of hovered window. Generally meaningfully larger than WindowBorderSize to make it easy to reach borders.<br/>
    ///</summary>
    ref float WindowBorderHoverPadding { get; }

    ///<summary>
    /// Minimum window size. This is a global setting. If you want to constrain individual windows, use SetNextWindowSizeConstraints().<br/>
    ///</summary>
    ref Vector2 WindowMinSize { get; }

    ///<summary>
    /// Alignment for title bar text. Defaults to (0.0f,0.5f) for left-aligned,vertically centered.<br/>
    ///</summary>
    ref Vector2 WindowTitleAlign { get; }

    ///<summary>
    /// Side of the collapsing/docking button in the title bar (None/Left/Right). Defaults to ImGuiDir_Left.<br/>
    ///</summary>
    ref int WindowMenuButtonPosition { get; }

    ///<summary>
    /// Radius of child window corners rounding. Set to 0.0f to have rectangular windows.<br/>
    ///</summary>
    ref float ChildRounding { get; }

    ///<summary>
    /// Thickness of border around child windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).<br/>
    ///</summary>
    ref float ChildBorderSize { get; }

    ///<summary>
    /// Radius of popup window corners rounding. (Note that tooltip windows use WindowRounding)<br/>
    ///</summary>
    ref float PopupRounding { get; }

    ///<summary>
    /// Thickness of border around popup/tooltip windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).<br/>
    ///</summary>
    ref float PopupBorderSize { get; }

    ///<summary>
    /// Padding within a framed rectangle (used by most widgets).<br/>
    ///</summary>
    ref Vector2 FramePadding { get; }

    ///<summary>
    /// Radius of frame corners rounding. Set to 0.0f to have rectangular frame (used by most widgets).<br/>
    ///</summary>
    ref float FrameRounding { get; }

    ///<summary>
    /// Thickness of border around frames. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).<br/>
    ///</summary>
    ref float FrameBorderSize { get; }

    ///<summary>
    /// Horizontal and vertical spacing between widgets/lines.<br/>
    ///</summary>
    ref Vector2 ItemSpacing { get; }

    ///<summary>
    /// Horizontal and vertical spacing between within elements of a composed widget (e.g. a slider and its label).<br/>
    ///</summary>
    ref Vector2 ItemInnerSpacing { get; }

    ///<summary>
    /// Padding within a table cell. Cellpadding.x is locked for entire table. CellPadding.y may be altered between different rows.<br/>
    ///</summary>
    ref Vector2 CellPadding { get; }

    ///<summary>
    /// Expand reactive bounding box for touch-based system where touch position is not accurate enough. Unfortunately we don't sort widgets so priority on overlap will always be given to the first widget. So don't grow this too much!<br/>
    ///</summary>
    ref Vector2 TouchExtraPadding { get; }

    ///<summary>
    /// Horizontal indentation when e.g. entering a tree node. Generally == (FontSize + FramePadding.x*2).<br/>
    ///</summary>
    ref float IndentSpacing { get; }

    ///<summary>
    /// Minimum horizontal spacing between two columns. Preferably &gt; (FramePadding.x + 1).<br/>
    ///</summary>
    ref float ColumnsMinSpacing { get; }

    ///<summary>
    /// Width of the vertical scrollbar, Height of the horizontal scrollbar.<br/>
    ///</summary>
    ref float ScrollbarSize { get; }

    ///<summary>
    /// Radius of grab corners for scrollbar.<br/>
    ///</summary>
    ref float ScrollbarRounding { get; }

    ///<summary>
    /// Padding of scrollbar grab within its frame (same for both axes).<br/>
    ///</summary>
    ref float ScrollbarPadding { get; }

    ///<summary>
    /// Minimum width/height of a grab box for slider/scrollbar.<br/>
    ///</summary>
    ref float GrabMinSize { get; }

    ///<summary>
    /// Radius of grabs corners rounding. Set to 0.0f to have rectangular slider grabs.<br/>
    ///</summary>
    ref float GrabRounding { get; }

    ///<summary>
    /// The size in pixels of the dead-zone around zero on logarithmic sliders that cross zero.<br/>
    ///</summary>
    ref float LogSliderDeadzone { get; }

    ///<summary>
    /// Thickness of border around Image() calls.<br/>
    ///</summary>
    ref float ImageBorderSize { get; }

    ///<summary>
    /// Radius of upper corners of a tab. Set to 0.0f to have rectangular tabs.<br/>
    ///</summary>
    ref float TabRounding { get; }

    ///<summary>
    /// Thickness of border around tabs.<br/>
    ///</summary>
    ref float TabBorderSize { get; }

    ///<summary>
    /// Minimum tab width, to make tabs larger than their contents. TabBar buttons are not affected.<br/>
    ///</summary>
    ref float TabMinWidthBase { get; }

    ///<summary>
    /// Minimum tab width after shrinking, when using ImGuiTabBarFlags_FittingPolicyMixed policy.<br/>
    ///</summary>
    ref float TabMinWidthShrink { get; }

    ///<summary>
    /// -1: always visible. 0.0f: visible when hovered. &gt;0.0f: visible when hovered if minimum width.<br/>
    ///</summary>
    ref float TabCloseButtonMinWidthSelected { get; }

    ///<summary>
    /// -1: always visible. 0.0f: visible when hovered. &gt;0.0f: visible when hovered if minimum width. FLT_MAX: never show close button when unselected.<br/>
    ///</summary>
    ref float TabCloseButtonMinWidthUnselected { get; }

    ///<summary>
    /// Thickness of tab-bar separator, which takes on the tab active color to denote focus.<br/>
    ///</summary>
    ref float TabBarBorderSize { get; }

    ///<summary>
    /// Thickness of tab-bar overline, which highlights the selected tab-bar.<br/>
    ///</summary>
    ref float TabBarOverlineSize { get; }

    ///<summary>
    /// Angle of angled headers (supported values range from -50.0f degrees to +50.0f degrees).<br/>
    ///</summary>
    ref float TableAngledHeadersAngle { get; }

    ///<summary>
    /// Alignment of angled headers within the cell<br/>
    ///</summary>
    ref Vector2 TableAngledHeadersTextAlign { get; }

    ///<summary>
    /// Default way to draw lines connecting TreeNode hierarchy. ImGuiTreeNodeFlags_DrawLinesNone or ImGuiTreeNodeFlags_DrawLinesFull or ImGuiTreeNodeFlags_DrawLinesToNodes.<br/>
    ///</summary>
    ref ImGuiTreeNodeFlags TreeLinesFlags { get; }

    ///<summary>
    /// Thickness of outlines when using ImGuiTreeNodeFlags_DrawLines.<br/>
    ///</summary>
    ref float TreeLinesSize { get; }

    ///<summary>
    /// Radius of lines connecting child nodes to the vertical line.<br/>
    ///</summary>
    ref float TreeLinesRounding { get; }

    ///<summary>
    /// Radius of the drag and drop target frame.<br/>
    ///</summary>
    ref float DragDropTargetRounding { get; }

    ///<summary>
    /// Thickness of the drag and drop target border.<br/>
    ///</summary>
    ref float DragDropTargetBorderSize { get; }

    ///<summary>
    /// Size to expand the drag and drop target from actual target item size.<br/>
    ///</summary>
    ref float DragDropTargetPadding { get; }

    ///<summary>
    /// Side of the color button in the ColorEdit4 widget (left/right). Defaults to ImGuiDir_Right.<br/>
    ///</summary>
    ref int ColorButtonPosition { get; }

    ///<summary>
    /// Alignment of button text when button is larger than text. Defaults to (0.5f, 0.5f) (centered).<br/>
    ///</summary>
    ref Vector2 ButtonTextAlign { get; }

    ///<summary>
    /// Alignment of selectable text. Defaults to (0.0f, 0.0f) (top-left aligned). It's generally important to keep this left-aligned if you want to lay multiple items on a same line.<br/>
    ///</summary>
    ref Vector2 SelectableTextAlign { get; }

    ///<summary>
    /// Thickness of border in SeparatorText()<br/>
    ///</summary>
    ref float SeparatorTextBorderSize { get; }

    ///<summary>
    /// Alignment of text within the separator. Defaults to (0.0f, 0.5f) (left aligned, center).<br/>
    ///</summary>
    ref Vector2 SeparatorTextAlign { get; }

    ///<summary>
    /// Horizontal offset of text from each edge of the separator + spacing on other axis. Generally small values. .y is recommended to be == FramePadding.y.<br/>
    ///</summary>
    ref Vector2 SeparatorTextPadding { get; }

    ///<summary>
    /// Apply to regular windows: amount which we enforce to keep visible when moving near edges of your screen.<br/>
    ///</summary>
    ref Vector2 DisplayWindowPadding { get; }

    ///<summary>
    /// Apply to every windows, menus, popups, tooltips: amount where we avoid displaying contents. Adjust if you cannot see the edges of your screen (e.g. on a TV where scaling has not been configured).<br/>
    ///</summary>
    ref Vector2 DisplaySafeAreaPadding { get; }

    ///<summary>
    /// Docking node has their own CloseButton() to close all docked windows.<br/>
    ///</summary>
    ref bool DockingNodeHasCloseButton { get; }

    ///<summary>
    /// Thickness of resizing border between docked windows<br/>
    ///</summary>
    ref float DockingSeparatorSize { get; }

    ///<summary>
    /// Scale software rendered mouse cursor (when io.MouseDrawCursor is enabled). We apply per-monitor DPI scaling over this scale. May be removed later.<br/>
    ///</summary>
    ref float MouseCursorScale { get; }

    ///<summary>
    /// Enable anti-aliased lines/borders. Disable if you are really tight on CPU/GPU. Latched at the beginning of the frame (copied to ImDrawList).<br/>
    ///</summary>
    ref bool AntiAliasedLines { get; }

    ///<summary>
    /// Enable anti-aliased lines/borders using textures where possible. Require backend to render with bilinear filtering (NOT point/nearest filtering). Latched at the beginning of the frame (copied to ImDrawList).<br/>
    ///</summary>
    ref bool AntiAliasedLinesUseTex { get; }

    ///<summary>
    /// Enable anti-aliased edges around filled shapes (rounded rectangles, circles, etc.). Disable if you are really tight on CPU/GPU. Latched at the beginning of the frame (copied to ImDrawList).<br/>
    ///</summary>
    ref bool AntiAliasedFill { get; }

    ///<summary>
    /// Tessellation tolerance when using PathBezierCurveTo() without a specific number of segments. Decrease for highly tessellated curves (higher quality, more polygons), increase to reduce quality.<br/>
    ///</summary>
    ref float CurveTessellationTol { get; }

    ///<summary>
    /// Maximum error (in pixels) allowed when using AddCircle()/AddCircleFilled() or drawing rounded corner rectangles with no explicit segment count specified. Decrease for higher quality but more geometry.<br/>
    ///</summary>
    ref float CircleTessellationMaxError { get; }

    ///<summary>
    /// Colors<br/>
    ///</summary>
    RangeAccessor<Vector4> Colors { get; }

    ///<summary>
    /// Delay for IsItemHovered(ImGuiHoveredFlags_Stationary). Time required to consider mouse stationary.<br/>
    ///<br/>
    /// Behaviors<br/>
    /// (It is possible to modify those fields mid-frame if specific behavior need it, unlike e.g. configuration fields in ImGuiIO)<br/>
    ///</summary>
    ref float HoverStationaryDelay { get; }

    ///<summary>
    /// Delay for IsItemHovered(ImGuiHoveredFlags_DelayShort). Usually used along with HoverStationaryDelay.<br/>
    ///</summary>
    ref float HoverDelayShort { get; }

    ///<summary>
    /// Delay for IsItemHovered(ImGuiHoveredFlags_DelayNormal). "<br/>
    ///</summary>
    ref float HoverDelayNormal { get; }

    ///<summary>
    /// Default flags when using IsItemHovered(ImGuiHoveredFlags_ForTooltip) or BeginItemTooltip()/SetItemTooltip() while using mouse.<br/>
    ///</summary>
    ref ImGuiHoveredFlags HoverFlagsForTooltipMouse { get; }

    ///<summary>
    /// Default flags when using IsItemHovered(ImGuiHoveredFlags_ForTooltip) or BeginItemTooltip()/SetItemTooltip() while using keyboard/gamepad.<br/>
    ///</summary>
    ref ImGuiHoveredFlags HoverFlagsForTooltipNav { get; }

    ///<summary>
    /// FIXME-WIP: Reference scale, as applied by ScaleAllSizes().<br/>
    ///<br/>
    /// [Internal]<br/>
    ///</summary>
    ref float _MainScale { get; }

    ///<summary>
    /// FIXME: Temporary hack until we finish remaining work.<br/>
    ///</summary>
    ref float _NextFrameFontSizeBase { get; }
}

///<summary>
/// [Internal] Storage used by IsKeyDown(), IsKeyPressed() etc functions.<br/>
/// If prior to 1.87 you used io.KeysDownDuration[] (which was marked as internal), you should use GetKeyData(key)-&gt;DownDuration and *NOT* io.KeysData[key]-&gt;DownDuration.<br/>
///</summary>
public unsafe interface IImGuiKeyData
{
    public nint NativePointer { get; }

    ///<summary>
    /// True for if key is down<br/>
    ///</summary>
    ref bool Down { get; }

    ///<summary>
    /// Duration the key has been down (&lt;0.0f: not pressed, 0.0f: just pressed, &gt;0.0f: time held)<br/>
    ///</summary>
    ref float DownDuration { get; }

    ///<summary>
    /// Last frame duration the key has been down<br/>
    ///</summary>
    ref float DownDurationPrev { get; }

    ///<summary>
    /// 0.0f..1.0f for gamepad values<br/>
    ///</summary>
    ref float AnalogValue { get; }
}

public unsafe interface IImGuiIO
{
    public nint NativePointer { get; }

    ///<summary>
    /// = 0               See ImGuiConfigFlags_ enum. Set by user/application. Keyboard/Gamepad navigation options, etc.<br/>
    ///</summary>
    ref ImGuiConfigFlags ConfigFlags { get; }

    ///<summary>
    /// = 0               See ImGuiBackendFlags_ enum. Set by backend (imgui_impl_xxx files or custom backend) to communicate features supported by the backend.<br/>
    ///</summary>
    ref ImGuiBackendFlags BackendFlags { get; }

    ///<summary>
    /// &lt;unset&gt;           Main display size, in pixels (== GetMainViewport()-&gt;Size). May change every frame.<br/>
    ///</summary>
    ref Vector2 DisplaySize { get; }

    ///<summary>
    /// = (1, 1)          Main display density. For retina display where window coordinates are different from framebuffer coordinates. This will affect font density + will end up in ImDrawData::FramebufferScale.<br/>
    ///</summary>
    ref Vector2 DisplayFramebufferScale { get; }

    ///<summary>
    /// = 1.0f/60.0f      Time elapsed since last frame, in seconds. May change every frame.<br/>
    ///</summary>
    ref float DeltaTime { get; }

    ///<summary>
    /// = 5.0f            Minimum time between saving positions/sizes to .ini file, in seconds.<br/>
    ///</summary>
    ref float IniSavingRate { get; }

    ///<summary>
    /// = "imgui.ini"     Path to .ini file (important: default "imgui.ini" is relative to current working dir!). Set NULL to disable automatic .ini loading/saving or if you want to manually call LoadIniSettingsXXX() / SaveIniSettingsXXX() functions.<br/>
    ///</summary>
    sbyte* IniFilename { get; set; }

    ///<summary>
    /// = "imgui_log.txt" Path to .log file (default parameter to ImGui::LogToFile when no file is specified).<br/>
    ///</summary>
    sbyte* LogFilename { get; set; }

    ///<summary>
    /// = NULL            Store your own data.<br/>
    ///</summary>
    void* UserData { get; set; }

    ///<summary>
    /// &lt;auto&gt;            Font atlas: load, rasterize and pack one or more fonts into a single texture.<br/>
    ///<br/>
    /// Font system<br/>
    ///</summary>
    IImFontAtlas Fonts { get; }

    ///<summary>
    /// = NULL            Font to use on NewFrame(). Use NULL to uses Fonts-&gt;Fonts[0].<br/>
    ///</summary>
    IImFont FontDefault { get; }

    ///<summary>
    /// = false           Allow user scaling text of individual window with Ctrl+Wheel.<br/>
    ///</summary>
    ref bool FontAllowUserScaling { get; }

    ///<summary>
    /// = false           Swap Activate&lt;&gt;Cancel (A&lt;&gt;B) buttons, matching typical "Nintendo/Japanese style" gamepad layout.<br/>
    ///<br/>
    /// Keyboard/Gamepad Navigation options<br/>
    ///</summary>
    ref bool ConfigNavSwapGamepadButtons { get; }

    ///<summary>
    /// = false           Directional/tabbing navigation teleports the mouse cursor. May be useful on TV/console systems where moving a virtual mouse is difficult. Will update io.MousePos and set io.WantSetMousePos=true.<br/>
    ///</summary>
    ref bool ConfigNavMoveSetMousePos { get; }

    ///<summary>
    /// = true            Sets io.WantCaptureKeyboard when io.NavActive is set.<br/>
    ///</summary>
    ref bool ConfigNavCaptureKeyboard { get; }

    ///<summary>
    /// = true            Pressing Escape can clear focused item + navigation id/highlight. Set to false if you want to always keep highlight on.<br/>
    ///</summary>
    ref bool ConfigNavEscapeClearFocusItem { get; }

    ///<summary>
    /// = false           Pressing Escape can clear focused window as well (super set of io.ConfigNavEscapeClearFocusItem).<br/>
    ///</summary>
    ref bool ConfigNavEscapeClearFocusWindow { get; }

    ///<summary>
    /// = true            Using directional navigation key makes the cursor visible. Mouse click hides the cursor.<br/>
    ///</summary>
    ref bool ConfigNavCursorVisibleAuto { get; }

    ///<summary>
    /// = false           Navigation cursor is always visible.<br/>
    ///</summary>
    ref bool ConfigNavCursorVisibleAlways { get; }

    ///<summary>
    /// = false           Simplified docking mode: disable window splitting, so docking is limited to merging multiple windows together into tab-bars.<br/>
    ///<br/>
    /// Docking options (when ImGuiConfigFlags_DockingEnable is set)<br/>
    ///</summary>
    ref bool ConfigDockingNoSplit { get; }

    ///<summary>
    /// = false           Simplified docking mode: disable window merging into a same tab-bar, so docking is limited to splitting windows.<br/>
    ///</summary>
    ref bool ConfigDockingNoDockingOver { get; }

    ///<summary>
    /// = false           Enable docking with holding Shift key (reduce visual noise, allows dropping in wider space)<br/>
    ///</summary>
    ref bool ConfigDockingWithShift { get; }

    ///<summary>
    /// = false           [BETA] [FIXME: This currently creates regression with auto-sizing and general overhead] Make every single floating window display within a docking node.<br/>
    ///</summary>
    ref bool ConfigDockingAlwaysTabBar { get; }

    ///<summary>
    /// = false           [BETA] Make window or viewport transparent when docking and only display docking boxes on the target viewport. Useful if rendering of multiple viewport cannot be synced. Best used with ConfigViewportsNoAutoMerge.<br/>
    ///</summary>
    ref bool ConfigDockingTransparentPayload { get; }

    ///<summary>
    /// = false;          Set to make all floating imgui windows always create their own viewport. Otherwise, they are merged into the main host viewports when overlapping it. May also set ImGuiViewportFlags_NoAutoMerge on individual viewport.<br/>
    ///<br/>
    /// Viewport options (when ImGuiConfigFlags_ViewportsEnable is set)<br/>
    ///</summary>
    ref bool ConfigViewportsNoAutoMerge { get; }

    ///<summary>
    /// = false           Disable default OS task bar icon flag for secondary viewports. When a viewport doesn't want a task bar icon, ImGuiViewportFlags_NoTaskBarIcon will be set on it.<br/>
    ///</summary>
    ref bool ConfigViewportsNoTaskBarIcon { get; }

    ///<summary>
    /// = true            Disable default OS window decoration flag for secondary viewports. When a viewport doesn't want window decorations, ImGuiViewportFlags_NoDecoration will be set on it. Enabling decoration can create subsequent issues at OS levels (e.g. minimum window size).<br/>
    ///</summary>
    ref bool ConfigViewportsNoDecoration { get; }

    ///<summary>
    /// = true            When false: set secondary viewports' ParentViewportId to main viewport ID by default. Expects the platform backend to setup a parent/child relationship between the OS windows based on this value. Some backend may ignore this. Set to true if you want viewports to automatically be parent of main viewport, otherwise all viewports will be top-level OS windows.<br/>
    ///</summary>
    ref bool ConfigViewportsNoDefaultParent { get; }

    ///<summary>
    ///= true  When a platform window is focused (e.g. using Alt+Tab, clicking Platform Title Bar), apply corresponding focus on imgui windows (may clear focus/active id from imgui windows location in other platform windows). In principle this is better enabled but we provide an opt-out, because some Linux window managers tend to eagerly focus windows (e.g. on mouse hover, or even a simple window pos/size change).<br/>
    ///</summary>
    ref bool ConfigViewportsPlatformFocusSetsImGuiFocus { get; }

    ///<summary>
    /// = false           [EXPERIMENTAL] Automatically overwrite style.FontScaleDpi when Monitor DPI changes. This will scale fonts but _NOT_ scale sizes/padding for now.<br/>
    ///<br/>
    /// DPI/Scaling options<br/>
    /// This may keep evolving during 1.92.x releases. Expect some turbulence.<br/>
    ///</summary>
    ref bool ConfigDpiScaleFonts { get; }

    ///<summary>
    /// = false           [EXPERIMENTAL] Scale Dear ImGui and Platform Windows when Monitor DPI changes.<br/>
    ///</summary>
    ref bool ConfigDpiScaleViewports { get; }

    ///<summary>
    /// = false           Request ImGui to draw a mouse cursor for you (if you are on a platform without a mouse cursor). Cannot be easily renamed to 'io.ConfigXXX' because this is frequently used by backend implementations.<br/>
    ///<br/>
    /// Miscellaneous options<br/>
    /// (you can visualize and interact with all options in 'Demo-&gt;Configuration')<br/>
    ///</summary>
    ref bool MouseDrawCursor { get; }

    ///<summary>
    /// = defined(__APPLE__)  Swap Cmd&lt;&gt;Ctrl keys + OS X style text editing cursor movement using Alt instead of Ctrl, Shortcuts using Cmd/Super instead of Ctrl, Line/Text Start and End using Cmd+Arrows instead of Home/End, Double click selects by word instead of selecting whole text, Multi-selection in lists uses Cmd/Super instead of Ctrl.<br/>
    ///</summary>
    ref bool ConfigMacOSXBehaviors { get; }

    ///<summary>
    /// = true            Enable input queue trickling: some types of events submitted during the same frame (e.g. button down + up) will be spread over multiple frames, improving interactions with low framerates.<br/>
    ///</summary>
    ref bool ConfigInputTrickleEventQueue { get; }

    ///<summary>
    /// = true            Enable blinking cursor (optional as some users consider it to be distracting).<br/>
    ///</summary>
    ref bool ConfigInputTextCursorBlink { get; }

    ///<summary>
    /// = false           [BETA] Pressing Enter will keep item active and select contents (single-line only).<br/>
    ///</summary>
    ref bool ConfigInputTextEnterKeepActive { get; }

    ///<summary>
    /// = false           [BETA] Enable turning DragXXX widgets into text input with a simple mouse click-release (without moving). Not desirable on devices without a keyboard.<br/>
    ///</summary>
    ref bool ConfigDragClickToInputText { get; }

    ///<summary>
    /// = true            Enable resizing of windows from their edges and from the lower-left corner. This requires ImGuiBackendFlags_HasMouseCursors for better mouse cursor feedback. (This used to be a per-window ImGuiWindowFlags_ResizeFromAnySide flag)<br/>
    ///</summary>
    ref bool ConfigWindowsResizeFromEdges { get; }

    ///<summary>
    /// = false       Enable allowing to move windows only when clicking on their title bar. Does not apply to windows without a title bar.<br/>
    ///</summary>
    ref bool ConfigWindowsMoveFromTitleBarOnly { get; }

    ///<summary>
    /// = false       [EXPERIMENTAL] Ctrl+C copy the contents of focused window into the clipboard. Experimental because: (1) has known issues with nested Begin/End pairs (2) text output quality varies (3) text output is in submission order rather than spatial order.<br/>
    ///</summary>
    ref bool ConfigWindowsCopyContentsWithCtrlC { get; }

    ///<summary>
    /// = true            Enable scrolling page by page when clicking outside the scrollbar grab. When disabled, always scroll to clicked location. When enabled, Shift+Click scrolls to clicked location.<br/>
    ///</summary>
    ref bool ConfigScrollbarScrollByPage { get; }

    ///<summary>
    /// = 60.0f           Timer (in seconds) to free transient windows/tables memory buffers when unused. Set to -1.0f to disable.<br/>
    ///</summary>
    ref float ConfigMemoryCompactTimer { get; }

    ///<summary>
    /// = 0.30f           Time for a double-click, in seconds.<br/>
    ///<br/>
    /// Inputs Behaviors<br/>
    /// (other variables, ones which are expected to be tweaked within UI code, are exposed in ImGuiStyle)<br/>
    ///</summary>
    ref float MouseDoubleClickTime { get; }

    ///<summary>
    /// = 6.0f            Distance threshold to stay in to validate a double-click, in pixels.<br/>
    ///</summary>
    ref float MouseDoubleClickMaxDist { get; }

    ///<summary>
    /// = 6.0f            Distance threshold before considering we are dragging.<br/>
    ///</summary>
    ref float MouseDragThreshold { get; }

    ///<summary>
    /// = 0.275f          When holding a key/button, time before it starts repeating, in seconds (for buttons in Repeat mode, etc.).<br/>
    ///</summary>
    ref float KeyRepeatDelay { get; }

    ///<summary>
    /// = 0.050f          When holding a key/button, rate at which it repeats, in seconds.<br/>
    ///</summary>
    ref float KeyRepeatRate { get; }

    ///<summary>
    /// = true        Enable error recovery support. Some errors won't be detected and lead to direct crashes if recovery is disabled.<br/>
    ///<br/>
    /// Options to configure Error Handling and how we handle recoverable errors [EXPERIMENTAL]<br/>
    /// - Error recovery is provided as a way to facilitate:<br/>
    ///    - Recovery after a programming error (native code or scripting language - the later tends to facilitate iterating on code while running).<br/>
    ///    - Recovery after running an exception handler or any error processing which may skip code after an error has been detected.<br/>
    /// - Error recovery is not perfect nor guaranteed! It is a feature to ease development.<br/>
    ///   You not are not supposed to rely on it in the course of a normal application run.<br/>
    /// - Functions that support error recovery are using IM_ASSERT_USER_ERROR() instead of IM_ASSERT().<br/>
    /// - By design, we do NOT allow error recovery to be 100% silent. One of the three options needs to be checked!<br/>
    /// - Always ensure that on programmers seats you have at minimum Asserts or Tooltips enabled when making direct imgui API calls!<br/>
    ///   Otherwise it would severely hinder your ability to catch and correct mistakes!<br/>
    /// Read https:github.com/ocornut/imgui/wiki/Error-Handling for details.<br/>
    /// - Programmer seats: keep asserts (default), or disable asserts and keep error tooltips (new and nice!)<br/>
    /// - Non-programmer seats: maybe disable asserts, but make sure errors are resurfaced (tooltips, visible log entries, use callback etc.)<br/>
    /// - Recovery after error/exception: record stack sizes with ErrorRecoveryStoreState(), disable assert, set log callback (to e.g. trigger high-level breakpoint), recover with ErrorRecoveryTryToRecoverState(), restore settings.<br/>
    ///</summary>
    ref bool ConfigErrorRecovery { get; }

    ///<summary>
    /// = true        Enable asserts on recoverable error. By default call IM_ASSERT() when returning from a failing IM_ASSERT_USER_ERROR()<br/>
    ///</summary>
    ref bool ConfigErrorRecoveryEnableAssert { get; }

    ///<summary>
    /// = true        Enable debug log output on recoverable errors.<br/>
    ///</summary>
    ref bool ConfigErrorRecoveryEnableDebugLog { get; }

    ///<summary>
    /// = true        Enable tooltip on recoverable errors. The tooltip include a way to enable asserts if they were disabled.<br/>
    ///</summary>
    ref bool ConfigErrorRecoveryEnableTooltip { get; }

    ///<summary>
    /// = false           Enable various tools calling IM_DEBUG_BREAK().<br/>
    ///<br/>
    /// Option to enable various debug tools showing buttons that will call the IM_DEBUG_BREAK() macro.<br/>
    /// - The Item Picker tool will be available regardless of this being enabled, in order to maximize its discoverability.<br/>
    /// - Requires a debugger being attached, otherwise IM_DEBUG_BREAK() options will appear to crash your application.<br/>
    ///   e.g. io.ConfigDebugIsDebuggerPresent = ::IsDebuggerPresent() on Win32, or refer to ImOsIsDebuggerPresent() imgui_test_engine/imgui_te_utils.cpp for a Unix compatible version.<br/>
    ///</summary>
    ref bool ConfigDebugIsDebuggerPresent { get; }

    ///<summary>
    /// = true            Highlight and show an error message popup when multiple items have conflicting identifiers.<br/>
    ///<br/>
    /// Tools to detect code submitting items with conflicting/duplicate IDs<br/>
    /// - Code should use PushID()/PopID() in loops, or append "##xx" to same-label identifiers.<br/>
    /// - Empty label e.g. Button("") == same ID as parent widget/node. Use Button("##xx") instead!<br/>
    /// - See FAQ https:github.com/ocornut/imgui/blob/master/docs/FAQ.md#q-about-the-id-stack-system<br/>
    ///</summary>
    ref bool ConfigDebugHighlightIdConflicts { get; }

    ///<summary>
    ///=true  Show "Item Picker" button in aforementioned popup.<br/>
    ///</summary>
    ref bool ConfigDebugHighlightIdConflictsShowItemPicker { get; }

    ///<summary>
    /// = false           First-time calls to Begin()/BeginChild() will return false. NEEDS TO BE SET AT APPLICATION BOOT TIME if you don't want to miss windows.<br/>
    ///<br/>
    /// Tools to test correct Begin/End and BeginChild/EndChild behaviors.<br/>
    /// - Presently Begin()/End() and BeginChild()/EndChild() needs to ALWAYS be called in tandem, regardless of return value of BeginXXX()<br/>
    /// - This is inconsistent with other BeginXXX functions and create confusion for many users.<br/>
    /// - We expect to update the API eventually. In the meanwhile we provide tools to facilitate checking user-code behavior.<br/>
    ///</summary>
    ref bool ConfigDebugBeginReturnValueOnce { get; }

    ///<summary>
    /// = false           Some calls to Begin()/BeginChild() will return false. Will cycle through window depths then repeat. Suggested use: add "io.ConfigDebugBeginReturnValue = io.KeyShift" in your main loop then occasionally press SHIFT. Windows should be flickering while running.<br/>
    ///</summary>
    ref bool ConfigDebugBeginReturnValueLoop { get; }

    ///<summary>
    /// = false           Ignore io.AddFocusEvent(false), consequently not calling io.ClearInputKeys()/io.ClearInputMouse() in input processing.<br/>
    ///<br/>
    /// Option to deactivate io.AddFocusEvent(false) handling.<br/>
    /// - May facilitate interactions with a debugger when focus loss leads to clearing inputs data.<br/>
    /// - Backends may have other side-effects on focus loss, so this will reduce side-effects but not necessary remove all of them.<br/>
    ///</summary>
    ref bool ConfigDebugIgnoreFocusLoss { get; }

    ///<summary>
    /// = false           Save .ini data with extra comments (particularly helpful for Docking, but makes saving slower)<br/>
    ///<br/>
    /// Option to audit .ini data<br/>
    ///</summary>
    ref bool ConfigDebugIniSettings { get; }

    ///<summary>
    /// = NULL<br/>
    ///<br/>
    /// Nowadays those would be stored in ImGuiPlatformIO but we are leaving them here for legacy reasons.<br/>
    /// Optional: Platform/Renderer backend name (informational only! will be displayed in About Window) + User data for backend/wrappers to store their own stuff.<br/>
    ///</summary>
    sbyte* BackendPlatformName { get; set; }

    ///<summary>
    /// = NULL<br/>
    ///</summary>
    sbyte* BackendRendererName { get; set; }

    ///<summary>
    /// = NULL            User data for platform backend<br/>
    ///</summary>
    void* BackendPlatformUserData { get; set; }

    ///<summary>
    /// = NULL            User data for renderer backend<br/>
    ///</summary>
    void* BackendRendererUserData { get; set; }

    ///<summary>
    /// = NULL            User data for non C++ programming language backend<br/>
    ///</summary>
    void* BackendLanguageUserData { get; set; }

    ///<summary>
    /// Set when Dear ImGui will use mouse inputs, in this case do not dispatch them to your main game/application (either way, always pass on mouse inputs to imgui). (e.g. unclicked mouse is hovering over an imgui window, widget is active, mouse was clicked over an imgui window, etc.).<br/>
    ///</summary>
    ref bool WantCaptureMouse { get; }

    ///<summary>
    /// Set when Dear ImGui will use keyboard inputs, in this case do not dispatch them to your main game/application (either way, always pass keyboard inputs to imgui). (e.g. InputText active, or an imgui window is focused and navigation is enabled, etc.).<br/>
    ///</summary>
    ref bool WantCaptureKeyboard { get; }

    ///<summary>
    /// Mobile/console: when set, you may display an on-screen keyboard. This is set by Dear ImGui when it wants textual keyboard input to happen (e.g. when a InputText widget is active).<br/>
    ///</summary>
    ref bool WantTextInput { get; }

    ///<summary>
    /// MousePos has been altered, backend should reposition mouse on next frame. Rarely used! Set only when io.ConfigNavMoveSetMousePos is enabled.<br/>
    ///</summary>
    ref bool WantSetMousePos { get; }

    ///<summary>
    /// When manual .ini load/save is active (io.IniFilename == NULL), this will be set to notify your application that you can call SaveIniSettingsToMemory() and save yourself. Important: clear io.WantSaveIniSettings yourself after saving!<br/>
    ///</summary>
    ref bool WantSaveIniSettings { get; }

    ///<summary>
    /// Keyboard/Gamepad navigation is currently allowed (will handle ImGuiKey_NavXXX events) = a window is focused and it doesn't use the ImGuiWindowFlags_NoNavInputs flag.<br/>
    ///</summary>
    ref bool NavActive { get; }

    ///<summary>
    /// Keyboard/Gamepad navigation highlight is visible and allowed (will handle ImGuiKey_NavXXX events).<br/>
    ///</summary>
    ref bool NavVisible { get; }

    ///<summary>
    /// Estimate of application framerate (rolling average over 60 frames, based on io.DeltaTime), in frame per second. Solely for convenience. Slow applications may not want to use a moving average or may want to reset underlying buffers occasionally.<br/>
    ///</summary>
    ref float Framerate { get; }

    ///<summary>
    /// Vertices output during last call to Render()<br/>
    ///</summary>
    ref int MetricsRenderVertices { get; }

    ///<summary>
    /// Indices output during last call to Render() = number of triangles * 3<br/>
    ///</summary>
    ref int MetricsRenderIndices { get; }

    ///<summary>
    /// Number of visible windows<br/>
    ///</summary>
    ref int MetricsRenderWindows { get; }

    ///<summary>
    /// Number of active windows<br/>
    ///</summary>
    ref int MetricsActiveWindows { get; }

    ///<summary>
    /// Mouse delta. Note that this is zero if either current or previous position are invalid (-FLT_MAX,-FLT_MAX), so a disappearing/reappearing mouse won't have a huge delta.<br/>
    ///</summary>
    ref Vector2 MouseDelta { get; }

    ///<summary>
    /// Parent UI context (needs to be set explicitly by parent).<br/>
    ///</summary>
    IImGuiContext Ctx { get; }

    ///<summary>
    /// Mouse position, in pixels. Set to ImVec2(-FLT_MAX, -FLT_MAX) if mouse is unavailable (on another screen, etc.)<br/>
    ///<br/>
    /// Main Input State<br/>
    /// (this block used to be written by backend, since 1.87 it is best to NOT write to those directly, call the AddXXX functions above instead)<br/>
    /// (reading from those variables is fair game, as they are extremely unlikely to be moving anywhere)<br/>
    ///</summary>
    ref Vector2 MousePos { get; }

    ///<summary>
    /// Mouse buttons: 0=left, 1=right, 2=middle + extras (ImGuiMouseButton_COUNT == 5). Dear ImGui mostly uses left and right buttons. Other buttons allow us to track if the mouse is being used by your application + available to user as a convenience via IsMouse** API.<br/>
    ///</summary>
    RangeAccessor<bool> MouseDown { get; }

    ///<summary>
    /// Mouse wheel Vertical: 1 unit scrolls about 5 lines text. &gt;0 scrolls Up, &lt;0 scrolls Down. Hold Shift to turn vertical scroll into horizontal scroll.<br/>
    ///</summary>
    ref float MouseWheel { get; }

    ///<summary>
    /// Mouse wheel Horizontal. &gt;0 scrolls Left, &lt;0 scrolls Right. Most users don't have a mouse with a horizontal wheel, may not be filled by all backends.<br/>
    ///</summary>
    ref float MouseWheelH { get; }

    ///<summary>
    /// Mouse actual input peripheral (Mouse/TouchScreen/Pen).<br/>
    ///</summary>
    ref int MouseSource { get; }

    ///<summary>
    /// (Optional) Modify using io.AddMouseViewportEvent(). With multi-viewports: viewport the OS mouse is hovering. If possible _IGNORING_ viewports with the ImGuiViewportFlags_NoInputs flag is much better (few backends can handle that). Set io.BackendFlags |= ImGuiBackendFlags_HasMouseHoveredViewport if you can provide this info. If you don't imgui will infer the value using the rectangles and last focused time of the viewports it knows about (ignoring other OS windows).<br/>
    ///</summary>
    ref uint MouseHoveredViewport { get; }

    ///<summary>
    /// Keyboard modifier down: Ctrl (non-macOS), Cmd (macOS)<br/>
    ///</summary>
    ref bool KeyCtrl { get; }

    ///<summary>
    /// Keyboard modifier down: Shift<br/>
    ///</summary>
    ref bool KeyShift { get; }

    ///<summary>
    /// Keyboard modifier down: Alt<br/>
    ///</summary>
    ref bool KeyAlt { get; }

    ///<summary>
    /// Keyboard modifier down: Windows/Super (non-macOS), Ctrl (macOS)<br/>
    ///</summary>
    ref bool KeySuper { get; }

    ///<summary>
    /// Key mods flags (any of ImGuiMod_Ctrl/ImGuiMod_Shift/ImGuiMod_Alt/ImGuiMod_Super flags, same as io.KeyCtrl/KeyShift/KeyAlt/KeySuper but merged into flags). Read-only, updated by NewFrame()<br/>
    ///<br/>
    /// Other state maintained from data above + IO function calls<br/>
    ///</summary>
    ref int KeyMods { get; }

    ///<summary>
    /// Key state for all known keys. MUST use 'key - ImGuiKey_NamedKey_BEGIN' as index. Use IsKeyXXX() functions to access this.<br/>
    ///</summary>
    RangeStructAccessor<IImGuiKeyData> KeysData { get; }

    ///<summary>
    /// Alternative to WantCaptureMouse: (WantCaptureMouse == true &amp;&amp; WantCaptureMouseUnlessPopupClose == false) when a click over void is expected to close a popup.<br/>
    ///</summary>
    ref bool WantCaptureMouseUnlessPopupClose { get; }

    ///<summary>
    /// Previous mouse position (note that MouseDelta is not necessary == MousePos-MousePosPrev, in case either position is invalid)<br/>
    ///</summary>
    ref Vector2 MousePosPrev { get; }

    ///<summary>
    /// Position at time of clicking<br/>
    ///</summary>
    RangeAccessor<Vector2> MouseClickedPos { get; }

    ///<summary>
    /// Time of last click (used to figure out double-click)<br/>
    ///</summary>
    RangeAccessor<double> MouseClickedTime { get; }

    ///<summary>
    /// Mouse button went from !Down to Down (same as MouseClickedCount[x] != 0)<br/>
    ///</summary>
    RangeAccessor<bool> MouseClicked { get; }

    ///<summary>
    /// Has mouse button been double-clicked? (same as MouseClickedCount[x] == 2)<br/>
    ///</summary>
    RangeAccessor<bool> MouseDoubleClicked { get; }

    ///<summary>
    /// == 0 (not clicked), == 1 (same as MouseClicked[]), == 2 (double-clicked), == 3 (triple-clicked) etc. when going from !Down to Down<br/>
    ///</summary>
    RangeAccessor<ushort> MouseClickedCount { get; }

    ///<summary>
    /// Count successive number of clicks. Stays valid after mouse release. Reset after another click is done.<br/>
    ///</summary>
    RangeAccessor<ushort> MouseClickedLastCount { get; }

    ///<summary>
    /// Mouse button went from Down to !Down<br/>
    ///</summary>
    RangeAccessor<bool> MouseReleased { get; }

    ///<summary>
    /// Time of last released (rarely used! but useful to handle delayed single-click when trying to disambiguate them from double-click).<br/>
    ///</summary>
    RangeAccessor<double> MouseReleasedTime { get; }

    ///<summary>
    /// Track if button was clicked inside a dear imgui window or over void blocked by a popup. We don't request mouse capture from the application if click started outside ImGui bounds.<br/>
    ///</summary>
    RangeAccessor<bool> MouseDownOwned { get; }

    ///<summary>
    /// Track if button was clicked inside a dear imgui window.<br/>
    ///</summary>
    RangeAccessor<bool> MouseDownOwnedUnlessPopupClose { get; }

    ///<summary>
    /// On a non-Mac system, holding Shift requests WheelY to perform the equivalent of a WheelX event. On a Mac system this is already enforced by the system.<br/>
    ///</summary>
    ref bool MouseWheelRequestAxisSwap { get; }

    ///<summary>
    /// (OSX) Set to true when the current click was a Ctrl+Click that spawned a simulated right click<br/>
    ///</summary>
    ref bool MouseCtrlLeftAsRightClick { get; }

    ///<summary>
    /// Duration the mouse button has been down (0.0f == just clicked)<br/>
    ///</summary>
    RangeAccessor<float> MouseDownDuration { get; }

    ///<summary>
    /// Previous time the mouse button has been down<br/>
    ///</summary>
    RangeAccessor<float> MouseDownDurationPrev { get; }

    ///<summary>
    /// Maximum distance, absolute, on each axis, of how much mouse has traveled from the clicking point<br/>
    ///</summary>
    RangeAccessor<Vector2> MouseDragMaxDistanceAbs { get; }

    ///<summary>
    /// Squared maximum distance of how much mouse has traveled from the clicking point (used for moving thresholds)<br/>
    ///</summary>
    RangeAccessor<float> MouseDragMaxDistanceSqr { get; }

    ///<summary>
    /// Touch/Pen pressure (0.0f to 1.0f, should be &gt;0.0f only when MouseDown[0] == true). Helper storage currently unused by Dear ImGui.<br/>
    ///</summary>
    ref float PenPressure { get; }

    ///<summary>
    /// Only modify via AddFocusEvent()<br/>
    ///</summary>
    ref bool AppFocusLost { get; }

    ///<summary>
    /// Only modify via SetAppAcceptingEvents()<br/>
    ///</summary>
    ref bool AppAcceptingEvents { get; }

    ///<summary>
    /// For AddInputCharacterUTF16()<br/>
    ///</summary>
    ref ushort InputQueueSurrogate { get; }

    ///<summary>
    /// Queue of _characters_ input (obtained by platform backend). Fill using AddInputCharacter() helper.<br/>
    ///</summary>
    ImVector<uint> InputQueueCharacters { get; }

    ///<summary>
    /// Moved io.FontGlobalScale to style.FontScaleMain in 1.92 (June 2025)<br/>
    ///</summary>
    ref float FontGlobalScale { get; }

    ///<summary>
    /// Legacy: before 1.91.1, clipboard functions were stored in ImGuiIO instead of ImGuiPlatformIO.<br/>
    /// As this is will affect all users of custom engines/backends, we are providing proper legacy redirection (will obsolete).<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint> GetClipboardTextFn { get; set; }

    delegate* unmanaged[Cdecl]<nint, nint, void> SetClipboardTextFn { get; set; }

    void* ClipboardUserData { get; set; }
}

///<summary>
/// Shared state of InputText(), passed as an argument to your callback when a ImGuiInputTextFlags_Callback* flag is used.<br/>
/// The callback function should return 0 by default.<br/>
/// Callbacks (follow a flag name and see comments in ImGuiInputTextFlags_ declarations for more details)<br/>
/// - ImGuiInputTextFlags_CallbackEdit:        Callback on buffer edit. Note that InputText() already returns true on edit + you can always use IsItemEdited(). The callback is useful to manipulate the underlying buffer while focus is active.<br/>
/// - ImGuiInputTextFlags_CallbackAlways:      Callback on each iteration<br/>
/// - ImGuiInputTextFlags_CallbackCompletion:  Callback on pressing TAB<br/>
/// - ImGuiInputTextFlags_CallbackHistory:     Callback on pressing Up/Down arrows<br/>
/// - ImGuiInputTextFlags_CallbackCharFilter:  Callback on character inputs to replace or discard them. Modify 'EventChar' to replace or discard, or return 1 in callback to discard.<br/>
/// - ImGuiInputTextFlags_CallbackResize:      Callback on buffer capacity changes request (beyond 'buf_size' parameter value), allowing the string to grow.<br/>
///</summary>
public unsafe interface IImGuiInputTextCallbackData
{
    public nint NativePointer { get; }

    ///<summary>
    /// Parent UI context<br/>
    ///</summary>
    IImGuiContext Ctx { get; }

    ///<summary>
    /// One ImGuiInputTextFlags_Callback*     Read-only<br/>
    ///</summary>
    ref ImGuiInputTextFlags EventFlag { get; }

    ///<summary>
    /// What user passed to InputText()       Read-only<br/>
    ///</summary>
    ref ImGuiInputTextFlags Flags { get; }

    ///<summary>
    /// What user passed to InputText()       Read-only<br/>
    ///</summary>
    void* UserData { get; set; }

    ///<summary>
    /// Character input                       Read-write    [CharFilter] Replace character with another one, or set to zero to drop. return 1 is equivalent to setting EventChar=0;<br/>
    ///<br/>
    /// Arguments for the different callback events<br/>
    /// - During Resize callback, Buf will be same as your input buffer.<br/>
    /// - However, during Completion/History/Always callback, Buf always points to our own internal data (it is not the same as your buffer)! Changes to it will be reflected into your own buffer shortly after the callback.<br/>
    /// - To modify the text buffer in a callback, prefer using the InsertChars() / DeleteChars() function. InsertChars() will take care of calling the resize callback if necessary.<br/>
    /// - If you know your edits are not going to resize the underlying buffer allocation, you may modify the contents of 'Buf[]' directly. You need to update 'BufTextLen' accordingly (0 &lt;= BufTextLen &lt; BufSize) and set 'BufDirty'' to true so InputText can update its internal state.<br/>
    ///</summary>
    ref uint EventChar { get; }

    ///<summary>
    /// Key pressed (Up/Down/TAB)             Read-only     [Completion,History]<br/>
    ///</summary>
    ref int EventKey { get; }

    ///<summary>
    /// Text buffer                           Read-write    [Resize] Can replace pointer / [Completion,History,Always] Only write to pointed data, don't replace the actual pointer!<br/>
    ///</summary>
    sbyte* Buf { get; set; }

    ///<summary>
    /// Text length (in bytes)                Read-write    [Resize,Completion,History,Always] Exclude zero-terminator storage. In C land: == strlen(some_text), in C++ land: string.length()<br/>
    ///</summary>
    ref int BufTextLen { get; }

    ///<summary>
    /// Buffer size (in bytes) = capacity+1   Read-only     [Resize,Completion,History,Always] Include zero-terminator storage. In C land: == ARRAYSIZE(my_char_array), in C++ land: string.capacity()+1<br/>
    ///</summary>
    ref int BufSize { get; }

    ///<summary>
    /// Set if you modify Buf/BufTextLen!     Write         [Completion,History,Always]<br/>
    ///</summary>
    ref bool BufDirty { get; }

    ///<summary>
    ///                                       Read-write    [Completion,History,Always]<br/>
    ///</summary>
    ref int CursorPos { get; }

    ///<summary>
    ///                                       Read-write    [Completion,History,Always] == to SelectionEnd when no selection<br/>
    ///</summary>
    ref int SelectionStart { get; }

    ///<summary>
    ///                                       Read-write    [Completion,History,Always]<br/>
    ///</summary>
    ref int SelectionEnd { get; }
}

///<summary>
/// Resizing callback data to apply custom constraint. As enabled by SetNextWindowSizeConstraints(). Callback is called during the next Begin().<br/>
/// NB: For basic min/max size constraint on each axis you don't need to use the callback! The SetNextWindowSizeConstraints() parameters are enough.<br/>
///</summary>
public unsafe interface IImGuiSizeCallbackData
{
    public nint NativePointer { get; }

    ///<summary>
    /// Read-only.   What user passed to SetNextWindowSizeConstraints(). Generally store an integer or float in here (need reinterpret_cast&lt;&gt;).<br/>
    ///</summary>
    void* UserData { get; set; }

    ///<summary>
    /// Read-only.   Window position, for reference.<br/>
    ///</summary>
    ref Vector2 Pos { get; }

    ///<summary>
    /// Read-only.   Current window size.<br/>
    ///</summary>
    ref Vector2 CurrentSize { get; }

    ///<summary>
    /// Read-write.  Desired size, based on user's mouse position. Write to this field to restrain resizing.<br/>
    ///</summary>
    ref Vector2 DesiredSize { get; }
}

///<summary>
/// [ALPHA] Rarely used / very advanced uses only. Use with SetNextWindowClass() and DockSpace() functions.<br/>
/// Important: the content of this class is still highly WIP and likely to change and be refactored<br/>
/// before we stabilize Docking features. Please be mindful if using this.<br/>
/// Provide hints:<br/>
/// - To the platform backend via altered viewport flags (enable/disable OS decoration, OS task bar icons, etc.)<br/>
/// - To the platform backend for OS level parent/child relationships of viewport.<br/>
/// - To the docking system for various options and filtering.<br/>
///</summary>
public unsafe interface IImGuiWindowClass
{
    public nint NativePointer { get; }

    ///<summary>
    /// User data. 0 = Default class (unclassed). Windows of different classes cannot be docked with each others.<br/>
    ///</summary>
    ref uint ClassId { get; }

    ///<summary>
    /// Hint for the platform backend. -1: use default. 0: request platform backend to not parent the platform. != 0: request platform backend to create a parent&lt;&gt;child relationship between the platform windows. Not conforming backends are free to e.g. parent every viewport to the main viewport or not.<br/>
    ///</summary>
    ref uint ParentViewportId { get; }

    ///<summary>
    /// ID of parent window for shortcut focus route evaluation, e.g. Shortcut() call from Parent Window will succeed when this window is focused.<br/>
    ///</summary>
    ref uint FocusRouteParentWindowId { get; }

    ///<summary>
    /// Viewport flags to set when a window of this class owns a viewport. This allows you to enforce OS decoration or task bar icon, override the defaults on a per-window basis.<br/>
    ///</summary>
    ref ImGuiViewportFlags ViewportFlagsOverrideSet { get; }

    ///<summary>
    /// Viewport flags to clear when a window of this class owns a viewport. This allows you to enforce OS decoration or task bar icon, override the defaults on a per-window basis.<br/>
    ///</summary>
    ref ImGuiViewportFlags ViewportFlagsOverrideClear { get; }

    ///<summary>
    /// [EXPERIMENTAL] TabItem flags to set when a window of this class gets submitted into a dock node tab bar. May use with ImGuiTabItemFlags_Leading or ImGuiTabItemFlags_Trailing.<br/>
    ///</summary>
    ref ImGuiTabItemFlags TabItemFlagsOverrideSet { get; }

    ///<summary>
    /// [EXPERIMENTAL] Dock node flags to set when a window of this class is hosted by a dock node (it doesn't have to be selected!)<br/>
    ///</summary>
    ref ImGuiDockNodeFlags DockNodeFlagsOverrideSet { get; }

    ///<summary>
    /// Set to true to enforce single floating windows of this class always having their own docking node (equivalent of setting the global io.ConfigDockingAlwaysTabBar)<br/>
    ///</summary>
    ref bool DockingAlwaysTabBar { get; }

    ///<summary>
    /// Set to true to allow windows of this class to be docked/merged with an unclassed window.  FIXME-DOCK: Move to DockNodeFlags override?<br/>
    ///</summary>
    ref bool DockingAllowUnclassed { get; }
}

///<summary>
/// Data payload for Drag and Drop operations: AcceptDragDropPayload(), GetDragDropPayload()<br/>
///</summary>
public unsafe interface IImGuiPayload
{
    public nint NativePointer { get; }

    ///<summary>
    /// Data (copied and owned by dear imgui)<br/>
    ///<br/>
    /// Members<br/>
    ///</summary>
    void* Data { get; set; }

    ///<summary>
    /// Data size<br/>
    ///</summary>
    ref int DataSize { get; }

    ///<summary>
    /// Source item id<br/>
    ///<br/>
    /// [Internal]<br/>
    ///</summary>
    ref uint SourceId { get; }

    ///<summary>
    /// Source parent id (if available)<br/>
    ///</summary>
    ref uint SourceParentId { get; }

    ///<summary>
    /// Data timestamp<br/>
    ///</summary>
    ref int DataFrameCount { get; }

    ///<summary>
    /// Data type tag (short user-supplied string, 32 characters max)<br/>
    ///</summary>
    RangeAccessor<byte> DataType { get; }

    ///<summary>
    /// Set when AcceptDragDropPayload() was called and mouse has been hovering the target item (nb: handle overlapping drag targets)<br/>
    ///</summary>
    ref bool Preview { get; }

    ///<summary>
    /// Set when AcceptDragDropPayload() was called and mouse button is released over the target item.<br/>
    ///</summary>
    ref bool Delivery { get; }
}

///<summary>
/// [Internal]<br/>
///</summary>
public unsafe interface IImGuiTextFilter_ImGuiTextRange
{
    public nint NativePointer { get; }

    sbyte* b { get; set; }

    sbyte* e { get; set; }
}

///<summary>
/// Helper: Parse and apply text filters. In format "aaaaa[,bbbb][,ccccc]"<br/>
///</summary>
public unsafe interface IImGuiTextFilter
{
    public nint NativePointer { get; }

    RangeAccessor<byte> InputBuf { get; }

    ImStructVectorWrapper<IImGuiTextFilter_ImGuiTextRange> Filters { get; }

    ref int CountGrep { get; }
}

///<summary>
/// Helper: Growable text buffer for logging/accumulating text<br/>
/// (this could be called 'ImGuiTextBuilder' / 'ImGuiStringBuilder')<br/>
///</summary>
public unsafe interface IImGuiTextBuffer
{
    public nint NativePointer { get; }

    ImVector<byte> Buf { get; }
}

///<summary>
/// Helper: Key-&gt;Value storage<br/>
/// Typically you don't have to worry about this since a storage is held within each Window.<br/>
/// We use it to e.g. store collapse state for a tree (Int 0/1)<br/>
/// This is optimized for efficient lookup (dichotomy into a contiguous buffer) and rare insertion (typically tied to user interactions aka max once a frame)<br/>
/// You can use it as custom user storage for temporary values. Declare your own storage if, for example:<br/>
/// - You want to manipulate the open/close state of a particular sub-tree in your interface (tree node uses Int 0/1 to store their state).<br/>
/// - You want to store custom debug data easily without adding or editing structures in your code (probably not efficient, but convenient)<br/>
/// Types are NOT stored, so it is up to you to make sure your Key don't collide with different types.<br/>
///</summary>
public unsafe interface IImGuiStorage
{
    public nint NativePointer { get; }

    ///<summary>
    /// [Internal]<br/>
    ///</summary>
    ImStructVectorWrapper<IImGuiStoragePair> Data { get; }
}

///<summary>
/// Flags for ImGuiListClipper (currently not fully exposed in function calls: a future refactor will likely add this to ImGuiListClipper::Begin function equivalent)<br/>
///</summary>
public enum ImGuiListClipperFlags
{
    ImGuiListClipperFlags_None = 0,
    ///<summary>
    /// [Internal] Disabled modifying table row counters. Avoid assumption that 1 clipper item == 1 table row.<br/>
    ///</summary>
    ImGuiListClipperFlags_NoSetTableRowCounters = 1 << 0
}

///<summary>
/// Helper: Manually clip large list of items.<br/>
/// If you have lots evenly spaced items and you have random access to the list, you can perform coarse<br/>
/// clipping based on visibility to only submit items that are in view.<br/>
/// The clipper calculates the range of visible items and advance the cursor to compensate for the non-visible items we have skipped.<br/>
/// (Dear ImGui already clip items based on their bounds but: it needs to first layout the item to do so, and generally<br/>
///  fetching/submitting your own data incurs additional cost. Coarse clipping using ImGuiListClipper allows you to easily<br/>
///  scale using lists with tens of thousands of items without a problem)<br/>
/// Usage:<br/>
///   ImGuiListClipper clipper;<br/>
///   clipper.Begin(1000);          We have 1000 elements, evenly spaced.<br/>
///   while (clipper.Step())<br/>
///       for (int i = clipper.DisplayStart; i &lt; clipper.DisplayEnd; i++)<br/>
///           ImGui::Text("line number %d", i);<br/>
/// Generally what happens is:<br/>
/// - Clipper lets you process the first element (DisplayStart = 0, DisplayEnd = 1) regardless of it being visible or not.<br/>
/// - User code submit that one element.<br/>
/// - Clipper can measure the height of the first element<br/>
/// - Clipper calculate the actual range of elements to display based on the current clipping rectangle, position the cursor before the first visible element.<br/>
/// - User code submit visible elements.<br/>
/// - The clipper also handles various subtleties related to keyboard/gamepad navigation, wrapping etc.<br/>
///</summary>
public unsafe interface IImGuiListClipper
{
    public nint NativePointer { get; }

    ///<summary>
    /// Parent UI context<br/>
    ///</summary>
    IImGuiContext Ctx { get; }

    ///<summary>
    /// First item to display, updated by each call to Step()<br/>
    ///</summary>
    ref int DisplayStart { get; }

    ///<summary>
    /// End of items to display (exclusive)<br/>
    ///</summary>
    ref int DisplayEnd { get; }

    ///<summary>
    /// [Internal] Number of items<br/>
    ///</summary>
    ref int ItemsCount { get; }

    ///<summary>
    /// [Internal] Height of item after a first step and item submission can calculate it<br/>
    ///</summary>
    ref float ItemsHeight { get; }

    ///<summary>
    /// [Internal] Cursor position at the time of Begin() or after table frozen rows are all processed<br/>
    ///</summary>
    ref double StartPosY { get; }

    ///<summary>
    /// [Internal] Account for frozen rows in a table and initial loss of precision in very large windows.<br/>
    ///</summary>
    ref double StartSeekOffsetY { get; }

    ///<summary>
    /// [Internal] Internal data<br/>
    ///</summary>
    void* TempData { get; set; }

    ///<summary>
    /// [Internal] Flags, currently not yet well exposed.<br/>
    ///</summary>
    ref int Flags { get; }
}

///<summary>
/// Flags for BeginMultiSelect()<br/>
///</summary>
public enum ImGuiMultiSelectFlags
{
    ImGuiMultiSelectFlags_None = 0,
    ///<summary>
    /// Disable selecting more than one item. This is available to allow single-selection code to share same code/logic if desired. It essentially disables the main purpose of BeginMultiSelect() tho!<br/>
    ///</summary>
    ImGuiMultiSelectFlags_SingleSelect = 1 << 0,
    ///<summary>
    /// Disable Ctrl+A shortcut to select all.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_NoSelectAll = 1 << 1,
    ///<summary>
    /// Disable Shift+selection mouse/keyboard support (useful for unordered 2D selection). With BoxSelect is also ensure contiguous SetRange requests are not combined into one. This allows not handling interpolation in SetRange requests.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_NoRangeSelect = 1 << 2,
    ///<summary>
    /// Disable selecting items when navigating (useful for e.g. supporting range-select in a list of checkboxes).<br/>
    ///</summary>
    ImGuiMultiSelectFlags_NoAutoSelect = 1 << 3,
    ///<summary>
    /// Disable clearing selection when navigating or selecting another one (generally used with ImGuiMultiSelectFlags_NoAutoSelect. useful for e.g. supporting range-select in a list of checkboxes).<br/>
    ///</summary>
    ImGuiMultiSelectFlags_NoAutoClear = 1 << 4,
    ///<summary>
    /// Disable clearing selection when clicking/selecting an already selected item.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_NoAutoClearOnReselect = 1 << 5,
    ///<summary>
    /// Enable box-selection with same width and same x pos items (e.g. full row Selectable()). Box-selection works better with little bit of spacing between items hit-box in order to be able to aim at empty space.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_BoxSelect1d = 1 << 6,
    ///<summary>
    /// Enable box-selection with varying width or varying x pos items support (e.g. different width labels, or 2D layout/grid). This is slower: alters clipping logic so that e.g. horizontal movements will update selection of normally clipped items.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_BoxSelect2d = 1 << 7,
    ///<summary>
    /// Disable scrolling when box-selecting near edges of scope.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_BoxSelectNoScroll = 1 << 8,
    ///<summary>
    /// Clear selection when pressing Escape while scope is focused.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_ClearOnEscape = 1 << 9,
    ///<summary>
    /// Clear selection when clicking on empty location within scope.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_ClearOnClickVoid = 1 << 10,
    ///<summary>
    /// Scope for _BoxSelect and _ClearOnClickVoid is whole window (Default). Use if BeginMultiSelect() covers a whole window or used a single time in same window.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_ScopeWindow = 1 << 11,
    ///<summary>
    /// Scope for _BoxSelect and _ClearOnClickVoid is rectangle encompassing BeginMultiSelect()/EndMultiSelect(). Use if BeginMultiSelect() is called multiple times in same window.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_ScopeRect = 1 << 12,
    ///<summary>
    /// Apply selection on mouse down when clicking on unselected item. (Default)<br/>
    ///</summary>
    ImGuiMultiSelectFlags_SelectOnClick = 1 << 13,
    ///<summary>
    /// Apply selection on mouse release when clicking an unselected item. Allow dragging an unselected item without altering selection.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_SelectOnClickRelease = 1 << 14,
    ///<summary>
    /// [Temporary] Enable navigation wrapping on X axis. Provided as a convenience because we don't have a design for the general Nav API for this yet. When the more general feature be public we may obsolete this flag in favor of new one.<br/>
    ///<br/>
    ///ImGuiMultiSelectFlags_RangeSelect2d       = 1 &lt;&lt; 15,   Shift+Selection uses 2d geometry instead of linear sequence, so possible to use Shift+up/down to select vertically in grid. Analogous to what BoxSelect does.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_NavWrapX = 1 << 16,
    ///<summary>
    /// Disable default right-click processing, which selects item on mouse down, and is designed for context-menus.<br/>
    ///</summary>
    ImGuiMultiSelectFlags_NoSelectOnRightClick = 1 << 17
}

///<summary>
/// Main IO structure returned by BeginMultiSelect()/EndMultiSelect().<br/>
/// This mainly contains a list of selection requests.<br/>
/// - Use 'Demo-&gt;Tools-&gt;Debug Log-&gt;Selection' to see requests as they happen.<br/>
/// - Some fields are only useful if your list is dynamic and allows deletion (getting post-deletion focus/state right is shown in the demo)<br/>
/// - Below: who reads/writes each fields? 'r'=read, 'w'=write, 'ms'=multi-select code, 'app'=application/user code.<br/>
///</summary>
public unsafe interface IImGuiMultiSelectIO
{
    public nint NativePointer { get; }

    ///<summary>
    ///  ms:w, app:r     /  ms:w  app:r    Requests to apply to your selection data.<br/>
    ///<br/>
    ///------------------------------------------ BeginMultiSelect / EndMultiSelect<br/>
    ///</summary>
    ImStructVectorWrapper<IImGuiSelectionRequest> Requests { get; }

    ///<summary>
    ///  ms:w  app:r     /                 (If using clipper) Begin: Source item (often the first selected item) must never be clipped: use clipper.IncludeItemByIndex() to ensure it is submitted.<br/>
    ///</summary>
    ref long RangeSrcItem { get; }

    ///<summary>
    ///  ms:w, app:r     /                 (If using deletion) Last known SetNextItemSelectionUserData() value for NavId (if part of submitted items).<br/>
    ///</summary>
    ref long NavIdItem { get; }

    ///<summary>
    ///  ms:w, app:r     /        app:r    (If using deletion) Last known selection state for NavId (if part of submitted items).<br/>
    ///</summary>
    ref bool NavIdSelected { get; }

    ///<summary>
    ///        app:w     /  ms:r           (If using deletion) Set before EndMultiSelect() to reset ResetSrcItem (e.g. if deleted selection).<br/>
    ///</summary>
    ref bool RangeSrcReset { get; }

    ///<summary>
    ///  ms:w, app:r     /        app:r    'int items_count' parameter to BeginMultiSelect() is copied here for convenience, allowing simpler calls to your ApplyRequests handler. Not used internally.<br/>
    ///</summary>
    ref int ItemsCount { get; }
}

///<summary>
/// Selection request type<br/>
///</summary>
public enum ImGuiSelectionRequestType
{
    ImGuiSelectionRequestType_None = 0,
    ///<summary>
    /// Request app to clear selection (if Selected==false) or select all items (if Selected==true). We cannot set RangeFirstItem/RangeLastItem as its contents is entirely up to user (not necessarily an index)<br/>
    ///</summary>
    ImGuiSelectionRequestType_SetAll,
    ///<summary>
    /// Request app to select/unselect [RangeFirstItem..RangeLastItem] items (inclusive) based on value of Selected. Only EndMultiSelect() request this, app code can read after BeginMultiSelect() and it will always be false.<br/>
    ///</summary>
    ImGuiSelectionRequestType_SetRange
}

///<summary>
/// Selection request item<br/>
///</summary>
public unsafe interface IImGuiSelectionRequest
{
    public nint NativePointer { get; }

    ///<summary>
    ///  ms:w, app:r     /  ms:w, app:r    Request type. You'll most often receive 1 Clear + 1 SetRange with a single-item range.<br/>
    ///<br/>
    ///------------------------------------------ BeginMultiSelect / EndMultiSelect<br/>
    ///</summary>
    ref ImGuiSelectionRequestType Type { get; }

    ///<summary>
    ///  ms:w, app:r     /  ms:w, app:r    Parameter for SetAll/SetRange requests (true = select, false = unselect)<br/>
    ///</summary>
    ref bool Selected { get; }

    ///<summary>
    ///                  /  ms:w  app:r    Parameter for SetRange request: +1 when RangeFirstItem comes before RangeLastItem, -1 otherwise. Useful if you want to preserve selection order on a backward Shift+Click.<br/>
    ///</summary>
    ref sbyte RangeDirection { get; }

    ///<summary>
    ///                  /  ms:w, app:r    Parameter for SetRange request (this is generally == RangeSrcItem when shift selecting from top to bottom).<br/>
    ///</summary>
    ref long RangeFirstItem { get; }

    ///<summary>
    ///                  /  ms:w, app:r    Parameter for SetRange request (this is generally == RangeSrcItem when shift selecting from bottom to top). Inclusive!<br/>
    ///</summary>
    ref long RangeLastItem { get; }
}

///<summary>
/// Optional helper to store multi-selection state + apply multi-selection requests.<br/>
/// - Used by our demos and provided as a convenience to easily implement basic multi-selection.<br/>
/// - Iterate selection with 'void* it = NULL; ImGuiID id; while (selection.GetNextSelectedItem(&amp;it, &amp;id)) { ... }'<br/>
///   Or you can check 'if (Contains(id)) { ... }' for each possible object if their number is not too high to iterate.<br/>
/// - USING THIS IS NOT MANDATORY. This is only a helper and not a required API.<br/>
/// To store a multi-selection, in your application you could:<br/>
/// - Use this helper as a convenience. We use our simple key-&gt;value ImGuiStorage as a std::set&lt;ImGuiID&gt; replacement.<br/>
/// - Use your own external storage: e.g. std::set&lt;MyObjectId&gt;, std::vector&lt;MyObjectId&gt;, interval trees, intrusively stored selection etc.<br/>
/// In ImGuiSelectionBasicStorage we:<br/>
/// - always use indices in the multi-selection API (passed to SetNextItemSelectionUserData(), retrieved in ImGuiMultiSelectIO)<br/>
/// - use the AdapterIndexToStorageId() indirection layer to abstract how persistent selection data is derived from an index.<br/>
/// - use decently optimized logic to allow queries and insertion of very large selection sets.<br/>
/// - do not preserve selection order.<br/>
/// Many combinations are possible depending on how you prefer to store your items and how you prefer to store your selection.<br/>
/// Large applications are likely to eventually want to get rid of this indirection layer and do their own thing.<br/>
/// See https:github.com/ocornut/imgui/wiki/Multi-Select for details and pseudo-code using this helper.<br/>
///</summary>
public unsafe interface IImGuiSelectionBasicStorage
{
    public nint NativePointer { get; }

    ///<summary>
    ///           Number of selected items, maintained by this helper.<br/>
    ///<br/>
    /// Members<br/>
    ///</summary>
    ref int Size { get; }

    ///<summary>
    /// = false   GetNextSelectedItem() will return ordered selection (currently implemented by two additional sorts of selection. Could be improved)<br/>
    ///</summary>
    ref bool PreserveOrder { get; }

    ///<summary>
    /// = NULL    User data for use by adapter function         e.g. selection.UserData = (void*)my_items;<br/>
    ///</summary>
    void* UserData { get; set; }

    ///<summary>
    /// e.g. selection.AdapterIndexToStorageId = [](ImGuiSelectionBasicStorage* self, int idx) { return ((MyItems**)self-&gt;UserData)[idx]-&gt;ID; };<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, int, uint> AdapterIndexToStorageId { get; set; }

    ///<summary>
    /// [Internal] Increasing counter to store selection order<br/>
    ///</summary>
    ref int _SelectionOrder { get; }

    ///<summary>
    /// [Internal] Selection set. Think of this as similar to e.g. std::set&lt;ImGuiID&gt;. Prefer not accessing directly: iterate with GetNextSelectedItem().<br/>
    ///</summary>
    IImGuiStorage _Storage { get; }
}

///<summary>
/// Optional helper to apply multi-selection requests to existing randomly accessible storage.<br/>
/// Convenient if you want to quickly wire multi-select API on e.g. an array of bool or items storing their own selection state.<br/>
///</summary>
public unsafe interface IImGuiSelectionExternalStorage
{
    public nint NativePointer { get; }

    ///<summary>
    /// User data for use by adapter function                                 e.g. selection.UserData = (void*)my_items;<br/>
    ///<br/>
    /// Members<br/>
    ///</summary>
    void* UserData { get; set; }

    ///<summary>
    /// e.g. AdapterSetItemSelected = [](ImGuiSelectionExternalStorage* self, int idx, bool selected) { ((MyItems**)self-&gt;UserData)[idx]-&gt;Selected = selected; }<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, int, byte, void> AdapterSetItemSelected { get; set; }
}

///<summary>
/// Typically, 1 command = 1 GPU draw call (unless command is a callback)<br/>
/// - VtxOffset: When 'io.BackendFlags &amp; ImGuiBackendFlags_RendererHasVtxOffset' is enabled,<br/>
///   this fields allow us to render meshes larger than 64K vertices while keeping 16-bit indices.<br/>
///   Backends made for &lt;1.71. will typically ignore the VtxOffset fields.<br/>
/// - The ClipRect/TexRef/VtxOffset fields must be contiguous as we memcmp() them together (this is asserted for).<br/>
///</summary>
public unsafe interface IImDrawCmd
{
    public nint NativePointer { get; }

    ///<summary>
    /// 4*4   Clipping rectangle (x1, y1, x2, y2). Subtract ImDrawData-&gt;DisplayPos to get clipping rectangle in "viewport" coordinates<br/>
    ///</summary>
    ref Vector4 ClipRect { get; }

    ///<summary>
    /// 16    Reference to a font/texture atlas (where backend called ImTextureData::SetTexID()) or to a user-provided texture ID (via e.g. ImGui::Image() calls). Both will lead to a ImTextureID value.<br/>
    ///</summary>
    ref ImTextureRef TexRef { get; }

    ///<summary>
    /// 4     Start offset in vertex buffer. ImGuiBackendFlags_RendererHasVtxOffset: always 0, otherwise may be &gt;0 to support meshes larger than 64K vertices with 16-bit indices.<br/>
    ///</summary>
    ref uint VtxOffset { get; }

    ///<summary>
    /// 4     Start offset in index buffer.<br/>
    ///</summary>
    ref uint IdxOffset { get; }

    ///<summary>
    /// 4     Number of indices (multiple of 3) to be rendered as triangles. Vertices are stored in the callee ImDrawList's vtx_buffer[] array, indices in idx_buffer[].<br/>
    ///</summary>
    ref uint ElemCount { get; }

    ///<summary>
    /// 4-8   If != NULL, call the function instead of rendering the vertices. clip_rect and texture_id will be set normally.<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint, void> UserCallback { get; set; }

    ///<summary>
    /// 4-8   Callback user data (when UserCallback != NULL). If called AddCallback() with size == 0, this is a copy of the AddCallback() argument. If called AddCallback() with size &gt; 0, this is pointing to a buffer where data is stored.<br/>
    ///</summary>
    void* UserCallbackData { get; set; }

    ///<summary>
    /// 4  Size of callback user data when using storage, otherwise 0.<br/>
    ///</summary>
    ref int UserCallbackDataSize { get; }

    ///<summary>
    /// 4  [Internal] Offset of callback user data when using storage, otherwise -1.<br/>
    ///</summary>
    ref int UserCallbackDataOffset { get; }
}

public unsafe interface IImDrawVert
{
    public nint NativePointer { get; }

    ref Vector2 pos { get; }

    ref Vector2 uv { get; }

    ref uint col { get; }
}

///<summary>
/// [Internal] For use by ImDrawList<br/>
///</summary>
public unsafe interface IImDrawCmdHeader
{
    public nint NativePointer { get; }

    ref Vector4 ClipRect { get; }

    ref ImTextureRef TexRef { get; }

    ref uint VtxOffset { get; }
}

///<summary>
/// [Internal] For use by ImDrawListSplitter<br/>
///</summary>
public unsafe interface IImDrawChannel
{
    public nint NativePointer { get; }

    ImStructVectorWrapper<IImDrawCmd> _CmdBuffer { get; }

    ImVector<ushort> _IdxBuffer { get; }
}

///<summary>
/// Split/Merge functions are used to split the draw list into different layers which can be drawn into out of order.<br/>
/// This is used by the Columns/Tables API, so items of each column can be batched together in a same draw call.<br/>
///</summary>
public unsafe interface IImDrawListSplitter
{
    public nint NativePointer { get; }

    ///<summary>
    /// Current channel number (0)<br/>
    ///</summary>
    ref int _Current { get; }

    ///<summary>
    /// Number of active channels (1+)<br/>
    ///</summary>
    ref int _Count { get; }

    ///<summary>
    /// Draw channels (not resized down so _Count might be &lt; Channels.Size)<br/>
    ///</summary>
    ImStructVectorWrapper<IImDrawChannel> _Channels { get; }
}

///<summary>
/// Flags for ImDrawList functions<br/>
/// (Legacy: bit 0 must always correspond to ImDrawFlags_Closed to be backward compatible with old API using a bool. Bits 1..3 must be unused)<br/>
///</summary>
public enum ImDrawFlags
{
    ImDrawFlags_None = 0,
    ///<summary>
    /// PathStroke(), AddPolyline(): specify that shape should be closed (Important: this is always == 1 for legacy reason)<br/>
    ///</summary>
    ImDrawFlags_Closed = 1 << 0,
    ///<summary>
    /// AddRect(), AddRectFilled(), PathRect(): enable rounding top-left corner only (when rounding &gt; 0.0f, we default to all corners). Was 0x01.<br/>
    ///</summary>
    ImDrawFlags_RoundCornersTopLeft = 1 << 4,
    ///<summary>
    /// AddRect(), AddRectFilled(), PathRect(): enable rounding top-right corner only (when rounding &gt; 0.0f, we default to all corners). Was 0x02.<br/>
    ///</summary>
    ImDrawFlags_RoundCornersTopRight = 1 << 5,
    ///<summary>
    /// AddRect(), AddRectFilled(), PathRect(): enable rounding bottom-left corner only (when rounding &gt; 0.0f, we default to all corners). Was 0x04.<br/>
    ///</summary>
    ImDrawFlags_RoundCornersBottomLeft = 1 << 6,
    ///<summary>
    /// AddRect(), AddRectFilled(), PathRect(): enable rounding bottom-right corner only (when rounding &gt; 0.0f, we default to all corners). Wax 0x08.<br/>
    ///</summary>
    ImDrawFlags_RoundCornersBottomRight = 1 << 7,
    ///<summary>
    /// AddRect(), AddRectFilled(), PathRect(): disable rounding on all corners (when rounding &gt; 0.0f). This is NOT zero, NOT an implicit flag!<br/>
    ///</summary>
    ImDrawFlags_RoundCornersNone = 1 << 8,
    ImDrawFlags_RoundCornersTop = ImDrawFlags_RoundCornersTopLeft | ImDrawFlags_RoundCornersTopRight,
    ImDrawFlags_RoundCornersBottom = ImDrawFlags_RoundCornersBottomLeft | ImDrawFlags_RoundCornersBottomRight,
    ImDrawFlags_RoundCornersLeft = ImDrawFlags_RoundCornersBottomLeft | ImDrawFlags_RoundCornersTopLeft,
    ImDrawFlags_RoundCornersRight = ImDrawFlags_RoundCornersBottomRight | ImDrawFlags_RoundCornersTopRight,
    ImDrawFlags_RoundCornersAll = ImDrawFlags_RoundCornersTopLeft | ImDrawFlags_RoundCornersTopRight | ImDrawFlags_RoundCornersBottomLeft | ImDrawFlags_RoundCornersBottomRight,
    ///<summary>
    /// Default to ALL corners if none of the _RoundCornersXX flags are specified.<br/>
    ///</summary>
    ImDrawFlags_RoundCornersDefault_ = ImDrawFlags_RoundCornersAll,
    ImDrawFlags_RoundCornersMask_ = ImDrawFlags_RoundCornersAll | ImDrawFlags_RoundCornersNone
}

///<summary>
/// Flags for ImDrawList instance. Those are set automatically by ImGui:: functions from ImGuiIO settings, and generally not manipulated directly.<br/>
/// It is however possible to temporarily alter flags between calls to ImDrawList:: functions.<br/>
///</summary>
public enum ImDrawListFlags
{
    ImDrawListFlags_None = 0,
    ///<summary>
    /// Enable anti-aliased lines/borders (*2 the number of triangles for 1.0f wide line or lines thin enough to be drawn using textures, otherwise *3 the number of triangles)<br/>
    ///</summary>
    ImDrawListFlags_AntiAliasedLines = 1 << 0,
    ///<summary>
    /// Enable anti-aliased lines/borders using textures when possible. Require backend to render with bilinear filtering (NOT point/nearest filtering).<br/>
    ///</summary>
    ImDrawListFlags_AntiAliasedLinesUseTex = 1 << 1,
    ///<summary>
    /// Enable anti-aliased edge around filled shapes (rounded rectangles, circles).<br/>
    ///</summary>
    ImDrawListFlags_AntiAliasedFill = 1 << 2,
    ///<summary>
    /// Can emit 'VtxOffset &gt; 0' to allow large meshes. Set when 'ImGuiBackendFlags_RendererHasVtxOffset' is enabled.<br/>
    ///</summary>
    ImDrawListFlags_AllowVtxOffset = 1 << 3
}

///<summary>
/// Draw command list<br/>
/// This is the low-level list of polygons that ImGui:: functions are filling. At the end of the frame,<br/>
/// all command lists are passed to your ImGuiIO::RenderDrawListFn function for rendering.<br/>
/// Each dear imgui window contains its own ImDrawList. You can use ImGui::GetWindowDrawList() to<br/>
/// access the current window draw list and draw custom primitives.<br/>
/// You can interleave normal ImGui:: calls and adding primitives to the current draw list.<br/>
/// In single viewport mode, top-left is == GetMainViewport()-&gt;Pos (generally 0,0), bottom-right is == GetMainViewport()-&gt;Pos+Size (generally io.DisplaySize).<br/>
/// You are totally free to apply whatever transformation matrix you want to the data (depending on the use of the transformation you may want to apply it to ClipRect as well!)<br/>
/// Important: Primitives are always added to the list and not culled (culling is done at higher-level by ImGui:: functions), if you use this API a lot consider coarse culling your drawn objects.<br/>
///</summary>
public unsafe interface IImDrawList
{
    public nint NativePointer { get; }

    ///<summary>
    /// Draw commands. Typically 1 command = 1 GPU draw call, unless the command is a callback.<br/>
    ///<br/>
    /// This is what you have to render<br/>
    ///</summary>
    ImStructVectorWrapper<IImDrawCmd> CmdBuffer { get; }

    ///<summary>
    /// Index buffer. Each command consume ImDrawCmd::ElemCount of those<br/>
    ///</summary>
    ImVector<ushort> IdxBuffer { get; }

    ///<summary>
    /// Vertex buffer.<br/>
    ///</summary>
    ImStructVectorWrapper<IImDrawVert> VtxBuffer { get; }

    ///<summary>
    /// Flags, you may poke into these to adjust anti-aliasing settings per-primitive.<br/>
    ///</summary>
    ref ImDrawListFlags Flags { get; }

    ///<summary>
    /// [Internal] generally == VtxBuffer.Size unless we are past 64K vertices, in which case this gets reset to 0.<br/>
    ///<br/>
    /// [Internal, used while building lists]<br/>
    ///</summary>
    ref uint _VtxCurrentIdx { get; }

    ///<summary>
    /// Pointer to shared draw data (you can use ImGui::GetDrawListSharedData() to get the one from current ImGui context)<br/>
    ///</summary>
    nint _Data { get; }

    ///<summary>
    /// [Internal] point within VtxBuffer.Data after each add command (to avoid using the ImVector&lt;&gt; operators too much)<br/>
    ///</summary>
    IImDrawVert _VtxWritePtr { get; }

    ///<summary>
    /// [Internal] point within IdxBuffer.Data after each add command (to avoid using the ImVector&lt;&gt; operators too much)<br/>
    ///</summary>
    ushort* _IdxWritePtr { get; set; }

    ///<summary>
    /// [Internal] current path building<br/>
    ///</summary>
    ImVector<Vector2> _Path { get; }

    ///<summary>
    /// [Internal] template of active commands. Fields should match those of CmdBuffer.back().<br/>
    ///</summary>
    IImDrawCmdHeader _CmdHeader { get; }

    ///<summary>
    /// [Internal] for channels api (note: prefer using your own persistent instance of ImDrawListSplitter!)<br/>
    ///</summary>
    IImDrawListSplitter _Splitter { get; }

    ///<summary>
    /// [Internal]<br/>
    ///</summary>
    ImVector<Vector4> _ClipRectStack { get; }

    ///<summary>
    /// [Internal]<br/>
    ///</summary>
    ImVector<ImTextureRef> _TextureStack { get; }

    ///<summary>
    /// [Internal]<br/>
    ///</summary>
    ImVector<byte> _CallbacksDataBuf { get; }

    ///<summary>
    /// [Internal] anti-alias fringe is scaled by this value, this helps to keep things sharp while zooming at vertex buffer content<br/>
    ///</summary>
    ref float _FringeScale { get; }

    ///<summary>
    /// Pointer to owner window's name for debugging<br/>
    ///</summary>
    sbyte* _OwnerName { get; set; }
}

///<summary>
/// All draw data to render a Dear ImGui frame<br/>
/// (NB: the style and the naming convention here is a little inconsistent, we currently preserve them for backward compatibility purpose,<br/>
/// as this is one of the oldest structure exposed by the library! Basically, ImDrawList == CmdList)<br/>
///</summary>
public unsafe interface IImDrawData
{
    public nint NativePointer { get; }

    ///<summary>
    /// Only valid after Render() is called and before the next NewFrame() is called.<br/>
    ///</summary>
    ref bool Valid { get; }

    ///<summary>
    /// == CmdLists.Size. (OBSOLETE: exists for legacy reasons). Number of ImDrawList* to render.<br/>
    ///</summary>
    ref int CmdListsCount { get; }

    ///<summary>
    /// For convenience, sum of all ImDrawList's IdxBuffer.Size<br/>
    ///</summary>
    ref int TotalIdxCount { get; }

    ///<summary>
    /// For convenience, sum of all ImDrawList's VtxBuffer.Size<br/>
    ///</summary>
    ref int TotalVtxCount { get; }

    ///<summary>
    /// Array of ImDrawList* to render. The ImDrawLists are owned by ImGuiContext and only pointed to from here.<br/>
    ///</summary>
    ImStructPtrVectorWrapper<IImDrawList> CmdLists { get; }

    ///<summary>
    /// Top-left position of the viewport to render (== top-left of the orthogonal projection matrix to use) (== GetMainViewport()-&gt;Pos for the main viewport, == (0.0) in most single-viewport applications)<br/>
    ///</summary>
    ref Vector2 DisplayPos { get; }

    ///<summary>
    /// Size of the viewport to render (== GetMainViewport()-&gt;Size for the main viewport, == io.DisplaySize in most single-viewport applications)<br/>
    ///</summary>
    ref Vector2 DisplaySize { get; }

    ///<summary>
    /// Amount of pixels for each unit of DisplaySize. Copied from viewport-&gt;FramebufferScale (== io.DisplayFramebufferScale for main viewport). Generally (1,1) on normal display, (2,2) on OSX with Retina display.<br/>
    ///</summary>
    ref Vector2 FramebufferScale { get; }

    ///<summary>
    /// Viewport carrying the ImDrawData instance, might be of use to the renderer (generally not).<br/>
    ///</summary>
    IImGuiViewport OwnerViewport { get; }

    ///<summary>
    /// List of textures to update. Most of the times the list is shared by all ImDrawData, has only 1 texture and it doesn't need any update. This almost always points to ImGui::GetPlatformIO().Textures[]. May be overridden or set to NULL if you want to manually update textures.<br/>
    ///</summary>
    ImStructPtrVectorPtrWrapper<IImTextureData> Textures { get; }
}

///<summary>
/// Most standard backends only support RGBA32 but we provide a single channel option for low-resource/embedded systems.<br/>
///</summary>
public enum ImTextureFormat
{
    ///<summary>
    /// 4 components per pixel, each is unsigned 8-bit. Total size = TexWidth * TexHeight * 4<br/>
    ///</summary>
    ImTextureFormat_RGBA32,
    ///<summary>
    /// 1 component per pixel, each is unsigned 8-bit. Total size = TexWidth * TexHeight<br/>
    ///</summary>
    ImTextureFormat_Alpha8
}

///<summary>
/// Status of a texture to communicate with Renderer Backend.<br/>
///</summary>
public enum ImTextureStatus
{
    ImTextureStatus_OK,
    ///<summary>
    /// Backend destroyed the texture.<br/>
    ///</summary>
    ImTextureStatus_Destroyed,
    ///<summary>
    /// Requesting backend to create the texture. Set status OK when done.<br/>
    ///</summary>
    ImTextureStatus_WantCreate,
    ///<summary>
    /// Requesting backend to update specific blocks of pixels (write to texture portions which have never been used before). Set status OK when done.<br/>
    ///</summary>
    ImTextureStatus_WantUpdates,
    ///<summary>
    /// Requesting backend to destroy the texture. Set status to Destroyed when done.<br/>
    ///</summary>
    ImTextureStatus_WantDestroy
}

///<summary>
/// Coordinates of a rectangle within a texture.<br/>
/// When a texture is in ImTextureStatus_WantUpdates state, we provide a list of individual rectangles to copy to the graphics system.<br/>
/// You may use ImTextureData::Updates[] for the list, or ImTextureData::UpdateBox for a single bounding box.<br/>
///</summary>
public unsafe interface IImTextureRect
{
    public nint NativePointer { get; }

    ///<summary>
    /// Upper-left coordinates of rectangle to update<br/>
    ///</summary>
    ref ushort x { get; }

    ///<summary>
    /// Upper-left coordinates of rectangle to update<br/>
    ///</summary>
    ref ushort y { get; }

    ///<summary>
    /// Size of rectangle to update (in pixels)<br/>
    ///</summary>
    ref ushort w { get; }

    ///<summary>
    /// Size of rectangle to update (in pixels)<br/>
    ///</summary>
    ref ushort h { get; }
}

///<summary>
/// Specs and pixel storage for a texture used by Dear ImGui.<br/>
/// This is only useful for (1) core library and (2) backends. End-user/applications do not need to care about this.<br/>
/// Renderer Backends will create a GPU-side version of this.<br/>
/// Why does we store two identifiers: TexID and BackendUserData?<br/>
/// - ImTextureID    TexID           = lower-level identifier stored in ImDrawCmd. ImDrawCmd can refer to textures not created by the backend, and for which there's no ImTextureData.<br/>
/// - void*          BackendUserData = higher-level opaque storage for backend own book-keeping. Some backends may have enough with TexID and not need both.<br/>
/// In columns below: who reads/writes each fields? 'r'=read, 'w'=write, 'core'=main library, 'backend'=renderer backend<br/>
///</summary>
public unsafe interface IImTextureData
{
    public nint NativePointer { get; }

    ///<summary>
    /// w    -    [DEBUG] Sequential index to facilitate identifying a texture when debugging/printing. Unique per atlas.<br/>
    ///<br/>
    ///------------------------------------------ core / backend ---------------------------------------<br/>
    ///</summary>
    ref int UniqueID { get; }

    ///<summary>
    /// rw   rw   ImTextureStatus_OK/_WantCreate/_WantUpdates/_WantDestroy. Always use SetStatus() to modify!<br/>
    ///</summary>
    ref ImTextureStatus Status { get; }

    ///<summary>
    /// -    rw   Convenience storage for backend. Some backends may have enough with TexID.<br/>
    ///</summary>
    void* BackendUserData { get; set; }

    ///<summary>
    /// r    w    Backend-specific texture identifier. Always use SetTexID() to modify! The identifier will stored in ImDrawCmd::GetTexID() and passed to backend's RenderDrawData function.<br/>
    ///</summary>
    ref ulong TexID { get; }

    ///<summary>
    /// w    r    ImTextureFormat_RGBA32 (default) or ImTextureFormat_Alpha8<br/>
    ///</summary>
    ref ImTextureFormat Format { get; }

    ///<summary>
    /// w    r    Texture width<br/>
    ///</summary>
    ref int Width { get; }

    ///<summary>
    /// w    r    Texture height<br/>
    ///</summary>
    ref int Height { get; }

    ///<summary>
    /// w    r    4 or 1<br/>
    ///</summary>
    ref int BytesPerPixel { get; }

    ///<summary>
    /// w    r    Pointer to buffer holding 'Width*Height' pixels and 'Width*Height*BytesPerPixels' bytes.<br/>
    ///</summary>
    byte* Pixels { get; set; }

    ///<summary>
    /// w    r    Bounding box encompassing all past and queued Updates[].<br/>
    ///</summary>
    IImTextureRect UsedRect { get; }

    ///<summary>
    /// w    r    Bounding box encompassing all queued Updates[].<br/>
    ///</summary>
    IImTextureRect UpdateRect { get; }

    ///<summary>
    /// w    r    Array of individual updates.<br/>
    ///</summary>
    ImStructVectorWrapper<IImTextureRect> Updates { get; }

    ///<summary>
    /// w    r    In order to facilitate handling Status==WantDestroy in some backend: this is a count successive frames where the texture was not used. Always &gt;0 when Status==WantDestroy.<br/>
    ///</summary>
    ref int UnusedFrames { get; }

    ///<summary>
    /// w    r    Number of contexts using this texture. Used during backend shutdown.<br/>
    ///</summary>
    ref ushort RefCount { get; }

    ///<summary>
    /// w    r    Tell whether our texture data is known to use colors (rather than just white + alpha).<br/>
    ///</summary>
    ref bool UseColors { get; }

    ///<summary>
    /// rw   -    [Internal] Queued to set ImTextureStatus_WantDestroy next frame. May still be used in the current frame.<br/>
    ///</summary>
    ref bool WantDestroyNextFrame { get; }
}

///<summary>
/// A font input/source (we may rename this to ImFontSource in the future)<br/>
///</summary>
public unsafe interface IImFontConfig
{
    public nint NativePointer { get; }

    ///<summary>
    /// &lt;auto&gt;    Name (strictly to ease debugging, hence limited size buffer)<br/>
    ///<br/>
    /// Data Source<br/>
    ///</summary>
    RangeAccessor<byte> Name { get; }

    ///<summary>
    ///           TTF/OTF data<br/>
    ///</summary>
    void* FontData { get; set; }

    ///<summary>
    ///           TTF/OTF data size<br/>
    ///</summary>
    ref int FontDataSize { get; }

    ///<summary>
    /// true      TTF/OTF data ownership taken by the owner ImFontAtlas (will delete memory itself).<br/>
    ///</summary>
    ref bool FontDataOwnedByAtlas { get; }

    ///<summary>
    /// false     Merge into previous ImFont, so you can combine multiple inputs font into one ImFont (e.g. ASCII font + icons + Japanese glyphs). You may want to use GlyphOffset.y when merge font of different heights.<br/>
    ///<br/>
    /// Options<br/>
    ///</summary>
    ref bool MergeMode { get; }

    ///<summary>
    /// false     Align every glyph AdvanceX to pixel boundaries. Useful e.g. if you are merging a non-pixel aligned font with the default font. If enabled, you can set OversampleH/V to 1.<br/>
    ///</summary>
    ref bool PixelSnapH { get; }

    ///<summary>
    /// true      Align Scaled GlyphOffset.y to pixel boundaries.<br/>
    ///</summary>
    ref bool PixelSnapV { get; }

    ///<summary>
    /// 0 (2)     Rasterize at higher quality for sub-pixel positioning. 0 == auto == 1 or 2 depending on size. Note the difference between 2 and 3 is minimal. You can reduce this to 1 for large glyphs save memory. Read https:github.com/nothings/stb/blob/master/tests/oversample/README.md for details.<br/>
    ///</summary>
    ref sbyte OversampleH { get; }

    ///<summary>
    /// 0 (1)     Rasterize at higher quality for sub-pixel positioning. 0 == auto == 1. This is not really useful as we don't use sub-pixel positions on the Y axis.<br/>
    ///</summary>
    ref sbyte OversampleV { get; }

    ///<summary>
    /// 0         Explicitly specify Unicode codepoint of ellipsis character. When fonts are being merged first specified ellipsis will be used.<br/>
    ///</summary>
    ref uint EllipsisChar { get; }

    ///<summary>
    ///           Size in pixels for rasterizer (more or less maps to the resulting font height).<br/>
    ///</summary>
    ref float SizePixels { get; }

    ///<summary>
    /// NULL      *LEGACY* THE ARRAY DATA NEEDS TO PERSIST AS LONG AS THE FONT IS ALIVE. Pointer to a user-provided list of Unicode range (2 value per range, values are inclusive, zero-terminated list).<br/>
    ///</summary>
    uint* GlyphRanges { get; set; }

    ///<summary>
    /// NULL      Pointer to a small user-provided list of Unicode ranges (2 value per range, values are inclusive, zero-terminated list). This is very close to GlyphRanges[] but designed to exclude ranges from a font source, when merging fonts with overlapping glyphs. Use "Input Glyphs Overlap Detection Tool" to find about your overlapping ranges.<br/>
    ///</summary>
    uint* GlyphExcludeRanges { get; set; }

    ///<summary>
    /// 0, 0      Offset (in pixels) all glyphs from this font input. Absolute value for default size, other sizes will scale this value.<br/>
    ///<br/>
    ///ImVec2        GlyphExtraSpacing;       0, 0      (REMOVED AT IT SEEMS LARGELY OBSOLETE. PLEASE REPORT IF YOU WERE USING THIS). Extra spacing (in pixels) between glyphs when rendered: essentially add to glyph-&gt;AdvanceX. Only X axis is supported for now.<br/>
    ///</summary>
    ref Vector2 GlyphOffset { get; }

    ///<summary>
    /// 0         Minimum AdvanceX for glyphs, set Min to align font icons, set both Min/Max to enforce mono-space font. Absolute value for default size, other sizes will scale this value.<br/>
    ///</summary>
    ref float GlyphMinAdvanceX { get; }

    ///<summary>
    /// FLT_MAX   Maximum AdvanceX for glyphs<br/>
    ///</summary>
    ref float GlyphMaxAdvanceX { get; }

    ///<summary>
    /// 0         Extra spacing (in pixels) between glyphs. Please contact us if you are using this.  FIXME-NEWATLAS: Intentionally unscaled<br/>
    ///</summary>
    ref float GlyphExtraAdvanceX { get; }

    ///<summary>
    /// 0         Index of font within TTF/OTF file<br/>
    ///</summary>
    ref uint FontNo { get; }

    ///<summary>
    /// 0         Settings for custom font builder. THIS IS BUILDER IMPLEMENTATION DEPENDENT. Leave as zero if unsure.<br/>
    ///</summary>
    ref uint FontLoaderFlags { get; }

    ///<summary>
    /// 1.0f      Linearly brighten (&gt;1.0f) or darken (&lt;1.0f) font output. Brightening small fonts may be a good workaround to make them more readable. This is a silly thing we may remove in the future.<br/>
    ///<br/>
    ///unsigned int  FontBuilderFlags;        --        [Renamed in 1.92] Ue FontLoaderFlags.<br/>
    ///</summary>
    ref float RasterizerMultiply { get; }

    ///<summary>
    /// 1.0f      [LEGACY: this only makes sense when ImGuiBackendFlags_RendererHasTextures is not supported] DPI scale multiplier for rasterization. Not altering other font metrics: makes it easy to swap between e.g. a 100% and a 400% fonts for a zooming display, or handle Retina screen. IMPORTANT: If you change this it is expected that you increase/decrease font scale roughly to the inverse of this, otherwise quality may look lowered.<br/>
    ///</summary>
    ref float RasterizerDensity { get; }

    ///<summary>
    /// Font flags (don't use just yet, will be exposed in upcoming 1.92.X updates)<br/>
    ///<br/>
    /// [Internal]<br/>
    ///</summary>
    ref int Flags { get; }

    ///<summary>
    /// Target font (as we merging fonts, multiple ImFontConfig may target the same font)<br/>
    ///</summary>
    IImFont DstFont { get; }

    ///<summary>
    /// Custom font backend for this source (default source is the one stored in ImFontAtlas)<br/>
    ///</summary>
    IImFontLoader FontLoader { get; }

    ///<summary>
    /// Font loader opaque storage (per font config)<br/>
    ///</summary>
    void* FontLoaderData { get; set; }
}

///<summary>
/// Hold rendering data for one glyph.<br/>
/// (Note: some language parsers may fail to convert the bitfield members, in this case maybe drop store a single u32 or we can rework this)<br/>
///</summary>
public unsafe interface IImFontGlyph
{
    public nint NativePointer { get; }

    ref uint _bitfield { get; }

    ///<summary>
    /// Flag to indicate glyph is colored and should generally ignore tinting (make it usable with no shift on little-endian as this is used in loops)<br/>
    ///</summary>
    uint Colored { get; set; }

    ///<summary>
    /// Flag to indicate glyph has no visible pixels (e.g. space). Allow early out when rendering.<br/>
    ///</summary>
    uint Visible { get; set; }

    ///<summary>
    /// Index of source in parent font<br/>
    ///</summary>
    uint SourceIdx { get; set; }

    ///<summary>
    /// 0x0000..0x10FFFF<br/>
    ///</summary>
    uint Codepoint { get; set; }

    ///<summary>
    /// Horizontal distance to advance cursor/layout position.<br/>
    ///</summary>
    ref float AdvanceX { get; }

    ///<summary>
    /// Glyph corners. Offsets from current cursor/layout position.<br/>
    ///</summary>
    ref float X0 { get; }

    ///<summary>
    /// Glyph corners. Offsets from current cursor/layout position.<br/>
    ///</summary>
    ref float Y0 { get; }

    ///<summary>
    /// Glyph corners. Offsets from current cursor/layout position.<br/>
    ///</summary>
    ref float X1 { get; }

    ///<summary>
    /// Glyph corners. Offsets from current cursor/layout position.<br/>
    ///</summary>
    ref float Y1 { get; }

    ///<summary>
    /// Texture coordinates for the current value of ImFontAtlas-&gt;TexRef. Cached equivalent of calling GetCustomRect() with PackId.<br/>
    ///</summary>
    ref float U0 { get; }

    ///<summary>
    /// Texture coordinates for the current value of ImFontAtlas-&gt;TexRef. Cached equivalent of calling GetCustomRect() with PackId.<br/>
    ///</summary>
    ref float V0 { get; }

    ///<summary>
    /// Texture coordinates for the current value of ImFontAtlas-&gt;TexRef. Cached equivalent of calling GetCustomRect() with PackId.<br/>
    ///</summary>
    ref float U1 { get; }

    ///<summary>
    /// Texture coordinates for the current value of ImFontAtlas-&gt;TexRef. Cached equivalent of calling GetCustomRect() with PackId.<br/>
    ///</summary>
    ref float V1 { get; }

    ///<summary>
    /// [Internal] ImFontAtlasRectId value (FIXME: Cold data, could be moved elsewhere?)<br/>
    ///</summary>
    ref int PackId { get; }
}

///<summary>
/// Helper to build glyph ranges from text/string data. Feed your application strings/characters to it then call BuildRanges().<br/>
/// This is essentially a tightly packed of vector of 64k booleans = 8KB storage.<br/>
///</summary>
public unsafe interface IImFontGlyphRangesBuilder
{
    public nint NativePointer { get; }

    ///<summary>
    /// Store 1-bit per Unicode code point (0=unused, 1=used)<br/>
    ///</summary>
    ImVector<uint> UsedChars { get; }
}

///<summary>
/// Output of ImFontAtlas::GetCustomRect() when using custom rectangles.<br/>
/// Those values may not be cached/stored as they are only valid for the current value of atlas-&gt;TexRef<br/>
/// (this is in theory derived from ImTextureRect but we use separate structures for reasons)<br/>
///</summary>
public unsafe interface IImFontAtlasRect
{
    public nint NativePointer { get; }

    ///<summary>
    /// Position (in current texture)<br/>
    ///</summary>
    ref ushort x { get; }

    ///<summary>
    /// Position (in current texture)<br/>
    ///</summary>
    ref ushort y { get; }

    ///<summary>
    /// Size<br/>
    ///</summary>
    ref ushort w { get; }

    ///<summary>
    /// Size<br/>
    ///</summary>
    ref ushort h { get; }

    ///<summary>
    /// UV coordinates (in current texture)<br/>
    ///</summary>
    ref Vector2 uv0 { get; }

    ///<summary>
    /// UV coordinates (in current texture)<br/>
    ///</summary>
    ref Vector2 uv1 { get; }
}

///<summary>
/// Flags for ImFontAtlas build<br/>
///</summary>
public enum ImFontAtlasFlags
{
    ImFontAtlasFlags_None = 0,
    ///<summary>
    /// Don't round the height to next power of two<br/>
    ///</summary>
    ImFontAtlasFlags_NoPowerOfTwoHeight = 1 << 0,
    ///<summary>
    /// Don't build software mouse cursors into the atlas (save a little texture memory)<br/>
    ///</summary>
    ImFontAtlasFlags_NoMouseCursors = 1 << 1,
    ///<summary>
    /// Don't build thick line textures into the atlas (save a little texture memory, allow support for point/nearest filtering). The AntiAliasedLinesUseTex features uses them, otherwise they will be rendered using polygons (more expensive for CPU/GPU).<br/>
    ///</summary>
    ImFontAtlasFlags_NoBakedLines = 1 << 2
}

///<summary>
/// Load and rasterize multiple TTF/OTF fonts into a same texture. The font atlas will build a single texture holding:<br/>
///  - One or more fonts.<br/>
///  - Custom graphics data needed to render the shapes needed by Dear ImGui.<br/>
///  - Mouse cursor shapes for software cursor rendering (unless setting 'Flags |= ImFontAtlasFlags_NoMouseCursors' in the font atlas).<br/>
///  - If you don't call any AddFont*** functions, the default font embedded in the code will be loaded for you.<br/>
/// It is the rendering backend responsibility to upload texture into your graphics API:<br/>
///  - ImGui_ImplXXXX_RenderDrawData() functions generally iterate platform_io-&gt;Textures[] to create/update/destroy each ImTextureData instance.<br/>
///  - Backend then set ImTextureData's TexID and BackendUserData.<br/>
///  - Texture id are passed back to you during rendering to identify the texture. Read FAQ entry about ImTextureID/ImTextureRef for more details.<br/>
/// Legacy path:<br/>
///  - Call Build() + GetTexDataAsAlpha8() or GetTexDataAsRGBA32() to build and retrieve pixels data.<br/>
///  - Call SetTexID(my_tex_id); and pass the pointer/identifier to your texture in a format natural to your graphics API.<br/>
/// Common pitfalls:<br/>
/// - If you pass a 'glyph_ranges' array to AddFont*** functions, you need to make sure that your array persist up until the<br/>
///   atlas is build (when calling GetTexData*** or Build()). We only copy the pointer, not the data.<br/>
/// - Important: By default, AddFontFromMemoryTTF() takes ownership of the data. Even though we are not writing to it, we will free the pointer on destruction.<br/>
///   You can set font_cfg-&gt;FontDataOwnedByAtlas=false to keep ownership of your data and it won't be freed,<br/>
/// - Even though many functions are suffixed with "TTF", OTF data is supported just as well.<br/>
/// - This is an old API and it is currently awkward for those and various other reasons! We will address them in the future!<br/>
///</summary>
public unsafe interface IImFontAtlas
{
    public nint NativePointer { get; }

    ///<summary>
    /// Build flags (see ImFontAtlasFlags_)<br/>
    ///<br/>
    /// Input<br/>
    ///</summary>
    ref ImFontAtlasFlags Flags { get; }

    ///<summary>
    /// Desired texture format (default to ImTextureFormat_RGBA32 but may be changed to ImTextureFormat_Alpha8).<br/>
    ///</summary>
    ref ImTextureFormat TexDesiredFormat { get; }

    ///<summary>
    /// FIXME: Should be called "TexPackPadding". Padding between glyphs within texture in pixels. Defaults to 1. If your rendering method doesn't rely on bilinear filtering you may set this to 0 (will also need to set AntiAliasedLinesUseTex = false).<br/>
    ///</summary>
    ref int TexGlyphPadding { get; }

    ///<summary>
    /// Minimum desired texture width. Must be a power of two. Default to 512.<br/>
    ///</summary>
    ref int TexMinWidth { get; }

    ///<summary>
    /// Minimum desired texture height. Must be a power of two. Default to 128.<br/>
    ///</summary>
    ref int TexMinHeight { get; }

    ///<summary>
    /// Maximum desired texture width. Must be a power of two. Default to 8192.<br/>
    ///</summary>
    ref int TexMaxWidth { get; }

    ///<summary>
    /// Maximum desired texture height. Must be a power of two. Default to 8192.<br/>
    ///</summary>
    ref int TexMaxHeight { get; }

    ///<summary>
    /// Store your own atlas related user-data (if e.g. you have multiple font atlas).<br/>
    ///</summary>
    void* UserData { get; set; }

    ///<summary>
    /// Latest texture.<br/>
    ///</summary>
    IImTextureData TexData { get; }

    ///<summary>
    /// Texture list (most often TexList.Size == 1). TexData is always == TexList.back(). DO NOT USE DIRECTLY, USE GetDrawData().Textures[]/GetPlatformIO().Textures[] instead!<br/>
    ///<br/>
    /// [Internal]<br/>
    ///</summary>
    ImStructPtrVectorWrapper<IImTextureData> TexList { get; }

    ///<summary>
    /// Marked as locked during ImGui::NewFrame()..EndFrame() scope if TexUpdates are not supported. Any attempt to modify the atlas will assert.<br/>
    ///</summary>
    ref bool Locked { get; }

    ///<summary>
    /// Copy of (BackendFlags &amp; ImGuiBackendFlags_RendererHasTextures) from supporting context.<br/>
    ///</summary>
    ref bool RendererHasTextures { get; }

    ///<summary>
    /// Set when texture was built matching current font input. Mostly useful for legacy IsBuilt() call.<br/>
    ///</summary>
    ref bool TexIsBuilt { get; }

    ///<summary>
    /// Tell whether our texture data is known to use colors (rather than just alpha channel), in order to help backend select a format or conversion process.<br/>
    ///</summary>
    ref bool TexPixelsUseColors { get; }

    ///<summary>
    /// = (1.0f/TexData-&gt;TexWidth, 1.0f/TexData-&gt;TexHeight). May change as new texture gets created.<br/>
    ///</summary>
    ref Vector2 TexUvScale { get; }

    ///<summary>
    /// Texture coordinates to a white pixel. May change as new texture gets created.<br/>
    ///</summary>
    ref Vector2 TexUvWhitePixel { get; }

    ///<summary>
    /// Hold all the fonts returned by AddFont*. Fonts[0] is the default font upon calling ImGui::NewFrame(), use ImGui::PushFont()/PopFont() to change the current font.<br/>
    ///</summary>
    ImStructPtrVectorWrapper<IImFont> Fonts { get; }

    ///<summary>
    /// Source/configuration data<br/>
    ///</summary>
    ImStructVectorWrapper<IImFontConfig> Sources { get; }

    ///<summary>
    /// UVs for baked anti-aliased lines<br/>
    ///</summary>
    RangeAccessor<Vector4> TexUvLines { get; }

    ///<summary>
    /// Next value to be stored in TexData-&gt;UniqueID<br/>
    ///</summary>
    ref int TexNextUniqueID { get; }

    ///<summary>
    /// Next value to be stored in ImFont-&gt;FontID<br/>
    ///</summary>
    ref int FontNextUniqueID { get; }

    ///<summary>
    /// List of users for this atlas. Typically one per Dear ImGui context.<br/>
    ///</summary>
    ImStructPtrVectorWrapper<IImDrawListSharedData> DrawListSharedDatas { get; }

    ///<summary>
    /// Opaque interface to our data that doesn't need to be public and may be discarded when rebuilding.<br/>
    ///</summary>
    IImFontAtlasBuilder Builder { get; }

    ///<summary>
    /// Font loader opaque interface (default to use FreeType when IMGUI_ENABLE_FREETYPE is defined, otherwise default to use stb_truetype). Use SetFontLoader() to change this at runtime.<br/>
    ///</summary>
    IImFontLoader FontLoader { get; }

    ///<summary>
    /// Font loader name (for display e.g. in About box) == FontLoader-&gt;Name<br/>
    ///</summary>
    sbyte* FontLoaderName { get; set; }

    ///<summary>
    /// Font backend opaque storage<br/>
    ///</summary>
    void* FontLoaderData { get; set; }

    ///<summary>
    /// Shared flags (for all fonts) for font loader. THIS IS BUILD IMPLEMENTATION DEPENDENT (e.g. Per-font override is also available in ImFontConfig).<br/>
    ///</summary>
    ref uint FontLoaderFlags { get; }

    ///<summary>
    /// Number of contexts using this atlas<br/>
    ///</summary>
    ref int RefCount { get; }

    ///<summary>
    /// Context which own the atlas will be in charge of updating and destroying it.<br/>
    ///</summary>
    IImGuiContext OwnerContext { get; }

    ///<summary>
    /// For old GetCustomRectByIndex() API<br/>
    ///<br/>
    /// Legacy: You can request your rectangles to be mapped as font glyph (given a font + Unicode point), so you can render e.g. custom colorful icons and use them as regular glyphs. --&gt; Prefer using a custom ImFontLoader.<br/>
    ///</summary>
    IImFontAtlasRect TempRect { get; }

    ///<summary>
    /// Latest texture identifier == TexData-&gt;GetTexRef().<br/>
    ///</summary>
    ref ImTextureRef TexRef { get; }

    ref ImTextureRef TexID { get; }
}

///<summary>
/// Font runtime data for a given size<br/>
/// Important: pointers to ImFontBaked are only valid for the current frame.<br/>
///</summary>
public unsafe interface IImFontBaked
{
    public nint NativePointer { get; }

    ///<summary>
    /// 12-16  out  Sparse. Glyphs-&gt;AdvanceX in a directly indexable way (cache-friendly for CalcTextSize functions which only this info, and are often bottleneck in large UI).<br/>
    ///<br/>
    /// [Internal] Members: Hot ~20/24 bytes (for CalcTextSize)<br/>
    ///</summary>
    ImVector<float> IndexAdvanceX { get; }

    ///<summary>
    /// 4      out  FindGlyph(FallbackChar)-&gt;AdvanceX<br/>
    ///</summary>
    ref float FallbackAdvanceX { get; }

    ///<summary>
    /// 4      in   Height of characters/line, set during loading (doesn't change after loading)<br/>
    ///</summary>
    ref float Size { get; }

    ///<summary>
    /// 4      in   Density this is baked at<br/>
    ///</summary>
    ref float RasterizerDensity { get; }

    ///<summary>
    /// 12-16  out  Sparse. Index glyphs by Unicode code-point.<br/>
    ///<br/>
    /// [Internal] Members: Hot ~28/36 bytes (for RenderText loop)<br/>
    ///</summary>
    ImVector<ushort> IndexLookup { get; }

    ///<summary>
    /// 12-16  out  All glyphs.<br/>
    ///</summary>
    ImStructVectorWrapper<IImFontGlyph> Glyphs { get; }

    ///<summary>
    /// 4      out  Index of FontFallbackChar<br/>
    ///</summary>
    ref int FallbackGlyphIndex { get; }

    ///<summary>
    /// 4+4    out  Ascent: distance from top to bottom of e.g. 'A' [0..FontSize] (unscaled)<br/>
    ///<br/>
    /// [Internal] Members: Cold<br/>
    ///</summary>
    ref float Ascent { get; }

    ///<summary>
    /// 4+4    out  Ascent: distance from top to bottom of e.g. 'A' [0..FontSize] (unscaled)<br/>
    ///<br/>
    /// [Internal] Members: Cold<br/>
    ///</summary>
    ref float Descent { get; }

    ref uint _bitfield { get; }

    ///<summary>
    /// 3   out  Total surface in pixels to get an idea of the font rasterization/texture cost (not exact, we approximate the cost of padding between glyphs)<br/>
    ///</summary>
    uint MetricsTotalSurface { get; set; }

    ///<summary>
    /// 0        Queued for destroy<br/>
    ///</summary>
    uint WantDestroy { get; set; }

    ///<summary>
    /// 0        Disable loading fallback in lower-level calls.<br/>
    ///</summary>
    uint LoadNoFallback { get; set; }

    ///<summary>
    /// 0        Enable a two-steps mode where CalcTextSize() calls will load AdvanceX *without* rendering/packing glyphs. Only advantagous if you know that the glyph is unlikely to actually be rendered, otherwise it is slower because we'd do one query on the first CalcTextSize and one query on the first Draw.<br/>
    ///</summary>
    uint LoadNoRenderOnLayout { get; set; }

    ///<summary>
    /// 4        Record of that time this was bounds<br/>
    ///</summary>
    ref int LastUsedFrame { get; }

    ///<summary>
    /// 4           Unique ID for this baked storage<br/>
    ///</summary>
    ref uint BakedId { get; }

    ///<summary>
    /// 4-8    in   Parent font<br/>
    ///</summary>
    IImFont OwnerFont { get; }

    ///<summary>
    /// 4-8         Font loader opaque storage (per baked font * sources): single contiguous buffer allocated by imgui, passed to loader.<br/>
    ///</summary>
    void* FontLoaderDatas { get; set; }
}

///<summary>
/// Font flags<br/>
/// (in future versions as we redesign font loading API, this will become more important and better documented. for now please consider this as internal/advanced use)<br/>
///</summary>
public enum ImFontFlags
{
    ImFontFlags_None = 0,
    ///<summary>
    /// Disable throwing an error/assert when calling AddFontXXX() with missing file/data. Calling code is expected to check AddFontXXX() return value.<br/>
    ///</summary>
    ImFontFlags_NoLoadError = 1 << 1,
    ///<summary>
    /// [Internal] Disable loading new glyphs.<br/>
    ///</summary>
    ImFontFlags_NoLoadGlyphs = 1 << 2,
    ///<summary>
    /// [Internal] Disable loading new baked sizes, disable garbage collecting current ones. e.g. if you want to lock a font to a single size. Important: if you use this to preload given sizes, consider the possibility of multiple font density used on Retina display.<br/>
    ///</summary>
    ImFontFlags_LockBakedSizes = 1 << 3
}

///<summary>
/// Font runtime data and rendering<br/>
/// - ImFontAtlas automatically loads a default embedded font for you if you didn't load one manually.<br/>
/// - Since 1.92.0 a font may be rendered as any size! Therefore a font doesn't have one specific size.<br/>
/// - Use 'font-&gt;GetFontBaked(size)' to retrieve the ImFontBaked* corresponding to a given size.<br/>
/// - If you used g.Font + g.FontSize (which is frequent from the ImGui layer), you can use g.FontBaked as a shortcut, as g.FontBaked == g.Font-&gt;GetFontBaked(g.FontSize).<br/>
///</summary>
public unsafe interface IImFont
{
    public nint NativePointer { get; }

    ///<summary>
    /// 4-8    Cache last bound baked. NEVER USE DIRECTLY. Use GetFontBaked().<br/>
    ///<br/>
    /// [Internal] Members: Hot ~12-20 bytes<br/>
    ///</summary>
    IImFontBaked LastBaked { get; }

    ///<summary>
    /// 4-8    What we have been loaded into.<br/>
    ///</summary>
    IImFontAtlas OwnerAtlas { get; }

    ///<summary>
    /// 4      Font flags.<br/>
    ///</summary>
    ref int Flags { get; }

    ///<summary>
    /// Current rasterizer density. This is a varying state of the font.<br/>
    ///</summary>
    ref float CurrentRasterizerDensity { get; }

    ///<summary>
    /// Unique identifier for the font<br/>
    ///<br/>
    /// [Internal] Members: Cold ~24-52 bytes<br/>
    /// Conceptually Sources[] is the list of font sources merged to create this font.<br/>
    ///</summary>
    ref uint FontId { get; }

    ///<summary>
    /// 4      in   Font size passed to AddFont(). Use for old code calling PushFont() expecting to use that size. (use ImGui::GetFontBaked() to get font baked at current bound size).<br/>
    ///</summary>
    ref float LegacySize { get; }

    ///<summary>
    /// 16     in   List of sources. Pointers within OwnerAtlas-&gt;Sources[]<br/>
    ///</summary>
    ImStructPtrVectorWrapper<IImFontConfig> Sources { get; }

    ///<summary>
    /// 2-4    out  Character used for ellipsis rendering ('...').<br/>
    ///</summary>
    ref uint EllipsisChar { get; }

    ///<summary>
    /// 2-4    out  Character used if a glyph isn't found (U+FFFD, '?')<br/>
    ///</summary>
    ref uint FallbackChar { get; }

    ///<summary>
    /// 1 bytes if ImWchar=ImWchar16, 16 bytes if ImWchar==ImWchar32. Store 1-bit for each block of 4K codepoints that has one active glyph. This is mainly used to facilitate iterations across all used codepoints.<br/>
    ///</summary>
    RangeAccessor<byte> Used8kPagesMap { get; }

    ///<summary>
    /// 1           Mark when the "..." glyph needs to be generated.<br/>
    ///</summary>
    ref bool EllipsisAutoBake { get; }

    ///<summary>
    /// 16          Remapping pairs when using AddRemapChar(), otherwise empty.<br/>
    ///</summary>
    IImGuiStorage RemapPairs { get; }

    ///<summary>
    /// 4      in   Legacy base font scale (~1.0f), multiplied by the per-window font scale which you can adjust with SetWindowFontScale()<br/>
    ///</summary>
    ref float Scale { get; }
}

///<summary>
/// Flags stored in ImGuiViewport::Flags, giving indications to the platform backends.<br/>
///</summary>
public enum ImGuiViewportFlags
{
    ImGuiViewportFlags_None = 0,
    ///<summary>
    /// Represent a Platform Window<br/>
    ///</summary>
    ImGuiViewportFlags_IsPlatformWindow = 1 << 0,
    ///<summary>
    /// Represent a Platform Monitor (unused yet)<br/>
    ///</summary>
    ImGuiViewportFlags_IsPlatformMonitor = 1 << 1,
    ///<summary>
    /// Platform Window: Is created/managed by the user application? (rather than our backend)<br/>
    ///</summary>
    ImGuiViewportFlags_OwnedByApp = 1 << 2,
    ///<summary>
    /// Platform Window: Disable platform decorations: title bar, borders, etc. (generally set all windows, but if ImGuiConfigFlags_ViewportsDecoration is set we only set this on popups/tooltips)<br/>
    ///</summary>
    ImGuiViewportFlags_NoDecoration = 1 << 3,
    ///<summary>
    /// Platform Window: Disable platform task bar icon (generally set on popups/tooltips, or all windows if ImGuiConfigFlags_ViewportsNoTaskBarIcon is set)<br/>
    ///</summary>
    ImGuiViewportFlags_NoTaskBarIcon = 1 << 4,
    ///<summary>
    /// Platform Window: Don't take focus when created.<br/>
    ///</summary>
    ImGuiViewportFlags_NoFocusOnAppearing = 1 << 5,
    ///<summary>
    /// Platform Window: Don't take focus when clicked on.<br/>
    ///</summary>
    ImGuiViewportFlags_NoFocusOnClick = 1 << 6,
    ///<summary>
    /// Platform Window: Make mouse pass through so we can drag this window while peaking behind it.<br/>
    ///</summary>
    ImGuiViewportFlags_NoInputs = 1 << 7,
    ///<summary>
    /// Platform Window: Renderer doesn't need to clear the framebuffer ahead (because we will fill it entirely).<br/>
    ///</summary>
    ImGuiViewportFlags_NoRendererClear = 1 << 8,
    ///<summary>
    /// Platform Window: Avoid merging this window into another host window. This can only be set via ImGuiWindowClass viewport flags override (because we need to now ahead if we are going to create a viewport in the first place!).<br/>
    ///</summary>
    ImGuiViewportFlags_NoAutoMerge = 1 << 9,
    ///<summary>
    /// Platform Window: Display on top (for tooltips only).<br/>
    ///</summary>
    ImGuiViewportFlags_TopMost = 1 << 10,
    ///<summary>
    /// Viewport can host multiple imgui windows (secondary viewports are associated to a single window).  FIXME: In practice there's still probably code making the assumption that this is always and only on the MainViewport. Will fix once we add support for "no main viewport".<br/>
    ///</summary>
    ImGuiViewportFlags_CanHostOtherWindows = 1 << 11,
    ///<summary>
    /// Platform Window: Window is minimized, can skip render. When minimized we tend to avoid using the viewport pos/size for clipping window or testing if they are contained in the viewport.<br/>
    ///<br/>
    /// Output status flags (from Platform)<br/>
    ///</summary>
    ImGuiViewportFlags_IsMinimized = 1 << 12,
    ///<summary>
    /// Platform Window: Window is focused (last call to Platform_GetWindowFocus() returned true)<br/>
    ///</summary>
    ImGuiViewportFlags_IsFocused = 1 << 13
}

///<summary>
/// - Currently represents the Platform Window created by the application which is hosting our Dear ImGui windows.<br/>
/// - With multi-viewport enabled, we extend this concept to have multiple active viewports.<br/>
/// - In the future we will extend this concept further to also represent Platform Monitor and support a "no main platform window" operation mode.<br/>
/// - About Main Area vs Work Area:<br/>
///   - Main Area = entire viewport.<br/>
///   - Work Area = entire viewport minus sections used by main menu bars (for platform windows), or by task bar (for platform monitor).<br/>
///   - Windows are generally trying to stay within the Work Area of their host viewport.<br/>
///</summary>
public unsafe interface IImGuiViewport
{
    public nint NativePointer { get; }

    ///<summary>
    /// Unique identifier for the viewport<br/>
    ///</summary>
    ref uint ID { get; }

    ///<summary>
    /// See ImGuiViewportFlags_<br/>
    ///</summary>
    ref ImGuiViewportFlags Flags { get; }

    ///<summary>
    /// Main Area: Position of the viewport (Dear ImGui coordinates are the same as OS desktop/native coordinates)<br/>
    ///</summary>
    ref Vector2 Pos { get; }

    ///<summary>
    /// Main Area: Size of the viewport.<br/>
    ///</summary>
    ref Vector2 Size { get; }

    ///<summary>
    /// Density of the viewport for Retina display (always 1,1 on Windows, may be 2,2 etc on macOS/iOS). This will affect font rasterizer density.<br/>
    ///</summary>
    ref Vector2 FramebufferScale { get; }

    ///<summary>
    /// Work Area: Position of the viewport minus task bars, menus bars, status bars (&gt;= Pos)<br/>
    ///</summary>
    ref Vector2 WorkPos { get; }

    ///<summary>
    /// Work Area: Size of the viewport minus task bars, menu bars, status bars (&lt;= Size)<br/>
    ///</summary>
    ref Vector2 WorkSize { get; }

    ///<summary>
    /// 1.0f = 96 DPI = No extra scale.<br/>
    ///</summary>
    ref float DpiScale { get; }

    ///<summary>
    /// (Advanced) 0: no parent. Instruct the platform backend to setup a parent/child relationship between platform windows.<br/>
    ///</summary>
    ref uint ParentViewportId { get; }

    ///<summary>
    /// (Advanced) Direct shortcut to ImGui::FindViewportByID(ParentViewportId). NULL: no parent.<br/>
    ///</summary>
    IImGuiViewport ParentViewport { get; }

    ///<summary>
    /// The ImDrawData corresponding to this viewport. Valid after Render() and until the next call to NewFrame().<br/>
    ///</summary>
    IImDrawData DrawData { get; }

    ///<summary>
    /// void* to hold custom data structure for the renderer (e.g. swap chain, framebuffers etc.). generally set by your Renderer_CreateWindow function.<br/>
    ///<br/>
    /// Platform/Backend Dependent Data<br/>
    /// Our design separate the Renderer and Platform backends to facilitate combining default backends with each others.<br/>
    /// When our create your own backend for a custom engine, it is possible that both Renderer and Platform will be handled<br/>
    /// by the same system and you may not need to use all the UserData/Handle fields.<br/>
    /// The library never uses those fields, they are merely storage to facilitate backend implementation.<br/>
    ///</summary>
    void* RendererUserData { get; set; }

    ///<summary>
    /// void* to hold custom data structure for the OS / platform (e.g. windowing info, render context). generally set by your Platform_CreateWindow function.<br/>
    ///</summary>
    void* PlatformUserData { get; set; }

    ///<summary>
    /// void* to hold higher-level, platform window handle (e.g. HWND for Win32 backend, Uint32 WindowID for SDL, GLFWWindow* for GLFW), for FindViewportByPlatformHandle().<br/>
    ///</summary>
    void* PlatformHandle { get; set; }

    ///<summary>
    /// void* to hold lower-level, platform-native window handle (always HWND on Win32 platform, unused for other platforms).<br/>
    ///</summary>
    void* PlatformHandleRaw { get; set; }

    ///<summary>
    /// Platform window has been created (Platform_CreateWindow() has been called). This is false during the first frame where a viewport is being created.<br/>
    ///</summary>
    ref bool PlatformWindowCreated { get; }

    ///<summary>
    /// Platform window requested move (e.g. window was moved by the OS / host window manager, authoritative position will be OS window position)<br/>
    ///</summary>
    ref bool PlatformRequestMove { get; }

    ///<summary>
    /// Platform window requested resize (e.g. window was resized by the OS / host window manager, authoritative size will be OS window size)<br/>
    ///</summary>
    ref bool PlatformRequestResize { get; }

    ///<summary>
    /// Platform window requested closure (e.g. window was moved by the OS / host window manager, e.g. pressing ALT-F4)<br/>
    ///</summary>
    ref bool PlatformRequestClose { get; }
}

///<summary>
/// Access via ImGui::GetPlatformIO()<br/>
///</summary>
public unsafe interface IImGuiPlatformIO
{
    public nint NativePointer { get; }

    ///<summary>
    /// Optional: Access OS clipboard<br/>
    /// (default to use native Win32 clipboard on Windows, otherwise uses a private clipboard. Override to access OS clipboard on other architectures)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint> Platform_GetClipboardTextFn { get; set; }

    delegate* unmanaged[Cdecl]<nint, nint, void> Platform_SetClipboardTextFn { get; set; }

    void* Platform_ClipboardUserData { get; set; }

    ///<summary>
    /// Optional: Open link/folder/file in OS Shell<br/>
    /// (default to use ShellExecuteW() on Windows, system() on Linux/Mac. expected to return false on failure, but some platforms may always return true)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint, byte> Platform_OpenInShellFn { get; set; }

    void* Platform_OpenInShellUserData { get; set; }

    ///<summary>
    /// Optional: Notify OS Input Method Editor of the screen position of your cursor for text input position (e.g. when using Japanese/Chinese IME on Windows)<br/>
    /// (default to use native imm32 api on Windows)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint, nint, void> Platform_SetImeDataFn { get; set; }

    void* Platform_ImeUserData { get; set; }

    ///<summary>
    /// '.'<br/>
    ///<br/>
    /// Optional: Platform locale<br/>
    /// [Experimental] Configure decimal point e.g. '.' or ',' useful for some languages (e.g. German), generally pulled from *localeconv()-&gt;decimal_point<br/>
    ///</summary>
    ref uint Platform_LocaleDecimalPoint { get; }

    ///<summary>
    /// Optional: Maximum texture size supported by renderer (used to adjust how we size textures). 0 if not known.<br/>
    ///</summary>
    ref int Renderer_TextureMaxWidth { get; }

    ref int Renderer_TextureMaxHeight { get; }

    ///<summary>
    /// Written by some backends during ImGui_ImplXXXX_RenderDrawData() call to point backend_specific ImGui_ImplXXXX_RenderState* structure.<br/>
    ///</summary>
    void* Renderer_RenderState { get; set; }

    ///<summary>
    /// . . U . .   Create a new platform window for the given viewport<br/>
    ///<br/>
    /// Platform Backend functions (e.g. Win32, GLFW, SDL) ------------------- Called by -----<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, void> Platform_CreateWindow { get; set; }

    ///<summary>
    /// N . U . D  <br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, void> Platform_DestroyWindow { get; set; }

    ///<summary>
    /// . . U . .   Newly created windows are initially hidden so SetWindowPos/Size/Title can be called on them before showing the window<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, void> Platform_ShowWindow { get; set; }

    ///<summary>
    /// . . U . .   Set platform window position (given the upper-left corner of client area)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, Vector2, void> Platform_SetWindowPos { get; set; }

    ///<summary>
    /// N . . . .   (Use ImGuiPlatformIO_SetPlatform_GetWindowPos() to set this from C, otherwise you will likely encounter stack corruption)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, Vector2> Platform_GetWindowPos { get; set; }

    ///<summary>
    /// . . U . .   Set platform window client area size (ignoring OS decorations such as OS title bar etc.)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, Vector2, void> Platform_SetWindowSize { get; set; }

    ///<summary>
    /// N . . . .   Get platform window client area size (Use ImGuiPlatformIO_SetPlatform_GetWindowSize() to set this from C, otherwise you will likely encounter stack corruption)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, Vector2> Platform_GetWindowSize { get; set; }

    ///<summary>
    /// N . . . .   Return viewport density. Always 1,1 on Windows, often 2,2 on Retina display on macOS/iOS. MUST BE INTEGER VALUES. (Use ImGuiPlatformIO_SetPlatform_GetWindowFramebufferScale() to set this from C, otherwise you will likely encounter stack corruption)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, Vector2> Platform_GetWindowFramebufferScale { get; set; }

    ///<summary>
    /// N . . . .   Move window to front and set input focus<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, void> Platform_SetWindowFocus { get; set; }

    ///<summary>
    /// . . U . .  <br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, byte> Platform_GetWindowFocus { get; set; }

    ///<summary>
    /// N . . . .   Get platform window minimized state. When minimized, we generally won't attempt to get/set size and contents will be culled more easily<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, byte> Platform_GetWindowMinimized { get; set; }

    ///<summary>
    /// . . U . .   Set platform window title (given an UTF-8 string)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint, void> Platform_SetWindowTitle { get; set; }

    ///<summary>
    /// . . U . .   (Optional) Setup global transparency (not per-pixel transparency)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, float, void> Platform_SetWindowAlpha { get; set; }

    ///<summary>
    /// . . U . .   (Optional) Called by UpdatePlatformWindows(). Optional hook to allow the platform backend from doing general book-keeping every frame.<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, void> Platform_UpdateWindow { get; set; }

    ///<summary>
    /// . . . R .   (Optional) Main rendering (platform side! This is often unused, or just setting a "current" context for OpenGL bindings). 'render_arg' is the value passed to RenderPlatformWindowsDefault().<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint, void> Platform_RenderWindow { get; set; }

    ///<summary>
    /// . . . R .   (Optional) Call Present/SwapBuffers (platform side! This is often unused!). 'render_arg' is the value passed to RenderPlatformWindowsDefault().<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint, void> Platform_SwapBuffers { get; set; }

    ///<summary>
    /// N . . . .   (Optional) [BETA] FIXME-DPI: DPI handling: Return DPI scale for this viewport. 1.0f = 96 DPI.<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, float> Platform_GetWindowDpiScale { get; set; }

    ///<summary>
    /// . F . . .   (Optional) [BETA] FIXME-DPI: DPI handling: Called during Begin() every time the viewport we are outputting into changes, so backend has a chance to swap fonts to adjust style.<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, void> Platform_OnChangedViewport { get; set; }

    ///<summary>
    /// N . . . .   (Optional) [BETA] Get initial work area inset for the viewport (won't be covered by main menu bar, dockspace over viewport etc.). Default to (0,0),(0,0). 'safeAreaInsets' in iOS land, 'DisplayCutout' in Android land. (Use ImGuiPlatformIO_SetPlatform_GetWindowWorkAreaInsets() to set this from C, otherwise you will likely encounter stack corruption)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, Vector4> Platform_GetWindowWorkAreaInsets { get; set; }

    ///<summary>
    /// (Optional) For a Vulkan Renderer to call into Platform code (since the surface creation needs to tie them both).<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, ulong, nint, nint, int> Platform_CreateVkSurface { get; set; }

    ///<summary>
    /// . . U . .   Create swap chain, frame buffers etc. (called after Platform_CreateWindow)<br/>
    ///<br/>
    /// Renderer Backend functions (e.g. DirectX, OpenGL, Vulkan) ------------ Called by -----<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, void> Renderer_CreateWindow { get; set; }

    ///<summary>
    /// N . U . D   Destroy swap chain, frame buffers etc. (called before Platform_DestroyWindow)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, void> Renderer_DestroyWindow { get; set; }

    ///<summary>
    /// . . U . .   Resize swap chain, frame buffers etc. (called after Platform_SetWindowSize)<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, Vector2, void> Renderer_SetWindowSize { get; set; }

    ///<summary>
    /// . . . R .   (Optional) Clear framebuffer, setup render target, then render the viewport-&gt;DrawData. 'render_arg' is the value passed to RenderPlatformWindowsDefault().<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint, void> Renderer_RenderWindow { get; set; }

    ///<summary>
    /// . . . R .   (Optional) Call Present/SwapBuffers. 'render_arg' is the value passed to RenderPlatformWindowsDefault().<br/>
    ///</summary>
    delegate* unmanaged[Cdecl]<nint, nint, void> Renderer_SwapBuffers { get; set; }

    ///<summary>
    /// (Optional) Monitor list<br/>
    /// - Updated by: app/backend. Update every frame to dynamically support changing monitor or DPI configuration.<br/>
    /// - Used by: dear imgui to query DPI info, clamp popups/tooltips within same monitor and not have them straddle monitors.<br/>
    ///</summary>
    ImStructVectorWrapper<IImGuiPlatformMonitor> Monitors { get; }

    ///<summary>
    /// List of textures used by Dear ImGui (most often 1) + contents of external texture list is automatically appended into this.<br/>
    ///<br/>
    /// Textures list (the list is updated by calling ImGui::EndFrame or ImGui::Render)<br/>
    /// The ImGui_ImplXXXX_RenderDrawData() function of each backend generally access this via ImDrawData::Textures which points to this. The array is available here mostly because backends will want to destroy textures on shutdown.<br/>
    ///</summary>
    ImStructPtrVectorWrapper<IImTextureData> Textures { get; }

    ///<summary>
    /// Main viewports, followed by all secondary viewports.<br/>
    ///<br/>
    /// Viewports list (the list is updated by calling ImGui::EndFrame or ImGui::Render)<br/>
    /// (in the future we will attempt to organize this feature to remove the need for a "main viewport")<br/>
    ///</summary>
    ImStructPtrVectorWrapper<IImGuiViewport> Viewports { get; }
}

///<summary>
/// (Optional) This is required when enabling multi-viewport. Represent the bounds of each connected monitor/display and their DPI.<br/>
/// We use this information for multiple DPI support + clamping the position of popups and tooltips so they don't straddle multiple monitors.<br/>
///</summary>
public unsafe interface IImGuiPlatformMonitor
{
    public nint NativePointer { get; }

    ///<summary>
    /// Coordinates of the area displayed on this monitor (Min = upper left, Max = bottom right)<br/>
    ///</summary>
    ref Vector2 MainPos { get; }

    ///<summary>
    /// Coordinates of the area displayed on this monitor (Min = upper left, Max = bottom right)<br/>
    ///</summary>
    ref Vector2 MainSize { get; }

    ///<summary>
    /// Coordinates without task bars / side bars / menu bars. Used to avoid positioning popups/tooltips inside this region. If you don't have this info, please copy the value for MainPos/MainSize.<br/>
    ///</summary>
    ref Vector2 WorkPos { get; }

    ///<summary>
    /// Coordinates without task bars / side bars / menu bars. Used to avoid positioning popups/tooltips inside this region. If you don't have this info, please copy the value for MainPos/MainSize.<br/>
    ///</summary>
    ref Vector2 WorkSize { get; }

    ///<summary>
    /// 1.0f = 96 DPI<br/>
    ///</summary>
    ref float DpiScale { get; }

    ///<summary>
    /// Backend dependant data (e.g. HMONITOR, GLFWmonitor*, SDL Display Index, NSScreen*)<br/>
    ///</summary>
    void* PlatformHandle { get; set; }
}

///<summary>
/// (Optional) Support for IME (Input Method Editor) via the platform_io.Platform_SetImeDataFn() function. Handler is called during EndFrame().<br/>
///</summary>
public unsafe interface IImGuiPlatformImeData
{
    public nint NativePointer { get; }

    ///<summary>
    /// A widget wants the IME to be visible.<br/>
    ///</summary>
    ref bool WantVisible { get; }

    ///<summary>
    /// A widget wants text input, not necessarily IME to be visible. This is automatically set to the upcoming value of io.WantTextInput.<br/>
    ///</summary>
    ref bool WantTextInput { get; }

    ///<summary>
    /// Position of input cursor (for IME).<br/>
    ///</summary>
    ref Vector2 InputPos { get; }

    ///<summary>
    /// Line height (for IME).<br/>
    ///</summary>
    ref float InputLineHeight { get; }

    ///<summary>
    /// ID of platform window/viewport.<br/>
    ///</summary>
    ref uint ViewportId { get; }
}

public enum ImGuiFreeTypeLoaderFlags
{
    ///<summary>
    /// Disable hinting. This generally generates 'blurrier' bitmap glyphs when the glyph are rendered in any of the anti-aliased modes.<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_NoHinting = 1 << 0,
    ///<summary>
    /// Disable auto-hinter.<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_NoAutoHint = 1 << 1,
    ///<summary>
    /// Indicates that the auto-hinter is preferred over the font's native hinter.<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_ForceAutoHint = 1 << 2,
    ///<summary>
    /// A lighter hinting algorithm for gray-level modes. Many generated glyphs are fuzzier but better resemble their original shape. This is achieved by snapping glyphs to the pixel grid only vertically (Y-axis), as is done by Microsoft's ClearType and Adobe's proprietary font renderer. This preserves inter-glyph spacing in horizontal text.<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_LightHinting = 1 << 3,
    ///<summary>
    /// Strong hinting algorithm that should only be used for monochrome output.<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_MonoHinting = 1 << 4,
    ///<summary>
    /// Styling: Should we artificially embolden the font?<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_Bold = 1 << 5,
    ///<summary>
    /// Styling: Should we slant the font, emulating italic style?<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_Oblique = 1 << 6,
    ///<summary>
    /// Disable anti-aliasing. Combine this with MonoHinting for best results!<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_Monochrome = 1 << 7,
    ///<summary>
    /// Enable FreeType color-layered glyphs<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_LoadColor = 1 << 8,
    ///<summary>
    /// Enable FreeType bitmap glyphs<br/>
    ///</summary>
    ImGuiFreeTypeLoaderFlags_Bitmap = 1 << 9,
    ImGuiFreeTypeBuilderFlags_NoHinting = ImGuiFreeTypeLoaderFlags_NoHinting,
    ImGuiFreeTypeBuilderFlags_NoAutoHint = ImGuiFreeTypeLoaderFlags_NoAutoHint,
    ImGuiFreeTypeBuilderFlags_ForceAutoHint = ImGuiFreeTypeLoaderFlags_ForceAutoHint,
    ImGuiFreeTypeBuilderFlags_LightHinting = ImGuiFreeTypeLoaderFlags_LightHinting,
    ImGuiFreeTypeBuilderFlags_MonoHinting = ImGuiFreeTypeLoaderFlags_MonoHinting,
    ImGuiFreeTypeBuilderFlags_Bold = ImGuiFreeTypeLoaderFlags_Bold,
    ImGuiFreeTypeBuilderFlags_Oblique = ImGuiFreeTypeLoaderFlags_Oblique,
    ImGuiFreeTypeBuilderFlags_Monochrome = ImGuiFreeTypeLoaderFlags_Monochrome,
    ImGuiFreeTypeBuilderFlags_LoadColor = ImGuiFreeTypeLoaderFlags_LoadColor,
    ImGuiFreeTypeBuilderFlags_Bitmap = ImGuiFreeTypeLoaderFlags_Bitmap
}

///<summary>
/// This is automatically assigned when using '#define IMGUI_ENABLE_FREETYPE'.<br/>
/// If you need to dynamically select between multiple builders:<br/>
/// - you can manually assign this builder with 'atlas-&gt;SetFontLoader(ImGuiFreeType::GetFontLoader())'<br/>
///</summary>
public unsafe interface IImGuiFreeTypeImDrawData
{
    public nint NativePointer { get; }
}