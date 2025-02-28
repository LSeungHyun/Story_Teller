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
        Debug.Log("나야나 싱글이야");
        if (keyInputManager.currentRow == null) return;
        
        string currentObjCode = keyInputManager.currentRow.objCode;
        
            
        string currentObjType = keyInputManager.currentRow.dataType.ToLower();

        if (currentObjType == null)
            return;

        if (currentObjType.Contains("hint"))
        {
            Debug.Log("이건 힌트입니다");
            return;
        }
        if (currentObjType.Contains("bubble"))
        {
            Debug.Log("이건 말풍선입니다");
            return;
        }
        if (currentObjType.Contains("centerlabel"))
        {
            Debug.Log("이건 센터라벨입니다");
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