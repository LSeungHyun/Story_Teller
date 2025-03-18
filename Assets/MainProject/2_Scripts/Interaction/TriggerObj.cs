using UnityEngine;

public class TriggerObj : MonoBehaviour
{
    public string objCode;
    public bool isPassiveObject = false;

    private void OnEnable()
    {
        if (isPassiveObject)
        {
            CurrentObjectManager.Instance.SetCurrentObjData(objCode);
        }
    }
}