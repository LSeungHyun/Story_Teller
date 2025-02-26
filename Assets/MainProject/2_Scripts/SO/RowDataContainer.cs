using UnityEngine;

[CreateAssetMenu(fileName = "RowDataContainer", menuName = "Scriptable Objects/RowDataContainer")]
public class RowDataContainer : ScriptableObject
{
    public string objCode;
    public RowData[] rowDatas;
}
