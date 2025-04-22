using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SFXCategory
{
    PLAYER,
    MOBS,
    BOSS,
    OBJECT
}

public enum BGMType
{
    MainMenu,
    MazeStage,
    BossStage
}

public enum PLAYER
{
    STEP,
    RUN,
    JUMP,
    ATTACK,
    SKILL,
    DIE
}

public enum BOSS
{
    STEP,
    RUN,
    JUMP,
    ATTACK,
    SKILL,
    DIE
}

public enum UIType
{
    SELECTCHAR,
    CROSSBTN,
    PUSHBTN
}



public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource bgmSource;
    private List<AudioSource> sfxSources = new List<AudioSource>(); 
    private List<AudioSource> uiSources = new List<AudioSource>(); 
    private Dictionary<SFXCategory, Dictionary<PLAYER, AudioClip>> 
    sfxClips = new Dictionary<SFXCategory, Dictionary<PLAYER, AudioClip>>();
    private Dictionary<UIType, AudioClip> uiClips = new Dictionary<UIType, AudioClip>();
    private Dictionary<BGMType, AudioClip> bgmClips = new Dictionary<BGMType, AudioClip>();

    private float bgmVolume = 1.0f;
    private float sfxVolume = 1.0f;
    private float uiVolume = 1.0f;
    // 사운드 뮤트 유무 
    private bool isBGMMuted = false;
    private bool isSFXMuted = false;
    private bool isUIMuted = false;

    public BGMType? CurrentBGM { get; private set; } = null;

    protected override void Awake()
    {
        base.Awake();
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        LoadBGM();
        LoadSFX();
    }

    void LoadBGM()
    {
        bgmClips[BGMType.MainMenu] = Resources.Load<AudioClip>("Sounds/BGM/MainMenu");
        bgmClips[BGMType.MazeStage] = Resources.Load<AudioClip>("Sounds/BGM/MazeStage");
        bgmClips[BGMType.BossStage] = Resources.Load<AudioClip>("Sounds/BGM/BossStage");
    }

    void LoadSFX()
    {
    // 플레이어 효과음 등록
    sfxClips[SFXCategory.PLAYER] = new Dictionary<PLAYER, AudioClip>();
    sfxClips[SFXCategory.PLAYER][PLAYER.STEP] = Resources.Load<AudioClip>("Sounds/SFX/Player/Step");
    sfxClips[SFXCategory.PLAYER][PLAYER.RUN] = Resources.Load<AudioClip>("Sounds/SFX/Player/Run");
    sfxClips[SFXCategory.PLAYER][PLAYER.JUMP] = Resources.Load<AudioClip>("Sounds/SFX/Player/Jump");
    sfxClips[SFXCategory.PLAYER][PLAYER.ATTACK] = Resources.Load<AudioClip>("Sounds/SFX/Player/Shoot");
    sfxClips[SFXCategory.PLAYER][PLAYER.SKILL] = Resources.Load<AudioClip>("Sounds/SFX/Player/Skill");
    sfxClips[SFXCategory.PLAYER][PLAYER.DIE] = Resources.Load<AudioClip>("Sounds/SFX/Player/PlayerDie");

    // 몹 효과음 등록
    sfxClips[SFXCategory.MOBS] = new Dictionary<PLAYER, AudioClip>();
    sfxClips[SFXCategory.MOBS][PLAYER.STEP] = Resources.Load<AudioClip>("Sounds/SFX/Mobs/Step");
    sfxClips[SFXCategory.MOBS][PLAYER.JUMP] = Resources.Load<AudioClip>("Sounds/SFX/Mobs/Jump");
    sfxClips[SFXCategory.MOBS][PLAYER.DIE] = Resources.Load<AudioClip>("Sounds/SFX/Mobs/MobDie");

    uiClips[UIType.SELECTCHAR] = Resources.Load<AudioClip>("Sounds/UI/SelectCharacter");
    uiClips[UIType.CROSSBTN] = Resources.Load<AudioClip>("Sounds/UI/CrossButton");
    uiClips[UIType.PUSHBTN] = Resources.Load<AudioClip>("Sounds/UI/PushButton");
    }

    // ✅ 🎵 BGM 재생 (페이드 인/아웃 적용)
    public void PlayBGM(BGMType type, float fadeDuration = 1.0f)
    {
        if (CurrentBGM == type) return; 

        if (bgmClips.TryGetValue(type, out AudioClip clip))
        {
            StartCoroutine(FadeBGM(clip, fadeDuration));
            CurrentBGM = type;
        }
    }

    private IEnumerator FadeBGM(AudioClip newClip, float duration)
    {
        float startVolume = bgmSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        bgmSource.volume = 0;
        bgmSource.clip = newClip;
        bgmSource.Play();

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, bgmVolume, t / duration);
            yield return null;
        }

        bgmSource.volume = bgmVolume;
    }

    public float BGMVolume
    {
        get => bgmVolume;
        set
        {
            bgmVolume = value;
            if (!isBGMMuted) // 음소거 상태가 아닐 때만 적용
            {
                // 🔹 BGM 볼륨 반영
                AudioSource bgmSource = GetComponent<AudioSource>();
                if (bgmSource != null)
                    bgmSource.volume = bgmVolume;
            }
        }
    }

    public float SFXVolume
    {
        get => sfxVolume;
        set => sfxVolume = value;
    }

    public float UIVolume
    {
        get => uiVolume;
        set => uiVolume = value;
    }

    // 🔹 BGM 음소거 기능 추가
    public bool IsBGMMuted
    {
        get => isBGMMuted;
        set
        {
            isBGMMuted = value;
            AudioSource bgmSource = GetComponent<AudioSource>();
            if (bgmSource != null)
                bgmSource.mute = isBGMMuted; // 음소거 적용
        }
    }

    // 🔹 SFX 음소거 기능 추가
    public bool IsSFXMuted
    {
        get => isSFXMuted;
        set => isSFXMuted = value;
    }

    // 🔹 UI 음소거 기능 추가
    public bool IsUIMuted
    {
        get => isUIMuted;
        set => isUIMuted = value;
    }

    // ✅ 🔊 SFX 재생 (같은 효과음 중복 방지)
    public void PlaySFX(SFXCategory category, PLAYER type, Vector3 position)
    {
    Debug.Log($"▶️ PlaySFX 호출됨: {category} - {type}, isSFXMuted: {isSFXMuted}");

    if (isSFXMuted)
    {
        Debug.LogWarning("🔇 SFX가 음소거 상태임! 소리 재생 안됨.");
        return;
    }

    if (sfxClips.TryGetValue(category, out var typeDict))
    {
        if (typeDict.TryGetValue(type, out AudioClip clip))
        {
            AudioSource sfxSource = GetPooledSFXSource();
            sfxSource.transform.position = position;
            sfxSource.spatialBlend = 1.0f;
            sfxSource.clip = clip;
            sfxSource.volume = sfxVolume;
            sfxSource.Play();
            Debug.Log($"🎵 [SFX] {type} 사운드 재생 완료!");
        }
        else
        {
            Debug.LogError($"⚠️ SFX Not Found: {category} - {type}");
        }
    }
    else
    {
        Debug.LogError($"⚠️ SFX Category Not Found: {category}");
    }
    }

    // 🎵 UI 사운드 재생
