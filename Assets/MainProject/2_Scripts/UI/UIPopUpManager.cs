using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPopUpManager : MonoBehaviour
{
    public KeyInputManager keyInputManager;
    
    public GameObject popUpGroup;
    public GameObject defaultPopUpGroup;
    public GameObject questPopUpGroup;
    public GameObject windowPopUp;

    public void OpenDefaultWindow()
    {
        popUpGroup.SetActive(true);
        defaultPopUpGroup.SetActive(true);
        windowPopUp.SetActive(true);
    }

    public void OpenQuestWindow()
    {
        popUpGroup.SetActive(true);
        questPopUpGroup.SetActive(true);
        windowPopUp.SetActive(true);
    }
    public void ClosePopUpWindow()
    {
        // 1) GameManager에서 현재 세션(IGameSession) 객체를 가져옴
        var session = GameManager.Instance.Session;
        // 2) 세션에게 팝업 닫기 로직 위임
        session.ClosePopUp(this);
    }
}
