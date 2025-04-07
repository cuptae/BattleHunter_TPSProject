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
    private Rigidbody rigid;

    public MonsterHPBar hpBar; // ✅ HP 바 참조

    public int maxHp;
    public int curHp;
    public bool isDead = false;

    private PhotonView pv;
    private Vector3 curPos;
    private Quaternion curRot;
    public NavMeshAgent navMeshAgent;

    IEnemyState curState;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    void OnEnable()
    {
        curHp = maxHp;
        isDead = false;
        ChangeState(new ChaseState());

        hpBar = MonsterHPBarManager.Instance.CreateHPBar(this); // 새 체력바 생성
    }

    public void OnDisable()
    {
        if (hpBar != null)
        {
            MonsterHPBarManager.Instance.RemoveHPBar(hpBar);
            hpBar = null;
        }
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

    public void GetDamage(int damage)
    {
        if(PhotonNetwork.isMasterClient)
        {
            pv.RPC("TakeDamage",PhotonTargets.AllBuffered,damage);
        }
    }

    [PunRPC]
public void TakeDamage(int damage, PhotonMessageInfo info)
{
    curHp -= damage;
    Debug.Log(damage);

    // 🟡 체력바 보여주기 (몬스터에 달린 MonsterHPBar 호출)
    MonsterHPBar hpBar = GetComponentInChildren<MonsterHPBar>();
    if (hpBar != null)
        {
            hpBar.UpdateHPBarUI();
        }

    if (curHp <= 0)
    {
        ChangeState(new DieState());
    }
}




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(curHp);
        }
        else
        {
            curHp = (int)stream.ReceiveNext(); 
        }
    }
}
