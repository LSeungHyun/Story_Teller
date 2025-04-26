using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Photon.Pun;


public class UINextSetter : MonoBehaviour
{
    public static UINextSetter Instance { get; private set; }

    [SerializeField] public NextDataContainer nextDataContainer;
    [SerializeField] public ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;
    public UIPopUpOnOffManager uiPopUpOnOffManager;

    public AbsctractGameSession session;

    public bool isTest = false;

    public string curObjCode = "";
    [System.Serializable]
    public class CurrentObjCodeList
    {
        public string key;
        public string value;
        public List<string> playersIsDone;
    }

    [SerializeField]
    public List<CurrentObjCodeList> currentObjCodeDict = new List<CurrentObjCodeList>()
{
    new CurrentObjCodeList { key = "dialogue", value = "" ,playersIsDone = new List<string>()},
    new CurrentObjCodeList { key = "image", value = "" ,playersIsDone = new List<string>()},
    new CurrentObjCodeList { key = "quest", value = "" ,playersIsDone = new List<string>()},
    new CurrentObjCodeList { key = "centerlabel", value = "" ,playersIsDone = new List<string>()}
};

    public void Awake()
    {        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        session = GameManager.Instance.Session;
    }

    public void Update()
    {
        session.SetValueUINextSetter(this);
    }
    public void SetNextCode(string currentObjCode)
    {
        if (string.IsNullOrEmpty(currentObjCode)) return;

        var matchingDataList = nextDataContainer?.nextDatas?.Where(data => data.objCode == currentObjCode);
        if (matchingDataList == null || !matchingDataList.Any()) return;

        if (objDataTypeContainer?.objDataType == null) return;

        ObjDataType objectDataType = objDataTypeContainer.objDataType.FirstOrDefault(data => data.objCode == currentObjCode);
        if (objectDataType == null) return;

        var foundItem = currentObjCodeDict?.Find(x => x.key == objectDataType.dataType);
        if (foundItem == null) return;

        foundItem.value = currentObjCode;
    }


    public void CleanNextCode(string currentObjCode)
    {
        var matchingDataList = nextDataContainer.nextDatas.Where(data => data.objCode == currentObjCode);

        if (!matchingDataList.Any())
            return;

        ObjDataType objectDataType = objDataTypeContainer.objDataType.FirstOrDefault(data => data.objCode == currentObjCode);
        currentObjCodeDict.Find(x => x.key == objectDataType.dataType).value = "";
        currentObjCodeDict.Find(x => x.key == objectDataType.dataType).playersIsDone = new List<string>();
    }

    public void ProcessNextCode(string currentObjCode)
    {
        var foundItem = currentObjCodeDict.Find(x => x.value == currentObjCode);
        if (foundItem == null || string.IsNullOrEmpty(foundItem.value))
            return;

        managerConnector.uiManager.DeactivateAllSpecialPopUps();
        var matchedDataList = nextDataContainer.nextDatas.Where(data => data.objCode == currentObjCode);

        curObjCode = currentObjCode;
        foreach (var data in matchedDataList)
        {
            if (!string.IsNullOrEmpty(data.isNextObj))
            {
                var session = GameManager.Instance.Session;
                session.ToggleObjectActive(this, data.isNextObj, data.deleteObj);
            }

            if (!string.IsNullOrEmpty(data.isNextData))
            {
                CurrentObjectManager.Instance.SetCurrentObjData(data.isNextData);
            }
        }


        // 모든 클라이언트에서 리스트 초기화
        session.ClearPlayerisDone(this);
    }



    public void AddPlayerToDoneList(string currentObjCode)
    {
        if(currentObjCode == "LivingWorld_Road_Dial" || currentObjCode == "LivingWorld_2ndFloor_Quest1")
        {
            UINextSetter.Instance.ProcessNextCode(currentObjCode);
            return;
        }
        var currentObj = currentObjCodeDict.Find(x => x.value == currentObjCode);

        if (currentObj == null)
            return;

        session.CheckDoneAndNext(this,currentObjCode);
    }

    public bool CheckEveryoneIsDone(string currentObjCode)
    {
        var currentObj = currentObjCodeDict.Find(x => x.value == currentObjCode);
        if (currentObj == null)
        {
            return false;
        }
        bool isAllDone = currentObj.playersIsDone.Count == PhotonNetwork.CurrentRoom.PlayerCount;
        return isAllDone;
    }
}