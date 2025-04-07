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
        float originalWidth = contentRect.rect.width;
        float originalHeight = contentRect.rect.height;

        if (originalHeight <= 0f) return;

        const float maxHeight = 854f;

        float scale = Mathf.Min(1f, maxHeight / originalHeight);
        float scaledHeight = originalHeight * scale;
        float scaledWidth = originalWidth * scale;

        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaledWidth);
        contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scaledHeight);

        float bgWidth = scaledWidth + 360f;
        float bgHeight = scaledHeight + 120f;

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
