using UnityEngine;

public class MultiSession : IGameSession
{
    public void ClosePopUp(UIPopUpManager uiPopUpManager)
    {
        // ��Ƽ(Photon) ���� �˾� �ݱ� ����
        uiPopUpManager.popUpGroup.SetActive(false);
        uiPopUpManager.dialoguePopUpGroup.SetActive(false);

        // ��: PhotonView.isMine üũ, ��Ʈ��ũ �󿡼� ����ȭ
        // ... 

        Debug.Log("MultiSession: PopUp closed in multi mode.");
    }
}