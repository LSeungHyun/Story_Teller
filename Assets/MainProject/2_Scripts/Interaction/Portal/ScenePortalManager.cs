using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortalManager : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public string worldName;

    private void Awake()
    {
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
        GameManager.Instance.SelectGameMode(false);
    }
}
