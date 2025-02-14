
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
            Debug.LogError("TSV ������ ã�� �� �����ϴ�: " + filePath);
            return;
        }

        using (StreamReader reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue; // �� �� ����

                var splitData = line.Split('\t'); // TSV�� ��('\t')���� ����
                if (splitData.Length < 5) continue; // �����Ͱ� ������ ��� ����

                bool parsedIsMine;
                Boolean.TryParse(splitData[2], out parsedIsMine);
                int parsedCloseTime;
                if (!int.TryParse(splitData[3].Trim(), out parsedCloseTime))
                {
                    Debug.LogWarning("������ �ƴ�");
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
                    Debug.LogWarning($"�ߺ��� Ű�� �����Ǿ����ϴ�: {textdata.objCode}");
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
            Debug.LogWarning("Bakery_In Ű�� ã�� �� �����ϴ�.");
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
        // �ܺ� ���̺��� �����͸� �����´ٰ� �����ϰ� ���� �����͸� �����մϴ�.
        HintData toolShopHint = new HintData("ö���� ��Ʈ", "ö������ ������ ����� ...", "����", HintState.Locked);
        hintDictionary.Add(toolShopHint.hintName, toolShopHint);
    }

    //��Ʈ�� ���������׸� ������ �ش� or ���� ��������� ���� ���Ƚ���� �����Ѵ� (
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
            
            Debug.Log($"{hintName}��(��) �رݵǾ����ϴ�.");
        }
        else
        {
            Debug.LogWarning($"��Ʈ {hintName}��(��) �������� �ʽ��ϴ�.");
        }
    }

    public void UseHint(string hintName)
    {
        if (hintDictionary.ContainsKey(hintName))
        {
            HintData hint = hintDictionary[hintName];
            if (hint.hintState == HintState.UnLocked)
            {
                //ī��Ʈ, Used���º�ȯ -> RPC
                hint.hintState = HintState.Used;
                Debug.Log($"{hintName}��(��) ���Ǿ����ϴ�.");
            }
            else if (hint.hintState == HintState.Locked)
            {
                Debug.LogWarning($"{hintName}��(��) ���� �رݵ��� �ʾҽ��ϴ�.");
            }
        }
    }

    public HintData GetHintData(string hintName)
    {
        if (hintDictionary.ContainsKey(hintName))
        {
            return hintDictionary[hintName];
        }
        Debug.LogWarning($"��Ʈ {hintName}��(��) �������� �ʽ��ϴ�.");
        return null;
    }
}
*/