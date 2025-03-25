using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public SoundContainer SoundContainer;
    public AudioSettingsData AudioSettingsData;

    public AudioMixer masterMixer;

    // BGM, SFX ����� AudioSource
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    // ������Ʈ���� ����� ��� ���带 ����صδ� ����Ʈ
    public List<SoundData> soundList = new List<SoundData>();

    [System.Serializable]
    public class SoundData
    {
        public string soundName;    // ���� �̸�(Ű��)
        public AudioClip clip;      // ���� AudioClip
        public bool loop;           // BGMó�� �ݺ� ������� ����
    }
    void Awake()
    {
        SoundContainer.soundManager = this;
         
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �ܺο��� ���� �̸����� ��� ��û
    /// </summary>
    public void Play(string soundName)
    {
        // soundList���� �̸��� ��ġ�ϴ� SoundData ã��
        SoundData data = soundList.Find(item => item.soundName == soundName);

        if (data == null)
        {
            Debug.LogWarning($"���� '{soundName}'��(��) ã�� �� �����ϴ�.");
            return;
        }

        // loop ���ο� ���� BGMSource �Ǵ� SFXSource�� ���� ���
        if (data.loop)
        {
            // BGMó�� �ݺ� ���
            bgmSource.clip = data.clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            // ȿ����ó�� 1ȸ ���
            sfxSource.PlayOneShot(data.clip);
        }
    }

    /// <summary>
    /// ���� ������� BGM�� ���ߴ� �޼���(�ɼ�)
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // �����̴��κ��� 0~1 ������ ������ ���޹޾�, dB ������ ��ȯ�Ͽ� Mixer�� ����
    public void SetBGMVolume(float volume)
    {
        AudioSettingsData.bgmVolume = volume;
        // dB ��ȯ: AudioMixer�� �Ϲ������� dB ���� ����
        float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        masterMixer.SetFloat("BGMVolume", dB);
    }

    public void SetSFXVolume(float volume)
    {
        AudioSettingsData.sfxVolume = volume;
        float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        masterMixer.SetFloat("SFXVolume", dB);
    }
}
