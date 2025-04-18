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

    public GameObject[] dropItems;
    [Range(0f, 1f)] public float dropChance = 0.5f; // 드랍될 확률 (70%)

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
    if (curHp < 0) curHp = 0;

    // UI 업데이트
    if (hpBar != null)
    {
        hpBar.UpdateHPBarUI();
    }

    // 기존 DieState 진입 제거
    // if (curHp <= 0) ChangeState(new DieState());

    // 대신 코루틴으로 지연 처리
    if (curHp <= 0)
    {
        StartCoroutine(WaitForHPBarDepletion());
        Vector3 dropPos = transform.position + Vector3.up * 1.0f; // 위로 살짝 띄우기
        DropItem();
    }
}

private IEnumerator WaitForHPBarDepletion()
{
    // hpBar가 null일 수도 있으니 예외처리
    if (hpBar != null)
    {
        // HP 슬라이더가 0에 가까워질 때까지 기다림
        while (hpBar.hpSlider != null && hpBar.hpSlider.value > 0.01f)
        {
            yield return null;
        }
    }

    // 완전히 닳았으면 오브젝트 제거
    ChangeState(new DieState());
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

    public void DropItem()
    {
        float roll = Random.value; // 0 ~ 1 사이의 랜덤 값

        if (roll < dropChance)
        {
            int rand = Random.Range(0, dropItems.Length);
            GameObject selectedItem = dropItems[rand];
            Instantiate(selectedItem, transform.position + Vector3.up, Quaternion.identity);
            Debug.Log("드랍된 아이템: " + selectedItem.name);
        }
        else
        {
            Debug.Log("아이템 드랍되지 않음");
        }
    }
}
