using UnityEngine;
using UnityEngine.UI;

public class RLSlider : MonoBehaviour
{
    public Sprite[] images;
    public Image displayImage;
    public int currentIndex = 0;

    public Text indexDisplay;

    void Start()
    {
        if (images != null && images.Length > 0)
        {
            displayImage.sprite = images[currentIndex];
            indexDisplay.text = "1";
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
        indexDisplay.text = $"{currentIndex + 1}";
    }
    public void onPrevBtn()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = images.Length - 1;
        }
        displayImage.sprite = images[currentIndex];
        indexDisplay.text = $"{currentIndex + 1}";
    }

}
