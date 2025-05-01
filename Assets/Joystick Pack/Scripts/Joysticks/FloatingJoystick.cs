using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private PointerEventData eventData2;
    protected override void Start()
    {
        base.Start();
        background.anchoredPosition = new Vector2(300, 242);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        eventData2 = eventData;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        eventData2 = eventData;
        background.anchoredPosition = new Vector2(300, 242);
        base.OnPointerUp(eventData);
    }

    public void OnEnable()
    {
        if(eventData2 != null)
        {
            //Debug.Log("조이스틱 켜짐");
            background.anchoredPosition = new Vector2(300, 242);
            base.OnPointerUp(eventData2);
        }
    }
}