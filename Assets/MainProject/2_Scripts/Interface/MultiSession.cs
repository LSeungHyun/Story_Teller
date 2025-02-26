using UnityEngine;

public class MultiSession : IGameSession
{
    public void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        // 멀티(Photon) 모드용 팝업 닫기 로직
        uiPopUpManager.popUpGroup.SetActive(false);
        uiPopUpManager.dialoguePopUpGroup.SetActive(false);

        // 예: PhotonView.isMine 체크, 네트워크 상에서 동기화
        // ... 

        Debug.Log("MultiSession: PopUp closed in multi mode.");
    }
}