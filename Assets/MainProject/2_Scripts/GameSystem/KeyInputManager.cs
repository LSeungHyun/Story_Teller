using UnityEngine;
using System.Linq;

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
        if (managerConnector.playerManager.isMove && Input.GetKeyDown(KeyCode.F))
        {
            HandleFKey();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiPopUpOnOffManager.ClosePopUpWindow();
        }
    }

    public void HandleFKey()
    {
        CurrentObjectManager.Instance.SetCurrentObjData(objDataTypeContainer.objCode);
    }
}
