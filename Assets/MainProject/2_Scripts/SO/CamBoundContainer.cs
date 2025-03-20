using UnityEngine;

[CreateAssetMenu(fileName = "CamBoundContainer", menuName = "Scriptable Objects/CamBoundContainer")]
public class CamBoundContainer : ScriptableObject
{
    public float lensSize;
    public Collider2D boundCol;
}
