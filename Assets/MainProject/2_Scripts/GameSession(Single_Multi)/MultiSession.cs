using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiSession : AbsctractGameSession
{
    // 포탈별 상태 (포탈에 들어온 플레이어 목록, 카운트다운 코루틴)
    private class PortalStatus
    {
        public HashSet<int> playersInside = new HashSet<int>();
        public Coroutine countdownCoroutine = null;
    }

    // 각 포탈마다 PortalStatus를 관리
    private Dictionary<PortalMananager, PortalStatus> portalStatuses = new Dictionary<PortalMananager, PortalStatus>();

    // 포탈 내에 플레이어가 들어올 때
    public override void StartPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        // 플레이어가 붙어있는 PlayerManager의 PhotonView를 가져옴
        PhotonView PV = portal.portalContainer.playerManager.PV;

        // PortalStatus 초기화
        if (!portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            status = new PortalStatus();
            portalStatuses.Add(portal, status);
        }

        // 충돌체(플레이어)의 고유 ID를 저장
        int playerID = collision.gameObject.GetInstanceID();
        status.playersInside.Add(playerID);

        // 현재 방의 플레이어 수
        int requiredPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // 아직 전원이 들어오지 않음
        if (status.playersInside.Count < requiredPlayerCount)
        {
            // 포탈 라벨 상태를 "Enter_All"로 지정
            portal.objCode = "Enter_All";

            // PlayerManager의 RPC로, 모든 클라이언트에서 라벨 표시
            PV.RPC("RPC_ShowPortalLabel", RpcTarget.AllBuffered, portal.objCode);
        }
        // 모든 플레이어가 들어옴
        else if (status.playersInside.Count == requiredPlayerCount)
        {
            portal.objCode = "Enter_Wait3";
            PV.RPC("RPC_ShowPortalLabel", RpcTarget.AllBuffered, portal.objCode);

            // 카운트다운 코루틴이 없으면 시작
            if (status.countdownCoroutine == null)
            {
                status.countdownCoroutine = portal.StartCoroutine(PortalCountdownCoroutine(portal));
            }
        }
    }

    // 포탈 내에서 플레이어가 나갈 때
    public override void StopPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            int playerID = collision.gameObject.GetInstanceID();
            status.playersInside.Remove(playerID);

            PhotonView PV = portal.portalContainer.playerManager.PV;

            // 포탈에 아무도 남아있지 않으면
            if (status.playersInside.Count == 0)
            {
                if (status.countdownCoroutine != null)
                {
                    portal.StopCoroutine(status.countdownCoroutine);
                    status.countdownCoroutine = null;
                }
                // 전부 빠져나갔으니 라벨 닫기
                PV.RPC("RPC_ClosePortalLabel", RpcTarget.AllBuffered);
                portalStatuses.Remove(portal);
            }
            // 일부만 남아있다면
            else if (status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                if (status.countdownCoroutine != null)
                {
                    portal.StopCoroutine(status.countdownCoroutine);
                    status.countdownCoroutine = null;
                }
                portal.objCode = "Enter_All";
                PV.RPC("RPC_ShowPortalLabel", RpcTarget.AllBuffered, portal.objCode);
            }
        }
    }

    // 모든 플레이어가 들어왔을 때 카운트다운 후 맵 이동
    private IEnumerator PortalCountdownCoroutine(PortalMananager portal)
    {
        float timer = 0f;
        while (timer < portal.countTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            status.countdownCoroutine = null;
        }
        yield return portal.StartCoroutine(LoadMapCoroutine(portal));
    }

    // 1초 대기 후 맵 이동
    private IEnumerator LoadMapCoroutine(PortalMananager portal)
    {
        portal.isAreadyMove = true;
        Debug.Log("멀티 모드: 이동 실행!");
        yield return new WaitForSeconds(1f);

        // 위치 동기화
        PhotonView photonView = portal.portalContainer.playerManager.PV;
        if (photonView != null)
        {
            // Vector3만 전달하면 직렬화 가능
            photonView.RPC("MoveTransform", RpcTarget.AllBuffered, portal.nextMap.position);
        }

        portal.isAreadyMove = false;
        if (portalStatuses.ContainsKey(portal))
        {
            portalStatuses.Remove(portal);
        }
    }

    //-------------------------------------------------------------------------
    // 예시: 플레이어 이동 로직 (싱글/멀티 공통)
    //-------------------------------------------------------------------------
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

        Debug.Log("멀티멀티멀티멀티멀티멀티멀티멀티멀티멀티멀티멀티.");
    }
    public override void OpenPopUp(UIPopUpOnOffManager uiPopUpOnOffManager, bool isQuest, bool isDial)
    {
        OpenPopUpBasic(uiPopUpOnOffManager, isQuest, isDial);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }
    public override void HandleInteraction(CurrentObjectManager currentObjectManager)
    {
        HandleInteractionBasic(currentObjectManager);
        Debug.Log("나야나 멀티야");
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