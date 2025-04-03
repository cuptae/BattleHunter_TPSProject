using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Warrior : PlayerCtrl
{
    private Rig aimRig;
    public bool canCombo = true;
    public int attackCombo = 0;  // 0: 대기, 1~3: 콤보 진행 중
    public Vector3 boxRange;
    public float boxFoward, boxUp;
    
    protected override void Awake()
    {
        base.Awake();
        characterStat.GetCharacterDataByName("Warrior");
        curHp = characterStat.MaxHp;
    }

    protected override void Start()
    {
        base.Start();
        canCombo = true;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("Shield", true);
        }
        else
        {
            animator.SetBool("Shield", false);
        }

        if (Input.GetMouseButtonDown(0) && canCombo)
        {
            StartCombo();
        }
    }

    void StartCombo()
    {
        if (isAttack) return;

        attackCombo = 1; // 첫 번째 공격 시작
        isAttack = true;
        canCombo = false;
        animator.SetBool("Attack", true);
        animator.SetInteger("Combo", attackCombo);
    }

    public void ContinueCombo()
    {
        if (attackCombo < 3) // 3단 공격까지 가능
        {
            attackCombo++;
            animator.SetInteger("Combo", attackCombo);
        }
        else
        {
            EndCombo();
        }
    }

    public void EndCombo()
    {
        attackCombo = 0;
        animator.SetBool("Attack", false);
        animator.SetInteger("Combo", 0);
        isAttack = false;
        canCombo = true;
    }

    public override void Attack()
    {
        if (attackCombo == 0) return;

        boxRange = new Vector3(3f, 2f, 2f);
        Vector3 attackPos = transform.position + transform.forward * boxFoward + transform.up * boxUp;
        Quaternion attackRot = transform.rotation;
        Collider[] monsterCollider = Physics.OverlapBox(attackPos, boxRange, attackRot, GameManager.Instance.enemyLayerMask);
        
        foreach (Collider col in monsterCollider)
        {
            if (col != null)
            {
                EnemyCtrl enemy = col.GetComponent<EnemyCtrl>();
                if (enemy != null)
                {
                    enemy.GetDamage(characterStat.Damage);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 attackPosition = transform.position + transform.forward * boxFoward + transform.up * boxUp;
        Gizmos.matrix = Matrix4x4.TRS(attackPosition, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxRange);
    }
}
