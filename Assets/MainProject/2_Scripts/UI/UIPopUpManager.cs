using UnityEngine;
using UnityEngine.UI;

public class UIPopUpManager : MonoBehaviour
{
    public string objCode;
    public GameObject popUpGroup;
    public Text text;

    public string[] textData;
    public int totalTextDataPage;
    public int currentTextPage;


    public void OpenPopUpWindow(RowData targetRow)
    {
        popUpGroup.SetActive(true);
        textData = targetRow.textData;
        totalTextDataPage = targetRow.textData.Length;
        currentTextPage = 0;
        SetPage();
    }
    public void NextPage()
    {
        if(currentTextPage < totalTextDataPage - 1) { currentTextPage++; }
        SetPage();
    }

    public void BackPage()
    {
        if(currentTextPage > 0) { currentTextPage--; }
        SetPage();
    }

    public void SetPage()
    {
        text.text = textData[currentTextPage];
    }
}
