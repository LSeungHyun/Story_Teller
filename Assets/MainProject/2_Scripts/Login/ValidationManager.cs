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
        public const string REDIRECT_URI = "https://script.google.com/macros/s/AKfycbwSNHws0ZE-0n0pe0SSopL9boDOQ4wqLwmpxrzYaotcoQC4s6ddj0xY8aTXO43_59sr/exec";
    }
    #endregion

    #region UI Elements
    [Header("Email UI")]
    public InputField emailInputField;
    public GameObject emailCheckGroup;
    

    [Header("Code UI")]
    public InputField codeInputField;
    public GameObject codeCheckGroup;
    

    [Header("Etc")]
    public Text resultText;
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
    private void Start()
    {
        InitializeUI();
    }
    
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
        if (!IsValidInput(emailInputField.text, "이메일을 입력해주세요.")) return;
        string emailInput = emailInputField.text;

        string noWhitespaceEmail = Regex.Replace(emailInput, @"\s+", "");
        StartCoroutine(SendEmailValidationRequest("sendEmail", noWhitespaceEmail));
        loggedInEmail = noWhitespaceEmail;
    }

    public void OnValidateCode()
    {
        if (!IsValidInput(codeInputField.text, "코드를 입력해주세요.")) return;
        string codeInput = codeInputField.text;

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
                Debug.Log($" 서버 응답: {request.downloadHandler.text}");
                HandleServerResponse(lastResponse);

                if (lastResponse.status == "SendEmail")
                {
                    isCheckingVerification = true;
                }
            }
            else
            {
                Debug.LogError($" 오류 발생: {request.error}");
                resultText.text = $"Validation failed: {request.error}";
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
                Debug.Log($" 코드 검증 응답: {request.downloadHandler.text}");
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
                Debug.Log(" 서버 응답 확인: " + lastResponse.status + lastResponse.message);
                HandleServerResponse(lastResponse);
            }
        }
    }

    private void HandleServerResponse(ValidationResponse response)
    {
        switch (response.status)
        {
            case "SendEmail":
                resultText.text = $"이메일 전송 완료: {response.message}";
                break;

            case "OK":
                resultText.text = $" 인증 완료: {response.message}";
                codeCheckGroup.SetActive(true); // 코드 입력 활성화
                codeInputField.gameObject.SetActive(true);

                emailCheckGroup.SetActive(false);
                emailInputField.gameObject.SetActive(false);
                break;

            case "CODE_OK":
                resultText.text = " 코드 인증 완료!";
                StartGame();
                break;

            case "PENDING":
                Debug.Log(" 인증 대기 중...");
                isCheckingVerification = true;
                break;

            case "ERROR":
                resultText.text = $" 오류 발생: {response.message}";
                break;

            default:
                resultText.text = $" 알 수 없는 응답: {response.message}";
                break;
        }
    }
    #endregion

    #region UI Management
    private void InitializeUI()
    {
       
    }
    #endregion

    #region Game Flow
    private void StartGame()
    {
        Debug.Log(" 게임을 시작합니다.");
        SceneManager.LoadScene("1_WaitingRoom");
    }
    #endregion

    #region Utility Methods
    private bool IsValidInput(string input, string errorMessage)
    {
        if (string.IsNullOrEmpty(input))
        {
            resultText.text = errorMessage;
            return false;
        }
        return true;
    }
    #endregion
}
