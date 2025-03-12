using System.Linq;
using UnityEngine;


public abstract class AbsctractGameSession
{
    // 포탈에 표시할 라벨 설정 (기본 구현)
    public virtual void ShowPortalCenterLabel(PortalMananager portalMananager)
    {
        // 포탈이 가진 objCode를 현재 상호작용 오브젝트에 설정
        CurrentObjectManager.Instance.SetCurrentObjData(portalMananager.objCode);
    }

    // 필요하다면 ClosePortalCenterLabel도 추가 가능
    public virtual void ClosePortalCenterLabel(PortalMananager portalMananager)
    {
        // 라벨 닫기 로직
        CurrentObjectManager.Instance.SetCurrentObjData(null);
    }


    // 포탈 카운트다운 시작/종료 메서드: 모드별로 다르게 구현 가능하도록 추상/가상 메서드로 선언
    public abstract void StartPortalCountdown(PortalMananager portal, Collider2D collision);
    public abstract void StopPortalCountdown(PortalMananager portal, Collider2D collision);

    // 1) 공통으로 필요한 추상 메서드 (기존 IGameSession의 메서드)
    public abstract void HandleInteraction(CurrentObjectManager currentObjectManager);
    public abstract void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode);
    public abstract void OpenPopUp(UIPopUpOnOffManager UIPopUpOnOffManager, bool isQuest, bool isDial);
    public abstract void OpenCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager);
    public abstract void CloseCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager);

    // 2) (선택) 공통 로직이 있다면 추상 클래스 내부에 보호(protected) 메서드나 virtual 필드로 작성 가능
    #region Player Basic Virtual Methods
    public virtual void MoveBasic(PlayerManager playerManager)
    {
        playerManager.inputVec.x = Input.GetAxisRaw("Horizontal");
        playerManager.inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = playerManager.inputVec.normalized * Time.fixedDeltaTime;
        playerManager.rigid.MovePosition(playerManager.rigid.position + nextVec);
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
        
        playerManager.interactableStack.Remove(collision);
        playerManager.interactableStack.Add(collision);
        playerManager.UpdateInteractObject();
    }

    public virtual void TriggerExitBasic(PlayerManager playerManager, Collider2D collision)
    {
        if (!collision.CompareTag("Interaction"))
        {
            return;
        }

        playerManager.interactableStack.Remove(collision);
        Renderer renderOfCurrentCollision = collision.GetComponent<Renderer>();
        if (renderOfCurrentCollision != null)
        {
            renderOfCurrentCollision.material = playerManager.originalMaterial;
        }
    }

    #endregion
    protected void HandleInteractionBasic(CurrentObjectManager currentObjectManager)
    {
        if (currentObjectManager.currentRow == null)
            return;

        if (string.IsNullOrEmpty(currentObjectManager.currentRow.dataType))
            return;

        string currentObjCode = currentObjectManager.currentRow.objCode;
        string currentObjType = currentObjectManager.currentRow.dataType.ToLower();

        bool hasHint = currentObjType.Contains("hint");
        bool hasDialogue = currentObjType.Contains("dialogue");
        bool hasQuest = currentObjType.Contains("quest");
        bool hasBubble = currentObjType.Contains("bubble");
        bool hasCenterLabel = currentObjType.Contains("centerlabel");
        bool hasImage = currentObjType.Contains("image");

        if (hasHint)
        {
            currentObjectManager.hintStateManager.HIntUnlocked(currentObjCode);
        }
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
            if (hasQuest)
                currentObjectManager.uiPopUpOnOffManager.OpenWindow(true, true);
            else
                currentObjectManager.uiPopUpOnOffManager.OpenWindow(false, true);
        }
        if (hasImage)
        {
            currentObjectManager.uiImageSetter.SetData(currentObjCode);
            if (hasQuest)
                currentObjectManager.uiPopUpOnOffManager.OpenWindow(true, false);
            else
                currentObjectManager.uiPopUpOnOffManager.OpenWindow(false, false);
        }
    }

    protected void OpenPopUpBasic(UIPopUpOnOffManager UIPopUpOnOffManager, bool isQuest, bool isDial)
    {
        UIPopUpOnOffManager.popUpGroup.SetActive(true);
        UIPopUpOnOffManager.windowPopUp.SetActive(true);

        UIPopUpOnOffManager.defaultPopUpGroup.SetActive(!isQuest);
        UIPopUpOnOffManager.questPopUpGroup.SetActive(isQuest);

        UIPopUpOnOffManager.imageGroup.SetActive(!isDial);
        UIPopUpOnOffManager.dialogueGroup.SetActive(isDial);
    }

    protected void ClosePopUpBasic(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode)
    {
        UIPopUpOnOffManager.popUpGroup.SetActive(false);
        UIPopUpOnOffManager.windowPopUp.SetActive(false);

        UIPopUpOnOffManager.defaultPopUpGroup.SetActive(false);
        UIPopUpOnOffManager.questPopUpGroup.SetActive(false);

        UIPopUpOnOffManager.imageGroup.SetActive(false);
        UIPopUpOnOffManager.dialogueGroup.SetActive(false);

        UIPopUpOnOffManager.uiPopUpManager.ClearData();

        string currentNextObjCode = null;
        string currentNextdata = null;

        NextData foundData = UIPopUpOnOffManager.nextDataContainer.nextDatas.FirstOrDefault(data => data.objCode == currentObjCode);

        if (foundData != null)
        {
            currentNextObjCode = foundData.isNextObj;
            currentNextdata = foundData.isNextData;
        }

        if (!string.IsNullOrEmpty(currentNextObjCode))
        {
            ObjectDictionary.Instance.ToggleObjectActive(currentNextObjCode);
        }

        if (!string.IsNullOrEmpty(currentNextdata))
        {
            UIPopUpOnOffManager.currentObjectManager.SetCurrentObjData(currentNextdata);
        }
        UIPopUpOnOffManager.currentObjectManager.currentObjCode = null;
    }

    protected void OpenCenterLabelBasic(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        uiCenterLabelOnOffManager.centerLabelGroup.SetActive(true);
    }

    protected void CloseCenterLabelBasic(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        uiCenterLabelOnOffManager.centerLabelGroup.SetActive(false);
        uiCenterLabelOnOffManager.uiCenterLabelSetter.ClearData();
    }
}
