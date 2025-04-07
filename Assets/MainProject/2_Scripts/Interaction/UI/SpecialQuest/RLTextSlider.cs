using UnityEngine;
using UnityEngine.UI;

public class RLTextSlider : MonoBehaviour
{
    [TextArea]
    public string[] texts;
    public Text displayText;
    private int currentIndex = 0;
    public Text indexDisplay;

    void Start()
    {
        if (texts != null && texts.Length > 0)
        {
            displayText.text = texts[currentIndex];
            indexDisplay.text = "1";
        }
    }

    public void OnNextBtn()
    {
        currentIndex++;
        if (currentIndex >= texts.Length)
        {
            currentIndex = 0;
        }
        displayText.text = texts[currentIndex];
    indexDisplay.text = $"{currentIndex + 1}";
    }

    public void OnPrevBtn()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = texts.Length - 1;
        }
        displayText.text = texts[currentIndex];
        indexDisplay.text = $"{currentIndex + 1}";
    }
}
