using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PortalManager : MonoBehaviour
{
    //public PortalSetter portalSetter;

    public ManagerConnector managerConnector;
    public Vector3 spawnAt;
    public CamBoundContainer camBoundContainer;

    public Image CutScene_Fade;
    public float fadeDuration = 1f;
    public Ease fadeEase = Ease.Linear;
    public AbsctractGameSession session;
    public bool isNextMap;


    public UIManager uiManager;
    private void Awake()
    {
        session = GameManager.Instance.Session;
        managerConnector.portalManager = this;
        if (uiManager != null) return;
        uiManager = managerConnector.uiManager;
    }

    public void OnEnable()
    {
        uiManager.RapidCloseAllUI();

        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        CutScene_Fade.gameObject.SetActive(true);

        yield return CutScene_Fade.DOFade(1f, fadeDuration)
                                  .SetEase(fadeEase)
                                  .WaitForCompletion();

        session.SetCamValue(camBoundContainer.camDontDes, camBoundContainer.boundCol, camBoundContainer.lensSize);
        isNextMap = true;
        session.MovePlayers(this);

        yield return new WaitForSeconds(0.3f);
       
        yield return CutScene_Fade.DOFade(0f, fadeDuration)
                                  .SetEase(fadeEase)
                                  .WaitForCompletion();

        CutScene_Fade.gameObject.SetActive(false);
    }
}
