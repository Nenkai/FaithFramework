using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Faith.Structs;

namespace FF16Framework.Services.ResourceManager;

public unsafe class ResourceHandle
{
    internal ResourceHandleStruct* NativeStruct;
    public bool IsValid { get; internal set; } = false;
    public bool IsReplacing { get; set; }

    public nint HandleAddress => (nint)NativeStruct;

    public int FormatLoadState => NativeStruct->FormatLoadState;
    public int OpenState => NativeStruct->OpenState;

    public nint FlagsRaw => NativeStruct->Flags;
    public nint BufferAddress { get => NativeStruct->FileBuffer; set => NativeStruct->FileBuffer = value; }
    public long FileSize { get => NativeStruct->FileSize; set => NativeStruct->FileSize = (uint)value; }

    private nint OriginalBufferAddress 
    { 
        get => IsReplacing ? _originalBufferAddress : BufferAddress; 
    }

    private long OriginalFileSize
    {
        get => IsReplacing ? _originalFileSize : FileSize;
    }

    private nint _originalBufferAddress;
    private long _originalFileSize;

    public nint FileNamePointer => (nint)NativeStruct->FileName;
    public ReadOnlySpan<byte> FileNameSpan => MemoryMarshal.CreateReadOnlySpanFromNullTerminated(NativeStruct->FileName);

    public ResourceHandle(ResourceHandleStruct* @struct)
    {
        NativeStruct = @struct;
        _originalBufferAddress = BufferAddress;
        _originalFileSize = FileSize;

    }

    public void ReplaceBuffer(ReadOnlySpan<byte> buffer)
    {
        if (!IsReplacing)
        {
            _originalBufferAddress = BufferAddress;
            _originalFileSize = FileSize;
        }
        else
        {
            // Free current buffer
            Marshal.FreeHGlobal(BufferAddress);
        }

        nint newBuffer = Marshal.AllocHGlobal(buffer.Length);
        IsReplacing = true;

        BufferAddress = newBuffer;
        FileSize = buffer.Length;

        fixed (byte* source = buffer)
            NativeMemory.Copy(source, (void*)newBuffer, (nuint)buffer.Length);
    }

    public void Restore()
    {
        if (IsReplacing)
        {
            Marshal.FreeHGlobal(BufferAddress);

            BufferAddress = _originalBufferAddress;
            FileSize = _originalFileSize;
        }

        IsReplacing = false;
    }

    public bool IsLoaded()
    {
        return FormatLoadState == 1;
    }
}
