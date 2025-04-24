using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;


public class UIQuestSetter : MonoBehaviour
{
    [SerializeField] public QuestContainer questContainer;
    public UIPopUpOnOffManager uiPopUpOnOffManager;
    public QuestDictionary questDictionary;
    public UIQuestDetailSetter uiQuestDetailSetter;
    public UIManager uiManager;
    public SoundContainer soundContainer;

    public QuestData targetRow;

    public string answer = "";
    public bool isDone = false;
    public int donePerson = 0;

    public GameObject isDoneFalseGroup;
    public GameObject isDoneTrueGroup;
    public Text nameDisplay;
    public InputField answerInput;
    public GameObject dark_Obj;
    
    public Transform pageDisplayParent;
    public GameObject currentPageDisplayInstance;

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
                if (soundContainer != null)
                {
                    soundContainer.soundManager.Play("Enter_Sound");
                }
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


            currentPageDisplayInstance = Instantiate(prefab, pageDisplayParent);
            uiQuestDetailSetter.SetQuestDetail(prefab);

            RectTransform rect = currentPageDisplayInstance.GetComponent<RectTransform>();
            if (rect != null)
            {
                Canvas.ForceUpdateCanvases();

                float width = rect.rect.width;
                float height = rect.rect.height;

                float maxWidth = 1700f;
                float maxHeight = 1400f;

                float scaleFactor = 1f;

                bool tooWide = width > maxWidth;
                bool tooTall = height > maxHeight;

                if (tooWide || tooTall)
                {
                    float widthRatio = maxWidth / width;
                    float heightRatio = maxHeight / height;
                    scaleFactor = Mathf.Min(widthRatio, heightRatio);
                }
                else
                {
                    float widthRatio = maxWidth / width;
                    float heightRatio = maxHeight / height;
                    scaleFactor = Mathf.Min(widthRatio, heightRatio);
                }

                rect.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }
        }
    }


    public void SetDonePage()
    {
        if(PhotonNetwork.IsConnected)
        {
            int donePlayerNum = UINextSetter.Instance.currentObjCodeDict.Find(x => x.value == currentObjCode).playersIsDone.Count;
            if (donePlayerNum > 0)
            donePlayerCount.text = "완료 인원:" + donePlayerNum + "/" + PhotonNetwork.CurrentRoom.PlayerCount;
            else
            {
                donePlayerCount.text = "닫기를 눌러 진행해주세요.";
            }
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
            if (soundContainer != null)
            {
                soundContainer.soundManager.Play("Correct_Sound");
            }
            targetRow.isDone = true;
            dark_Obj.SetActive(false);
            uiPopUpOnOffManager.ClosePopUpWindow();
            UINextSetter.Instance.AddPlayerToDoneList(currentObjCode);
            CurrentObjectManager.Instance.SetCurrentObjData("Correct_Answer");
        }
        else
        {
            if (soundContainer != null)
            {
                soundContainer.soundManager.Play("Wrong_Sound");
            }
            uiManager.ClickAnimWithoutSound();
            CurrentObjectManager.Instance.SetCurrentObjData("Wrong_Answer");
            answerInput.text = "";
        }
    }
}
