using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]  private ObjDataTypeContainer objDataTypeContainer;
    public PortalContainer portalContainer;
    public PhotonView PV;
    public PhotonTransformView PTV;
    public PhotonAnimatorView PAV;

    [Header("Player Components")]
    public Rigidbody2D rigid;
    public Animator anim;

    [Header("Direct Assignment")]
    public FixedJoystick joystick;
    public GameObject webglBtn;

    [Header("Input Keys")]
    public readonly KeyCode[] horizontalKeys = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.A, KeyCode.D };
    public readonly KeyCode[] verticalKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.W, KeyCode.S };
    public Vector2 inputVec;


    public List<Collider2D> interactableStack = new List<Collider2D>();
    public Material originalMaterial;
    public Material outlineMaterial;

    public AbsctractGameSession session;
    #region LifeCycle Methods
    void Start()
    {
        if (!GameManager.Instance.isType)
        {
            Destroy(PV);
            Destroy(PTV);
            Destroy(PAV);
        }

        session = GameManager.Instance.Session;
        //if (session != null)
            //Debug.Log("Session 받아옴");
        //else
            //Debug.Log("Session 없음");

        portalContainer.playerManager = this;
    }

    void Update()
    {
        if (session != null)
        {
            session.MoveBasic(this);
            session.AnimControllerBasic(this);
        }
        //Move();
    }
    #endregion


    #region KeyCode Input

    //TODO : 나경님 테스트 끝나고 추후에 지울 메서드
    public void Move()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * Time.fixedDeltaTime;
       rigid.MovePosition(rigid.position + nextVec);
    }

    /// <summary>
    /// KeyInput 입력 끝난 뒤 초기화 메서드
    /// </summary>
    public void ResetInputOnKeyUp()
    {
        if (IsKeyPressed(horizontalKeys))
        {
            inputVec.x = 0;
        }

        if (IsKeyPressed(verticalKeys))
        {
            inputVec.y = 0;
        }
    }
    /// <summary>
    /// 전달된 KeyCode 중 하나라도 눌렸는지 확인
    /// </summary>
    /// <param name="keys">확인할 KeyCode 배열</param>
    /// <returns>키가 눌렸으면 true</returns>
    public bool IsKeyPressed(params KeyCode[] keys)
    {
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!collision.CompareTag("Interaction"))
        //{
        //    return;
        //}

        //interactableStack.Remove(collision);
        //interactableStack.Add(collision);
        //UpdateInteractObject();
        if (session != null)
        {
            session.TriggerEnterBasic(this, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (!collision.CompareTag("Interaction"))
        //{
        //    return;
        //}

        //interactableStack.Remove(collision);
        //Renderer renderOfCurrentCollision = collision.GetComponent<Renderer>();
        //if (renderOfCurrentCollision != null)
        //{
        //    renderOfCurrentCollision.material = originalMaterial;
        //}
        if (session != null)
        {
            session.TriggerExitBasic(this, collision);
        }
    }

    public void UpdateInteractObject()
    {

        if (objDataTypeContainer != null)
        {
            if (interactableStack.Count > 0)
            {
                TriggerObj triggerObj = interactableStack[interactableStack.Count - 1].GetComponent<TriggerObj>();
                Transform objTransform = interactableStack[interactableStack.Count - 1].transform;

                objDataTypeContainer.objCode = triggerObj != null ? triggerObj.objCode : null;
                Debug.Log("현재 objCode: " + objDataTypeContainer.objCode);
                objDataTypeContainer.SetTransform(objTransform); 
            }
            else
            {
                objDataTypeContainer.objCode = null;
            }
        }

        for (int i = 0; i < interactableStack.Count; i++)
        {
            Collider2D indexedCollision = interactableStack[i];
            if (indexedCollision == null)
            {
                continue;
            }

            Renderer rend = indexedCollision.GetComponent<Renderer>();
            if (rend == null) return;

            if (i == interactableStack.Count - 1)
            {
                rend.material = outlineMaterial;
            }
            else
            {
                rend.material = originalMaterial;
            }
        }
    }
    #endregion

    #region PunRPC

    [PunRPC]
    public void RPC_ShowPortalLabel(string labelCode)
    {
        Debug.Log($"[RPC_ShowPortalLabel] labelCode = {labelCode}");
        // 실제 UI 라벨 표시 로직
        CurrentObjectManager.Instance.SetCurrentObjData(labelCode);
    }

    // [PunRPC] : 모든 클라이언트에서 라벨 닫기
    [PunRPC]
    public void RPC_ClosePortalLabel()
    {
        Debug.Log("다 나갔다 싹다 꺼버려");
        // 실제 UI 라벨 제거 로직
        session.CloseCenterLabel(portalContainer.uICenterLabelOnOffManager);
    }

    [PunRPC]
    public void MoveTransform(Vector3 targetPosition)
    {
        // 모든 클라이언트에서 이 GameObject의 위치를 targetPosition으로 변경
        transform.position = targetPosition;
        //Debug.Log("가자잇!!!");
    }

    [PunRPC]
    public void RPC_ShowIsMineData(string objCode)
    {
        Debug.Log($"[RPC_ShowIsMineData] objCode = {objCode}");
        // 실제 UI 라벨 표시 로직
        objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == objCode).isMine = true;
        CurrentObjectManager.Instance.SetCurrentObjData(objCode);
    }
    #endregion
}