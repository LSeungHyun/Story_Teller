using UnityEngine;

public class MultiSession : AbsctractGameSession
{
    #region Player Abstract Methods;
    public override void Move(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            Debug.Log("��Ƽ�����̱�");
            playerManager.inputVec.x = Input.GetAxisRaw("Horizontal");
            playerManager.inputVec.y = Input.GetAxisRaw("Vertical");

            Vector2 nextVec = playerManager.inputVec.normalized * Time.fixedDeltaTime;
            playerManager.rigid.MovePosition(playerManager.rigid.position + nextVec);
        }
    }

    public override void AnimController(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            bool isMoving = playerManager.inputVec.x != 0 || playerManager.inputVec.y != 0;
            if (isMoving || playerManager.joystick != null && (playerManager.joystick.Horizontal != 0 || playerManager.joystick.Vertical != 0))
            {
                playerManager.anim.SetBool("Walking", true);

                if (playerManager.inputVec.x != 0)
                {
                    playerManager.anim.SetFloat("DirX", playerManager.inputVec.x);
                    playerManager.anim.SetFloat("DirY", 0);
                }
                else if (playerManager.inputVec.y != 0)
                {
                    playerManager.anim.SetFloat("DirX", 0);
                    playerManager.anim.SetFloat("DirY", playerManager.inputVec.y);
                }
            }
            else
            {
                playerManager.anim.SetBool("Walking", false);
            }

            playerManager.ResetInputOnKeyUp();
        }
    }


    public override void TriggerEnter(PlayerManager playerManager,Collider2D collision)
    {
        if (playerManager.PV.IsMine)
        {
            Debug.Log("��Ƽ�浹 �Ϸ�!");
            if (!collision.CompareTag("Interaction"))
            {
                return;
            }

            playerManager.interactableStack.Remove(collision);
            playerManager.interactableStack.Add(collision);
            playerManager.UpdateInteractObject();
        }
    }

    public override void TriggerExit(PlayerManager playerManager, Collider2D collision)
    {
        if (playerManager.PV.IsMine)
        {

        }
    }
    #endregion
    public override void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode)
    {
        ClosePopUpBasic(UIPopUpOnOffManager, currentObjCode);

        Debug.Log("��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ.");
    }
    public override void OpenPopUp(UIPopUpOnOffManager uiPopUpOnOffManager, bool isQuest, bool isDial)
    {
        OpenPopUpBasic(uiPopUpOnOffManager, isQuest, isDial);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }
    public override void HandleInteraction(CurrentObjectManager currentObjectManager)
    {
        HandleInteractionBasic(currentObjectManager);
        Debug.Log("���߳� ��Ƽ��");
    }
    public override void OpenCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        OpenCenterLabelBasic(uiCenterLabelOnOffManager);
    }
    public override void CloseCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        CloseCenterLabelBasic(uiCenterLabelOnOffManager);
    }
}