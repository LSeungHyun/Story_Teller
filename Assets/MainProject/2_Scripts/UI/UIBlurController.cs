using UnityEngine;
using UnityEngine.UI;

public class SubUIBlurController : MonoBehaviour
{
    [Header("UI Components")]
    public RawImage blurPanel; // ���� UI �г� ���� ��ġ�� RawImage (BlurPanel)

    [Header("Blur Settings")]
    public Material blurMaterial;   // ������ ������ �� ��Ƽ����
    [Range(0, 10)]
    public float blurSize = 2.0f;     // �� ���� (���ϴ� ������ ����)

    [Header("Camera Settings")]
    public Camera uiCamera;       // UI�� �������ϴ� ī�޶� (Screen Space - Camera ��� ����)
    private RenderTexture renderTexture;

    void Start()
    {
        // RenderTexture �ػ󵵸� �ٿ���ø��Ͽ� ���� ����ȭ (��: ȭ�� �ػ��� 1/2)
        int rtWidth = Screen.width / 2;
        int rtHeight = Screen.height / 2;
        renderTexture = new RenderTexture(rtWidth, rtHeight, 16);

        // UI ī�޶� RenderTexture�� �Ҵ��Ͽ� ��� ĸ�� �غ�
        uiCamera.targetTexture = renderTexture;

        // BlurPanel�� ĸ�ĵ� �ؽ�ó�� �� ��Ƽ���� �Ҵ�
        blurPanel.texture = renderTexture;
        blurPanel.material = blurMaterial;
        blurPanel.material.SetFloat("_BlurSize", blurSize);

        // �⺻������ ���� UI�� ���� ���� BlurPanel�� ��Ȱ��ȭ (ù UI�� �״��)
        blurPanel.gameObject.SetActive(false);
    }

    // ���� UI ���� �� ȣ���ϴ� �Լ�
    public void OpenSubUI()
    {
        // ĸ�ĵ� RenderTexture�� uiCamera�� �������ϸ鼭 ������Ʈ��
        // �ʿ��� ���, uiCamera.Render()�� ȣ���Ͽ� ������ �ֽ� �̹����� ĸ���� �� �ֽ��ϴ�.
        uiCamera.Render();

        // �� �Ķ���� ������Ʈ
        blurPanel.material.SetFloat("_BlurSize", blurSize);

        // BlurPanel Ȱ��ȭ�Ͽ� ��� �� ȿ�� ���� (ù UI ���� �����)
        blurPanel.gameObject.SetActive(true);

        // ���� ���� UI �������� Ȱ��ȭ�ϰų� �߰� UI ������ �����մϴ�.
    }

    // ���� UI ���� �� ȣ���ϴ� �Լ�
    public void CloseSubUI()
    {
        // BlurPanel�� ��Ȱ��ȭ�ϸ� ù��° UI�� ������� ���Դϴ�.
        blurPanel.gameObject.SetActive(false);

        // �ʿ�� uiCamera.targetTexture�� ������ ���� �ֽ��ϴ�.
    }
}
