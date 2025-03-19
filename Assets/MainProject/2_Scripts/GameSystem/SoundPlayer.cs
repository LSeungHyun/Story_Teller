using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public SoundContainer soundContainer;
    public Collider2D camBound;

        
    public string soundName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        soundContainer.soundManager.Play(soundName);
        CamDontDes.instance.SetBound(camBound);
        this.gameObject.SetActive(false);
    }
}
