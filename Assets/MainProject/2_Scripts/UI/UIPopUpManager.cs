using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPopUpManager : MonoBehaviour
{
    public string objCode;
    public GameObject popUpGroup;
    public GameObject dialoguePopUpGroup;
    public Text text;
    public Button backBtn;
    public Button nextBtn;

    public string[] textData;
    public int totalTextDataPage;
    public int currentTextPage = 1;

    public List<Sprite> spriteData;

    public RowData nextObjCode;

    public void OpenPopUpWindow(RowData targetRow)
    {
        popUpGroup.SetActive(true);
        dialoguePopUpGroup.SetActive(true);
        textData = targetRow.dataList;
        totalTextDataPage = targetRow.dataList.Length;
        currentTextPage = 1;
        SetPage();
    }

    public void SetNextCode(RowData targetRow)
    {
        nextObjCode = targetRow;
    }
    public void ClosePopUpWindow()
    {
        // 1) GameManager에서 현재 세션(IGameSession) 객체를 가져옴
        var session = GameManager.Instance.Session;
        // 2) 세션에게 팝업 닫기 로직 위임
        session.ClosePopUp(this);
    }
    //public void ClosePopUpWindow()
    //{
    //    popUpGroup.SetActive(false);
    //    dialoguePopUpGroup.SetActive(false);
    //    if(nextObjCode.IsNextObj != null)
    //    {
    //        OpenPopUpWindow(nextObjCode);
    //        nextObjCode = null;
    //    }
    //}


    public void OpenImage(List<Sprite> spriteList)
    {
        spriteData = spriteList;
    }

    public void NextPage()
    {
        if(currentTextPage < totalTextDataPage) { currentTextPage++; }
        SetPage();
    }

    public void BackPage()
    {
        if(currentTextPage > 1) { currentTextPage--; }
        SetPage();
    }

    public void SetPage()
    {
        text.text = textData[currentTextPage-1];
        backBtn.gameObject.SetActive(currentTextPage > 1);
        nextBtn.gameObject.SetActive(currentTextPage < totalTextDataPage);
    }
}
