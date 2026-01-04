using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Utils;

public class ColorUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RGBA(byte r, byte g, byte b, byte a = 255)
        => r | ((uint)g << 8) | ((uint)b << 16) | ((uint)a << 24);
}
