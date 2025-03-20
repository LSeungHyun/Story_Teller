using UnityEngine;
using System.Linq;
using System;
public class HintStateManager : MonoBehaviour
{
    public ManagerConnector managerConnector;

    public HintContainer hintContainer;
    public HintData targetRow;

    [ContextMenu("targetRow")]
    private void ShowProperties()
    {
        Debug.Log(JsonUtility.ToJson(targetRow, true));
    }

    void Awake()
    {
        managerConnector.hintStateManager = this;
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
        var session = GameManager.Instance.Session;
        session.SetHintState(this, currentObjCode, "used");
    }
}
