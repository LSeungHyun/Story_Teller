using System.Collections;
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
    public AudioSource bgmSourceA;
    public AudioSource bgmSourceB;
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

    public float crossfadeDuration = 1.0f; // ũ�ν����̵� �ð� (�� ����)
    private AudioSource activeBgmSource;
    private AudioSource inactiveBgmSource;

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

        activeBgmSource = bgmSourceA;
        inactiveBgmSource = bgmSourceB;
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

        // loop ���ο� ���� BGM �Ǵ� SFX�� ���
        if (data.loop)
        {
            // �̹� ���� Ŭ���� Ȱ�� AudioSource���� ������̸� ũ�ν����̵带 �������� �ʽ��ϴ�.
            if (activeBgmSource.clip == data.clip)
                return;

            // ���ο� BGM ����� ���� inactive AudioSource�� �Ҵ�
            inactiveBgmSource.clip = data.clip;
            inactiveBgmSource.loop = true;
            inactiveBgmSource.volume = 0f; // 0���� �����Ͽ� ũ�ν����̵� ����
            inactiveBgmSource.Play();

            // �� BGM AudioSource ���̿� ũ�ν����̵� ����
            StartCoroutine(CrossfadeBGM());
        }
        else
        {
            // ȿ����(SFX)�� �� �� ���
            sfxSource.PlayOneShot(data.clip);
        }
    }

    /// <summary>
    /// ũ�ν����̵� �ڷ�ƾ : activeBgmSource�� ������ ������ ���̰�, inactiveBgmSource�� ������ ���ÿ� �ø��ϴ�.
    /// ��ȯ�� �Ϸ�Ǹ� �� AudioSource�� ������ ��ü�մϴ�.
    /// </summary>
    private IEnumerator CrossfadeBGM()
    {
        float timer = 0f;
        // �⺻ �������� AudioSettingsData�� ������ �� (���� 1.0f ����)
        float initialVolume = AudioSettingsData.bgmVolume;

        // ũ�ν����̵� ����
        while (timer < crossfadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / crossfadeDuration;

            activeBgmSource.volume = Mathf.Lerp(initialVolume, 0f, t);
            inactiveBgmSource.volume = Mathf.Lerp(0f, initialVolume, t);

            yield return null;
        }

        // ��ȯ �Ϸ� ��, ���� AudioSource�� ����
        activeBgmSource.Stop();
        activeBgmSource.volume = initialVolume; // ������ ����� ���� �⺻ ���� ����

        // ���� ��ü: inactive AudioSource�� ���ο� active �ҽ��� �˴ϴ�.
        AudioSource temp = activeBgmSource;
        activeBgmSource = inactiveBgmSource;
        inactiveBgmSource = temp;
    }
    /// <summary>
    /// ���� ������� BGM�� ���ߴ� �޼���(�ɼ�)
    /// </summary>
    public void StopBGM()
    {
        activeBgmSource.Stop();
        inactiveBgmSource.Stop();
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
