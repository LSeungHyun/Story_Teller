using UnityEngine;
using System.Linq;
using System;
public class HintStateManager : MonoBehaviour
{

    [SerializeField]
    private HintContainer hintContainer;
    private HintData targetRow;

    [ContextMenu("targetRow")]
    private void ShowProperties()
    {
        Debug.Log(JsonUtility.ToJson(targetRow, true));
    }

    public void HIntUnlocked(string currentObjCode)
    {
         targetRow = hintContainer.hintDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        if (targetRow == null)
            return;
        targetRow.isUsed = "unlocked";
    }
    public void HIntUsed(string currentObjCode)
    {
        targetRow = hintContainer.hintDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        if (targetRow == null)
            return;
        targetRow.isUsed = "used";
    }
}
