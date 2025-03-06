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

    // 오브젝트 가져오기
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

    // 오브젝트 삭제
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

    // 모든 오브젝트 출력 (디버깅용)
    public void PrintAllObjects()
    {
        foreach (var kvp in objectDictionary)
        {
            Debug.Log($"Key: {kvp.Key}, Object: {kvp.Value.name}");
        }
    }
}
