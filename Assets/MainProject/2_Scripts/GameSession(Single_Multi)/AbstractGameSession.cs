using UnityEditor.PackageManager.Requests;
using UnityEngine;

public abstract class AbsctractGameSession
{
    // 1) �������� �ʿ��� �߻� �޼��� (���� IGameSession�� �޼���)
    public abstract void HandleInteraction(KeyInputManager keyInputManager);
    public abstract void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager);
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

        if (currentObjType.Contains("hint"))
        {
            Debug.Log("�̰� ��Ʈ�Դϴ�");
            return;
        }
        if (currentObjType.Contains("bubble"))
        {
            Debug.Log("�̰� ��ǳ���Դϴ�");
            return;
        }
        if (currentObjType.Contains("centerlabel"))
        {
            keyInputManager.uiCenterLabelSetter.SetData(currentObjCode);
            keyInputManager.uiCenterLabelOnOffManager.OpenCenterLabelWindow();
            return;
        }

        if (currentObjType.Contains("dialogue"))
        {
            keyInputManager.uiTextSetter.SetData(currentObjCode);
            if (currentObjType.Contains("quest"))
                keyInputManager.uiPopUpOnOffManager.OpenQuestWindow();
            else
                keyInputManager.uiPopUpOnOffManager.OpenDefaultWindow();
            return;
        }

        if (currentObjType.Contains("image"))
        {
            keyInputManager.uiImageSetter.SetData(currentObjCode);
            if (currentObjType.Contains("quest"))
                keyInputManager.uiPopUpOnOffManager.OpenQuestWindow();
            else
                keyInputManager.uiPopUpOnOffManager.OpenDefaultWindow();
            return;
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

        if (UIPopUpOnOffManager.uiTextSetter.targetRow?.IsNextObj != null)
        {
            currentNextObjCode = UIPopUpOnOffManager.uiTextSetter.targetRow.IsNextObj;
        }
        else if (UIPopUpOnOffManager.uiImageSetter.targetRow?.IsNextObj != null)
        {
            currentNextObjCode = UIPopUpOnOffManager.uiImageSetter.targetRow.IsNextObj;
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
