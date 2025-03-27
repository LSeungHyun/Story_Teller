using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragManager : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform dragArea;
    private RectTransform myRect;        // 자신을 RectTransform으로 캐시
    private Transform originalParent;
    private Vector2 pointerOffset;

    private void Awake()
    {
        // 미리 RectTransform 캐싱
        myRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        myRect.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = myRect.parent;
        myRect.SetParent(dragArea);

        // 드래그 영역 좌표계에서 마우스 위치를 얻고, 현재 anchoredPosition과 비교해 오프셋을 계산
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragArea, eventData.position, eventData.pressEventCamera, out var pointerLocal
        );
        pointerOffset = pointerLocal - myRect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragArea, eventData.position, eventData.pressEventCamera, out var localPoint
        );

        // 잡은 위치 그대로 이동하기 위해 offset 보정
        var targetPos = localPoint - pointerOffset;

        // dragArea 범위 내 이동 제한
        if (dragArea.rect.Contains(targetPos))
        {
            myRect.anchoredPosition = targetPos;
        }
        else if (dragArea.rect.Contains(new Vector2(dragArea.rect.center.x, targetPos.y)))
        {
            myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, targetPos.y);
        }
        else if (dragArea.rect.Contains(new Vector2(targetPos.x, dragArea.rect.center.y)))
        {
            myRect.anchoredPosition = new Vector2(targetPos.x, myRect.anchoredPosition.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        myRect.SetParent(originalParent);
        myRect.SetAsLastSibling();
    }
}
