using UnityEngine;
using UnityEngine.UI;

public class UIBlurController : MonoBehaviour
{
    public Camera blurCamera;       // 블러 효과를 적용할 카메라 (SeparableBlurEffect가 붙은 카메라)
    public RawImage blurRawImage;   // UI에 배경으로 사용할 RawImage
    public RenderTexture targetTexture;

    void Start()
    {
        // 해상도를 낮춰 성능 최적화 가능 (예: Screen.width/2)
        //targetTexture = new RenderTexture(Screen.width, Screen.height, 16);
        blurCamera.targetTexture = targetTexture;
        blurRawImage.texture = targetTexture;
    }
}
