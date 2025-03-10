using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]  private ObjDataTypeContainer objDataTypeContainer;

    [Header("Player Components")]
    public Rigidbody2D rigid;
    public Animator anim;

    [Header("Direct Assignment")]
    public FixedJoystick joystick;
    public GameObject webglBtn;

    [Header("Input Keys")]
    public readonly KeyCode[] horizontalKeys = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.A, KeyCode.D };
    public readonly KeyCode[] verticalKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.W, KeyCode.S };

    [HideInInspector] public Vector2 inputVec;
    public List<Collider2D> interactableStack = new List<Collider2D>();
    public Material originalMaterial;
    public Material outlineMaterial;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    #region Animation Methods
    /// <summary>
    /// 플레이어 애니메이션 관리 추상메서드
    /// </summary>
    public void AnimController()
    {
        bool isMoving = inputVec.x != 0 || inputVec.y != 0;
        if (isMoving || joystick != null && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            anim.SetBool("Walking", true);

            if (inputVec.x != 0)
            {
                anim.SetFloat("DirX", inputVec.x);
                anim.SetFloat("DirY", 0);
            }
            else if (inputVec.y != 0)
            {
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", inputVec.y);
            }
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        ResetInputOnKeyUp();
    }

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

    #endregion

    #region KeyCode Input
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

    private void UpdateInteractObject()
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

    public void Move()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
}
