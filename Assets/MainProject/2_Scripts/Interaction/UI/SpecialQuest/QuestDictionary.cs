using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuizPrefabPair
{
    public string objCode;
    public GameObject preFab;
}
public class QuestDictionary : MonoBehaviour
{
    [SerializeField] private List<QuizPrefabPair> quizPageList = new List<QuizPrefabPair>();
    public Dictionary<string, GameObject> preFabDictionary = new Dictionary<string, GameObject>();
    private void Awake()
    {
        PopulateDictionary();
    }
    public void PopulateDictionary()
    {
        preFabDictionary.Clear();
        foreach (var pair in quizPageList)
        {
            if (!string.IsNullOrEmpty(pair.objCode) && pair.preFab != null)
            {
                if (!preFabDictionary.ContainsKey(pair.objCode))
                {
                    preFabDictionary.Add(pair.objCode, pair.preFab);
                }
            }
        }
    }
}
