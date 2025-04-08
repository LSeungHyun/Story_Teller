using UnityEngine;
using UnityEngine.UI;
using static PhotonManager;

public class ChatButtonSetting : MonoBehaviour
{

    public UIManager UI_Manager;

    [System.Serializable]
    public class Chating_Group
    {
        public InputField Send_Message;
        public ScrollRect Chat_Scroll_View;
        public GameObject New_Chat_Notice;
        public GameObject Content;
    }

    [System.Serializable]
    public class Chating_Btn_Event
    {
        public Button Send_Btn;
        public Button New_Chat_Notice;
        public Scrollbar Scrollbar_Vertical;
    }

    [Header("Chating Group")]
    public Chating_Group chating_Group;

    [Header("Chating Btn Event Group")]
    public Chating_Btn_Event chating_Btn_Event_Group;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null)
        {
            instance.UIManager = UI_Manager;

            SetChatBtnToPM();
            SetChatBtnListenerToPM();
        }
    }

    public void SetChatBtnToPM()
    {
        instance.chating_Group.Send_Message = chating_Group.Send_Message;
        instance.chating_Group.Chat_Scroll_View = chating_Group.Chat_Scroll_View;
        instance.chating_Group.New_Chat_Notice = chating_Group.New_Chat_Notice;
        instance.chating_Group.Content = chating_Group.Content;
    }

    public void SetChatBtnListenerToPM()
    {
        chating_Btn_Event_Group.Send_Btn.onClick.AddListener(instance.SendMessage);
        chating_Btn_Event_Group.Scrollbar_Vertical.onValueChanged.AddListener(OnScrollChanged);
        chating_Btn_Event_Group.New_Chat_Notice.onClick.AddListener(instance.OnNewMessageNotificationClicked);
    }

    private void OnScrollChanged(float value)
    {
        instance.OnScrollChanged();
    }
}
