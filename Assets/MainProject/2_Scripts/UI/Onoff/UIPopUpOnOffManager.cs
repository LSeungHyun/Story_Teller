using UnityEngine;

public class UIPopUpOnOffManager : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;

    public CurrentObjectManager currentObjectManager;
    public UIPopUpManager uiPopUpManager;

    public GameObject popUpGroup;
    public GameObject defaultPopUpGroup;
    public GameObject questPopUpGroup;
    public GameObject windowPopUp;
    public GameObject imageGroup;
    public GameObject dialogueGroup;

    public void OpenWindow(bool isQuest, bool isDial)
    {
        var session = GameManager.Instance.Session;
        session.OpenPopUp(this, isQuest, isDial);
    }

    public void CloseAndCheckPopUpWindow()
    {
        // GameManager���� ���� ����(IGameSession) ��ü�� ������ �˾� �ݱ� ���� ����
        var session = GameManager.Instance.Session;
        session.ClosePopUp(this, currentObjectManager.currentObjCode);
        session.CheckNextCode(this, currentObjectManager.currentObjCode);
    }

    public void ClosePopUpWindow()
    {
        var session = GameManager.Instance.Session;
        session.ClosePopUp(this, currentObjectManager.currentObjCode);
    }
}
