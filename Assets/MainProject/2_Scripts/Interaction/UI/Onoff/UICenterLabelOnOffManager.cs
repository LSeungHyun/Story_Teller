using UnityEngine;
using System.Linq;

public class UICenterLabelOnOffManager : MonoBehaviour
{
    [SerializeField] public NextDataContainer nextDataContainer;
    public CurrentObjectManager currentObjectManager;

    public GameObject centerLabelGroup;
    public UICenterLabelSetter uiCenterLabelSetter;

    public void OpenCenterLabelWindow()
    {
        var session = GameManager.Instance.Session;
        session.OpenCenterLabelBasic(this);
    }
    public void CloseCenterLabelWindow()
    {
        var session = GameManager.Instance.Session;
        session.CloseCenterLabelBasic(this);
    }
}
