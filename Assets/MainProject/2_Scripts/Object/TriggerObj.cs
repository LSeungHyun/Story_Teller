using UnityEngine;

public class TriggerObj : MonoBehaviour
{
    public string objCode;
    public bool isPassiveObject = false;

    private void Awake()
    {
        if (isPassiveObject)
        {
            CurrentObjectManager.Instance.SetCurrentObjData(objCode);
        }
    }
}