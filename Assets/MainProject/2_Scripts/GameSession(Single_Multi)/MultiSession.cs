using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSession : AbsctractGameSession
{
    private Dictionary<PortalMananager, Coroutine> portalCoroutines = new Dictionary<PortalMananager, Coroutine>();

    public override void ShowPortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        // ��Ƽ ��� Ưȭ ���� �߰� ���� (��: ��Ʈ��ũ ����ȭ ����)
        base.ShowPortalCenterLabel(portalMananager, collision);
    }

    public override void ClosePortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        // ��Ƽ ��� Ưȭ ���� �߰� ����
        base.ClosePortalCenterLabel(portalMananager, collision);
    }

    public override void StartPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (!portalCoroutines.ContainsKey(portal))
        {
            Coroutine c = portal.StartCoroutine(PortalCountdownCoroutine(portal));
            portalCoroutines.Add(portal, c);
        }
    }

    public override void StopPortalCountdown(PortalMananager portal, Collider2D collision)
    {
        if (portalCoroutines.TryGetValue(portal, out Coroutine c))
        {
            portal.StopCoroutine(c);
            portalCoroutines.Remove(portal);
        }
    }

    private IEnumerator PortalCountdownCoroutine(PortalMananager portal)
    {
        float timer = 0f;
        // ��Ƽ ��忡���� ī��Ʈ �ð��̳� ������ �޸��� �� ����
        while (timer < portal.countTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        portalCoroutines.Remove(portal);
        portal.StartCoroutine(LoadMapCoroutine(portal));
    }

    private IEnumerator LoadMapCoroutine(PortalMananager portal)
    {
        portal.isAreadyMove = true;
        Debug.Log("�̵� (��Ƽ ���)!");
        yield return new WaitForSeconds(1f);
        // ��Ƽ ��忡���� �߰� ��Ʈ��ũ ����ȭ ���� ���� ���� �� ����
        portal.portalContainer.playerManager.gameObject.transform.position = portal.nextMap.position;
        portal.isAreadyMove = false;
    }
    #region Player Abstract Methods;
    public override void MoveBasic(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            base.MoveBasic(playerManager);
        }
    }

    public override void AnimControllerBasic(PlayerManager playerManager)
    {
        if (playerManager.PV.IsMine)
        {
            base.AnimControllerBasic(playerManager);
        }
    }


    public override void TriggerEnterBasic(PlayerManager playerManager,Collider2D collision)
    {
        if (playerManager.PV.IsMine)
        {
            base.TriggerExitBasic(playerManager, collision);
        }     
    }

    public override void TriggerExitBasic(PlayerManager playerManager, Collider2D collision)
    {
        if (playerManager.PV.IsMine)
        {
            base.TriggerExitBasic(playerManager, collision);
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