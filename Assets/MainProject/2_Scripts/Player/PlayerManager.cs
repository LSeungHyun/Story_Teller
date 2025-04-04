using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    //public static PlayerManager instance;

    public float testSpeed = 3f;
    [SerializeField] private ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;
    public PhotonView PV;
    public PhotonTransformView PTV;
    public PhotonAnimatorView PAV;

    public SpriteRenderer playerSprite;

    [Header("Player Components")]
    public Rigidbody2D rigid;
    public Animator anim;
    public SpriteRenderer sprite;

    [Header("Direct Assignment")]
    public FloatingJoystick joystick;
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

    public bool isMobile = false;
    public bool isMove = true;
    public bool isCutScene = false;

    #region LifeCycle Methods
    void Awake()
    {
        managerConnector.playerManager = this;

        DontDestroyOnLoad(gameObject);

        joystick = managerConnector.joystick;
        webglBtn = managerConnector.webglBtn;
        isMobile = managerConnector.isMobile;
    }
    void Start()
    {
        testSpeed = 3f;
        isMove = false;
        if (!GameManager.Instance.isType)
        {
            Destroy(PV);
            Destroy(PTV);
            Destroy(PAV);
        }

        session = GameManager.Instance.Session;
    }

    void Update()
    {
        if (session != null && isMove)
        {
            session.AnimControllerBasic(this);
        }
        //Move();
    }

    void FixedUpdate()
    {
        if (session != null && isMove)
        {
            if (isMobile)
            {
                session.JoystickMoveBasic(this);
            }
            else
            {
                session.MoveBasic(this);
            }
        }
        //Move();
    }
    #endregion

    public void Move()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec * 3);
    }

    #region KeyCode Input
    /// <summary>
    /// KeyInput �Է� ���� �� �ʱ�ȭ �޼���
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
    /// ���޵� KeyCode �� �ϳ��� ���ȴ��� Ȯ��
    /// </summary>
    /// <param name="keys">Ȯ���� KeyCode �迭</param>
    /// <returns>Ű�� �������� true</returns>
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


    // ���� "Back" �±׸� ���� Ʈ���ſ� ���� Ƚ��
    private int backTriggerCount = 0;

    #region Collision Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (session != null)
        {
            session.TriggerEnterBasic(this, collision);
        }

        //// "Back" �±����� Ȯ��
        if (collision.CompareTag("Back"))
        {
            // ī��Ʈ ����
            backTriggerCount++;

            // ù ��° Back Ʈ���ſ� �� ���� Ȥ�� �̹� ����ִ� ��Ȳ������ ��� "BackPlayer" ����
            sprite.sortingLayerName = "BackPlayer";
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (session != null)
        {
            session.TriggerExitBasic(this, collision);
        }
        if (collision.CompareTag("Back"))
        {
            // ī��Ʈ ����
            backTriggerCount--;

            // Ȥ�ó� ����ġ �ʰ� ������ ���� �ʵ��� 0���� Ŭ����
            if (backTriggerCount < 0)
                backTriggerCount = 0;

            // �� �̻� � Back Ʈ���ſ��� ������� �ʴٸ� Default�� ����
            if (backTriggerCount == 0)
            {
                sprite.sortingLayerName = "Default";
            }
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
        if (isConfirmOn)
        {
            confirmOn.sprite = confirmOn_PC;
        }
        else
        {
            confirmOn.sprite = null;
        }
    }

    public void CutSceneOnOff()
    {
        if (isCutScene)
        {
            playerSprite.enabled = false;
        }
        else
        {
            playerSprite.enabled = true;
        }
    }
    #endregion

    #region PunRPC
    [PunRPC]
    public void RPC_ShowIsMineData(string objCode)
    {
        objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == objCode).isMine = true;
        CurrentObjectManager.Instance.SetCurrentObjData(objCode);

        objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == objCode).isMine = false;
    }

    [PunRPC]
    public void RPC_SetNextObj(string nextObjCode, bool isDelete)
    {
        ObjectDictionary.Instance.ToggleObjectActive(nextObjCode, isDelete);
    }

    [PunRPC]
    public void RPC_AddPlayerToDoneList(string currentObjCode)
    {
        string playerID = PhotonNetwork.LocalPlayer.UserId;
        UINextSetter.Instance.SetNextCode(currentObjCode);
        var currentObj = UINextSetter.Instance.currentObjCodeDict.Find(x => x.value == currentObjCode);
        currentObj.playersIsDone.Add(playerID);
    }

    [PunRPC]
    public void RPC_MoveTransform(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    [PunRPC]
    public void MoveNextScene(string worldName)
    {
        PhotonNetwork.LoadLevel(worldName);
    }

    [PunRPC]
    public void RPC_SetHintState(string currentObjCode, string state)
    {
        managerConnector.hintStateManager.targetRow = managerConnector.hintStateManager.hintContainer.hintDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        if (managerConnector.hintStateManager.targetRow == null)
            return;
        managerConnector.hintStateManager.targetRow.isUsed = state;
    }

    [PunRPC]
    public void ChangePlayerisMove(bool isMove, bool isAnim)
    {
        this.isMove = isMove;
        anim.SetBool("Walking", isAnim);
    }
    #endregion
}