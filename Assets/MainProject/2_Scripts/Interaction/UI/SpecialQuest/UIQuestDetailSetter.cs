using UnityEngine;

public class UIQuestDetailSetter : MonoBehaviour
{
    public RectTransform backgroundRect;
    public GameObject currentPageDetailDisplayInstance;
    public RectTransform closeButtonRect;

    public void SetQuestDetail(GameObject prefab)
    {
        if (currentPageDetailDisplayInstance != null)
        {
            Destroy(currentPageDetailDisplayInstance);
        }

        currentPageDetailDisplayInstance = Instantiate(prefab, backgroundRect);
        ResizeImageAndBackground(currentPageDetailDisplayInstance.GetComponent<RectTransform>());
    }

    private void ResizeImageAndBackground(RectTransform contentRect)
    {
        if (contentRect == null) return;

        Canvas.ForceUpdateCanvases();

        float originalWidth = contentRect.rect.width;
        float originalHeight = contentRect.rect.height;

        if (originalHeight <= 0f) return;

        float maxWidth = 1500f;
        float maxHeight = 854f;

        float scaleFactor = 1f;

        bool tooWide = originalWidth > maxWidth;
        bool tooTall = originalHeight > maxHeight;

        if (tooWide || tooTall)
        {
            float widthRatio = maxWidth / originalWidth;
            float heightRatio = maxHeight / originalHeight;
            scaleFactor = Mathf.Min(widthRatio, heightRatio);
        }

        contentRect.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

        float bgWidth = originalWidth * scaleFactor + 360f;
        float bgHeight = originalHeight * scaleFactor + 120f;

        backgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bgWidth);
        backgroundRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bgHeight);

        if (closeButtonRect != null)
        {
            closeButtonRect.SetParent(backgroundRect);
            closeButtonRect.anchorMin = closeButtonRect.anchorMax = new Vector2(1f, 1f);
            closeButtonRect.pivot = new Vector2(1f, 1f);
            closeButtonRect.anchoredPosition = new Vector2(-20f, -20f);
        }
    }
}
