using UnityEngine;
using UnityEngine.UI;

public class WebGLBtn : MonoBehaviour
{
    public bool isClick;
    public Image image;
    public Sprite buttonDefaultImage;
    public Sprite buttonActiontImage;
    public SoundContainer soundContainer;

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
        if (soundContainer != null)
        {
            soundContainer.soundManager.Play("Click_Sound");
        }
        image.sprite = buttonActiontImage;
        Invoke(nameof(ResetClick), 0.1f);
    }

    // Ŭ�� ���¸� �ʱ�ȭ�ϴ� �޼���
    private void ResetClick()
    {
        isClick = false;
        image.sprite = buttonDefaultImage;
    }
}