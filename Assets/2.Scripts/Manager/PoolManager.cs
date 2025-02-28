using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{   
    //프리팹들을 보관할 변수
    public GameObject[] prefabs; 
    //풀 담당 리스트
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        Debug.Log(pools.Length);
        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }
    public GameObject GetObject(int idx,Vector3 pos, Quaternion dir)
    {
        GameObject select = null;
        foreach(GameObject go in pools[idx])
        {
            if(!go.activeSelf)
            {
                select = go;
                select.transform.position = pos;
                select.transform.rotation = dir;
                select.SetActive(true);
                break;
            }
        }
        if(!select)
        {
            select =Instantiate(prefabs[idx],pos,dir);
            select.transform.parent = transform;
            pools[idx].Add(select);
        }
        return select;
    }
}
