using UnityEngine;

[CreateAssetMenu(fileName = "ManagerConnector", menuName = "Scriptable Objects/ManagerConnector")]
public class ManagerConnector : ScriptableObject
{
    public PlayerManager playerManager;
    public UINextSetter uiNextSetter;
    public PortalManager portalManager;
    public HintStateManager hintStateManager;
    public CurrentObjectManager currentObjectManager;

    public FloatingJoystick joystick;
    public GameObject webglBtn;
    public bool isMobile = false;
}
