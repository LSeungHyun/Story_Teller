using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Net.WebSockets;

public class KeyInputManager : MonoBehaviour
{
    [SerializeField] private ObjDataTypeContainer objDataTypeContainer;
    public UIPopUpManager uiPopUpManager;
    public UITextSetter uiTextSetter;
    public UIImageSetter uiImageSetter;

    public ObjDataType currentRow;
    public string currentObjCode;
    public string currentObjType;
    public bool currentObjisMine;

    private void Update()
    {   
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentObjCode = objDataTypeContainer.objCode;
            SetCurrentObjData(currentObjCode);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiPopUpManager.ClosePopUpWindow();
        }
    }

    public void SetCurrentObjData(string currentObjCode)
    {
        currentRow = objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == currentObjCode);
        var session = GameManager.Instance.Session;
        session.HandleActionInteraction(this);
    }

    /* private void HandleActionInteraction(ObjDataType currentRow)
     {
         currentObjType = currentRow.dataType.ToLower();
         currentObjisMine = currentRow.isMine;

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
     }*/
}
