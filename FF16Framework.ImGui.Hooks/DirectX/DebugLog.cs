using System.Diagnostics;

using FF16Framework.ImGui.Hooks.Misc;

namespace FF16Framework.ImGui.Hooks.DirectX;

public class DebugLog
{
    [Conditional("DEBUG")]
    public static void DebugWriteLine(string text) => SDK.Debug?.Invoke(text);
    public static void WriteLine(string text) => SDK.Debug?.Invoke(text);
}
