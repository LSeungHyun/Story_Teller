using UnityEngine;

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
    

    public bool isColliding;
    void Start()
    {
        basicLayer = spriteRenderer.sortingLayerName;
        //thePlayer = managerConnector.playerManager;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        spriteRenderer.sortingLayerName = basicLayer;
        SetLayerName(basicLayer);
    }

    public void SetLayerName(string layerName)
    {
        for (int i = 0; i < childSpriteGroup.Length; i++)
        {
            childSpriteGroup[i].sortingLayerName = layerName;
        }
    }
}