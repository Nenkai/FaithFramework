using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Interfaces;

public interface IImGuiStoragePair
{
    public nint NativePointer { get; }

    public ref uint key { get; }

    public ref int val_i { get; }
    public ref float val_f { get; }
    public ref nint val_p { get; }
}
