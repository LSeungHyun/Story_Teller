using UnityEngine;
using UnityEngine.EventSystems;  // 반드시 추가
using DG.Tweening;

public class DoTweenManager : MonoBehaviour
{
    public SoundContainer SoundContainer;

    [Header("Animation Settings")]
    public float shrinkScale = 0.9f;   // 눌렸을 때 줄일 비율
    public float tweenDuration = 0.1f; // 축소/확대 시간
    public Ease easeType = Ease.OutQuad;

    // Fade 관련
    [Header("DOTween Settings")]
    public float fadeDuration = 1f; // 로고 페이드 인/아웃 시간
    public float scaleDuration = 0.3f; // 팝업 열고 닫을 때 스케일 애니메이션 시간
    public Ease fadeEase = Ease.Linear;
    public Ease scaleEase = Ease.OutBack;

    // 모든 버튼 OnClick에서 이 메서드를 호출
    public void ClickAnim()
    {
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

        //SoundContainer.soundManager.Play("button_sound");

        // 3) 이전 트위닝 중단
        rect.DOKill();

        // 4) 축소 후 다시 확대
        rect.DOScale(shrinkScale, tweenDuration).SetEase(easeType)
            .OnComplete(() =>
            {
                rect.DOScale(1f, tweenDuration).SetEase(easeType);
            });

        //Debug.Log("클릭애니메이션 완료 for " + clickedObj.name);
    }

    /// <summary>
    /// 팝업(패널) 보여줄 때 (스케일 0→1)
    /// </summary>
    public void ShowUI(GameObject ui)
    {
        // 먼저 활성화
        ui.SetActive(true);

        RectTransform rect = ui.GetComponent<RectTransform>();
        if (rect == null) return;

        // 이전 트위닝이 있으면 중단
        rect.DOKill();

        // 스케일을 0으로 세팅
        rect.localScale = Vector3.zero;

        // DOTween 애니메이션: 0 -> 1
        rect.DOScale(1f, scaleDuration)
            .SetEase(scaleEase);
    }

    /// <summary>
    /// 팝업(패널) 숨길 때 (스케일 1→0)
    /// </summary>
    public void HideUI(GameObject ui)
    {
        RectTransform rect = ui.GetComponent<RectTransform>();
        if (rect == null) return;

        rect.DOKill();

        // 퇴장: 1 -> 0, Ease.InBack
        rect.DOScale(0f, 0.125f)
            .SetEase(Ease.InBack)
            .OnComplete(() => ui.SetActive(false));
    }
}