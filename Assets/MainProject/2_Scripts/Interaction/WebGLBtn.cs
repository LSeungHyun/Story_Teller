using UnityEngine;
using UnityEngine.UI;

public class WebGLBtn : MonoBehaviour
{
    public bool isClick;
    public Image image;
    public Sprite buttonDefaultImage;
    public Sprite buttonActiontImage;

    private void Awake()
    {
        ResetClick();
    }

    private void OnEnable()
    {
        ResetClick();
    }

    public void PressBtnF()
    {
        isClick = true;
        image.sprite = buttonActiontImage;
        Invoke(nameof(ResetClick), 0.1f);
    }

    // 클릭 상태를 초기화하는 메서드
    private void ResetClick()
    {
        isClick = false;
        image.sprite = buttonDefaultImage;
    }
}