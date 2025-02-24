using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//현재 스크립트에서 넓게는 현재 게임오브젝트에서 반드시 필요로하는 컴포넌트를 Attribute로 명시하여 해당 컴포넌트의 자동 생성 및 삭제되는 것을 막는다.
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    //오디오 클립 저장 배열 선언 
    public AudioClip[] soundFile;

    //사운드 Volume 설정 변수
    public float soundVolume = 1.0f;
    //사운드 Mute 설정 변수 
    public bool isSoundMute = false;
    //슬라이더 컴포넌트 연결 변수 
    public Slider sl;
    //토글 컴포넌트 연결 변수 
    public Toggle tg;
    //Sound 오브젝트 연결 변수 
    public GameObject Sound;
    //Sound Ui버튼 오브젝트 연결 변수 
    public GameObject PlaySoundBtn;

    AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        //이 오브젝트는 씬 전환시 사라지지 않음
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //게임 로드 
        LoadData();
        soundVolume = sl.value;
        isSoundMute = tg.isOn;
        PlaySoundBtn.SetActive(true);// 비 활성화 되었던 사운드 Ui 실행 버튼이 활성화 되어져 보일것이다.
        AudioSet();
    }

	//Slider 와 Toggle 컴포넌트에서 이벤트 발생시 호출해줄 함수를 선언 (public 키워드에 의해 외부접근 가능)
	public void SetSound(){
		soundVolume = sl.value;
		isSoundMute = tg.isOn;
        AudioSet();
    }

	//AudioSource 셋팅 (사운드 UI에서 설정 한 값의 적용 )
	void AudioSet(){
        //AudioSource의 볼륨 셋팅 
        audio.volume = soundVolume;
        //AudioSource의 Mute 셋팅 
        audio.mute = isSoundMute;
	}

	//사운드 UI 창 오픈 
	public void SoundUiOpen(){
		// 사운드 UI 활성화 
		Sound.SetActive (true); 
		// 사운드 UI 오픈 버튼 비활성화 
		PlaySoundBtn.SetActive (false);
	}

	//사운드 UI 창 닫음
	public void SoundUiClose(){
		// 사운드 UI 비 활성화 
		Sound.SetActive (false);
		// 사운드 UI 오픈 버튼 활성화 
		PlaySoundBtn.SetActive (true);

        //게임 세이브 
        SaveData();
    }

    //스테이지 시작시 호출되는 함수  
    public void PlayBackground(int stage)
    {
        // AudioSource의 사운드 연결
        audio.clip = soundFile[stage - 1];
        // AudioSource 셋팅 
        AudioSet();
        // 사운드 플레이. Mute 설정시 사운드 안나옴
        audio.Play();
    }

    //사운드 공용함수 정의(어디서든 동적으로 사운드 게임오브젝트 생성)
    public void PlayEffct(Vector3 pos, AudioClip sfx)
    {
        //Mute 옵션 설정시 이 함수를 바로 빠져나가자.
        if (isSoundMute)
        {
            return;
        }

        //게임오브젝트의 동적 생성하자.
        GameObject _soundObj = new GameObject("sfx");
        //사운드 발생 위치 지정하자. 
        _soundObj.transform.position = pos;

        //생성한 게임오브젝트에 AudioSource 컴포넌트를 추가하자.
        AudioSource _audioSource = _soundObj.AddComponent<AudioSource>();
        //AudioSource 속성을 설정 
        //사운드 파일 연결하자.
        _audioSource.clip = sfx;
        //설정되어있는 볼륨을 적용시키자. 즉 soundVolume 으로 게임전체 사운드 볼륨 조절.
        _audioSource.volume = soundVolume;
        //사운드 3d 셋팅에 최소 범위를 설정하자.
        _audioSource.minDistance = 15.0f;
        //사운드 3d 셋팅에 최대 범위를 설정하자.
        _audioSource.maxDistance = 30.0f;

        //사운드를 실행시키자.
        _audioSource.Play();

        //모든 사운드가 플레이 종료되면 동적 생성된 게임오브젝트 삭제하자.
        Destroy(_soundObj, sfx.length + 0.2f);

    }

	//게임 사운드데이타 저장 
	public void SaveData() {

		PlayerPrefs.SetFloat("SOUNDVOLUME",soundVolume);
		//PlayerPrefs 클래스 내부 함수에는 bool형을 저장해주는 함수가 없다.
		//bool형 데이타는 형변환을 해야  PlayerPrefs.SetInt() 함수를 사용가능
		PlayerPrefs.SetInt("ISSOUNDMUTE",System.Convert.ToInt32(isSoundMute));

	}

	//게임 사운드데이타 불러오기 
	//바로 사운드 UI 슬라이드 와 토글에 적용하자.
	public void LoadData() {

		sl.value = PlayerPrefs.GetFloat("SOUNDVOLUME");
		//int 형 데이타는 bool 형으로 형변환.
		tg.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("ISSOUNDMUTE"));

		// 첫 세이브시 설정 -> 이 로직없으면 첫 시작시 사운드 볼륨 0
		int isSave = PlayerPrefs.GetInt ("ISSAVE");
		if (isSave == 0) {
			sl.value = 1.0f;
			tg.isOn = false;
			// 첫 세이브는 soundVolume = 1.0f; isSoundMute = false; 이 디폴트 값으로 저장 된다.
			SaveData();
			PlayerPrefs.SetInt("ISSAVE",1);
		}

	}

}
