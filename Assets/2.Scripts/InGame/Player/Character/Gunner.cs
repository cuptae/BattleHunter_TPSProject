using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class Gunner : PlayerCtrl
{
    public GameObject bulletEffect;
    public GameObject gunFire;
    private Rig aimRig;
    private Ray ray;
    MultiAimConstraint multiAimConstraint;
    private float nextFireTime = 0f;
    private Transform aimingPos;
    public Transform firePos;
    public float attackDistance;
    public float attackRange;
    

    protected override void Awake()
    {
        base.Awake();
        characterStat.GetCharacterDataByName("Gunner");
        aimRig = GetComponentInChildren<Rig>();
        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();
        aimingPos = GameObject.FindWithTag("AimingPos").transform;
        //curHp = characterStat.MaxHp;
        SetHPInit(characterStat.MaxHp);
    }

    protected override void Start()
    {
        base.Start();
        WeightedTransformArray sourceObjects = multiAimConstraint.data.sourceObjects;
        sourceObjects.Add(new WeightedTransform(aimingPos,aimRig.weight));
        PoolManager.Instance.CreatePool(bulletEffect.name, bulletEffect, 10);
        PoolManager.Instance.CreatePool(gunFire.name,gunFire,10);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if(pv.isMine)
        {
            FireAnim();
            // if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            // {
            //     Attack();
            //     nextFireTime = Time.time + characterStat.AttackRate; // 다음 발사 시간 설정
            // }
        }
    }

    void FireAnim()
    {
        if (Input.GetMouseButton(0))
        {
            isAttack = true;
            SetRigWeight(1);
            animator.SetBool("Attack", true);
        }
        else
        {
            isAttack = false;
            SetRigWeight(0);
            animator.SetBool("Attack", false);
        }
    }

    void SetRigWeight(float weight)
    {
        aimRig.weight = weight;
    }

    public override void Attack()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {

            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            Vector3 direction = ray.direction.normalized;
            ray = new Ray(firePos.position, direction);
            FireEffect();
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 50.0f))
            {
                aimingPos.transform.position = hitInfo.point + Vector3.up * 0.5f;
                // 부모에서 EnemyCtrl을 찾도록 수정
                var enemy = hitInfo.collider.GetComponent<IDamageable>();
                
                Quaternion hitDir = Quaternion.LookRotation(-direction);
                if (enemy != null) // 적 오브젝트를 찾았다면
                {
                    enemy.GetDamage(characterStat.Damage); 
                    BulletEffect(hitInfo.point, hitDir);
                }
                // if (Boss != null)
                // {
                //     Boss.GetDamage(characterStat.Damage);
                //     BulletEffect(hitInfo.point, hitDir); // 중복되더라도 상관 없으면 그대로
                // }
            }
            SoundManager.Instance.PlaySFX(SFXCategory.PLAYER, PLAYER.ATTACK, tr.position);
            nextFireTime = Time.time + characterStat.AttackRate; // 다음 발사 시간 설정
        }
    }
    protected override void  OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream,info);
        if(stream.isWriting)
        {
            stream.SendNext(isAttack);
            stream.SendNext(animator.GetBool("Attack"));
        }
        else
        {
            isAttack = (bool)stream.ReceiveNext();
            animator.SetBool("Attack",(bool)stream.ReceiveNext());
        }
    }

    void BulletEffect(Vector3 pos, Quaternion rot)
    {
        GameObject effect = PoolManager.Instance.GetObject(bulletEffect.name,pos, rot);
    }
    void FireEffect()
    {
        GameObject gunFire = PoolManager.Instance.GetObject(this.gunFire.name,firePos.position,Quaternion.LookRotation(firePos.forward));
    }

    private void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR

            // 공격 범위 세팅
            Vector3 boxRange = new Vector3(attackRange, 3f, attackDistance);
            Vector3 boxCenter = new Vector3(0, -1, -1 + boxRange.z / 2f);
            Vector3 attackPos = transform.position + transform.forward * 2f + transform.up * 2f;
            Quaternion attackRot = transform.rotation;

            // 최종 중심 계산
            Vector3 center = attackPos + attackRot * boxCenter;

            // 회전 행렬 적용
            Gizmos.color = Color.red;
            Matrix4x4 rotMatrix = Matrix4x4.TRS(center, attackRot, Vector3.one);
            Gizmos.matrix = rotMatrix;

            Gizmos.DrawWireCube(Vector3.zero, boxRange);
        #endif
    }
}


