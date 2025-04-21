using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    [Tooltip("기본 레이어 이름. 보통 'Default' 같은 걸 씁니다.")]
    public string sortingLayerName = "Default";

    [Tooltip("정렬 민감도: 100으로 놓으면 y=1.23 → order = -123")]
    public int sortOrderFactor = 100;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        // 레이어는 한 번만 세팅
        sr.sortingLayerName = sortingLayerName;
    }

    void LateUpdate()
    {
        // y 좌표가 낮을수록(화면 아래일수록) order가 높아져서 앞에 그려집니다.
        int order = Mathf.RoundToInt(-transform.position.y * sortOrderFactor);
        sr.sortingOrder = order;
    }
}
