using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class AbsctractGameSession
{
    #region Portal
    public virtual void OnEnterPortal(PortalSetter portalSetter, Collider2D collision)
    {
        if (portalSetter.status == null)
        {
            portalSetter.status = new PortalSetter.PortalStatus();
        }
        portalSetter.status.playersInside.Add(collision.GetInstanceID());
    }
    public virtual void OnExitPortal(PortalSetter portalSetter, Collider2D collision)
    {
        portalSetter.status.playersInside.Remove(collision.GetInstanceID());
    }
    public abstract void MovePlayers(PortalManager portalManager);
    #endregion

    #region IsNext
    public abstract void CheckDoneAndNext(UINextSetter uiNextSetter,string currentObjCode);
    public abstract void ToggleObjectActive(UINextSetter uiNextSetter, string nextObjCode, bool isDelete);
    #endregion

    #region Hint
    public abstract void SetHintState(HintStateManager hintStateManager, string currentObjCode, string state);
    #endregion

    #region Player

    public virtual void SetPlayerValue(PlayerManager playerManager)
    {
        playerManager.managerConnector.playerManager = playerManager;

        playerManager.joystick = playerManager.managerConnector.joystick;
        playerManager.webglBtn = playerManager.managerConnector.webglBtn;
        playerManager.isMobile = playerManager.managerConnector.isMobile;
    }
    public abstract void MovedPlayerScene(ScenePortalManager scenePortalManager, string world);

    public abstract void ChangePlayerisMoved(PlayerManager playerManager, bool isMove, bool isAnim);

    public abstract void ChangePlayerisMovedAll(PlayerManager playerManager, bool isMove, bool isAnim);

    public virtual void MoveBasic(PlayerManager playerManager)
    {
        playerManager.inputVec.x = Input.GetAxisRaw("Horizontal");
        playerManager.inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = playerManager.inputVec.normalized * Time.fixedDeltaTime;
        playerManager.rigid.MovePosition(playerManager.rigid.position + nextVec * playerManager.playerSpeed);
    }

    public virtual void JoystickMoveBasic(PlayerManager playerManager)
    {
        playerManager.inputVec.x = playerManager.joystick.Horizontal;
        playerManager.inputVec.y = playerManager.joystick.Vertical;

        Vector2 nextVec = playerManager.inputVec.normalized * Time.fixedDeltaTime;

        playerManager.rigid.MovePosition(playerManager.rigid.position + nextVec * playerManager.playerSpeed);
    }
    protected enum Direction { None, Left, Right, Up, Down }

    // 클래스 멤버로 추가
    private Direction lastKeyPressed = Direction.None;
    private HashSet<Direction> heldDirections = new HashSet<Direction>();

    public virtual void AnimControllerBasic(PlayerManager playerManager)
    {
        bool isMoving = playerManager.inputVec.x != 0 || playerManager.inputVec.y != 0;
        if (isMoving || playerManager.joystick != null && (playerManager.joystick.Horizontal != 0 || playerManager.joystick.Vertical != 0))
        {
            playerManager.anim.SetBool("Walking", true);
            if (Application.isMobilePlatform)
            {
                if (playerManager.inputVec.x != 0)
                {
                    playerManager.anim.SetFloat("DirX", playerManager.inputVec.x);
                }
                if (playerManager.inputVec.y != 0)
                {
                    playerManager.anim.SetFloat("DirY", playerManager.inputVec.y);
                }
            }
            else
            {
                if (playerManager.inputVec.y != 0)
                {
                    playerManager.anim.SetFloat("DirY", playerManager.inputVec.y);
                    playerManager.anim.SetFloat("DirX", 0);
                }
                if (playerManager.inputVec.x != 0)
                {
                    playerManager.anim.SetFloat("DirX", playerManager.inputVec.x);
                    playerManager.anim.SetFloat("DirY", 0);
                }
            }
        }
        else
        {
            playerManager.anim.SetBool("Walking", false);
        }

        playerManager.ResetInputOnKeyUp();
    }



    // Direction → Vector2 매핑 헬퍼
    private Vector2 DirectionToVector(Direction d)
    {
        switch (d)
        {
            case Direction.Left: return Vector2.left;
            case Direction.Right: return Vector2.right;
            case Direction.Up: return Vector2.up;
            case Direction.Down: return Vector2.down;
            default: return Vector2.zero;
        }
    }
    public virtual void TriggerEnterBasic(PlayerManager playerManager, Collider2D collision)
    {
        if (!collision.CompareTag("Interaction"))
        {
            return;
        }

        var triggerObj = collision.GetComponent<TriggerObj>();
        if (triggerObj == null)
        {
            Debug.LogWarning($"TriggerObj 컴포넌트가 {collision.name}에 없습니다.");
            return;
        }

        playerManager.interactableStack.Remove(collision);
        playerManager.interactableStack.Add(collision);

        if (triggerObj.isTouchObject)
        {
            CurrentObjectManager.Instance.SetCurrentObjData(triggerObj.objCode);
            playerManager.interactableStack.Remove(collision);
        }
        else
        {
            playerManager.UpdateInteractObject();
        }
    }

    public virtual void TriggerExitBasic(PlayerManager playerManager, Collider2D collision)
    {
        if (!collision.CompareTag("Interaction"))
        {
            return;
        }
        playerManager.interactableStack.Remove(collision);
        playerManager.UpdateInteractObject();

        Renderer renderOfCurrentCollision = collision.GetComponent<Renderer>();
        if (renderOfCurrentCollision != null)
        {
            renderOfCurrentCollision.material = playerManager.originalMaterial;
        }
    }

    public abstract void CutSceneEnter(PlayerManager playerManager, bool isCutScene);

    public abstract void CutScenePlayerValue(PlayerManager playerManager, bool isCutScene);
    #endregion

    #region Interaction
    public virtual void HandleInteractionBasic(CurrentObjectManager currentObjectManager, string currentObjCode)
    {
        if (currentObjectManager?.objDataTypeContainer?.objDataType == null)
            return;

        ObjDataType currentRow = currentObjectManager.objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == currentObjCode);
        if (currentRow == null || string.IsNullOrEmpty(currentRow.dataType))
            return;

        string currentObjType = currentRow.dataType.ToLower();

        switch (currentObjType)
        {
            case "bubble":
                if (currentObjectManager.bubbleSetter == null) return;
                currentObjectManager.bubbleSetter.currentObjOffset = currentObjectManager.objDataTypeContainer.position;
                currentObjectManager.bubbleSetter.SetData(currentObjCode);
                break;

            case "centerlabel":
                if (currentObjectManager.uiCenterLabelSetter == null || currentObjectManager.uiCenterLabelOnOffManager == null) return;
                currentObjectManager.uiCenterLabelSetter.SetData(currentObjCode);
                currentObjectManager.uiCenterLabelOnOffManager.OpenCenterLabelWindow();
                currentObjectManager.uiCenterLabelSetter.currentObjCode = currentObjCode;
                break;

            case "dialogue":
                if (currentObjectManager.uiDialogueSetter == null || currentObjectManager.uiPopUpOnOffManager == null) return;
                OnOffPlayerBtnGroup(currentObjectManager.managerConnector, false);

                currentObjectManager.uiDialogueSetter.SetData(currentObjCode);
                currentObjectManager.uiPopUpOnOffManager.OpenWindow(false, true);
                currentObjectManager.uiDialogueSetter.currentObjCode = currentObjCode;
                break;

            case "image":
                if (currentObjectManager.uiImageSetter == null || currentObjectManager.uiPopUpOnOffManager == null) return;
                OnOffPlayerBtnGroup(currentObjectManager.managerConnector, false);

                currentObjectManager.uiImageSetter.SetData(currentObjCode);
                currentObjectManager.uiPopUpOnOffManager.OpenWindow(false, false);
                currentObjectManager.uiImageSetter.currentObjCode = currentObjCode;
                break;

            case "prefab":
                if (currentObjectManager.uiDialogueSetter == null || currentObjectManager.uiPopUpOnOffManager == null) return;
                OnOffPlayerBtnGroup(currentObjectManager.managerConnector, false);
                currentObjectManager.onOffPrefabs.HandlePrefabInteraction(currentObjCode);
                break;

            case "quest":
                if (currentObjectManager.hintStateManager == null || currentObjectManager.uiQuestSetter == null || currentObjectManager.uiPopUpOnOffManager == null) return;
                OnOffPlayerBtnGroup(currentObjectManager.managerConnector, false);

                currentObjectManager.hintStateManager.HIntUnlocked(currentObjCode);
                currentObjectManager.uiQuestSetter.SetQuestBg(currentObjCode);
                currentObjectManager.uiPopUpOnOffManager.OpenWindow(true, false);
                currentObjectManager.uiQuestSetter.currentObjCode = currentObjCode;
                break;
        }
    }

    public virtual void SortingLayerIsCollision(DynamicSortingLayer layer, bool isCol)
    {
        layer.isColliding = isCol;
    }

    public virtual void SortingLayerName(DynamicSortingLayer layer)
    {
        layer.spriteRenderer.sortingLayerName = layer.basicLayer;
        layer.SetLayerName(layer.basicLayer);
    }

    public virtual void OnOffPrefabsPopUp(OnOffPrefabs onOffPrefabs, string prefabCode)
    {
        onOffPrefabs.uIManager.OpenPopUp(prefabCode);
        OnOffPlayerBtnGroup(onOffPrefabs.managerConnector, false);
    }
    #endregion

    #region UI On Off
    public virtual void OpenPopUpBasic(UIPopUpOnOffManager UIPopUpOnOffManager, bool isQuest, bool isDial)
    {
        UIPopUpOnOffManager.uiPopupStructure.canvas.popUp.SetActive(true);
        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUp.SetActive(true);

        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUpGroup.defaultPopUp.SetActive(!isQuest);
        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUpGroup.questPopUp.SetActive(isQuest);

        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUpGroup.defaultPopUpGroup.imageGroup.SetActive(!isDial);
        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUpGroup.defaultPopUpGroup.dialogueGroup.SetActive(isDial);
    }
    public virtual void ClosePopUpBasic(UIPopUpOnOffManager UIPopUpOnOffManager)
    {
        UIPopUpOnOffManager.uiPopupStructure.canvas.popUp.SetActive(true);
        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUp.SetActive(false);

        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUpGroup.defaultPopUp.SetActive(false);
        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUpGroup.questPopUp.SetActive(false);

        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUpGroup.defaultPopUpGroup.imageGroup.SetActive(false);
        UIPopUpOnOffManager.uiPopupStructure.canvas.popUpGroup.windowPopUpGroup.defaultPopUpGroup.dialogueGroup.SetActive(false);

        UIPopUpOnOffManager.uiPopUpManager.ClearData();
    }
    public virtual void OpenCenterLabelBasic(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        uiCenterLabelOnOffManager.centerLabelGroup.SetActive(true);
    }
    public virtual void CloseCenterLabelBasic(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        uiCenterLabelOnOffManager.centerLabelGroup.SetActive(false);
        uiCenterLabelOnOffManager.uiCenterLabelSetter.ClearData();
    }

    public virtual void OnOffPlayerBtnGroup(ManagerConnector managerConnector, bool isActive)
    {
        if (managerConnector.isMobile && !managerConnector.isCutScenePlaying)
        {
            managerConnector.joystick.gameObject.SetActive(isActive);
            managerConnector.webglBtn.gameObject.SetActive(isActive);
        }
    }

    public virtual void CheckIsMobile(PlayerInstantiateManager player, ManagerConnector managerConnector)
    {
        if (Application.isMobilePlatform)
        {
            managerConnector.webglBtn = player.webglBtn;
            managerConnector.joystick = player.joystick;
            managerConnector.isMobile = true;
        }
        else
        {
            player.webglBtn.gameObject.SetActive(false);
            player.joystick.gameObject.SetActive(false);
            managerConnector.isMobile = false;
        }
    }
    #endregion

    #region Camera

    public virtual void SetCamera(CamDontDes camDontDes, GameObject playerObj)
    {
        camDontDes.virtualCam.Follow = playerObj.transform;
        camDontDes.virtualCam.LookAt = playerObj.transform;
    }

    public virtual void SetBoundLens(SetBound setBound)
    {
        setBound.camBoundContainer.lensSize = setBound.curLensSize;
        setBound.camBoundContainer.boundCol = setBound.camBound;
    }

    public virtual void SetCamValue(CamDontDes camDontDes, Collider2D newBound, float lens)
    {
        camDontDes.confinerBound.BoundingShape2D = newBound;
        camDontDes.virtualCam.Lens.OrthographicSize = lens;
    }
    #endregion

    #region UINextSetter
    public virtual void SetValueUINextSetter(UINextSetter uINextSetter)
    {
        uINextSetter.managerConnector.uiNextSetter = uINextSetter;
        uINextSetter.isTest = true;
    }

    public abstract void ClearPlayerisDone(UINextSetter uIINextSetter);
    #endregion
    
    #region CutScene

    #endregion
}