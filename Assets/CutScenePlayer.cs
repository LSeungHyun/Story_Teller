using UnityEngine;

public class CutScenePlayer : MonoBehaviour
{
    public PlayerManager player;
    public UIManager UIManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        var session = GameManager.Instance.Session;
        SoundManager.Instance.Play("50meru-constellations");

        if (player != null)
        {
            session.CutSceneEnter(player, false);
        }
        else
        {
            player = UIManager.managerConnector.playerManager;
            session.CutSceneEnter(player, false);
        }
        
        UIManager.CutSceneOnOff(true);
        this.gameObject.SetActive(false);
    }
}
