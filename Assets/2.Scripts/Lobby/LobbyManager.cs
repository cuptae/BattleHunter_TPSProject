using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class LobbyManager : MonoBehaviour {
	
	// 캐선창 변수 선언
	public GameObject ChrChoice;

	// 인벤 Panel을 가리키는 변수 선언 
	public GameObject UIOption;
	// 로비 화면의 버튼을 참조 할 배열 선언 
	public GameObject[] lobbyBtn;
	// 캐릭터 선택 버튼을 참조 할 배열 선언
	public GameObject[] chrBtn;
	public GameObject TiName;

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


	// 왠만하면 이런식으로 실행 순서를 맞춰 주자. 간혹 명령문의 순서가 잘 못 되어서 프로그램이 꼬이는 경우가 있다.
	//인벤 오픈
	public void ChrOpen(){
		for(int i=0;i<3;i++){
		lobbyBtn[i].SetActive (false);  
		}

		TiName.SetActive(false); 
		ChrChoice.SetActive (true); 

		for(int i=0;i<3;i++){
		chrBtn[i].SetActive (true); 
		}
	}
	//인벤 클로즈 
	public void ChrClose(){
		for(int i=0;i<3;i++){
		chrBtn[i].SetActive (false);
		}

		ChrChoice.SetActive (false); 
		TiName.SetActive(true);

		for(int i=0;i<3;i++){
		lobbyBtn[i].SetActive (true);  
		}
	}
	

	
	//인벤 오픈
	public void OptionOpen(){
		for(int i=0;i<3;i++){
		lobbyBtn[i].SetActive (false);  
		}
		TiName.SetActive(false); 
		UIOption.SetActive (true);  
	}
	//인벤 클로즈 
	public void OptionClose(){
		UIOption.SetActive (false); 
		TiName.SetActive(true); 
		for(int i=0;i<3;i++){
		lobbyBtn[i].SetActive (true);  
		}
        //게임 세이브 
        SaveData();
	}

	// 사운드 중지 (로딩중엔 사운드 없다)
	//사운드 ui 비 활성화 (로딩창엔 실수로도 아무것도 안나오게 하자)
	//scPlayUi 씬 로드 
	public void PlayGame(){  
		GameObject.Find ("SoundManager").GetComponent<AudioSource> ().Stop ();  
		GameObject.Find ("SoundCanvas").GetComponent<Canvas> ().enabled = false;  
		//Application.LoadLevel ("scPlayUi");  
		//SceneManager.LoadScene("scPlayUi");  
	}
}