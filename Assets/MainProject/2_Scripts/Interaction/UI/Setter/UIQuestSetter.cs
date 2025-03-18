using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;

public class UIQuestSetter : MonoBehaviour
{
    [SerializeField] public QuestContainer questContainer;
    public UIPopUpOnOffManager uiPopUpOnOffManager;
    public ManagerConnector managerConnector;

    public QuestData targetRow;

    public string answer = "";
    public bool isDone = false;
    public int donePerson = 0;

    public GameObject isDoneFalseGroup;
    public GameObject isDoneTrueGroup;
    public Text nameDisplay;
    public InputField answerInput;

    public Text donePlayerCount;
    public Text doneAnswer;

    [SerializeField] public QuestStatus status = new QuestStatus();

    [System.Serializable]
    public class QuestStatus
    {
        public List<int> playersIsDone = new List<int>();
    }
    public void Awake()
    {
        managerConnector.uiQuestSetter = this;
    }

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
                if (isDone)
                {
                    SetDonePage();
                }
            }
        }
    }
    public void SetDonePage()
    {
        donePlayerCount.text = "완료 인원:" + status.playersIsDone.Count + "/" + PhotonNetwork.CurrentRoom.PlayerCount;
        doneAnswer.text = targetRow.answer;
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
            var session = GameManager.Instance.Session;
            session.OnEnterAnswer(this);
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
