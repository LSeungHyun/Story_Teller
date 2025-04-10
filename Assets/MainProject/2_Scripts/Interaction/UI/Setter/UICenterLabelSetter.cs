using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICenterLabelSetter : UIContentsManager
{
    [SerializeField] public CenterLabelContainer centerLabelContainer;

    public SoundContainer soundContainer;
    public ManagerConnector managerConnector;
    public UICenterLabelOnOffManager uiCenterLabelOnOffManager;
    public UINextSetter uiNextSetter;

    public CenterLabelList[] centerLabelList;
    public Text textDisplay;

    public int closeTime = 0;
    public string currentObjCode;

    private void Awake()
    {
        managerConnector.uICenterLabelSetter = this;
    }
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
        if(CurrentObjectManager.Instance.newObjCode != null)
        {
            CurrentObjectManager.Instance.newObjCode = "";
        }
        
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
                uiCenterLabelOnOffManager.CloseCenterLabelWindow();
                UINextSetter.Instance.ProcessNextCode(currentObjCode);
            }
        }
    }
}
