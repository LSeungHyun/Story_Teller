using UnityEngine;
using System.Linq;

public class SingleSession : AbsctractGameSession
{
    #region Portal
    public override void OnEnterPortal(PortalSetter portalSetter, Collider2D collision)
    {
        portalSetter.portalManager.isNextMap = false;

        base.OnEnterPortal(portalSetter, collision);
        if (portalSetter.status.playersInside.Count == 1)
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
            portalSetter.portalManager.gameObject.SetActive(false);
            if (!portalSetter.portalManager.isNextMap)
            {
                CurrentObjectManager.Instance.uiCenterLabelOnOffManager.CloseCenterLabelWindow();
                portalSetter.portalManager.gameObject.SetActive(false);
            }
            portalSetter.status = null;
        }
    }
    public override void MovePlayers(PortalManager portalManager)
    {
        Debug.Log("싱글로 옮기기!");
        base.MovePlayers(portalManager);
    }
    #endregion

    #region IsNext
    public override void AfterQuest(UIQuestSetter uiQuestSetter)
    {
        uiQuestSetter.uiPopUpOnOffManager.ClosePopUpWindow();
        uiQuestSetter.uiNextSetter.CheckDoneAndNext();
    }
    public override void CheckDoneAndNext(UINextSetter uiNextSetter)
    {
        uiNextSetter.uiPopUpOnOffManager.ClosePopUpWindow();
        uiNextSetter.CheckNextCodeBasic();
    }
    public override void ToggleObjectActive(UINextSetter uiNextSetter, string nextObjCode)
    {
        ObjectDictionary.Instance.ToggleObjectActive(nextObjCode);
    }
    #endregion

    #region Hint
    public override void SetHintState(HintStateManager hintStateManager, string currentObjCode, string state)
    {
        hintStateManager.targetRow = hintStateManager.hintContainer.hintDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        if (hintStateManager.targetRow == null)
            return;
        hintStateManager.targetRow.isUsed = state;
    }
    #endregion

    #region Player
    public override void ChangePlayerisMoved(PlayerManager playerManager, bool isMove, bool isAnim)
    {
        playerManager.ChangePlayerisMove(isMove, isAnim);
    }
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

    #region Camera
    public override void SetCamera(CamDontDes camera, GameObject playerObj)
    {
        base.SetCamera(camera, playerObj);
    }

    public override void SetBoundLens(SetBound setBound)
    {
        base.SetBoundLens(setBound);
    }

    public override void SetCamValue(CamDontDes camDontDes, Collider2D newBound, float lens)
    {
        base.SetCamValue(camDontDes, newBound, lens);
        //Debug.Log("1" + newBound);
        //Debug.Log("2" + lens);
    }
    #endregion
}