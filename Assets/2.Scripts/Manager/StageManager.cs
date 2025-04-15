using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    PhotonView pv;
    Transform[] playerSpawnPos;

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
        switch(character)
        {
            case Character.GUNNER:
                player = PhotonNetwork.Instantiate("Gunner",playerSpawnPos[curRoom.PlayerCount].position,playerSpawnPos[curRoom.PlayerCount].rotation,0);
                SkillManager.Instance.player = player.GetComponent<PlayerCtrl>();
                player.GetComponent<PlayerCtrl>().activeSkills = SkillManager.Instance.SkillAdd();
                break;
            case Character.WARRIOR:
                player = PhotonNetwork.Instantiate("Warrior",playerSpawnPos[curRoom.PlayerCount].position,playerSpawnPos[curRoom.PlayerCount].rotation,0);
                SkillManager.Instance.player = player.GetComponent<PlayerCtrl>();
                player.GetComponent<PlayerCtrl>().activeSkills = SkillManager.Instance.SkillAdd();
                break;
            case Character.HACKER:
                player = PhotonNetwork.Instantiate("Hacker",playerSpawnPos[curRoom.PlayerCount].position,playerSpawnPos[curRoom.PlayerCount].rotation,0);
                SkillManager.Instance.player = player.GetComponent<PlayerCtrl>();
                break;
            default:
                break;
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

    
}
