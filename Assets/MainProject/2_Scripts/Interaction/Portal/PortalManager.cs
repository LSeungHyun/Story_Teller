using UnityEngine;


public class PortalManager : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public Vector3 spawnAt;
    public CamBoundContainer camBoundContainer;

    private void Awake()
    {
        managerConnector.portalManager = this;
    }
    public void OnEnable()
    {
        var session = GameManager.Instance.Session;
        session.MovePlayers(this);
        CamDontDes.instance.SetCamValue(camBoundContainer.boundCol, camBoundContainer.lensSize);
    }
}
