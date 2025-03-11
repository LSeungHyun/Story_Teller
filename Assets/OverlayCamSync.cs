using UnityEngine;
using UnityEngine.UI;

public class OverlayCamSyncWithClamp : MonoBehaviour
{
    // ���� ī�޶� (���� ���� �ڵ尡 ����� ī�޶�)
    public Camera mainCamera;
    // Overlay ĵ���� �� ���� UI�� ��ġ�� ���� Panel (RectTransform)
    public RectTransform overlayPanel;

    // �ּ� normalized viewport ũ�� (0~1 ����)
    private const float minViewportWidth = 0.35f;
    private const float minViewportHeight = 0.35f;
    // �ִ� normalized viewport�� ��ü ȭ�� (��, 1.0)
    private const float maxViewportWidth = 1.0f;
    private const float maxViewportHeight = 1.0f;

    private int lastScreenWidth;
    private int lastScreenHeight;

    void Start()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        UpdateOverlayPanel();
    }

    void Update()
    {
        // �ػ󵵳� ȭ�� ũ�Ⱑ ����Ǹ� �ٽ� ����
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            UpdateOverlayPanel();
        }
    }

    void UpdateOverlayPanel()
    {
        if (mainCamera == null || overlayPanel == null)
        {
            Debug.LogError("mainCamera �Ǵ� overlayPanel�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // ���� ī�޶� ���� �������ϴ� ���� (�ȼ� ����)
        Rect camPixelRect = mainCamera.pixelRect;
        float canvasWidth = Screen.width;
        float canvasHeight = Screen.height;

        // ���� ī�޶��� pixelRect�� normalized (0~1) ��ǥ�� ��ȯ
        float normX = camPixelRect.x / canvasWidth;
        float normY = camPixelRect.y / canvasHeight;
        float normWidth = camPixelRect.width / canvasWidth;
        float normHeight = camPixelRect.height / canvasHeight;

        // �ּҿ� �ִ� normalized ũ��� Ŭ���� (�ʹ� �۰ų� �ʹ� Ŀ���� �ʰ�)
        normWidth = Mathf.Clamp(normWidth, minViewportWidth, maxViewportWidth);
        normHeight = Mathf.Clamp(normHeight, minViewportHeight, maxViewportHeight);
        
        // ���� normalized width�� �ִ�(1.0)��� �¿� ��ü�� ����ϵ��� X�� 0���� ����
        if (Mathf.Approximately(normWidth, 1.0f))
            normX = 0f;
        else
            // �߾� ����: ������ ���ʿ� �յ��ϰ� ���
            normX = (1f - normWidth) / 2f;
        
        // ���ε� �����ϰ�, ���� �ִ��� Y�� 0����, �׷��� ������ �߾� ����
        if (Mathf.Approximately(normHeight, 1.0f))
            normY = 0f;
        else
            normY = (1f - normHeight) / 2f;

        // normalized ��ǥ�� �ٽ� �ȼ� ���� offset���� ��ȯ
        Vector2 newOffsetMin = new Vector2(normX * canvasWidth, normY * canvasHeight);
        // offsetMax�� (normalized x + normalized width)���� ��ü ĵ���� ũ�⸦ �� ��(����)���� ����
        Vector2 newOffsetMax = new Vector2((normX + normWidth - 1f) * canvasWidth, (normY + normHeight - 1f) * canvasHeight);

        overlayPanel.offsetMin = newOffsetMin;
        overlayPanel.offsetMax = newOffsetMax;

        Debug.Log($"Overlay Panel ����ȭ��: offsetMin={newOffsetMin}, offsetMax={newOffsetMax}");
    }
}
