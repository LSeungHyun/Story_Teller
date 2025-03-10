using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class AbsctractGameSession
{
    // 1) 공통으로 필요한 추상 메서드 (기존 IGameSession의 메서드)
    public abstract void HandleInteraction(CurrentObjectManager currentObjectManager);
    public abstract void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode);
    public abstract void OpenPopUp(UIPopUpOnOffManager UIPopUpOnOffManager, bool isQuest, bool isDial);
    public abstract void OpenCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager);
    public abstract void CloseCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager);

    // 2) (선택) 공통 로직이 있다면 추상 클래스 내부에 보호(protected) 메서드나 필드로 작성 가능
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
            UIPopUpOnOffManager.currentObjectManager.SetCurrentObjData(currentNextObjCode);
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
