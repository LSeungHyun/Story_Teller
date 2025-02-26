using UnityEngine;

[ExecuteInEditMode]
public class SeparableBlurEffect : MonoBehaviour
{
    public Material blurMaterial; // ������ ������ BlurMaterial

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // �ӽ� RenderTexture�� �ٿ���ø�(�ɼ�) ���� ������ ���� �ְ�, �ʿ�� �ػ󵵸� �ٿ� �����մϴ�.
        RenderTexture temp = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

        // Pass 0: ���� �� ���� (���̴��� ù��° Pass ���)
        Graphics.Blit(source, temp, blurMaterial, 0);

        // Pass 1: ���� �� ���� (���̴��� �ι�° Pass ���)
        Graphics.Blit(temp, destination, blurMaterial, 1);

        RenderTexture.ReleaseTemporary(temp);
    }
}
