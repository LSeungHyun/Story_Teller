using UnityEngine;

public class UIPopUpOnOffManager : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;
    public CurrentObjectManager currentObjectManager;
    public ManagerConnector managerConnector;
    public UIPopUpStructure uiPopupStructure;
    public UIPopUpManager uiPopUpManager;
    public SoundContainer soundContainer;
    //��Ż���Ϳ� �ִ� isCutScene�޾ƿͼ� ���̽�ƽ ó�� �߰�

    public void OpenWindow(bool isQuest, bool isDial)
    {
        var session = GameManager.Instance.Session;
        session.OpenPopUpBasic(this, isQuest, isDial);

        session.ChangePlayerisMoved(managerConnector.playerManager, false, false);

        session.OnOffPlayerBtnGroup(managerConnector, false);

        if(soundContainer != null )
        {
            soundContainer.soundManager.Play("Enter_Sound");
        }
    }
    public void ClosePopUpWindow()
    {
        var session = GameManager.Instance.Session;

        session.ClosePopUpBasic(this);

        session.ChangePlayerisMoved(managerConnector.playerManager, true, false);

        session.OnOffPlayerBtnGroup(managerConnector, true);
    }
}
