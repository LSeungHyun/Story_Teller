using UnityEngine;
using UnityEngine.EventSystems;  // 반드시 추가
using DG.Tweening;

public class DoTweenManager : MonoBehaviour
{
    [Header("Animation Settings")]
    public float shrinkScale = 0.9f;   // 눌렸을 때 줄일 비율
    public float tweenDuration = 0.1f; // 축소/확대 시간
    public Ease easeType = Ease.OutQuad;

    // 모든 버튼 OnClick에서 이 메서드를 호출
    public void ClickAnim()
    {
        // 1) 현재 클릭된 UI 오브젝트 가져오기
        GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
        if (clickedObj == null)
        {
            Debug.LogWarning("No currentSelectedGameObject found.");
            return;
        }

        // 2) RectTransform 가져오기
        RectTransform rect = clickedObj.GetComponent<RectTransform>();
        if (rect == null)
        {
            Debug.LogWarning("Clicked object has no RectTransform.");
            return;
        }

        // 3) 이전 트위닝 중단
        rect.DOKill();

        // 4) 축소 후 다시 확대
        rect.DOScale(shrinkScale, tweenDuration).SetEase(easeType)
            .OnComplete(() =>
            {
                rect.DOScale(1f, tweenDuration).SetEase(easeType);
            });

        Debug.Log("클릭애니메이션 완료 for " + clickedObj.name);
    }
}