using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public SoundContainer soundContainer;

    public string soundName;
    private void Start()
    {
        soundContainer.soundManager.Play(soundName);
    }
}
