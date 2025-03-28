using Photon.Pun;
using Unity.Cinemachine;
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

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            managerConnector.webglBtn = webglBtn;
            managerConnector.joystick = joystick;
            managerConnector.isMobile = true;
        }
        else
        {
            webglBtn.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
            managerConnector.isMobile = false;
        }
        
        var session = GameManager.Instance.Session;
        if (!GameManager.Instance.isType)
        {
            ChatBtn.SetActive(false);
            Destroy(PV);
        }


        if (GameManager.Instance.isType == false)
        {
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
                    prefabName = "MasterPlayer_Yellow";
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
