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

    [Header("PopUp List")]
    [SerializeField]
    private List<PopupItem> popupList;

    // ���ο��� (popupName -> popupObject) ����
    public Dictionary<string, GameObject> popupDict;

    private void Awake()
    {
        // ����Ʈ�� ��ųʸ��� ��ȯ
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
    /// popupName���� ������ �˾��� ����
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
    /// popupName���� ������ �˾��� �ݱ�
    /// </summary>
    public void ClosePopUp(string popupName)
    {
        if (popupDict.ContainsKey(popupName))
        {
            HideUI(popupDict[popupName]);
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
    /// ��� �˾��� ��Ȱ��ȭ
    /// </summary>
    public void CloseAllPopUps()
    {
        foreach (var kvp in popupDict)
        {
            kvp.Value.SetActive(false);
        }
    }
}
