using UnityEngine;

public class OnOffPrefabs : MonoBehaviour
{
    public UIManager uIManager;
    public ManagerConnector managerConnector;
    void OnTriggerEnter2D()
    {
        if (managerConnector.PV.IsMine)
        {
            uIManager.OpenPopUp("Help_PopUp");
        }
    }
}