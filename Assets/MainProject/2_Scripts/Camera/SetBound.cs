using UnityEngine;

public class SetBound : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public CamBoundContainer camBoundContainer;
    public Collider2D camBound;

    public AbsctractGameSession session;
    public float curLensSize;

    private void Start()
    {
        session = GameManager.Instance.Session;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        session.SetBoundLens(this);
    }
}