using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICenterLabelSetter : UIContentsManager
{
    [SerializeField] public CenterLabelContainer centerLabelContainer;
    public UICenterLabelOnOffManager uiCenterLabelOnOffManager;

    public CenterLabelData targetRow;
    public Text textDisplay;
    public string[] textData;

    public int closeTime = 0;

    public override void SetData(string currentObjCode)
    {
        targetRow = centerLabelContainer.centerLabelDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        if (targetRow == null)
            return;

        textData = targetRow.dataList;
        closeTime = targetRow.closeTime;
        totalDataPage = textData != null ? textData.Length : 0;
        currentDataPage = 1;
        DisplayPage();
    }

    public override void ClearData()
    {
        textData = new string[0];
        if (textDisplay != null)
            textDisplay.text = "";
        CancelInvoke("AdvancePageOrClose");
    }

    protected override void DisplayPageContent()
    {
        if (textData != null && textData.Length > 0)
        {
            textDisplay.text = textData[currentDataPage - 1];
            CancelInvoke("AdvancePageOrClose");
            Invoke("AdvancePageOrClose", closeTime);
        }
    }
    private void AdvancePageOrClose()
    {
        if (currentDataPage < totalDataPage)
        {
            NextPage(); 
        }
        else
        {
            if (uiCenterLabelOnOffManager != null)
            {
                uiCenterLabelOnOffManager.CloseCenterLabelWindow();
            }
        }
    }
}
