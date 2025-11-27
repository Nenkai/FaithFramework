using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Helpers;
using Reloaded.Memory.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGui.Hooks.DirectX12;

/// <summary>
/// Represents a hook that changes a function pointer.
/// </summary>
/// <typeparam name="TFunction"></typeparam>
public class FunctionPointerHook<TFunction> : IHook<TFunction>
{
    private nuint _pointerAddress;
    private nuint _originalFunctionAddress;

    /// <summary>
    /// Hooks an individual vtable function pointer from a already hooked vtable pointer.
    /// </summary>
    /// <param name="vTableHook">The already hooked virtual function table</param>
    /// <param name="originalVtableFuncEntryAddress">The address of the original virtual function pointer
    /// This will be read to store the original function pointer</param>
    /// <param name="index">The index of the virtual function pointer in the virtual function table</param>
    /// <param name="function">The hook function</param>
    internal unsafe FunctionPointerHook(nuint originalVtableFuncEntryAddress, TFunction function)
    {
        _pointerAddress = originalVtableFuncEntryAddress;

        _originalFunctionAddress = Reloaded.Memory.Memory.Instance.Read<nuint>(originalVtableFuncEntryAddress);

        ReverseWrapper = CreateReverseWrapper(function);
        OriginalFunction = CreateWrapper(_originalFunctionAddress, out nuint originalFunctionWrapperAddress);
        OriginalFunctionAddress = _originalFunctionAddress.ToSigned();
        OriginalFunctionWrapperAddress = originalFunctionWrapperAddress.ToSigned();
        IsHookActivated = false;
    }

    /// <inheritdoc />
    public TFunction OriginalFunction { get; }

    /// <inheritdoc />
    public IReverseWrapper<TFunction> ReverseWrapper { get; }

    /// <inheritdoc />
    public bool IsHookEnabled { get; private set; }

    /// <inheritdoc />
    public bool IsHookActivated { get; private set; }

    /// <inheritdoc />
    public IntPtr OriginalFunctionAddress { get; }

    /// <inheritdoc />
    public IntPtr OriginalFunctionWrapperAddress { get; }

    /// <inheritdoc />
    protected IReverseWrapper<TFunction> CreateReverseWrapper(TFunction function)
    {
        return SDK.Hooks.CreateReverseWrapper<TFunction>(function);
    }

    /// <inheritdoc />
    protected TFunction CreateWrapper(nuint functionAddress, out nuint wrapperAddress)
    {
        var res = SDK.Hooks.CreateWrapper<TFunction>((long)functionAddress, out nint wrapperAddress_);
        wrapperAddress = (nuint)wrapperAddress_;
        return res;
    }

    /// <inheritdoc />
    IHook IHook.Activate() => Activate();

    /// <inheritdoc />
    public IHook<TFunction> Activate()
    {
        Enable();
        IsHookActivated = true;
        return this;
    }

    /// <inheritdoc />
    public void Disable()
    {
        Reloaded.Memory.Memory.Instance.SafeWrite(_pointerAddress, BitConverter.GetBytes(_originalFunctionAddress));
        IsHookEnabled = false;
    }

    /// <inheritdoc />
    public void Enable()
    {
        Reloaded.Memory.Memory.Instance.SafeWrite(_pointerAddress, BitConverter.GetBytes(ReverseWrapper.WrapperPointer.ToUnsigned()));
        IsHookEnabled = true;
    }
}

