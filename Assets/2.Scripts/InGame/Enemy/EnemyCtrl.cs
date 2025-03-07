using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum State
{
    ROAMING,
    CHASE,
    DIE,
    ATTACK,
}
public class EnemyCtrl : MonoBehaviour
{
    public float maxHp = 1000;
    public float curHp;
    public bool isDead = false;

    private PhotonView pv;
    private Vector3 curPos;
    private Quaternion curRot;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        curHp = maxHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(float damage)
    {
        if(PhotonNetwork.isMasterClient)
        {
            Debug.Log("Test");
            pv.RPC("TakeDamage",PhotonTargets.AllBuffered,damage);
        }
    }

    [PunRPC]
    public void TakeDamage(float damage,PhotonMessageInfo info)
    {
        curHp -= damage;
        if(curHp<0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(curHp);
        }
        else
        {
            curHp = (float)stream.ReceiveNext(); 
        }
    }
}
