using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

public class MultiSession : AbsctractGameSession
{
    #region Portal
    public override void OnEnterPortal(PortalSetter portalSetter, Collider2D collision)
    {
        portalSetter.portalManager.isNextMap = false;
        portalSetter.portalManager.gameObject.SetActive(false);

        base.OnEnterPortal(portalSetter, collision);
        if (portalSetter.status.playersInside.Count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            if (portalSetter.isCutScene)
            {
                portalSetter.cutsceneManager.cutSceneTrigger = portalSetter.targetObj;
            }
            else if (portalSetter.isScenePortal)
            {
                portalSetter.ScenePortalManager.worldName = portalSetter.worldName;
            }
            else
            {
                portalSetter.portalManager.spawnAt = portalSetter.targetObj.transform.position;
            }
            portalSetter.SetPortalObjects(false, false, true);
        }
        else if (portalSetter.status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount && portalSetter.status.playersInside.Count > 0)
        {
            portalSetter.SetPortalObjects(false, true, false);
        }
    }
    public override void OnExitPortal(PortalSetter portalSetter, Collider2D collision)
    {
        base.OnExitPortal(portalSetter, collision);
        if (portalSetter.status.playersInside.Count == 0)
        {
            if (portalSetter.isCutScene)
            {
                portalSetter.cutsceneManager.cutSceneTrigger = null;
            }
            else if (portalSetter.isScenePortal)
            {
                portalSetter.ScenePortalManager.worldName = null;
            }

            portalSetter.SetPortalObjects(true, false, false);

            if (!portalSetter.portalManager.isNextMap)
            {
                CurrentObjectManager.Instance.uiCenterLabelOnOffManager.CloseCenterLabelWindow();
            }
            portalSetter.status = null;
        }
        else if (portalSetter.status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount && !portalSetter.portalManager.isNextMap)
        {
            CurrentObjectManager.Instance.uiCenterLabelOnOffManager.CloseCenterLabelWindow();
            portalSetter.SetPortalObjects(false, true, false);
        }
    }
    public override void MovePlayers(PortalManager portalManager)
    {
        portalManager.managerConnector.playerManager.PV.RPC("RPC_MoveTransform", RpcTarget.AllBuffered, portalManager.spawnAt);
    }
    #endregion

    #region IsNext

    public override void CheckDoneAndNext(UINextSetter uiNextSetter, string currentObjCode)
    {
        uiNextSetter.managerConnector.playerManager.PV.RPC("RPC_AddPlayerToDoneList", RpcTarget.AllBuffered, currentObjCode, PhotonNetwork.LocalPlayer.NickName);
    }

    public override void ToggleObjectActive(UINextSetter uiNextSetter, string nextObjCode, bool isDelete)
    {
        uiNextSetter.managerConnector.playerManager.PV.RPC("RPC_SetNextObj", RpcTarget.AllBuffered, nextObjCode, isDelete);
    }
    #endregion

    #region Hint
    public override void SetHintState(HintStateManager hintStateManager, string currentObjCode, string state)
    {
        hintStateManager.managerConnector.playerManager.PV.RPC("RPC_SetHintState", RpcTarget.AllBuffered, currentObjCode, state);
    }
    #endregion

