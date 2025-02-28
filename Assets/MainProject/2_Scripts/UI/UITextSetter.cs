using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UITextSetter : UIContentsManager
{
    [SerializeField] public DialogueContainer dialogueContainer;
    public DialogueData targetRow;
    public Text textDisplay;
    public string[] textData;

    public void SetTextData(string currentObjCode)
    {
        targetRow = dialogueContainer.dialogueDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        
        textData = targetRow.dataList;
        totalDataPage = textData != null ? textData.Length : 0;
        currentDataPage = 1;
        DisplayPage();
    }
    public override void ClearData()
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
