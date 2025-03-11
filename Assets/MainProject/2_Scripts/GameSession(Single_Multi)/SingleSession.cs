using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSession : AbsctractGameSession
{
    #region Portal & CenterLabel Methods
    // 각 포탈에 대한 코루틴 참조를 관리하기 위한 딕셔너리
    private Dictionary<PortalMananager, Coroutine> portalCoroutines = new Dictionary<PortalMananager, Coroutine>();

    public override void ShowPortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        // 필요한 싱글 모드 전용 로직 추가 가능
        base.ShowPortalCenterLabel(portalMananager, collision);
    }

    public override void ClosePortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        // 싱글 모드 전용 로직 추가 가능
        base.ClosePortalCenterLabel(portalMananager, collision);
    }

    public override void StartPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (!portalCoroutines.ContainsKey(portal))
        {
            Coroutine c = portal.StartCoroutine(PortalCountdownCoroutine(portal));
            portalCoroutines.Add(portal, c);
        }
    }

    public override void StopPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (portalCoroutines.TryGetValue(portal, out Coroutine c))
        {
            portal.StopCoroutine(c);
            portalCoroutines.Remove(portal);
        }
    }

    private IEnumerator PortalCountdownCoroutine(PortalMananager portal)
    {
        float timer = 0f;
        while (timer < portal.countTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        // 카운트다운 완료 시 코루틴 참조 삭제
        portalCoroutines.Remove(portal);
        // 1초 후에 이동 실행
        portal.StartCoroutine(LoadMapCoroutine(portal));
    }

    private IEnumerator LoadMapCoroutine(PortalMananager portal)
    {
        portal.isAreadyMove = true;
        Debug.Log("이동 (싱글 모드)!");
        yield return new WaitForSeconds(1f);
        // 실제 이동 처리 (예: 플레이어 위치 변경)
        portal.portalContainer.playerManager.gameObject.transform.position = portal.nextMap.position;
        portal.isAreadyMove = false;
    }
    #endregion

    #region Player Abstract Methods;
    public override void MoveBasic(PlayerManager playerManager)
    {
        //MoveBasic(playerManager);
        base.MoveBasic(playerManager);
    }

    public override void AnimControllerBasic(PlayerManager playerManager)
    {
        base.AnimControllerBasic(playerManager);
    }

    public override void TriggerEnterBasic(PlayerManager playerManager, Collider2D collision)
    {
        base.TriggerExitBasic(playerManager, collision);
    }

    public override void TriggerExitBasic(PlayerManager playerManager, Collider2D collision)
    {
        base.TriggerExitBasic(playerManager, collision);
    }
    #endregion
    public override void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode)
    {
        ClosePopUpBasic(UIPopUpOnOffManager, currentObjCode);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }

    public override void OpenPopUp(UIPopUpOnOffManager uiPopUpOnOffManager, bool isQuest, bool isDial)
    {
        OpenPopUpBasic(uiPopUpOnOffManager, isQuest, isDial);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }

    public override void HandleInteraction(CurrentObjectManager currentObjectManager)
    {
        HandleInteractionBasic(currentObjectManager);
        Debug.Log("나야나 싱글이야");
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