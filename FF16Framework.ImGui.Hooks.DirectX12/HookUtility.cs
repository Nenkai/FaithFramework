using Reloaded.Hooks.Definitions.Structs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Windows.Win32;
using Windows.Win32.Foundation;

namespace FF16Framework.ImGui.Hooks.DirectX12;

internal class HookUtility
{
    /// <summary>
    /// Gets the module where the specified addresses resides.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public unsafe static HMODULE? GetModuleWithin(nuint address)
    {
        const int GET_MODULE_HANDLE_EX_FLAG_PIN                 = 0x00000001;
        const int GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT  = 0x00000002;
        const int GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS = 0x00000004;

        HMODULE module_;
        if (PInvoke.GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT, new PCWSTR((char*)address), &module_))
            return (HMODULE)module_.Value;

        return null;
    }

    /// <summary>
    /// Returns the altered original bytes of a function at the specified address, if it has been hooked.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public static List<byte>? GetOriginalBytesIfHooked(nuint address)
    {
        HMODULE? moduleWithin = GetModuleWithin(address);

        if (moduleWithin is null)
            return null;

        return GetOriginalBytes(moduleWithin.Value, address);
    }

    /// <summary>
    /// Returns the raw file bytes of the specified module from disk.
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    public static byte[]? ReadModuleFromDisk(HMODULE module)
    {
        string? path = GetModulePath(module);
        if (string.IsNullOrEmpty(path))
            return null;

        return File.ReadAllBytes(path);
    }

    /// <summary>
    /// Returns the path for a specified module.
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    public unsafe static string? GetModulePath(HMODULE module)
    {
        const int MAX_PATH = 260;
        char[] bytes = new char[MAX_PATH];

        fixed (char* filename = bytes)
        {
            if (PInvoke.GetModuleFileName(module, new PWSTR(filename), MAX_PATH) >= MAX_PATH)
                return null;
        }

        return Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(new string(bytes))).TrimEnd('\0');
    }

    /// <summary>
    /// Returns the non-altered, original bytes of the specified module, for the specified address. <br/>
    /// The number of bytes returned is equal to the amount of bytes that are non-matching.
    /// </summary>
    /// <param name="module"></param>
    /// <param name="address"></param>
    /// <returns></returns>
    public static unsafe List<byte>? GetOriginalBytes(HMODULE module, nuint address)
    {
        byte[]? diskData = ReadModuleFromDisk(module);

        if (diskData is null)
            return null;

        nuint? moduleBase = GetDllImageBase(module);
        if (moduleBase is null)
            return null;

        nuint moduleRVA = (nuint)(address - moduleBase);

        // obtain the file offset of the address now
        nuint? diskPtr = PtrFromRVA(diskData, moduleRVA);

        if (diskPtr is null)
            return null;

        List<byte> originalBytes = [];

        byte* moduleBytes = (byte*)address;
        byte* diskBytes = (byte*)diskPtr;

        // copy the bytes from the disk data to the original bytes
        // copy only until the bytes start to match eachother
        for (int i = 0; ; ++i)
        {
            if (moduleBytes[i] == diskBytes[i])
            {
                bool matches = true;

                // Lookahead 4 bytes to check if any other part is different before breaking out.
                for (int j = 1; j <= 4; ++j)
                {
                    if (moduleBytes[i + j] != diskBytes[i + j])
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches)
                    break;
            }

            originalBytes.Add(diskBytes[i]);
        }

        if (originalBytes.Count == 0)
            return null;

        return originalBytes;
    }

    unsafe static IMAGE_SECTION_HEADER* IMAGE_FIRST_SECTION(ref IMAGE_NT_HEADERS64 ntHeaders)
    {
        byte* basePtr = (byte*)Unsafe.AsPointer(ref ntHeaders);
        int offset = Marshal.OffsetOf<IMAGE_NT_HEADERS64>(nameof(IMAGE_NT_HEADERS64.OptionalHeader)).ToInt32() + ntHeaders.FileHeader.SizeOfOptionalHeader;

        return (IMAGE_SECTION_HEADER*)(basePtr + offset);
    }

    public static unsafe nuint? PtrFromRVA(byte[] dll, nuint rva)
    {
        // Get the first section.
        fixed (byte* dllPtr = dll)
        {
            ref var dosHeader = ref Unsafe.AsRef<IMAGE_DOS_HEADER>(dllPtr);
            ref var ntHeaders = ref Unsafe.AsRef<IMAGE_NT_HEADERS64>((void*)(dllPtr + dosHeader.e_lfanew));
            var section = IMAGE_FIRST_SECTION(ref ntHeaders);

            // Go through each section searching for where the rva lands.
            for (ushort i = 0; i < ntHeaders.FileHeader.NumberOfSections; ++i, ++section)
            {
                var size = section->VirtualSize;
                if (size == 0)
                    size = section->SizeOfRawData;

                if (rva >= section->VirtualAddress && rva < (section->VirtualAddress + size))
                {
                    var delta = section->VirtualAddress - section->PointerToRawData;
                    return (nuint)(dllPtr + (rva - delta));
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the image base for the specified dll.
    /// </summary>
    /// <param name="dll">Pointer of the dll header.</param>
    /// <returns></returns>
    public unsafe static nuint? GetDllImageBase(nint dll)
    {
        if (dll == 0)
            return null;

        // Get the dos header and verify that it seems valid.
        ref IMAGE_DOS_HEADER dosHeader = ref Unsafe.AsRef<IMAGE_DOS_HEADER>((void*)dll);

        if (dosHeader.e_magic != 0x5A4D) // IMAGE_DOS_SIGNATURE
            return null;

        // Get the nt headers and verify that they seem valid.
        ref IMAGE_NT_HEADERS64 ntHeaders = ref Unsafe.AsRef<IMAGE_NT_HEADERS64>((void*)(dll + dosHeader.e_lfanew));

        if (ntHeaders.Signature != 0x4550) // IMAGE_NT_SIGNATURE
            return null;

        return (nuint)ntHeaders.OptionalHeader.ImageBase;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct IMAGE_DOS_HEADER
    {
        public ushort e_magic;       // Magic number
        public ushort e_cblp;    // Bytes on last page of file
        public ushort e_cp;      // Pages in file
        public ushort e_crlc;    // Relocations
        public ushort e_cparhdr;     // Size of header in paragraphs
        public ushort e_minalloc;    // Minimum extra paragraphs needed
        public ushort e_maxalloc;    // Maximum extra paragraphs needed
        public ushort e_ss;      // Initial (relative) SS value
        public ushort e_sp;      // Initial SP value
        public ushort e_csum;    // Checksum
        public ushort e_ip;      // Initial IP value
        public ushort e_cs;      // Initial (relative) CS value
        public ushort e_lfarlc;      // File address of relocation table
        public ushort e_ovno;    // Overlay number
        public fixed ushort e_res[4]; // 0x1C
        public ushort e_oemid;       // OEM identifier (for e_oeminfo)
        public ushort e_oeminfo;     // OEM information; e_oemid specific
        public fixed ushort e_res2[10]; // 0x28
        public int e_lfanew;      // File address of new exe header
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_NT_HEADERS64
    {
        public uint Signature;
        public IMAGE_FILE_HEADER FileHeader;
        public IMAGE_OPTIONAL_HEADER64 OptionalHeader;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_FILE_HEADER
    {
        public ushort Machine;
        public ushort NumberOfSections;
        public uint TimeDateStamp;
        public uint PointerToSymbolTable;
        public uint NumberOfSymbols;
        public ushort SizeOfOptionalHeader;
        public ushort Characteristics;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct IMAGE_OPTIONAL_HEADER64
    {
        public ushort Magic;
        public byte MajorLinkerVersion;
        public byte MinorLinkerVersion;
        public uint SizeOfCode;
        public uint SizeOfInitializedData;
        public uint SizeOfUninitializedData;
        public uint AddressOfEntryPoint;
        public uint BaseOfCode;
        public ulong ImageBase;
        public uint SectionAlignment;
        public uint FileAlignment;
        public ushort MajorOperatingSystemVersion;
        public ushort MinorOperatingSystemVersion;
        public ushort MajorImageVersion;
        public ushort MinorImageVersion;
        public ushort MajorSubsystemVersion;
        public ushort MinorSubsystemVersion;
        public uint Win32VersionValue;
        public uint SizeOfImage;
        public uint SizeOfHeaders;
        public uint CheckSum;
        public ushort Subsystem;
        public ushort DllCharacteristics;
        public ulong SizeOfStackReserve;
        public ulong SizeOfStackCommit;
        public ulong SizeOfHeapReserve;
        public ulong SizeOfHeapCommit;
        public uint LoaderFlags;
        public uint NumberOfRvaAndSizes;
        public fixed uint DataDirectory[32];
    }

    public struct IMAGE_DATA_DIRECTORY
    {
        public uint VirtualAddress;
        public uint Size;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct IMAGE_SECTION_HEADER
    {
        public fixed byte Name[8];
        public uint VirtualSize;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public uint PointerToRelocations;
        public uint PointerToLinenumbers;
        public ushort NumberOfRelocations;
        public ushort NumberOfLinenumbers;
        public DataSectionFlags Characteristics;

        [Flags]
        public enum DataSectionFlags : uint
        {
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeReg = 0x00000000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeDsect = 0x00000001,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeNoLoad = 0x00000002,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeGroup = 0x00000004,
            /// <summary>
            /// The section should not be padded to the next boundary. This flag is obsolete and is replaced by IMAGE_SCN_ALIGN_1BYTES. This is valid only for object files.
            /// </summary>
            TypeNoPadded = 0x00000008,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeCopy = 0x00000010,
            /// <summary>
            /// The section contains executable code.
            /// </summary>
            ContentCode = 0x00000020,
            /// <summary>
            /// The section contains initialized data.
            /// </summary>
            ContentInitializedData = 0x00000040,
            /// <summary>
            /// The section contains uninitialized data.
            /// </summary>
            ContentUninitializedData = 0x00000080,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            LinkOther = 0x00000100,
            /// <summary>
            /// The section contains comments or other information. The .drectve section has this type. This is valid for object files only.
            /// </summary>
            LinkInfo = 0x00000200,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            TypeOver = 0x00000400,
            /// <summary>
            /// The section will not become part of the image. This is valid only for object files.
            /// </summary>
            LinkRemove = 0x00000800,
            /// <summary>
            /// The section contains COMDAT data. For more information, see section 5.5.6, COMDAT Sections (Object Only). This is valid only for object files.
            /// </summary>
            LinkComDat = 0x00001000,
            /// <summary>
            /// Reset speculative exceptions handling bits in the TLB entries for this section.
            /// </summary>
            NoDeferSpecExceptions = 0x00004000,
            /// <summary>
            /// The section contains data referenced through the global pointer (GP).
            /// </summary>
            RelativeGP = 0x00008000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            MemPurgeable = 0x00020000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            Memory16Bit = 0x00020000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            MemoryLocked = 0x00040000,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            MemoryPreload = 0x00080000,
            /// <summary>
            /// Align data on a 1-byte boundary. Valid only for object files.
            /// </summary>
            Align1Bytes = 0x00100000,
            /// <summary>
            /// Align data on a 2-byte boundary. Valid only for object files.
            /// </summary>
            Align2Bytes = 0x00200000,
            /// <summary>
            /// Align data on a 4-byte boundary. Valid only for object files.
            /// </summary>
            Align4Bytes = 0x00300000,
            /// <summary>
            /// Align data on an 8-byte boundary. Valid only for object files.
            /// </summary>
            Align8Bytes = 0x00400000,
            /// <summary>
            /// Align data on a 16-byte boundary. Valid only for object files.
            /// </summary>
            Align16Bytes = 0x00500000,
            /// <summary>
            /// Align data on a 32-byte boundary. Valid only for object files.
            /// </summary>
            Align32Bytes = 0x00600000,
            /// <summary>
            /// Align data on a 64-byte boundary. Valid only for object files.
            /// </summary>
            Align64Bytes = 0x00700000,
            /// <summary>
            /// Align data on a 128-byte boundary. Valid only for object files.
            /// </summary>
            Align128Bytes = 0x00800000,
            /// <summary>
            /// Align data on a 256-byte boundary. Valid only for object files.
            /// </summary>
            Align256Bytes = 0x00900000,
            /// <summary>
            /// Align data on a 512-byte boundary. Valid only for object files.
            /// </summary>
            Align512Bytes = 0x00A00000,
            /// <summary>
            /// Align data on a 1024-byte boundary. Valid only for object files.
            /// </summary>
            Align1024Bytes = 0x00B00000,
            /// <summary>
            /// Align data on a 2048-byte boundary. Valid only for object files.
            /// </summary>
            Align2048Bytes = 0x00C00000,
            /// <summary>
            /// Align data on a 4096-byte boundary. Valid only for object files.
            /// </summary>
            Align4096Bytes = 0x00D00000,
            /// <summary>
            /// Align data on an 8192-byte boundary. Valid only for object files.
            /// </summary>
            Align8192Bytes = 0x00E00000,
            /// <summary>
            /// The section contains extended relocations.
            /// </summary>
            LinkExtendedRelocationOverflow = 0x01000000,
            /// <summary>
            /// The section can be discarded as needed.
            /// </summary>
            MemoryDiscardable = 0x02000000,
            /// <summary>
            /// The section cannot be cached.
            /// </summary>
            MemoryNotCached = 0x04000000,
            /// <summary>
            /// The section is not pageable.
            /// </summary>
            MemoryNotPaged = 0x08000000,
            /// <summary>
            /// The section can be shared in memory.
            /// </summary>
            MemoryShared = 0x10000000,
            /// <summary>
            /// The section can be executed as code.
            /// </summary>
            MemoryExecute = 0x20000000,
            /// <summary>
            /// The section can be read.
            /// </summary>
            MemoryRead = 0x40000000,
            /// <summary>
            /// The section can be written to.
            /// </summary>
            MemoryWrite = 0x80000000
        }
    }
}
