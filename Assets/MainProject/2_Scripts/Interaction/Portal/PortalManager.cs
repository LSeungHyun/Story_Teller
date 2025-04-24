using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PortalManager : MonoBehaviour
{
    public AbsctractGameSession session;

    public UIManager uiManager;

    public ManagerConnector managerConnector;  
    public CamBoundContainer camBoundContainer;

    public GameObject centerLabelGroup;
    public Image CutScene_Fade;
    public float fadeDuration = 1f;
    public Ease fadeEase = Ease.Linear;

    public Vector3 spawnAt;

    public bool isNextMap;

    
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
        if (centerLabelGroup == null)
        {
            centerLabelGroup = managerConnector.uiCenterLabelSetter.uiCenterLabelOnOffManager.centerLabelGroup;
        }
        if (CutScene_Fade == null)
        {
            CutScene_Fade = managerConnector.FadeImage;
        }
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

        if(centerLabelGroup != null)
        {
            centerLabelGroup.SetActive(true);
            managerConnector.uiCenterLabelSetter.SetData("Arrive");
        }
        
        CutScene_Fade.gameObject.SetActive(false);
    }
}