    #region Player
    public override void SetPlayerValue(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            base.SetPlayerValue(playerManager);
        }
    }
    public override void MovedPlayerScene(ScenePortalManager scenePortalManager, string world)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            scenePortalManager.managerConnector.playerManager.PV.RPC("MoveNextScene", RpcTarget.AllBuffered, scenePortalManager.worldName);
        }
    }
    public override void ChangePlayerisMoved(PlayerManager playerManager, bool isMove,bool isAnim)
    {
        if (playerManager.PV.IsMine)
        {
            playerManager.ChangePlayerisMove(isMove, isAnim);
        }
    }

    public override void ChangePlayerisMovedAll(PlayerManager playerManager, bool isMove, bool isAnim)
    {
        //Debug.Log("다켜라제발");
        playerManager.PV.RPC("ChangePlayerisMove", RpcTarget.AllBuffered, isMove, isAnim);
    }
    public override void MoveBasic(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            base.MoveBasic(playerManager);
        }
    }
    public override void JoystickMoveBasic(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            base.JoystickMoveBasic(playerManager);
        }
    }
    public override void AnimControllerBasic(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            base.AnimControllerBasic(playerManager);
        }
    }
    public override void TriggerEnterBasic(PlayerManager playerManager, Collider2D collision)
    {
        if (playerManager.PV.IsMine)
        {
            base.TriggerEnterBasic(playerManager, collision);
        }
    }
    public override void TriggerExitBasic(PlayerManager playerManager, Collider2D collision)
    {
        if (playerManager.PV.IsMine)
        {
            base.TriggerExitBasic(playerManager, collision);
        }
    }

    public override void CutSceneEnter(PlayerManager playerManager, bool isCutScene)
    {
        playerManager.PV.RPC("CutSceneUseAble", RpcTarget.AllBuffered, isCutScene);
    }

    public override void CutScenePlayerValue(PlayerManager playerManager, bool isCutScene)
    {
        playerManager.PV.RPC("CutScenePlayerSetValue", RpcTarget.AllBuffered, isCutScene);
    }
    #endregion

    #region Interaction
    public override void HandleInteractionBasic(CurrentObjectManager currentObjectManager, string currentObjCode)
    {
        ObjDataType currentRow = currentObjectManager.objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == currentObjCode);

        if (currentRow == null) return;
        bool currentIsMine = currentRow.isMine;

        if (!currentIsMine)
        {
            currentObjectManager.managerConnector.playerManager.PV.RPC("RPC_ShowIsMineData", RpcTarget.OthersBuffered, currentObjCode);
        }
        base.HandleInteractionBasic(currentObjectManager, currentObjCode);
    }

    public override void SortingLayerIsCollision(DynamicSortingLayer layer, bool isCol)
    {
        if (layer.managerConnector.playerManager.PV.IsMine)
        {
            base.SortingLayerIsCollision(layer, isCol);
        }
    }
    public override void SortingLayerName(DynamicSortingLayer layer)
    {
        if(layer.managerConnector.playerManager.PV.IsMine)
        {
            base.SortingLayerName(layer);
        }
    }

    public override void OnOffPrefabsPopUp(OnOffPrefabs onOffPrefabs, string prefabCode)
    {
            base.OnOffPrefabsPopUp(onOffPrefabs, prefabCode);
    }
    #endregion

    #region UI On Off
    public override void OpenPopUpBasic(UIPopUpOnOffManager uiPopUpOnOffManager, bool isQuest, bool isDial)
    {
        base.OpenPopUpBasic(uiPopUpOnOffManager, isQuest, isDial);
    }
    public override void ClosePopUpBasic(UIPopUpOnOffManager UIPopUpOnOffManager)
    {
        base.ClosePopUpBasic(UIPopUpOnOffManager);
    }
    public override void OpenCenterLabelBasic(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        base.OpenCenterLabelBasic(uiCenterLabelOnOffManager);
    }
    public override void CloseCenterLabelBasic(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        base.CloseCenterLabelBasic(uiCenterLabelOnOffManager);
    }

    public override void OnOffPlayerBtnGroup(ManagerConnector managerConnector, bool isActive)
    {
        if (managerConnector.playerManager.PV.IsMine)
        {
            base.OnOffPlayerBtnGroup(managerConnector, isActive);
        }
    }

    public override void CheckIsMobile(PlayerInstantiateManager player, ManagerConnector managerConnector)
    {
        base.CheckIsMobile(player, managerConnector);
    }
    #endregion

    #region Camera
    public override void SetCamera(CamDontDes camera, GameObject playerObj)
    {
        base.SetCamera(camera, playerObj);
    }

    public override void SetBoundLens(SetBound setBound)
    {
        base.SetBoundLens(setBound);
    }

    public override void SetCamValue(CamDontDes camDontDes, Collider2D newBound, float lens)
    {
        base.SetCamValue(camDontDes, newBound, lens);
    }
    #endregion

    #region UINextSetter
    public override void SetValueUINextSetter(UINextSetter uINextSetter)
    {
        if (uINextSetter.managerConnector.playerManager != null && uINextSetter.managerConnector.playerManager.PV.IsMine && !uINextSetter.isTest)
        {
           base.SetValueUINextSetter(uINextSetter);
        }
    }

    public override void ClearPlayerisDone(UINextSetter uINextSetter)
    {
        uINextSetter.managerConnector.playerManager.PV.RPC("ClearPlayerisDone", RpcTarget.AllBuffered, uINextSetter.curObjCode);
    }
    #endregion

    #region CutScene

    #endregion
}