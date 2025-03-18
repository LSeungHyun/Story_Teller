using UnityEngine;

[CreateAssetMenu(fileName = "ManagerConnector", menuName = "Scriptable Objects/ManagerConnector")]
public class ManagerConnector : ScriptableObject
{
    public PlayerManager playerManager;
    public UIQuestSetter uiQuestSetter;
    public PortalManager portalManager;
    public CurrentObjectManager currentObjectManager;
}
