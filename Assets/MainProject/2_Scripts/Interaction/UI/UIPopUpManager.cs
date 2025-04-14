using System;
using UnityEngine.UI;

public abstract class UIPopUpManager : UIContentsManager
{
    public SoundContainer soundContainer;
    public Button backBtn;
    public Button nextBtn;
    public event Action<int, int> OnPageChanged;

    protected virtual void Awake()
    {
        OnPageChanged += HandlePageChanged;
    }

    protected virtual void OnDestroy()
    {
        OnPageChanged -= HandlePageChanged;
    }

    void HandlePageChanged(int currentPage, int totalPages)
    {
        UpdateNavigationButtons(currentPage, totalPages);
    }

    public virtual void UpdateNavigationButtons(int currentPage, int totalPages)
    {
        if (backBtn != null)
            backBtn.gameObject.SetActive(currentPage > 1);
        if (nextBtn != null)
            nextBtn.gameObject.SetActive(currentPage < totalPages);
    }

    protected void StartOnPageChanged(int currentPage, int totalPages)
    {
        OnPageChanged?.Invoke(currentPage, totalPages);
    }

    public void NextPage()
    {
        if (currentDataPage < totalDataPage)
        {
            currentDataPage++;
            if(soundContainer != null)
            {
                soundContainer.soundManager.Play("Enter_Sound");
            }
            DisplayPage();
        }
    }

    public void BackPage()
    {
        if (currentDataPage > 1)
        {
            currentDataPage--;
            if (soundContainer != null)
            {
                soundContainer.soundManager.Play("Enter_Sound");
            }
            DisplayPage();
        }
    }
}
