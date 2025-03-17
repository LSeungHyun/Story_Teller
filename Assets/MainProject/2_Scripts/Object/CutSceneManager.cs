using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public GameObject cutSceneTrigger;
    public void OnEnable()
    {
        cutSceneTrigger.SetActive(true);
        gameObject.SetActive(false);
    }
}
