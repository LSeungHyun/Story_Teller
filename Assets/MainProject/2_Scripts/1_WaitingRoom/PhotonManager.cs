using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // .jslib���� ������ �Լ���� ����
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);


    public RoomUIManager roomUIManager;
    public PhotonView PV;
    // �� �ڵ� ���Ǵ� ���� ��, �ҹ��� �� ����
    private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    // ���� ���� ���¸� �˷��ִ� �ؽ�Ʈ �� �� �ڵ带 �Է� �޴� ������Ʈ
    public Text StatusText;
    public InputField room_Code_Input;
    public Text room_Code_Text; 

    public Text current_Player_Text;

    // Button �̺�Ʈ�� �ﰢ������ ������Ʈ�� ������ ���� �ƴ� ��Ȳ�� �°� ON/OFF �����ϰ� ĳ��
    public GameObject Room;
    public GameObject Server;
    
    //public GameObject GameStartBtn;
    public GameObject CenterLabel;
    public GameObject CheckMyName;

    // �׵θ�������Ʈ�� �̸�Text ��� �迭
    public List<GameObject> OutLineList;
    public List<Text> NameTextList;

    // �� �ڵ�
    [SerializeField]
    public string roomCode;


    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30; 
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    #region ��ü �Լ� ����

    /// <summary>
    /// ���� ���� ��� �� �г��� ���� ex) Player312
    /// </summary>
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// ���� ���� ���� �� �г��� �ʱ�ȭ
    /// </summary>
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// ���� 6�ڸ� �ڵ带 ����Ͽ� 4�� ���� ������ �� ����
    /// </summary>
    
    public void CreateRoom()
    {
        roomCode = GenerateRoomCode();

        //room_Code_Text.text = roomCode;

        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 4 });

        Debug.Log($"Room Created with Code : {roomCode}");
    }

    /// <summary>
    /// 6�ڸ� ���ڵ� ������ִ� ��
    /// </summary>
    /// <returns> �� �ڵ� 6�ڸ� </returns>
    private string GenerateRoomCode()
    {
        System.Random random = new System.Random();

        return new string(Enumerable.Repeat(characters, 6).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public void JoinRoom()
    {
        room_Code_Input.text.Trim();

        if (room_Code_Input.text == "")
        {
            RoomCodeIsNull();

            return;
        }

        PhotonNetwork.JoinRoom(room_Code_Input.text);
    }

    /// <summary>
    /// ���� ���� �� ��ǲ �ʵ� �� �ʱ�ȭ
    /// </summary>
    public void LeaveRoom()
    {
        ResetPlayerRoomInfo();
        Debug.Log("�� ������ : " + room_Code_Input.text);
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// ���������׸� ���� ��ŸƮ ��ư ǥ��
    /// </summary>
    private void MasterStartBtnOnOff()
    {
        roomUIManager.ClosePopUp("Info_Group_Master");
        roomUIManager.ClosePopUp("Info_Group_Guest");
        if (PhotonNetwork.IsMasterClient)
        {
            roomUIManager.OpenPopUp("Info_Group_Master");
        }
        else
        {
            roomUIManager.OpenPopUp("Info_Group_Guest");
        }
    }

    /// <summary>
    /// �� �ڵ� ����
    /// </summary>
    public static void Copy(string text)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL ���� ȯ�濡���� .jslib �Լ� ȣ��
        CopyToClipboard(text);
#else
        // �����ͳ� �ٸ� �÷��������� systemCopyBuffer ���
        GUIUtility.systemCopyBuffer = text;
        Debug.Log("�����ͺ���");
#endif
    }
    public void CopyToClipBoard()
    {
        if(PhotonNetwork.CurrentRoom.Name != null)
        {
            Copy(PhotonNetwork.CurrentRoom.Name);
        }
    }

    public void GameStart()
    {
        if(PhotonNetwork.IsConnected) 
        {
            PV.RPC("MoveNextScene", RpcTarget.AllBuffered);
            GameManager.Instance.SelectGameMode(true);
            return;
        }
        SceneManager.LoadScene("2_UnderWorld");
        GameManager.Instance.SelectGameMode(false);
    }


    [PunRPC]
    public void MoveNextScene()
    {
        PhotonNetwork.LoadLevel("2_UnderWorld");
    }
    private void ResetPlayerRoomInfo()
    {
        room_Code_Input.text = "";
        PhotonNetwork.NickName = "";
    }

    private void RoomCodeIsNull()
    {
        Debug.Log("�ڵ忡��â ����");
        roomUIManager.OpenPopUp("Room_Code_Error");
    }
   

    /// <summary>
    /// ���� ���� Log�� Ȯ��
    /// </summary>
    private void LogUpdate()
    {
        //�޼���� �и� �� ��� Title�� ��ư ��ɿ� �Ҵ��ϱ�
        StatusText.text = "Log : " + PhotonNetwork.NetworkClientState.ToString();
    }

    #endregion

    #region �ݹ� �Լ� ����
    public override void OnConnectedToMaster()
    {
        Debug.Log("���� �ݹ� �Ϸ�");

        roomUIManager.popupDict["Title_Btn_Group"].SetActive(false);
        roomUIManager.ClosePopUp("Single_Multi_Select");

        roomUIManager.OpenPopUp("Lobby_Group");

        LogUpdate();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("���� ���� �ݹ� �Ϸ�");

        roomUIManager.popupDict["Title_Btn_Group"].SetActive(true);
        roomUIManager.ClosePopUp("Lobby_Group");

        roomUIManager.OpenPopUp("Single_Multi_Select");

        LogUpdate();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� �ݹ� �Ϸ�");

        roomUIManager.CloseAllPopUps();
        roomUIManager.OpenPopUp("Waiting_Room");

        room_Code_Text.text = PhotonNetwork.CurrentRoom.Name;

        MasterStartBtnOnOff();
        RoomInfoUpdate();

        LogUpdate();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("�� ������ �ݹ� �Ϸ�");

        roomUIManager.ClosePopUp("Waiting_Room");

        LogUpdate();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RoomUpdate();
        RoomInfoUpdate();
        Debug.Log("������ ���Դ�!");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomUpdate();
        RoomInfoUpdate();
        MasterStartBtnOnOff();
        Debug.Log("������ ������!");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        roomUIManager.OpenPopUp("Room_Create_Error");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        RoomCodeIsNull();
    }

    #endregion

    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }

        else
        {
            print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            print("�� ���� : " + PhotonNetwork.CountOfRooms);
            print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            print("�κ� �ִ���? : " + PhotonNetwork.InLobby);
            print("����ƴ���? : " + PhotonNetwork.IsConnected);
        }
    }

    /// <summary>
    /// �� �ڵ�� �÷��̾� �� ������ ������Ʈ �����ִ� �ż���
    /// </summary>
    private void RoomInfoUpdate()
    {
        //���� �� �ο���
        if (PhotonNetwork.InRoom)
        {
            current_Player_Text.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            RefreshPlayerUI();
        }
    }

    /// <summary>
    /// ���� ����Ʈ �� �ִ� �ο����� ���� ������Ʈ Ȱ��ȭ
    /// </summary>
    /// <param name="isTrue"> Ȱ������ �ʴ� ������ ON/OFF ���� ���� ���� </param>
    public void RefreshPlayerUI()
    {
        // ���� �� ����
        int currentCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxCount = PhotonNetwork.CurrentRoom.MaxPlayers;

        for (int i = 0; i < maxCount; i++)
        {
            bool isActive = (i < currentCount);
            OutLineList[i].SetActive(isActive);

            if (isActive)
            {
                // PhotonNetwork.PlayerList[i]�� i��° �÷��̾�
                Player p = PhotonNetwork.PlayerList[i];

                // �г��� ���� (��: "����� i+1")
                NameTextList[i].text = "����� " + (i + 1);
                p.NickName = NameTextList[i].text;

                // ���� �÷��̾����� üũ
                if (p == PhotonNetwork.LocalPlayer)
                {
                    // ���� �÷��̾� => ������
                    NameTextList[i].color = Color.red;
                }
                else
                {
                    // �ٸ� �÷��̾� => ���
                    NameTextList[i].color = Color.white;
                }
            }
            else
            {
                // ���� �÷��̾ ���� ����
                NameTextList[i].text = "����� . . .";
                NameTextList[i].color = Color.white;
            }
        }

        // ����׿�
        Debug.Log("�� �̸�: " + PhotonNetwork.LocalPlayer.NickName);
    }
}
