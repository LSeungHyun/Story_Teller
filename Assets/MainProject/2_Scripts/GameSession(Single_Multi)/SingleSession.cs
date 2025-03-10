using UnityEngine;

public class SingleSession : AbsctractGameSession
{
    #region Player Abstract Methods;
    public override void Move(PlayerManager playerManager)
    {
        Debug.Log("�̱ۿ����̱�");
        playerManager.inputVec.x = Input.GetAxisRaw("Horizontal");
        playerManager.inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = playerManager.inputVec.normalized * Time.fixedDeltaTime;
        playerManager.rigid.MovePosition(playerManager.rigid.position + nextVec);
    }

    public override void AnimController(PlayerManager playerManager)
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


    #endregion
    public override void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode)
    {
        ClosePopUpBasic(UIPopUpOnOffManager, currentObjCode);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }

    public override void OpenPopUp(UIPopUpOnOffManager uiPopUpOnOffManager, bool isQuest, bool isDial)
    {
        OpenPopUpBasic(uiPopUpOnOffManager, isQuest, isDial);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }

    public override void HandleInteraction(CurrentObjectManager currentObjectManager)
    {
        HandleInteractionBasic(currentObjectManager);
        Debug.Log("���߳� �̱��̾�");
    }
    public override void OpenCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        OpenCenterLabelBasic(uiCenterLabelOnOffManager);
    }
    public override void CloseCenterLabel(UICenterLabelOnOffManager uiCenterLabelOnOffManager)
    {
        CloseCenterLabelBasic(uiCenterLabelOnOffManager);
    }

    public override void TriggerEnter(PlayerManager playerManager, Collider2D collision)
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerExit(PlayerManager playerManager, Collider2D collision)
    {
        throw new System.NotImplementedException();
    }
}