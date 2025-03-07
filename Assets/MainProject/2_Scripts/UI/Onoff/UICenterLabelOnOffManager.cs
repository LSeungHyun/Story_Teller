using UnityEngine;

public class UICenterLabelOnOffManager : MonoBehaviour
{
    public UICenterLabelSetter uiCenterLabelSetter;

    public GameObject centerLabelGroup;

    public void OpenCenterLabelWindow()
    {
        var session = GameManager.Instance.Session;
        session.OpenCenterLabel(this);
    }

    public void CloseCenterLabelWindow()
    {
        var session = GameManager.Instance.Session;
        session.CloseCenterLabel(this);
    }
}
