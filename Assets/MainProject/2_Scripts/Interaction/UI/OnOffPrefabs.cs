using Photon.Pun;
using UnityEngine;

public class OnOffPrefabs : MonoBehaviour
{
    public UIManager uIManager;
    public ManagerConnector managerConnector;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var session = GameManager.Instance.Session;

        session.OnOffPrefabsPopUp(this, collision);
        //PhotonView pv = collision.transform.GetComponent<PhotonView>();

        //PV = pv;

        //if (PV.IsMine)
        //{
        //    uIManager.OpenPopUp("Help_PopUp");
        //}
    }
}