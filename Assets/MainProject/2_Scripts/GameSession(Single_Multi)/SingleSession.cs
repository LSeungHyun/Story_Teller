using System.Collections.Generic;
using UnityEngine;

public class SingleSession : AbsctractGameSession
{
    public override void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        ClosePopUpBasic(uiPopUpManager);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }

    public override void HandleActionInteraction(KeyInputManager keyInputManager)
    {
        Debug.Log("���߳� �̱��̾�");
        if (keyInputManager.currentRow == null) return;
        
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