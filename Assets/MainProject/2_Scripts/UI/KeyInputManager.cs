using UnityEngine;
using System.Linq;


public class KeyInputManager : MonoBehaviour
{
    [SerializeField] private RowDataContainer rowDataContainer;
    private UIPopUpManager uiPopUpManager;

    void Awake()
    {
        uiPopUpManager = GetComponent<UIPopUpManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            string currentobjcode = rowDataContainer.objCode; 
            RowData targetRow = rowDataContainer.rowDatas.FirstOrDefault(r => r.objCode == currentobjcode);
            if (targetRow != null && targetRow.textData != null)
            {
                uiPopUpManager.OpenPopUpWindow(targetRow);
            }
        }
    }
}
