using UnityEngine;
using UnityEngine.UI;

public class UITextOverflowChecker : MonoBehaviour
{
    public RectTransform masterRect;
    public RectTransform verticalLayoutGroup;

    public Text uiText; // 할당된 UI Text 컴포넌트

    private RectTransform textRect;

    void Start()
    {
        textRect = uiText.GetComponent<RectTransform>();
    }

    void Update()
    {
        var settings = uiText.GetGenerationSettings(textRect.rect.size);

        float preferredHeight = uiText.cachedTextGeneratorForLayout.GetPreferredHeight(uiText.text, settings);
        float containerHeight = textRect.rect.height;

        if (preferredHeight > containerHeight)
        {
            ReSizeRect(textRect, 236);
            LayoutRebuilder.ForceRebuildLayoutImmediate(verticalLayoutGroup);
        }
    }

    public void ReSizeRect(RectTransform Rect, float num)
    {
        float vertical = Rect.sizeDelta.y;
        vertical += num;

        Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vertical);
        masterRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (vertical/10)*3);
    }
}
