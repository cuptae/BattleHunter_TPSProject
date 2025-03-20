using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public enum BGMType
{
    MainMenu,
    MazeStage,
    BossStage
}

public enum SFXType
{
    STEP,
    RUN,
    JUMP,
    SHOOT,
    SKILL
}

public enum UIType
{
    SELECTCHAR,
    CROSSBTN,
    PUSHBTN
}

public enum SFXCategory
{
    PLAYER,
    MOBS,
    BOSS,
    OBJECT,
    UI
}

public class SoundManager : MonoSingleton<SoundManager>
{
    
    private AudioSource bgmSource;

    private Dictionary<SFXCategory, Dictionary<SFXType, AudioClip>> sfxClips = new Dictionary<SFXCategory, Dictionary<SFXType, AudioClip>>();

    private List<AudioSource> sfxSources = new List<AudioSource>();

    private Dictionary<BGMType, AudioClip> bgmClips = new Dictionary<BGMType, AudioClip>();


    private float bgmVolume = 1.0f;
    private float sfxVolume = 1.0f;
    private bool isMuted = false;

    protected override void Awake()
    {
        base.Awake();
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        LoadBGM();

        for (int i = 0; i < 10; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(sfxSource);
        }
    }

    void LoadBGM()
    {
        bgmClips[BGMType.MainMenu] = Resources.Load<AudioClip>("Sounds/BGM/MainMenu");
        bgmClips[BGMType.MazeStage] = Resources.Load<AudioClip>("Sounds/BGM/MazeStage");
        bgmClips[BGMType.BossStage] = Resources.Load<AudioClip>("Sounds/BGM/BossStage");
    }
    

    // 🎵배경음악 볼륨 조정
    public float BGMVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = Mathf.Clamp01(value);  // 0~1 범위 제한
            bgmSource.volume = isMuted ? 0 : bgmVolume;
        }
    }

    public BGMType? CurrentBGM { get; private set; } = null;
    
    public void PlayBGM(BGMType type)
    {
    if (bgmClips.TryGetValue(type, out AudioClip clip))
    {
        if (CurrentBGM == type) return; // 같은 음악이면 재생하지 않음

        bgmSource.clip = clip;
        bgmSource.volume = isMuted ? 0 : bgmVolume;
        bgmSource.Play();
        CurrentBGM = type;
    }
    }

    // 🔊효과음 볼륨 조정
    public float SFXVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = Mathf.Clamp01(value);
            foreach (var source in sfxSources)
            {
                source.volume = isMuted ? 0 : sfxVolume;
            }
        }
    }

    // 🔇전체 사운드 뮤트
    public bool IsMuted
    {
        get { return isMuted; }
        set
        {
            isMuted = value;
            bgmSource.volume = isMuted ? 0 : bgmVolume;
            foreach (var source in sfxSources)
            {
                source.volume = isMuted ? 0 : sfxVolume;
            }
        }
    }

    public void PlaySFXDynamic(SFXCategory category, SFXType type, Vector3 position)
{
    if (sfxClips.TryGetValue(category, out var typeDict) && typeDict.TryGetValue(type, out AudioClip clip))
    {
        GameObject sfxObject = new GameObject($"SFX_{category}_{type}");
        AudioSource sfxSource = sfxObject.AddComponent<AudioSource>();
        sfxSource.spatialBlend = 1.0f;  // 3D 사운드 적용
        sfxSource.transform.position = position;
        sfxSource.clip = clip;
        sfxSource.Play();

        Destroy(sfxObject, clip.length + 0.1f);
    }
}
}

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