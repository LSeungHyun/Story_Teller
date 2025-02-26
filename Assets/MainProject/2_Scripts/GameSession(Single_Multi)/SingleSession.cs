using System.Collections.Generic;
using UnityEngine;

public class SingleSession : AbstractGameSession
{
    public override void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        // ½Ì±Û ¸ðµå¿ë ÆË¾÷ ´Ý±â ·ÎÁ÷
        uiPopUpManager.popUpGroup.SetActive(false);
        uiPopUpManager.windowPopUp.SetActive(false);
        uiPopUpManager.defaultPopUpGroup.SetActive(false);
        uiPopUpManager.questPopUpGroup.SetActive(false);

        UITextSetter textPopUp = uiPopUpManager.GetComponent<UITextSetter>();
        if (textPopUp != null)
        {
            textPopUp.ClearTextData();
        }

        UIImageSetter imagePopUp = uiPopUpManager.GetComponent<UIImageSetter>();
        if (imagePopUp != null)
        {
            imagePopUp.ClearImageData();
        }

        /*if (uiPopUpManager.nextObjCode != null && uiPopUpManager.nextObjCode.IsNextObj != null)
        {
            //uiPopUpManager.OpenPopUpWindow(uiPopUpManager.nextObjCode);
            uiPopUpManager.nextObjCode = null;
        }*/
        Debug.Log("SingleSession: PopUp closed in single mode.");
    }
}