using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    PhotonView pv;
    Transform[] playerSpawnPos;

    //public List<PlayerCtrl> players = new List<PlayerCtrl>();

    public bool isKill50Reached = false;
    public bool isKill100Reached = false;
    public bool isKill150Reached = false;

    public Animator[] vertDoor;
    public Animator[] lDoor;
    public Animator[] rDoor;

    public Image gameoverImage;
    CanvasGroup gameOverCanvasGroup;

    public LevelUpManager levelUpManager;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerSpawnPos = GameObject.FindWithTag("PlayerSpawnPoint").GetComponentsInChildren<Transform>();
        StartCoroutine(CreatePlayer(GameManager.Instance.curCharacter));
        PhotonNetwork.isMessageQueueRunning = true;
        gameOverCanvasGroup = gameoverImage.GetComponent<CanvasGroup>();
        gameOverCanvasGroup.alpha = 0f;
        gameoverImage.gameObject.SetActive(false);
    }
    IEnumerator Start()
    {
        // 플레이어 태그가 붙은 오브젝트를 한 번씩 찾아 리스트에 추가
        void RegisterPlayersOnce()
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
            {
                var pc = go.GetComponent<PlayerCtrl>();
                if (pc != null && !GameManager.Instance.players.Contains(pc))
                    GameManager.Instance.players.Add(pc);
            }
        }

        // 1) 처음에도 한 번 등록
        RegisterPlayersOnce();

        // 2) 두 번째 플레이어가 들어올 때까지 대기
        yield return new WaitUntil(() =>
        {
            RegisterPlayersOnce();
            return GameManager.Instance.players.Count == 2;
        });

        // 3) 두 명 모두 등록된 상태
        Debug.Log("두 플레이어 모두 등록됨, 게임 시작 가능!");

        // 여기에 몬스터 스폰이나 게임 시작 로직 호출
        // e.g. Spawner.Instance.StartSpawn();
    }
    void Update()
    {
        int pointCount = GameManager.Instance.pointCount;

        if (!isKill50Reached && pointCount >= 5)
        {
            isKill50Reached = true;
            vertDoor[0].SetBool("Open", true);
        }

        if (!isKill100Reached && pointCount >= 8)
        {
            isKill100Reached = true;
            vertDoor[1].SetBool("Open", true);
        }

        if (!isKill150Reached && pointCount >= 10)
        {
            isKill150Reached = true;
            vertDoor[2].SetBool("Open", true);
        }

        if (isKill50Reached && isKill100Reached && isKill150Reached)
        {
            foreach (var door in lDoor)
            {
                door.SetBool("Open", true);
            }

            foreach (var door in rDoor)
            {
                door.SetBool("Open", true);
            }
        }
    
        if (!GameManager.Instance.gameEnd && GameManager.Instance.players.Count > 0 && GameManager.Instance.players.TrueForAll(p => p.isDead))
        {
            GameManager.Instance.gameEnd = true;
            StartCoroutine(FadeInGameOver());
        }
    }

    IEnumerator FadeInGameOver()
{
        gameoverImage.gameObject.SetActive(true);
        float duration = 2f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / duration);
            gameOverCanvasGroup.alpha = alpha;
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;
    }

    IEnumerator CreatePlayer(Character character)
    {
        Room curRoom = PhotonNetwork.room;
        GameObject player;


        switch (character)
        {
            case Character.GUNNER:
                player = PhotonNetwork.Instantiate("Gunner", playerSpawnPos[curRoom.PlayerCount].position, playerSpawnPos[curRoom.PlayerCount].rotation, 0);
                SkillManager.Instance.GetSkillIcon();
                break;
            case Character.WARRIOR:
                player = PhotonNetwork.Instantiate("Warrior", playerSpawnPos[curRoom.PlayerCount].position, playerSpawnPos[curRoom.PlayerCount].rotation, 0);
                SkillManager.Instance.GetSkillIcon();
                break;
            default:
                yield break;
        }

        // 자신의 플레이어인지 확인
        PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();

        if (player.GetComponent<PhotonView>().isMine)
        {
            SkillManager.Instance.player = playerCtrl;
            playerCtrl.activeSkills = SkillManager.Instance.SkillAdd();
        }

        yield return null;
    }
    public void OnClickLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        GameManager.Instance.players.Clear();
    }
    
}
