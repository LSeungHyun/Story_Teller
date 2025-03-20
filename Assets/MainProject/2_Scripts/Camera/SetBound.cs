using UnityEngine;

public class SetBound : MonoBehaviour
{
    public Collider2D camBound;
    public void OnEnterTrigger2D(Collider2D other)
    {
        CamDontDes.instance.SetBound(camBound);
        this.gameObject.SetActive(false);
    }
}