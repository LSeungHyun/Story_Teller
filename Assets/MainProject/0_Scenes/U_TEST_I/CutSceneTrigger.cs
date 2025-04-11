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
    public AbsctractGameSession session;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        session = GameManager.Instance.Session;
        UIManager = managerConnector.uiManager;

        if (cutsceneDirector != null)
        {
            cutScenePlayer.player = managerConnector.playerManager;
            cutScenePlayer.UIManager = UIManager;

            UIManager.CutSceneOnOff(false);

            StartCoroutine(FadeInOut());
        }
        else
        {
            Debug.LogError("PlayableDirector가 할당되지 않았습니다.");
        }
    }

    IEnumerator FadeInOut()
    {
        CutScene_Fade.gameObject.SetActive(true);

        yield return CutScene_Fade.DOFade(1f, fadeDuration)
                                  .SetEase(fadeEase)
                                  .WaitForCompletion();

        session.CutSceneEnter(managerConnector.playerManager, true);

        yield return new WaitForSeconds(0.3f);

        session.CutScenePlayerValue(this.transform, managerConnector.playerManager, true);

        cutsceneDirector.Play();
        SoundManager.Instance.Play("FIXED FOCUS_dry flower");

        yield return CutScene_Fade.DOFade(0f, fadeDuration)
                                  .SetEase(fadeEase)
                                  .WaitForCompletion();

        CutScene_Fade.gameObject.SetActive(false);
    }
}
