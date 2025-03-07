using System.Linq;
using UnityEngine;

public class UIImageSetter : UIContentsManager
{
    [SerializeField] private ImageContainer imageContainer;
    public ImageList[] imageList;
    public SpriteRenderer spriteDisplay;
    public Sprite[] spriteData;

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
        imageList = imageContainer.imageDatas.FirstOrDefault(r => r.objCode == currentObjCode).dataList;
        spriteData = SetImage(imageList);
        totalDataPage = (imageList != null) ? imageList.Length : 0;
        currentDataPage = 1;
        DisplayPage();
    }

    public override void ClearData()
    {
        spriteData = new Sprite[0];
        if (spriteDisplay != null)
            spriteDisplay.sprite = null;
    }

    protected override void DisplayPageContent()
    {
        if (spriteData != null && spriteData.Length > 0)
        {
            spriteDisplay.sprite = spriteData[currentDataPage - 1];
        }
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
