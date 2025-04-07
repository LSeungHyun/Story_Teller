using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.InputSystem;


public class UIQuestSetter : MonoBehaviour
{
    [SerializeField] public QuestContainer questContainer;
    public UIPopUpOnOffManager uiPopUpOnOffManager;
    public QuestDictionary questDictionary;
    public UIQuestDetailSetter uiQuestDetailSetter;

    public QuestData targetRow;

    public string answer = "";
    public bool isDone = false;
    public int donePerson = 0;

    public GameObject isDoneFalseGroup;
    public GameObject isDoneTrueGroup;
    public Text nameDisplay;
    public InputField answerInput; 
    
    public Transform pageDisplayParent;
    private GameObject currentPageDisplayInstance;

    public Text donePlayerCount;
    public Text doneAnswer;
    public string currentObjCode;


    public void SetQuestBg(string currentObjCode)
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
                SetQuestQuiz(targetRow.objCode);
                if (isDone)
                {
                    SetDonePage();
                }
            }
        }
    }

    public void SetQuestQuiz(string objCode)
    {
        if (questDictionary.preFabDictionary.ContainsKey(objCode))
        {
            GameObject prefab = questDictionary.preFabDictionary[objCode];
            if (currentPageDisplayInstance != null)
            {
                Destroy(currentPageDisplayInstance);
            }

            uiQuestDetailSetter.SetQuestDetail(prefab);

            currentPageDisplayInstance = Instantiate(prefab, pageDisplayParent);
        }
    }




    public void SetDonePage()
    {
        if(PhotonNetwork.IsConnected)
        {
            donePlayerCount.text = "완료 인원:" + UINextSetter.Instance.currentObjCodeDict.Find(x => x.value == currentObjCode).playersIsDone.Count + "/" + PhotonNetwork.CurrentRoom.PlayerCount;
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
            uiPopUpOnOffManager.ClosePopUpWindow();
            UINextSetter.Instance.AddPlayerToDoneList(currentObjCode);
            UINextSetter.Instance.CheckDoneAndNext(currentObjCode);
        }
        else
        {
            CurrentObjectManager.Instance.SetCurrentObjData("Wrong_Answer");
            answerInput.text = "";
        }
    }
}
