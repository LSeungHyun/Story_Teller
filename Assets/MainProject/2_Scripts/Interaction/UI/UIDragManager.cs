using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragManager : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform dragArea; // �巡���� ���� (���� ĵ������ RectTransform)
    private Transform originalParent;

    // ���콺 Ŭ��(��ġ) �� ȣ��: ���� �θ� ������ �� �ڷ� ������ ȭ��� ���� �տ� ���� ��
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    // �巡�� ���� �� ȣ��: ���� �θ�(�׸���) ������ �����ϰ�, �巡�� ���� �������� �̵�
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(dragArea);
    }

    // �巡�� ��: dragArea ��ǥ�踦 �������� ��ġ ������Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragArea, eventData.position, eventData.pressEventCamera, out localPoint
        );

        // dragArea�� Rect ���� �ִٸ� �״�� �̵�
        if (dragArea.rect.Contains(localPoint))
        {
            transform.localPosition = localPoint;
        }
        // x ��ǥ�� ������ �����, y ��ǥ�� ���� ����� y���� ������Ʈ
        else if (dragArea.rect.Contains(new Vector2(dragArea.rect.center.x, localPoint.y)))
        {
            transform.localPosition = new Vector2(transform.localPosition.x, localPoint.y);
        }
        // y ��ǥ�� ������ �����, x ��ǥ�� ���� ����� x���� ������Ʈ
        else if (dragArea.rect.Contains(new Vector2(localPoint.x, dragArea.rect.center.y)))
        {
            transform.localPosition = new Vector2(localPoint.x, transform.localPosition.y);
        }
    }


    // �巡�� ���� ��: ���� �θ�� ������ ��, ���� �θ� ������ �� �ڷ� �����Ͽ� �ٸ� ������Ʈ ���� �ֵ��� ��
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.SetAsLastSibling();
    }
}
