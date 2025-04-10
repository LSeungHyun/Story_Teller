using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;

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
    public GameObject DarkObject;
    public GameObject BlurObject;

    [Header("UI Button Group")]
    public GameObject UI_Button_Group;

    public Dictionary<string, PopUp_Group> popUpDict;
    public Dictionary<string, Panel_Group> panelDict;

    public bool blurAble = true;
    public bool chatOn = false;

    [SerializeField]
    private bool NotEvent = false;

    [SerializeField]
    private bool isMove = false;

    public bool rapidClose = false;

    void Awake()
    {
        managerConnector.uiManager = this;
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

    public void CutSceneOnOff(bool active)
    {
        UI_Button_Group.SetActive(active);
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
        rapidClose = false;

        CloseAllPopUps();
        session.ChangePlayerisMoved(managerConnector.playerManager, false, false);

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

    public void RapidCloseAllUI()
    {
        rapidClose = true;

        CloseAllPanels();
        CloseAllPopUps();
        BlurOnOff(false);
        Master_Panel.SetActive(false);
    }

    /// <summary>
    /// PopUp_Name으로 지정한 팝업을 닫기
    /// </summary>
    public void ClosePopUp(string popUp_Name)
    {
        if (popUpDict.TryGetValue(popUp_Name, out PopUp_Group popUp))
        {
            if (popUp.isAnim && !rapidClose)
            {
                HideUI(popUp.PopUp_Obj);
            }
            else
            {
                popUp.PopUp_Obj.SetActive(false);
            }

            popUp.isActive = false;

            if (rapidClose)
            {
                return;
            }
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {popUp_Name}");
        }

        if (isMove)
        {
            session.ChangePlayerisMoved(managerConnector.playerManager, true, false);
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

    #region 패널 관련된 기능

    /// <summary>
    /// panel_Name으로 지정한 팝업을 열기
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
                    ClosePanel(panel_Name);
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
        rapidClose = false;

        CloseAllPanels();

        yield return new WaitForSeconds(0.25f);

        Master_Panel.SetActive(true);
        DarkObject.SetActive(false);

        if (panelDict.TryGetValue(panel_Name, out Panel_Group panel))
        {
            if (panel.isAnim && !rapidClose)
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
    /// panel_Name으로 지정한 팝업을 닫기
    /// </summary>
    public void ClosePanel(string panel_Name)
    {
        StartCoroutine(PanelCloseCoroutine(panel_Name));
    }

    IEnumerator PanelCloseCoroutine(string panel_Name)
    {
        if (panelDict.TryGetValue(panel_Name, out Panel_Group panel))
        {
            if (panel.isAnim && !rapidClose)
            {
                HideUI(panel.Panel_Obj);
            }
            else
            {
                panel.Panel_Obj.SetActive(false);

                if (rapidClose)
                {
                    yield break;
                }
            }
        }
        else
        {
            Debug.LogWarning($"No popup found with name: {panel_Name}");
        }

        yield return new WaitForSeconds(0.25f);

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

    public void ChatBool()
    {
        popUpDict.TryGetValue("Chat_PopUp_Group", out PopUp_Group popUp);

        chatOn = popUp.isActive;
        
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

    public void SceneMove(string sceneName)
    {
        //ExitGame();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        //#if UNITY_EDITOR
        //        // 에디터에서는 플레이 모드 종료
        //        EditorApplication.isPlaying = false;
        //#elif UNITY_WEBGL && !UNITY_EDITOR
        //            // WebGL에서는 창 닫기를 시도합니다.
        //            // 주의: 대부분의 브라우저는 스크립트로 열린 창만 window.close()를 허용합니다.
        //            QuitWebGL();
        //#else
        //            // 빌드된 게임에서는 일반적으로 Application.Quit() 사용
        //            Application.Quit();
        //#endif
        //    }

        //#if UNITY_WEBGL && !UNITY_EDITOR
        //    [DllImport("__Internal")]
        //    private static extern void QuitWebGL();
        //#endif
    }
}
