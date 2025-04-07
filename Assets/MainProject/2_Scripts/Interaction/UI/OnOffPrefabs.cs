using Photon.Pun;
using UnityEngine;

public class OnOffPrefabs : MonoBehaviour
{
    public UIManager uIManager;
    public PhotonView PV;
    void OnTriggerEnter2D()
    {
        if (PV != null && PV.IsMine)
        {
            uIManager.OpenPopUp("Help_PopUp");
        }
        else
        {
            uIManager.OpenPopUp("Help_PopUp");
        }
    }
}