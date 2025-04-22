using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public LobbySoundManager LSoundManager;
    

    //Lobby BTN GO
    public GameObject StartBtn;
    public GameObject SettingBtn;
    public GameObject CreditBtn;
    public GameObject QuitBtn;

    //UI GO
    public GameObject LobbyUI;
    public GameObject StartUI;
    public GameObject SettingUI;
    public GameObject CreditUI;
    public GameObject StartOptionUI;
    public GameObject SelectWorldUI;
    public GameObject SelectServerUI;
    public GameObject NewWorldUI;

    //StartUI BTN GO
    public GameObject Start_BackBtn;
    public GameObject Start_StartBtn;
   

    //SettingUI BTN GO
    public GameObject Setting_BackBtn;
    public GameObject Setting_OKBtn;
    
    
    //CreditUI BTN GO
    public GameObject Credit_BackBtn;

    //StartOptionUI BTN GO
    public GameObject StartOption_StartGameBtn;
    public GameObject StartOption_JoinGameBtn;
    public GameObject StartOption_BackBtn;

    //StartGameUI BTN GO
    public GameObject StartGameUI_NewBtn;
 
    public GameObject StartGameUI_StartBtn;
    

    //NewWorldUI BTN GO
    public GameObject NewWorldUI_CancleBtn;
    public GameObject NewWorldUI_DoneBtn;

    //NewWorldUI InputField GO
    public InputField NewWorldUI_NewWorldIF;

    //JoinGameUI BTN GO
    public GameObject JoinGameUI_ConnectBtn;

    //NewCharacterUI BTN GO
   

    //public GameObject CreateCharacterBackBtn;

    //ServerCreate GameObject;
    public InputField serverName;
    public InputField password;
    public GameObject scrollContents;
    public GameObject serverItem;
    public GameObject photonInit;
    public GameObject fadeOutPnl;
    public Animator anim;


    //private GameObject currentWindow;
    //public void OnButtonClick(GameObject window)
    //{
    //    if(currentWindow != null)
    //    {
    //        currentWindow.SetActive(false);
    //    }
    //    OpenUI(window);
    //    currentWindow = window;
    //}

    public void Awake()
    {
        Animator anim = GetComponent<Animator>();
    }
    public void CloseUI(GameObject window)
    {
        window.SetActive(false);
        LobbyUI.SetActive(true);
    }

    public void OpenUI(GameObject window)
    {
        window.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Lobby Button
        StartBtn.GetComponent<Button>().onClick.AddListener(delegate { StartUI.SetActive(true); LobbyUI.SetActive(false); ToggleAnimation1();});
        SettingBtn.GetComponent<Button>().onClick.AddListener(delegate { SettingUI.SetActive(true); LobbyUI.SetActive(false); LSoundManager.LoadData();});
        CreditBtn.GetComponent<Button>().onClick.AddListener(delegate { CreditUI.SetActive(true); LobbyUI.SetActive(false);});
        QuitBtn.GetComponent<Button>().onClick.AddListener(delegate { Application.Quit(); });

        //StartUI Button
        Start_BackBtn.GetComponent<Button>().onClick.AddListener(delegate { LobbyUI.SetActive(true); StartUI.SetActive(false); ToggleAnimation2(); });
        Start_StartBtn.GetComponent<Button>().onClick.AddListener(delegate { StartOptionUI.SetActive(true); StartUI.SetActive(false); });
        

        //SettingUI Button
        Setting_BackBtn.GetComponent<Button>().onClick.AddListener(delegate { LobbyUI.SetActive(true); SettingUI.SetActive(false); });
        Setting_OKBtn.GetComponent<Button>().onClick.AddListener(delegate { LobbyUI.SetActive(true); SettingUI.SetActive(false); LSoundManager.SaveData(); });

        //CreditUI Button
        Credit_BackBtn.GetComponent<Button>().onClick.AddListener(delegate { LobbyUI.SetActive(true); CreditUI.SetActive(false); });

        //StartOptionUI Button
        StartOption_StartGameBtn.GetComponent<Button>().onClick.AddListener(delegate { SelectWorldUI.SetActive(true); SelectServerUI.SetActive(false); });
        StartOption_JoinGameBtn.GetComponent<Button>().onClick.AddListener(delegate { SelectServerUI.SetActive(true); SelectWorldUI.SetActive(false); });
        StartOption_BackBtn.GetComponent<Button>().onClick.AddListener(delegate { StartUI.SetActive(true); StartOptionUI.SetActive(false); });
        StartGameUI_NewBtn.GetComponent<Button>().onClick.AddListener(delegate { NewWorldUI.SetActive(true); });
        StartGameUI_StartBtn.GetComponent<Button>().onClick.AddListener(delegate { fadeOutPnl.SetActive(true); });
        NewWorldUI_CancleBtn.GetComponent<Button>().onClick.AddListener(delegate { NewWorldUI.SetActive(false); });
        NewWorldUI_DoneBtn.GetComponent<Button>().onClick.AddListener(delegate { NewWorldUI.SetActive(false); });

        //NewCharacterUI Button
       
    }

    void ToggleAnimation1()
    {
       
        anim.SetTrigger("PressCharac");
    }

    void ToggleAnimation2()
    {
        
        anim.SetTrigger("returnLobby");
    }

    void OnReceiveServerListUpdate()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SERVER_ITEM"))
        {
            Destroy(obj);
        }

        int rowCount = 0;

        scrollContents.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        foreach (RoomInfo _server in PhotonNetwork.GetRoomList())
        {
            Debug.Log(_server.Name);
            //RoomItem 프리팹을 동적으로 생성 하자
            GameObject server = (GameObject)Instantiate(serverItem);
            //생성한 RoomItem 프리팹의 Parent를 지정
            server.transform.SetParent(scrollContents.transform, false);

            /*
             * room.transform.parent = scrollContents.transform;
             * 
             * (자식 게임오브젝트).transform.parent = (부모 게임오브젝트).transform;
             * 이 방법보다는 UI 항목을 차일드화할 때는 스케일과 관련된 문제가 발생할
             * 수 있기 때문에 앞에서 사용한 방법보다 SetParent 메서드를 사용하는 것이 
             * 편리함. worldPositionStays 인자를 false로 설정하면 로컬 기준의 정보를 유지한 채
             * 차일드화 된다. (그냥 전 경고 메시지가 안떠서 이걸로 하는게 좋은거 같아요)
             * 
             */

            //생성한 RoomItem에 룸 정보를 표시하기 위한 텍스트 정보 전달
            ServerData serverData = server.GetComponent<ServerData>();
            PhotonLobby photonInit = GetComponent<PhotonLobby>();
            serverData.serverName = _server.Name;


            //텍스트 정보를 표시 
            serverData.DisplayServerData();

            //RoomItem의  Button 컴포넌트에 클릭 이벤트를 동적으로 연결
            serverData.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate 
            { photonInit.roomName = serverName; photonInit.UserPass = password; });
            /*
             * delegate (인자) { 실행코드 };  => 인자는 생략 가능하다
             * delegate (room.name) { OnClickRoomItem( room.name ); Debug.Log("Room Click " + room.name); };
             * delegate { OnClickRoomItem( roomData.roomName ); };
             */

            //Grid Layout Group 컴포넌트의 Constraint Count 값을 증가시키자
            scrollContents.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            //스크롤 영역의 높이를 증가시키자
            scrollContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 20);
        }
    }
}
