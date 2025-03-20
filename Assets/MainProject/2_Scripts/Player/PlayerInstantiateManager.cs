using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerInstantiateManager : MonoBehaviour
{
    public PhotonView PV;
    public GameObject singlePlayer;
    public GameObject startPoint;
    public CamDontDes dontDes;

    public GameObject playerObj;

    private void Awake()
    {
        if (!GameManager.Instance.isType)
        {
            Destroy(PV);
        }


        if (GameManager.Instance.isType == false)
        {
            //Debug.Log("�̱۸���Դϴ�");
            playerObj = Instantiate(singlePlayer, startPoint.transform.position, Quaternion.identity);
            dontDes.SetFollowCam(playerObj);
        }

        else
        {
            //Debug.Log("��Ƽ����Դϴ�");
            int localIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

            // 2) �ε������� �ٸ� ������ �̸�
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
                    // Ȥ�� ������ ����� ������ġ
                    prefabName = "MasterPlayer_Yellow";
                    break;
            }

            // 3) ���� ��ġ/ȸ��
            Vector3 spawnPos = startPoint.transform.position;
            Quaternion spawnRot = Quaternion.identity;

            // 4) PhotonNetwork.Instantiate ȣ�� (������ �̸�, ��ġ, ȸ��)
            playerObj = PhotonNetwork.Instantiate(prefabName, spawnPos, spawnRot);
            dontDes.SetFollowCam(playerObj);
        }
        //PlayerManager playerManager = playerObj.GetComponent<PlayerManager>();

        //�ʿ�� ���
        //playerManager.joystick = joyStick;
        //playerManager.webglBtn = webGLbtn;
    }
}
