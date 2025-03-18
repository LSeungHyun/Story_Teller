using UnityEngine;
using UnityEngine.EventSystems;  // �ݵ�� �߰�
using DG.Tweening;

public class DoTweenManager : MonoBehaviour
{
    public SoundContainer SoundContainer;

    [Header("Animation Settings")]
    public float shrinkScale = 0.9f;   // ������ �� ���� ����
    public float tweenDuration = 0.1f; // ���/Ȯ�� �ð�
    public Ease easeType = Ease.OutQuad;

    // Fade ����
    [Header("DOTween Settings")]
    public float fadeDuration = 1f; // �ΰ� ���̵� ��/�ƿ� �ð�
    public float scaleDuration = 0.3f; // �˾� ���� ���� �� ������ �ִϸ��̼� �ð�
    public Ease fadeEase = Ease.Linear;
    public Ease scaleEase = Ease.OutBack;

    // ��� ��ư OnClick���� �� �޼��带 ȣ��
    public void ClickAnim()
    {
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

        //SoundContainer.soundManager.Play("button_sound");

        // 3) ���� Ʈ���� �ߴ�
        rect.DOKill();

        // 4) ��� �� �ٽ� Ȯ��
        rect.DOScale(shrinkScale, tweenDuration).SetEase(easeType)
            .OnComplete(() =>
            {
                rect.DOScale(1f, tweenDuration).SetEase(easeType);
            });

        //Debug.Log("Ŭ���ִϸ��̼� �Ϸ� for " + clickedObj.name);
    }

    /// <summary>
    /// �˾�(�г�) ������ �� (������ 0��1)
    /// </summary>
    public void ShowUI(GameObject ui)
    {
        // ���� Ȱ��ȭ
        ui.SetActive(true);

        RectTransform rect = ui.GetComponent<RectTransform>();
        if (rect == null) return;

        // ���� Ʈ������ ������ �ߴ�
        rect.DOKill();

        // �������� 0���� ����
        rect.localScale = Vector3.zero;

        // DOTween �ִϸ��̼�: 0 -> 1
        rect.DOScale(1f, scaleDuration)
            .SetEase(scaleEase);
    }

    /// <summary>
    /// �˾�(�г�) ���� �� (������ 1��0)
    /// </summary>
    public void HideUI(GameObject ui)
    {
        RectTransform rect = ui.GetComponent<RectTransform>();
        if (rect == null) return;

        rect.DOKill();

        // ����: 1 -> 0, Ease.InBack
        rect.DOScale(0f, 0.125f)
            .SetEase(Ease.InBack)
            .OnComplete(() => ui.SetActive(false));
    }
}