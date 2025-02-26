using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UITextSetter : UIContentsManager
{
    [SerializeField] private DialogueContainer dialogueContainer;

    public Text textDisplay;
    public string[] textData;

    public void SetTextData(string currentObjCode)
    {
        DialogueData targetRow = dialogueContainer.dialogueDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        
        textData = targetRow.dataList;
        totalDataPage = (targetRow.dataList != null) ? targetRow.dataList.Length : 0;
        currentDataPage = 1;
        DisplayPage();
    }
    public void ClearTextData()
    {
        textData = new string[0];
        if (textDisplay != null)
            textDisplay.text = "";
    }

    public override void DisplayPage()
    {
        if (textData != null && textData.Length > 0)
        {
            textDisplay.text = textData[currentDataPage - 1];
            UpdateNavigationButtons();
        }
    }
}
