using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public float playerSpeed = 1.5f;
    [SerializeField] private ObjDataTypeContainer objDataTypeContainer;
    public ManagerConnector managerConnector;
    public PhotonView PV;
    public PhotonTransformView PTV;
    public PhotonAnimatorView PAV;

    public GameObject playerNickname;

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
        playerSpeed = 1.5f;
        isMove = false;
        if (!GameManager.Instance.isType)
        {
            Destroy(PV);
            PV = null;
            Destroy(PTV);
            Destroy(PAV);
        }

        DontDestroyOnLoad(this.gameObject);
        session = GameManager.Instance.Session;
    }

    void Start()
    {
        session.SetPlayerValue(this);
    }
    void Update()
    {
        if (session != null && isMove)
        {
            session.AnimControllerBasic(this);
        }
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


    // 현재 "Back" 태그를 가진 트리거에 들어온 횟수
    private int backTriggerCount = 0;

    #region Collision Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (session != null)
        {
            session.TriggerEnterBasic(this, collision);
        }

        if (collision.CompareTag("CutScene"))
        {
            session.CutSceneEnter(this, collision);
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
                ChangeConfirmOn(false);
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
                ChangeConfirmOn(true);
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

    [PunRPC]
    public void CutSceneUseAble(bool able)
    {
        isCutScene = able;

        CutSceneOnOff();
    }

    [PunRPC]
    public void CutScenePlayerSetValue(bool isCutScene)
    {
        this.transform.position = managerConnector.cutSceneTransform.position;
        anim.SetFloat("DirX", 0);
        anim.SetFloat("DirY", 1);
        anim.SetBool("Walking", isCutScene);
    }

    public void CutSceneOnOff()
    {
        if (isCutScene)
        {
            sprite.enabled = false;
            playerNickname.SetActive(false);
            isMove = false;
            session.OnOffPlayerBtnGroup(managerConnector, false);
        }
        else
        {
            sprite.enabled = true;
            playerNickname.SetActive(true);
            isMove = true;
            session.OnOffPlayerBtnGroup(managerConnector, true);
        }
    }
    #endregion

    #region PunRPC
    [PunRPC]
    public void RPC_ShowIsMineData(string objCode)
    {
        StartCoroutine(WaitAndApply_ShowIsMineData(objCode));
    }

    private IEnumerator WaitAndApply_ShowIsMineData(string objCode)
    {
        yield return new WaitUntil(() =>
            CurrentObjectManager.Instance != null &&
            GameManager.Instance?.Session != null
        );

        var data = objDataTypeContainer.objDataType.FirstOrDefault(r => r.objCode == objCode);
        if (data != null)
        {
            data.isMine = true;
        }

        CurrentObjectManager.Instance.SetCurrentObjData(objCode);

        if (data != null)
        {
            data.isMine = false;
        }
    }

    [PunRPC]
    public void RPC_SetNextObj(string nextObjCode, bool isDelete)
    {
        StartCoroutine(WaitAndApply_SetNextObj(nextObjCode, isDelete));
    }

    private IEnumerator WaitAndApply_SetNextObj(string code, bool isDelete)
    {
        yield return new WaitUntil(() => ObjectDictionary.Instance != null);
        ObjectDictionary.Instance.ToggleObjectActive(code, isDelete);
    }


    [PunRPC]
    public void RPC_AddPlayerToDoneList(string currentObjCode, string playerID)
    {
        Debug.Log("1 : 플레이어 추가 RPC");
        UINextSetter.Instance.SetNextCode(currentObjCode);
        var currentObj = UINextSetter.Instance.currentObjCodeDict.Find(x => x.value == currentObjCode);
        if (!currentObj.playersIsDone.Contains(playerID))
        {
            currentObj.playersIsDone.Add(playerID);
        }
        else
        {
            Debug.Log($"플레이어 {playerID}는 이미 추가되어 있습니다.");
        }

        if (!UINextSetter.Instance.CheckEveryoneIsDone(currentObjCode)) return;

        UINextSetter.Instance.ProcessNextCode(currentObjCode);
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
        this.gameObject.transform.position = new Vector3(-30, 0, 0);
        managerConnector.textDataManager.loadingUI.SetActive(true);
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
        if (!isCutScene)
        {
            this.isMove = isMove;
            anim.SetBool("Walking", isAnim);
        }
    }

    [PunRPC]
    public void ClearPlayerisDone(string currentObjCode)
    {
        var item = managerConnector.uiNextSetter.currentObjCodeDict.Find(x => x.value == currentObjCode);
        if (item != null)
            item.playersIsDone.Clear();
    }
    #endregion
}