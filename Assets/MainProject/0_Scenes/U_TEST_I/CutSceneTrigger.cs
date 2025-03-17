using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class CutsceneTrigger : DoTweenManager
{
    public PlayableDirector cutsceneDirector;

    public Image CutScene_Fade;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneDirector != null)
            {
                StartCoroutine(FadeInOut());
            }
            else
            {
                Debug.LogError("PlayableDirector가 할당되지 않았습니다.");
            }
        }
    }

    IEnumerator FadeInOut()
    {
        CutScene_Fade.gameObject.SetActive(true);

        yield return CutScene_Fade.DOFade(1f, fadeDuration)
                                  .SetEase(fadeEase)
                                  .WaitForCompletion();

        yield return new WaitForSeconds(0.3f);

        cutsceneDirector.Play();

        yield return CutScene_Fade.DOFade(0f, fadeDuration)
                                  .SetEase(fadeEase)
                                  .WaitForCompletion();

        CutScene_Fade.gameObject.SetActive(false);
    }
}
