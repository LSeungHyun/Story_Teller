using UnityEngine;
using UnityEngine.UI;

public class SetAlphaHitTest : MonoBehaviour
{
    [Range(0f, 1f)]
    public float alphaThreshold = 0.1f;

    public Image img;
    private void Awake()
    {
        //Image img = GetComponent<Image>();
        if (img != null)
        {
            img.alphaHitTestMinimumThreshold = alphaThreshold;
        }
    }
}