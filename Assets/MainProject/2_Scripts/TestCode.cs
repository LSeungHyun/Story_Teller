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
//        dialogData.textData = "�ȳ��ϼ���! �̰��� ��ȭ �����Դϴ�.";
//        dialogData.closeTime = 0;
//        dataDictionary.Add(dialogData.objCode, dialogData);

//        DataList popupData = new DataList();
//        popupData.objCode = "Popup1";
//        popupData.objType = "Popup";
//        popupData.name = "���";
//        popupData.textData = "�̰��� �˾� �����Դϴ�.";
//        popupData.closeTime = 5;
//        dataDictionary.Add(popupData.objCode, popupData);

//        DataList sampleData = new DataList();
//        sampleData.objCode = "objCode";
//        sampleData.objType = "Dialog";
//        sampleData.name = "NPC";
//        sampleData.isMine = false;
//        sampleData.textData = "�� �����ʹ� 'objCode' Ű�� �ҷ��ɴϴ�.";
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
//            Debug.LogError("Ű 'objCode'�� �ش��ϴ� DataList �����Ͱ� �����ϴ�.");
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
//                Debug.LogWarning("�� �� ���� objType: " + typeData);
//                break;
//        }
//    }

//    private void ProcessDialog(DataList data)
//    {
//        Debug.Log("��ȭ(Dialog) ó�� ����");
//        Debug.Log("ĳ���� �̸�: " + data.name);
//        Debug.Log("��ȭ ����: " + data.textData);
//        Debug.Log("���� ���ϴ��� ����(isMine): " + data.isMine);
//    }

//    private void ProcessPopup(DataList data)
//    {
//        Debug.Log("�˾�(Popup) ó�� ����");
//        Debug.Log("�˾� ����: " + data.name);
//        Debug.Log("�˾� �޽���: " + data.textData);
//        if (data.closeTime > 0)
//        {
//            Debug.Log("�˾��� " + data.closeTime + "�� �� �ڵ����� �����ϴ�.");
//        }
//        else
//        {
//            Debug.Log("�˾��� �������� ������ �մϴ�.");
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
