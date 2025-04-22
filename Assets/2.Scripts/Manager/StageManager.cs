using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    PhotonView pv;
    Transform[] playerSpawnPos;

    public List<PlayerCtrl> players = new List<PlayerCtrl>();

    public bool isKill50Reached = false;
    public bool isKill100Reached = false;
    public bool isKill150Reached = false;

    public Animator[] vertDoor;
    public Animator[] lDoor;
    public Animator[] rDoor;

    public Image gameoverImage;
    CanvasGroup gameOverCanvasGroup;


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
        while(true)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            if(players.Length == 2)
            {
                foreach(var player in players)
                {
                    PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
                    if(playerCtrl != null && !this.players.Contains(playerCtrl))
                    {
                        this.players.Add(playerCtrl);
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    void Update()
    {
        int pointCount = GameManager.Instance.pointCount;

        if (!isKill50Reached && pointCount >= 50)
        {
            isKill50Reached = true;
            vertDoor[0].SetBool("Open", true);
        }

        if (!isKill100Reached && pointCount >= 100)
        {
            isKill100Reached = true;
            vertDoor[1].SetBool("Open", true);
        }

        if (!isKill150Reached && pointCount >= 150)
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
    
        if (!GameManager.Instance.gameEnd && players.Count > 0 && players.TrueForAll(p => p.isDead))
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
        SkillManager.Instance.GetSkillIcon();

        switch (character)
        {
            case Character.GUNNER:
                player = PhotonNetwork.Instantiate("Gunner", playerSpawnPos[curRoom.PlayerCount].position, playerSpawnPos[curRoom.PlayerCount].rotation, 0);
                break;
            case Character.WARRIOR:
                player = PhotonNetwork.Instantiate("Warrior", playerSpawnPos[curRoom.PlayerCount].position, playerSpawnPos[curRoom.PlayerCount].rotation, 0);
                break;
            case Character.HACKER:
                player = PhotonNetwork.Instantiate("Hacker", playerSpawnPos[curRoom.PlayerCount].position, playerSpawnPos[curRoom.PlayerCount].rotation, 0);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {

        }
        else
        {

        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
}
