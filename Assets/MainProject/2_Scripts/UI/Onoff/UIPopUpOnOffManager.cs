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
        // GameManager���� ���� ����(IGameSession) ��ü�� ������ �˾� �ݱ� ���� ����
        var session = GameManager.Instance.Session;
        session.ClosePopUp(this, keyInputManager.currentObjCode);
    }
}
