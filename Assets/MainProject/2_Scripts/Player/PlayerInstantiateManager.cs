using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInstantiateManager : MonoBehaviour
{
    public GameObject singlePlayer;
    public GameObject StartPoint;

    public GameObject playerObj;
    public CamDontDes dontDes;

    private void Awake()
    {
        if (GameManager.Instance.isType == false)
        {
            Debug.Log("싱글모드입니다");
            Instantiate(singlePlayer, StartPoint.transform.position, Quaternion.identity);
        }

        else
        {
            Debug.Log("멀티모드입니다");
            int localIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

            // 2) 인덱스별로 다른 프리팹 이름
            string prefabName = "";
            switch (localIndex)
            {
                case 0:
                    prefabName = "MasterPlayer_Yellow";
                    break;
                case 1:
                    prefabName = "GuestPlayer_Green";
                    break;
                case 2:
                    prefabName = "GuestPlayer_Blue";
                    break;
                case 3:
                    prefabName = "GuestPlayer_Red";
                    break;
                default:
                    // 혹시 범위를 벗어나면 안전장치
                    prefabName = "GuestPlayer_Green";
                    break;
            }

            // 3) 생성 위치/회전
            Vector3 spawnPos = StartPoint.transform.position;
            Quaternion spawnRot = Quaternion.identity;

            // 4) PhotonNetwork.Instantiate 호출 (프리팹 이름, 위치, 회전)
            playerObj = PhotonNetwork.Instantiate(prefabName, spawnPos, spawnRot);
        }
        //PlayerManager playerManager = playerObj.GetComponent<PlayerManager>();
        dontDes.SetFollowCam(playerObj);


        //필요시 사용
        //playerManager.joystick = joyStick;
        //playerManager.webglBtn = webGLbtn;
    }
}
