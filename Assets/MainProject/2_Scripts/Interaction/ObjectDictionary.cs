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
    public static ObjectDictionary Instance { get; private set; }

    [SerializeField] private List<StringGameObjectPair> objectList = new List<StringGameObjectPair>();
    private Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PopulateDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void ToggleObjectActive(string key, bool isDelete)
    {
        if (objectDictionary.ContainsKey(key))
        {
            Debug.Log(key);
            GameObject obj = objectDictionary[key];
            obj.SetActive(!isDelete);
        }
    }
}
