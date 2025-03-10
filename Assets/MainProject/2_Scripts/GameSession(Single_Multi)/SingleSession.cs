using UnityEngine;

public class SingleSession : AbsctractGameSession
{
    #region Player Abstract Methods;
    public override void Move(PlayerManager playerManager)
    {
        Debug.Log("싱글움직이기");
        playerManager.inputVec.x = Input.GetAxisRaw("Horizontal");
        playerManager.inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = playerManager.inputVec.normalized * Time.fixedDeltaTime;
        playerManager.rigid.MovePosition(playerManager.rigid.position + nextVec);
    }

    #endregion
    public override void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager, string currentObjCode)
    {
        ClosePopUpBasic(UIPopUpOnOffManager, currentObjCode);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }

    public override void OpenPopUp(UIPopUpOnOffManager UIPopUpOnOffManager, bool isQuest)
    {
        OpenPopUpBasic(UIPopUpOnOffManager, isQuest);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }

    public override void HandleInteraction(KeyInputManager keyInputManager)
    {
        HandleInteractionBasic(keyInputManager);
        Debug.Log("나야나 싱글이야");
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