using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.Windows;

public class ValidationManager : MonoBehaviour
{
    #region Constants
    private static class AuthConstants
    {
        public const string REDIRECT_URI = "https://script.google.com/macros/s/AKfycbz_7LAdgvaWfHCwPY3Qiih4dwNTON3eFVALTmlpEX865xrpjXDJSekvtdT4NR01Cm1Z/exec";
    }

    public LoginUIManager loginUIManager;
    #endregion

    #region User Data
    public string loggedInEmail = string.Empty;
    
    private bool isCheckingVerification = false;
    private ValidationResponse lastResponse;

    [System.Serializable]
    public class ValidationResponse
    {
        public string status;
        public string message;
        public string email;
    }
    #endregion

    #region LifeCycle Methods

    private void Update()
    {
        if (isCheckingVerification)
        {
            StartCoroutine(CheckVerificationStatus(loggedInEmail));
            isCheckingVerification = false;
        }
    }
    #endregion

    #region OnClick Methods

    public void OnSendEmail()
    {
        if (!loginUIManager.IsValidInput(loginUIManager.emailInputField.text, "�̸����� �Է����ּ���.")) 
            return;

        string emailInput = loginUIManager.emailInputField.text;

        string noWhitespaceEmail = Regex.Replace(emailInput, @"\s+", "");
        StartCoroutine(SendEmailValidationRequest("sendEmail", noWhitespaceEmail));
        loggedInEmail = noWhitespaceEmail;
    }

    public void OnValidateCode()
    {
        if (!loginUIManager.IsValidInput(loginUIManager.codeInputField.text, "�ڵ带 �Է����ּ���.")) return;
        string codeInput = loginUIManager.codeInputField.text;

        string noWhitespaceCode = Regex.Replace(codeInput, @"\s+", "");
        StartCoroutine(SendCodeValidationRequest(noWhitespaceCode, loggedInEmail));
    }
    #endregion

    #region Network Communication
    private IEnumerator SendEmailValidationRequest(string action, string email)
    {
        string validationUrl = $"{AuthConstants.REDIRECT_URI}?action={action}&email={UnityWebRequest.EscapeURL(email)}";

        using (UnityWebRequest request = UnityWebRequest.Get(validationUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                lastResponse = JsonUtility.FromJson<ValidationResponse>(request.downloadHandler.text);
                Debug.Log($" ���� ����: {request.downloadHandler.text}");
                HandleServerResponse(lastResponse);

                if (lastResponse.status == "SendEmail")
                {
                    isCheckingVerification = true;
                }
            }
            else
            {
                Debug.LogError($" ���� �߻�: {request.error}");
                //resultText.text = $"Validation failed: {request.error}";
            }
        }
    }

    private IEnumerator SendCodeValidationRequest(string code, string email)
    {
        string validationUrl = $"{AuthConstants.REDIRECT_URI}?action=validateCode&code={UnityWebRequest.EscapeURL(code)}&email={UnityWebRequest.EscapeURL(email)}";

        using (UnityWebRequest request = UnityWebRequest.Get(validationUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                lastResponse = JsonUtility.FromJson<ValidationResponse>(request.downloadHandler.text);
                //Debug.Log($" �ڵ� ���� ����: {request.downloadHandler.text}");
                loginUIManager.codeCheckText.text = lastResponse.message;
                HandleServerResponse(lastResponse);
            }
        }
    }
    #endregion

    #region Response Handling
    private IEnumerator CheckVerificationStatus(string email)
    {
        string url = $"{AuthConstants.REDIRECT_URI}?action=checkVerificationStatus&email={UnityWebRequest.EscapeURL(email)}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                lastResponse = JsonUtility.FromJson<ValidationResponse>(request.downloadHandler.text);
                Debug.Log(" ���� ���� Ȯ��: " + lastResponse.status + lastResponse.message);
                HandleServerResponse(lastResponse);
            }
        }
    }

    private void HandleServerResponse(ValidationResponse response)
    {
        switch (response.status)
        {
            case "SendEmail":
                //resultText.text = $"�̸��� ���� �Ϸ�: {response.message}";
                break;

            //�̸��� ���� �Ϸ�
            case "OK":
                loginUIManager.OkEmailValidation();
                break;

            case "PENDING":
                loginUIManager.PendingEmailValidation();

                isCheckingVerification = true;
                break;

            case "CODE_OK":
                //resultText.text = " �ڵ� ���� �Ϸ�!";
                loginUIManager.TitleUIManagement();
                break;

            case "ERROR":
                //resultText.text = $" ���� �߻�: {response.message}";
                break;

            default:
                //resultText.text = $" �� �� ���� ����: {response.message}";
                break;
        }
    }
    #endregion
}