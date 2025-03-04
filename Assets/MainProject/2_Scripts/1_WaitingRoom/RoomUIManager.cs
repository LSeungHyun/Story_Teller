using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomUIManager : MonoBehaviour
{
    [Header ("Canvase BackGround")]
    public GameObject bg_Blur;
    public GameObject bg_Start;
    //public void ActiveBlur(bool isBlur)
    //{
    //    bg_Blur.SetActive(isBlur);
    //    bg_Start.SetActive(!isBlur);
    //}
    public void SceneMove(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
