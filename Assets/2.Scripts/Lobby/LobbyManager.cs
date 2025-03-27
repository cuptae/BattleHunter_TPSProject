using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    // UI 요소 연결 변수
    public GameObject uiOption;   // 옵션 UI
    public GameObject titleName;  // 타이틀 UI

    // 슬라이더와 토글 컴포넌트
    public Slider bgmVolumeSlider;   // BGM 볼륨 슬라이더
    public Slider sfxVolumeSlider;   // SFX 볼륨 슬라이더
    public Slider uiVolumeSlider;    // UI 볼륨 슬라이더
    public Toggle BGMMUTE;           // BGM 음소거 토글
    public Toggle SFXMUTE;           // SFX 음소거 토글
    public Toggle UIMUTE;            // UI 음소거 토글

    // Start()에서 설정 불러오기
    void Start()
    {
        // 🔹 설정 불러오기 (슬라이더, 음소거 버튼 값)
        LoadSoundSettings();

        // 🔹 이벤트 리스너 추가 (슬라이더, 토글 값 변경 시 자동 적용)
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        uiVolumeSlider.onValueChanged.AddListener(OnUIVolumeChanged);
        BGMMUTE.onValueChanged.AddListener(OnBGMMuteChanged);
        SFXMUTE.onValueChanged.AddListener(OnSFXMuteChanged);
        UIMUTE.onValueChanged.AddListener(OnUIMuteChanged);

        // 🔹 메인 메뉴 BGM 재생
        SoundManager.Instance.PlayBGM(BGMType.MainMenu);
    }

    // 🔸 BGM 볼륨 변경
    public void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.BGMVolume = value;
        PlayerPrefs.SetFloat("BGMVolume", value);
        PlayerPrefs.Save();
    }

    // 🔸 SFX 볼륨 변경
    public void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.SFXVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    // 🔸 UI 볼륨 변경
    public void OnUIVolumeChanged(float value)
    {
        SoundManager.Instance.UIVolume = value;
        PlayerPrefs.SetFloat("UIVolume", value);
        PlayerPrefs.Save();
    }

    // 🔸 BGM 음소거 설정
    public void OnBGMMuteChanged(bool isMuted)
    {
        SoundManager.Instance.IsBGMMuted = isMuted;
        PlayerPrefs.SetInt("IsBGMMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 🔸 SFX 음소거 설정
    public void OnSFXMuteChanged(bool isMuted)
    {
        SoundManager.Instance.IsSFXMuted = isMuted;
        PlayerPrefs.SetInt("IsSFXMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 🔸 UI 음소거 설정
    public void OnUIMuteChanged(bool isMuted)
    {
        SoundManager.Instance.IsUIMuted = isMuted;
        PlayerPrefs.SetInt("IsUIMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 🔹 저장된 설정 불러오기
    public void LoadSoundSettings()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        float uiVolume = PlayerPrefs.GetFloat("UIVolume", 1.0f);
        bool isBGMMuted = PlayerPrefs.GetInt("IsBGMMuted", 0) == 1;
        bool isSFXMuted = PlayerPrefs.GetInt("IsSFXMuted", 0) == 1;
        bool isUIMuted = PlayerPrefs.GetInt("IsUIMuted", 0) == 1;

        // 🔸 불러온 값을 SoundManager에 적용
        SoundManager.Instance.BGMVolume = bgmVolume;
        SoundManager.Instance.SFXVolume = sfxVolume;
        SoundManager.Instance.UIVolume = uiVolume;
        SoundManager.Instance.IsBGMMuted = isBGMMuted;
        SoundManager.Instance.IsSFXMuted = isSFXMuted;
        SoundManager.Instance.IsUIMuted = isUIMuted;

        // 🔸 불러온 값을 UI에도 적용
        bgmVolumeSlider.value = bgmVolume;
        sfxVolumeSlider.value = sfxVolume;
        uiVolumeSlider.value = uiVolume;
        BGMMUTE.isOn = isBGMMuted;
        SFXMUTE.isOn = isSFXMuted;
        UIMUTE.isOn = isUIMuted;
    }
}
