using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Photon.Pun;

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
    public DialogueRawData[] dialogueData;
    public ImageRawData[] imageData;
    public CenterLabelRawData[] centerLabelData;
    public BubbleData[] bubbleData;
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

#region CenterLabel Data
[Serializable]
public class CenterLabelRawData
{
    public string objCode;
    public string pageNum;
    public int closeTime;
    public string centerLabelData;
}

[Serializable]
public class CenterLabelData
{
    public string objCode;
    public CenterLabelList[] dataList;
}

[Serializable]
public class CenterLabelList
{
    public string pageNum;
    public int closeTime;
    public string centerLabelData;
}
#endregion


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

#region Image Data
[Serializable]
public class ImageRawData
{
    public string objCode;
    public string pageNum;
    public string imageData;
}

[Serializable]
public class ImageData
{
    public string objCode;
    public ImageList[] dataList;
}

[Serializable]
public class ImageList
{
    public string pageNum;
    public string imageData;
}
#endregion

[Serializable]
public class BubbleData
{
    public string objCode;
    public int closeTime;
    public string dataList;
}

[Serializable]
public class QuestData
{
    public string objCode;
    public string dataList;
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
    public PhotonView PV;

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

        if (!GameManager.Instance.isType)
        {
            Destroy(PV);
        }
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

                // 파싱
                AllSheetsResponse resp = JsonUtility.FromJson<AllSheetsResponse>(json);

                if (resp != null && resp.data != null)
                {
                    Debug.Log("Status: " + resp.status + ", Msg: " + resp.message);

                    if (centerLabelContainer != null && resp.data.centerLabelData != null)
                    {
                        centerLabelContainer.centerLabelDatas = GroupCenterLabelData(resp.data.centerLabelData);
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
                        imageContainer.imageDatas = GroupImageData(resp.data.imageData); 
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

    private CenterLabelData[] GroupCenterLabelData(CenterLabelRawData[] centerLabelList)
    {
        Dictionary<string, List<CenterLabelList>> centerLabelDic = new Dictionary<string, List<CenterLabelList>>();

        foreach (var centerLabel in centerLabelList)
        {
            if (!centerLabelDic.ContainsKey(centerLabel.objCode))
            {
                centerLabelDic[centerLabel.objCode] = new List<CenterLabelList>();
            }
            centerLabelDic[centerLabel.objCode].Add(new CenterLabelList
            {
                pageNum = centerLabel.pageNum,
                closeTime = centerLabel.closeTime,
                centerLabelData = centerLabel.centerLabelData
            });
        }
        
        List<CenterLabelData> centerLabelDataList = new List<CenterLabelData>();

        foreach (var entry in centerLabelDic)
        {
            centerLabelDataList.Add(new CenterLabelData
            {
                objCode = entry.Key,
                dataList = entry.Value.ToArray()
            });
        }

        return centerLabelDataList.ToArray();
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
        }

        List<DialogueData> dialogueDataList = new List<DialogueData>();

        foreach (var entry in dialogueDict)
        {
            dialogueDataList.Add(new DialogueData
            {
                objCode = entry.Key,
                dataList = entry.Value.ToArray()
            });
        }

        return dialogueDataList.ToArray();
    }

    private ImageData[] GroupImageData(ImageRawData[] imageList)
    {
        Dictionary<string, List<ImageList>> imageDic = new Dictionary<string, List<ImageList>>();

        foreach (var image in imageList)
        {
            if (!imageDic.ContainsKey(image.objCode))
            {
                imageDic[image.objCode] = new List<ImageList>();
            }
            imageDic[image.objCode].Add(new ImageList
            {
                pageNum = image.pageNum,
                imageData = image.imageData
            });
        }

        List<ImageData> imageDataList = new List<ImageData>();

        foreach (var entry in imageDic)
        {
            imageDataList.Add(new ImageData
            {
                objCode = entry.Key,
                dataList = entry.Value.ToArray()
            });
        }

        return imageDataList.ToArray();
    }

}
