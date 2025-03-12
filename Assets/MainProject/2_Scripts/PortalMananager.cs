using Photon.Pun;
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
        session.ShowPortalCenterLabel(this);
        session.StartPortalCountdown(this, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isAreadyMove)
            return;

        Debug.Log("��Ż�� ���");
        session.StopPortalCountdown(this, collision);

        // 2) ������ ���� ���ų� 'Enter_All'�� ����
        session.ClosePortalCenterLabel(this);

        // 3) �̱�/��Ƽ �������� ���̴� CenterLabel UI�� �ִٸ� ���� �޼���
        session.CloseCenterLabel(portalContainer.uICenterLabelOnOffManager);
    }
}
