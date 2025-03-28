using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortalManager : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public CamBoundContainer camBoundContainer;
    public AbsctractGameSession session;

    //[SerializeField]
    //private GameObject loadingUI;

    public string worldName;

    private void Awake()
    {
        session = GameManager.Instance.Session;
        managerConnector.scenePortalManager = this;
    }
    public void OnEnable()
    {
        if (PhotonNetwork.IsConnected)
        {
            managerConnector.playerManager.PV.RPC("MoveNextScene", RpcTarget.AllBuffered, worldName);
            GameManager.Instance.SelectGameMode(true);
            return;
        }
        SceneManager.LoadScene(worldName);
        managerConnector.playerManager.gameObject.transform.position = new Vector3(-30,0,0);
        managerConnector.loadingUI.SetActive(true);
        GameManager.Instance.SelectGameMode(false);
    }
}
