using System.Linq;
using UnityEngine;

public class UIPopUpOnOffManager : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;
    public CurrentObjectManager currentObjectManager;

    public UIPopUpStructure uiPopupStructure;
    public UIPopUpManager uiPopUpManager;

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
        // GameManager���� ���� ����(IGameSession) ��ü�� ������ �˾� �ݱ� ���� ����
        var session = GameManager.Instance.Session;
        session.ClosePopUpBasic(this);
        CheckNextCodeBasic(currentObjectManager.currentObjCode);
    }
    public void CheckNextCodeBasic(string currentObjCode)
    {
        string currentNextObjCode = null;
        string currentNextdata = null;

        NextData foundData = nextDataContainer.nextDatas.FirstOrDefault(data => data.objCode == currentObjCode);

        if (foundData != null)
        {
            currentNextObjCode = foundData.isNextObj;
            currentNextdata = foundData.isNextData;
        }

        if (!string.IsNullOrEmpty(currentNextObjCode))
        {
            ObjectDictionary.Instance.ToggleObjectActive(currentNextObjCode);
        }

        if (!string.IsNullOrEmpty(currentNextdata))
        {
            currentObjectManager.SetCurrentObjData(currentNextdata);
        }
       currentObjectManager.currentObjCode = null;
    }
}
