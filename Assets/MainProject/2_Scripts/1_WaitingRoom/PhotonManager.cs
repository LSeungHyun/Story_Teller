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
    // .jslib에서 정의한 함수명과 동일
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);


    public RoomUIManager roomUIManager;
    public PhotonView PV;
    // 방 코드 사용되는 영어 대, 소문자 및 숫자
    private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    // 현재 서버 상태를 알려주는 텍스트 및 방 코드를 입력 받는 오브젝트
    public Text StatusText;
    public InputField room_Code_Input;
    public Text room_Code_Text;

    public Text current_Player_Text;

    // 테두리오브젝트와 이름Text 담는 배열
    public List<GameObject> OutLineList;
    public List<Text> NameTextList;

    public InputField Message_InputField;
    public GameObject myMessage;
    public GameObject otherMessage;
    public GameObject Content;

    private Color[] localPlayerColors = new Color[4];

    // 방 코드
    [SerializeField]
    public string roomCode;


    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AutomaticallySyncScene = true;

        // Hex -> Color 파싱
        ColorUtility.TryParseHtmlString("#daae6f", out localPlayerColors[0]); // 노랑
        ColorUtility.TryParseHtmlString("#929ebe", out localPlayerColors[1]); // 파랑
        ColorUtility.TryParseHtmlString("#d07e7e", out localPlayerColors[2]); // 빨강
        ColorUtility.TryParseHtmlString("#aebe9c", out localPlayerColors[3]); // 초록
    }
    #region 자체 함수 모음

    /// <summary>
    /// 서버 연결 기능 및 닉네임 생성 ex) Player312
    /// </summary>
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// 서버 연결 해제 및 닉네임 초기화
    /// </summary>
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// 랜덤 6자리 코드를 사용하여 4인 참여 가능한 방 생성
    /// </summary>

    public void CreateRoom()
    {
        roomCode = GenerateRoomCode();

        //room_Code_Text.text = roomCode;

        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 4 });

        //Debug.Log($"Room Created with Code : {roomCode}");
    }

    /// <summary>
    /// 6자리 방코드 만들어주는 곳
    /// </summary>
    /// <returns> 방 코드 6자리 </returns>
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
    /// 방을 떠날 때 인풋 필드 값 초기화
    /// </summary>
    public void LeaveRoom()
    {
        ResetPlayerRoomInfo();
        //Debug.Log("방 나왔음 : " + room_Code_Input.text);
        PhotonNetwork.LeaveRoom();
    }

    public void SendMessage()
    {
        string trimmedMessage = Message_InputField.text.Trim();

        // 입력값이 빈 문자열이면 메시지를 전송하지 않습니다.
        if (string.IsNullOrEmpty(trimmedMessage))
        {
            Debug.Log("공백 메시지는 전송되지 않습니다.");
            return;
        }

        // 1) 발신자: 자신의 메시지 셀 생성
        GameObject myMessageBox = Instantiate(myMessage, Vector3.zero, Quaternion.identity, Content.transform);
        myMessageBox.GetComponent<Message>().MyMessage.text = Message_InputField.text;

        // 2) 수신자: RPC를 호출하여 발신자 메시지와 이름 전달
        PV.RPC("GetMessage", RpcTarget.Others, Message_InputField.text, PhotonNetwork.NickName);

        Message_InputField.text = null;
    }

    [PunRPC]
    public void GetMessage(string receiveMessage, string senderName)
    {
        // 수신자의 경우, 다른 메시지 셀 생성
        GameObject messageBox = Instantiate(otherMessage, Vector3.zero, Quaternion.identity, Content.transform);
        messageBox.GetComponent<Message>().MyMessage.text = receiveMessage;
        messageBox.GetComponent<Message>().MyName.text = senderName;
    }

    public void ClearChatMessage()
    {
        RectTransform Chat = Content.GetComponent<RectTransform>();

        for (int i = Chat.childCount - 1; i >= 0; i--)
        {
            Destroy(Chat.GetChild(i).gameObject);
        }

        // 필요하다면 강제로 레이아웃을 갱신합니다.
        LayoutRebuilder.ForceRebuildLayoutImmediate(Chat);
    }


    /// <summary>
    /// 방 코드 복사
    /// </summary>
    public static void Copy(string text)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL 빌드 환경에서는 .jslib 함수 호출
        CopyToClipboard(text);
