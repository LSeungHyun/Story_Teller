using UnityEngine;

public abstract class AbstractGameSession : MonoBehaviour
{
    public abstract void ClosePopUp(UIPopUpManager uiPopUpManager);
    //�̱�.��Ƽ ��������� �� �޼��� �߰�

    public abstract void HandleActionInteraction(KeyInputManager keyInputManager);
}
