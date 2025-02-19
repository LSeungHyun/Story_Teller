using UnityEngine;

[CreateAssetMenu(fileName = "RowDataContainer", menuName = "MyGame/RowDataContainer")]
public class RowDataContainer : ScriptableObject
{
    public string objCode;
    public RowData[] rowDatas;
}
