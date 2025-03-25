using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettingsData", menuName = "Scriptable Objects/AudioSettingsData")]
public class AudioSettingsData : ScriptableObject
{
    [Range(0f, 1f)]
    public float bgmVolume = 1f;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;
}
