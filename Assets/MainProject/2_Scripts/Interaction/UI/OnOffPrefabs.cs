using UnityEngine;

public class OnOffPrefabs : MonoBehaviour
{
    public UIManager uIManager;

    void OnTriggerEnter2D()
    {
        uIManager.OpenPopUp("Help_PopUp");
    }
}