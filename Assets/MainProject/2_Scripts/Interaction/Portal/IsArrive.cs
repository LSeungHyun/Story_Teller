using UnityEngine;

public class IsArrive : MonoBehaviour
{
    public ManagerConnector managerConnector;
    private void OnTriggerEnter2D(Collider2D collision) {
        var session = GameManager.Instance.Session;
        session.ChangePlayerisMoved(managerConnector.playerManager, true, true);
    }
}