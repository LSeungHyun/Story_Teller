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

    // Button 이벤트로 즉각적으로 오브젝트가 꺼지는 것이 아닌 상황에 맞게 ON/OFF 가능하게 캐싱
    public GameObject Room;
    public GameObject Server;
    
    //public GameObject GameStartBtn;
    public GameObject CenterLabel;
    public GameObject CheckMyName;

    // 테두리오브젝트와 이름Text 담는 배열
    public List<GameObject> OutLineList;
    public List<Text> NameTextList;

    // 방 코드
    [SerializeField]
    public string roomCode;


    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30; 
        PhotonNetwork.AutomaticallySyncScene = true;
        //Screen.SetResolution(1920, 1080, false);
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

        Debug.Log($"Room Created with Code : {roomCode}");
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
        
        if (room_Code_Input.text == "")
        {
            RoomCodeIsNull();

            return;
        }
        room_Code_Input.text.Trim();

        PhotonNetwork.JoinRoom(room_Code_Input.text);
    }

    /// <summary>
    /// 방을 떠날 때 인풋 필드 값 초기화
    /// </summary>
    public void LeaveRoom()
    {
        ResetPlayerRoomInfo();
        Debug.Log("방 나왔음 : " + room_Code_Input.text);
        PhotonNetwork.LeaveRoom();
        //RoomUpdate();
        //SetWaitList(true);
    }

    /// <summary>
    /// 마스터한테만 게임 스타트 버튼 표시
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
        Debug.Log("에디터복사");
#endif
    }
    public void CopyToClipBoard()
    {
        Copy(PhotonNetwork.CurrentRoom.Name);
    }

    public void GameStart()
    {
        PV.RPC("MoveNextScene", RpcTarget.AllBuffered);
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
        Debug.Log("코드에러창 띄우기");
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
        Debug.Log("접속 콜백 완료");

        roomUIManager.CloseAllPopUps();
        roomUIManager.OpenPopUp("Lobby_Group");

        LogUpdate();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("접속 끊기 콜백 완료");

        roomUIManager.CloseAllPopUps();
        roomUIManager.OpenPopUp("Single_Multi_Select");

        LogUpdate();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 콜백 완료");

        roomUIManager.CloseAllPopUps();
        roomUIManager.OpenPopUp("Waiting_Room");

        room_Code_Text.text = PhotonNetwork.CurrentRoom.Name;

        MasterStartBtnOnOff();
        RoomInfoUpdate();

       
        LogUpdate();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("방 나가기 콜백 완료");

        roomUIManager.CloseAllPopUps();
        roomUIManager.OpenPopUp("Lobby_Group");
        LogUpdate();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RoomUpdate();
        RoomInfoUpdate();
        Debug.Log("누군가 들어왔다!");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomUpdate();
        RoomInfoUpdate();
        MasterStartBtnOnOff();
        Debug.Log("누군가 나갔다!");
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
    /// 대기실 이름 명단 업데이트 및 본인 이름 색 변환
    /// </summary>
    private void WaitRoomUpdate()
    {
        PhotonNetwork.NickName = "";

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Text childText = NameTextList[i];

            PhotonNetwork.PlayerList[i].NickName = "";

            // 플레이어 닉네임과 역할(M 또는 P) 설정
            childText.text = (i == 0) ? $"{PhotonNetwork.PlayerList[i].NickName = "Master"}" : $"{PhotonNetwork.PlayerList[i].NickName = "Player" + i}";

            Debug.Log(i + "번째 이름: " + PhotonNetwork.PlayerList[i].NickName);

            // 본인 닉네임인지 확인하여 색상 설정
            childText.color = (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                ? Color.red
                : Color.black;
        }

        if (PhotonNetwork.PlayerList.Length - PhotonNetwork.CurrentRoom.MaxPlayers <= -1)
        {
            for (int i = PhotonNetwork.PlayerList.Length; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                TMP_Text childText = OutLineList[i].GetComponentInChildren<TMP_Text>();

                childText.text = "대기 중...";
                childText.color = Color.gray; // 대기 중 메시지는 회색
            }
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
    /// <param name="isTrue"> 활용하지 않는 대기실을 ON/OFF 할지 선택 가능 </param>
    public void RefreshPlayerUI()
    {
        int currentCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxCount = PhotonNetwork.CurrentRoom.MaxPlayers;

        for (int i = 0; i < maxCount; i++)
        {
            // 1) i < currentCount => 활성화, 아니면 비활성화
            bool isActive = (i < currentCount);
            OutLineList[i].SetActive(isActive);

            if (isActive)
            {
                // 플레이어가 존재하는 슬롯
                // 닉네임/텍스트 세팅
                NameTextList[i].text = "집배원 " + (i + 1);

                // PhotonNetwork.PlayerList[i]가 i인덱스와 1:1 매칭된다고 가정
                // (플레이어 순서가 정렬되는지 확인 필요)
                PhotonNetwork.PlayerList[i].NickName = NameTextList[i].text;
            }
            else
            {
                // 플레이어가 없는 슬롯
                NameTextList[i].text = "대기중 . . .";
            }
        }

        // (선택) 디버그용
        Debug.Log("내이름 : " + PhotonNetwork.NickName);
    }
}
