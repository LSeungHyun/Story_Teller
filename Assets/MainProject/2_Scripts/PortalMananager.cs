using UnityEngine;

public class PortalMananager : MonoBehaviour
{
    public string objCode;
    public PortalContainer portalContainer;
    public AbsctractGameSession session;

    void Start()
    {
        session = GameManager.Instance.Session;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("���� ��Ҵ�!!");
        session.PortalEnter(this, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("���� ������!!");
        session.PortalExit(this, collision);
        session.CloseCenterLabel(portalContainer.uICenterLabelOnOffManager);
    }
}
