using System.Collections.Generic;
using UnityEngine;

public class ObjectDictionary : MonoBehaviour
{
    public Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();

    public void CacheObject(string key, GameObject obj)
    {
        if (obj != null)
        {
            if (!objectDictionary.ContainsKey(key))
            {
                objectDictionary.Add(key, obj);
                Debug.Log($"Object '{key}' cached and added.");
            }
            else
            {
                Debug.LogWarning($"Key '{key}' already exists in the dictionary.");
            }
        }
        else
        {
            Debug.LogWarning($"Cannot cache null object for key '{key}'.");
        }
    }

    // ������Ʈ ��������
    public GameObject GetObject(string key)
    {
        if (objectDictionary.TryGetValue(key, out GameObject obj))
        {
            return obj;
        }
        else
        {
            Debug.LogWarning($"Object with key '{key}' not found.");
            return null;
        }
    }

    // ������Ʈ ����
    public void RemoveObject(string key)
    {
        if (objectDictionary.Remove(key))
        {
            Debug.Log($"Object '{key}' removed.");
        }
        else
        {
            Debug.LogWarning($"Failed to remove. Key '{key}' not found.");
        }
    }

    // ��� ������Ʈ ��� (������)
    public void PrintAllObjects()
    {
        foreach (var kvp in objectDictionary)
        {
            Debug.Log($"Key: {kvp.Key}, Object: {kvp.Value.name}");
        }
    }
}
