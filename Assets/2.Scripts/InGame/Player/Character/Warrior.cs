using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Warrior : PlayerCtrl
{
    private Rig aimRig;
    public Vector3 boxRange;
    public float boxFoward, boxUp;
    public GameObject shildEffect;
    
    protected override void Awake()
    {
        base.Awake();
        characterStat.GetCharacterDataByName("Warrior");
        //curHp = characterStat.MaxHp;
        SetHPInit(characterStat.MaxHp);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("WAttack");
        }
        UniqueAbility();
    }

    public override void Attack()
    {
        boxRange = new Vector3(3f, 2f, 2f);
        Vector3 attackPos = transform.position + transform.forward * boxFoward + transform.up * boxUp;
        Quaternion attackRot = transform.rotation;
        Collider[] monsterCollider = Physics.OverlapBox(attackPos, boxRange, attackRot, GameManager.Instance.enemyLayerMask);
        
        foreach (Collider col in monsterCollider)
        {
            if (col != null)
            {
                var enemy = col.GetComponent<IDamageable>();
                if (enemy != null)
                {
                    enemy.GetDamage(characterStat.Damage);
                }
            }
        }
    }
    public override void UniqueAbility()
    {
        if (!canAbility) // canAbility가 false면 UniqueAbility를 사용할 수 없음
        {
            shildEffect.SetActive(false); // 방패 이펙트 비활성화
            damageReduceRate = 1f; // 원래대로 돌아옴
            // 방패를 내릴 때 게이지를 천천히 채움
            abilitycooldownbar.fillAmount = Mathf.MoveTowards(abilitycooldownbar.fillAmount, 1f, Time.deltaTime / 5f);
            if (abilitycooldownbar.fillAmount >= 1f)
            {
                canAbility = true; // 게이지가 완전히 차면 다시 사용할 수 있음
            }
            animator.SetBool("Shield", false);
            return;
        }
    
        if (Input.GetMouseButton(1) && abilitycooldownbar.fillAmount > 0f)
        {
            // 방패를 들고 있는 동안
            damageReduceRate = 0.25f; // 25% 데미지만 받음
            abilitycooldownbar.fillAmount -= Time.deltaTime / 5f; // 5초 동안 완전히 닳도록 설정
            shildEffect.SetActive(true); // 방패 이펙트 활성화
            animator.SetBool("Shield", true);
    
            if (abilitycooldownbar.fillAmount <= 0f)
            {
                canAbility = false; // 게이지가 0이 되면 UniqueAbility를 사용할 수 없도록 설정
            }
        }
        else
        {
            // 방패를 내릴 때
            damageReduceRate = 1f; // 원래대로 돌아옴
            abilitycooldownbar.fillAmount = Mathf.MoveTowards(abilitycooldownbar.fillAmount, 1f, Time.deltaTime / 10f); // 5초 동안 완전히 차도록 설정
            shildEffect.SetActive(false); // 방패 이펙트 비활성화
            animator.SetBool("Shield", false);
    
            if (abilitycooldownbar.fillAmount >= 1f)
            {
                canAbility = true; // 게이지가 완전히 차면 다시 사용할 수 있음
            }
        }
    }
    // public override void UniqueAbility()
    // {
    //     if (Input.GetMouseButton(1) && abilitycooldownbar.fillAmount > 0f)
    //     {
    //         // 방패를 들고 있는 동안
    //         damageReduceRate = 0.25f;//25%데미지만 받음
    //         abilitycooldownbar.fillAmount -= Time.deltaTime / 5f; // 5초 동안 완전히 닳도록 설정
    //         animator.SetBool("Shield", true);
    //     }
    //     else
    //     {
    //         // 방패를 내릴 때
    //         damageReduceRate = 1f; // 원래대로 돌아옴
    //         abilitycooldownbar.fillAmount = Mathf.MoveTowards(abilitycooldownbar.fillAmount, 1f, Time.deltaTime / 5f); // 5초 동안 완전히 차도록 설정
    //         animator.SetBool("Shield", false);
    //     }
    // }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 attackPosition = transform.position + transform.forward * boxFoward + transform.up * boxUp;
        Gizmos.matrix = Matrix4x4.TRS(attackPosition, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxRange);
    }


}
