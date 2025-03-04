using UnityEngine;

public abstract class AbsctractGameSession
{
    // 1) 공통으로 필요한 추상 메서드 (기존 IGameSession의 메서드)
    public abstract void HandleInteraction(KeyInputManager keyInputManager);
    public abstract void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager);
    public abstract void OpenPopUp(UIPopUpOnOffManager UIPopUpOnOffManager, bool isQuest);
    public abstract void OpenCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager);
    public abstract void CloseCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager);

    // 2) (선택) 공통 로직이 있다면 추상 클래스 내부에 보호(protected) 메서드나 필드로 작성 가능
    protected void HandleInteractionBasic(KeyInputManager keyInputManager)
    {
        if (keyInputManager.currentRow == null)
            return;

        if (string.IsNullOrEmpty(keyInputManager.currentRow.dataType))
            return;

        string currentObjCode = keyInputManager.currentRow.objCode;
        string currentObjType = keyInputManager.currentRow.dataType.ToLower();

        bool hasHint = currentObjType.Contains("hint");
        bool hasDialogue = currentObjType.Contains("dialogue");
        bool hasQuest = currentObjType.Contains("quest");
        bool hasBubble = currentObjType.Contains("bubble");
        bool hasCenterLabel = currentObjType.Contains("centerlabel");
        bool hasImage = currentObjType.Contains("image");

        if (hasHint)
        {
            keyInputManager.hintStateManager.HIntUnlocked(currentObjCode);
        }
        if (hasBubble)
        {
            Debug.Log("이건 말풍선입니다");
        }
        if (hasCenterLabel)
        {
            keyInputManager.uiCenterLabelSetter.SetData(currentObjCode);
            keyInputManager.uiCenterLabelOnOffManager.OpenCenterLabelWindow();
        }
        if (hasDialogue)
        {
            keyInputManager.uiTextSetter.SetData(currentObjCode);
            if (hasQuest)
                keyInputManager.uiPopUpOnOffManager.OpenQuestWindow();
            else
                keyInputManager.uiPopUpOnOffManager.OpenDefaultWindow();
        }
        if (hasImage)
        {
            keyInputManager.uiImageSetter.SetData(currentObjCode);
            if (hasQuest)
                keyInputManager.uiPopUpOnOffManager.OpenQuestWindow();
            else
                keyInputManager.uiPopUpOnOffManager.OpenDefaultWindow();
        }
    }

    protected void OpenPopUpBasic(UIPopUpOnOffManager UIPopUpOnOffManager, bool isQuest)
    {
        UIPopUpOnOffManager.popUpGroup.SetActive(true);
        UIPopUpOnOffManager.windowPopUp.SetActive(true);

        UIPopUpOnOffManager.defaultPopUpGroup.SetActive(!isQuest);
        UIPopUpOnOffManager.questPopUpGroup.SetActive(isQuest);
    }

    protected void ClosePopUpBasic(UIPopUpOnOffManager UIPopUpOnOffManager)
    {
        UIPopUpOnOffManager.popUpGroup.SetActive(false);
        UIPopUpOnOffManager.windowPopUp.SetActive(false);

        UIPopUpOnOffManager.defaultPopUpGroup.SetActive(false);
        UIPopUpOnOffManager.questPopUpGroup.SetActive(false);

        UIPopUpOnOffManager.uiTextSetter.ClearData();
        UIPopUpOnOffManager.uiImageSetter.ClearData();

        string currentNextObjCode = null;

        if (UIPopUpOnOffManager.uiTextSetter.targetRow?.isNextObj != null)
        {
            currentNextObjCode = UIPopUpOnOffManager.uiTextSetter.targetRow.isNextObj;
        }
        else if (UIPopUpOnOffManager.uiImageSetter.targetRow?.isNextObj != null)
        {
            currentNextObjCode = UIPopUpOnOffManager.uiImageSetter.targetRow.isNextObj;
        }

        if (!string.IsNullOrEmpty(currentNextObjCode))
        {
            UIPopUpOnOffManager.keyInputManager.SetCurrentObjData(currentNextObjCode);
        }
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
