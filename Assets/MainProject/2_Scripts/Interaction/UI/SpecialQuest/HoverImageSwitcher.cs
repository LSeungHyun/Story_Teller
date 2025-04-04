using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverImageSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image popUpImage;
    [SerializeField] private Sprite hoverSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        popUpImage.sprite = hoverSprite;
        popUpImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        popUpImage.gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (popUpImage != null && hoverSprite != null)
        {
            popUpImage.sprite = hoverSprite;
            popUpImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (popUpImage != null)
        {
            popUpImage.gameObject.SetActive(false);
        }
    }
}

