using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverMessage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    public string name;
    public SpecialCreditManager specialCreditManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        specialCreditManager.ShowInfoForIndex(index, name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        specialCreditManager.HideInfo();
    }
}
