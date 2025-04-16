using Photon.Pun;
using UnityEngine;

public class OnOffPrefabs : MonoBehaviour
{
    public UIManager uIManager;
    public ManagerConnector managerConnector;

    public bool isClickTrigger = false;
    public string prefabCode;

    public Collider2D currentCollision;

    private void Awake()
    {
        uIManager = managerConnector.uiManager;
    }

    private void Update()
    {
        if (isClickTrigger && managerConnector.playerManager != null && managerConnector.playerManager.isMove && currentCollision != null && Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.Session.OnOffPrefabsPopUp(this, currentCollision, prefabCode);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentCollision = collision;

        if (!isClickTrigger)
        {
            GameManager.Instance.Session.OnOffPrefabsPopUp(this, collision, prefabCode);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == currentCollision)
        {
            currentCollision = null;
        }
    }
}
