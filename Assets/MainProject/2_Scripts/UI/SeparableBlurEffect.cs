using UnityEngine;

[ExecuteInEditMode]
public class SeparableBlurEffect : MonoBehaviour
{
    public Material blurMaterial; // 위에서 생성한 BlurMaterial

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // 임시 RenderTexture를 다운샘플링(옵션) 없이 생성할 수도 있고, 필요시 해상도를 줄여 생성합니다.
        RenderTexture temp = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

        // Pass 0: 수평 블러 적용 (셰이더의 첫번째 Pass 사용)
        Graphics.Blit(source, temp, blurMaterial, 0);

        // Pass 1: 수직 블러 적용 (셰이더의 두번째 Pass 사용)
        Graphics.Blit(temp, destination, blurMaterial, 1);

        RenderTexture.ReleaseTemporary(temp);
    }
}
