using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]  private ObjDataTypeContainer objDataTypeContainer;

    public PhotonView PV;

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
        session = GameManager.Instance.Session;
        if (session != null)
            Debug.Log("Session �޾ƿ�");
        else
            Debug.Log("Session ����");
    }

    void Update()
    {
        //if (session != null)
        //{
        //    session.Move(this);
        //    session.AnimController(this);
        //}
        Move();
    }
    #endregion


    #region KeyCode Input

    //TODO : ����� �׽�Ʈ ������ ���Ŀ� ���� �޼���
    public void Move()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * Time.fixedDeltaTime;
       rigid.MovePosition(rigid.position + nextVec);
    }

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

    #region Collision Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Interaction"))
        {
            return;
        }

        interactableStack.Remove(collision);
        interactableStack.Add(collision);
        UpdateInteractObject();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Interaction"))
        {
            return;
        }

        interactableStack.Remove(collision);
        Renderer renderOfCurrentCollision = collision.GetComponent<Renderer>();
        if (renderOfCurrentCollision != null)
        {
            renderOfCurrentCollision.material = originalMaterial;
        }

        UpdateInteractObject();
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

}