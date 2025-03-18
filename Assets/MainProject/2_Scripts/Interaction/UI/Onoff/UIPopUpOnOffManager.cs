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
        var matchingDataList = nextDataContainer.nextDatas.Where(data => data.objCode == currentObjCode);

        foreach (var foundData in matchingDataList)
        {
            if (!string.IsNullOrEmpty(foundData.isNextObj))
            {
                ObjectDictionary.Instance.ToggleObjectActive(foundData.isNextObj);
            }

            if (!string.IsNullOrEmpty(foundData.isNextData))
            {
                currentObjectManager.SetCurrentObjData(foundData.isNextData);
            }
        }
        currentObjectManager.currentObjCode = null;
    }
}
