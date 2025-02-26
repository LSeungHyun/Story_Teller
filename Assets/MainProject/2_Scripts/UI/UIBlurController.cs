using UnityEngine;
using UnityEngine.UI;

public class UIBlurController : MonoBehaviour
{
    public Camera blurCamera;       // �� ȿ���� ������ ī�޶� (SeparableBlurEffect�� ���� ī�޶�)
    public RawImage blurRawImage;   // UI�� ������� ����� RawImage
    public RenderTexture targetTexture;

    void Start()
    {
        // �ػ󵵸� ���� ���� ����ȭ ���� (��: Screen.width/2)
        //targetTexture = new RenderTexture(Screen.width, Screen.height, 16);
        blurCamera.targetTexture = targetTexture;
        blurRawImage.texture = targetTexture;
    }
}
