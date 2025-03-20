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
        // WebGL이 아닌 Unity일때는 조이스틱 off / unity 이동함수 사용
        virtualCam.Lens.OrthographicSize = 2.5f;

#endif
        // WebGL이면서 모바일일때
        if (Application.isMobilePlatform)
            virtualCam.Lens.OrthographicSize = 3.7f;

        //WebGL이면서 컴퓨터일때
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