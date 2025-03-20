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
        CamDontDes.instance.SetBound(camBoundContainer.boundCol);
        CamDontDes.instance.SetLensSize(camBoundContainer.lensSize);
    }
}
