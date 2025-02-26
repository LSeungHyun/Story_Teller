using System.Collections.Generic;
using UnityEngine;

public class SingleSession : AbstractGameSession
{
    public override void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        // �̱� ���� �˾� �ݱ� ����
        uiPopUpManager.popUpGroup.SetActive(false);
        uiPopUpManager.dialoguePopUpGroup.SetActive(false); 
        uiPopUpManager.textData = new string[0];
        uiPopUpManager.spriteData = new List<Sprite>();
        uiPopUpManager.text.text = "";
        uiPopUpManager.sprite.sprite = null;

        if (uiPopUpManager.nextObjCode != null && uiPopUpManager.nextObjCode.IsNextObj != null)
        {
            //uiPopUpManager.OpenPopUpWindow(uiPopUpManager.nextObjCode);
            uiPopUpManager.nextObjCode = null;
        }
        Debug.Log("SingleSession: PopUp closed in single mode.");
    }
}