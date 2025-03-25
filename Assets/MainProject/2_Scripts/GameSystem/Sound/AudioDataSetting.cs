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

        Debug.Log(bgmSlider.value);
        Debug.Log(sfxSlider.value);
    }
}
