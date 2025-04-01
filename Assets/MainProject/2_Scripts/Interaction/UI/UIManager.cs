using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class UIManager : DoTweenManager
{
    [System.Serializable]

    public class PopUp_Group
    {
        public string PopUp_Name;
        public GameObject PopUp_Obj;
        public bool isAnim;
    }

    [System.Serializable]
    public class Panel_Group
    {
        public string Panel_Name;
        public GameObject Panel_Obj;
        public bool isAnim;
    }

    [System.Serializable]
    public class ChatStatus
    {
        public string Status;
        public GameObject ShadowObj;
        public GameObject IconObj;
    }

    [Header("PopUp List")]
    [SerializeField]
    private List<PopUp_Group> PopUp_List;

    [Header("Panel List")]
    [SerializeField]
    private List<Panel_Group> Panel_List;

    [Header("OnOffChat List")]
    [SerializeField]
    private List<ChatStatus> ChatStatusList;

    [Header("Blur Object")]
    [SerializeField]
    private GameObject blurObject;

    public Dictionary<string, PopUp_Group> popUpDict;
    public Dictionary<string, Panel_Group> panelDict;

    public bool blurAble = true;
    public bool chatOn = false;

    void Awake()
    {
        SetPopUpDict();
        SetPanelDict();
    }

    void Start()
    {

    }

    #region 딕셔너리 정보값 세팅 / 팝업, 패널

    /// <summary>
    /// Dictionary에 Class인 PopUp_Group 자체를 넣었음
    /// </summary>
    public void SetPopUpDict()
    {
        popUpDict = new Dictionary<string, PopUp_Group>();

        foreach (var item in PopUp_List)
        {
            if (!popUpDict.ContainsKey(item.PopUp_Name))
            {
                popUpDict.Add(item.PopUp_Name, item);
            }
            else
            {
                Debug.LogWarning($"Duplicate PopUpName: {item.PopUp_Name}");
            }
        }
    }

    public void SetPanelDict()
    {
        panelDict = new Dictionary<string, Panel_Group>();

        foreach (var item in Panel_List)
        {
            if (!panelDict.ContainsKey(item.Panel_Name))
            {
                panelDict.Add(item.Panel_Name, item);
            }
            else
            {
                Debug.LogWarning($"Duplicate PanelName: {item.Panel_Name}");
            }
        }
    }
    #endregion

    #region 팝업 관련된 기능

    /// <summary>
    /// popUp_Name으로 지정한 팝업을 열기
    /// </summary>
    public void OpenPopUp(string popUp_Name)
    {
        CloseAllPopUps();

        if (popUpDict.TryGetValue(popUp_Name, out PopUp_Group popUp))
        {
            if (popUp.isAnim)
            {
                ShowUI(popUp.PopUp_Obj);
            }
            else
            {
                popUp.PopUp_Obj.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {popUp_Name}");
        }
    }

    /// <summary>
    /// panel_Name으로 지정한 팝업을 닫기
    /// </summary>
    public void ClosePopUp(string popUp_Name)
    {
        if (popUpDict.TryGetValue(popUp_Name, out PopUp_Group popUp))
        {
            if (popUp.isAnim)
            {
                HideUI(popUp.PopUp_Obj);
            }
            else
            {
                popUp.PopUp_Obj.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {popUp_Name}");
        }
    }

    public void CloseAllPopUps()
    {
        foreach (var kvp in popUpDict)
        {
            ClosePopUp(kvp.Key);
        }
    }

    #endregion

    #region 패널 관련된 기능

    /// <summary>
    /// panel_Name으로 지정한 팝업을 열기
    /// </summary>
    public void OpenPanel(string panel_Name)
    {
        if (panelDict.TryGetValue(panel_Name, out Panel_Group panel))
        {
            if (panel.isAnim)
            {
                ShowUI(panel.Panel_Obj);
            }
            else
            {
                panel.Panel_Obj.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {panel_Name}");
        }
    }

    /// <summary>
    /// panel_Name으로 지정한 팝업을 닫기
    /// </summary>
    public void ClosePanel(string panel_Name)
    {
        if (panelDict.TryGetValue(panel_Name, out Panel_Group panel))
        {
            if (panel.isAnim)
            {
                HideUI(panel.Panel_Obj);
            }
            else
            {
                panel.Panel_Obj.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {panel_Name}");
        }
    }

    public void CloseAllPanels()
    {
        foreach (var kvp in panelDict)
        {
            ClosePopUp(kvp.Key);
        }


    }

    #endregion
}
