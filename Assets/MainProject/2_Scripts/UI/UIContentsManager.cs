using UnityEngine;
using UnityEngine.UI;

public abstract class UIContentsManager : MonoBehaviour
{
    public int currentDataPage = 1;
    public int totalDataPage = 0;
    public Button backBtn;
    public Button nextBtn;

    public virtual void NextPage()
    {
        if (currentDataPage < totalDataPage)
        {
            currentDataPage++;
            DisplayPage();
        }
    }

    public virtual void BackPage()
    {
        if (currentDataPage > 1)
        {
            currentDataPage--;
            DisplayPage();
        }
    }

    protected void UpdateNavigationButtons()
    {
        if (backBtn != null)
            backBtn.gameObject.SetActive(currentDataPage > 1);
        if (nextBtn != null)
            nextBtn.gameObject.SetActive(currentDataPage < totalDataPage);
    }

    public abstract void DisplayPage();
}