public void PlayUISound(UIType type)
{
    Debug.Log($"▶️ PlayUISound 호출됨: {type}, isUIMuted: {isUIMuted}");

    if (isUIMuted)
    {
        Debug.LogWarning("🔇 UI 사운드가 음소거 상태임! 재생 안됨.");
        return;
    }

    if (uiClips.TryGetValue(type, out AudioClip clip))
    {
        AudioSource uiSource = GetPooledUISource();
        uiSource.clip = clip;
        uiSource.volume = uiVolume;
        uiSource.Play();

        Debug.Log($"🎵 [UI] {type} 사운드 재생 완료!");
    }
    else
    {
        Debug.LogError($"⚠️ UI 사운드 클립이 없음: {type}");
    }
}


    // ✅ 🎵 객체 풀링을 활용하여 오디오 소스를 재사용
    private AudioSource GetPooledSFXSource()
    {
    foreach (var source in sfxSources)
    {
        if (!source.isPlaying)
        {
            Debug.Log("♻️ 기존 AudioSource 재사용");
            return source;
        }
    }

    AudioSource newSource = gameObject.AddComponent<AudioSource>();
    newSource.playOnAwake = false;
    newSource.spatialBlend = 1.0f; // 3D 사운드 적용
    newSource.volume = sfxVolume;
    sfxSources.Add(newSource);
    
    Debug.Log("🆕 새로운 AudioSource 추가됨");
    return newSource;
    }

    // 🎵 UI 사운드 전용 AudioSource 풀링
private AudioSource GetPooledUISource()
{
    foreach (var source in uiSources)
    {
        if (!source.isPlaying)
        {
            Debug.Log("♻️ 기존 UI AudioSource 재사용");
            return source;
        }
    }

    AudioSource newSource = gameObject.AddComponent<AudioSource>();
    newSource.playOnAwake = false;
    newSource.volume = uiVolume;
    uiSources.Add(newSource);

    Debug.Log("🆕 새로운 UI AudioSource 추가됨");
    return newSource;
}


}