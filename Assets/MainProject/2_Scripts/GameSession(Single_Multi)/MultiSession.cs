using System.Collections.Generic;
using UnityEngine;

public class MultiSession : AbstractGameSession
{
    public override void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        // 멀티(Photon) 모드용 팝업 닫기 로직
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

        // 예: PhotonView.isMine 체크, 네트워크 상에서 동기화
        // ... 

        Debug.Log("MultiSession: PopUp closed in multi mode.");
    }
}