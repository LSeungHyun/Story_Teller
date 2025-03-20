using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICenterLabelSetter : UIContentsManager
{
    [SerializeField] public CenterLabelContainer centerLabelContainer;

    public SoundContainer soundContainer;
    public ManagerConnector managerConnector;
    public UICenterLabelOnOffManager uiCenterLabelOnOffManager;

    public CenterLabelList[] centerLabelList;
    public Text textDisplay;

    public int closeTime = 0;

    public override void SetData(string currentObjCode)
    {
        centerLabelList = centerLabelContainer.centerLabelDatas.FirstOrDefault(r => r.objCode == currentObjCode).dataList;
        totalDataPage = centerLabelList != null ? centerLabelList.Length : 0;
        currentDataPage = 1;
        DisplayPage();
    }

    public override void ClearData()
    {
        centerLabelList = new CenterLabelList[0];
        if (textDisplay != null)
            textDisplay.text = "";
        CancelInvoke("AdvancePageOrClose");
    }

    public override void DisplayPage()
    {
        if (centerLabelList != null && centerLabelList.Length > 0)
        {
            textDisplay.text = centerLabelList[currentDataPage - 1].centerLabelData;
            closeTime = centerLabelList[currentDataPage - 1].closeTime;
            CancelInvoke("AdvancePageOrClose");
            Invoke("AdvancePageOrClose", closeTime);
        }
    }
    private void AdvancePageOrClose()
    {
        if (currentDataPage < totalDataPage)
        {
            currentDataPage++;
            DisplayPage(); 
        }

        else
        {
            if (uiCenterLabelOnOffManager != null)
            {
                //플레이어 이동 제한걸기
                uiCenterLabelOnOffManager.CloseAndCheckCenterLabelWindow();
            }
        }
    }
}
