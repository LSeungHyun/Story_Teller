using UnityEngine;

public class IsArrive : MonoBehaviour
{
    public ManagerConnector managerConnector;
    private void OnTriggerEnter2D(Collider2D collision) {
        managerConnector.playerManager.isMove = true;
    }
}