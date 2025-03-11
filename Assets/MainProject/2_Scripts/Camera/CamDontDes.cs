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
        //StartCoroutine(FindPlayerCoroutine());
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
        }
        Debug.Log("ī�޶󻡷ο��");
    }
    //private IEnumerator FindPlayerCoroutine()
    //{
    //    while (thePlayer == null)
    //    {
    //        // ��� PlayerManager ��ü�� ã��
    //        PlayerManager[] players = FindObjectsOfType<PlayerManager>();

    //        // ���� �÷��̾ ã�� (PhotonView.IsMine�� true�� �÷��̾�)
    //        foreach (PlayerManager player in players)
    //        {
    //            PhotonView playerPV = player.GetComponent<PhotonView>();
    //            if (playerPV != null && playerPV.IsMine) // �� �ڽ��� �÷��̾����� Ȯ��
    //            {
    //                thePlayer = player.gameObject;
    //                break;
    //            }
    //        }

    //        yield return null; // ���� �����ӱ��� ���
    //    }

    //    // ���⼭ player ������Ʈ�� �����Ͽ� �ʱ�ȭ
    //    var playerComponent = thePlayer.GetComponent<PlayerManager>();
    //    if (playerComponent != null)
    //    {
    //        // �ʿ��� ���� ����
    //        virtualCam.Follow = thePlayer.transform;
    //        virtualCam.LookAt = thePlayer.transform;
    //    }
    //}
    public void SetBound(Collider2D newBound)
    {
        boundingShape = newBound;
        //confinerBound.m_BoundingShape2D = boundingShape;
    }

    public void Teleport()
    {
        isTeleporting = true;
    }
}