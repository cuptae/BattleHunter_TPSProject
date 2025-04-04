using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Warrior : PlayerCtrl
{
    private Rig aimRig;
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
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("WAttack");

        }

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("Shield", true);
        }
        else
        {
            animator.SetBool("Shield", false);
        }

  
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
