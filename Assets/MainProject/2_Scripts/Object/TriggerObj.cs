using UnityEngine;

public class TriggerObj : MonoBehaviour
{
    public string objCode;
    public ObjType objType;

    public enum ObjType
    {
        centerLabel,
        dialogue,
        bubble
    }
}


