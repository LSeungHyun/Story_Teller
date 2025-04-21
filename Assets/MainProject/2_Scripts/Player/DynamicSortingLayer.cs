using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DynamicSortingLayer : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public PlayerManager thePlayer;
    public SpriteRenderer spriteRenderer;

    public SpriteRenderer[] childSpriteGroup;

    public string basicLayer;
    public string currentSortingLayer;

    // ������Ʈ�� �ڿ� ���� �� ����� ���̾� �̸�
    public string underObjectLayer;
    // ������Ʈ�� �տ� ���� �� ����� ���̾� �̸�
    public string aboveObjectLayer;

    // ������ �� �ٸ� ������Ʈ (��: �÷��̾�)
    public AbsctractGameSession session;

    public bool isColliding;
    void Start()
    {
        basicLayer = spriteRenderer.sortingLayerName;
        //thePlayer = managerConnector.playerManager;
        session = GameManager.Instance.Session;
    }


    void Update()
    {
        if (thePlayer == null && managerConnector.playerManager != null)
        {
            thePlayer = managerConnector.playerManager;
        }

        if (thePlayer && isColliding)
        {
            string newSortingLayer = (transform.position.y > thePlayer.transform.position.y) ? underObjectLayer : aboveObjectLayer;
            currentSortingLayer = newSortingLayer;
            spriteRenderer.sortingLayerName = currentSortingLayer;

            if(childSpriteGroup != null)
            {
                SetLayerName(currentSortingLayer);
            }
        }
    }

    // ��ȿ�� �浹���� �Ǻ��� �ִ� ����
    private bool IsValidCollision(Collider2D collision)
    {
        if (!GameManager.Instance.isType)
        {
            // isType==false �� �׻� ���
            return true;
        }

        // isType==true ��, PlayerManager�� �ְ� �� PV�� ��츸 ���
        var player = collision.GetComponent<PlayerManager>();
        return (player != null && player.PV.IsMine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidCollision(collision))
        {
            session.SortingLayerIsCollision(this, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsValidCollision(collision))
        {
            session.SortingLayerIsCollision(this, false);
            session.SortingLayerName(this);
        }
    }

    public void SetLayerName(string layerName)
    {
        for (int i = 0; i < childSpriteGroup.Length; i++)
        {
            childSpriteGroup[i].sortingLayerName = layerName;
        }
    }
}