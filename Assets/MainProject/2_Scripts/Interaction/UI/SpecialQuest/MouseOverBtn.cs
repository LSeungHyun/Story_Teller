using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverUIPopup : DoTweenManager, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject popupImage; // 마우스 오버 시 띄울 팝업 이미지 GameObject

    private void Awake()
    {
        // 시작할 때 팝업 이미지를 비활성화합니다.
        if (popupImage != null)
            popupImage.SetActive(false);
    }

    // 마우스가 해당 UI 위에 들어오면 팝업 이미지 활성화
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (popupImage != null)
        {
            popupImage.SetActive(true);
        }
    }

    // 마우스가 UI에서 나가면 팝업 이미지 비활성화
    public void OnPointerExit(PointerEventData eventData)
    {
        if (popupImage != null)
        {
            popupImage.SetActive(false);
        }
    }
}