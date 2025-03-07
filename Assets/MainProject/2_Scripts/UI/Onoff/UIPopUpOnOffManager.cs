using UnityEngine;

public class UIPopUpOnOffManager : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;

    public KeyInputManager keyInputManager;
    public UIPopUpManager uiPopUpManager;

    public GameObject popUpGroup;
    public GameObject defaultPopUpGroup;
    public GameObject questPopUpGroup;
    public GameObject windowPopUp;

    public void OpenDefaultWindow()
    {
        var session = GameManager.Instance.Session;
        session.OpenPopUp(this, false);
    }

    public void OpenQuestWindow()
    {
        var session = GameManager.Instance.Session;
        session.OpenPopUp(this, true);
    }
    public void ClosePopUpWindow()
    {
        // GameManager에서 현재 세션(IGameSession) 객체를 가져와 팝업 닫기 로직 위임
        var session = GameManager.Instance.Session;
        session.ClosePopUp(this, keyInputManager.currentObjCode);
    }
}
