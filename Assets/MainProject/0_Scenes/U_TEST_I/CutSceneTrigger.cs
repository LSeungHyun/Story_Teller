using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class CutsceneTrigger : DoTweenManager
{
    public PlayableDirector cutsceneDirector;
    public CutScenePlayer cutScenePlayer;
    public UIManager UIManager;

    public ManagerConnector managerConnector;
    public PlayerManager player;

    public Image CutScene_Fade;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager = managerConnector.uiManager;

            player = other.gameObject.GetComponent<PlayerManager>();

            if (cutsceneDirector != null)
            {
                cutScenePlayer.player = player;
                cutScenePlayer.UIManager = UIManager;

                UIManager.CutSceneOnOff(false);

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

        player.playerSprite.enabled = false;
        player.playerNickname.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        player.transform.position = this.transform.position;
        player.anim.SetFloat("DirX", 0);
        player.anim.SetFloat("DirY", 1);
        cutsceneDirector.Play();
        SoundManager.Instance.Play("FIXED FOCUS_dry flower");

        yield return CutScene_Fade.DOFade(0f, fadeDuration)
                                  .SetEase(fadeEase)
                                  .WaitForCompletion();

        CutScene_Fade.gameObject.SetActive(false);
    }
}
