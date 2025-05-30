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

    // BGM, SFX 재생용 AudioSource
    public AudioSource bgmSourceA;
    public AudioSource bgmSourceB;
    public AudioSource sfxSource;

    // 프로젝트에서 사용할 모든 사운드를 등록해두는 리스트
    public List<SoundData> soundList = new List<SoundData>();

    [System.Serializable]
    public class SoundData
    {
        public string soundName;    // 사운드 이름(키값)
        public AudioClip clip;      // 실제 AudioClip
        public bool loop;           // BGM처럼 반복 재생할지 여부
    }

    public float crossfadeDuration = 1.0f; // 크로스페이드 시간 (초 단위)
    private AudioSource activeBgmSource;
    private AudioSource inactiveBgmSource;

    private Coroutine currentCrossfade;
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
    /// 외부에서 사운드 이름으로 재생 요청
    /// </summary>


    public void Play(string soundName)
    {
        // soundList에서 이름이 일치하는 SoundData 찾기
        SoundData data = soundList.Find(item => item.soundName == soundName);

        if (data == null)
        {
            Debug.LogWarning($"사운드 '{soundName}'을(를) 찾을 수 없습니다.");
            return;
        }

        // loop 여부에 따라 BGM 또는 SFX 재생
        if (data.loop)
        {
            // (원래 같은 클립 재생 중이면 중단하는 조건을 넣을 수도 있음)
            // if (activeBgmSource.clip == data.clip)
            //    return;

            // 만약 이미 진행 중인 crossfade가 있다면 중단합니다.
            if (currentCrossfade != null)
            {
                StopCoroutine(currentCrossfade);
                currentCrossfade = null;

                // 이전 crossfade로 인해 변경된 activeBgmSource 볼륨 복원
                activeBgmSource.volume = AudioSettingsData.bgmVolume;
            }

            // 새로운 BGM 재생을 위해 inactive AudioSource에 할당
            inactiveBgmSource.clip = data.clip;
            inactiveBgmSource.loop = true;
            inactiveBgmSource.volume = 0f; // 0부터 시작하여 크로스페이드 진행
            inactiveBgmSource.Play();

            // 두 BGM AudioSource 사이에 크로스페이드 진행
            currentCrossfade = StartCoroutine(CrossfadeBGM());
        }
        else
        {
            // 효과음(SFX)은 한 번 재생
            sfxSource.PlayOneShot(data.clip);
        }
    }

    /// <summary>
    /// 크로스페이드 코루틴: activeBgmSource의 볼륨을 서서히 줄이고,
    /// inactiveBgmSource의 볼륨을 동시에 올립니다. 전환이 완료되면 두 AudioSource의 역할을 교체합니다.
    /// </summary>
    private IEnumerator CrossfadeBGM()
    {
        float timer = 0f;
        float initialVolume = AudioSettingsData.bgmVolume; // 기본 볼륨값

        while (timer < crossfadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / crossfadeDuration;

            activeBgmSource.volume = Mathf.Lerp(initialVolume, 0f, t);
            inactiveBgmSource.volume = Mathf.Lerp(0f, initialVolume, t);

            yield return null;
        }

        // 전환 완료 후 activeBgmSource를 정지하고, 기본 볼륨 복원
        activeBgmSource.Stop();
        activeBgmSource.volume = initialVolume;

        // 역할 교체: inactive AudioSource가 새로운 active 소스가 됩니다.
        AudioSource temp = activeBgmSource;
        activeBgmSource = inactiveBgmSource;
        inactiveBgmSource = temp;

        // 코루틴 참조 초기화
        currentCrossfade = null;
    }
    /// <summary>
    /// 현재 재생중인 BGM을 멈추는 메서드(옵션)
    /// </summary>
    public void StopBGM()
    {
        activeBgmSource.Stop();
        inactiveBgmSource.Stop();
    }

    // 슬라이더로부터 0~1 범위의 볼륨을 전달받아, dB 값으로 변환하여 Mixer에 적용
    public void SetBGMVolume(float volume)
    {
        AudioSettingsData.bgmVolume = volume;
        // dB 변환: AudioMixer는 일반적으로 dB 값을 받음
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
