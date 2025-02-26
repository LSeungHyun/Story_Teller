using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class KeyInputManager : MonoBehaviour
{
    [SerializeField] private ObjDataTypeContainer objDataTypeContainer;
    public UIPopUpManager uiPopUpManager;
    public UITextSetter uiTextSetter;
    public UIImageSetter uiImageSetter;

    public string currentObjCode;
    public string currentObjType;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentObjCode = objDataTypeContainer.objCode;
            HandleActionInteraction(currentObjCode);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiPopUpManager.ClosePopUpWindow();
        }
    }

    private void HandleActionInteraction(string currentObjCode)
    {
        currentObjType = objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == currentObjCode).dataType.ToLower();

        if (currentObjType == null)
            return;

        if (currentObjType.Contains("hint"))
        {
            Debug.Log("이건 힌트입니다");
            return;
        }
        if (currentObjType.Contains("bubble"))
        {
            Debug.Log("이건 말풍선입니다");
            return;
        }
        if (currentObjType.Contains("centerlabel"))
        {
            Debug.Log("이건 센터라벨입니다");
            return;
        }


        if (currentObjType.Contains("dialogue"))
        {
            uiTextSetter.SetTextData(currentObjCode);

            if (currentObjType.Contains("quest"))
                uiPopUpManager.OpenQuestWindow();
            else 
                uiPopUpManager.OpenDefaultWindow();

            return;
        }

        if (currentObjType.Contains("image"))
        {
            uiImageSetter.SetImageData(currentObjCode);

            if (currentObjType.Contains("quest"))
                uiPopUpManager.OpenQuestWindow();
            else
                uiPopUpManager.OpenDefaultWindow();

            return;
        }



        /*        targetRow = rowDataContainer.rowDatas.FirstOrDefault(r => r.objCode == currentObjCode);

                if (targetRow.IsNextObj != null)
                {
                    nextTargetRow = rowDataContainer.rowDatas.FirstOrDefault(r => r.objCode == targetRow.IsNextObj);
                    uiPopUpManager.SetNextCode(nextTargetRow);
                }*/

    }
}
