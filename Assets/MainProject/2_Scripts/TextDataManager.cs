using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

[Serializable]
public class SheetResponse
{
    public string status;
    public string message;
    public RowData[] data;
}

[Serializable]
public class RowData
{
    public string objCode;
    public string name;      // 새로 추가
    public bool isMine;
    public int closeTime;
    public string textData;
}

public class TextDataManager : MonoBehaviour
{
    [SerializeField]
    private string REDIRECT_URI = "https://script.google.com/macros/s/AKfycbxEiT8dlM_jo-QM3q_4NnwJ3WhlCC4Hf88SsWnNqcDVnmqIlmsaHYmHz1AmegE6lvIR/exec";

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
                Debug.Log("Raw JSON: " + json);

                SheetResponse sheetResponse = JsonUtility.FromJson<SheetResponse>(json);

                if (sheetResponse != null && sheetResponse.status == "Gotcha" && sheetResponse.data != null)
                {
                    foreach (RowData row in sheetResponse.data)
                    {
                        Debug.Log(
                            $"objCode: {row.objCode}, " +
                            $"name: {row.name}, " +
                            $"isMine: {row.isMine}, " +
                            $"closeTime: {row.closeTime}, " +
                            $"textData: {row.textData}\n"
                        );
                    }
                }
                else
                {
                    Debug.LogWarning("No data or status != SUCCESS");
                }
            }
        }
    }
}
