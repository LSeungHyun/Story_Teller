using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UITextSetter : UIContentsManager
{
    [SerializeField] public DialogueContainer dialogueContainer;
    public DialogueList[] dialogueList;
    public Text textDisplay;

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
        dialogueList = dialogueContainer.dialogueDatas.FirstOrDefault(r => r.objCode == currentObjCode).dataList;
        totalDataPage = (dialogueList != null) ? dialogueList.Length : 0;
        currentDataPage = 1;
        DisplayPage();
    }

    public override void ClearData()
    {
        dialogueList = new DialogueList[0];
        if (textDisplay != null)
            textDisplay.text = "";
    }

    public void ChatDataJson()
    {

    }

    protected override void DisplayPageContent()
    {
        if (dialogueList != null && dialogueList.Length > 0)
        {
            textDisplay.text = dialogueList[currentDataPage - 1].textData;
        }
    }
}
