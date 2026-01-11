using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Utils;

public static class ColorUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RGBA(byte r, byte g, byte b, byte a = 255)
        => r | ((uint)g << 8) | ((uint)b << 16) | ((uint)a << 24);

    public static Vector4 ToVector4(this Color color) => new(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
}