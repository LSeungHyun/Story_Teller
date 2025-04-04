using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class HintSetter : MonoBehaviour
{
    public HintContainer hintContainer;
    public HintOnOffManager hintOnOffManager;
    public HintStateManager hintStateManager;
    public UIManager uiManager;

    public Transform parentObject;
    public List<Transform> childList = new List<Transform>();

    public Text hintCount;

    public GameObject detailAnswerBtn;
    public Text detailTitleName;
    public Text detailAnswer;
    public Text detailHintData;

    public Sprite unusedImage;
    public Sprite usedImage;

    public void GetChildObj()
    {
        childList.Clear();

        foreach (Transform child in parentObject)
        {
            childList.Add(child);
        }
    }

    public void InitHintState()
    {
        int usedHintCount = 0;
        GetChildObj();
        for (int i = 0; i < hintContainer.hintDatas.Length; i++)
        {
            string hintusedData = hintContainer.hintDatas[i].isUsed;
            string hintName = hintContainer.hintDatas[i].name;

            switch (hintusedData)
            {
                case "locked":
                    GetDetail(childList[i].gameObject, 0, hintName);
                    break;
                case "unlocked":
                    GetDetail(childList[i].gameObject, 1, hintName);
                    break;
                case "used":
                    GetDetail(childList[i].gameObject, 2, hintName);
                    usedHintCount++;
                    break;
                default:
                    GetDetail(childList[i].gameObject, 0, hintName);
                    break;
            }
        }
        SetTitle(usedHintCount);
    }

    public void GetDetail(GameObject childObj, int hintDepth, string hintName)
    {
        Image displayImage = childObj.transform.GetChild(0).GetComponent<Image>();
        Text displayText = displayImage.transform.GetChild(0).GetComponent<Text>();

        if (hintDepth > 0)
        {
            displayText.text = hintName;
        }
        else
        {
            displayText.text = "???";
        }
        if (hintDepth > 1)
        {
            displayImage.sprite = usedImage;
        }
        else
        {
            displayImage.sprite = unusedImage;
        }
    }
    public void HintDivide(string hintCode)
    {
        HintData currentRow = hintContainer.hintDatas.FirstOrDefault(r => r.objCode == hintCode);

        switch (currentRow.isUsed)
        {
            case "locked":
                CurrentObjectManager.Instance.SetCurrentObjData("Use_Locked_Hint");
                break;
            case "unlocked":
                SetHintDetailPopUp(currentRow);

                uiManager.OpenPanel("Hint_Detail_Panel");
                hintStateManager.HIntUsed(hintCode);
                CurrentObjectManager.Instance.SetCurrentObjData("Use_Unlocked_Hint");
                break;
            case "used":
                SetHintDetailPopUp(currentRow);

                uiManager.OpenPanel("Hint_Detail_Panel");
                break;
            default:
                CurrentObjectManager.Instance.SetCurrentObjData("Use_Locked_Hint");
                break;
        }
    }

    public void SetHintDetailPopUp(HintData currentRow)
    {
        detailAnswerBtn.SetActive(true);
        detailTitleName.text = currentRow.name;
        detailHintData.text = currentRow.hintTextData;
        detailAnswer.text = currentRow.answer;
    }

    public void SetTitle(int usedHintCount)
    {
        hintCount.text = "(" + usedHintCount + "/14)";
    }
}
