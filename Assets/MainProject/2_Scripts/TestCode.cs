
using UnityEngine;
using System.Collections.Generic;
using static Data_Dictionary;
using System.IO;
using System;
using System.ComponentModel;
using System.Linq;

public class DataList
{
    public string objCode;
    public string name;
    public bool isMine;
    public int closeTime;
    public List<String> textData;
}

public class TestCode : MonoBehaviour
{
    public Dictionary<string, DataList> dicData = new Dictionary<string, DataList>();

    private void Awake()
    {
        ReadTSV();
    }

    private void ReadTSV()
    {
        string filePath = Path.Combine(Application.dataPath, "MainProject/8_Excel/Data_Story.tsv");
        if (!File.Exists(filePath))
        {
            Debug.LogError("TSV 파일을 찾을 수 없습니다: " + filePath);
            return;
        }

        using (StreamReader reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue; // 빈 줄 방지

                var splitData = line.Split('\t'); // TSV는 탭('\t')으로 구분
                if (splitData.Length < 5) continue; // 데이터가 부족한 경우 무시

                bool parsedIsMine;
                Boolean.TryParse(splitData[2], out parsedIsMine);
                int parsedCloseTime;
                if (!int.TryParse(splitData[3].Trim(), out parsedCloseTime))
                {
                    Debug.LogWarning("정수가 아님");
                }

                DataList textdata = new DataList
                {
                    objCode = splitData[0],
                    name = splitData[1],
                    isMine = parsedIsMine,
                    closeTime = parsedCloseTime,
                    textData = splitData[4].Split(';').ToList()
                };

                if (!dicData.ContainsKey(textdata.objCode))
                {
                    dicData.Add(textdata.objCode, textdata);
                }
                else
                {
                    Debug.LogWarning($"중복된 키가 감지되었습니다: {textdata.objCode}");
                }
            }
        }

        if (dicData.ContainsKey("Bakery_In"))
        {
            DataList data = dicData["Bakery_In"];
            for (int i = 0; i < data.textData.Count; i++)
            {
                Debug.Log($"Line {i + 1}: {data.textData[i]}");
            }
        }
        else
        {
            Debug.LogWarning("Bakery_In 키를 찾을 수 없습니다.");
        }
    }
}

/*
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
*/