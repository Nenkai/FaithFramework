using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Abstractions;
using FF16Framework.Faith.Hooks;

namespace FF16Framework.ImGuiManager.Windows;

public unsafe class GameOverlay : IImGuiComponent
{
    public bool IsOverlay => true;

    private bool _open = true;

    private readonly IImGui _imGui;
    private readonly EntityManagerHooks _entityManager;

    public GameOverlay(IImGui imGui, EntityManagerHooks entityManagerHooks)
    {
        _imGui = imGui;
        _entityManager = entityManagerHooks;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        _imGui.MenuItemBoolPtr("Enable Overlay"u8, ""u8, ref _open, true);
    }

    public void Render(IImGuiShell imGuiShell)
    {
        if (!_open)
            return;

        float barHeight = 0;
        if (imGuiShell.IsMainMenuOpen)
            barHeight += _imGui.GetFrameHeight();

        _imGui.SetNextWindowBgAlpha(0.35f);
        if (_imGui.Begin("overlay"u8, ref _open, ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration |
            ImGuiWindowFlags.ImGuiWindowFlags_NoDocking |
            ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize |
            ImGuiWindowFlags.ImGuiWindowFlags_NoSavedSettings |
            ImGuiWindowFlags.ImGuiWindowFlags_NoFocusOnAppearing |
            ImGuiWindowFlags.ImGuiWindowFlags_NoNav))
        {
            // TODO: Find the current controlling actor id.
            // TODO: Find camera.
            if (_entityManager.CliveActorId != 0 && _entityManager.ManagerPointer != 0)
            {
                nint* staticActorInfo = null;
                var clive = _entityManager.StaticActorManager_GetOrCreateHook.OriginalFunction(_entityManager.ManagerPointer, &staticActorInfo, _entityManager.CliveActorId);
                if (_entityManager.HasEntityDataFunction(*staticActorInfo) != 0)
                {
                    NodePositionPair position;
                    _entityManager.GetPositionFunction((nint)staticActorInfo, &position);

                    Vector3 rotation;
                    _entityManager.GetRotationFunction((nint)staticActorInfo, &rotation);

                    Vector3 fwVector;
                    _entityManager.GetForwardVectorFunction((nint)staticActorInfo, &fwVector);

                    Vector3 forwardXZ;
                    _entityManager.GetForwardXZFunction((nint)staticActorInfo, &forwardXZ);

                    _imGui.Text($"Pos: {position.Position:F2}");
                    _imGui.Text($"Rot: {rotation:F2}");
                    _imGui.Text($"Forward Vec: {fwVector:F2}");
                    _imGui.Text($"Forward XZ: {forwardXZ:F2}");
                }
            }
            
            _imGui.SetWindowPos(new Vector2()
            {
                X = _imGui.GetIO().DisplaySize.X - _imGui.GetWindowWidth() - 10,
                Y = barHeight + 5 /* padding */
            }, ImGuiCond.ImGuiCond_Always);
        }

        _imGui.End();
    }
}
