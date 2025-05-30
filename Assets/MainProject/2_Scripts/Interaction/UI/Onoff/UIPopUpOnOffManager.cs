using UnityEngine;

public class UIPopUpOnOffManager : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;
    public CurrentObjectManager currentObjectManager;
    public ManagerConnector managerConnector;
    public UIPopUpStructure uiPopupStructure;
    public UIPopUpManager uiPopUpManager;
    //포탈세터에 있는 isCutScene받아와서 조이스틱 처리 추가

    public void OpenWindow(bool isQuest, bool isDial)
    {
        var session = GameManager.Instance.Session;
        session.OpenPopUpBasic(this, isQuest, isDial);

        session.ChangePlayerisMoved(managerConnector.playerManager, false, false);

        session.OnOffPlayerBtnGroup(managerConnector, false);
    }
    public void ClosePopUpWindow()
    {
        var session = GameManager.Instance.Session;

        session.ClosePopUpBasic(this);

        session.ChangePlayerisMoved(managerConnector.playerManager, true, false);

        session.OnOffPlayerBtnGroup(managerConnector, true);
    }
}
