using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiSession : AbsctractGameSession
{

/*    private IEnumerator PortalCountdownCoroutine(PortalMananager portal)
    {
        float timer = 0f;
        while (timer < portal.countTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // ī��Ʈ�ٿ� �Ϸ�
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            status.countdownCoroutine = null;
        }
        yield return portal.StartCoroutine(LoadMapCoroutine(portal));
    }

    // 1�� ��� �� �� �̵�
    private IEnumerator LoadMapCoroutine(PortalMananager portal)
    {
        portal.isAreadyMove = true;
        Debug.Log("��Ƽ ���: �̵� ����!");
        yield return new WaitForSeconds(1f);

        // ��ġ ����ȭ
        PhotonView photonView = portal.portalContainer.playerManager.PV;
        if (photonView != null)
        {
            photonView.RPC("MoveTransform", RpcTarget.AllBuffered, portal.nextMap.position);
        }

        portal.isAreadyMove = false;
        if (portalStatuses.ContainsKey(portal))
        {
            portalStatuses.Remove(portal);
        }
    }*/

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