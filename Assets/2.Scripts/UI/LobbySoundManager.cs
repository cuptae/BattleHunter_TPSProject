using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class LobbySoundManager : MonoSingleton<LobbySoundManager>
{
    public AudioClip[] soundFile;
    public float soundVolume = 1.0f;
    public bool isSoundMute = false;

    public Slider sl;
    public Toggle tg;

    public AudioSource _audio;

    protected override void Awake()
    {
        base.Awake();
        _audio = GetComponent<AudioSource>(); // 이 오브젝트는 씬 전환시 사라지지 않음

    }
    
    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        SetSound();
        //PlayBackground(SceneManager.sceneCountInBuildSettings);
        //if (SceneManager.sceneCount == 1)
        //    PlayBackground(0);
        //if (SceneManager.sceneCount == 3)
        //    PlayBackground(3);
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                break;
            case 1:
                PlayBackground(0);
                break;
            case 2:
                break;
            case 3:
                PlayBackground(3);
                break;
        }
        AudioSet();
    }

    // Update is called once per frame


    public void SetSound()
    {
        soundVolume = sl.value;
        isSoundMute = tg.isOn;
        AudioSet();
        SaveData();
    }

    public void AudioSet()
    {
        // audio.volume = soundVolume;
        _audio.volume = soundVolume;
        _audio.mute = isSoundMute;
    }





    public void PlayBackground(int stage)
    {
        _audio.clip = soundFile[stage];
        AudioSet();
        _audio.Play();
    }

    public void PlayEffect(AudioClip sfx)
    {
        if (isSoundMute)
        {
            return;
        }

        GameObject _soundObj = new GameObject("sfx");



        AudioSource _audioSource = _soundObj.AddComponent<AudioSource>();

        _audioSource.clip = sfx;
        _audioSource.volume = soundVolume;
        _audioSource.minDistance = 15.0f;
        _audioSource.maxDistance = 30.0f;
        _audioSource.Play();

        Destroy(_soundObj, sfx.length + 0.2f);
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("SOUNDVOLUME", soundVolume);
        PlayerPrefs.SetInt("ISSOUNDMUTE", System.Convert.ToInt32(isSoundMute));
    }

    public void LoadData()
    {
        sl.value = PlayerPrefs.GetFloat("SOUNDVOLUME");
        tg.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("ISSOUNDMUTE"));

        int isSave = PlayerPrefs.GetInt("ISSAVE");
        if (isSave == 0)
        {
            sl.value = 1.0f;
            tg.isOn = false;

            SaveData();
            PlayerPrefs.SetInt("ISSAVE", 1);
        }
    }
}