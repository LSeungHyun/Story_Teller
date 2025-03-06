using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StringGameObjectPair
{
    public string key;
    public GameObject value;
}

public class ObjectDictionary : MonoBehaviour
{
    [SerializeField] private List<StringGameObjectPair> objectList = new List<StringGameObjectPair>();
    private Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        PopulateDictionary();
    }

    public void PopulateDictionary()
    {
        objectDictionary.Clear();
        foreach (var pair in objectList)
        {
            if (!string.IsNullOrEmpty(pair.key) && pair.value != null)
            {
                if (!objectDictionary.ContainsKey(pair.key))
                {
                    objectDictionary.Add(pair.key, pair.value);
                }
            }
        }
    }

    public void ToggleObjectActive(string key)
    {
        if (objectDictionary.ContainsKey(key))
        {
            GameObject obj = objectDictionary[key];
            obj.SetActive(!obj.activeSelf);
        }
    }
}
