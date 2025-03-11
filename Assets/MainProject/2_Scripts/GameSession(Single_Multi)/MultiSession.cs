using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiSession : AbsctractGameSession
{
    // �� ��Ż(PortalMananager)���� ���� ��Ż �ȿ� ���� �÷��̾���� �����ϴ� Ŭ����
    private class PortalStatus
    {
        // �÷��̾� ���� ID�� ���� (collision.gameObject.GetInstanceID() ���)
        public HashSet<int> playersInside = new HashSet<int>();
        // �ش� ��Ż�� ī��Ʈ�ٿ� �ڷ�ƾ ����
        public Coroutine countdownCoroutine = null;
    }

    // �� PortalMananager�� ���¸� �����ϴ� ��ųʸ�
    private Dictionary<PortalMananager, PortalStatus> portalStatuses = new Dictionary<PortalMananager, PortalStatus>();

    [PunRPC]
    public override void ShowPortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        // ��Ƽ ��� Ưȭ ���� �߰� ���� (��Ʈ��ũ ����ȭ, UI ������Ʈ ��)
        base.ShowPortalCenterLabel(portalMananager, collision);
    }
    [PunRPC]
    public override void ClosePortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        base.ClosePortalCenterLabel(portalMananager, collision);
    }

    // ��Ż ���� �÷��̾ ���� �� ȣ��Ǵ� �޼���
    public override void StartPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        PhotonView PV = portal.portalContainer.playerManager.gameObject.GetComponent<PhotonView>();

        if (!portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            status = new PortalStatus();
            portalStatuses.Add(portal, status);
        }

        int playerID = collision.gameObject.GetInstanceID();
        status.playersInside.Add(playerID);

        int requiredPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // �� ������ ��ΰ� ��Ż�� ������ �ʾҴٸ� "Enter_All" ���·� ���Ͷ� ǥ��
        if (status.playersInside.Count < requiredPlayerCount)
        {
            portal.objCode = "Enter_All";
            PV.RPC("ShowPortalCenterLabel", RpcTarget.AllBuffered, portal, collision);
        }
        // ���� ��� �÷��̾ ��Ż�� ���� ��� "Enter_Wait3" ���·� ���Ͷ� ǥ�� �� ī��Ʈ�ٿ� ����
        else if (status.playersInside.Count == requiredPlayerCount)
        {
            portal.objCode = "Enter_Wait3";
            PV.RPC("ShowPortalCenterLabel", RpcTarget.AllBuffered, portal, collision);

            if (status.countdownCoroutine == null)
            {
                status.countdownCoroutine = portal.StartCoroutine(PortalCountdownCoroutine(portal));
            }
        }
    }

    // ��Ż���� �÷��̾ ���� �� ȣ�� (RPC�� ���� ���Ͷ� ����)
    public override void StopPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            int playerID = collision.gameObject.GetInstanceID();
            status.playersInside.Remove(playerID);

            PhotonView PV = portal.portalContainer.playerManager.gameObject.GetComponent<PhotonView>();

            // ��Ż ���� �ƹ��� ������ ���Ͷ� ���� RPC ȣ��
            if (status.playersInside.Count == 0)
            {
                if (status.countdownCoroutine != null)
                {
                    portal.StopCoroutine(status.countdownCoroutine);
                    status.countdownCoroutine = null;
                }
                PV.RPC("ClosePortalCenterLabel", RpcTarget.AllBuffered, portal, collision);
                portalStatuses.Remove(portal);
            }
            // ��� ������ ���� ���¶�� "Enter_All" ���·� ������Ʈ
            else if (status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                if (status.countdownCoroutine != null)
                {
                    portal.StopCoroutine(status.countdownCoroutine);
                    status.countdownCoroutine = null;
                }
                portal.objCode = "Enter_All";
                PV.RPC("ShowPortalCenterLabel", RpcTarget.AllBuffered, portal, collision);
            }
        }
    }

    // ��Ż�� ��� �÷��̾ ���� ���� �� ����Ǵ� ī��Ʈ�ٿ� �ڷ�ƾ
    private IEnumerator PortalCountdownCoroutine(PortalMananager portal)
    {
        float timer = 0f;
        while (timer < portal.countTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        // ī��Ʈ�ٿ� �Ϸ� ��, �ڷ�ƾ ���� �ʱ�ȭ
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            status.countdownCoroutine = null;
        }
        // 1�� ��� �� �̵� ó�� ����
        yield return portal.StartCoroutine(LoadMapCoroutine(portal));
    }

    // �̵� ó�� �ڷ�ƾ (1�� ��� �� ���� �̵�)
    private IEnumerator LoadMapCoroutine(PortalMananager portal)
    {
        // �÷��̾� �Ŵ����� ������ PhotonView�� ������
        PhotonView photonView = portal.portalContainer.playerManager.gameObject.GetComponent<PhotonView>();
        portal.isAreadyMove = true;
        Debug.Log("��Ƽ ���: �̵� ����!");
        yield return new WaitForSeconds(1f);

        
        if (photonView != null)
        {
            // ��� Ŭ���̾�Ʈ���� �̵��� ����ȭ�ϱ� ���� nextMap.position�� Vector3�� ����
            photonView.RPC("MoveTransform", RpcTarget.AllBuffered, portal.nextMap.position);
        }
        else
        {
            Debug.LogError("PhotonView�� playerManager ��ü�� �������� �ʽ��ϴ�.");
        }

        portal.isAreadyMove = false;
        if (portalStatuses.ContainsKey(portal))
        {
            portalStatuses.Remove(portal);
        }
    }

    [PunRPC]
    public void MoveTransform(PortalMananager portal)
    {
        portal.portalContainer.playerManager.gameObject.transform.position = portal.nextMap.position;
    }
    #region Player Abstract Methods;
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