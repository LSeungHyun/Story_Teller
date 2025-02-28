using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIImageSetter : UIContentsManager
{
    [SerializeField] private ImageContainer imageContainer;
    public ImageData targetRow;
    public SpriteRenderer spriteDisplay;
    public List<Sprite> spriteData;

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
        targetRow = imageContainer.imageDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        if (targetRow == null)
            return;

        spriteData = SetImage(targetRow);
        totalDataPage = (spriteData != null) ? spriteData.Count : 0;
        currentDataPage = 1;
        DisplayPage();
    }

    public override void ClearData()
    {
        spriteData = new List<Sprite>();
        if (spriteDisplay != null)
            spriteDisplay.sprite = null;
    }

    // 角力 能刨明 钎矫 肺流
    protected override void DisplayPageContent()
    {
        if (spriteData != null && spriteData.Count > 0)
        {
            spriteDisplay.sprite = spriteData[currentDataPage - 1];
        }
    }

    private List<Sprite> SetImage(ImageData targetRow)
    {
        var sprites = new List<Sprite>();

        foreach (string resource in targetRow.dataList)
        {
            Sprite sprite = Resources.Load<Sprite>(resource);
            if (sprite == null)
            {
                Debug.LogWarning("Failed to load sprite: " + resource);
            }
            sprites.Add(sprite);
        }

        return sprites;
    }
}
