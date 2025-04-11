using UnityEngine;
using System.Linq;
using Photon.Pun;
using UnityEngine.SceneManagement;

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
            else if (portalSetter.isScenePortal)
            {
                portalSetter.ScenePortalManager.worldName = portalSetter.worldName;
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

            else if (portalSetter.isScenePortal)
            {
                portalSetter.ScenePortalManager.worldName = null;
            }
            else
            {
                portalSetter.portalManager.spawnAt = Vector3.zero;
            }
            portalSetter.SetPortalObjects(true, false, false);
            if (portalSetter.portalManager.gameObject.activeSelf)
            {
                portalSetter.portalManager.gameObject.SetActive(false);
            }
            
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
        portalManager.managerConnector.playerManager.gameObject.transform.position = portalManager.spawnAt;
    }
    #endregion

    #region IsNext

    public override void CheckDoneAndNext(UINextSetter uiNextSetter, string currentObjCode)
    {
        uiNextSetter.ProcessNextCode(currentObjCode);
    }
    public override void ToggleObjectActive(UINextSetter uiNextSetter, string nextObjCode, bool isDelete)
    {
        ObjectDictionary.Instance.ToggleObjectActive(nextObjCode, isDelete);
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
    public override void SetPlayerValue(PlayerManager playerManager)
    {
        base.SetPlayerValue(playerManager);
    }

    public override void MovedPlayerScene(ScenePortalManager scenePortalManager, string world)
    {
        SceneManager.LoadScene(scenePortalManager.worldName);
        scenePortalManager.managerConnector.playerManager.gameObject.transform.position = new Vector3(-30, 0, 0);
        scenePortalManager.managerConnector.textDataManager.loadingUI.SetActive(true);
    }


    public override void ChangePlayerisMoved(PlayerManager playerManager, bool isMove, bool isAnim)
    {
        playerManager.ChangePlayerisMove(isMove, isAnim);
    }

    public override void ChangePlayerisMovedAll(PlayerManager playerManager, bool isMove, bool isAnim)
    {
        ChangePlayerisMoved(playerManager, isMove,isAnim);
        //playerManager.ChangePlayerisMove(isMove, isAnim);
    }

    public override void MoveBasic(PlayerManager playerManager)
    {
        base.MoveBasic(playerManager);
    }
    public override void JoystickMoveBasic(PlayerManager playerManager)
    {
        base.JoystickMoveBasic(playerManager);
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

    public override void CutSceneEnter(PlayerManager playerManager, bool isCutScene)
    {
        playerManager.CutSceneUseAble(isCutScene);
    }

    public override void CutScenePlayerValue(Transform cutsceneTrigger, PlayerManager playerManager, bool isCutScene)
    {
        playerManager.CutScenePlayerSetValue(cutsceneTrigger, isCutScene);
    }
    #endregion

    #region Interaction
    public override void HandleInteractionBasic(CurrentObjectManager currentObjectManager, string currentObjCode)
    {
        base.HandleInteractionBasic(currentObjectManager, currentObjCode);
    }

    public override void SortingLayerIsCollision(DynamicSortingLayer layer, bool isCol)
    {
        base.SortingLayerIsCollision(layer,isCol);
    }

    public override void SortingLayerName(DynamicSortingLayer layer)
    {
        base.SortingLayerName(layer);
    }

    public override void OnOffPrefabsPopUp(OnOffPrefabs onOffPrefabs, Collider2D collision)
    {
        onOffPrefabs.uIManager.OpenPopUp("Help_PopUp");
        OnOffPlayerBtnGroup(onOffPrefabs.managerConnector, false);
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

    public override void OnOffPlayerBtnGroup(ManagerConnector managerConnector, bool isActive)
    {
        base.OnOffPlayerBtnGroup(managerConnector, isActive);
    }

    public override void CheckIsMobile(PlayerInstantiateManager player, ManagerConnector managerConnector)
    {
        base.CheckIsMobile(player, managerConnector);
    }
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
    }
    #endregion

    #region UINextSetter
    public override void SetValueUINextSetter(UINextSetter uINextSetter)
    {
        base.SetValueUINextSetter(uINextSetter);
    }

    public override void ClearPlayerisDone(UINextSetter uINextSetter)
    {
        uINextSetter.managerConnector.playerManager.ClearPlayerisDone(uINextSetter.curObjCode);
    }
    #endregion

    #region CutScene

    #endregion
}