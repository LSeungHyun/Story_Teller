using UnityEngine;
using System.Linq;

public class UICenterLabelOnOffManager : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;
    public CurrentObjectManager currentObjectManager;

    public GameObject centerLabelGroup;
    public UICenterLabelSetter uiCenterLabelSetter;

    public void OpenCenterLabelWindow()
    {
        var session = GameManager.Instance.Session;
        session.OpenCenterLabelBasic(this);
    }
    public void CloseCenterLabelWindow()
    {
        var session = GameManager.Instance.Session;
        session.CloseCenterLabelBasic(this);
    }
    public void CloseAndCheckCenterLabelWindow()
    {
        var session = GameManager.Instance.Session;
        session.CloseCenterLabelBasic(this);
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
            Debug.Log(currentNextObjCode);
            Debug.Log(currentNextdata);
        }

        if (!string.IsNullOrEmpty(currentNextObjCode))
        {
            ObjectDictionary.Instance.ToggleObjectActive(currentNextObjCode);
        }

        if (!string.IsNullOrEmpty(currentNextdata))
        {
            currentObjectManager.SetCurrentObjData(currentNextdata);
        }
        currentObjectManager.currentObjCode = currentNextdata;
    }
}
