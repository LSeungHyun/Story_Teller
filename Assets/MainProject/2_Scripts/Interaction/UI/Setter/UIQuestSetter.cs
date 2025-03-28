using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;

public class UIQuestSetter : MonoBehaviour
{
    [SerializeField] public QuestContainer questContainer;
    public UIPopUpOnOffManager uiPopUpOnOffManager;
    public UINextSetter uiNextSetter;

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
        if(PhotonNetwork.IsConnected)
        {
            donePlayerCount.text = "완료 인원:" + uiNextSetter.status.playersIsDone.Count + "/" + PhotonNetwork.CurrentRoom.PlayerCount;
        }
        else
        {
            donePlayerCount.text = "닫기를 눌러 진행해주세요.";
        }
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
            targetRow.isDone = true;
            var session = GameManager.Instance.Session;
            session.AfterQuest(this);
        }
        else
        {
            CurrentObjectManager.Instance.SetCurrentObjData("Wrong_Answer");
            answerInput.text = "";
        }
    }
    public void onCloseBtn()
    {
        uiPopUpOnOffManager.ClosePopUpWindow();
    }
}
