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
    }
    public void OnEnable()
    {
        uiManager.CloseAllPanels();
        uiManager.CloseAllPopUpsNotAnim();

        session.SetCamValue(camBoundContainer.camDontDes, camBoundContainer.boundCol, camBoundContainer.lensSize);

        isNextMap = true;
        session.MovePlayers(this);
    }
}
