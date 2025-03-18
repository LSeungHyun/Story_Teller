using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class UINextSetter : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;
    public CurrentObjectManager currentObjectManager;
    public ManagerConnector managerConnector;
    public UIPopUpOnOffManager uiPopUpOnOffManager;

    [SerializeField] public DoneStatus status = new DoneStatus();
    [System.Serializable]
    public class DoneStatus
    {
        public List<int> playersIsDone = new List<int>();
    }
    public void Awake()
    {
        managerConnector.uiNextSetter = this;
    }

    public void CheckNextCodeBasic()
    {
        var matchingDataList = nextDataContainer.nextDatas.Where(data => data.objCode == currentObjectManager.currentObjCode);

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

    public void CheckEveryoneIsDone()
    {
        var session = GameManager.Instance.Session;
        session.CheckEveryoneIsDone(this);
    }
}