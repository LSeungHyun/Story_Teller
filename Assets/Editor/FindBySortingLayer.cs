// Assets/Editor/FindBySortingLayer.cs
using UnityEngine;
using UnityEditor;
using System.Linq;

public class FindBySortingLayer : EditorWindow
{
    string layerName = "FrontObj";

    [MenuItem("Tools/Find By Sorting Layer…")]
    static void OpenWindow()
    {
        GetWindow<FindBySortingLayer>("Find By Sorting Layer");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("SpriteRenderer Sorting Layer 검색", EditorStyles.boldLabel);
        layerName = EditorGUILayout.TextField("Layer Name", layerName);

        if (GUILayout.Button("찾아서 선택하기"))
        {
            // 씬에 있는 모든 SpriteRenderer 중에서
            var all = GameObject.FindObjectsOfType<SpriteRenderer>();
            var matches = all
                .Where(sr => sr.sortingLayerName == layerName)
                .Select(sr => sr.gameObject)
                .ToArray();

            if (matches.Length == 0)
                Debug.LogWarning($"Sorting Layer '{layerName}'인 오브젝트가 없습니다.");
            else
            {
                Selection.objects = matches;
                Debug.Log($"Found {matches.Length} object(s) with Sorting Layer '{layerName}'.");
            }
        }
    }
}
