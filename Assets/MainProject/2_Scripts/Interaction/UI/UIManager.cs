using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

public class UIManager : DoTweenManager
{
    public ManagerConnector managerConnector;
    public AbsctractGameSession session;

    [System.Serializable]

    public class PopUp_Group
    {
        public string PopUp_Name;
        public GameObject PopUp_Obj;
        public bool isAnim;
        public bool isActive;
    }

    [System.Serializable]
    public class Panel_Group
    {
        public string Panel_Name;
        public GameObject Panel_Obj;
        public bool isAnim;
        public bool isActive;
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
    private GameObject Master_Panel;

    [SerializeField]
    private List<Panel_Group> Panel_List;

    [Header("OnOffChat List")]
    [SerializeField]
    private List<ChatStatus> ChatStatusList;

    [Header("Blur Object")]
    [SerializeField]
    private GameObject DarkObject;
    [SerializeField]
    private GameObject BlurObject;

    public Dictionary<string, PopUp_Group> popUpDict;
    public Dictionary<string, Panel_Group> panelDict;

    public bool blurAble = true;
    public bool chatOn = false;

    [SerializeField]
    private bool NotEvent = false;

    [SerializeField]
    private bool isMove = false;

    void Awake()
    {
        SetPopUpDict();
        SetPanelDict();
    }

    void Start()
    {
        session = GameManager.Instance.Session;
    }


    public void BlurOnOff(bool active)
    {
        DarkObject.SetActive(active);
        BlurObject.SetActive(active);
    }

    public void PlayerMovementControl(bool isMove)
    {
        if (isMove)
        {
            session.ChangePlayerisMoved(managerConnector.playerManager, true, false);
        }
        else
        {
            session.ChangePlayerisMoved(managerConnector.playerManager, false, false);
        }
        
    }

    #region ��ųʸ� ������ ���� / �˾�, �г�

    /// <summary>
    /// Dictionary�� Class�� PopUp_Group ��ü�� �־���
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

    #region �˾� ���õ� ���

    /// <summary>
    /// popUp_Name���� ������ �˾��� ����
    /// </summary>
    public void OpenPopUp(string popUp_Name)
    {
        if (!NotEvent)
        {
            ClickAnim();

            if (popUpDict.TryGetValue(popUp_Name, out PopUp_Group popUp))
            {
                if (popUp.isActive)
                {
                    isMove = true;
                    ClosePopUp(popUp_Name);
                }
                else
                {
                    isMove = false;
                    StartCoroutine(PopUpCoroutine(popUp_Name));
                }
            }
        }
    }

    IEnumerator PopUpCoroutine(string popUp_Name)
    {
        NotEvent = true;

        CloseAllPopUps();
        PlayerMovementControl(false);

        yield return new WaitForSeconds(0.25f);

        BlurOnOff(true);

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

            popUp.isActive = true;
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {popUp_Name}");
        }

        NotEvent = false;
    }

    /// <summary>
    /// PopUp_Name���� ������ �˾��� �ݱ�
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

            popUp.isActive = false;
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {popUp_Name}");
        }

        if (isMove)
        {
            PlayerMovementControl(true);
        }

        BlurOnOff(false);
    }

    public void CloseAllPopUps()
    {
        foreach (var kvp in popUpDict)
        {
            ClosePopUp(kvp.Key);
        }

        isMove = true;
    }

    #endregion

    #region �г� ���õ� ���

    /// <summary>
    /// panel_Name���� ������ �˾��� ����
    /// </summary>
    public void OpenPanel(string panel_Name)
    {
        if (!NotEvent)
        {
            ClickAnim();

            if (panelDict.TryGetValue(panel_Name, out Panel_Group panel))
            {
                if (panel.isActive)
                {
                    ClosePopUp(panel_Name);
                }
                else
                {
                    StartCoroutine(PanelCoroutine(panel_Name));
                }
            }
        }
    }

    IEnumerator PanelCoroutine(string panel_Name)
    {
        NotEvent = true;

        CloseAllPanels();

        yield return new WaitForSeconds(0.25f);

        Master_Panel.SetActive(true);
        DarkObject.SetActive(false);

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

        NotEvent = false;
    }

    /// <summary>
    /// panel_Name���� ������ �˾��� �ݱ�
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

        Master_Panel.SetActive(false);
        DarkObject.SetActive(true);
    }

    public void CloseAllPanels()
    {
        foreach (var kvp in panelDict)
        {
            ClosePanel(kvp.Key);
        }
    }

    #endregion

    public void ChatBool(bool chatStatus)
    {
        chatOn = chatStatus;
        if (chatOn)
        {
            ChatUIStatus();
        }
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
