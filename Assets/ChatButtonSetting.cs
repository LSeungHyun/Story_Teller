using UnityEngine;
using UnityEngine.UI;

public class ChatButtonSetting : MonoBehaviour
{
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
    public Chating_Group chating_Group;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
