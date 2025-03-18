using UnityEngine;
using System.Linq;

public class HintOnOffManager : MonoBehaviour
{
    public GameObject hintPopUp;
    public GameObject hintDetailPopUp;


    public void HintDetailPopUpOn()
    {
        hintDetailPopUp.SetActive(true);
    }
    public void HintDetailPopUpOff()
    {
        hintDetailPopUp.SetActive(false);
    }

    public void HintPopUpOn()
    {
        hintPopUp.SetActive(true);
    }
    public void HintPopUpOff()
    {
        HintDetailPopUpOff();
        hintPopUp.SetActive(false); 
    }
}
