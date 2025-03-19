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

    private void Update()
    {
        if (isTeleporting)
        {
            // �����̵�
            this.transform.position = new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, -10f);
            isTeleporting = false;
        }
    }

    public void SetFollowCam(GameObject playerObj)
    {
        if (playerObj != null)
        {
            // �ʿ��� ���� ����
            virtualCam.Follow = playerObj.transform;
            virtualCam.LookAt = playerObj.transform;
            Debug.Log("ī�޶󻡷ο��");
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