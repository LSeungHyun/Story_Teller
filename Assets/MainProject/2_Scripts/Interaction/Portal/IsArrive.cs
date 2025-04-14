using System.Collections;
using UnityEngine;

public class IsArrive : MonoBehaviour
{
    public CamBoundContainer camBoundContainer;
    public ManagerConnector managerConnector;
    public float delayTime = 0f; // 로딩 UI와 동일한 지연 시간

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var session = GameManager.Instance.Session;

        if (managerConnector != null && managerConnector.textDataManager.loadingUI.activeSelf)
        {
            // TextDataManager의 코루틴을 실행 (이 코루틴 내에서 로딩UI는 2초 후 비활성화됩니다)
            StartCoroutine(managerConnector.textDataManager.ActiveLoadingUI());
            // 2초 후에 이 게임오브젝트를 비활성화하는 코루틴 실행
        }
        if (camBoundContainer != null)
        {
            session.SetCamValue(camBoundContainer.camDontDes, camBoundContainer.boundCol, camBoundContainer.lensSize);
        }
        StartCoroutine(DisableAfterDelay(delayTime));
    }

    // delayTime 만큼 대기 후 게임오브젝트를 비활성화하는 코루틴
    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}