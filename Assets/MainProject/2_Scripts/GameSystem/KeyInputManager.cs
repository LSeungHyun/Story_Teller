using UnityEngine;
using System.Linq;

public class KeyInputManager : MonoBehaviour
{
    public UIPopUpOnOffManager uiPopUpOnOffManager;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            HandleFKey();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiPopUpOnOffManager.ClosePopUpWindow();
        }
    }

    public void HandleFKey()
    {
        CurrentObjectManager.Instance.SetCurrentObjData(null);
    }
}
