using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("ÀÌ½Â¾ÀÂ¥ÀÜ");
        managerConnector.playerManager.gameObject.transform.position = spawnPoint.position;
    }
}
