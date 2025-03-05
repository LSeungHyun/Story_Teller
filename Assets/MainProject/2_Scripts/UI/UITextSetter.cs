using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UITextSetter : UIContentsManager
{
    [SerializeField] public DialogueContainer dialogueContainer;
    public DialogueData targetRow;
    public Text textDisplay;
    public string[] textData;

    // UIPopUpBtnManager를 인스펙터에서 할당 (이벤트를 통해 자동 업데이트됩니다)
    public UIPopUpBtnManager uiPopUpBtnManager;

    private void Awake()
    {
        OnPageChanged += HandlePageChanged;
    }

    private void OnDestroy()
    {
        OnPageChanged -= HandlePageChanged;
    }

    private void HandlePageChanged(int currentPage, int totalPages)
    {
        if (uiPopUpBtnManager != null)
        {
            uiPopUpBtnManager.UpdateNavigationButtons(currentPage, totalPages);
        }
    }

    public override void SetData(string currentObjCode)
    {
        targetRow = dialogueContainer.dialogueDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        if (targetRow == null)
            return;

        textData = targetRow.dataList;
        totalDataPage = (textData != null) ? textData.Length : 0;
        currentDataPage = 1;
        DisplayPage();
    }

    public override void ClearData()
    {
        textData = new string[0];
        if (textDisplay != null)
            textDisplay.text = "";
    }

    public void ChatDataJson()
    {

    }

    protected override void DisplayPageContent()
    {
        if (textData != null && textData.Length > 0)
        {
            textDisplay.text = textData[currentDataPage - 1];
        }
    }
}
