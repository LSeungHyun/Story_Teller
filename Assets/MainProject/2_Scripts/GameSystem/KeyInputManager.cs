using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class KeyInputManager : MonoBehaviour
{
    [SerializeField] private RowDataContainer rowDataContainer;
    private UIPopUpManager uiPopUpManager;

    public string currentObjCode;
    public RowData targetRow;
    public RowData nextTargetRow;
    private void Awake()
    {
        uiPopUpManager = GetComponent<UIPopUpManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            HandleActionInteraction();
            if (targetRow.IsNextObj != null)
            {
                nextTargetRow = rowDataContainer.rowDatas.FirstOrDefault(r => r.objCode == targetRow.IsNextObj);
                uiPopUpManager.SetNextCode(nextTargetRow);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiPopUpManager.ClosePopUpWindow();
        }
    }

    private void HandleActionInteraction()
    {
        currentObjCode = rowDataContainer.objCode;
        targetRow = rowDataContainer.rowDatas.FirstOrDefault(r => r.objCode == currentObjCode);

        if (targetRow == null || targetRow.dataList == null)
            return;

        switch (targetRow.dataType)
        {
            case "centerLabel":
                Debug.Log("이건 센터라벨입니다ㅣ");
                break;
            case "dialogue":
                uiPopUpManager.OpenPopUpWindow(targetRow);
                break;
            case "image":
                TrimImageList(targetRow.dataList);
                break;
            default:
                Debug.LogWarning("데이터타입 없음");
                break;
        }
    }

    private void TrimImageList(string[] imageResources)
    {
        var sprites = new List<Sprite>();

        foreach (string resource in imageResources)
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
