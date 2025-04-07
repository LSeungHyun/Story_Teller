using UnityEngine;

public class CutScenePlayer : MonoBehaviour
{
    public PlayerManager player;
    public UIManager UIManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        playerManager.CutSceneUseAble(false);
        player.transform.position = this.transform.position;
        UIManager.CutSceneOnOff(true);
        this.gameObject.SetActive(false);
    }
}
