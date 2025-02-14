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
                if (splitData.Length < 3) continue; // 데이터가 부족한 경우 무시

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
                    Debug.LogWarning($"중복된 키가 감지되었습니다: {textdata.name}");
                }
            }
        }

        if (dicData.ContainsKey("Bakery_In"))
        {
            Debug.Log(dicData["Bakery_In"].data);
        }
        else
        {
            Debug.LogWarning("Bakery_In 키를 찾을 수 없습니다.");
        }
    }
}
