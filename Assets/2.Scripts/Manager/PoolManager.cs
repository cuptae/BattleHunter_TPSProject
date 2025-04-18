using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.LowLevel;

public class PoolManager : MonoSingleton<PoolManager>
{  
    //오브젝트 풀 딕셔너리
    private Dictionary<string,Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    public void CreatePool(string key, GameObject prefab, int size)        
    {
        GameObject pa = new GameObject(key);
        //만약 풀에 해당 key 값이 없다면
        if(!pool.ContainsKey(key))
        {
            //해당 key값을 가진 큐를 딕셔너리에 생성
            pool[key] = new Queue<GameObject>();

            //전해진 size만큼 for문
            for(int i = 0; i< size; i++)
            {
                GameObject go = Instantiate(prefab,pa.transform);//프리팹 생성
                go.transform.name = prefab.name;
                go.SetActive(false);//비활성
                pool[key].Enqueue(go);//큐에 넣는다
            }
        }
    }

    public void CreatePhotonPool(string key, GameObject prefab, int size)
    {
        GameObject pa = new GameObject(key);
        //만약 풀에 해당 key 값이 없다면
        if(!pool.ContainsKey(key))
        {
            //해당 key값을 가진 큐를 딕셔너리에 생성
            pool[key] = new Queue<GameObject>();

            //전해진 size만큼 for문
            for(int i = 0; i< size; i++)
            {
                GameObject go = PhotonNetwork.Instantiate(prefab.name,transform.position,transform.rotation,0);//프리팹 생성
                go.SetActive(false);//비활성
                go.transform.parent = pa.transform;
                go.transform.name = prefab.name;
                pool[key].Enqueue(go);//큐에 넣는다
            }
        }
    }

    public GameObject GetObject(string key,Vector3 pos, Quaternion rot)
    {
        //풀이 키값을 가지고 있고 그 키값을 가진 풀의 요소가 하나 이상일 때
        if(pool.ContainsKey(key)&&pool[key].Count>0)
        {
            GameObject go = pool[key].Dequeue();
            go.transform.position = pos;
            go.transform.rotation = rot;
            if(go.activeSelf == false)
            {
                go.SetActive(true);
            }
            return go;
        }
        return null;
    }

    public GameObject PvGetObject(string key, Vector3 pos, Quaternion rot)
    {
        if (pool.ContainsKey(key) && pool[key].Count > 0)
        {
            GameObject go = pool[key].Dequeue();
            go.transform.position = pos;
            go.transform.rotation = rot;

            PhotonView pv = go.GetComponent<PhotonView>();
            if (pv != null)
            {
                // 모든 클라이언트에서 활성화하도록 RPC 호출
                pv.RPC("EnableObject", PhotonTargets.All, pos, rot);
            }
            else
            {
                Debug.LogWarning("PhotonView not found on pooled object.");
                go.SetActive(true);
            }

            return go;
        }
        return null;
    }



    public void ReturnObject(string key, GameObject go)
    {
        go.SetActive(false);
        if (pool.ContainsKey(key))
        {
            pool[key].Enqueue(go);
        }
        else
        {
            pool[key] = new Queue<GameObject>();
            pool[key].Enqueue(go);
        }
    }

    public void PvReturnObject(string key, GameObject go)
    {
        PhotonView pv = go.GetComponent<PhotonView>();
        if (pv != null)
        {
            pv.RPC("DisableObject", PhotonTargets.All);
        }
        else
        {
            go.SetActive(false); // fallback
        }

        if (pool.ContainsKey(key))
        {
            pool[key].Enqueue(go);
        }
        else
        {
            pool[key] = new Queue<GameObject>();
            pool[key].Enqueue(go);
        }
    }

}
