using System.Collections.Generic;
using UnityEngine;

public class MultiSession : IGameSession
{
    public void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        // ��Ƽ(Photon) ���� �˾� �ݱ� ����
        uiPopUpManager.popUpGroup.SetActive(false);
        uiPopUpManager.windowPopUp.SetActive(false);
        uiPopUpManager.defaultPopUpGroup.SetActive(false);
        uiPopUpManager.questPopUpGroup.SetActive(false);

        UITextSetter textPopUp = uiPopUpManager.GetComponent<UITextSetter>();
        if (textPopUp != null)
        {
            textPopUp.ClearData();
        }

        UIImageSetter imagePopUp = uiPopUpManager.GetComponent<UIImageSetter>();
        if (imagePopUp != null)
        {
            imagePopUp.ClearData();
        }

        // ��: PhotonView.isMine üũ, ��Ʈ��ũ �󿡼� ����ȭ
        // ... 

        Debug.Log("MultiSession: PopUp closed in multi mode.");
    }
    public void HandleActionInteraction(KeyInputManager keyInputManager)
    {
        //��Ƽ�϶� Ű ��ǲ�� ���õ� ����
        string currentObjCode = keyInputManager.currentRow.objCode;
        string currentObjType = keyInputManager.currentRow.dataType.ToLower();
        bool currentObjisMine = keyInputManager.currentRow.isMine;

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