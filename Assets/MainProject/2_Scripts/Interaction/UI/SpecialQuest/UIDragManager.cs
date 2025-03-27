using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragManager : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform dragArea; // 드래그할 영역 (보통 캔버스의 RectTransform)
    private Transform originalParent;

    // 마우스 클릭(터치) 시 호출: 원래 부모 내에서 맨 뒤로 보내어 화면상 가장 앞에 오게 함
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    // 드래그 시작 시 호출: 원래 부모(그리드) 정보를 저장하고, 드래그 전용 영역으로 이동
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(dragArea);
    }

    // 드래그 중: dragArea 좌표계를 기준으로 위치 업데이트
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragArea, eventData.position, eventData.pressEventCamera, out localPoint
        );

        // dragArea의 Rect 내에 있다면 그대로 이동
        if (dragArea.rect.Contains(localPoint))
        {
            transform.localPosition = localPoint;
        }
        // x 좌표가 범위를 벗어나고, y 좌표는 범위 내라면 y값만 업데이트
        else if (dragArea.rect.Contains(new Vector2(dragArea.rect.center.x, localPoint.y)))
        {
            transform.localPosition = new Vector2(transform.localPosition.x, localPoint.y);
        }
        // y 좌표가 범위를 벗어나고, x 좌표는 범위 내라면 x값만 업데이트
        else if (dragArea.rect.Contains(new Vector2(localPoint.x, dragArea.rect.center.y)))
        {
            transform.localPosition = new Vector2(localPoint.x, transform.localPosition.y);
        }
    }


    // 드래그 종료 시: 원래 부모로 복귀한 후, 원래 부모 내에서 맨 뒤로 설정하여 다른 오브젝트 위에 있도록 함
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.SetAsLastSibling();
    }
}
