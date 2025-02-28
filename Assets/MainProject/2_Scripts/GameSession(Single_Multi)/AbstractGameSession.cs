using UnityEngine;

public abstract class AbstractGameSession : MonoBehaviour
{
    public abstract void ClosePopUp(UIPopUpManager uiPopUpManager);
    //싱글.멀티 구분지어야 할 메서드 추가

    public abstract void HandleActionInteraction(KeyInputManager keyInputManager);
}
