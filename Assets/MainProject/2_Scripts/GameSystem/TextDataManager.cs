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
    public RowData[] email; // 이름 꼭 고쳐볼 예정
}

[Serializable]
public class RowData
{
    public string objCode;
    public string dataType;
    public string IsNextObj;
    public string name;      // 새로 추가
    public bool isMine;
    public int closeTime;
    public string[] dataList;
}

public class Category
{
    public string categoryName;
    public RowData[] rowDatas;
}

public class TextDataManager : MonoBehaviour
{
    [SerializeField]
    private string REDIRECT_URI;

    // SO 레퍼런스
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
                // ';' 구분 파싱 로직
                json = SplitTextData(json);

                // JsonUtility로 파싱
                SheetResponse sheetResponse = JsonUtility.FromJson<SheetResponse>(json);

                // ScriptableObject에 반영
                if (sheetResponse != null && sheetResponse.email != null && rowDataContainer != null)
                {
                    rowDataContainer.rowDatas = sheetResponse.email;
                    Debug.Log("RowDataContainer updated with new data!");
                }
            }
        }
    }

    private string SplitTextData(string json)
    {
        string pattern = "\"dataList\"\\s*:\\s*\"([^\"]*)\"";
        return Regex.Replace(json, pattern, match =>
        {
            string text = match.Groups[1].Value;
            string[] parts = text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim();
            }
            string jsonArray = "[" + string.Join(",", parts.Select(p => "\"" + p.Replace("\"", "\\\"") + "\"")) + "]";
            return "\"dataList\": " + jsonArray;
        });
    }
}
