using UnityEngine;

public class PortalMananager : MonoBehaviour
{
    public string objCode = "";
    public PortalContainer portalContainer;
    public AbsctractGameSession session;

    // ��Ż ���� �ӹ����� �ð� (��: 2��)
    public float countTime = 2f;
    // �̵��� ��ġ (�Ǵ� �� ��ȯ �� Ȱ��)
    public Transform nextMap;
    // ��Ż �̵� ���� ����
    public bool isAreadyMove;

    void Start()
    {
        session = GameManager.Instance.Session;

        if(GameManager.Instance.isType == false)
        {
            objCode = "Enter_Wait3";
        }
        else
        {
            objCode = "Enter_All";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAreadyMove)
            return;

        Debug.Log("��Ż�� ����");
        session.ShowPortalCenterLabel(this, collision);
        session.StartPortalCountdown(this, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isAreadyMove)
            return;

        Debug.Log("��Ż�� ���");
        session.ClosePortalCenterLabel(this, collision);
        session.CloseCenterLabel(portalContainer.uICenterLabelOnOffManager);
        session.StopPortalCountdown(this, collision);
    }
}
