using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPos;
    public GameObject enemyPrefabs;

    void Start()
    {
        if(PhotonNetwork.isMasterClient)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        GameObject enemy = PhotonNetwork.Instantiate("Mutant",spawnPos[0].position,spawnPos[0].rotation,0);
    }
}
