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
        // �ΰ� ���̵� �ִϸ��̼� ����
        StartCoroutine(LedaOn());
    }

    #region UI Management

    /// <summary>
    /// �ΰ� ���� �� ������� �ڷ�ƾ (DOTween ���)
    /// </summary>
    IEnumerator LedaOn()
    {
        // 1) �ΰ� �ʱ� ����
        leda_Logo.gameObject.SetActive(true);
        leda_Logo.color = new Color(leda_Logo.color.r, leda_Logo.color.g, leda_Logo.color.b, 0f);

        // 2) �ΰ� ���̵� �� (0 �� 1)
        yield return leda_Logo.DOFade(1f, fadeDuration)
                              .SetEase(fadeEase)
                              .WaitForCompletion();

        // ��� ���
        yield return new WaitForSeconds(0.3f);

        // 3) �ΰ� ���̵� �ƿ� (1 �� 0)
        yield return leda_Logo.DOFade(0f, fadeDuration)
                              .SetEase(fadeEase)
                              .WaitForCompletion();

        // 4) �ΰ� ������Ʈ ��Ȱ��
        leda_Logo.gameObject.SetActive(false);
        leda_Logo_Group.SetActive(false);

        // 5) Start UI Ȱ��ȭ
        Start_Group.SetActive(true);
        //SoundContainer.soundManager.Play("�޴����_������");
        // 6) ��ư ������ �ڷ�ƾ
        StartCoroutine(SmoothBlink());
    }

    /// <summary>
    /// Start ��ư �̹��� �����̴� �ڷ�ƾ (���� ��� ����)
    /// </summary>
    IEnumerator SmoothBlink()
    {
        isBlinking = true;
        Color c = Start_Img.color;

        while (isBlinking)
        {
            // 1) ���� 1��0
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

            // 2) ���� 0��1
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

    

    // ======= ���� �޼������ ShowUI/HideUI�� ��ü�ϰų� �ʿ��� ������ ���� =======

    public void PendingEmailValidation()
    {
        if (!isWaiting)
        {
            StopAllCoroutines(); // DotAnimation ����
            StartCoroutine(DotAnimation("���� ��� ��"));
        }
        isWaiting = true;
        emailSendBtnText.text = "������";
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
            if (errorMessage.Contains("�̸���"))
                emailCheckText.text = errorMessage;
            if (errorMessage.Contains("�ڵ�"))
                codeCheckText.text = errorMessage;

            return false;
        }
        return true;
    }

    #endregion
}
