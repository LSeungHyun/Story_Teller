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
            portal.objCode = "Enter_All";
            PV.RPC("ChangeObjCode",RpcTarget.AllBuffered,portal.objCode);
            PV.RPC("RPC_ShowPortalLabel", RpcTarget.AllBuffered, portal.objCode);
        }
        // 전원이 들어옴
        else if (status.playersInside.Count == requiredPlayerCount)
        {
            portal.objCode = "Enter_Wait3";
            PV.RPC("ChangeObjCode", RpcTarget.AllBuffered, portal.objCode);
            PV.RPC("RPC_ShowPortalLabel", RpcTarget.AllBuffered, portal.objCode);

            // 카운트다운 코루틴이 없으면 시작
            if (status.countdownCoroutine == null)
            {
                status.countdownCoroutine = portal.StartCoroutine(PortalCountdownCoroutine(portal));
            }
        }
    }

    //------------------------------------------------------------------------
    // 2) 포탈에서 플레이어가 나갔을 때 "카운트다운만" 중단
    //------------------------------------------------------------------------
    public override void StopPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            // 나간 플레이어 제거
            int playerID = collision.gameObject.GetInstanceID();
            status.playersInside.Remove(playerID);

            // 진행 중인 코루틴이 있으면 중단
            if (status.countdownCoroutine != null)
            {
                portal.StopCoroutine(status.countdownCoroutine);
                status.countdownCoroutine = null;
            }
            // 여기서는 '라벨' 관련 로직은 수행하지 않음 (기능 분리)
            // 라벨 처리는 ClosePortalCenterLabel(...)에서 담당
        }
    }

    //------------------------------------------------------------------------
    // 3) "센터 라벨"을 실제로 끄거나, 'Enter_All'로 변경
    //------------------------------------------------------------------------
    public override void ClosePortalCenterLabel(PortalMananager portal)
    {
        PhotonView PV = portal.portalContainer.playerManager.PV;

        // 아직 portalStatuses에 정보가 남아 있는지 확인
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            // 일부만 남았다면 'Enter_All' 상태로 갱신
            if (status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                portal.objCode = "Enter_All";
                PV.RPC("ChangeObjCode", RpcTarget.AllBuffered, portal.objCode);
                PV.RPC("RPC_ShowPortalLabel", RpcTarget.AllBuffered, portal.objCode);

                Debug.Log("다른사람들 센터라벨 켜주기" + portal.objCode);
            }
            // 아무도 안 남았다면 라벨 닫고, 딕셔너리 제거
            else if (status.playersInside.Count == 0)
            {
                PV.RPC("RPC_ClosePortalLabel", RpcTarget.AllBuffered);
                portalStatuses.Remove(portal);
            }
        }
        //else
        //{
        //    // 혹은 portalStatuses에 없다면 이미 전부 정리된 상태이므로
        //    PV.RPC("RPC_ClosePortalLabel", RpcTarget.AllBuffered);
        //}
    }

    //------------------------------------------------------------------------
    // 4) 모든 플레이어가 들어왔을 때 카운트다운 후 맵 이동
    //------------------------------------------------------------------------
    private IEnumerator PortalCountdownCoroutine(PortalMananager portal)
    {
        float timer = 0f;
        while (timer < portal.countTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // 카운트다운 완료
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
            photonView.RPC("MoveTransform", RpcTarget.AllBuffered, portal.nextMap.position);
        }

        portal.isAreadyMove = false;
        if (portalStatuses.ContainsKey(portal))
        {
            portalStatuses.Remove(portal);
        }
    }

    //------------------------------------------------------------------------
    // (싱글/멀티 공통) 플레이어 이동 로직 등
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