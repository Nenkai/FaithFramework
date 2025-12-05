using Reloaded.Hooks.Definitions.Structs;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Windows.Win32;
using Windows.Win32.Foundation;

namespace NenTools.ImGui.Hooks.DirectX12;

internal class RTTIUtility
{
    public unsafe static TypeDescriptor? GetTypeInfo(void* obj)
    {
        var locator = GetLocator(obj);

        var modules = Process.GetCurrentProcess().Modules;
        foreach (ProcessModule module in modules)
        {
            if ((nint)locator >= module.BaseAddress && (nint)locator < module.BaseAddress + module.ModuleMemorySize)
            {

                // Marshal TypeDescriptor
                var typeDesc = Marshal.PtrToStructure<TypeDescriptor>((nint)module.BaseAddress + locator->pTypeDescriptor);
            }
        }


        return null;
    }

    public unsafe static RTTICompleteObjectLocator* GetLocator(void* obj)
    {
        if (obj is null || *(void**)obj == null)
            return null;

        return *(RTTICompleteObjectLocator**)(*(nuint*)obj - (nuint)sizeof(nuint));
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TypeDescriptor
    {
        public IntPtr pVFTable;
        public IntPtr spare;
        public IntPtr name; // pointer to decorated name
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PMD
    {
        public int mdisp;
        public int pdisp;
        public int vdisp;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RTTICompleteObjectLocator
    {
        public uint signature;
        public uint offset;
        public uint cdOffset;
        public int pTypeDescriptor;
        public int pClassHierarchyDescriptor;
        public int pSelf; // usually same as start
    }

    [DllImport("Dbghelp.dll", CharSet = CharSet.Ansi)]
    static extern uint UnDecorateSymbolName(
    string name,
    StringBuilder outputString,
    int maxStringLength,
    uint flags);

    const uint UNDNAME_COMPLETE = 0x0000;
}
