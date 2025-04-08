using UnityEngine;
using UnityEngine.UI;

public class AudioDataSetting : MonoBehaviour
{
    public AudioSettingsData audioSettingsData;

    public Slider bgmSlider;
    public Slider sfxSlider;
    void Start()
    {
        bgmSlider.value = audioSettingsData.bgmVolume;
        sfxSlider.value = audioSettingsData.sfxVolume;


        bgmSlider.onValueChanged.AddListener((volume) => {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetBGMVolume(volume);
            }
            else
            {
                Debug.LogWarning("SoundManager Instance가 존재하지 않습니다.");
            }
        });

        sfxSlider.onValueChanged.AddListener((volume) => {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetSFXVolume(volume);
            }
            else
            {
                Debug.LogWarning("SoundManager Instance가 존재하지 않습니다.");
            }
        });
    }
}
