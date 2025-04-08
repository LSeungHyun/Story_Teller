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

    public bool isTest = false;
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
    }

    public void Update()
    {
        if (managerConnector.playerManager != null && managerConnector.playerManager.PV.IsMine && !isTest)
        {
            managerConnector.uiNextSetter = this;
            isTest = true;
        }
    }
    public void SetNextCode(string currentObjCode)
    {
        Debug.Log("SetNextCode"+currentObjCode);
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
        Debug.Log("ProcessNextCode" + currentObjCode);
        var foundItem = currentObjCodeDict.Find(x => x.value == currentObjCode);
        if (foundItem == null || string.IsNullOrEmpty(foundItem.value))
            return;

        var matchedDataList = nextDataContainer.nextDatas.Where(data => data.objCode == currentObjCode);

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

        // ProcessNextCode 실행 후 playersIsDone 리스트를 초기화하여 다음 진행 시 중복 체크를 방지합니다.

        // 모든 클라이언트에서 리스트 초기화
        managerConnector.playerManager.PV.RPC("ClearPlayerisDone", RpcTarget.AllBuffered, currentObjCode);
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

    public void AddPlayerToDoneList(string currentObjCode)
    {
        var currentObj = currentObjCodeDict.Find(x => x.value == currentObjCode);

        if (currentObj == null)
            return;

        if (!managerConnector.playerManager.PV) return;
        managerConnector.playerManager.PV.RPC("RPC_AddPlayerToDoneList", RpcTarget.AllBuffered, currentObjCode, PhotonNetwork.LocalPlayer.NickName);

    }

    public void CheckDoneAndNext(string currentObjCode)
    {
        var session = GameManager.Instance.Session;
        session.CheckDoneAndNext(this, currentObjCode);
    }
}