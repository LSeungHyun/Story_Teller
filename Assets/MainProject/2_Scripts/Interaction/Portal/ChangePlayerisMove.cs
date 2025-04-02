using UnityEngine;

public class ChangePlayerisMove : MonoBehaviour
{
    public ManagerConnector managerConnector;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var session = GameManager.Instance.Session;

        if (managerConnector != null)
        {
            session.ChangePlayerisMoved(managerConnector.playerManager, true, false);
        }

        this.gameObject.SetActive(false);
    }
}
