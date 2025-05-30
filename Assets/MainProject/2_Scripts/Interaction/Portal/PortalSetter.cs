using System.Collections.Generic;
using UnityEngine;

public class PortalSetter : MonoBehaviour
{
    public PortalManager portalManager;
    public CutSceneManager cutsceneManager;
    public ScenePortalManager ScenePortalManager;

    public GameObject targetObj;
    public string worldName;
    [SerializeField] private GameObject Enter_None;
    [SerializeField] private GameObject Enter_All;
    [SerializeField] private GameObject Enter_Wait;

    public bool isCutScene = false;
    public bool isScenePortal = false;

    [SerializeField] public PortalStatus status;
    [System.Serializable]
    public class PortalStatus
    {
        public List<int> playersInside = new List<int>();
    }

    void Start()
    {
        SetPortalObjects(true, false, false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var session = GameManager.Instance.Session;
        session.OnEnterPortal(this, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var session = GameManager.Instance.Session;
        session.OnExitPortal(this, collision);
    }

    public void SetPortalObjects(bool first, bool second, bool third)
    {
        Enter_None.SetActive(first);
        Enter_All.SetActive(second);
        Enter_Wait.SetActive(third);
    }
}
