using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Native;

namespace NenTools.ImGui.Implementation;

public unsafe struct ImGuiStoragePair : IImGuiStoragePair
{
    public nint NativePointer { get; }

    public ImGuiStoragePair(ImGuiStoragePairStruct* nativePtr) => NativePointer = (nint)nativePtr;
    public readonly ref uint key => ref Unsafe.AsRef<uint>(&((ImGuiStoragePairStruct*)NativePointer)->key);
    public readonly ref int val_i => ref ((ImGuiStoragePairStruct*)NativePointer)->val_i;
    public readonly ref float val_f => ref ((ImGuiStoragePairStruct*)NativePointer)->val_f;
    public readonly ref nint val_p => ref ((ImGuiStoragePairStruct*)NativePointer)->val_p;
}