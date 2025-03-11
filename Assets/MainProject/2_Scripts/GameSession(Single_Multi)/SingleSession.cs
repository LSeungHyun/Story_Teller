using UnityEngine;

public class SingleSession : AbsctractGameSession
{
    public override void PortalEnter(PortalMananager portalMananager, Collider2D collision)
    {
        //Enter_Wait3 출력 -> 끝나면 Move출력
        Debug.Log("싱글 포탈 이동 고고");
        base.PortalEnter(portalMananager, collision);
        
    }

    public override void PortalExit(PortalMananager portalMananager, Collider2D collision)
    {
        //센터라벨 제거, 타이머 초기화
        base.PortalExit(portalMananager, collision);

        //도착은 나경님이 만든 isPassive 활용 
    }
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