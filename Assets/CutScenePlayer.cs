using UnityEngine;

public class CutScenePlayer : MonoBehaviour
{
    public ManagerConnector managerConnector;
    public PlayerManager player;
    public UIManager UIManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        player = managerConnector.playerManager;

        SoundManager.Instance.Play("50meru-constellations");
        player.CutSceneUseAble(false);
        UIManager.CutSceneOnOff(true);
        this.gameObject.SetActive(false);
    }
}
