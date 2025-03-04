using UnityEngine;
using System.Linq;

public class KeyInputManager : MonoBehaviour
{
    [SerializeField] public ObjDataTypeContainer objDataTypeContainer;

    public UIPopUpOnOffManager uiPopUpOnOffManager;
    public UITextSetter uiTextSetter;
    public UIImageSetter uiImageSetter; 

    public UICenterLabelOnOffManager uiCenterLabelOnOffManager;
    public UICenterLabelSetter uiCenterLabelSetter;

    public HintStateManager hintStateManager;
    public BubbleSetter bubbleSetter;

    public ObjDataType currentRow;
    public string currentObjCode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            HandleFKey();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiPopUpOnOffManager.ClosePopUpWindow();
        }
    }

    private void HandleFKey()
    {
        currentObjCode = objDataTypeContainer.objCode;
        SetCurrentObjData(currentObjCode);
    }

    public void SetCurrentObjData(string currentObjCode)
    {
        currentRow = objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == currentObjCode);
        if (currentRow == null)
            return;
        var session = GameManager.Instance.Session;
        session.HandleInteraction(this);
    }
}
