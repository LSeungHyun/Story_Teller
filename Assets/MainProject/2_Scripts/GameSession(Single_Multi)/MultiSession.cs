using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiSession : AbsctractGameSession
{
    // 각 포탈(PortalMananager)마다 현재 포탈 안에 들어온 플레이어들을 관리하는 클래스
    private class PortalStatus
    {
        // 플레이어 고유 ID를 저장 (collision.gameObject.GetInstanceID() 사용)
        public HashSet<int> playersInside = new HashSet<int>();
        // 해당 포탈의 카운트다운 코루틴 참조
        public Coroutine countdownCoroutine = null;
    }

    // 각 PortalMananager별 상태를 저장하는 딕셔너리
    private Dictionary<PortalMananager, PortalStatus> portalStatuses = new Dictionary<PortalMananager, PortalStatus>();

    public override void ShowPortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        // 멀티 모드 특화 로직 추가 가능 (네트워크 동기화, UI 업데이트 등)
        base.ShowPortalCenterLabel(portalMananager, collision);
    }

    public override void ClosePortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        base.ClosePortalCenterLabel(portalMananager, collision);
    }

    // 포탈 내에 플레이어가 들어올 때 호출되는 메서드
    public override void StartPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (!portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            status = new PortalStatus();
            portalStatuses.Add(portal, status);
        }

        // 플레이어의 고유 ID를 추가
        int playerID = collision.gameObject.GetInstanceID();
        status.playersInside.Add(playerID);

        // 현재 포톤 방에 참여한 플레이어 수를 가져옴
        int requiredPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // 모든 플레이어가 아직 포탈에 들어오지 않은 경우
        if (status.playersInside.Count < requiredPlayerCount)
        {
            portal.objCode = "Enter_All";
            ShowPortalCenterLabel(portal, collision);
        }
        // 방의 모든 플레이어가 포탈에 들어온 경우
        else if (status.playersInside.Count == requiredPlayerCount)
        {
            portal.objCode = "Enter_Wait3";
            ShowPortalCenterLabel(portal, collision);

            // 카운트다운이 진행 중이 아니라면 시작
            if (status.countdownCoroutine == null)
            {
                status.countdownCoroutine = portal.StartCoroutine(PortalCountdownCoroutine(portal));
            }
        }
    }

    // 포탈에서 플레이어가 나갈 때 호출되는 메서드
    public override void StopPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            int playerID = collision.gameObject.GetInstanceID();
            status.playersInside.Remove(playerID);

            // 방의 모든 플레이어가 아직 포탈에 들어와 있지 않으면
            if (status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                if (status.countdownCoroutine != null)
                {
                    portal.StopCoroutine(status.countdownCoroutine);
                    status.countdownCoroutine = null;
                }
                portal.objCode = "Enter_All";
                ShowPortalCenterLabel(portal, collision);
            }
            // 포탈 내부에 아무도 없으면 상태 삭제
            if (status.playersInside.Count == 0)
            {
                portalStatuses.Remove(portal);
            }
        }
    }

    // 포탈에 모든 플레이어가 들어와 있을 때 진행되는 카운트다운 코루틴
    private IEnumerator PortalCountdownCoroutine(PortalMananager portal)
    {
        float timer = 0f;
        while (timer < portal.countTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        // 카운트다운 완료 시, 코루틴 참조 초기화
        if (portalStatuses.TryGetValue(portal, out PortalStatus status))
        {
            status.countdownCoroutine = null;
        }
        // 1초 대기 후 이동 처리 실행
        yield return portal.StartCoroutine(LoadMapCoroutine(portal));
    }

    // 이동 처리 코루틴 (1초 대기 후 실제 이동)
    private IEnumerator LoadMapCoroutine(PortalMananager portal)
    {
        portal.isAreadyMove = true;
        Debug.Log("멀티 모드: 이동 실행!");
        yield return new WaitForSeconds(1f);

        // 플레이어 매니저에 부착된 PhotonView를 가져옴
        PhotonView photonView = portal.portalContainer.playerManager.gameObject.GetComponent<PhotonView>();
        if (photonView != null)
        {
            // 모든 클라이언트에서 이동을 동기화하기 위해 nextMap.position을 Vector3로 전달
            photonView.RPC("MoveTransform", RpcTarget.AllBuffered, portal.nextMap.position);
        }
        else
        {
            Debug.LogError("PhotonView가 playerManager 객체에 존재하지 않습니다.");
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