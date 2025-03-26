using System.Linq;
using UnityEngine;

public class UIPopUpOnOffManager : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;
    public CurrentObjectManager currentObjectManager;
    public ManagerConnector managerConnector;
    public UIPopUpStructure uiPopupStructure;
    public UIPopUpManager uiPopUpManager;

    public UINextSetter uiNextSetter;

    public void OpenWindow(bool isQuest, bool isDial)
    {
        var session = GameManager.Instance.Session;
        session.OpenPopUpBasic(this, isQuest, isDial);
    }
    public void ClosePopUpWindow()
    {
        var session = GameManager.Instance.Session;
        session.ClosePopUpBasic(this);
    }
    public void CloseAndCheckPopUpWindow()
    {
        var session = GameManager.Instance.Session;
        session.ClosePopUpBasic(this);
    }
}