#else
        // 에디터나 다른 플랫폼에서는 systemCopyBuffer 사용
        GUIUtility.systemCopyBuffer = text;
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
        //Debug.Log("코드에러창 띄우기");
        roomUIManager.OpenPopUp("Room_Code_Error");
    }


    /// <summary>
    /// 게임 상태 Log로 확인
    /// </summary>
    private void LogUpdate()
    {
        //메서드로 분리 후 모든 Title씬 버튼 기능에 할당하기
        StatusText.text = "Log : " + PhotonNetwork.NetworkClientState.ToString();
    }

    #endregion

    #region 콜백 함수 모음
    public override void OnConnectedToMaster()
    {
        //Debug.Log("접속 콜백 완료");

        roomUIManager.popupDict["Title_Btn_Group"].SetActive(false);
        roomUIManager.ClosePopUp("Single_Multi_Select");

        roomUIManager.OpenPopUp("Lobby_Group");

        LogUpdate();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //Debug.Log("접속 끊기 콜백 완료");

        roomUIManager.popupDict["Title_Btn_Group"].SetActive(true);
        roomUIManager.ClosePopUp("Lobby_Group");

        roomUIManager.OpenPopUp("Single_Multi_Select");

        roomUIManager.BlurBoolStatus(true);

        LogUpdate();
    }

    public override void OnJoinedRoom()
    {
        //Debug.Log("방 참가 콜백 완료");

        roomUIManager.CloseAllPopUps();
        roomUIManager.OpenPopUp("Waiting_Room");
        roomUIManager.OpenPopUp("UI_Btn_Group");

        room_Code_Text.text = PhotonNetwork.CurrentRoom.Name;

        //MasterStartBtnOnOff();
        RoomInfoUpdate();

        LogUpdate();

        ClearChatMessage();
    }

    public override void OnLeftRoom()
    {
        //Debug.Log("방 나가기 콜백 완료");

        roomUIManager.ClosePopUp("Waiting_Room");
        roomUIManager.ClosePopUp("UI_Btn_Group");

        LogUpdate();

        ClearChatMessage();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RoomUpdate();
        RoomInfoUpdate();
        
        //Debug.Log("누군가 들어왔다!");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomUpdate();
        RoomInfoUpdate();
        
        //Debug.Log("누군가 나갔다!");
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

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }

        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }

    /// <summary>
    /// 방 코드와 플레이어 수 정보를 업데이트 시켜주는 매서드
    /// </summary>
    private void RoomInfoUpdate()
    {
        //현재 방 인원수
        if (PhotonNetwork.InRoom)
        {
            current_Player_Text.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            RefreshPlayerUI();
        }
    }

    /// <summary>
    /// 대기실 리스트 방 최대 인원수에 따라 오브젝트 활성화
    /// </summary>
    /// <param name="isTrue"> 활용하지 않는 대기실을 ON/OFF 할지 선택 가능  </param>
    public void RefreshPlayerUI()
    {
        // 1) 현재 방 인원 정보
        int currentCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxCount = PhotonNetwork.CurrentRoom.MaxPlayers;

        // -- 먼저 마스터/게스트 Info 초기화 (둘 다 끔) --
        roomUIManager.ClosePopUpNotDot("Info_Group_Master");
        roomUIManager.ClosePopUpNotDot("Info_Group_Guest");

        // 2) 로컬 플레이어의 인덱스 찾기
        int localIndex = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);

        // 3) 로컬 플레이어가 0번이면 마스터 Info, 아니면 게스트 Info
        if (localIndex == 0)
        {
            // 플레이어0 → 마스터 Info

            roomUIManager.OpenPopUpNotDot("Info_Group_Master");
        }
        else
        {
            // 플레이어1,2,3 → 게스트 Info
            
            roomUIManager.OpenPopUpNotDot("Info_Group_Guest");
        }

        // 4) 나머지 슬롯(OutLineList, NameTextList) 갱신
        for (int i = 0; i < maxCount; i++)
        {
            bool isActive = (i < currentCount);
            // 기본적으로 Outline은 끔
            OutLineList[i].SetActive(false);

            if (isActive)
            {
                Player p = PhotonNetwork.PlayerList[i];

                // 닉네임 설정
                NameTextList[i].text = "집배원 " + (i + 1);
                p.NickName = NameTextList[i].text;

                // 로컬 플레이어 슬롯이면 색상 + Outline
                if (p == PhotonNetwork.LocalPlayer)
                {
                    NameTextList[i].color = localPlayerColors[i];
                    OutLineList[i].SetActive(true);
                }
                else
                {
                    NameTextList[i].color = Color.white;
                }
            }
            else
            {
                NameTextList[i].text = "대기중 . . .";
                NameTextList[i].color = Color.white;
            }
        }

        //Debug.Log("내 이름: " + PhotonNetwork.LocalPlayer.NickName);
    }
}