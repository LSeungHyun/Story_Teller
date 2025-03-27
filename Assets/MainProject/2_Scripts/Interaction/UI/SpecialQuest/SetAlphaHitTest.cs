using UnityEngine;
using UnityEngine.UI;

// Image를 상속하고, ICanvasRaycastFilter를 구현하여 픽셀 알파 기반 필터링
//[RequireComponent(typeof(Image))]
public class SetAlphaHitTest : Image, ICanvasRaycastFilter
{
    [Range(0f, 1f)]
    public float alphaThreshold = 0.1f; // 알파값 임계치

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (overrideSprite == null)
            return true; // 스프라이트가 없으면 그냥 통과

        // 스크린 좌표 → 이 Image의 로컬 좌표
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, screenPoint, eventCamera, out var localPoint))
        {
            return false;
        }

        // 픽셀 보정된 Rect
        Rect pixelRect = GetPixelAdjustedRect();

        // 로컬 좌표를 [0..1] 범위로 변환
        float x = (localPoint.x - pixelRect.x) / pixelRect.width;
        float y = (localPoint.y - pixelRect.y) / pixelRect.height;

        // 범위를 벗어나면 클릭 불가
        if (x < 0f || x > 1f || y < 0f || y > 1f)
            return false;

        // 실제 스프라이트 텍스처의 해당 픽셀 알파 확인
        Texture2D tex = overrideSprite.texture;
        Color c = tex.GetPixelBilinear(x, y);

        // 알파값이 임계치 이상이면 클릭 가능
        return (c.a >= alphaThreshold);
    }
}
