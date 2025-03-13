using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPos;
    public GameObject[] enemyPrefabs;

    public int maxEnemyCnt;
    public int oneceSpawnCnt;

    void Awake()
    {
        spawnPos = GameObject.FindWithTag("EnemySpawnPoint").GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        if(PhotonNetwork.isMasterClient)
        {
            PoolManager.Instance.CreatePhotonPool("Mutant",enemyPrefabs[0],maxEnemyCnt);
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);
            
            for(int i =0; i<oneceSpawnCnt; i++)
            {
                int randPos = Random.Range(1,spawnPos.Length);
                Debug.Log(randPos);
                GameObject Mutant = PoolManager.Instance.GetObject("Mutant",spawnPos[randPos].position,spawnPos[randPos].rotation);
            }
        }
    }
}
