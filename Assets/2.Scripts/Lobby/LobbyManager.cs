using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour {
	
	// 캐선창 변수 선언
	public GameObject ChrChoice;

	// 인벤 Panel을 가리키는 변수 선언 
	public GameObject UIOption;
	// 로비 화면의 버튼을 참조 할 배열 선언 
	public GameObject[] lobbyBtn;
	public GameObject TiName;


	// 왠만하면 이런식으로 실행 순서를 맞춰 주자. 간혹 명령문의 순서가 잘 못 되어서 프로그램이 꼬이는 경우가 있다.
	//인벤 오픈
	public void ChrOpen(){
		lobbyBtn[0].SetActive (false);  // 1
		lobbyBtn[1].SetActive (false);  // 1
        lobbyBtn[2].SetActive (false);  // 1
		TiName.SetActive(false); //1
		UIOption.SetActive (true);       // 2
	}
	//인벤 클로즈 
	public void ChrClose(){
		UIOption.SetActive (false);     // 1
		TiName.SetActive(true); //2
		lobbyBtn[0].SetActive (true);  // 2
		lobbyBtn[1].SetActive (true);  // 2
        lobbyBtn[2].SetActive (true);  // 2
	}
	

	// 왠만하면 이런식으로 실행 순서를 맞춰 주자. 간혹 명령문의 순서가 잘 못 되어서 프로그램이 꼬이는 경우가 있다.
	//인벤 오픈
	public void OptionOpen(){
		lobbyBtn[0].SetActive (false);  // 1
		lobbyBtn[1].SetActive (false);  // 1
        lobbyBtn[2].SetActive (false);  // 1
		TiName.SetActive(false); //1
		UIOption.SetActive (true);       // 2
	}
	//인벤 클로즈 
	public void OptionClose(){
		UIOption.SetActive (false);     // 1
		TiName.SetActive(true); //2
		lobbyBtn[0].SetActive (true);  // 2
		lobbyBtn[1].SetActive (true);  // 2
        lobbyBtn[2].SetActive (true);  // 2
	}

	// 사운드 중지 (로딩중엔 사운드 없다)
	//사운드 ui 비 활성화 (로딩창엔 실수로도 아무것도 안나오게 하자)
	//scPlayUi 씬 로드 
	public void PlayGame(){  
		GameObject.Find ("SoundManager").GetComponent<AudioSource> ().Stop (); //1
		GameObject.Find ("SoundCanvas").GetComponent<Canvas> ().enabled = false; //2
		//Application.LoadLevel ("scPlayUi");  //3
		//SceneManager.LoadScene("scPlayUi"); //3
	}
}