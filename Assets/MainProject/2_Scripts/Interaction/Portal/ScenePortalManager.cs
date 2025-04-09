using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (PhotonNetwork.IsConnected)
        {
            managerConnector.playerManager.PV.RPC("MoveNextScene", RpcTarget.AllBuffered, worldName);
            return;
        }

        SceneManager.LoadScene(worldName);
        managerConnector.playerManager.gameObject.transform.position = new Vector3(-30,0,0);
        managerConnector.textDataManager.loadingUI.SetActive(true);
    }
}
