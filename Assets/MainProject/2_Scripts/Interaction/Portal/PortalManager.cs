using UnityEngine;


public class PortalManager : MonoBehaviour
{
    //public PortalSetter portalSetter;
    public ManagerConnector managerConnector;
    public Vector3 spawnAt;
    public CamBoundContainer camBoundContainer;

    public AbsctractGameSession session;
    public bool isNextMap;
    private void Awake()
    {
        session = GameManager.Instance.Session;
        managerConnector.portalManager = this;
    }
    public void OnEnable()
    {
        session.SetCamValue(camBoundContainer.camDontDes, camBoundContainer.boundCol, camBoundContainer.lensSize);
        Debug.Log("3" + camBoundContainer.boundCol);
        Debug.Log("4" + camBoundContainer.lensSize);
        isNextMap = true;
        session.MovePlayers(this);
    }
}
