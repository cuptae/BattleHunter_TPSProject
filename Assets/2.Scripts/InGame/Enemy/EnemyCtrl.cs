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

    public PhotonView pv;
    private Vector3 curPos;
    private Quaternion curRot;
    public NavMeshAgent navMeshAgent;
    public float attackRange = 11f; // ���� ��Ÿ�
    public Transform targetPlayer;
    public float rotationSpeed = 5f;

    IEnemyState curState;
    public EnemyState currState;

    public bool isTaunt;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        navMeshAgent = GetComponent<NavMeshAgent>();



    }

    void Start()
    {
        if(!pv.isMine)
        {
            rigid.isKinematic = true;
            curPos = tr.position;
            curRot = tr.rotation;
            navMeshAgent.enabled = false;
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
        }
        else
        {
            if(isDead)
            {
                gameObject.SetActive(false);
            }
        }
        curState?.UpdateState(this);
    }

    void FixedUpdate()
    {
        if (!pv.isMine)
        {
            if (navMeshAgent.enabled)
                navMeshAgent.enabled = false;

            tr.position = Vector3.Lerp(tr.position, curPos, Time.fixedDeltaTime * 10f);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.fixedDeltaTime * rotationSpeed);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        curState?.ExitState(this); // 이전 상태 종료
        curState = newState;
        curState.EnterState(this); // 새로운 상태 진입
    }



    public virtual void Attack(){}


    public Transform FindClosestPlayer()
    {
        if(isTaunt)
            return targetPlayer;
        else
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
    }

    public void GetDamage(int damage)
    {
        if (!PhotonNetwork.isMasterClient)
        {
            pv.RPC("RequestDamage", PhotonTargets.MasterClient, damage);
        }
        else
        {
            TakeDamage(damage, new PhotonMessageInfo()); // 로컬 마스터가 직접 처리
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        // 🟡 체력바 보여주기 (몬스터에 달린 MonsterHPBar 호출)
        MonsterHPBar hpBar = GetComponentInChildren<MonsterHPBar>();
        if (hpBar != null)
        {
            hpBar.UpdateHPBarUI();
        }
        curHp -= damage;
        Debug.Log(damage);


        if (curHp <= 0)
        {

            ChangeState(new EnemyDieState());
        }
    }

    public void Taunt(Transform target, float time)
    {

        pv.RPC("RPC_Taunt", PhotonTargets.All, target.GetComponent<PhotonView>().viewID, time);
        
    }
    [PunRPC]
    public void RPC_Taunt(int targetViewID, float duration)
    {
        Debug.Log("RPC_Taunt Call");
        PhotonView targetPv = PhotonView.Find(targetViewID);
        if (targetPv != null)
        {
            targetPlayer = targetPv.transform;
            isTaunt = true;
            Invoke(nameof(UnTaunt), 5.0f);
        }
        else
        {
            Debug.Log("targetPv is null");
        }
    }
    [PunRPC]
    public void RequestDamage(int damage, PhotonMessageInfo info)
    {
        // 마스터에서만 실행됨
        TakeDamage(damage, info);
    }
    public void Die()
    {
        if (PhotonNetwork.isMasterClient)
        {
            // 모든 클라이언트에게 비활성화 지시
            PoolManager.Instance.PvReturnObject("Dragoon", gameObject);
        }
    }


    [PunRPC]
    public void EnableObject()
    {
        gameObject.SetActive(true);
    }
    [PunRPC]
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    // public void Taunt(Transform target,float time)
    // {
    //     targetPlayer = target;
    //     isTaunt = true;

    //     //Invoke("UnTaunt",200f);
    // }

    public void UnTaunt()
    {
        isTaunt = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(curHp);
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(currState);
            stream.SendNext(isTaunt);
            //stream.SendNext(targetPlayer);
        }
        else
        {
            curHp = (int)stream.ReceiveNext(); 
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
            currState = (EnemyState)stream.ReceiveNext();
            isTaunt = (bool)stream.ReceiveNext();
            //targetPlayer = (Transform)stream.ReceiveNext();
        }
    }



    // public void Effected(SKILLCONSTANT.SkillEffect effect)
    // {
    //     switch(effect)
    //     {
    //         case SKILLCONSTANT.SkillEffect.
    //     }
    // }
}
