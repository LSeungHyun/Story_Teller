using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSession : AbsctractGameSession
{
    #region Portal & CenterLabel Methods

/*    private IEnumerator LoadMapCoroutine(PortalMananager portal)
    {
        portal.isAreadyMove = true;
        Debug.Log("이동 (싱글 모드)!");
        yield return new WaitForSeconds(1f);
        // 실제 이동 처리 (예: 플레이어 위치 변경)
        portal.portalContainer.playerManager.gameObject.transform.position = portal.nextMap.position;
        portal.isAreadyMove = false;
    }*/
    #endregion

    #region Player
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
        base.TriggerEnterBasic(playerManager, collision);
    }
    public override void TriggerExitBasic(PlayerManager playerManager, Collider2D collision)
    {
        base.TriggerExitBasic(playerManager, collision);
    }
    #endregion

    #region Interaction
    public override void HandleInteractionBasic(CurrentObjectManager currentObjectManager)
    {
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