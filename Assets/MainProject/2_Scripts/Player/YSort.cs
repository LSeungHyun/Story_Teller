using UnityEngine;

[ExecuteAlways]
public class YSort : MonoBehaviour
{
    [Tooltip("공통으로 사용할 Sorting Layer 이름")]
    public string sortingLayerName = "Default";

    [Tooltip("정렬 민감도: 클수록 Y값 변화에 따른 order 변화가 큽니다.")]
    public int sortOrderFactor = 100;

    [Tooltip("수동 할당을 우선시할 SpriteRenderer")]
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        // 인스펙터에 할당된 게 없으면, 우선 자기 자신에서 찾고
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        // 그래도 없으면 부모에서 찾아보고
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }
        // 그래도 없으면 에러 로그
        if (spriteRenderer == null)
        {
            Debug.LogError($"[YSort] SpriteRenderer를 찾을 수 없습니다: {name}", this);
            enabled = false;
            return;
        }

        // 레이어 이름은 한 번만 세팅
        spriteRenderer.sortingLayerName = sortingLayerName;
    }

    void LateUpdate()
    {
        if (spriteRenderer == null) return;

        // y가 낮을수록 앞에 그려지도록 order 계산
        int order = Mathf.RoundToInt(-transform.position.y * sortOrderFactor);
        spriteRenderer.sortingOrder = order;
    }
}
