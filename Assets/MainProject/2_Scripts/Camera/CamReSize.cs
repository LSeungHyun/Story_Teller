using UnityEngine;

public class CamReSize : MonoBehaviour
{
    public Camera camera;

    // 목표 비율: 16:9 (1920/1080)
    private const float targetAspect = 16f / 9f;
    // 최소 뷰포트 높이와 너비 (0~1 비율)
    private const float minViewportHeight = 0.35f;
    private const float minViewportWidth = 0.35f;

    // 이전 화면 방향을 저장할 변수
    private ScreenOrientation lastOrientation;
    // 또는 이전 해상도를 저장할 수 있음
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
        // 방법 1: Screen.orientation이 바뀌었는지 확인
        if (Screen.orientation != lastOrientation)
        {
            AdjustCameraViewport();
            lastOrientation = Screen.orientation;
        }

        // 방법 2: 해상도가 바뀌었는지 확인 (방향 회전 시 해상도가 변경됨)
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
            Debug.LogError("메인 카메라를 찾을 수 없습니다.");
            return;
        }

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        Rect rect = cam.rect;

        if (scaleHeight < 1.0f)
        {
            // 화면 비율이 좁은 경우: 위/아래에 레터박스
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            // 화면 비율이 넓은 경우: 좌우에 필러박스
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        // 최소 크기 제한: 너무 작으면 강제로 고정
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
        Debug.Log("조정된 카메라 뷰포트: " + rect);
    }
}
