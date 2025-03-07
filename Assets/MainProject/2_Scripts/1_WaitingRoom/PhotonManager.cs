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

    // �׵θ�������Ʈ�� �̸�Text ��� �迭
    public List<GameObject> OutLineList;
    public List<Text> NameTextList;

    private Color[] localPlayerColors = new Color[4];

    // �� �ڵ�
    [SerializeField]
    public string roomCode;


    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AutomaticallySyncScene = true;

        // Hex -> Color �Ľ�
        ColorUtility.TryParseHtmlString("#daae6f", out localPlayerColors[0]); // ���
        ColorUtility.TryParseHtmlString("#929ebe", out localPlayerColors[1]); // �Ķ�
        ColorUtility.TryParseHtmlString("#d07e7e", out localPlayerColors[2]); // ����
        ColorUtility.TryParseHtmlString("#aebe9c", out localPlayerColors[3]); // �ʷ�
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
    //private void MasterStartBtnOnOff()
    //{
    //    //roomUIManager.ClosePopUp("Info_Group_Master");
    //    //roomUIManager.ClosePopUp("Info_Group_Guest");
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        roomUIManager.OpenPopUp("Info_Group_Master");
    //        Debug.Log("������");
    //    }
    //    else
    //    {
    //        roomUIManager.OpenPopUp("Info_Group_Guest");
    //        Debug.Log("�Խ�Ʈ");
    //    }
    //}

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
        if (PhotonNetwork.CurrentRoom.Name != null)
        {
            Copy(PhotonNetwork.CurrentRoom.Name);
        }
    }

    public void GameStart()
    {
        if (PhotonNetwork.IsConnected)
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

        //MasterStartBtnOnOff();
        RoomInfoUpdate();

        LogUpdate();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("�� ������ �ݹ� �Ϸ�");
        roomUIManager.ClosePopUp("Info_Group_Guest");
        roomUIManager.ClosePopUp("Info_Group_Master");
        roomUIManager.ClosePopUp("Waiting_Room");

        LogUpdate();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RoomUpdate();
        RoomInfoUpdate();
        //MasterStartBtnOnOff();
        Debug.Log("������ ���Դ�!");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomUpdate();
        RoomInfoUpdate();
        //MasterStartBtnOnOff();
        Debug.Log("������ ������!");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        RoomInfoUpdate();
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
    /// <param name="isTrue"> Ȱ������ �ʴ� ������ ON/OFF ���� ���� ����  </param>
    public void RefreshPlayerUI()
    {
        // 1) ���� �� �ο� ����
        int currentCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxCount = PhotonNetwork.CurrentRoom.MaxPlayers;

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //roomUIManager.ClosePopUp("Info_Group_Guest");
            roomUIManager.OpenPopUp("Info_Group_Master");
            Debug.Log("���� ������ (���� �÷��̾�)");
        }
        else
        {
            //roomUIManager.ClosePopUp("Info_Group_Master");
            roomUIManager.OpenPopUp("Info_Group_Guest");
            Debug.Log("���� �Խ�Ʈ (���� �÷��̾�)");
        }

        // 3) ����(OutLineList, NameTextList) ����
        for (int i = 0; i < maxCount; i++)
        {
            // ���� Ȱ��ȭ ���� (���� �ο����� ������ Ȱ��ȭ)
            bool isActive = (i < currentCount);
            // �켱 Outline�� ��� �� (�ʿ� �� ���� �÷��̾ �� ����)
            OutLineList[i].SetActive(false);

            if (isActive)
            {
                // PhotonNetwork.PlayerList[i]�� i��° �÷��̾�
                Player p = PhotonNetwork.PlayerList[i];

                // �г��� ���� (��: "����� i+1")
                NameTextList[i].text = "����� " + (i + 1);
                p.NickName = NameTextList[i].text;

                // ���� �÷��̾� �������� Ȯ��
                if (p == PhotonNetwork.LocalPlayer)
                {
                    // �� ���� �� ���� ���� & Outline �ѱ�
                    NameTextList[i].color = localPlayerColors[i];
                    OutLineList[i].SetActive(true);
                    if (PhotonNetwork.IsMasterClient)
                    {

                    }
                }
                else
                {
                    // �ٸ� �÷��̾� �� ��� �г���
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

        Debug.Log("�� �̸�: " + PhotonNetwork.LocalPlayer.NickName);
    }

}