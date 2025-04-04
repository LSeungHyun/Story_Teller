using UnityEngine;

public class IsArrive : MonoBehaviour
{
    public CamBoundContainer camBoundContainer;
    public ManagerConnector managerConnector;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var session = GameManager.Instance.Session;

        if (managerConnector != null && managerConnector.textDataManager.loadingUI.activeSelf)
        {
            managerConnector.textDataManager.loadingUI.SetActive(false);
        }
        if (camBoundContainer != null)
        {
            session.SetCamValue(camBoundContainer.camDontDes, camBoundContainer.boundCol, camBoundContainer.lensSize);
        }
        this.gameObject.SetActive(false);
    }
}