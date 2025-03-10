using UnityEngine;

public class MultiSession : AbsctractGameSession
{
    #region Player Abstract Methods;
    public override void Move(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            Debug.Log("멀티움직이기");
            playerManager.inputVec.x = Input.GetAxisRaw("Horizontal");
            playerManager.inputVec.y = Input.GetAxisRaw("Vertical");

            Vector2 nextVec = playerManager.inputVec.normalized * Time.fixedDeltaTime;
            playerManager.rigid.MovePosition(playerManager.rigid.position + nextVec);
        }
    }
    #endregion
    public override void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode)
    {
        ClosePopUpBasic(UIPopUpOnOffManager, currentObjCode);

        Debug.Log("멀티멀티멀티멀티멀티멀티멀티멀티멀티멀티멀티멀티.");
    }
    public override void OpenPopUp(UIPopUpOnOffManager uiPopUpOnOffManager, bool isQuest, bool isDial)
    {
        OpenPopUpBasic(uiPopUpOnOffManager, isQuest, isDial);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }
    public override void HandleInteraction(CurrentObjectManager currentObjectManager)
    {
        HandleInteractionBasic(currentObjectManager);
        Debug.Log("나야나 멀티야");
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