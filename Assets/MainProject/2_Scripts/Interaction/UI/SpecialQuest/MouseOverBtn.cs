using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverUIPopup : DoTweenManager, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject popupImage; // ���콺 ���� �� ��� �˾� �̹��� GameObject

    private void Awake()
    {
        // ������ �� �˾� �̹����� ��Ȱ��ȭ�մϴ�.
        if (popupImage != null)
            popupImage.SetActive(false);
    }

    // ���콺�� �ش� UI ���� ������ �˾� �̹��� Ȱ��ȭ
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (popupImage != null)
        {
            popupImage.SetActive(true);
        }
    }

    // ���콺�� UI���� ������ �˾� �̹��� ��Ȱ��ȭ
    public void OnPointerExit(PointerEventData eventData)
    {
        if (popupImage != null)
        {
            popupImage.SetActive(false);
        }
    }
}