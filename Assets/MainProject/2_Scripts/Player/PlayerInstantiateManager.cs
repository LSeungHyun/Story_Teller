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
                    prefabName = "GuestPlayer_Red";
                    break;
            }

            // 3) ���� ��ġ/ȸ��
            Vector3 spawnPos = startPoint.transform.position;
            Quaternion spawnRot = Quaternion.identity;

            // 4) PhotonNetwork.Instantiate ȣ�� (������ �̸�, ��ġ, ȸ��)
            playerObj = PhotonNetwork.Instantiate(prefabName, spawnPos, spawnRot);

            session.SetCamera(dontDes, playerObj);
        }
    }
}
