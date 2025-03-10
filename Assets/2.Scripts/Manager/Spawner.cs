using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPos;
    public GameObject[] enemyPrefabs;

    public int maxEnemyCnt;

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
            yield return new WaitForSeconds(3.0f);
            PoolManager.Instance.GetObject("Mutant",spawnPos[1].position,spawnPos[1].rotation);
        }
    }
}
