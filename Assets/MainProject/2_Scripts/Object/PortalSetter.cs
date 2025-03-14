using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PortalSetter : MonoBehaviour
{
    [SerializeField] private ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;

    [SerializeField] private GameObject Enter_None;
    [SerializeField] private GameObject Enter_All;
    [SerializeField] private GameObject Enter_Wait;

    void Start()
    {
        managerConnector.portalSetter = this;
        SetPortalObjects(true, false, false);
    }

    private class PortalStatus
    {
        public HashSet<int> playersInside = new HashSet<int>();
        public Coroutine countdownCoroutine = null;
    }

    private Dictionary<ManagerConnector, PortalStatus> portalStatuses = new Dictionary<ManagerConnector, PortalStatus>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView PV = managerConnector.playerManager.PV;
        if (!portalStatuses.ContainsKey(managerConnector))
        {
            portalStatuses[managerConnector] = new PortalStatus();
        }

        PortalStatus status = portalStatuses[managerConnector];
        status.playersInside.Add(collision.GetInstanceID());

        if (status.playersInside.Count == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            SetPortalObjects(false, false, true);
        }
        else if (status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount && status.playersInside.Count > 0)
        {
            SetPortalObjects(false, true, false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PhotonView PV = managerConnector.playerManager.PV;
        PortalStatus status = portalStatuses[managerConnector];
        status.playersInside.Remove(collision.GetInstanceID());

        if (status.playersInside.Count == 0)
        {
            SetPortalObjects(true, false, false);
            portalStatuses.Remove(managerConnector);
        }
        else if (status.playersInside.Count < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            SetPortalObjects(false, true, false);
        }
    }

    private void SetPortalObjects(bool first, bool second, bool third)
    {
        Enter_None.SetActive(first);
        Enter_All.SetActive(second);
        Enter_Wait.SetActive(third);
    }
}
