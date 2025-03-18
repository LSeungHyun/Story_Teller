using System.Linq;
using UnityEngine;

public class UIPopUpOnOffManager : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;
    public CurrentObjectManager currentObjectManager;

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
        // GameManager에서 현재 세션(IGameSession) 객체를 가져와 팝업 닫기 로직 위임
        var session = GameManager.Instance.Session;
        session.ClosePopUpBasic(this);
        uiNextSetter.CheckNextCodeBasic();
    }
}
