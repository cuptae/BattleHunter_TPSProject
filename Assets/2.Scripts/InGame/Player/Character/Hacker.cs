using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Hacker : PlayerCtrl
{

    public GameObject bulletEffect;
    private Rig aimRig;
    private Ray ray;
    MultiAimConstraint multiAimConstraint;
    private float nextFireTime = 0f;
    private Transform aimingPos;

    protected override void Awake()
    {
        base.Awake();
        aimRig = GetComponentInChildren<Rig>();
        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();
        aimingPos = GameObject.FindWithTag("AimingPos").transform;
        characterStat.GetCharacterDataByName("Hacker");
        curHp = characterStat.MaxHp;
    }

    protected override void Start()
    {
        base.Start();
        //WeightedTransformArray sourceObjects = multiAimConstraint.data.sourceObjects;
        //sourceObjects.Add(new WeightedTransform(aimingPos,aimRig.weight));
        PoolManager.Instance.CreatePool(bulletEffect.name, bulletEffect, 10);
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if(pv.isMine)
        {
            FireAnim();
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                Attack();
                nextFireTime = Time.time + characterStat.AttackRate; // 다음 발사 시간 설정
            }
        }
    }

    void FireAnim()
    {
        if (Input.GetMouseButton(0))
        {
            isAttack = true;
            //SetRigWeight(1);
            animator.SetBool("Attack", true);
        }
        else
        {
            isAttack = false;
            //SetRigWeight(0);
            animator.SetBool("Attack", false);
        }
    }


    public override void Attack()
    {
        if(isDodge)
            return;
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = ray.direction.normalized;
        ray = new Ray(mainCamera.transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 50.0f))
        {
            aimingPos.transform.position = hitInfo.point + Vector3.up * 0.5f;
            // 부모에서 EnemyCtrl을 찾도록 수정
            EnemyCtrl enemy = hitInfo.collider.GetComponentInParent<EnemyCtrl>();

            if (enemy != null) // 적 오브젝트를 찾았다면
            {
                Quaternion hitDir = Quaternion.LookRotation(-direction);
                enemy.GetDamage(characterStat.Damage); // 부모의 EnemyCtrl에서 데미지 처리
                StartCoroutine(BulletEffect(hitInfo.point, hitDir));
            }
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

    IEnumerator BulletEffect(Vector3 pos, Quaternion rot)
    {
        GameObject effect = PoolManager.Instance.GetObject(bulletEffect.name,pos, rot);
        yield return new WaitForSeconds(0.3f);
        PoolManager.Instance.ReturnObject(bulletEffect.name,effect);
    }
 
 
    // void SetRigWeight(float weight)
    // {
    //     aimRig.weight = weight;
    // }

}