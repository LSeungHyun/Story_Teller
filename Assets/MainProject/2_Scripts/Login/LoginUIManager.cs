using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
    #region UI Elements
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

    public bool isWaiting;

    #region UI Management

    public void PendingEmailValidation()
    {
        if (!isWaiting)
            StartCoroutine(DotAnimation());

        isWaiting = true;

        emailSendBtnText.text = "������";
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
