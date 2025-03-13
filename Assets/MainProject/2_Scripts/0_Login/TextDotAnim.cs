using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextDotAnim : MonoBehaviour
{
    public Text Loadingtext;
    void Start()
    {
        StartCoroutine(DotAnimation("Loading"));
    }

    public IEnumerator DotAnimation(string text)
    {
        int dotCount = 0;

        while (true)
        {
            dotCount++;
            if (dotCount > 3) dotCount = 1;

            Loadingtext.text = text + new string('.', dotCount);
            yield return new WaitForSeconds(1f);
        }
    }
}
