using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public Transform spawnPoint;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        managerConnector.playerManager.gameObject.transform.position = spawnPoint.position;
    }
}
