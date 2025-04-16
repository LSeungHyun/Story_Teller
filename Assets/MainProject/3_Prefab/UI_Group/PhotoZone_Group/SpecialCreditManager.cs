using UnityEngine;
using UnityEngine.UI;

public class SpecialCreditManager : MonoBehaviour
{
    public GameObject infoPanel;
    public Text nameText;
    public Text infoText;

    public string[] dataArray;

    public void ShowInfoForIndex(int index, string indexname)
    {
        string name = indexname;
        string data = dataArray[index];

        infoPanel.SetActive(true);
        nameText.text = name+"´ÔÀÇ ¸Þ¼¼Áö";
        infoText.text = data;
    }
    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }
}
