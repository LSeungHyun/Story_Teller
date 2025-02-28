using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
    #region UI Elements
    [Header("Logo UI")]
    public Image leda_Logo;
    public GameObject black_BackGround;

    [Header("Start UI")]
    public GameObject Start_Group;

    [Header("Email UI")]
    public InputField emailInputField;
    public GameObject emailCheckGroup;
    public Text emailCheckText;
    public Text emailSendBtnText;

    [Header("Code UI")]
    public InputField codeInputField;
    public GameObject codeCheckGroup;
    public Text codeCheckText;

    [Header("Title Start UI")]
    public GameObject backGround_Blur;
    public GameObject backGround_Start;
    public GameObject Title_Btn_Group;
    #endregion

    float time = 0f;
    float F_time = 1f;

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
        //ActiveEmailCheckGroup();
        //SceneManager.LoadScene(transferMapName);
    }

    public void PendingEmailValidation()
    {
        if (!isWaiting)
            StartCoroutine(DotAnimation());

        isWaiting = true;

        emailSendBtnText.text = "������";
    }

    public void ActiveEmailCheckGroup()
    {
        emailCheckGroup.SetActive(true);
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

        //���� �ε�����ִ� 
        //SceneManager.LoadScene("1_WaitingRoom");
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
