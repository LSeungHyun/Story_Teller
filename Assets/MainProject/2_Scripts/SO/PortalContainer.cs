using UnityEngine;

[CreateAssetMenu(fileName = "PortalContainer", menuName = "Scriptable Objects/PortalContainer")]
public class PortalContainer : ScriptableObject
{
    public UICenterLabelOnOffManager uICenterLabelOnOffManager;
    public PlayerManager playerManager;
    public PortalMananager portalMananager;
}
