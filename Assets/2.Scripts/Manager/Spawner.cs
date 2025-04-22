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
    public int eliteEnemyCnt;
    public int oneceSpawnCnt;
    public float spawnDelay = 5.0f;

    void Awake()
    {
        spawnPos = GameObject.FindWithTag("EnemySpawnPoint").GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        if(!PhotonNetwork.isMasterClient)
        {
            return;
        }
        if(PhotonNetwork.isMasterClient)
        {
            PoolManager.Instance.CreatePhotonPool("Dragoon",enemyPrefabs[0],maxEnemyCnt);
            PoolManager.Instance.CreatePhotonPool("DragoonProjectile",enemyProjectile,maxEnemyCnt*3);                
        }
    }

    IEnumerator SpawnEnemy()
    {
    while (GameManager.Instance.startGame)
        {
            yield return new WaitForSeconds(spawnDelay);

            for (int i = 0; i < oneceSpawnCnt; i++)
            {
                int randPos = Random.Range(1, spawnPos.Length);

                // 30마리 중 1마리 확률로 eliteEnemy 스폰
                bool isElite = Random.Range(0, 30) == 0;

                if (isElite)
                {
                    // eliteEnemy 스폰
                    GameObject eliteEnemy = PoolManager.Instance.PvGetObject("Mantis", spawnPos[randPos].position, spawnPos[randPos].rotation);
                    if (eliteEnemy == null)
                    {
                        continue;
                    }
                }
                else
                {
                    // 일반 적 스폰
                    GameObject dragoon = PoolManager.Instance.PvGetObject("Dragoon", spawnPos[randPos].position, spawnPos[randPos].rotation);
                    if (dragoon == null)
                    {
                        continue;
                    }
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
