// Assets/Editor/FindBySortingLayer.cs
using UnityEngine;
using UnityEditor;
using System.Linq;

public class FindBySortingLayer : EditorWindow
{
    string layerName = "FrontObj";

    [MenuItem("Tools/Find By Sorting Layer��")]
    static void OpenWindow()
    {
        GetWindow<FindBySortingLayer>("Find By Sorting Layer");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("SpriteRenderer Sorting Layer �˻�", EditorStyles.boldLabel);
        layerName = EditorGUILayout.TextField("Layer Name", layerName);

        if (GUILayout.Button("ã�Ƽ� �����ϱ�"))
        {
            // ���� �ִ� ��� SpriteRenderer �߿���
            var all = GameObject.FindObjectsOfType<SpriteRenderer>();
            var matches = all
                .Where(sr => sr.sortingLayerName == layerName)
                .Select(sr => sr.gameObject)
                .ToArray();

            if (matches.Length == 0)
                Debug.LogWarning($"Sorting Layer '{layerName}'�� ������Ʈ�� �����ϴ�.");
            else
            {
                Selection.objects = matches;
                Debug.Log($"Found {matches.Length} object(s) with Sorting Layer '{layerName}'.");
            }
        }
    }
}
