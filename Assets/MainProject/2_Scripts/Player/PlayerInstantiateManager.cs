using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;

public class PlayerInstantiateManager : MonoBehaviour
{
    public GameObject[] playerList;
    public GameObject StartPoint;

    private void Awake()
    {
        if (GameManager.Instance.isType == false)
        {
            Debug.Log("�̱۸���Դϴ� �ı��մϴ�");
            Destroy(this.gameObject);
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
                    prefabName = "GuestPlayer_Green";
                    break;
                case 2:
                    prefabName = "GuestPlayer_Blue";
                    break;
                case 3:
                    prefabName = "GuestPlayer_Red";
                    break;
                default:
                    // Ȥ�� ������ ����� ������ġ
                    prefabName = "GuestPlayer_Green";
                    break;
            }

            // 3) ���� ��ġ/ȸ��
            Vector3 spawnPos = StartPoint.transform.position;
            Quaternion spawnRot = Quaternion.identity;

            // 4) PhotonNetwork.Instantiate ȣ�� (������ �̸�, ��ġ, ȸ��)
            GameObject playerObj = PhotonNetwork.Instantiate(prefabName, spawnPos, spawnRot);
        }
    }
}
