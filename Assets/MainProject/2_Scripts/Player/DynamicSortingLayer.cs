using UnityEngine;

public class DynamicSortingLayer : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public PlayerManager thePlayer;
    public SpriteRenderer spriteRenderer;

    public string basicLayer;
    public string currentSortingLayer;

    // ������Ʈ�� �ڿ� ���� �� ����� ���̾� �̸�
    public string underObjectLayer;
    // ������Ʈ�� �տ� ���� �� ����� ���̾� �̸�
    public string aboveObjectLayer;

    // ������ �� �ٸ� ������Ʈ (��: �÷��̾�)
    

    public bool isColliding;
    void Start()
    {
        basicLayer = spriteRenderer.sortingLayerName;
        thePlayer = managerConnector.playerManager;
    }


    void Update()
    {
        if (thePlayer && isColliding)
        {
            string newSortingLayer = (transform.position.y > thePlayer.transform.position.y) ? underObjectLayer : aboveObjectLayer;
            currentSortingLayer = newSortingLayer;
            spriteRenderer.sortingLayerName = currentSortingLayer;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        spriteRenderer.sortingLayerName = basicLayer;
    }
}