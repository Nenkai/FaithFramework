using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;

using FF16Framework.Faith.Hooks;
using FF16Framework.Faith.Structs;
using FF16Framework.Utils;

namespace FF16Framework.ImGuiManager.Windows.Visualizers;

public unsafe class EidVisualizerComponent : IImGuiComponent
{
    private readonly IImGui _imGui;
    private readonly EntityManagerHooks _entityManager;
    private readonly MapHooks _mapHooks;
    private readonly GameContext _gameContext;
    private readonly FrameworkConfig _frameworkConfig;
    private readonly CameraHooks _cameraHooks;
    private readonly UnkList35Hooks _unkList34Hooks;
    private readonly EidHooks _eidHooks;

    public bool IsOverlay => true;

    public EidVisualizerComponent(IImGui imGui, FrameworkConfig frameworkConfig,
        GameContext gameContext,
        EntityManagerHooks entityManagerHooks, MapHooks mapHooks, CameraHooks cameraHooks, UnkList35Hooks unkList35Hooks, EidHooks eidHooks)
    {
        _imGui = imGui;
        _gameContext = gameContext;
        _entityManager = entityManagerHooks;
        _mapHooks = mapHooks;
        _frameworkConfig = frameworkConfig;
        _cameraHooks = cameraHooks;
        _unkList34Hooks = unkList35Hooks;
        _eidHooks = eidHooks;
    }

    public void Render(IImGuiShell imGuiShell)
    {
        uint currentActorId = *(uint*)(_entityManager.UnkSingletonPlayerOrCameraRelated + 0xC8);
        if (currentActorId == 0)
            return;

        IImDrawList drawList = _imGui.GetBackgroundDrawList();
        var actorMan = (ActorListBase*)_entityManager.ActorManager->Types[41];
        if (actorMan != null)
        {
            ActorEidDataEntry* actorEidData = (ActorEidDataEntry*)actorMan->vtable->GetByActorId(actorMan, currentActorId);
            if (actorEidData is not null)
            {
                FaithVector<EidMatrixInfoStruct> eids = actorEidData->Impl.List; // We don't foreach, otherwise it'll copy structs
                for (int i = 0; i < eids.Size; i++)
                {
                    ref EidMatrixInfoStruct eidElem = ref eids[i];

                    NodePositionPair posPair;
                    byte res = _eidHooks.GetPosition(actorEidData, eidElem.EidId, &posPair);
                    if (res == 1)
                    {
                        Vector3 eidWorldPos = _mapHooks.ComputeWorldPosition(&posPair);
                        _imGui.ImDrawList_AddText(drawList, _cameraHooks.WorldToScreen(eidWorldPos), ColorUtils.RGBA(0xFF, 0xFF, 0xFF, 0xFF), eidElem.EidId.ToString());
                    }
                }
            }
        }
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        throw new NotImplementedException();
    }
}
