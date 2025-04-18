using Photon.Pun;
using UnityEngine;

public class OnOffPrefabs : MonoBehaviour
{
    public UIManager uIManager;
    public ManagerConnector managerConnector;

    private void Awake()
    {
        uIManager = managerConnector.uiManager;
    }

    public void HandlePrefabInteraction(string currentObjCode)
    {
        GameManager.Instance.Session.OnOffPrefabsPopUp(this, currentObjCode);
    }
}
