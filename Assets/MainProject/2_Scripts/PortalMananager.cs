using UnityEngine;

public class PortalMananager : MonoBehaviour
{
    public string objCode = "";
    public PortalContainer portalContainer;
    public AbsctractGameSession session;

    // 포탈 내에 머무르는 시간 (예: 2초)
    public float countTime = 2f;
    // 이동할 위치 (또는 씬 전환 시 활용)
    public Transform nextMap;
    // 포탈 이동 진행 여부
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

        Debug.Log("포탈에 진입");
        session.ShowPortalCenterLabel(this, collision);
        session.StartPortalCountdown(this, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isAreadyMove)
            return;

        Debug.Log("포탈을 벗어남");
        session.ClosePortalCenterLabel(this, collision);
        session.CloseCenterLabel(portalContainer.uICenterLabelOnOffManager);
        session.StopPortalCountdown(this, collision);
    }
}
