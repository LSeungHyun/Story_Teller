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
    public SpriteRenderer sprite;
    public Button backBtn;
    public Button nextBtn;

    public string[] textData;
    public List<Sprite> spriteData;
    public int totalDataPage;
    public int currentDataPage = 1;

    public RowData nextObjCode;

    public void OpenPopUpWindow(DialogueData targetRow)
    {
        popUpGroup.SetActive(true);
        dialoguePopUpGroup.SetActive(true);
        textData = targetRow.dataList;
        totalDataPage = targetRow.dataList.Length;
        currentDataPage = 1;
        SetPage();
    }

    public void OpenImage(List<Sprite> spriteList)
    {
        popUpGroup.SetActive(true);
        dialoguePopUpGroup.SetActive(true);
        spriteData = spriteList;
        totalDataPage = spriteList.Count;
        currentDataPage = 1;
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


    public void NextPage()
    {
        if(currentDataPage < totalDataPage) { currentDataPage++; }
        SetPage();
    }

    public void BackPage()
    {
        if(currentDataPage > 1) { currentDataPage--; }
        SetPage();
    }

    public void SetPage()
    {
        text.text = textData[currentDataPage-1];
        sprite.sprite = spriteData[currentDataPage - 1];
        backBtn.gameObject.SetActive(currentDataPage > 1);
        nextBtn.gameObject.SetActive(currentDataPage < totalDataPage);
    }
}
