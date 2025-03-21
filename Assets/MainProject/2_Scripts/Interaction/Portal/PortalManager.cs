using UnityEngine;


public class PortalManager : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public Vector3 spawnAt;
    public CamBoundContainer camBoundContainer;
    public bool isNextMap;
    private void Awake()
    {
        managerConnector.portalManager = this;
    }
    public void OnEnable()
    {
        var session = GameManager.Instance.Session;
        session.MovePlayers(this);
        isNextMap = true;
        CamDontDes.instance.SetCamValue(camBoundContainer.boundCol, camBoundContainer.lensSize);
    }
}
