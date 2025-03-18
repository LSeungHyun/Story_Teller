using UnityEngine;

public class CenterLabelCleaner : MonoBehaviour
{
    private void OnEnable()
    {
      CurrentObjectManager.Instance.uiCenterLabelOnOffManager.CloseCenterLabelWindow();
    }
}
