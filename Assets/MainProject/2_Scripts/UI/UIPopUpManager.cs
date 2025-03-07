using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.U2D.ScriptablePacker;

public class UIPopUpManager : UIContentsManager
{
    [SerializeField] public DialogueContainer dialogueContainer;
    [SerializeField] private ImageContainer imageContainer;
    private Dictionary<BgType, Sprite> bgTypeSpritesCache = new Dictionary<BgType, Sprite>();


    public Button backBtn;
    public Button nextBtn;

    public DialogueList[] dialogueList;
    public Text textDisplay;

    public ImageList[] imageList;
    public SpriteRenderer spriteDisplay;
    public Sprite[] spriteData;

    public Sprite defaultImg;
    public Sprite monoImg;
    public Sprite saraImg;

    public event Action<int, int> OnPageChanged;
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
        UpdateNavigationButtons(currentPage, totalPages);
    }

    public void UpdateNavigationButtons(int currentPage, int totalPages)
    {
        if (backBtn != null)
            backBtn.gameObject.SetActive(currentPage > 1);
        if (nextBtn != null)
            nextBtn.gameObject.SetActive(currentPage < totalPages);
    }

    public void NextPage()
    {
        if (currentDataPage < totalDataPage)
        {
            currentDataPage++;
            DisplayPage();  
        }
    }

    public void BackPage()
    {
        if (currentDataPage > 1)
        {
            currentDataPage--;
            DisplayPage(); 
        }
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

        if (imageContainer != null && imageContainer.imageDatas != null)
        {
            var imageData = imageContainer.imageDatas.FirstOrDefault(r => r.objCode == currentObjCode);
            if (imageData != null)
            {
                imageList = imageData.dataList;
                spriteData = SetImage(imageList);
                totalDataPage = spriteData.Length;
            }
        }


        Debug.Log("totalDataPage: " + totalDataPage);
        currentDataPage = 1;
        DisplayPage();
    }
    private void ManualCacheSprites()
    {
        bgTypeSpritesCache[BgType.기본] = defaultImg;
        bgTypeSpritesCache[BgType.독백] = monoImg;
        bgTypeSpritesCache[BgType.사라도령] = saraImg;
    }


    public override void DisplayPage()
    {

        if (spriteData != null && spriteData.Length > 0)
        {
            spriteDisplay.sprite = spriteData[currentDataPage - 1];
        }


        if (dialogueList != null && dialogueList.Length > 0)
        {
            textDisplay.text = dialogueList[currentDataPage - 1].textData;
            BgType bgType = dialogueList[currentDataPage - 1].bgType;

            spriteDisplay.sprite = bgTypeSpritesCache[bgType];
        }

        OnPageChanged?.Invoke(currentDataPage, totalDataPage);
        Debug.Log(currentDataPage + "," + totalDataPage);
    }


    public override void ClearData()
    {
        dialogueList = new DialogueList[0];
        if (textDisplay != null)
            textDisplay.text = "";

        spriteData = new Sprite[0];
        if (spriteDisplay != null)
            spriteDisplay.sprite = null;
    }

    private Sprite[] SetImage(ImageList[] imageList)
    {
        Sprite[] sprites = new Sprite[imageList.Length];

        for (int i = 0; i < imageList.Length; i++)
        {
            Sprite sprite = Resources.Load<Sprite>(imageList[i].imageData);
            if (sprite == null)
            {
                Debug.LogWarning("Failed to load sprite: " + imageList[i].imageData);
            }
            sprites[i] = sprite;
        }

        return sprites;
    }
}
