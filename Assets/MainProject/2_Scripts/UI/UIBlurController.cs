using UnityEngine;
using UnityEngine.UI;

public class SubUIBlurController : MonoBehaviour
{
    [Header("UI Components")]
    public RawImage blurPanel; // 서브 UI 패널 내에 배치된 RawImage (BlurPanel)

    [Header("Blur Settings")]
    public Material blurMaterial;   // 위에서 생성한 블러 머티리얼
    [Range(0, 10)]
    public float blurSize = 2.0f;     // 블러 강도 (원하는 값으로 조절)

    [Header("Camera Settings")]
    public Camera uiCamera;       // UI를 렌더링하는 카메라 (Screen Space - Camera 모드 권장)
    private RenderTexture renderTexture;

    void Start()
    {
        // RenderTexture 해상도를 다운샘플링하여 성능 최적화 (예: 화면 해상도의 1/2)
        int rtWidth = Screen.width / 2;
        int rtHeight = Screen.height / 2;
        renderTexture = new RenderTexture(rtWidth, rtHeight, 16);

        // UI 카메라에 RenderTexture를 할당하여 배경 캡쳐 준비
        uiCamera.targetTexture = renderTexture;

        // BlurPanel에 캡쳐된 텍스처와 블러 머티리얼 할당
        blurPanel.texture = renderTexture;
        blurPanel.material = blurMaterial;
        blurPanel.material.SetFloat("_BlurSize", blurSize);

        // 기본적으로 서브 UI가 열릴 때는 BlurPanel을 비활성화 (첫 UI는 그대로)
        blurPanel.gameObject.SetActive(false);
    }

    // 서브 UI 열릴 때 호출하는 함수
    public void OpenSubUI()
    {
        // 캡쳐된 RenderTexture는 uiCamera가 렌더링하면서 업데이트됨
        // 필요한 경우, uiCamera.Render()를 호출하여 강제로 최신 이미지를 캡쳐할 수 있습니다.
        uiCamera.Render();

        // 블러 파라미터 업데이트
        blurPanel.material.SetFloat("_BlurSize", blurSize);

        // BlurPanel 활성화하여 배경 블러 효과 적용 (첫 UI 위에 덮어쓰기)
        blurPanel.gameObject.SetActive(true);

        // 이후 서브 UI 콘텐츠를 활성화하거나 추가 UI 로직을 실행합니다.
    }

    // 서브 UI 닫을 때 호출하는 함수
    public void CloseSubUI()
    {
        // BlurPanel을 비활성화하면 첫번째 UI가 원래대로 보입니다.
        blurPanel.gameObject.SetActive(false);

        // 필요시 uiCamera.targetTexture를 해제할 수도 있습니다.
    }
}
