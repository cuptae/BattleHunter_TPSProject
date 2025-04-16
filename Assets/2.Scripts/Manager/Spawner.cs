using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPos;
    public GameObject[] enemyPrefabs;
    public GameObject enemyProjectile;

    public int maxEnemyCnt;
    public int oneceSpawnCnt;
    public float spawnDelay = 5.0f;

    void Awake()
    {
        spawnPos = GameObject.FindWithTag("EnemySpawnPoint").GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        if(PhotonNetwork.isMasterClient)
        {
            PoolManager.Instance.CreatePhotonPool("Dragoon",enemyPrefabs[0],maxEnemyCnt);
            PoolManager.Instance.CreatePhotonPool("DragoonProjectile",enemyProjectile,maxEnemyCnt*3);                
        }
    }

    IEnumerator SpawnEnemy()
    {
        while(GameManager.Instance.startGame)
        {
            yield return new WaitForSeconds(spawnDelay);
            
            for(int i =0; i<oneceSpawnCnt; i++)
            {
                int randPos = Random.Range(1,spawnPos.Length);
                GameObject dragoon = PoolManager.Instance.PvGetObject("Dragoon",spawnPos[randPos].position,spawnPos[randPos].rotation);
                if (dragoon == null)
                {
                    continue;
                }
            }
        }
    }

    public void StartSpawn()
    {
        Debug.Log("Spawn Start");
        GameManager.Instance.SetStartGame(true);
        StartCoroutine(SpawnEnemy());
    }


}
