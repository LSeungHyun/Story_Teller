using UnityEngine;
using Photon.Pun;
using static UIQuestSetter;

public class MultiSession : AbsctractGameSession
{
    #region Portal
    public override void OnEnterPortal(PortalSetter portalSetter, Collider2D collision)
    {
        base.OnEnterPortal(portalSetter, collision);
        if (portalSetter.status.playersInside.Count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            if (portalSetter.isCutScene)
            {
                portalSetter.cutsceneManager.cutSceneTrigger = portalSetter.targetObj;
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
            else
            {
                portalSetter.portalManager.spawnAt = Vector3.zero;
            }
            portalSetter.SetPortalObjects(true, false, false);
            portalSetter.status = null;
        }
        else if (portalSetter.status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            portalSetter.SetPortalObjects(false, true, false);
        }
    }
    public override void MovePlayers(PortalManager portalManager)
    {
        portalManager.managerConnector.playerManager.PV.RPC("RPC_MoveTransform", RpcTarget.AllBuffered, portalManager.spawnAt);
        base.MovePlayers(portalManager);
    }
    #endregion

    #region IsNext
    public override void OnEnterAnswer(UIQuestSetter uiQuestSetter)
    {
        base.OnEnterAnswer(uiQuestSetter);

        if (uiQuestSetter.status == null)
        {
            uiQuestSetter.status = new QuestStatus();
        }

        int playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        if (!uiQuestSetter.status.playersIsDone.Contains(playerID))
        {
            uiQuestSetter.managerConnector.playerManager.PV.RPC("RPC_AddPlayerToDoneList", RpcTarget.AllBuffered, playerID);
        }

        if (uiQuestSetter.status.playersIsDone.Count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            uiQuestSetter.uiPopUpOnOffManager.CloseAndCheckPopUpWindow();
        }
        else
        {
            uiQuestSetter.uiPopUpOnOffManager.ClosePopUpWindow();
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