using UnityEngine;

public class CurrentObjectManager : MonoBehaviour
{
    public static CurrentObjectManager Instance { get; private set; }

    [SerializeField] public ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;

    public UIContentsManager uiContentsManager;

    public UIPopUpOnOffManager uiPopUpOnOffManager;
    public UIPopUpManager uiPopUpManager;
    public UIDialogueSetter uiDialogueSetter;
    public UIImageSetter uiImageSetter;
    public UIQuestSetter uiQuestSetter;

    public UICenterLabelOnOffManager uiCenterLabelOnOffManager;
    public UICenterLabelSetter uiCenterLabelSetter;

    public HintStateManager hintStateManager;
    public BubbleSetter bubbleSetter;

    public string newObjCode;

    private void Awake()
    {
        managerConnector.currentObjectManager = this;
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    
    public void SetCurrentObjData(string currentObjCode)
    {
        var session = GameManager.Instance.Session;

        if (currentObjCode == "Enter_Move")
        {
            session.ChangePlayerisMoved(managerConnector.playerManager, false, false);
        }

        if ((newObjCode == "Enter_Wait" || newObjCode == "Enter_Move") && (currentObjCode != "Enter_Wait" && currentObjCode != "Enter_Move"))
        {
            return;
        }


        newObjCode = currentObjCode;
        UINextSetter.Instance.SetNextCode(currentObjCode);
        session.HandleInteractionBasic(this, currentObjCode);
    }
}
