using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PortalSetter : MonoBehaviour
{
    [SerializeField] private ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;
    public PortalManager portalManager;

    public Transform targetPosition;
    [SerializeField] private GameObject Enter_None;
    [SerializeField] private GameObject Enter_All;
    [SerializeField] private GameObject Enter_Wait;

    void Start()
    {
        managerConnector.portalSetter = this;
        SetPortalObjects(true, false, false);
    }

    public PortalStatus status;

    public class PortalStatus
    {
        public HashSet<int> playersInside = new HashSet<int>();
        public Coroutine countdownCoroutine = null;
    }

    public Dictionary<ManagerConnector, PortalStatus> portalStatuses = new Dictionary<ManagerConnector, PortalStatus>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var session = GameManager.Instance.Session;
        session.OnEnterPortal(this, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var session = GameManager.Instance.Session;
        session.OnExitPortal(this, collision);
    }

    public void SetPortalObjects(bool first, bool second, bool third)
    {
        Enter_None.SetActive(first);
        Enter_All.SetActive(second);
        Enter_Wait.SetActive(third);
    }
}
