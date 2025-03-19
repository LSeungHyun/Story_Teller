using UnityEngine;

[CreateAssetMenu(fileName = "ManagerConnector", menuName = "Scriptable Objects/ManagerConnector")]
public class ManagerConnector : ScriptableObject
{
    public PlayerManager playerManager;
    public UINextSetter uiNextSetter;
    public PortalManager portalManager;
    public CurrentObjectManager currentObjectManager;
}
