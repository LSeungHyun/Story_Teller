using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
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

    [Header("Title Start UI")]
    public GameObject backGround_Blur;
    public GameObject backGround_Start;
    public GameObject Title_Btn_Group;

    [Header("Single Multi UI")]
    public GameObject singlePlay_Btn;
    public GameObject multiPlay_Btn;
    #endregion

    float time = 0f;
    float F_time = 1f;

    private bool isBlinking = false;

    public bool isWaiting;

    void Start()
    {
        StartCoroutine(LedaOn());
    }

    #region UI Management

    IEnumerator LedaOn()
    {
        leda_Logo.gameObject.SetActive(true);

        Color alpha = leda_Logo.color;

        while (alpha.a < 5f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 5, time);
            leda_Logo.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(time);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(3, 0, time);
            leda_Logo.color = alpha;
            yield return null;
        }

        leda_Logo.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        Start_Group.SetActive(true);
        leda_Logo_Group.SetActive(false);
        StartCoroutine(SmoothBlink());
    }

    IEnumerator SmoothBlink()
    {
        isBlinking = true;

        Color c = Start_Img.color;
        while (isBlinking)
        {
            // 1) ���ĸ� 1��0���� ������ ���� (0.5�� ����)
            float duration = 0.5f;
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float normalizedTime = t / duration; // 0��1
                c.a = Mathf.Lerp(1f, 0f, normalizedTime);
                Start_Img.color = c;
                yield return null; // ������ ���
            }
            // �������� Ȯ���� 0����
            c.a = 0f;
            Start_Img.color = c;

            // 2) ���ĸ� 0��1�� ������ ���� (0.5�� ����)
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float normalizedTime = t / duration; // 0��1
                c.a = Mathf.Lerp(0f, 1f, normalizedTime);
                Start_Img.color = c;
                yield return null;
            }
            // �������� Ȯ���� 1��
            c.a = 1f;
            Start_Img.color = c;
        }
    }
    public void PendingEmailValidation()
    {
        if (!isWaiting)
        {
            StopAllCoroutines();
            StartCoroutine(DotAnimation());
        }

        isWaiting = true;

        emailSendBtnText.text = "������";
    }

    public void ActiveEmailCheckGroup()
    {
        emailCheckGroup.SetActive(true);
    }
    public void ActiveCodeAccessGroup()
    {
        emailCheckGroup.SetActive(false);
        codeCheckGroup.SetActive(false);
        accessCompleted.SetActive(true);
    }
    public void ActiveCodeErrorGroup()
    {
        codeCheckGroup.SetActive(false);
        codeErrorGroup.SetActive(true);
    }

    public IEnumerator DotAnimation()
    {
        int dotCount = 0;

        while (true)
        {
            // 1) �� ���� ����
            dotCount++;

            // 2) 3���� �ʰ��ϸ� �ٽ� 1�� ��ȯ
            if (dotCount > 3)
            {
                dotCount = 1;
            }

            // 3) "���� ��� ��" + ��(dotCount��)
            //    new string('.', dotCount)�� ���� dotCount��ŭ �ݺ��� ���ڿ� ����
            emailCheckText.text = "���� ��� ��" + new string('.', dotCount);

            // 4) 1�� ���
            yield return new WaitForSeconds(1f);
        }
    }
    public void OkEmailValidation()
    {
        codeCheckGroup.SetActive(true); // �ڵ� �Է� Ȱ��ȭ
        codeInputField.gameObject.SetActive(true);

        emailCheckGroup.SetActive(false);
        emailInputField.gameObject.SetActive(false);
    }

    public void TitleUIManagement()
    {
        emailCheckGroup.SetActive(false);
        codeCheckGroup.SetActive(false);
        backGround_Blur.SetActive(false);

        backGround_Start.SetActive(true);
        Title_Btn_Group.SetActive(true);

        //���� �ε�����ִ� �޼���

        //SceneManager.LoadScene("1_WaitingRoom");
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

            if(errorMessage.Contains("�ڵ�"))
                codeCheckText.text = errorMessage;

            return false;
        }
        return true;
    }
    #endregion
}
