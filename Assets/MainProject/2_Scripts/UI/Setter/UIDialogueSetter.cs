using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueSetter : UIPopUpManager
{
    [SerializeField] public DialogueContainer dialogueContainer;

    private Dictionary<BgType, Sprite> bgTypeSpritesCache = new Dictionary<BgType, Sprite>();

    public Sprite defaultImg;
    public Sprite monoImg;
    public Sprite saraImg;

    public Sprite whiteCloseBtn;
    public Sprite blackCloseBtn;

    public DialogueList[] dialogueList;

    public Text textDisplay;
    public Image spriteDisplay;
    public Image closeBtn;

    void ManualCacheSprites()
    {
        bgTypeSpritesCache[BgType.기본] = defaultImg;
        bgTypeSpritesCache[BgType.독백] = monoImg;
        bgTypeSpritesCache[BgType.사라도령] = saraImg;
    }

    public override void SetData(string currentObjCode)
    {
        ManualCacheSprites();
        if (dialogueContainer != null && dialogueContainer.dialogueDatas != null)
        {
            var dialogueData = dialogueContainer.dialogueDatas.FirstOrDefault(r => r.objCode == currentObjCode);
            if (dialogueData != null)
            {
                dialogueList = dialogueData.dataList;
                totalDataPage = dialogueList.Length;
            }
        }
        currentDataPage = 1;
        DisplayPage();
    }

    public override void DisplayPage()
    {
        if (dialogueList != null && dialogueList.Length > 0)
        {
            textDisplay.text = dialogueList[currentDataPage - 1].textData;
            BgType bgType = dialogueList[currentDataPage - 1].bgType;
            spriteDisplay.sprite = bgTypeSpritesCache[bgType];
            if (bgType == BgType.기본 || bgType == BgType.사라도령)
            {
                closeBtn.sprite = blackCloseBtn;
                textDisplay.color = new Color32(35, 35, 35, 255);
            }
            else if (bgType == BgType.독백)
            {
                closeBtn.sprite = whiteCloseBtn;
                textDisplay.color = new Color32(249, 235, 232, 255);
            }
        }
        StartOnPageChanged(currentDataPage, totalDataPage);
        Debug.Log(currentDataPage + "," + totalDataPage);
    }

    public override void ClearData()
    {
        dialogueList = new DialogueList[0];
        if (textDisplay != null)
            textDisplay.text = "";
    }
}
