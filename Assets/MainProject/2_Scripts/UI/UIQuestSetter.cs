using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UIQuestSetter : MonoBehaviour
{
    [SerializeField] public QuestContainer questContainer;
    public QuestData RowData;

    public string answer = "";

    public bool isDone = false;
    public int donePerson = 0;

    public Text nameDisplay;
    public InputField answerInput;

    public void SetQuest(string currentObjCode)
    {
        if (questContainer != null && questContainer.questDatas != null)
        {
            RowData = questContainer.questDatas.FirstOrDefault(r => r.objCode == currentObjCode);
            if (RowData != null)
            {
                nameDisplay.text = RowData.name;
                answer = RowData.answer;
            }
        }
    }

    public void onConfirmBtn()
    {
        if(answerInput.text == answer)
        {
            Debug.Log("dd");
        }
        else
        {
            Debug.Log("nn");
        }
    }
}
