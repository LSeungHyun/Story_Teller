using UnityEngine;

public class SetBound : MonoBehaviour
{
    public CamBoundContainer camBoundContainer;
    public Collider2D camBound;

    public float curLensSize;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        camBoundContainer.lensSize = curLensSize;
        camBoundContainer.boundCol = camBound;
        Debug.Log("카메라바운드 ㄱㄱ");
    }
}