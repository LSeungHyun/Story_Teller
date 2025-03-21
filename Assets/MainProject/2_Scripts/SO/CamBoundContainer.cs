using UnityEngine;

[CreateAssetMenu(fileName = "CamBoundContainer", menuName = "Scriptable Objects/CamBoundContainer")]
public class CamBoundContainer : ScriptableObject
{
    public CamDontDes camDontDes;
    public float lensSize;
    public Collider2D boundCol;
}
