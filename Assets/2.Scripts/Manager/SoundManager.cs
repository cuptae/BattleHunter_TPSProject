using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public class SoundManager : MonoSingleton<SoundManager> 
{
    public AudioSource audioSource; 
    public AudioClip[] SoundClips; 
    public bool bgmMute = false; //bgm Mute 설정 변수 
    public float bgmVolume = 1.0f; // BGM 볼륨 추가 
    public bool sfxMute = false; //sfx Mute 설정 변수 
    public float sfxVolume = 1.0f; // SFX 볼륨 추가 

    protected virtual void Awake() 
    {
        base.Awake(); 
        DontDestroyOnLoad(this.gameObject); 
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>(); 
            audioSource.volume = bgmVolume; 
            audioSource.loop = true; // BGM 기본 반복 설정 
        }
    }

    // 배경음악 재생
    public void PlayBGM(int index) 
    {
        if (index < 0 || index >= SoundClips.Length) return; 
        if (audioSource.clip == SoundClips[index] && audioSource.isPlaying) return; // 중복 재생 방지 
        audioSource.clip = SoundClips[index]; 
        audioSource.Play(); 
    }

    public void MuteBGM(){ 
        
    }

    // 배경음악 정지
    public void StopBGM() 
    {
        audioSource.Stop(); 
    }

    // 볼륨 조절
    public void SetBGMVolume(float volume) 
    {
        bgmVolume = volume; 
        audioSource.volume = bgmVolume; 
    }

    public void SetSFXVolume(float volume) 
    { 
        sfxVolume = volume; 
    } 

    // 효과음 재생
    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        if (clip == null) return; 

        GameObject sfxObj = new GameObject("SFX_" + clip.name); 
        AudioSource sfxSource = sfxObj.AddComponent<AudioSource>(); 
        sfxSource.clip = clip; 
        sfxSource.volume = sfxVolume; // SFX 볼륨 적용
        sfxSource.spatialBlend = 1.0f; // 3D 사운드 효과 적용
        sfxSource.Play(); 

        Destroy(sfxObj, clip.length); 
    } 
} 

// public class SoundManager : MonoSingleton<SoundManager>
// {
//     public AudioSource bgmSource;
//     private List<AudioSource> sfxSources = new List<AudioSource>();
//     private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
//     private int maxSFXSources = 10;

//     protected override void Awake()
//     {
//         base.Awake();
//         bgmSource = gameObject.AddComponent<AudioSource>();
//         bgmSource.loop = true;

//         for (int i = 0; i < maxSFXSources; i++)
//         {
//             AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
//             sfxSources.Add(sfxSource);
//         }

//         LoadSFX();
//     }

//     void LoadSFX()
//     {
//         AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/SFX");
//         foreach (var clip in clips)
//         {
//             sfxClips[clip.name] = clip;
//         }
//     }

//     public void PlayBGM(AudioClip clip)
//     {
//         bgmSource.clip = clip;
//         bgmSource.Play();
//     }

//     public void PlaySFX(string clipName)
//     {
//         if (sfxClips.TryGetValue(clipName, out AudioClip clip))
//         {
//             foreach (AudioSource sfxSource in sfxSources)
//             {
//                 if (!sfxSource.isPlaying)
//                 {
//                     sfxSource.clip = clip;
//                     sfxSource.Play();
//                     return;
//                 }
//             }
//         }
//     }

//     public void PlaySFXDynamic(string clipName, Vector3 position)
//     {
//         if (sfxClips.TryGetValue(clipName, out AudioClip clip))
//         {
//             GameObject sfxObject = new GameObject("SFX_" + clipName);
//             AudioSource sfxSource = sfxObject.AddComponent<AudioSource>();

//             sfxSource.clip = clip;
//             sfxSource.Play();
            
//             Destroy(sfxObject, clip.length + 0.1f);
//         }
//     }
// }