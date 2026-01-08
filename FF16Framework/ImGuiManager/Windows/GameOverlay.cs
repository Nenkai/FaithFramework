using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Interfaces.Shell;

using FF16Framework.Faith.Hooks;
using FF16Framework.Faith.Structs;

namespace FF16Framework.ImGuiManager.Windows;

public unsafe class GameOverlay : IImGuiComponent
{
    public bool IsOverlay => true;

    private bool _open = true;

    private readonly IImGui _imGui;
    private readonly EntityManagerHooks _entityManager;
    private readonly MapHooks _mapHooks;
    private readonly GameContext _gameContext;
    private readonly FrameworkConfig _frameworkConfig;
    private readonly CameraHooks _uiControllerHooks;

    private bool hasSetPos = false;
    public GameOverlay(IImGui imGui, GameContext gameContext, EntityManagerHooks entityManagerHooks, MapHooks mapHooks, FrameworkConfig frameworkConfig,
        CameraHooks uiControllerHooks )
    {
        _imGui = imGui;
        _gameContext = gameContext;
        _entityManager = entityManagerHooks;
        _mapHooks = mapHooks;
        _frameworkConfig = frameworkConfig;
        _uiControllerHooks = uiControllerHooks;
    }

    private Vector2? PositionToRender = null;

    public void RenderMenu(IImGuiShell imGuiShell)
    {

    }

    public void Render(IImGuiShell imGuiShell)
    {
        if (!_open)
            return;

        if (!ShouldDisplay())
            return;

        float barHeight = 0;
        if (imGuiShell.IsMainMenuOpen)
            barHeight += _imGui.GetFrameHeight();

        ImGuiWindowFlags overlayWindowFlags = ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration |
            ImGuiWindowFlags.ImGuiWindowFlags_NoDocking |
            ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize |
            ImGuiWindowFlags.ImGuiWindowFlags_NoSavedSettings |
            ImGuiWindowFlags.ImGuiWindowFlags_NoFocusOnAppearing |
            ImGuiWindowFlags.ImGuiWindowFlags_NoNav | 
            ImGuiWindowFlags.ImGuiWindowFlags_NoMove | 
            ImGuiWindowFlags.ImGuiWindowFlags_NoMouseInputs;

        // If the shell is open, allow moving it around
        if (imGuiShell.IsMainMenuOpen)
        {
            overlayWindowFlags &= ~(ImGuiWindowFlags.ImGuiWindowFlags_NoMouseInputs | ImGuiWindowFlags.ImGuiWindowFlags_NoMove | ImGuiWindowFlags.ImGuiWindowFlags_NoResize);
        }

        IImGuiViewport? viewport = _imGui.GetMainViewport();

        _imGui.SetNextWindowBgAlpha(0.35f);
        _imGui.SetNextWindowPosEx(
            pos: new Vector2()
            {
                X = viewport.WorkPos.X + viewport.WorkSize.X - 10,
                Y = viewport.WorkPos.Y + barHeight + 5 /* padding */
            }, 
            cond: ImGuiCond.ImGuiCond_Once,
            pivot: new Vector2(1.0f, 0.0f)
        );


        if (_imGui.Begin("overlay"u8, ref _open, overlayWindowFlags))
        {
            RenderContents();

            Vector2 windowPos = _imGui.GetWindowPos();
            Vector2 windowSize = _imGui.GetWindowSize();

            Vector2 minPos = viewport.WorkPos;
            Vector2 maxPos = viewport.WorkPos + viewport.WorkSize - windowSize;
            maxPos = Vector2.Max(maxPos, minPos);

            Vector2 clampedPos = Vector2.Clamp(windowPos, minPos, maxPos);
            if (clampedPos != windowPos)
                _imGui.SetWindowPos(clampedPos, ImGuiCond.ImGuiCond_Always);
        }

        _imGui.End();
    }

    // TODO: Find a better way to figure out whether to render the window
    public bool ShouldDisplay()
    {
        if (_gameContext.GameType == FaithGameType.FFXVI)
        {
            if (_frameworkConfig.GameInfoOverlay.ShowCurrentActorInfo)
                return true;
        }

        return false;
    }

    private void RenderContents()
    {
        if (_gameContext.GameType == FaithGameType.FFXVI)
        {
            if (_frameworkConfig.GameInfoOverlay.ShowCurrentActorInfo)
                ShowControllingActorInfo();
            //ShowMapInfo(); // Do not use. Odd crash when loading into a map (even though it's displayed right for a brief amount of time??)
        }
    }

    private void ShowControllingActorInfo()
    {
        if (_entityManager.UnkSingletonPlayerOrCameraRelated == 0 ||
            _entityManager.StaticActorManager == 0 ||
            _entityManager.ActorManager == 0)
            return;

        _imGui.SeparatorText("Camera"u8);
        Vector3? camSrcPos = _uiControllerHooks.GetCameraSourcePos();
        Vector3? camTgtPos = _uiControllerHooks.GetCameraTargetPos();
        if (camSrcPos is not null)
        {
            _imGui.Text($"Cam Source XYZ: {camSrcPos:F2}");
            _imGui.Text($"Cam Target XYZ: {camTgtPos:F2}");
        }

        _imGui.SeparatorText("Actor"u8);
        uint currentActorId = *(uint*)(_entityManager.UnkSingletonPlayerOrCameraRelated + 0xC8);
        if (currentActorId == 0)
        {
            _imGui.Text("(No current actor)"u8);
            return;
        }

        nint* staticActorInfo = null;
        var actor = _entityManager.StaticActorManager_GetOrCreateHook.OriginalFunction(_entityManager.StaticActorManager, &staticActorInfo, currentActorId);
        if (_entityManager.HasEntityDataFunction(*staticActorInfo) != 0)
        {
            ActorReference* actorRef = _entityManager.ActorManager_GetActorByKeyFunction(_entityManager.ActorManager, currentActorId);
            _imGui.TextColored(new Vector4(1.0f, 0.7f, 0.7f, 1.0f), $"Current: {(EntityType)(actorRef->EntityID >> 24)} {actorRef->EntityID & 0xFFFFFF} (actor id {currentActorId:X})");

            NodePositionPair position;
            _entityManager.GetPositionFunction((nint)staticActorInfo, &position);

            Vector3 rotation;
            _entityManager.GetRotationFunction((nint)staticActorInfo, &rotation);

            Vector3 fwVector;
            _entityManager.GetForwardVectorFunction((nint)staticActorInfo, &fwVector);

            //Vector3 forwardXZ;
            //_entityManager.GetForwardXZFunction((nint)staticActorInfo, &forwardXZ);

            _imGui.Text($"Pos: {position.Position:F2}");
            _imGui.Text($"Rot: {rotation:F2}");
            _imGui.Text($"Forward Vec: {fwVector:F2}");
            //_imGui.Text($"Forward XZ: {forwardXZ:F2}");
        }
    }

    private void ShowMapInfo()
    {
        _imGui.Text("[Map Info]"u8);
        _imGui.Text($"Current Map Id: {_mapHooks.GameMapId & 0xFFFFFFFF}");
    }
}
