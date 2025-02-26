using UnityEngine;

public class TriggerObj : MonoBehaviour
{
    public string objCode;
   

    public DataType objType;
    public enum DataType 
    {
        dial,
        bubble,
        quest,
        image
    }
}