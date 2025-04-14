using System.Collections;
using UnityEngine;

public class IsArrive : MonoBehaviour
{
    public CamBoundContainer camBoundContainer;
    public ManagerConnector managerConnector;
    public float delayTime = 2f; // �ε� UI�� ������ ���� �ð�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var session = GameManager.Instance.Session;

        if (managerConnector != null && managerConnector.textDataManager.loadingUI.activeSelf)
        {
            // TextDataManager�� �ڷ�ƾ�� ���� (�� �ڷ�ƾ ������ �ε�UI�� 2�� �� ��Ȱ��ȭ�˴ϴ�)
            StartCoroutine(managerConnector.textDataManager.ActiveLoadingUI());
            // 2�� �Ŀ� �� ���ӿ�����Ʈ�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ ����
            StartCoroutine(DisableAfterDelay(delayTime));
        }
        if (camBoundContainer != null)
        {
            session.SetCamValue(camBoundContainer.camDontDes, camBoundContainer.boundCol, camBoundContainer.lensSize);
        }
    }

    // delayTime ��ŭ ��� �� ���ӿ�����Ʈ�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}