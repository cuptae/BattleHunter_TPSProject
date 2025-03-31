using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class Gunner : PlayerCtrl
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
        characterStat.GetCharacterDataByName("Gunner");
        aimRig = GetComponentInChildren<Rig>();
        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();
        aimingPos = GameObject.FindWithTag("AimingPos").transform;
        curHp = characterStat.MaxHp;
    }

    protected override void Start()
    {
        base.Start();
        WeightedTransformArray sourceObjects = multiAimConstraint.data.sourceObjects;
        sourceObjects.Add(new WeightedTransform(aimingPos,aimRig.weight));
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

    protected override void Attack()
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
        SoundManager.Instance.PlaySFX(SFXCategory.PLAYER, PLAYER.ATTACK, tr.position);
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

    private void OnDrawGizmos()
    {

        Vector3 boxRange = new Vector3(3f, 2f, 3f);
        Vector3 attackPos = transform.position + transform.forward * 2f + transform.up * 2f;
        Quaternion attackRot = transform.rotation;

        Gizmos.color = Color.red; // 기즈모 색상 설정
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(attackPos, attackRot, Vector3.one); // 회전 적용

        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, boxRange); // 중심을 (0,0,0)으로 설정하여 회전 적용
    }
}