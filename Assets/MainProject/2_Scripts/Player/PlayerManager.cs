using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]  private ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;
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


    [Header("Interaction Objects")]
    public List<Collider2D> interactableStack = new List<Collider2D>();
    public Material originalMaterial;
    public Material outlineMaterial;

    public SpriteRenderer confirmOn;
    public Sprite confirmOn_PC;
    public Sprite confirmOn_Mob;

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
        managerConnector.playerManager = this;
    }

    void Update()
    {
        if (session != null)
        {
            session.MoveBasic(this);
            session.AnimControllerBasic(this);
        }
    }
    #endregion

    #region KeyCode Input
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
        if (session != null)
        {
            session.TriggerEnterBasic(this, collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
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
    public void ChangeConfirmOn(bool isConfirmOn)
    {
        if(isConfirmOn)
        {
            confirmOn.sprite = confirmOn_PC;
        }
        else
        {
            confirmOn.sprite = null;
        }
    }
    #endregion

    #region PunRPC
    public void MoveTransform(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    [PunRPC]
    public void RPC_ShowIsMineData(string objCode)
    {
        objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == objCode).isMine = true;
        CurrentObjectManager.Instance.SetCurrentObjData(objCode);

        objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == objCode).isMine = false;
    }

    #endregion
}