using System.Collections;
using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

public class CamDontDes : MonoBehaviour
{
    public static CamDontDes instance;

    public CinemachineConfiner2D confinerBound;
    public CinemachineCamera virtualCam;
    public Collider2D boundingShape;
    public GameObject thePlayer;

    private bool isTeleporting = false;

    // Start is called before the first frame update
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

    private void Update()
    {
        if (isTeleporting)
        {
            // 순간이동
            this.transform.position = new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, -10f);
            isTeleporting = false;
        }
    }

    public void SetFollowCam(GameObject playerObj)
    {
        if (playerObj != null)
        {
            // 필요한 참조 설정
            virtualCam.Follow = playerObj.transform;
            virtualCam.LookAt = playerObj.transform;
            Debug.Log("카메라빨로우미");
        }
    }
    
    public void SetBound(Collider2D newBound)
    {
        //boundingShape = newBound;
        confinerBound.BoundingShape2D = newBound;
    }

    public void Teleport()
    {
        isTeleporting = true;
    }
}