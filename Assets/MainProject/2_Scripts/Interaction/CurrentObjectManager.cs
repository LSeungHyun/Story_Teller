using UnityEngine;
using System.Linq;

public class CurrentObjectManager : MonoBehaviour
{
    public static CurrentObjectManager Instance { get; private set; }

    [SerializeField] public ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;
    public OnOffPrefabs onOffPrefabs;
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

    string[] validCodes = { "Enter_Wait", "Enter_Move", "Enter_Wait_Scene", "Enter_Move_Scene" };
    string[] moveCodes = { "Enter_Move", "Enter_Move_Scene" };

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

        if (moveCodes.Contains(currentObjCode))
        {
            session.ChangePlayerisMoved(managerConnector.playerManager, false, false);
        }

        // newObjCode가 validCodes에 속하지만 currentObjCode가 validCodes에 속하지 않는 경우 return
        if (validCodes.Contains(newObjCode) && !validCodes.Contains(currentObjCode))
        {
            return;
        }

        newObjCode = currentObjCode;
        UINextSetter.Instance.SetNextCode(currentObjCode);
        session.HandleInteractionBasic(this, currentObjCode);
    }
}
