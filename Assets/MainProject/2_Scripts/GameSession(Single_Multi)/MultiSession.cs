using System.Collections.Generic;
using UnityEngine;

public class MultiSession : AbsctractGameSession
{
    public override void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        ClosePopUpBasic(uiPopUpManager);

        Debug.Log("��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ.");
    }

    public override void HandleActionInteraction(KeyInputManager keyInputManager)
    {
        Debug.Log("���߳� ��Ƽ��");
        //��Ƽ�϶� Ű ��ǲ�� ���õ� ����
        if (keyInputManager.currentRow == null) return;

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