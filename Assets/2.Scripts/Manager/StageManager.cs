using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    PhotonView pv;
    Transform[] playerSpawnPos;

    Transform[] enemySpawnPos;

    GameObject[] enemys;

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
                Debug.Log("Gunner");
                player = PhotonNetwork.Instantiate("Gunner",playerSpawnPos[curRoom.PlayerCount].position,playerSpawnPos[curRoom.PlayerCount].rotation,0);
                break;
            case Character.WARRIOR:
                Debug.Log("Warrior");
                player = PhotonNetwork.Instantiate("Warrior",playerSpawnPos[curRoom.PlayerCount].position,playerSpawnPos[curRoom.PlayerCount].rotation,0);
                break;
            case Character.HUNTER:
                Debug.Log("Hunter");
                player = PhotonNetwork.Instantiate("Hunter",playerSpawnPos[curRoom.PlayerCount].position,playerSpawnPos[curRoom.PlayerCount].rotation,0);
                break;
        }
        yield return null;
    }
    IEnumerator InitEnemy()
    {
        for(int i = 1; i<enemySpawnPos.Length; i++)
        {
            PhotonNetwork.InstantiateSceneObject("Enemy", enemySpawnPos[i].localPosition, enemySpawnPos[i].localRotation, 0, null);
        }
        yield return null;
    }

    public void OnClickLobby()
    {
        PhotonNetwork.LeaveRoom();
    }
    void OnLeftRoom()
    {
        SceneManager.LoadScene("TestLobby");
    }
}
