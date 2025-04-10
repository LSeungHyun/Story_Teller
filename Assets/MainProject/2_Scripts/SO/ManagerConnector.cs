using UnityEngine;

[CreateAssetMenu(fileName = "ManagerConnector", menuName = "Scriptable Objects/ManagerConnector")]
public class ManagerConnector : ScriptableObject
{
    public PhotonManager photonManager;
    public PlayerManager playerManager;

    public UINextSetter uiNextSetter;
    public UICenterLabelSetter uICenterLabelSetter;
    public UIManager uiManager;
    public PortalManager portalManager;
    public ScenePortalManager scenePortalManager;
    public HintStateManager hintStateManager;
    public CurrentObjectManager currentObjectManager;
    public KeyInputManager keyInputManager;

    public FloatingJoystick joystick;
    public GameObject webglBtn;

    public TextDataManager textDataManager;
    public bool isMobile = false;
}
