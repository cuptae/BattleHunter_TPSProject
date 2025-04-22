using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    PhotonView pv;
    Transform[] playerSpawnPos;

    public List<PlayerCtrl> players = new List<PlayerCtrl>();


    void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerSpawnPos = GameObject.FindWithTag("PlayerSpawnPoint").GetComponentsInChildren<Transform>();
        StartCoroutine(CreatePlayer(GameManager.Instance.curCharacter));
        PhotonNetwork.isMessageQueueRunning = true;
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
        foreach(PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
    
        }
        // 자신의 플레이어인지 확인
        PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
        if (player.GetComponent<PhotonView>().isMine)
        {
            SkillManager.Instance.player = playerCtrl; // 내 플레이어 설정
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
    
}
