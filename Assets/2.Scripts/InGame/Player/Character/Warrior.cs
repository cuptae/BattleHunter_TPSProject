using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class Warrior : PlayerCtrl
{
    private Rig aimRig;
    public int comboStep = 0;
    private float lastClickTime = 0f;
    private float comboDelay = 1.0f;

    public bool canCombo = true;
    public Vector3 boxRange;
    public float boxFoward,boxUp;

    protected override void Awake()
    {
        base.Awake();
        characterStat.GetCharacterDataByName("Warrior");
        curHp = characterStat.MaxHp;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        canCombo = true;
    }
    protected override void Update()
    // Update is called once per frame
    {

        base.Update();
        if(Input.GetMouseButtonDown(0))
        {
            if(canCombo)
                HandleCombo();
        }

        if (Time.time - lastClickTime > comboDelay)
        {
            ResetCombo();
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
    private void HandleCombo()
    {
        lastClickTime = Time.time;
        StartCoroutine(CanCombo());
        animator.SetBool("Attack",true);
        animator.SetInteger("Combo",comboStep);
        Attack();
    }
    IEnumerator CanCombo()
    {
        if(comboStep == 0)
        {
            isAttack = true;
            comboStep+=1;
            canCombo = false;
        }
        else if(comboStep == 1)
        {
            comboStep+=1;
            canCombo = false;
        }
        else if(comboStep == 2)
        {
            comboStep+=1;
            canCombo = false; 
        }
        yield return new WaitForSeconds(characterStat.AttackRate);
        if(comboStep<3)
        {
            canCombo = true;
        }
        else
        {
            canCombo = true;
            ResetCombo();
        }
    }

    
    public void ResetCombo()
    {
        comboStep = 0;
        isAttack = false;
        animator.SetBool("Attack",isAttack);
        animator.SetInteger("Combo",comboStep);
    }


    protected override void Attack()
    {
        boxRange = new Vector3(3f,2f,2f);
        Vector3 attackPos = transform.position+transform.forward*boxFoward+transform.up*boxUp;
        Quaternion attackRot = transform.rotation;
        Collider[] monsterCollider = Physics.OverlapBox(attackPos, boxRange, attackRot, GameManager.Instance.enemyLayerMask);
        foreach(Collider col in monsterCollider)
        {
            Debug.Log(col.name);
            if(col != null)
            {
                EnemyCtrl enemy = col.transform.GetComponent<EnemyCtrl>();
                if(enemy != null)
                    enemy.GetDamage(characterStat.Damage);
            }
        }
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // 파란색으로 표시
        Vector3 attackPosition = transform.position+transform.forward*boxFoward+transform.up*boxUp; // OverlapBox 위치
        Vector3 boxSize = boxRange; // 박스 크기 설정
        Gizmos.matrix = Matrix4x4.TRS(attackPosition, transform.rotation, Vector3.one); // 회전 고려
        Gizmos.DrawWireCube(Vector3.zero, boxSize); // 박스를 그리기
    }

}
