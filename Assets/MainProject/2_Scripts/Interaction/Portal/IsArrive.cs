using System.Collections;
using UnityEngine;

public class IsArrive : MonoBehaviour
{
    public CamBoundContainer camBoundContainer;
    public ManagerConnector managerConnector;
    public float delayTime = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy || !enabled)
            return;

        var session = GameManager.Instance.Session;

        if (managerConnector != null
            && managerConnector.textDataManager.loadingUI.activeSelf
            && managerConnector.textDataManager.gameObject.activeInHierarchy)
        {
            managerConnector.textDataManager.StartCoroutine(managerConnector.textDataManager.ActiveLoadingUI());
        }

        if (camBoundContainer != null)
        {
            session.SetCamValue(camBoundContainer.camDontDes, camBoundContainer.boundCol, camBoundContainer.lensSize);
        }

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DisableAfterDelay(delayTime));
        }
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }
}
