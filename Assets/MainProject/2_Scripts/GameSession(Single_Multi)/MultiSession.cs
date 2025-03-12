using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiSession : AbsctractGameSession
{
    // ��Ż�� ���� (��Ż�� ���� �÷��̾� ���, ī��Ʈ�ٿ� �ڷ�ƾ)
    private class PortalStatus
    {
        public HashSet<int> playersInside = new HashSet<int>();
        public Coroutine countdownCoroutine = null;
    }

    // �� ��Ż���� PortalStatus�� ����
    private Dictionary<PortalMananager, PortalStatus> portalStatuses = new Dictionary<PortalMananager, PortalStatus>();

    // ��Ż ���� �÷��̾ ���� ��
    public override void StartPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        PhotonView PV = portal.portalContainer.playerManager.PV;
        // PortalStatus �ʱ�ȭ
        if (!portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            status = new PortalStatus();
            portalStatuses.Add(portal, status);
        }

        // �浹ü(�÷��̾�)�� ���� ID�� ����
        int playerID = collision.gameObject.GetInstanceID();
        status.playersInside.Add(playerID);

        // ���� ���� �÷��̾� ��
        int requiredPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // ���� ������ ������ ����
        if (status.playersInside.Count < requiredPlayerCount)
        {
            portal.objCode = "Enter_All";
            PV.RPC("ChangeObjCode",RpcTarget.AllBuffered,portal.objCode);
            PV.RPC("RPC_ShowPortalLabel", RpcTarget.AllBuffered, portal.objCode);
        }
        // ������ ����
        else if (status.playersInside.Count == requiredPlayerCount)
        {
            portal.objCode = "Enter_Wait3";
            PV.RPC("ChangeObjCode", RpcTarget.AllBuffered, portal.objCode);
            PV.RPC("RPC_ShowPortalLabel", RpcTarget.AllBuffered, portal.objCode);

            // ī��Ʈ�ٿ� �ڷ�ƾ�� ������ ����
            if (status.countdownCoroutine == null)
            {
                status.countdownCoroutine = portal.StartCoroutine(PortalCountdownCoroutine(portal));
            }
        }
    }

    //------------------------------------------------------------------------
    // 2) ��Ż���� �÷��̾ ������ �� "ī��Ʈ�ٿ" �ߴ�
    //------------------------------------------------------------------------
    public override void StopPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            // ���� �÷��̾� ����
            int playerID = collision.gameObject.GetInstanceID();
            status.playersInside.Remove(playerID);

            // ���� ���� �ڷ�ƾ�� ������ �ߴ�
            if (status.countdownCoroutine != null)
            {
                portal.StopCoroutine(status.countdownCoroutine);
                status.countdownCoroutine = null;
            }
            // ���⼭�� '��' ���� ������ �������� ���� (��� �и�)
            // �� ó���� ClosePortalCenterLabel(...)���� ���
        }
    }

    //------------------------------------------------------------------------
    // 3) "���� ��"�� ������ ���ų�, 'Enter_All'�� ����
    //------------------------------------------------------------------------
    public override void ClosePortalCenterLabel(PortalMananager portal)
    {
        PhotonView PV = portal.portalContainer.playerManager.PV;

        // ���� portalStatuses�� ������ ���� �ִ��� Ȯ��
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            // �Ϻθ� ���Ҵٸ� 'Enter_All' ���·� ����
            if (status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                portal.objCode = "Enter_All";
                PV.RPC("ChangeObjCode", RpcTarget.AllBuffered, portal.objCode);
                PV.RPC("RPC_ShowPortalLabel", RpcTarget.AllBuffered, portal.objCode);

                Debug.Log("�ٸ������ ���Ͷ� ���ֱ�" + portal.objCode);
            }
            // �ƹ��� �� ���Ҵٸ� �� �ݰ�, ��ųʸ� ����
            else if (status.playersInside.Count == 0)
            {
                PV.RPC("RPC_ClosePortalLabel", RpcTarget.AllBuffered);
                portalStatuses.Remove(portal);
            }
        }
        //else
        //{
        //    // Ȥ�� portalStatuses�� ���ٸ� �̹� ���� ������ �����̹Ƿ�
        //    PV.RPC("RPC_ClosePortalLabel", RpcTarget.AllBuffered);
        //}
    }

    //------------------------------------------------------------------------
    // 4) ��� �÷��̾ ������ �� ī��Ʈ�ٿ� �� �� �̵�
    //------------------------------------------------------------------------
    private IEnumerator PortalCountdownCoroutine(PortalMananager portal)
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
    }

    //------------------------------------------------------------------------
    // (�̱�/��Ƽ ����) �÷��̾� �̵� ���� ��
    //------------------------------------------------------------------------
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

    public override void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode)
    {
        ClosePopUpBasic(UIPopUpOnOffManager, currentObjCode);

        Debug.Log("��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ.");
    }
    public override void OpenPopUp(UIPopUpOnOffManager uiPopUpOnOffManager, bool isQuest, bool isDial)
    {
        OpenPopUpBasic(uiPopUpOnOffManager, isQuest, isDial);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }
    public override void HandleInteraction(CurrentObjectManager currentObjectManager)
    {
        HandleInteractionBasic(currentObjectManager);
        Debug.Log("���߳� ��Ƽ��");
    }
    public override void OpenCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        OpenCenterLabelBasic(uiCenterLabelOnOffManager);
    }
    public override void CloseCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        CloseCenterLabelBasic(uiCenterLabelOnOffManager);
    }
}