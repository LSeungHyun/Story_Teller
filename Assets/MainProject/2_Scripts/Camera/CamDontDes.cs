using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

public class CamDontDes : MonoBehaviour
{
    public static CamDontDes instance;

    public PhotonView PV;

    public CamBoundContainer camBoundContainer;
    public CinemachineConfiner2D confinerBound;
    public CinemachineCamera virtualCam;

    public AbsctractGameSession session;
    void Awake()
    {
        session = GameManager.Instance.Session;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

#if !(!UNITY_EDITOR && UNITY_WEBGL)
        // WebGL�� �ƴ� Unity�϶��� ���̽�ƽ off / unity �̵��Լ� ���
        virtualCam.Lens.OrthographicSize = 2.5f;

#endif
        // WebGL�̸鼭 ������϶�
        if (Application.isMobilePlatform)
            virtualCam.Lens.OrthographicSize = 3.7f;

        //WebGL�̸鼭 ��ǻ���϶�
        else
            virtualCam.Lens.OrthographicSize = 2.5f;
    }

    private void Start()
    {
        if (!GameManager.Instance.isType)
        {
            Destroy(PV);
        }
    }
    public void SetFollowCam(GameObject playerObj)
    {
        if (playerObj != null)
        {
            session.SetCamera(this, playerObj);
        }
    }
    
    public void SetCamValue(Collider2D newBound, float lens)
    {
        confinerBound.BoundingShape2D = newBound;
        virtualCam.Lens.OrthographicSize = lens;
    }
}