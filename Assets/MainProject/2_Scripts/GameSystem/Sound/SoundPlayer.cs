using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public SoundContainer soundContainer;
        
    public string soundName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        soundContainer.soundManager.Play(soundName);
        this.gameObject.SetActive(false);
    }
}
