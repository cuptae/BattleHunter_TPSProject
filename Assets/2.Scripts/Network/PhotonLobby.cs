using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class PhotonLobby : MonoBehaviour
{
    public string version = "Ver 0.10";
    public PhotonLogLevel logLevel = PhotonLogLevel.Informational;
    public Text userId;
    public GameObject roomItem;
    public GameObject scrollContents;
    public InputField roomName;

    public InputField userIdInputField;
    
    void Awake()
    {
        if(!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(version);
            Debug.Log("connected!");
            PhotonNetwork.playerName = userIdInputField.text;
        }
    }

    void OnJoinedLobby()
    {
        Debug.Log("Join Lobby!!!");
        userId.text = GetUserID();
    }

    void OnReceivedRoomListUpdate()
    {
        Debug.Log("receiveroomlist");
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("RoomItem"))
        {
            Destroy(obj);
        }

        //수신받은 룸 목록의 정보로 RoomItem 프리팹 객체를 생성
        //GetRoomList 함수는 RoomInfo 클래스 타입의 배열을 반환
        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            Debug.Log(_room.Name);
            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContents.transform, false);

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.Name;
            roomData.connectPlayer = _room.PlayerCount;
            roomData.maxPlayers = _room.MaxPlayers;

            roomData.DisplayRoomData();

            roomData.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); Debug.Log("Room Click " + roomData.roomName); });
        }
    }

    void OnJoinedRoom()
    {
        StartCoroutine(LoadStage());
    }
    
    string GetUserID()
    {
        string userId = PlayerPrefs.GetString("USER_ID");
        if(userId.IsNullOrEmpty())
        {
            userId = userIdInputField.text;
            GameManager.Instance.userId = userId;
        }
        return userId;
    }
    void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.player.NickName =userId.text;

        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickRoomMake()
    {
        string _roomName = roomName.text;

        //룸 이름이 없거나 Null일 경우 룸 이름 지정
        if (string.IsNullOrEmpty(roomName.text))
        {
            // 자릿수 맞춰서 반환
            _roomName = "ROOM_" + Random.Range(0, 999).ToString("000");
        }

        //로컬 플레이어의 이름을 설정
        PhotonNetwork.player.NickName = userId.text;

        //플레이어의 이름을 로컬 저장
        //PlayerPrefs.SetString("USER_ID", userId.text);

        //생성할 룸의 조건 설정 1
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 3;

        //생성할 룸의 조건 설정 2 (객체 생성과 동시에 멤버변수 초기화)
        //RoomOptions roomOptions = new RoomOptions() { IsOpen=true, IsVisible=true, MaxPlayers=50 };

        //지정한 조건에 맞는 룸 생성 함수 
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    IEnumerator LoadStage()
    {
        PhotonNetwork.isMessageQueueRunning = false;

        AsyncOperation ao = SceneManager.LoadSceneAsync("Ingame");

        yield return ao;
    }


    // void OnGUI()
    // {

    //     //화면 좌측 상단에 접속 과정에 대한 로그를 출력(포톤 클라우드 접속 상태 메시지 출력)
    //     // PhotonNetwork.ConnectUsingSettings 함수 호출시 속성 PhotonNetwork.connectionStateDetailed는
    //     //포톤 클라우드 서버에 접속하는 단계별 메시지를 반환함.
    //     //Joined Lobby 메시지시 포톤 클라우드 서버로 접속해 로비에 안전하게 입장했다는 뜻
    //     GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

    //     //만약 포톤네트워크에 연결 되었다면...
    //     if (PhotonNetwork.connected)
    //     {
    //         GUI.Label(new Rect(0, 50, 200, 100), "Connected");

    //         //룸 리스트를 배열로 받아온다.
    //         RoomInfo[] roomList = PhotonNetwork.GetRoomList();

    //         if (roomList.Length > 0)
    //         {
    //             foreach (RoomInfo info in roomList)
    //             {
    //                 GUI.Label(new Rect(0, 80, 400, 100), "Room: " + info.Name
    //                     + " PlayerCount/MaxPlayer :" + info.PlayerCount + "/" + info.MaxPlayers //현재 플레이어/최대 플레이어
    //                     + " CustomProperties Count " + info.CustomProperties.Count // 설정한 CustomProperties 수 
    //                     + " Map ???: " + info.CustomProperties.ContainsKey("Map") //키로 설정한 Map이 있나
    //                     + " Map Count " + info.CustomProperties["Map"] // 설정한 키 값 
    //                     + " GameType ??? " + info.CustomProperties.ContainsKey("GameType") //키로 설정한 GameType이 있나
    //                     + " GameType " + info.CustomProperties["GameType"]);// 설정한 키 값 
    //             }
    //         }
    //         else
    //         {
    //             GUI.Label(new Rect(0, 80, 400, 100), "No Room List");
    //         }
    //     }

        

    //     //PhotonServerSettings 값 가져오기
    //     {
    //         GUI.Label(new Rect(0, 170, 400, 100), "AppID  :  " +
    //             PhotonNetwork.PhotonServerSettings.AppID);
    //         GUI.Label(new Rect(0, 200, 200, 100), "HostType  :  " +
    //             PhotonNetwork.PhotonServerSettings.HostType);
    //         GUI.Label(new Rect(0, 230, 200, 100), "ServerAddress  :  " +
    //             PhotonNetwork.PhotonServerSettings.ServerAddress);
    //         GUI.Label(new Rect(0, 260, 200, 100), "ServerPort  :  " +
    //             PhotonNetwork.PhotonServerSettings.ServerPort);
    //         //PhotonNetwork.PhotonServerSettings.UseCloud(); 

    //         //핑 테스트
    //         int pingTime = PhotonNetwork.GetPing();
    //         GUI.Label(new Rect(0, 310, 200, 100), "Ping: " + pingTime.ToString());
    //     }
    // }

}
