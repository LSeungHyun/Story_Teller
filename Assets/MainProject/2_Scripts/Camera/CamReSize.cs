using UnityEngine;

public class CamReSize : MonoBehaviour
{
    public Camera camera;

    // ��ǥ ����: 16:9 (1920/1080)
    private const float targetAspect = 16f / 9f;
    // �ּ� ����Ʈ ���̿� �ʺ� (0~1 ����)
    private const float minViewportHeight = 0.35f;
    private const float minViewportWidth = 0.35f;

    // ���� ȭ�� ������ ������ ����
    private ScreenOrientation lastOrientation;
    // �Ǵ� ���� �ػ󵵸� ������ �� ����
    private int lastScreenWidth;
    private int lastScreenHeight;

    void Start()
    {
        lastOrientation = Screen.orientation;
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        AdjustCameraViewport();
    }

    void Update()
    {
        // ��� 1: Screen.orientation�� �ٲ������ Ȯ��
        if (Screen.orientation != lastOrientation)
        {
            AdjustCameraViewport();
            lastOrientation = Screen.orientation;
        }

        // ��� 2: �ػ󵵰� �ٲ������ Ȯ�� (���� ȸ�� �� �ػ󵵰� �����)
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            AdjustCameraViewport();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
        }
    }

    void AdjustCameraViewport()
    {
        Camera cam = camera;
        if (cam == null)
        {
            Debug.LogError("���� ī�޶� ã�� �� �����ϴ�.");
            return;
        }

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        Rect rect = cam.rect;

        if (scaleHeight < 1.0f)
        {
            // ȭ�� ������ ���� ���: ��/�Ʒ��� ���͹ڽ�
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            // ȭ�� ������ ���� ���: �¿쿡 �ʷ��ڽ�
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        // �ּ� ũ�� ����: �ʹ� ������ ������ ����
        if (rect.height < minViewportHeight)
        {
            rect.height = minViewportHeight;
            rect.y = (1.0f - minViewportHeight) / 2.0f;
        }
        if (rect.width < minViewportWidth)
        {
            rect.width = minViewportWidth;
            rect.x = (1.0f - minViewportWidth) / 2.0f;
        }

        cam.rect = rect;
        Debug.Log("������ ī�޶� ����Ʈ: " + rect);
    }
}
