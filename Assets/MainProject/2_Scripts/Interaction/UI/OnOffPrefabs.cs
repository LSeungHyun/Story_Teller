using Photon.Pun;
using UnityEngine;

public class OnOffPrefabs : MonoBehaviour
{
    public UIManager uIManager;
    public PhotonView PV;
    void OnTriggerEnter2D()
    {
        if (PhotonNetwork.LocalPlayer == PV.Owner)
        {
            Debug.Log("≥ª≤®¿”");
            uIManager.OpenPopUp("Help_PopUp");
        }
        //else
        //{
        //    uIManager.OpenPopUp("Help_PopUp");
        //}
    }
}