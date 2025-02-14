using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Data_Dictionary : MonoBehaviour
{
    public Dictionary<string, TextData> dicData = new Dictionary<string, TextData>();

    [System.Serializable]
    public class TextData
    {
        public string type;
        public string name;
        public string data;
    }

    private void Start()
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
                if (splitData.Length < 3) continue; // �����Ͱ� ������ ��� ����

                TextData textdata = new TextData
                {
                    type = splitData[0],
                    name = splitData[1],
                    data = splitData[2]
                };

                if (!dicData.ContainsKey(textdata.name))
                {
                    dicData.Add(textdata.name, textdata);
                }
                else
                {
                    Debug.LogWarning($"�ߺ��� Ű�� �����Ǿ����ϴ�: {textdata.name}");
                }
            }
        }

        if (dicData.ContainsKey("Bakery_In"))
        {
            Debug.Log(dicData["Bakery_In"].data);
        }
        else
        {
            Debug.LogWarning("Bakery_In Ű�� ã�� �� �����ϴ�.");
        }
    }
}
