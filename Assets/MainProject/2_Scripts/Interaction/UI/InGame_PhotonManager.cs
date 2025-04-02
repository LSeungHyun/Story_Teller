using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class InGame_PhotonManager : MonoBehaviourPunCallbacks
{
    public static InGame_PhotonManager instance;

    public UIManager UIManager;
    public PhotonView PV;

    [System.Serializable]

    public class Chating_Group
    {
        public InputField Send_Message;
        public ScrollRect Chat_Scroll_View;
        public GameObject New_Chat_Notice;
        public GameObject My_Msg;
        public GameObject Other_Msg;
        public GameObject Content;
    }

    [Header("Chating Group")]
    [SerializeField]
    private Chating_Group chating_Group;

    public bool OneCheck = false;

    private Color[] localPlayerColors = new Color[4];

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AutomaticallySyncScene = true;

        // Hex -> Color 파싱
        ColorUtility.TryParseHtmlString("#daae6f", out localPlayerColors[0]); // 노랑
        ColorUtility.TryParseHtmlString("#929ebe", out localPlayerColors[1]); // 파랑
        ColorUtility.TryParseHtmlString("#d07e7e", out localPlayerColors[2]); // 빨강
        ColorUtility.TryParseHtmlString("#aebe9c", out localPlayerColors[3]); // 초록
    }

    public void SendMessage()
    {
        string trimmedMessage = chating_Group.Send_Message.text.Trim();

        if (string.IsNullOrEmpty(trimmedMessage))
        {
            Debug.Log("공백 메시지는 전송되지 않습니다.");
            return;
        }

        GameObject myMessageBox = Instantiate(chating_Group.My_Msg, Vector3.zero, Quaternion.identity, chating_Group.Content.transform);
        myMessageBox.GetComponent<Message>().MyMessage.text = chating_Group.Send_Message.text;


        PV.RPC("GetMessage", RpcTarget.Others, chating_Group.Send_Message.text, PhotonNetwork.NickName);

        ScrollToBottom();

        chating_Group.Send_Message.text = null;
    }

    public Color CheckNickNameColor(string name)
    {
        char lastChar = name[name.Length - 1];
        int lastNum = int.Parse(lastChar.ToString());

        Color color = localPlayerColors[lastNum - 1];

        return color;
    }

    [PunRPC]
    public void GetMessage(string receiveMessage, string senderName)
    {
        GameObject messageBox = Instantiate(chating_Group.Other_Msg, Vector3.zero, Quaternion.identity, chating_Group.Content.transform);

        Text myMsg = messageBox.GetComponent<Message>().MyMessage;
        Text myName = messageBox.GetComponent<Message>().MyName;

        Color color = CheckNickNameColor(senderName);

        myMsg.text = receiveMessage;
        myMsg.color = color;

        myName.text = senderName;
        myName.color = color;

        UIManager.ChatUIStatus();

        if (OneCheck)
        {
            CheckScrollSize();
        }
        else
        {
            Invoke("CheckScrollSize", 0.05f);
        }
    }

    private void CheckScrollSize()
    {
        if (!IsScrolledToBottom_Size())
        {
            chating_Group.New_Chat_Notice.SetActive(true);
        }
    }
    private bool IsScrolledToBottom_Size()
    {
        return chating_Group.Chat_Scroll_View.verticalScrollbar.size == 1;
    }

    private bool IsScrolledToBottom()
    {
        return chating_Group.Chat_Scroll_View.verticalNormalizedPosition <= 0.01f;
    }

    public void OnScrollChanged()
    {
        if (IsScrolledToBottom())
        {
            chating_Group.New_Chat_Notice.SetActive(false);
        }
    }

    public void OnNewMessageNotificationClicked()
    {
        ScrollToBottom();
        chating_Group.New_Chat_Notice.SetActive(false);
    }

    public void ScrollToBottom()
    {
        StartCoroutine(ScrollToBottomCoroutine());
    }

    private IEnumerator ScrollToBottomCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        chating_Group.Chat_Scroll_View.verticalNormalizedPosition = 0f;
    }

    public void ClearChatMessage()
    {
        RectTransform Chat = chating_Group.Content.GetComponent<RectTransform>();

        for (int i = Chat.childCount - 1; i >= 0; i--)
        {
            Destroy(Chat.GetChild(i).gameObject);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(Chat);
    }
}