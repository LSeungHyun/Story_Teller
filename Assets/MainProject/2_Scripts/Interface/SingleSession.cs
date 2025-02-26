using UnityEngine;

public class SingleSession : IGameSession
{

    public void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        // ½Ì±Û ¸ðµå¿ë ÆË¾÷ ´Ý±â ·ÎÁ÷
        uiPopUpManager.popUpGroup.SetActive(false);
        uiPopUpManager.dialoguePopUpGroup.SetActive(false);

        uiPopUpManager.textData = null;
        uiPopUpManager.spriteData = null;

        if (uiPopUpManager.nextObjCode != null && uiPopUpManager.nextObjCode.IsNextObj != null)
        {
            //uiPopUpManager.OpenPopUpWindow(uiPopUpManager.nextObjCode);
            uiPopUpManager.nextObjCode = null;
        }
        Debug.Log("SingleSession: PopUp closed in single mode.");
    }
}