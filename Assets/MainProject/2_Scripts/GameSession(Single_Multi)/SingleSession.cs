using UnityEngine;

public class SingleSession : AbstractGameSession
{
    public override void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        // �̱� ���� �˾� �ݱ� ����
        uiPopUpManager.popUpGroup.SetActive(false);
        uiPopUpManager.dialoguePopUpGroup.SetActive(false);

        if (uiPopUpManager.nextObjCode != null && uiPopUpManager.nextObjCode.IsNextObj != null)
        {
            uiPopUpManager.OpenPopUpWindow(uiPopUpManager.nextObjCode);
            uiPopUpManager.nextObjCode = null;
        }
        Debug.Log("SingleSession: PopUp closed in single mode.");
    }
}