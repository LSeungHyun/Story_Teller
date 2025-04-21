using UnityEngine;

[ExecuteAlways]
public class YSort : MonoBehaviour
{
    [Tooltip("�������� ����� Sorting Layer �̸�")]
    public string sortingLayerName = "Default";

    [Tooltip("���� �ΰ���: Ŭ���� Y�� ��ȭ�� ���� order ��ȭ�� Ů�ϴ�.")]
    public int sortOrderFactor = 100;

    [Tooltip("���� �Ҵ��� �켱���� SpriteRenderer")]
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        // �ν����Ϳ� �Ҵ�� �� ������, �켱 �ڱ� �ڽſ��� ã��
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        // �׷��� ������ �θ𿡼� ã�ƺ���
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }
        // �׷��� ������ ���� �α�
        if (spriteRenderer == null)
        {
            Debug.LogError($"[YSort] SpriteRenderer�� ã�� �� �����ϴ�: {name}", this);
            enabled = false;
            return;
        }

        // ���̾� �̸��� �� ���� ����
        spriteRenderer.sortingLayerName = sortingLayerName;
    }

    void LateUpdate()
    {
        if (spriteRenderer == null) return;

        // y�� �������� �տ� �׷������� order ���
        int order = Mathf.RoundToInt(-transform.position.y * sortOrderFactor);
        spriteRenderer.sortingOrder = order;
    }
}
