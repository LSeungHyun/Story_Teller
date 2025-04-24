using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ManagerConnector", menuName = "Scriptable Objects/ManagerConnector")]
public class ManagerConnector : ScriptableObject
{
    public PhotonManager photonManager;
    public PlayerManager playerManager;

    public UINextSetter uiNextSetter;
    public UIManager uiManager;
    public UICenterLabelSetter uiCenterLabelSetter;
    public PortalManager portalManager;
    public ScenePortalManager scenePortalManager;
    public HintStateManager hintStateManager;
    public CurrentObjectManager currentObjectManager;
    public KeyInputManager keyInputManager;

    public Transform cutSceneTransform;
    public FloatingJoystick joystick;
    public GameObject webglBtn;
    public Image FadeImage;

    public TextDataManager textDataManager;
    public bool isMobile = false;
}
