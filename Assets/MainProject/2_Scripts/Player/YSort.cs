using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    [Tooltip("�⺻ ���̾� �̸�. ���� 'Default' ���� �� ���ϴ�.")]
    public string sortingLayerName = "Default";

    [Tooltip("���� �ΰ���: 100���� ������ y=1.23 �� order = -123")]
    public int sortOrderFactor = 100;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        // ���̾�� �� ���� ����
        sr.sortingLayerName = sortingLayerName;
    }

    void LateUpdate()
    {
        // y ��ǥ�� ��������(ȭ�� �Ʒ��ϼ���) order�� �������� �տ� �׷����ϴ�.
        int order = Mathf.RoundToInt(-transform.position.y * sortOrderFactor);
        sr.sortingOrder = order;
    }
}
