using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Linq;
using System.Text.RegularExpressions;

[Serializable]
public class SheetResponse
{
    public string status;
    public string message;
    public RowData[] email;
}

[Serializable]
public class RowData
{
    public string objCode;
    public string name;      // ���� �߰�
    public bool isMine;
    public int closeTime;
    public string[] textData;
}

public class TextDataManager : MonoBehaviour
{
    [SerializeField]
    private string REDIRECT_URI;

    // SO ���۷���
    [SerializeField]
    private RowDataContainer rowDataContainer;

    private void Start()
    {
        StartCoroutine(LoadSheetData());
    }

    private IEnumerator LoadSheetData()
    {
        string requestUrl = REDIRECT_URI + "?action=getTextDataSheet";

        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError
                || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                // ';' ���� �Ľ� ����
                json = SplitTextData(json);

                // JsonUtility�� �Ľ�
                SheetResponse sheetResponse = JsonUtility.FromJson<SheetResponse>(json);

                // ScriptableObject�� �ݿ�
                if (sheetResponse != null && sheetResponse.email != null && rowDataContainer != null)
                {
                    rowDataContainer.rowDatas = sheetResponse.email;
                    Debug.Log("RowDataContainer updated with new data!");
                }

                // ����: Ư�� RowData Ȯ��
                CheckDataLog(sheetResponse);
            }
        }
    }

    private string SplitTextData(string json)
    {
        string pattern = "\"textData\"\\s*:\\s*\"([^\"]*)\"";
        return Regex.Replace(json, pattern, match =>
        {
            string text = match.Groups[1].Value;
            string[] parts = text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim();
            }
            string jsonArray = "[" + string.Join(",", parts.Select(p => "\"" + p.Replace("\"", "\\\"") + "\"")) + "]";
            return "\"textData\": " + jsonArray;
        });
    }

    private void CheckDataLog(SheetResponse sheetResponse)
    {
        if (sheetResponse != null && sheetResponse.email != null)
        {
            RowData targetRow = sheetResponse.email.FirstOrDefault(r => r.objCode == "Bakery_In");
            if (targetRow != null && targetRow.textData != null)
            {
                for (int i = 0; i < targetRow.textData.Length; i++)
                {
                    Debug.Log($"Line {i + 1}: {targetRow.textData[i]}");
                }
            }
        }
        else
        {
            Debug.LogWarning("�����;���");
        }
    }
}
