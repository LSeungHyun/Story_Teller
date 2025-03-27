using UnityEngine;
using UnityEngine.UI;

// Image�� ����ϰ�, ICanvasRaycastFilter�� �����Ͽ� �ȼ� ���� ��� ���͸�
//[RequireComponent(typeof(Image))]
public class SetAlphaHitTest : Image, ICanvasRaycastFilter
{
    [Range(0f, 1f)]
    public float alphaThreshold = 0.1f; // ���İ� �Ӱ�ġ

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (overrideSprite == null)
            return true; // ��������Ʈ�� ������ �׳� ���

        // ��ũ�� ��ǥ �� �� Image�� ���� ��ǥ
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, screenPoint, eventCamera, out var localPoint))
        {
            return false;
        }

        // �ȼ� ������ Rect
        Rect pixelRect = GetPixelAdjustedRect();

        // ���� ��ǥ�� [0..1] ������ ��ȯ
        float x = (localPoint.x - pixelRect.x) / pixelRect.width;
        float y = (localPoint.y - pixelRect.y) / pixelRect.height;

        // ������ ����� Ŭ�� �Ұ�
        if (x < 0f || x > 1f || y < 0f || y > 1f)
            return false;

        // ���� ��������Ʈ �ؽ�ó�� �ش� �ȼ� ���� Ȯ��
        Texture2D tex = overrideSprite.texture;
        Color c = tex.GetPixelBilinear(x, y);

        // ���İ��� �Ӱ�ġ �̻��̸� Ŭ�� ����
        return (c.a >= alphaThreshold);
    }
}
