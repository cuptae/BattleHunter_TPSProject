using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using SKILLCONSTANT;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    CHASE,
    DIE,
    ATTACK,
    STUN,
    KNOCKBACK
}
public class EnemyCtrl : MonoBehaviour,IDamageable
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

    public int amorBreakRate = 1;

    public bool isStunned = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        curHp = maxHp;
        isDead = false;
        ChangeState(new EnemyChaseState());
        UpdateTargetPlayer();
        InvokeRepeating("UpdateTargetPlayer",0f,1.0f);
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


    // Update is called once per frame
    protected virtual void Update()
    {
        if (isDead) return;
        if (isStunned) return;

        if(pv.isMine)
        {
            curState?.UpdateState(this);
        }
        else
        {
            if(isDead)
            {
                gameObject.SetActive(false);
            }
        }
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
    public Transform FindClosestPlayer()
    {
        if(isTaunt) return targetPlayer;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> alivePlayers = new List<GameObject>();

        // 살아 있는 플레이어만 필터링
        foreach (GameObject player in players)
        {
            PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
            if (playerCtrl != null && !playerCtrl.isDead)
            {
                alivePlayers.Add(player);
            }
        }

        // 살아 있는 플레이어가 없으면 null 반환
        if (alivePlayers.Count == 0)
        {
            return null;
        }

        // 거리 순으로 정렬
        alivePlayers.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(transform.position, a.transform.position);
            float distanceB = Vector3.Distance(transform.position, b.transform.position);
            return distanceA.CompareTo(distanceB);
        });

        // 가장 가까운 플레이어 반환
        return alivePlayers[0].transform;
    }


    public virtual void Attack(){}


    private void UpdateTargetPlayer()
    {
        targetPlayer = FindClosestPlayer();
    }



    public void GetDamage(int damage,ActiveData skill = null)
    {
        if (!PhotonNetwork.isMasterClient)
        {
            int casterViewID = skill != null ? skill.caster.GetComponent<PhotonView>().viewID : -1;
            SkillEffect effect = skill != null ? skill.skillEffectParam : SkillEffect.NONE;
            float duration = skill != null ? skill.duration : 0f;
             pv.RPC("RequestDamage", PhotonTargets.MasterClient, damage, (int)effect, duration, casterViewID);
        }
        else
        {
            TakeDamage(damage,skill); // 로컬 마스터가 직접 처리
        }
    }

    public void TakeDamage(int damage,ActiveData skill = null)
    {
        MonsterHPBar hpBar = GetComponentInChildren<MonsterHPBar>();
        if (hpBar != null)
        {
            hpBar.UpdateHPBarUI();
        }
        int totalDamage = damage*amorBreakRate;
        curHp -= totalDamage;
        Debug.Log(totalDamage);


        if (curHp <= 0)
        {

            ChangeState(new EnemyDieState());
        }
        if(!isDead&&skill != null)
        {
            switch(skill.skillEffectParam)
            {
                case SkillEffect.TAUNT:
                Taunt(skill.caster.transform,skill.duration);
                break;
                case SkillEffect.STUN:
                Stun(skill.duration);
                break;
                case SkillEffect.AMORBREAK:
                Debug.Log("아머브레이크");
                AmorBreak(skill.duration);
                break;
                case SkillEffect.KNOCKBACK:
                Debug.Log("넉백!");
                KnockBack(skill.caster.transform);
                break;
                default:
                break;
            }
        }

    }

    [PunRPC]
    public void RequestDamage(int damage, int effect, float duration, int casterViewID)
    {
        ActiveData skill = null;
        if (effect != (int)SkillEffect.NONE && casterViewID != -1)
        {
            GameObject casterObj = PhotonView.Find(casterViewID)?.gameObject;
            if (casterObj != null)
            {
                skill = new ActiveData();
                skill.SetSkillEffectParam((SkillEffect)effect);
                skill.SetDuration(duration);
                skill.SetCaster(casterObj.GetComponent<PlayerCtrl>());
            }
        }

        TakeDamage(damage, skill);
    }

    public void Taunt(Transform target, float time)
    {
        pv.RPC("RPC_Taunt", PhotonTargets.All, target.GetComponent<PhotonView>().viewID, time);
    }
    [PunRPC]
    public void RPC_Taunt(int targetViewID, float duration)
    {
        PhotonView targetPv = PhotonView.Find(targetViewID);
        if (targetPv != null)
        {
            targetPlayer = targetPv.transform;
            isTaunt = true;
            //hpBar.deBuff[0].SetActive(true);
            EnalbeDebuffMark(0);
            Invoke(nameof(UnTaunt), duration);
        }
        else
        {
            Debug.Log("targetPv is null");
        }
    }
    public void UnTaunt()
    {
        if(hpBar.deBuff[0] != null)
        {
            //hpBar.deBuff[0].SetActive(false);
            DisableDebuffMark(0);
        }
        isTaunt = false;
    }

    public void Stun(float duration){pv.RPC("RPC_Stun", PhotonTargets.All, duration);}
    [PunRPC]
    public void RPC_Stun(float duration)
    {
        isStunned = true;
        Invoke("UnStun", duration);
        // 마크 표시
        EnalbeDebuffMark(1);
    }
    public void UnStun()
    {
        isStunned = false;
        // 마크 제거
        DisableDebuffMark(1);
    }

    public void AmorBreak(float duration)
    {
        pv.RPC("RPC_AmorBreak", PhotonTargets.All, duration);
    }
    [PunRPC]
    public void RPC_AmorBreak(float duration)
    {
        amorBreakRate = 2;
        // 마크 표시
        EnalbeDebuffMark(2);
        Invoke("RecoverAmor",duration);
    }
    public void RecoverAmor()
    {
        amorBreakRate = 1;
        // 마크 제거
        DisableDebuffMark(2);
    }




    public void KnockBack(Transform caster)
    {
        int casterViewID = caster.GetComponent<PhotonView>().viewID;
        pv.RPC("RPC_KnockBack", PhotonTargets.All, casterViewID);
    }

    [PunRPC]
    public void RPC_KnockBack(int casterViewID)
    {
        PhotonView casterPv = PhotonView.Find(casterViewID);
        if (casterPv != null)
        {
            Transform casterTr = casterPv.transform;
            ChangeState(new EnemyKnockBackState(casterTr));
        }
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
    public void EnalbeDebuffMark(int mark)
    {
        hpBar.deBuff[mark].SetActive(true);       
    }
    [PunRPC]
    public void DisableDebuffMark(int mark)
    {
        hpBar.deBuff[mark].SetActive(false);
    }

    [PunRPC]
    public void RPC_EnableRigBuilder()
    {
        GetComponentInChildren<UnityEngine.Animations.Rigging.RigBuilder>().enabled = true;
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

    [PunRPC]
    public void PlusPoint()
    {
        GameManager.Instance.AddPoint();
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
        }
        else
        {
            curHp = (int)stream.ReceiveNext(); 
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
            currState = (EnemyState)stream.ReceiveNext();
            isTaunt = (bool)stream.ReceiveNext();
        }
    }
}
