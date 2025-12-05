using System.Diagnostics;

using NenTools.ImGui.Hooks.Misc;

namespace NenTools.ImGui.Hooks.DirectX;

public class DebugLog
{
    [Conditional("DEBUG")]
    public static void DebugWriteLine(string text) => SDK.Debug?.Invoke(text);
    public static void WriteLine(string text) => SDK.Debug?.Invoke(text);
}
