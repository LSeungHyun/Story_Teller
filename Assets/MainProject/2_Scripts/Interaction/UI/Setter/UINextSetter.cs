using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Photon.Pun;

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

    public bool CheckEveryoneIsDone()
    {
        bool isAllDone = status.playersIsDone.Count == PhotonNetwork.CurrentRoom.PlayerCount;
        return isAllDone;
    }

    public void AddPlayerToDoneList()
    {
        int playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        if (!status.playersIsDone.Contains(playerID))
        {
            managerConnector.playerManager.PV.RPC("RPC_AddPlayerToDoneList", RpcTarget.AllBuffered, playerID);
        }
    }
    public void CheckDoneAndNext()
    {
        var session = GameManager.Instance.Session;
        session.CheckDoneAndNext(this);
    }
}