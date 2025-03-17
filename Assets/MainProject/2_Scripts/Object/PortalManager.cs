using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PortalManager : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public Vector3 spawnAt;

    private void Awake()
    {
        managerConnector.portalManager = this;
    }
    public void OnEnable()
    {
        var session = GameManager.Instance.Session;
        session.MovePlayers(this);
    }
}
