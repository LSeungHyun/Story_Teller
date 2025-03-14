using UnityEngine;
using System.Linq;

public class CurrentObjectManager : MonoBehaviour
{
    public static CurrentObjectManager Instance { get; private set; }

    [SerializeField] public ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;

    public ObjDataType currentRow;
    public string currentObjCode;

    public UIPopUpOnOffManager uiPopUpOnOffManager;
    public UIPopUpManager uiPopUpManager;
    public UIDialogueSetter uiDialogueSetter;
    public UIImageSetter uiImageSetter;
    public UIQuestSetter uiQuestSetter;

    public UICenterLabelOnOffManager uiCenterLabelOnOffManager;
    public UICenterLabelSetter uiCenterLabelSetter;

    public HintStateManager hintStateManager;
    public BubbleSetter bubbleSetter;

    private void Awake()
    {
        managerConnector.currentObjectManager = this;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetCurrentObjData(string outterCurrentObjCode)
    {
        if (outterCurrentObjCode == null)
            currentObjCode = objDataTypeContainer.objCode;
        else
            currentObjCode = outterCurrentObjCode;

        currentRow = objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == currentObjCode);
        if (currentRow == null)
            return;
        var session = GameManager.Instance.Session;
        session.HandleInteractionBasic(this);
    }
}
