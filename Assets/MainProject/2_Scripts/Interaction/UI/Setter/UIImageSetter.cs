using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIImageSetter : UIPopUpManager
{
    [SerializeField] private ImageContainer imageContainer;

    public Image defaultSpriteDisplay;
    public RectTransform backgroundRect;
    public RectTransform closeButtonRect;
    public ImageList[] imageLists;

    public string currentObjCode;

    public override void SetData(string currentObjCode)
    {
        imageLists = imageContainer?.imageDatas?.FirstOrDefault(data => data.objCode == currentObjCode)?.dataList;
        currentDataPage = 1;
        totalDataPage = imageLists?.Length ?? 0;
        DisplayPage();
    }

    public override void DisplayPage()
    {
        if (imageLists == null || imageLists.Length == 0) return;

        Sprite currentSprite = LoadSprite(imageLists[currentDataPage - 1]);
        defaultSpriteDisplay.sprite = currentSprite;

        ResizeImageAndBackground(currentSprite);

        StartOnPageChanged(currentDataPage, totalDataPage);
    }

    public override void UpdateNavigationButtons(int currentPage, int totalPages)
    {
        base.UpdateNavigationButtons(currentPage, totalPages);
    }

    public override void ClearData()
    {
        imageLists = null;
        if (defaultSpriteDisplay != null)
            defaultSpriteDisplay.sprite = null;
    }

    private Sprite LoadSprite(ImageList imageInfo)
    {
        if (imageInfo == null || string.IsNullOrEmpty(imageInfo.imageData)) return null;

        Sprite sprite = Resources.Load<Sprite>(imageInfo.imageData);
        if (sprite == null)
            Debug.LogWarning($"Failed to load sprite: {imageInfo.imageData}");
        return sprite;
    }

    private void ResizeImageAndBackground(Sprite sprite)
    {
        if (sprite == null) return;

        RectTransform imageRect = defaultSpriteDisplay.GetComponent<RectTransform>();

        float originalWidth = sprite.rect.width;
        float originalHeight = sprite.rect.height;

        const float maxWidth = 1024f;
        const float maxHeight = 720f;

        float widthScale = maxWidth / originalWidth;
        float heightScale = maxHeight / originalHeight;
        float scale = Mathf.Min(1f, Mathf.Min(widthScale, heightScale));

        float scaledWidth = originalWidth * scale;
        float scaledHeight = originalHeight * scale;

        imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaledWidth);
        imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scaledHeight);

        if (backgroundRect != null)
        {
            float bgWidth = scaledWidth + 60f;
            float bgHeight = scaledHeight + 132f;

            backgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bgWidth);
            backgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bgHeight);

            imageRect.SetParent(backgroundRect);
            imageRect.anchorMin = imageRect.anchorMax = new Vector2(0.5f, 0.5f);
            imageRect.pivot = new Vector2(0.5f, 0.5f);
            imageRect.anchoredPosition = new Vector2(0f, 36f);

            if (closeButtonRect != null)
            {
                closeButtonRect.SetParent(backgroundRect);
                closeButtonRect.anchorMin = closeButtonRect.anchorMax = new Vector2(0.5f, 0f);
                closeButtonRect.pivot = new Vector2(0.5f, 0.5f);
                closeButtonRect.anchoredPosition = Vector2.zero;
            }
        }
    }

    public void onCloseBtnForDone()
    {
        UINextSetter.Instance.AddPlayerToDoneList(currentObjCode);
    }
}
