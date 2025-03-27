using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragManager : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform dragArea;
    private RectTransform myRect;        // �ڽ��� RectTransform���� ĳ��
    private Transform originalParent;
    private Vector2 pointerOffset;

    private void Awake()
    {
        // �̸� RectTransform ĳ��
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

        // �巡�� ���� ��ǥ�迡�� ���콺 ��ġ�� ���, ���� anchoredPosition�� ���� �������� ���
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

        // ���� ��ġ �״�� �̵��ϱ� ���� offset ����
        var targetPos = localPoint - pointerOffset;

        // dragArea ���� �� �̵� ����
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
