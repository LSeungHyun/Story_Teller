using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
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
    public Text room_Code_Input;
    public Text room_Code_Text; 

    public Text current_Player_Text;

    // Button �̺�Ʈ�� �ﰢ������ ������Ʈ�� ������ ���� �ƴ� ��Ȳ�� �°� ON/OFF �����ϰ� ĳ��
    public GameObject Room;
    public GameObject Server;
    
    //public GameObject GameStartBtn;
    public GameObject CenterLabel;
    public GameObject CheckMyName;

    // ������� ���� ������ ��Ÿ���ִ� ������Ʈ
    public List<GameObject> WaitList;
    public List<Text> RoomInfo;

    // �� �ڵ�
    [SerializeField]
    public string roomCode;


    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
    }
    #region ��ü �Լ� ����

    /// <summary>
    /// ���� ���� ��� �� �г��� ���� ex) Player312
    /// </summary>
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("���� �Ϸ�");
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

    public void JoinRoom()
    {
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

        PhotonNetwork.LeaveRoom();
        //RoomUpdate();
        //SetWaitList(true);
    }

    /// <summary>
    /// ���������׸� ���� ��ŸƮ ��ư ǥ��
    /// </summary>
    private void MasterStartBtnOnOff()
    {
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
        //GUIUtility.systemCopyBuffer = PhotonNetwork.CurrentRoom.Name;
        //GUIUtility.systemCopyBuffer = roomCode;
        Copy(PhotonNetwork.CurrentRoom.Name);
    }

    public void GameStart()
    {
        PV.RPC("MoveNextScene", RpcTarget.AllBuffered);
        //��� ���Ӿ����� �̵�
    }
    /// <summary>
    /// ���� ����Ʈ �� �ִ� �ο����� ���� ������Ʈ Ȱ��ȭ
    /// </summary>
    /// <param name="isTrue"> Ȱ������ �ʴ� ������ ON/OFF ���� ���� ���� </param>
    public void SetWaitList(bool isTrue)
    {
        //if (PhotonNetwork.CurrentRoom.MaxPlayers < 5)
        //{
        //    for (int i = PhotonNetwork.CurrentRoom.MaxPlayers; i < 5; i++)
        //    {
        //        WaitList[i].SetActive(isTrue);
        //    }
        //}
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
        //roomUIManager.CloseAllPopUps();
        roomUIManager.OpenPopUp("Room_Code_Error");
    }

    /// <summary>
    /// �� ������Ʈ �ż��� ����
    /// </summary>
    private void RoomUpdate()
    {
        CheckMyNickName();
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
        }
    }


    

    /// <summary>
    /// ���� �̸� ��� ������Ʈ �� ���� �̸� �� ��ȯ
    /// </summary>
    private void WaitRoomUpdate()
    {
        //PhotonNetwork.NickName = "";

        //for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        //{
        //    TMP_Text childText = WaitList[i].GetComponentInChildren<TMP_Text>();

        //    PhotonNetwork.PlayerList[i].NickName = "";

        //    // �÷��̾� �г��Ӱ� ����(M �Ǵ� P) ����
        //    childText.text = (i == 0) ? $"{PhotonNetwork.PlayerList[i].NickName = "Master"}" : $"{PhotonNetwork.PlayerList[i].NickName = "Player" + i}";

        //    Debug.Log(i + "��° �̸�: " + PhotonNetwork.PlayerList[i].NickName);

        //    // ���� �г������� Ȯ���Ͽ� ���� ����
        //    childText.color = (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
        //        ? Color.red
        //        : Color.black;
        //}

        //if (PhotonNetwork.PlayerList.Length - PhotonNetwork.CurrentRoom.MaxPlayers <= -1)
        //{
        //    for (int i = PhotonNetwork.PlayerList.Length; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        //    {
        //        TMP_Text childText = WaitList[i].GetComponentInChildren<TMP_Text>();

        //        childText.text = "��� ��...";
        //        childText.color = Color.gray; // ��� �� �޽����� ȸ��
        //    }
        //}
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

    private void CheckMyNickName()
    {
        //TMP_Text childText = CheckMyName.GetComponentInChildren<TMP_Text>();
        //childText.text = $"���� �г��� : {PhotonNetwork.NickName} ==> �г��� ��í";
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
        roomUIManager.CloseAllPopUps();
        roomUIManager.OpenPopUp("Lobby_Group");
        LogUpdate();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("���� ���� �ݹ� �Ϸ�");
        roomUIManager.CloseAllPopUps();
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

        SetWaitList(false);
        LogUpdate();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("�� ������ �ݹ� �Ϸ�");

        roomUIManager.CloseAllPopUps();
        roomUIManager.OpenPopUp("Lobby_Group");
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
        Debug.Log($"Room creation failed : {message}");
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
}
