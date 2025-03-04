using System;
using UnityEngine;

public abstract class UIContentsManager : MonoBehaviour
{
    public int currentDataPage = 1;
    public int totalDataPage = 0;

    public event Action<int, int> OnPageChanged;

    public abstract void SetData(string currentObjCode);
    public abstract void ClearData();
    protected abstract void DisplayPageContent();

    public void NextPage()
    {
        if (currentDataPage < totalDataPage)
        {
            currentDataPage++;
            DisplayPage();
        }
    }

    public void BackPage()
    {
        if (currentDataPage > 1)
        {
            currentDataPage--;
            DisplayPage();
        }
    }

    public void DisplayPage()
    {
        DisplayPageContent();
        OnPageChanged?.Invoke(currentDataPage, totalDataPage);
    }
}
