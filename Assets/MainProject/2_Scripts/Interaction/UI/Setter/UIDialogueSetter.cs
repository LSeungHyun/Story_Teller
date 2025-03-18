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
        bgTypeSpritesCache[BgType.�⺻] = defaultImg;
        bgTypeSpritesCache[BgType.����] = monoImg;
        bgTypeSpritesCache[BgType.��󵵷�] = saraImg;
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
            if (bgType == BgType.�⺻ || bgType == BgType.��󵵷�)
            {
                closeBtn.sprite = blackCloseBtn;
                textDisplay.color = new Color32(35, 35, 35, 255);
            }
            else if (bgType == BgType.����)
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
