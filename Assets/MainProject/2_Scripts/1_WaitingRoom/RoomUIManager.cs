using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomUIManager : DoTweenManager
{
    [System.Serializable]
    public class PopupItem
    {
        public string popupName;     // "LobbyGroup", "Single_Multi_Select", ...
        public GameObject popupObject;
    }

    [Header("PopUp List")]
    [SerializeField]
    private List<PopupItem> popupList;

    // 내부에서 (popupName -> popupObject) 매핑
    private Dictionary<string, GameObject> popupDict;

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

    /// <summary>
    /// popupName으로 지정한 팝업을 열기
    /// </summary>
    public void OpenPopUp(string popupName)
    {
        if (popupDict.ContainsKey(popupName))
        {
            ShowUI(popupDict[popupName]);
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
            //popupDict[popupName].SetActive(false);
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
}
