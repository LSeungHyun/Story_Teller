using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using static PortalSetter;

public class MultiSession : AbsctractGameSession
{
    #region Portal
    public override void OnEnterPortal(PortalSetter portalSetter, Collider2D collision)
    {
        base.OnEnterPortal(portalSetter, collision);
        if (portalSetter.status.playersInside.Count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            portalSetter.portalManager.targetObj = portalSetter.targetPosition;
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
            portalSetter.portalManager.targetObj = null;
            portalSetter.SetPortalObjects(true, false, false);
            portalSetter.portalStatuses.Remove(portalSetter.managerConnector);
        }
        else if (portalSetter.status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            portalSetter.SetPortalObjects(false, true, false);
        }
    }
    #endregion

    #region Player
    public override void MoveBasic(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            base.MoveBasic(playerManager);
        }
    }
    public override void AnimControllerBasic(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            base.AnimControllerBasic(playerManager);
        }
    }
    public override void TriggerEnterBasic(PlayerManager playerManager,Collider2D collision)
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
    #endregion

    #region Interaction
    public override void HandleInteractionBasic(CurrentObjectManager currentObjectManager)
    {
        string currentObjCode = currentObjectManager.currentRow.objCode;
        bool currentIsMine = currentObjectManager.currentRow.isMine;
        if (!currentIsMine)
        {
            currentObjectManager.managerConnector.playerManager.PV.RPC("RPC_ShowIsMineData", RpcTarget.AllBuffered, currentObjCode);
        }
        base.HandleInteractionBasic(currentObjectManager);
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
    #endregion
}