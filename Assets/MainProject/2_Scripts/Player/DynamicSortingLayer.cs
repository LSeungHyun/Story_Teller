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

    // 오브젝트가 뒤에 있을 때 사용할 레이어 이름
    public string underObjectLayer;
    // 오브젝트가 앞에 있을 때 사용할 레이어 이름
    public string aboveObjectLayer;

    // 기준이 될 다른 오브젝트 (예: 플레이어)
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

    // 유효한 충돌인지 판별해 주는 헬퍼
    private bool IsValidCollision(Collider2D collision)
    {
        if (!GameManager.Instance.isType)
        {
            // isType==false 면 항상 허용
            return true;
        }

        // isType==true 면, PlayerManager가 있고 내 PV인 경우만 허용
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