using UnityEngine;


public class PortalManager : MonoBehaviour
{
    //public PortalSetter portalSetter;

    public ManagerConnector managerConnector;
    public Vector3 spawnAt;
    public CamBoundContainer camBoundContainer;

    public AbsctractGameSession session;
    public bool isNextMap;


    public UIManager uiManager;
    private void Awake()
    {
        session = GameManager.Instance.Session;
        managerConnector.portalManager = this;
        if (uiManager != null) return;
        uiManager = managerConnector.uiManager;

    }

    private void Start()
    {
        
    }
    public void OnEnable()
    {
        uiManager.RapidCloseAllUI();

        session.SetCamValue(camBoundContainer.camDontDes, camBoundContainer.boundCol, camBoundContainer.lensSize);

        isNextMap = true;
        session.MovePlayers(this);
    }
}
