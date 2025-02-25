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
    public int currentPage = 1;

    public RowData nextObjCode;

    public void OpenPopUpWindow(RowData targetRow)
    {
        popUpGroup.SetActive(true);
        dialoguePopUpGroup.SetActive(true);
        textData = targetRow.dataList;
        totalDataPage = targetRow.dataList.Length;
        currentPage = 1;
        SetPage();
    }

    public void SetNextCode(RowData targetRow)
    {
        nextObjCode = targetRow;
    }
    public void ClosePopUpWindow()
    {
        popUpGroup.SetActive(false);
        dialoguePopUpGroup.SetActive(false);
        text.text = "";
        sprite.sprite = null;
        if (nextObjCode.IsNextObj != null)
        {
            OpenPopUpWindow(nextObjCode);
            nextObjCode = null;
        }
    }


    public void OpenImage(List<Sprite> spriteList)
    {
        popUpGroup.SetActive(true);
        dialoguePopUpGroup.SetActive(true);
        spriteData = spriteList;
        totalDataPage = spriteList.Count;
        currentPage = 1;
        SetPage();
    }

    public void NextPage()
    {
        if (currentPage < totalDataPage) { currentPage++; }
        SetPage();
    }

    public void BackPage()
    {
        if (currentPage > 1) { currentPage--; }
        SetPage();
    }

    public void SetPage()
    {
        if (textData.Length > 0)
            text.text = textData[currentPage - 1];
        if (spriteData.Count > 0)
            sprite.sprite = spriteData[currentPage - 1];
        backBtn.gameObject.SetActive(currentPage > 1);
        nextBtn.gameObject.SetActive(currentPage < totalDataPage);
    }
}
