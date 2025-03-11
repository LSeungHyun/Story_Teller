using UnityEngine;
using UnityEngine.UI;

public class OverlayCamSyncWithClamp : MonoBehaviour
{
    // 메인 카메라 (고정 비율 코드가 적용된 카메라)
    public Camera mainCamera;
    // Overlay 캔버스 내 실제 UI가 배치될 상위 Panel (RectTransform)
    public RectTransform overlayPanel;

    // 최소 normalized viewport 크기 (0~1 범위)
    private const float minViewportWidth = 0.35f;
    private const float minViewportHeight = 0.35f;
    // 최대 normalized viewport는 전체 화면 (즉, 1.0)
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
        // 해상도나 화면 크기가 변경되면 다시 갱신
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
            Debug.LogError("mainCamera 또는 overlayPanel이 할당되지 않았습니다.");
            return;
        }

        // 메인 카메라가 실제 렌더링하는 영역 (픽셀 단위)
        Rect camPixelRect = mainCamera.pixelRect;
        float canvasWidth = Screen.width;
        float canvasHeight = Screen.height;

        // 메인 카메라의 pixelRect를 normalized (0~1) 좌표로 변환
        float normX = camPixelRect.x / canvasWidth;
        float normY = camPixelRect.y / canvasHeight;
        float normWidth = camPixelRect.width / canvasWidth;
        float normHeight = camPixelRect.height / canvasHeight;

        // 최소와 최대 normalized 크기로 클램핑 (너무 작거나 너무 커지지 않게)
        normWidth = Mathf.Clamp(normWidth, minViewportWidth, maxViewportWidth);
        normHeight = Mathf.Clamp(normHeight, minViewportHeight, maxViewportHeight);
        
        // 만약 normalized width가 최대(1.0)라면 좌우 전체를 사용하도록 X를 0으로 고정
        if (Mathf.Approximately(normWidth, 1.0f))
            normX = 0f;
        else
            // 중앙 정렬: 여백을 양쪽에 균등하게 배분
            normX = (1f - normWidth) / 2f;
        
        // 세로도 동일하게, 만약 최대라면 Y를 0으로, 그렇지 않으면 중앙 정렬
        if (Mathf.Approximately(normHeight, 1.0f))
            normY = 0f;
        else
            normY = (1f - normHeight) / 2f;

        // normalized 좌표를 다시 픽셀 단위 offset으로 변환
        Vector2 newOffsetMin = new Vector2(normX * canvasWidth, normY * canvasHeight);
        // offsetMax는 (normalized x + normalized width)에서 전체 캔버스 크기를 뺀 값(음수)으로 설정
        Vector2 newOffsetMax = new Vector2((normX + normWidth - 1f) * canvasWidth, (normY + normHeight - 1f) * canvasHeight);

        overlayPanel.offsetMin = newOffsetMin;
        overlayPanel.offsetMax = newOffsetMax;

        Debug.Log($"Overlay Panel 동기화됨: offsetMin={newOffsetMin}, offsetMax={newOffsetMax}");
    }
}
