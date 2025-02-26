using System.Collections.Generic;
using UnityEngine;

public class MultiSession : AbstractGameSession
{
    public override void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        // ��Ƽ(Photon) ���� �˾� �ݱ� ����
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

        // ��: PhotonView.isMine üũ, ��Ʈ��ũ �󿡼� ����ȭ
        // ... 

        Debug.Log("MultiSession: PopUp closed in multi mode.");
    }
}