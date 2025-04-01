using UnityEngine;
using UnityEngine.InputSystem;
using static PortalSetter;


public abstract class AbsctractGameSession
{
    #region Portal
    public virtual void OnEnterPortal(PortalSetter portalSetter, Collider2D collision)
    {
        if (portalSetter.status == null)
        {
            portalSetter.status = new PortalStatus();
        }
        portalSetter.status.playersInside.Add(collision.GetInstanceID());
    }
    public virtual void OnExitPortal(PortalSetter portalSetter, Collider2D collision)
    {
        portalSetter.status.playersInside.Remove(collision.GetInstanceID());
    }
    public virtual void MovePlayers(PortalManager portalManager)
    {
        portalManager.managerConnector.playerManager.gameObject.transform.position = portalManager.spawnAt;
    }
    #endregion

    #region IsNext
    public abstract void AfterQuest(UIQuestSetter uiQuestSetter);
    public abstract void CheckDoneAndNext(UINextSetter uiNextSetter);
    public abstract void ToggleObjectActive(UINextSetter uiNextSetter, string nextObjCode, bool isDelete);
    #endregion

    #region Hint
    public abstract void SetHintState(HintStateManager hintStateManager, string currentObjCode, string state);
    #endregion

    #region Player

    public abstract void ChangePlayerisMoved(PlayerManager playerManager,bool isMove, bool isAnim);
    public virtual void MoveBasic(PlayerManager playerManager)
    {
        playerManager.inputVec.x = Input.GetAxisRaw("Horizontal");
        playerManager.inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = playerManager.inputVec.normalized * Time.fixedDeltaTime;
        playerManager.rigid.MovePosition(playerManager.rigid.position + nextVec * 3f);
    }

    public virtual void JoystickMoveBasic(PlayerManager playerManager)
    {
        playerManager.inputVec.x = playerManager.joystick.Horizontal;
        playerManager.inputVec.y = playerManager.joystick.Vertical;

        Vector2 nextVec = playerManager.inputVec.normalized * Time.fixedDeltaTime;

        playerManager.rigid.MovePosition(playerManager.rigid.position + nextVec * 3f);
    }
    public virtual void AnimControllerBasic(PlayerManager playerManager)
    {
        bool isMoving = playerManager.inputVec.x != 0 || playerManager.inputVec.y != 0;
        if (isMoving || playerManager.joystick != null && (playerManager.joystick.Horizontal != 0 || playerManager.joystick.Vertical != 0))
        {
            playerManager.anim.SetBool("Walking", true);

            if (playerManager.inputVec.x != 0)
            {
                playerManager.anim.SetFloat("DirX", playerManager.inputVec.x);
                playerManager.anim.SetFloat("DirY", 0);
            }
            else if (playerManager.inputVec.y != 0)
            {
                playerManager.anim.SetFloat("DirX", 0);
                playerManager.anim.SetFloat("DirY", playerManager.inputVec.y);
            }
        }
        else
        {
            playerManager.anim.SetBool("Walking", false);
        }

        playerManager.ResetInputOnKeyUp();
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
        }
        else
        {
            playerManager.UpdateInteractObject();
            playerManager.ChangeConfirmOn(true);
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
        playerManager.ChangeConfirmOn(false);

        Renderer renderOfCurrentCollision = collision.GetComponent<Renderer>();
        if (renderOfCurrentCollision != null)
        {
            renderOfCurrentCollision.material = playerManager.originalMaterial;
        }
    }
    #endregion

    #region Interaction
    public virtual void HandleInteractionBasic(CurrentObjectManager currentObjectManager)
    {
        if (currentObjectManager.currentRow == null)
            return;

        if (string.IsNullOrEmpty(currentObjectManager.currentRow.dataType))
            return;

        string currentObjCode = currentObjectManager.currentRow.objCode;

        string currentObjType = currentObjectManager.currentRow.dataType.ToLower();
        bool hasDialogue = currentObjType.Contains("dialogue");
        bool hasQuest = currentObjType.Contains("quest");
        bool hasBubble = currentObjType.Contains("bubble");
        bool hasCenterLabel = currentObjType.Contains("centerlabel");
        bool hasImage = currentObjType.Contains("image");

        if (hasBubble)
        {
            currentObjectManager.bubbleSetter.currentObjOffset = currentObjectManager.objDataTypeContainer.position;
            currentObjectManager.bubbleSetter.SetData(currentObjCode);
        }
        if (hasCenterLabel)
        {
            currentObjectManager.uiCenterLabelSetter.SetData(currentObjCode);
            currentObjectManager.uiCenterLabelOnOffManager.OpenCenterLabelWindow();
        }
        if (hasDialogue)
        {
            currentObjectManager.uiDialogueSetter.SetData(currentObjCode);
            currentObjectManager.uiPopUpOnOffManager.OpenWindow(hasQuest, hasDialogue);
        }
        if (hasImage)
        {
            currentObjectManager.uiImageSetter.SetData(currentObjCode);
            currentObjectManager.uiPopUpOnOffManager.OpenWindow(hasQuest, hasDialogue);
        }
        if (hasQuest)
        {
            currentObjectManager.hintStateManager.HIntUnlocked(currentObjCode);
            currentObjectManager.uiQuestSetter.SetQuestBg(currentObjCode);
            currentObjectManager.uiPopUpOnOffManager.OpenWindow(hasQuest, hasDialogue);
        }
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

    public virtual void OnOffPlayerBtnGroup(ManagerConnector managerConnector,bool isActive)
    {
        if (managerConnector.playerManager.isMobile)
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

    public virtual void SetCamera(CamDontDes camDontDes, GameObject playerObj) {
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
}
