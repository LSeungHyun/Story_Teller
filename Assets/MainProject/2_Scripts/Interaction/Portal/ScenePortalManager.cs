using UnityEngine;

public class ScenePortalManager : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public CamBoundContainer camBoundContainer;
    public UIManager uiManager;

    public AbsctractGameSession session;

    public string worldName;

    private void Awake()
    {
        session = GameManager.Instance.Session;
        managerConnector.scenePortalManager = this;
        if (uiManager != null) return;
        uiManager = managerConnector.uiManager;
    }
    public void OnEnable()
    {
        uiManager.RapidCloseAllUI();

        managerConnector.portalManager.isNextMap = true;
        session.MovedPlayerScene(this, worldName);
    }
}
