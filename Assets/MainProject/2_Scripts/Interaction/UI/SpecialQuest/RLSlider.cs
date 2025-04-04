using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    public Sprite[] images;
    public Image displayImage;
    private int currentIndex = 0;

    void Start()
    {
        if (images != null && images.Length > 0)
        {
            displayImage.sprite = images[currentIndex];
        }
    }

    public void onNextBtn()
    {
            currentIndex++;
            if (currentIndex >= images.Length)
            {
                currentIndex = 0;
            }
            displayImage.sprite = images[currentIndex];
    }
    public void onPrevBtn()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = images.Length - 1;
        }
        displayImage.sprite = images[currentIndex];
    }

}
