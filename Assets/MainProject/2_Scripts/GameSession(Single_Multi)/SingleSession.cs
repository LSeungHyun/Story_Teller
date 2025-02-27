using System.Collections.Generic;
using UnityEngine;

public class SingleSession : IGameSession
{
    public void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        
        // �̱� ���� �˾� �ݱ� ����
        uiPopUpManager.popUpGroup.SetActive(false);
        uiPopUpManager.windowPopUp.SetActive(false);
        uiPopUpManager.defaultPopUpGroup.SetActive(false);
        uiPopUpManager.questPopUpGroup.SetActive(false);

        UITextSetter uiTextSetter = uiPopUpManager.GetComponent<UITextSetter>();
        if (uiTextSetter != null)
        {
            uiTextSetter.ClearData();
            if (uiTextSetter.targetRow.IsNextObj != null)
            {
                uiPopUpManager.keyInputManager.SetCurrentObjData(uiTextSetter.targetRow.IsNextObj);
                uiTextSetter.targetRow.IsNextObj = null;
            }
        }

        UIImageSetter uiImageSetter = uiPopUpManager.GetComponent<UIImageSetter>();
        if (uiImageSetter != null)
        {
            uiImageSetter.ClearData();
            if (uiImageSetter.targetRow.IsNextObj != null)
            {
                uiPopUpManager.keyInputManager.SetCurrentObjData(uiTextSetter.targetRow.IsNextObj);
                uiImageSetter.targetRow.IsNextObj = null;
            }
        }

       

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }

    public void HandleActionInteraction(KeyInputManager keyInputManager)
    {
        string currentObjCode = keyInputManager.currentRow.objCode;
        string currentObjType = keyInputManager.currentRow.dataType.ToLower();

        if (currentObjType == null)
            return;

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
            Debug.Log("�̰� ���Ͷ��Դϴ�");
            return;
        }


        if (currentObjType.Contains("dialogue"))
        {
            keyInputManager.uiTextSetter.SetTextData(currentObjCode);

            if (currentObjType.Contains("quest"))
                keyInputManager.uiPopUpManager.OpenQuestWindow();
            else
                keyInputManager.uiPopUpManager.OpenDefaultWindow();

            return;
        }

        if (currentObjType.Contains("image"))
        {
            keyInputManager.uiImageSetter.SetImageData(currentObjCode);

            if (currentObjType.Contains("quest"))
                keyInputManager.uiPopUpManager.OpenQuestWindow();
            else
                keyInputManager.uiPopUpManager.OpenDefaultWindow();

            return;
        }
    }
}