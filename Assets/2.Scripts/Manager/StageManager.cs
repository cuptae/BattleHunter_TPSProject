using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    PhotonView pv;
    Transform[] playerSpawnPos;

    Transform[] enemySpawnPos;

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
        GameObject player = null;

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
                break;
            case Character.HACKER:
                player = PhotonNetwork.Instantiate("Hacker",playerSpawnPos[curRoom.PlayerCount].position,playerSpawnPos[curRoom.PlayerCount].rotation,0);
                SkillManager.Instance.player = player.GetComponent<PlayerCtrl>();
                break;
        }

        if (player != null)
        {
            FindObjectOfType<scJson>()?.SetPlayer(player.GetComponent<PlayerCtrl>());
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
