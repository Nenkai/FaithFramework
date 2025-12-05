using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Native.Windows;

public class NativeMethods
{
    [DllImport("user32.dll", EntryPoint = "IsWindowUnicode")]
    private static extern bool IsWindowUnicode(nint hWnd);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern IntPtr GetWindowLongPtr32(nint hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr64(nint hWnd, int nIndex);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongW")]
    private static extern IntPtr GetWindowLongPtr32W(nint hWnd, int nIndex);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongPtrW")]
    private static extern IntPtr GetWindowLongPtr64W(nint hWnd, int nIndex);

    public static IntPtr GetWindowLong(nint hWnd, GWL nIndex)
    {
        if (IsWindowUnicode(hWnd))
            return GetWindowLongW(hWnd, nIndex);

        return GetWindowLongA(hWnd, nIndex);
    }

    public static IntPtr GetWindowLongA(nint hWnd, GWL nIndex)
    {
        var is64Bit = Environment.Is64BitProcess;
        return is64Bit ? GetWindowLongPtr64(hWnd, (int)nIndex) : GetWindowLongPtr32(hWnd, (int)nIndex);
    }

    public static IntPtr GetWindowLongW(nint hWnd, GWL nIndex)
    {
        var is64Bit = Environment.Is64BitProcess;
        return is64Bit ? GetWindowLongPtr64W(hWnd, (int)nIndex) : GetWindowLongPtr32W(hWnd, (int)nIndex);
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int X;
    public int Y;

    public static implicit operator Point(POINT point)
    {
        return new Point(point.X, point.Y);
    }
}