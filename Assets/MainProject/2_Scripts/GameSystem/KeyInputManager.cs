using UnityEngine;

public class KeyInputManager : MonoBehaviour
{
    public UIPopUpOnOffManager uiPopUpOnOffManager;
    public ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;

    private void Awake()
    {
        managerConnector.keyInputManager = this;
    }
    private void Update()
    {
        if (managerConnector.playerManager == null) return;

        if (managerConnector.playerManager != null && managerConnector.playerManager.isMove && Input.GetKeyDown(KeyCode.F))
        {
            HandleFKey();
        }
    }

    public void HandleFKey()
    {
        CurrentObjectManager.Instance.SetCurrentObjData(objDataTypeContainer.objCode);
    }
}
