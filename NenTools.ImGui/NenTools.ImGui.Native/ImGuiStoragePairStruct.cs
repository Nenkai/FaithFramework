using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Native;

public unsafe partial struct ImGuiStoragePairStruct
{
    public uint key;
    public _Anonymous_e__Union Anonymous;

    public ref int val_i => ref MemoryMarshal.GetReference(MemoryMarshal.CreateSpan(ref Anonymous.val_i, 1));
    public ref float val_f => ref MemoryMarshal.GetReference(MemoryMarshal.CreateSpan(ref Anonymous.val_f, 1));
    public ref nint val_p => ref MemoryMarshal.GetReference(MemoryMarshal.CreateSpan(ref this, 1)).Anonymous.val_p;

    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct _Anonymous_e__Union
    {
        [FieldOffset(0)]
        public int val_i;

        [FieldOffset(0)]
        public float val_f;

        [FieldOffset(0)]
        public nint val_p;
    }
}
