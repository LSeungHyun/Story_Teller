using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSession : AbsctractGameSession
{
    #region Portal & CenterLabel Methods
    // �� ��Ż�� ���� �ڷ�ƾ ������ �����ϱ� ���� ��ųʸ�
    private Dictionary<PortalMananager, Coroutine> portalCoroutines = new Dictionary<PortalMananager, Coroutine>();

    public override void ShowPortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        // �ʿ��� �̱� ��� ���� ���� �߰� ����
        base.ShowPortalCenterLabel(portalMananager, collision);
    }

    public override void ClosePortalCenterLabel(PortalMananager portalMananager, Collider2D collision)
    {
        // �̱� ��� ���� ���� �߰� ����
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
        while (timer < portal.countTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        // ī��Ʈ�ٿ� �Ϸ� �� �ڷ�ƾ ���� ����
        portalCoroutines.Remove(portal);
        // 1�� �Ŀ� �̵� ����
        portal.StartCoroutine(LoadMapCoroutine(portal));
    }

    private IEnumerator LoadMapCoroutine(PortalMananager portal)
    {
        portal.isAreadyMove = true;
        Debug.Log("�̵� (�̱� ���)!");
        yield return new WaitForSeconds(1f);
        // ���� �̵� ó�� (��: �÷��̾� ��ġ ����)
        portal.portalContainer.playerManager.gameObject.transform.position = portal.nextMap.position;
        portal.isAreadyMove = false;
    }
    #endregion

    #region Player Abstract Methods;
    public override void MoveBasic(PlayerManager playerManager)
    {
        //MoveBasic(playerManager);
        base.MoveBasic(playerManager);
    }

    public override void AnimControllerBasic(PlayerManager playerManager)
    {
        base.AnimControllerBasic(playerManager);
    }

    public override void TriggerEnterBasic(PlayerManager playerManager, Collider2D collision)
    {
        base.TriggerExitBasic(playerManager, collision);
    }

    public override void TriggerExitBasic(PlayerManager playerManager, Collider2D collision)
    {
        base.TriggerExitBasic(playerManager, collision);
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
}