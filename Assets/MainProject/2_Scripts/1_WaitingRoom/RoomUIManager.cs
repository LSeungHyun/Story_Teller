using System.Collections.Generic;
using UnityEngine;

public class RoomUIManager : DoTweenManager
{
    [System.Serializable]
    public class PopupItem
    {
        public string popupName;     // "LobbyGroup", "Single_Multi_Select", ...
        public GameObject popupObject;
    }

    [System.Serializable]
    public class ChatStatus
    {
        public string justName;
        public GameObject ShadowObj;
        public GameObject IconObj;
    }

    [Header("PopUp List")]
    [SerializeField]
    private List<PopupItem> popupList;

    [Header("OnOffChat List")]
    [SerializeField]
    private List<ChatStatus> ChatStatusList;

    [Header("Blur Object")]
    [SerializeField]
    private GameObject blurObject;

    // 내부에서 (popupName -> popupObject) 매핑
    public Dictionary<string, GameObject> popupDict;

    public bool blurAble = true;
    public bool chatOn = false;

    private void Awake()
    {
        // 리스트를 딕셔너리로 변환
        popupDict = new Dictionary<string, GameObject>();
        foreach (var item in popupList)
        {
            if (!popupDict.ContainsKey(item.popupName))
            {
                popupDict.Add(item.popupName, item.popupObject);
            }
            else
            {
                Debug.LogWarning($"Duplicate popupName: {item.popupName}");
            }
        }
    }

    void Start()
    {
        SoundContainer.soundManager.Play("달담뮤직_오렌지");
    }
    /// <summary>
    /// popupName으로 지정한 팝업을 열기
    /// </summary>
    public void OpenPopUp(string popupName)
    {
        if (popupDict.ContainsKey(popupName))
        {
            if (popupName == "Chat_PopUp_Group" && chatOn)
            {
                ClosePopUp(popupName);
                chatOn = false;
                return;
            }

            ShowUI(popupDict[popupName]);
            if (popupName == "Chat_PopUp_Group")
            {
                chatOn = true;
            }
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {popupName}");
        }
    }

    /// <summary>
    /// popupName으로 지정한 팝업을 닫기
    /// </summary>
    public void ClosePopUp(string popupName)
    {
        if (popupDict.ContainsKey(popupName))
        {
            HideUI(popupDict[popupName]);

            if(popupName == "Chat_PopUp_Group")
            {
                chatOn = false;
            }
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {popupName}");
        }
    }

    public void OpenPopUpNotDot(string popupName)
    {
        if (popupDict.ContainsKey(popupName))
        {
            popupDict[popupName].SetActive(true);
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {popupName}");
        }
    }
    public void ClosePopUpNotDot(string popupName)
    {
        if (popupDict.ContainsKey(popupName))
        {
            popupDict[popupName].SetActive(false);
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {popupName}");
        }
    }

    /// <summary>
    /// 모든 팝업을 비활성화
    /// </summary>
    public void CloseAllPopUps()
    {
        foreach (var kvp in popupDict)
        {
            kvp.Value.SetActive(false);
        }
    }

    public void BlurOn()
    {
        if (blurAble)
            blurObject.SetActive(true);
    }

    public void BlurOff()
    {
        if (blurAble)
            blurObject.SetActive(false);
    }

    public void BlurBoolStatus(bool OnOff)
    {
        blurAble = OnOff;
    }

    public void ChatUIStatus()
    {
        if (!chatOn)
        {
            ChatStatusList[0].ShadowObj.SetActive(false);
            ChatStatusList[0].IconObj.SetActive(false);

            ChatStatusList[1].ShadowObj.SetActive(true);
            ChatStatusList[1].IconObj.SetActive(true);

            SoundContainer.soundManager.Play("chat_Alert_Sound");
        }
        else
        {
            ChatStatusList[0].ShadowObj.SetActive(true);
            ChatStatusList[0].IconObj.SetActive(true);

            ChatStatusList[1].ShadowObj.SetActive(false);
            ChatStatusList[1].IconObj.SetActive(false);
        }
    }
}
