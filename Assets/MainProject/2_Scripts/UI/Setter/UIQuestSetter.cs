using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UIQuestSetter : MonoBehaviour
{
    [SerializeField] public QuestContainer questContainer;
    public UIPopUpOnOffManager uiPopUpOnOffManager;
    public QuestData targetRow;

    public string answer = "";
    public bool isDone = false;
    public int donePerson = 0;

    public GameObject isDoneFalseGroup;
    public GameObject isDoneTrueGroup;
    public Text nameDisplay;
    public InputField answerInput;

    public void SetQuest(string currentObjCode)
    {
        ClearData();
        if (questContainer != null && questContainer.questDatas != null)
        {
            targetRow = questContainer.questDatas.FirstOrDefault(r => r.objCode == currentObjCode);
            if (targetRow != null)
            {
                nameDisplay.text = targetRow.name;
                answer = targetRow.answer;
                isDone = targetRow.isDone;
                isDoneFalseGroup.SetActive(!isDone);
                isDoneTrueGroup.SetActive(isDone);
            }
        }
    }

    public void ClearData()
    {
        isDoneFalseGroup.SetActive(false);
        isDoneTrueGroup.SetActive(false);
        nameDisplay.text = "";
        answerInput.text = "";
    }

    public void onConfirmBtn()
    {
        if(answerInput.text == answer)
        {
            uiPopUpOnOffManager.CloseAndCheckPopUpWindow();
            targetRow.isDone = true;
        }
        else
        {
            Debug.Log("nn");
        }
    }
    public void onCloseBtn()
    {
        uiPopUpOnOffManager.ClosePopUpWindow();
    }
}
