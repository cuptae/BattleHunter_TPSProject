using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvAutoParticleReturn : MonoBehaviour
{
    PhotonView pv;
    private Vector3 curPos;
    private Quaternion curRot;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    
    void OnParticleSystemStopped()
    {
        if(gameObject.activeSelf == true)
            PoolManager.Instance.PvReturnObject(gameObject.name, gameObject);
    }

    void Start()
    {
        var ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
    [PunRPC]
    public void EnableObject(Vector3 pos,Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;

        // Lerp 기준값도 업데이트
        curPos = pos;
        curRot = rot;
        gameObject.SetActive(true);
    }
    [PunRPC]
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
