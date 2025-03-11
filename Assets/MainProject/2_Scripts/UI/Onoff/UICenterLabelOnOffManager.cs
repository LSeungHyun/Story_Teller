using UnityEngine;

public class UICenterLabelOnOffManager : MonoBehaviour
{
    public UICenterLabelSetter uiCenterLabelSetter;

    public PortalContainer portalContainer;
    public GameObject centerLabelGroup;

    private void Start()
    {
        portalContainer.uICenterLabelOnOffManager = this;
    }
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
