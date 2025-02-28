using System.Collections.Generic;
using UnityEngine;

public class MultiSession : AbsctractGameSession
{
    public override void ClosePopUp(UIPopUpOnOffManager UIPopUpOnOffManager)
    {
        ClosePopUpBasic(UIPopUpOnOffManager);

        Debug.Log("��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ��Ƽ.");
    }
    public override void OpenPopUp(UIPopUpOnOffManager uiPopUpOnOffManager, bool isQuest)
    {
        OpenPopUpBasic(uiPopUpOnOffManager, isQuest);

        Debug.Log("SingleSession: PopUp closed in single mode.");
    }
    public override void HandleInteraction(KeyInputManager keyInputManager)
    {
        HandleInteractionBasic(keyInputManager);
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