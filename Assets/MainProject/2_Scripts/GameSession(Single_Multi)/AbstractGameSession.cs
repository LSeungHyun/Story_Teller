using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class AbsctractGameSession
{
    public abstract void Move(PlayerManager playerManager);





    //���� ���� ���� �� Discard
    // 1) �������� �ʿ��� �߻� �޼��� (���� IGameSession�� �޼���)
    public abstract void HandleInteraction(KeyInputManager keyInputManager);
    public abstract void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode);
    public abstract void OpenPopUp(UIPopUpOnOffManager UIPopUpOnOffManager, bool isQuest);
    public abstract void OpenCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager);
    public abstract void CloseCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager);

    // 2) (����) ���� ������ �ִٸ� �߻� Ŭ���� ���ο� ��ȣ(protected) �޼��峪 �ʵ�� �ۼ� ����
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
            keyInputManager.bubbleSetter.currentObjOffset = keyInputManager.objDataTypeContainer.position;
            keyInputManager.bubbleSetter.SetData(currentObjCode);
        }
        if (hasCenterLabel)
        {
            keyInputManager.uiCenterLabelSetter.SetData(currentObjCode);
            keyInputManager.uiCenterLabelOnOffManager.OpenCenterLabelWindow();
        }
        if (hasDialogue)
        {
            keyInputManager.uiPopUpManager.SetData(currentObjCode);
            if (hasQuest)
                keyInputManager.uiPopUpOnOffManager.OpenQuestWindow();
            else
                keyInputManager.uiPopUpOnOffManager.OpenDefaultWindow();
        }
        if (hasImage)
        {
            keyInputManager.uiPopUpManager.SetData(currentObjCode);
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

    protected void ClosePopUpBasic(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode)
    {
        UIPopUpOnOffManager.popUpGroup.SetActive(false);
        UIPopUpOnOffManager.windowPopUp.SetActive(false);

        UIPopUpOnOffManager.defaultPopUpGroup.SetActive(false);
        UIPopUpOnOffManager.questPopUpGroup.SetActive(false);

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
            UIPopUpOnOffManager.keyInputManager.SetCurrentObjData(currentNextdata);
        }
        UIPopUpOnOffManager.keyInputManager.currentObjCode = null;
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
