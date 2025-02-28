using UnityEngine;

public abstract class AbsctractGameSession
{
    // 1) �������� �ʿ��� �߻� �޼��� (���� IGameSession�� �޼���)
    public abstract void ClosePopUp(UIPopUpManager uiPopUpManager);
    public abstract void HandleActionInteraction(KeyInputManager keyInputManager);

    // 2) (����) ���� ������ �ִٸ� �߻� Ŭ���� ���ο� ��ȣ(protected) �޼��峪 �ʵ�� �ۼ� ����
    protected void ClosePopUpBasic(UIPopUpManager uiPopUpManager)
    {
        uiPopUpManager.popUpGroup.SetActive(false);
        uiPopUpManager.windowPopUp.SetActive(false);
        uiPopUpManager.defaultPopUpGroup.SetActive(false);
        uiPopUpManager.questPopUpGroup.SetActive(false);

        UITextSetter uiTextSetter = uiPopUpManager.GetComponent<UITextSetter>();
        if (uiTextSetter != null)
        {
            uiTextSetter.ClearData();
            if (uiTextSetter.targetRow.IsNextObj != null)
            {
                uiPopUpManager.keyInputManager.SetCurrentObjData(uiTextSetter.targetRow.IsNextObj);
                uiTextSetter.targetRow.IsNextObj = null;
            }
        }

        UIImageSetter uiImageSetter = uiPopUpManager.GetComponent<UIImageSetter>();
        if (uiImageSetter != null)
        {
            uiImageSetter.ClearData();
            if (uiImageSetter.targetRow.IsNextObj != null)
            {
                uiPopUpManager.keyInputManager.SetCurrentObjData(uiTextSetter.targetRow.IsNextObj);
                uiImageSetter.targetRow.IsNextObj = null;
            }
        }
    }
}
