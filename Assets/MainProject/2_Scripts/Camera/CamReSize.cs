using UnityEngine;

public class CamSize : MonoBehaviour
{
    private float camScale;
    private void Awake()
    {
        camScale = 19.20f / 10.80f;
    }

    private void Update()
    {
        float targetAspect = camScale;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }

        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        // 추가적으로, 너무 작은 화면 비율에서 기본적인 크기를 유지
        if (camera.rect.height < 0.35f) // 예: 0.5f 이하로 작아지면 강제로 0.5f로 고정
        {
            Rect rect = camera.rect;
            rect.height = 0.35f;
            rect.y = (1.0f - 0.35f) / 2.0f;
            camera.rect = rect;
        }
    }

}
