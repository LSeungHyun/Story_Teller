using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PortalManager : MonoBehaviour
{
    public ManagerConnector managerConnector;

    public Transform targetObj;


    private void Awake()
    {
        managerConnector.portalManager = this;
    }
    public void OnEnable()
    {
        Vector3 spawnAt = targetObj.position;
        managerConnector.playerManager.MoveTransform(spawnAt);
        gameObject.SetActive(false);
    }
}
