using UnityEngine;
using UnityEngine.EventSystems;  // �ݵ�� �߰�
using DG.Tweening;

public class DoTweenManager : MonoBehaviour
{
    [Header("Animation Settings")]
    public float shrinkScale = 0.9f;   // ������ �� ���� ����
    public float tweenDuration = 0.1f; // ���/Ȯ�� �ð�
    public Ease easeType = Ease.OutQuad;

    // ��� ��ư OnClick���� �� �޼��带 ȣ��
    public void ClickAnim()
    {
        // 1) ���� Ŭ���� UI ������Ʈ ��������
        GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
        if (clickedObj == null)
        {
            Debug.LogWarning("No currentSelectedGameObject found.");
            return;
        }

        // 2) RectTransform ��������
        RectTransform rect = clickedObj.GetComponent<RectTransform>();
        if (rect == null)
        {
            Debug.LogWarning("Clicked object has no RectTransform.");
            return;
        }

        // 3) ���� Ʈ���� �ߴ�
        rect.DOKill();

        // 4) ��� �� �ٽ� Ȯ��
        rect.DOScale(shrinkScale, tweenDuration).SetEase(easeType)
            .OnComplete(() =>
            {
                rect.DOScale(1f, tweenDuration).SetEase(easeType);
            });

        Debug.Log("Ŭ���ִϸ��̼� �Ϸ� for " + clickedObj.name);
    }
}