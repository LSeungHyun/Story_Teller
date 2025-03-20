using System.Collections;
using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

public class CamDontDes : MonoBehaviour
{
    public static CamDontDes instance;

    public CamBoundContainer camBoundContainer;
    public CinemachineConfiner2D confinerBound;
    public CinemachineCamera virtualCam;

    void Awake()
    {
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


    public void SetFollowCam(GameObject playerObj)
    {
        if (playerObj != null)
        {
            virtualCam.Follow = playerObj.transform;
            virtualCam.LookAt = playerObj.transform;
        }
    }
    
    public void SetBound(Collider2D newBound)
    {
        confinerBound.BoundingShape2D = newBound;
    }

    public void SetLensSize(float lens)
    {
        virtualCam.Lens.OrthographicSize = lens;
    }
}