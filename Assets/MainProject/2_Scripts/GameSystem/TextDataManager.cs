using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
    public ObjDataType[] objDataType;    // ObjDataTypeSheet
    public NextData[] textData;
    public CenterLabelData[] centerLabelData;
    public DialogueRawData[] dialogueData;
    public BubbleData[] bubbleData;
    public ImageData[] imageData;
    public QuestData[] questData;
    public HintData[] hintData;
}

[Serializable]
public class ObjDataType
{
    public string objCode;
    public string dataType;
    public bool isMine;
}

[Serializable]
public class NextData
{
    public string objCode;
    public string isNextData;
    public string isNextObj;
}


[Serializable]
public class CenterLabelData
{
    public string objCode;
    public int closeTime;
    public string[] dataList;
}

#region Dialogue Data
[Serializable]
public class DialogueRawData
{
    public string pageNum;
    public string objCode;
    public string bgType;
    public string textData;
}

[Serializable]
public class DialogueData
{
    public string objCode;
    public DialogueList[] dataList;
}

[Serializable]
public class DialogueList
{
    public string pageNum;
    public BgType bgType;
    public string textData;
}

public enum BgType
{
    기본,
    독백,
    사라도령
}

#endregion

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
    private NextDataContainer nextDataContainer;

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

                    if (centerLabelContainer != null && resp.data.centerLabelData != null)
                    {
                        centerLabelContainer.centerLabelDatas = resp.data.centerLabelData;
                        Debug.Log("CenterLabelData loaded: " + resp.data.centerLabelData.Length);
                    }

                    if (centerLabelContainer != null && resp.data.textData != null)
                    {
                        nextDataContainer.nextDatas = resp.data.textData;
                        Debug.Log("NextData loaded: " + resp.data.textData.Length);
                    }


                    // 3) DialogueData
                    if (dialogueContainer != null && resp.data.dialogueData != null)
                    {
                        dialogueContainer.dialogueDatas = GroupDialogueData(resp.data.dialogueData);
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

    private DialogueData[] GroupDialogueData(DialogueRawData[] dialogueList)
    {
        Dictionary<string, List<DialogueList>> dialogueDict = new Dictionary<string, List<DialogueList>>();

        foreach (var dialogue in dialogueList)
        {
            BgType bgTypeEnum;
            if (Enum.TryParse(dialogue.bgType, out bgTypeEnum))
            {
                if (!dialogueDict.ContainsKey(dialogue.objCode))
                {
                    dialogueDict[dialogue.objCode] = new List<DialogueList>();
                }
                dialogueDict[dialogue.objCode].Add(new DialogueList
                {
                    pageNum = dialogue.pageNum,
                    bgType = bgTypeEnum,
                    textData = dialogue.textData
                });
            }
            else
            {
                Debug.LogError("타입 이상해");
            }
        }

        return dialogueDict.Select(entry => new DialogueData
        {
            objCode = entry.Key,
            dataList = entry.Value.ToArray()
        }).ToArray();
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

        if (resp.data?.objDataType != null)
        {
            foreach (var item in resp.data.objDataType)
            {
                item.isMine = (item.isMine.ToString().ToLower() == "true");
            }
        }

        return JsonUtility.ToJson(resp);
    }

}
