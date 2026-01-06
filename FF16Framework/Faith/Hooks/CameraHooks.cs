using System;
using System.Collections;
using System.Collections.Concurrent.Extended;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

using FF16Framework.Faith.Structs;

namespace FF16Framework.Faith.Hooks;

public unsafe class CameraHooks : HookGroupBase
{
    // TODO: Don't pass by UIAddonOrControllerManager. We should use faith::Graphics::Object::Camera if possible.
    // These don't exist in FFT:IVC.
    public delegate nint UIAddonOrControllerManager_Ctor(UIAddonOrControllerManager* @this);
    public IHook<UIAddonOrControllerManager_Ctor> UIControllerManagerCtor_Hook { get; private set; }

    public delegate nint faith_UI_ProjectWorldToScreen(Vector3* worldPos, Matrix4x4* viewProj, Size* viewportSize, Vector2* outScreenPos);
    public faith_UI_ProjectWorldToScreen ProjectWorldToScreenFunction { get; private set; }

    public UIAddonOrControllerManager* UIAddonOrControllerManagerPtr { get; private set; }

    // Camera things
    // TODO: This also doesn't really seem to exist in FFT:IVC? Should use the main faith::Graphics::Object::Camera directly.
    public delegate nint CameraManager_Ctor(nint @this);
    public IHook<CameraManager_Ctor> CameraManagerHook_CtorHook { get; private set; }

    public delegate CameraManagerEntry* CameraManager_GetCamera(nint @this, int index);
    public CameraManager_GetCamera CameraManager_GetCameraFunction { get; private set; }

    public nint CameraManagerPtr { get; private set; }

    public CameraHooks(Config config, IModConfig modConfig, ILogger logger)
        : base(config, modConfig, logger)
    {
    }

    public override void SetupHooks()
    {
        Project.Scans.AddScanHook(nameof(UIAddonOrControllerManager_Ctor), (result, hooks)
            => UIControllerManagerCtor_Hook = hooks.CreateHook<UIAddonOrControllerManager_Ctor>(UIAddonOrControllerManager_CtorImpl, result).Activate());
        Project.Scans.AddScanHook(nameof(faith_UI_ProjectWorldToScreen), (result, hooks)
            => ProjectWorldToScreenFunction = hooks.CreateWrapper<faith_UI_ProjectWorldToScreen>(result, out _));

        Project.Scans.AddScanHook(nameof(CameraManager_Ctor), (result, hooks)
            => CameraManagerHook_CtorHook = hooks.CreateHook<CameraManager_Ctor>(CameraManager_CtorImpl, result).Activate());
        Project.Scans.AddScanHook(nameof(CameraManager_GetCamera), (result, hooks)
            => CameraManager_GetCameraFunction = hooks.CreateWrapper<CameraManager_GetCamera>(result, out _));

    }

    private nint UIAddonOrControllerManager_CtorImpl(UIAddonOrControllerManager* @this)
    {
        UIAddonOrControllerManagerPtr = @this;
        var res = UIControllerManagerCtor_Hook.OriginalFunction(@this);
        UIControllerManagerCtor_Hook.Disable();

        return res;
    }

    private nint CameraManager_CtorImpl(nint @this)
    {
        CameraManagerPtr = @this;
        var res = CameraManagerHook_CtorHook.OriginalFunction(@this);
        CameraManagerHook_CtorHook.Disable();

        return res;
    }

    // Public stuff.
    public Vector2 WorldToScreen(Vector3 position)
    {
        var tempOut = new Vector2();
        ProjectWorldToScreenFunction(&position, &UIAddonOrControllerManagerPtr->ViewProjMatrix, &UIAddonOrControllerManagerPtr->ViewportSize, &tempOut);
        return tempOut;
    }

    public Vector3? GetCameraSourcePos()
    {
        if (CameraManagerPtr == nint.Zero)
            return null;

        var camera = CameraManager_GetCameraFunction(CameraManagerPtr, 0);
        if (camera is null)
            return null;

        if (camera->Source.ParentNode is null)
            return null;

        return camera->Source.Position;
    }

    public Vector3? GetCameraTargetPos()
    {
        if (CameraManagerPtr == nint.Zero)
            return null;

        var camera = CameraManager_GetCameraFunction(CameraManagerPtr, 0);
        if (camera is null)
            return null;

        if (camera->Source.ParentNode is null)
            return null;

        return camera->TargetLookAt.Position;
    }
}