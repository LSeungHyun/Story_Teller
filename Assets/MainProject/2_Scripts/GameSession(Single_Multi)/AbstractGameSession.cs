using UnityEngine;

public abstract class AbsctractGameSession
{
    // 1) 공통으로 필요한 추상 메서드 (기존 IGameSession의 메서드)
    public abstract void ClosePopUp(UIPopUpManager uiPopUpManager);
    public abstract void HandleActionInteraction(KeyInputManager keyInputManager);

    // 2) (선택) 공통 로직이 있다면 추상 클래스 내부에 보호(protected) 메서드나 필드로 작성 가능
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
