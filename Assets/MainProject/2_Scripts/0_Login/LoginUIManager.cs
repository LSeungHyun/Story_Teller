using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoginUIManager : DoTweenManager
{
    #region UI Elements
    [Header("Logo UI")]
    public GameObject leda_Logo_Group;
    public Image leda_Logo;
    public GameObject black_BackGround;

    [Header("Start UI")]
    public GameObject Start_Group;
    public Image Start_Img;

    [Header("Email UI")]
    public InputField emailInputField;
    public GameObject emailCheckGroup;
    public Text emailCheckText;
    public Text emailSendBtnText;

    [Header("Code UI")]
    public InputField codeInputField;
    public GameObject codeCheckGroup;
    public GameObject codeErrorGroup;
    public GameObject accessCompleted;
    public Text codeCheckText;
    #endregion

    private bool isBlinking = false;
    public bool isWaiting;

    void Start()
    {
        // 로고 페이드 애니메이션 시작
        StartCoroutine(LedaOn());
    }

    #region UI Management

    /// <summary>
    /// 로고 등장 후 사라지는 코루틴 (DOTween 사용)
    /// </summary>
    IEnumerator LedaOn()
    {
        // 1) 로고 초기 세팅
        leda_Logo.gameObject.SetActive(true);
        leda_Logo.color = new Color(leda_Logo.color.r, leda_Logo.color.g, leda_Logo.color.b, 0f);

        // 2) 로고 페이드 인 (0 → 1)
        yield return leda_Logo.DOFade(1f, fadeDuration)
                              .SetEase(fadeEase)
                              .WaitForCompletion();

        // 잠시 대기
        yield return new WaitForSeconds(0.3f);

        // 3) 로고 페이드 아웃 (1 → 0)
        yield return leda_Logo.DOFade(0f, fadeDuration)
                              .SetEase(fadeEase)
                              .WaitForCompletion();

        // 4) 로고 오브젝트 비활성
        leda_Logo.gameObject.SetActive(false);
        leda_Logo_Group.SetActive(false);

        // 5) Start UI 활성화
        Start_Group.SetActive(true);
        //SoundContainer.soundManager.Play("달담뮤직_오렌지");
        // 6) 버튼 깜빡임 코루틴
        StartCoroutine(SmoothBlink());
    }

    /// <summary>
    /// Start 버튼 이미지 깜빡이는 코루틴 (기존 방식 유지)
    /// </summary>
    IEnumerator SmoothBlink()
    {
        isBlinking = true;
        Color c = Start_Img.color;

        while (isBlinking)
        {
            // 1) 알파 1→0
            float duration = 0.5f;
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float normalizedTime = t / duration;
                c.a = Mathf.Lerp(1f, 0f, normalizedTime);
                Start_Img.color = c;
                yield return null;
            }
            c.a = 0f;
            Start_Img.color = c;

            // 2) 알파 0→1
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float normalizedTime = t / duration;
                c.a = Mathf.Lerp(0f, 1f, normalizedTime);
                Start_Img.color = c;
                yield return null;
            }
            c.a = 1f;
            Start_Img.color = c;
        }
    }

    

    // ======= 기존 메서드들을 ShowUI/HideUI로 대체하거나 필요한 로직만 남김 =======

    public void PendingEmailValidation()
    {
        if (!isWaiting)
        {
            StopAllCoroutines(); // DotAnimation 정지
            StartCoroutine(DotAnimation("인증 대기 중"));
        }
        isWaiting = true;
        emailSendBtnText.text = "재전송";
    }

    public void ActiveEmailCheckGroup()
    {
        ShowUI(emailCheckGroup);
    }

    public void ActiveCodeAccessGroup()
    {
        HideUI(emailCheckGroup);
        HideUI(codeCheckGroup);
        ShowUI(accessCompleted);
    }

    public void ActiveCodeErrorGroup()
    {
        HideUI(codeCheckGroup);
        ShowUI(codeErrorGroup);
    }

    public IEnumerator DotAnimation(string text)
    {
        int dotCount = 0;

        while (true)
        {
            dotCount++;
            if (dotCount > 3) dotCount = 1;

            emailCheckText.text = text + new string('.', dotCount);
            yield return new WaitForSeconds(1f);
        }
    }

    public void OkEmailValidation()
    {
        HideUI(emailCheckGroup);

        ShowUI(codeCheckGroup);
    }

    public void SceneMove(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    #endregion

    #region Utility Methods

    public bool IsValidInput(string input, string errorMessage)
    {
        if (string.IsNullOrEmpty(input))
        {
            if (errorMessage.Contains("이메일"))
                emailCheckText.text = errorMessage;
            if (errorMessage.Contains("코드"))
                codeCheckText.text = errorMessage;

            return false;
        }
        return true;
    }

    #endregion
}
