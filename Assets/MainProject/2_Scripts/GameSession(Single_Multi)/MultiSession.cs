using UnityEngine;
using Photon.Pun;
using static UINextSetter;
using System.Linq;

public class MultiSession : AbsctractGameSession
{
    #region Portal
    public override void OnEnterPortal(PortalSetter portalSetter, Collider2D collision)
    {
        portalSetter.portalManager.isNextMap = false;

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
            else
            {
                portalSetter.portalManager.spawnAt = Vector3.zero;
            }
            portalSetter.SetPortalObjects(true, false, false);
            portalSetter.portalManager.gameObject.SetActive(false);
            if (!portalSetter.portalManager.isNextMap)
            {
                CurrentObjectManager.Instance.uiCenterLabelOnOffManager.CloseCenterLabelWindow();
                portalSetter.portalManager.gameObject.SetActive(false);
            }
            portalSetter.status = null;
        }
        else if (portalSetter.status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            portalSetter.SetPortalObjects(false, true, false);
        }
    }
    public override void MovePlayers(PortalManager portalManager)
    {
        Debug.Log("멀티로 옮기기!");
        base.MovePlayers(portalManager);

        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            portalManager.managerConnector.playerManager.PV.RPC("RPC_MoveTransform", RpcTarget.AllBuffered, portalManager.spawnAt);
        }
            
    }
    #endregion

    #region IsNext
    public override void CheckDoneAndNext(UINextSetter uiNextSetter, string currentObjCode)
    {
        bool isdone = uiNextSetter.CheckEveryoneIsDone(currentObjCode);
        if (isdone)
        {
            uiNextSetter.ProcessNextCode(currentObjCode);
        }
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
    public override void ChangePlayerisMoved(PlayerManager playerManager, bool isMove,bool isAnim)
    {
        if (playerManager.PV.IsMine)
        {
            playerManager.ChangePlayerisMove(isMove, isAnim);
        }
    }

    public override void ChangePlayerisMovedAll(PlayerManager playerManager, bool isMove, bool isAnim)
    {
        Debug.Log("다켜라제발");
        playerManager.PV.RPC("ChangePlayerisMove", RpcTarget.AllBuffered, isMove, isAnim);
    }
    public override void PlayerMovementControl(PlayerManager playerManager, bool isMove)
    {
        if (playerManager.PV.IsMine)
        {
            base.PlayerMovementControl(playerManager, isMove);
        }
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

    public override void CutSceneEnter(PlayerManager playerManager, Collider2D collision)
    {
        if (playerManager.PV.IsMine)
        {
            base.CutSceneEnter(playerManager, collision);
        }
    }
    #endregion

    #region Interaction
    public override void HandleInteractionBasic(CurrentObjectManager currentObjectManager, string currentObjCode)
    {
        ObjDataType currentRow = currentObjectManager.objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == currentObjCode);
        bool currentIsMine = currentRow.isMine;
        if (!currentIsMine)
        {
            currentObjectManager.managerConnector.playerManager.PV.RPC("RPC_ShowIsMineData", RpcTarget.AllBuffered, currentObjCode);
        }
        base.HandleInteractionBasic(currentObjectManager, currentObjCode);
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
        base.OnOffPlayerBtnGroup(managerConnector, isActive);
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
}