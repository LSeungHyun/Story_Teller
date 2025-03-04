using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Linq;
using System.Text.RegularExpressions;

#region Data Array
[Serializable]
public class AllSheetsResponse
{
    public string status;
    public string message;
    public AllData data;
}


//Apps script에서 반환해주는 var allData 내부의 변수명과
//Unity에서 선언한 배열의 변수 명이 일치해야 받아와짐
[Serializable]
public class AllData 
{
    public RowData[] textData;       // 기존 TextDataSheet
    public ObjDataType[] objDataType;    // ObjDataTypeSheet
    public CenterLabelData[] centerLabelData;
    public DialogueData[] dialogueData;
    public BubbleData[] bubbleData;
    public ImageData[] imageData;
    public QuestData[] questData;
    public HintData[] hintData;
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

[Serializable]
public class ObjDataType
{
    public string objCode;
    public string dataType;
    public bool isMine;
}

[Serializable]
public class CenterLabelData
{
    public string objCode;
    public int closeTime;
    public string[] dataList;
}

[Serializable]
public class DialogueData
{
    public string objCode;
    public string isNextObj;
    public string[] dataList;
}

[Serializable]
public class BubbleData
{
    public string objCode;
    public int closeTime;
    public string[] dataList;
}

[Serializable]
public class ImageData
{
    public string objCode;
    public string isNextObj;
    public string[] dataList;
}

[Serializable]
public class QuestData
{
    public string objCode;
    public string[] dataList;
}

[Serializable]
public class HintData
{
    public string objCode;
    public string name;
    public string answer;
    public string isUsed;
    public string hintTextData;
}
#endregion

public class TextDataManager : MonoBehaviour
{
    [SerializeField]
    private string REDIRECT_URI = "https://script.google.com/macros/s/AKfycbz_7LAdgvaWfHCwPY3Qiih4dwNTON3eFVALTmlpEX865xrpjXDJSekvtdT4NR01Cm1Z/exec";

    // SO 레퍼런스
    [SerializeField]
    private ObjDataTypeContainer objDataTypeContainer;

    [SerializeField]
    private RowDataContainer rowDataContainer;

    [SerializeField]
    private CenterLabelContainer centerLabelContainer;

    [SerializeField]
    private DialogueContainer dialogueContainer;

    [SerializeField]
    private BubbleContainer bubbleContainer;

    [SerializeField]
    private ImageContainer imageContainer;

    [SerializeField]
    private QuestContainer questContainer;

    [SerializeField]
    private HintContainer hintContainer;

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
                // JSON 응답
                string json = request.downloadHandler.text;

                // 필요 시 ';' 파싱 등 추가 처리
                json = SplitTextData(json);
                json = checkBooleanValue(json);
                // 파싱
                AllSheetsResponse resp = JsonUtility.FromJson<AllSheetsResponse>(json);

                if (resp != null && resp.data != null)
                {
                    Debug.Log("Status: " + resp.status + ", Msg: " + resp.message);
/*
                    // 1) RowData
                    if (rowDataContainer != null && resp.data.textData != null)
                    {
                        rowDataContainer.rowDatas = resp.data.textData;
                        Debug.Log("RowData loaded: " + resp.data.textData.Length);
                    }*/

                    // 2) CenterLabelData
                    if (centerLabelContainer != null && resp.data.centerLabelData != null)
                    {
                        centerLabelContainer.centerLabelDatas = resp.data.centerLabelData;
                        Debug.Log("CenterLabelData loaded: " + resp.data.centerLabelData.Length);
                    }

                    // 3) DialogueData
                    if (dialogueContainer != null && resp.data.dialogueData != null)
                    {
                        dialogueContainer.dialogueDatas = resp.data.dialogueData;
                        Debug.Log("DialogueData loaded: " + resp.data.dialogueData.Length);
                    }

                    // 4) ObjDataType
                    if (objDataTypeContainer != null && resp.data.objDataType != null)
                    {
                        objDataTypeContainer.objDataType = resp.data.objDataType;
                        Debug.Log("ObjDataType loaded: " + resp.data.objDataType.Length);
                    }

                    // 5) BubbleData
                    if (bubbleContainer != null && resp.data.bubbleData != null)
                    {
                        bubbleContainer.bubbleDatas = resp.data.bubbleData;
                        Debug.Log("BubbleData loaded: " + resp.data.bubbleData.Length);
                    }

                    // 6) ImageData
                    if (imageContainer != null && resp.data.imageData != null)
                    {
                        imageContainer.imageDatas = resp.data.imageData;
                        Debug.Log("ImageData loaded: " + resp.data.imageData.Length);
                    }

                    // 7) QuestData
                    if (questContainer != null && resp.data.questData != null)
                    {
                        questContainer.questDatas = resp.data.questData;
                        Debug.Log("QuestData loaded: " + resp.data.questData.Length);
                    }

                    // 8) HintData
                    if (hintContainer != null && resp.data.hintData != null)
                    {
                        hintContainer.hintDatas = resp.data.hintData;
                        Debug.Log("HintData loaded: " + resp.data.hintData.Length);
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid response or data is null.");
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

    private string checkBooleanValue(string json)
    {
        AllSheetsResponse resp = JsonUtility.FromJson<AllSheetsResponse>(json);

        if (resp.data?.textData != null)
        {
            foreach (var item in resp.data.textData)
            {
                item.isMine = (item.isMine.ToString().ToLower() == "true");
            }
        }

        return JsonUtility.ToJson(resp);
    }

}
