using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public SoundContainer SoundContainer;

    // BGM, SFX 재생용 AudioSource
    public AudioSource bgmSource;
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

        // loop 여부에 따라 BGMSource 또는 SFXSource를 통해 재생
        if (data.loop)
        {
            // BGM처럼 반복 재생
            bgmSource.clip = data.clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            // 효과음처럼 1회 재생
            sfxSource.PlayOneShot(data.clip);
        }
    }

    /// <summary>
    /// 현재 재생중인 BGM을 멈추는 메서드(옵션)
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
