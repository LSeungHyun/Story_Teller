using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class KeyInputManager : MonoBehaviour
{
    [SerializeField] private ObjDataTypeContainer objDataTypeContainer;
    [SerializeField] private CenterLabelContainer centerLabelContainer;
    [SerializeField] private DialogueContainer dialogueContainer;
    [SerializeField] private BubbleContainer bubbleContainer;
    [SerializeField] private ImageContainer imageContainer;
    [SerializeField] private QuestContainer questContainer;
    [SerializeField] private HintContainer hintContainer;
    public UIPopUpManager uiPopUpManager;

    public string currentObjCode;
    public string currentObjType;
    public RowData targetRow;
    public RowData nextTargetRow;

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
        currentObjType = objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == currentObjCode).dataType;

        if (currentObjType == null)
            return;

        switch (currentObjType)
        {
            case "centerLabel":
                Debug.Log("이건 센터라벨입니다ㅣ");
                break;
            case "dialogue":
                SetDialogue(currentObjCode);
                break;
            case "image":
                SetImagePopUp(currentObjCode);
                break;
            default:
                Debug.LogWarning("데이터타입 없음");
                break;
        }

/*        targetRow = rowDataContainer.rowDatas.FirstOrDefault(r => r.objCode == currentObjCode);

        if (targetRow.IsNextObj != null)
        {
            nextTargetRow = rowDataContainer.rowDatas.FirstOrDefault(r => r.objCode == targetRow.IsNextObj);
            uiPopUpManager.SetNextCode(nextTargetRow);
        }*/

    }

    private void SetDialogue(string currentObjCode)
    {
        DialogueData targetRow = dialogueContainer.dialogueDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        uiPopUpManager.OpenPopUpWindow(targetRow);
    }
    private void SetImagePopUp(string currentObjCode)
    {
        ImageData targetRow = imageContainer.imageDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        var sprites = new List<Sprite>();

        foreach (string resource in targetRow.dataList)
        {
            string resourceName = resource.Trim(); 
            string resourcePath = "ImagePopup/" + (resourceName.Contains("_") ? resourceName.Replace('_', '/') : resourceName);

            Sprite sprite = Resources.Load<Sprite>(resourcePath);
            if (sprite == null)
            {
                Debug.LogWarning("Failed to load sprite: " + resourceName);
            }
            sprites.Add(sprite);
        }

        uiPopUpManager.OpenImage(sprites);
    }
}
