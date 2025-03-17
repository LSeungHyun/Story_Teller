using UnityEngine;

public class DynamicSortingLayer : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public PlayerManager thePlayer;
    public SpriteRenderer spriteRenderer;

    public string basicLayer;
    public string currentSortingLayer;

    // 오브젝트가 뒤에 있을 때 사용할 레이어 이름
    public string underObjectLayer;
    // 오브젝트가 앞에 있을 때 사용할 레이어 이름
    public string aboveObjectLayer;

    // 기준이 될 다른 오브젝트 (예: 플레이어)
    

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