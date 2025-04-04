using Photon.Pun;
using UnityEngine;

public class PlayerInstantiateManager : MonoBehaviour
{

    public PhotonView PV;
    public GameObject singlePlayer;
    public GameObject startPoint;
    public GameObject ChatBtn;
    public CamDontDes dontDes;

    public GameObject playerObj;

    public ManagerConnector managerConnector;
    public FloatingJoystick joystick;
    public GameObject webglBtn;

    private void Start()
    {        
        var session = GameManager.Instance.Session;

        session.CheckIsMobile(this, managerConnector);
        if (GameManager.Instance.isType == false)
        {
            ChatBtn.SetActive(false);
            managerConnector.photonManager.gameObject.SetActive(false);
            Destroy(PV);

            playerObj = Instantiate(singlePlayer, startPoint.transform.position, Quaternion.identity);
            session.SetCamera(dontDes, playerObj);
        }

        else
        {
            int localIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

            // 2) 인덱스별로 다른 프리팹 이름
            string prefabName = "";
            switch (localIndex)
            {
                case 0:
                    prefabName = "MasterPlayer_Yellow";
                    break;
                case 1:
                    prefabName = "GuestPlayer_Blue";
                    break;
                case 2:
                    prefabName = "GuestPlayer_Green";
                    break;
                case 3:
                    prefabName = "GuestPlayer_Red";
                    break;
                default:
                    // 혹시 범위를 벗어나면 안전장치
                    prefabName = "GuestPlayer_Red";
                    break;
            }

            // 3) 생성 위치/회전
            Vector3 spawnPos = startPoint.transform.position;
            Quaternion spawnRot = Quaternion.identity;

            // 4) PhotonNetwork.Instantiate 호출 (프리팹 이름, 위치, 회전)
            playerObj = PhotonNetwork.Instantiate(prefabName, spawnPos, spawnRot);

            session.SetCamera(dontDes, playerObj);
        }
    }
}
