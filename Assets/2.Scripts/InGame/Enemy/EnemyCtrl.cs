using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    CHASE,
    DIE,
    ATTACK,
}
public class EnemyCtrl : MonoBehaviour
{
    private Rigidbody rigid;
    protected Transform tr;

    public MonsterHPBar hpBar; // ✅ HP 바 참조

    public int maxHp;
    public int curHp;
    public bool isDead = false;

    private PhotonView pv;
    private Vector3 curPos;
    private Quaternion curRot;
    public NavMeshAgent navMeshAgent;
    public float attackRange = 11f; // ���� ��Ÿ�
    public Transform targetPlayer;
    public float rotationSpeed = 5f;

    IEnemyState curState;
    public EnemyState currState;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        navMeshAgent = GetComponent<NavMeshAgent>();

        if(!pv.isMine)
        {
            rigid.isKinematic = true;
            // curPos = tr.position;
            // curRot = tr.rotation;
        }

    }
    void OnEnable()
    {
        curHp = maxHp;
        isDead = false;
        ChangeState(new EnemyChaseState());

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
    protected virtual void Update()
    {
        targetPlayer = FindClosestPlayer();
        if(pv.isMine)
        {
            
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            curState?.UpdateState(this);
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position,curPos,Time.fixedDeltaTime *navMeshAgent.speed);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.fixedDeltaTime * rotationSpeed);
        }
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


    public virtual void Attack(){}


    public Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }
        return closestPlayer;
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
            ChangeState(new EnemyDieState());
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(curHp);
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);

        }
        else
        {
            curHp = (int)stream.ReceiveNext(); 
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
