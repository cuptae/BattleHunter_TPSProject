using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    CHASE,
    DIE,
    ATTACK,
}
public class EnemyCtrl : MonoBehaviour
{
    public float maxHp;
    public float curHp;
    public bool isDead = false;

    private PhotonView pv;
    private Vector3 curPos;
    private Quaternion curRot;
    public NavMeshAgent navMeshAgent;

    IEnemyState curState;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void OnEnable()
    {   
        curHp = maxHp;
        ChangeState(new ChaseState());
    }


    // Update is called once per frame
    void Update()
    {
        curState?.UpdateState(this);
    }

    public void ChangeState(IEnemyState newState)
    {
        curState?.ExitState(this); // 이전 상태 종료
        curState = newState;
        curState.EnterState(this); // 새로운 상태 진입
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
            ChangeState(new DieState());
        }
    }

    public void Die()
    {
        curHp = 0;
        isDead = true;
        PoolManager.Instance.ReturnObject("Mutant",this.gameObject);
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
