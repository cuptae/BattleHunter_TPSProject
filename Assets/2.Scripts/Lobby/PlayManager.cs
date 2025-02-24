using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour {
	// 유저인터페이스를 연결하기 위한 변수
	public GameObject playrUi;
	//로딩(설명)Panel을 연결 하기위한 배열 
	public GameObject[] exPnl;
	//로딩 progress Text 컴포넌트를 연결하기 위한 변수 
	public Text progressText;

	// Start 보다 먼저 호출 됨 
	// 순서를 지켜 주자 그래야 코딩도 편해지고 성능이 안좋은 플랫폼에서 문제 발생을 방지.
	void Awake () {
		// 로딩(설명)Panel 렌덤으로 선택
		exPnl [Random.Range (0, 2)].SetActive (true);  // 1
		playrUi.SetActive (true);                      // 2
	}

	// Use this for initialization
	// 코루틴 함수 호출 
	void Start () {
		StartCoroutine (this.Loading());
	}

	//유니티엔진에게 역할을 분담시켜 유니티엔진에 렌더링 루프와 별도로 처리되어 성능향상 .
	IEnumerator Loading(){
		// 1초 정도는 화면에 설명을 뛰워주자 바로 게임으로 넘어가면 이상함 
		yield return new WaitForSeconds(1.0f);

		// 씬을 비동기방식으로 추가하자.
		//AsyncOperation async = Application.LoadLevelAdditiveAsync ("scStage1");
		AsyncOperation async = SceneManager.LoadSceneAsync("scStage1", LoadSceneMode.Additive);
		//scStage1 대신 인게임 씬 추가하면 될 듯

		//While 문으로 로딩진행사항을 표시해주자 
		//현재 로딩중일때 
		while (!async.isDone)
		{
			// async.progress 값은 0~1 값이다 따라서 100을 곱해주자 그래야 % 값 얻음. 
			float progress = async.progress * 100.0f;  
			//  Mathf.RoundToInt는 float를 받아서 올림해서 인트형으로 반환 
			int pRounded = Mathf.RoundToInt(progress);
			//Text 컴포넌트에 text 요소를 다음과 같이 셋팅 
			progressText.text = "Loading..." + pRounded.ToString() + "%";
			//  한프레임동안 대기한후 무한루프를 다시 수행한다
			//  IEnumerator문을 바로 탈출하려면 yield break문을 사용하면 된다.
			yield return true;
		}

        progressText.text = "Loading..." + Mathf.RoundToInt(async.progress * 100.0f).ToString() + "%";
        yield return new WaitForSeconds(3.0f);
        // 로딩 완료후 설명서 비활성화 사운드 Ui 활성화 
        exPnl [0].transform.parent.gameObject.SetActive (false);
		GameObject.Find ("OptionCanvas").GetComponent<Canvas> ().enabled = true;

	}

}