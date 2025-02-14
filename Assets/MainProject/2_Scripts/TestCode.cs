//using UnityEngine;
//using System.Collections.Generic;

//public class DataList
//{
//    public string objCode;
//    public string objType;
//    public string name;
//    public bool isMine;
//    public int closeTime;
//    public string textData;
//}

//public class TestCode : MonoBehaviour
//{
//    public Dictionary<string, DataList> dataDictionary = new Dictionary<string, DataList>();

//    private void Awake()
//    {
//        DataList dialogData = new DataList();
//        dialogData.objCode = "Dialog1";
//        dialogData.objType = "Dialog";
//        dialogData.name = "Hero";
//        dialogData.isMine = true;
//        dialogData.textData = "안녕하세요! 이것은 대화 예제입니다.";
//        dialogData.closeTime = 0;
//        dataDictionary.Add(dialogData.objCode, dialogData);

//        DataList popupData = new DataList();
//        popupData.objCode = "Popup1";
//        popupData.objType = "Popup";
//        popupData.name = "경고";
//        popupData.textData = "이것은 팝업 예제입니다.";
//        popupData.closeTime = 5;
//        dataDictionary.Add(popupData.objCode, popupData);

//        DataList sampleData = new DataList();
//        sampleData.objCode = "objCode";
//        sampleData.objType = "Dialog";
//        sampleData.name = "NPC";
//        sampleData.isMine = false;
//        sampleData.textData = "이 데이터는 'objCode' 키로 불러옵니다.";
//        sampleData.closeTime = 0;
//        if (!dataDictionary.ContainsKey("objCode"))
//        {
//            dataDictionary.Add("objCode", sampleData);
//        }
//    }

//    public void DivideType()
//    {
//        if (!dataDictionary.ContainsKey("objCode"))
//        {
//            Debug.LogError("키 'objCode'에 해당하는 DataList 데이터가 없습니다.");
//            return;
//        }

//        string typeData = dataDictionary["objCode"].objType;
//        Debug.Log("Retrieved objType: " + typeData);

//        switch (typeData)
//        {
//            case "Dialog":
//                ProcessDialog(dataDictionary["objCode"]);
//                break;
//            case "Popup":
//                ProcessPopup(dataDictionary["objCode"]);
//                break;
//            default:
//                Debug.LogWarning("알 수 없는 objType: " + typeData);
//                break;
//        }
//    }

//    private void ProcessDialog(DataList data)
//    {
//        Debug.Log("대화(Dialog) 처리 시작");
//        Debug.Log("캐릭터 이름: " + data.name);
//        Debug.Log("대화 내용: " + data.textData);
//        Debug.Log("내가 말하는지 여부(isMine): " + data.isMine);
//    }

//    private void ProcessPopup(DataList data)
//    {
//        Debug.Log("팝업(Popup) 처리 시작");
//        Debug.Log("팝업 제목: " + data.name);
//        Debug.Log("팝업 메시지: " + data.textData);
//        if (data.closeTime > 0)
//        {
//            Debug.Log("팝업은 " + data.closeTime + "초 후 자동으로 닫힙니다.");
//        }
//        else
//        {
//            Debug.Log("팝업은 수동으로 닫혀야 합니다.");
//        }
//    }
//}

using UnityEngine;
using System.Collections.Generic;


public class HintData
{
    public string hintName;
    public string hintData;
    public string hintAnswer;
    public HintState hintState;

    public HintData(string hintName, string hintData, string hintAnswer, HintState hintState)
    {
        this.hintName = hintName;
        this.hintData = hintData;
        this.hintAnswer = hintAnswer;
        this.hintState = hintState;
    }
}
public enum HintState
{
    Locked,
    UnLocked,
    Used
}
public class HintManager : MonoBehaviour
{
    private Dictionary<string, HintData> hintDictionary = new Dictionary<string, HintData>();

    public int hintCount = 0;

    private void Awake()
    {
        // 외부 테이블에서 데이터를 가져온다고 가정하고 예제 데이터를 생성합니다.
        HintData toolShopHint = new HintData("철물점 힌트", "철물점에 숨겨진 비밀은 ...", "정답", HintState.Locked);
        hintDictionary.Add(toolShopHint.hintName, toolShopHint);
    }

    //힌트를 마스터한테만 권한을 준다 or 각자 보고싶을때 보고 사용횟수를 공유한다 (
    public void CountHintUsed()
    {
        int usedCount = 0;
        foreach (var hint in hintDictionary.Values)
        {
            if(hint.hintState == HintState.Used)
            {
                usedCount++;
            }
        }
    }

    public void UnlockHint(string hintName)
    {
        if (hintDictionary.ContainsKey(hintName))
        {
            hintDictionary[hintName].hintState = HintState.UnLocked;
            
            Debug.Log($"{hintName}이(가) 해금되었습니다.");
        }
        else
        {
            Debug.LogWarning($"힌트 {hintName}이(가) 존재하지 않습니다.");
        }
    }

    public void UseHint(string hintName)
    {
        if (hintDictionary.ContainsKey(hintName))
        {
            HintData hint = hintDictionary[hintName];
            if (hint.hintState == HintState.UnLocked)
            {
                //카운트, Used상태변환 -> RPC
                hint.hintState = HintState.Used;
                Debug.Log($"{hintName}이(가) 사용되었습니다.");
            }
            else if (hint.hintState == HintState.Locked)
            {
                Debug.LogWarning($"{hintName}은(는) 아직 해금되지 않았습니다.");
            }
        }
    }

    public HintData GetHintData(string hintName)
    {
        if (hintDictionary.ContainsKey(hintName))
        {
            return hintDictionary[hintName];
        }
        Debug.LogWarning($"힌트 {hintName}이(가) 존재하지 않습니다.");
        return null;
    }
}
