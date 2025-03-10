using System;
using UnityEngine;

public abstract class UIContentsManager : MonoBehaviour
{
    public static int currentDataPage = 1;
    public static int totalDataPage = 0;

    public abstract void SetData(string currentObjCode);
    public abstract void ClearData();
    public abstract void DisplayPage();
}
