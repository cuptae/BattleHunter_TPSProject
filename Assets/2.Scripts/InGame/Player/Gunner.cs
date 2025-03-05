using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gunner : PlayerCtrl
{
    public GameObject[] bulletEffect;
    private Rig aimRig;
    private Ray ray;
    MultiAimConstraint multiAimConstraint;
    private float nextFireTime = 0f;

    public Transform aimingPos;
    public float damage = 56.7f;
    public float fireRate = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        aimRig = GetComponentInChildren<Rig>();
        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();
        aimingPos = GameObject.FindWithTag("AimingPos").transform;
    }

    void Start()
    {
        WeightedTransformArray sourceObjects = multiAimConstraint.data.sourceObjects;
        sourceObjects.Add(new WeightedTransform(aimingPos,aimRig.weight));

        for (int i = 0; i < bulletEffect.Length; i++)
        {
            PoolManager.Instance.CreatePool(bulletEffect[i].name, bulletEffect[i], 10);
        }
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
                Fire();
                nextFireTime = Time.time + fireRate; // 다음 발사 시간 설정
            }
        }
    }

    void FireAnim()
    {
        if (Input.GetMouseButton(0))
        {
            isFire = true;
            SetRigWeight(1);
            animator.SetBool("Fire", true);
        }
        else
        {
            isFire = false;
            SetRigWeight(0);
            animator.SetBool("Fire", false);
        }
    }

    void SetRigWeight(float weight)
    {
        aimRig.weight = weight;
    }

    void Fire()
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
                enemy.GetDamage(damage); // 부모의 EnemyCtrl에서 데미지 처리
                StartCoroutine(BulletEffect(hitInfo.point, hitDir));
            }
        }
    }

    protected override void  OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream,info);
        if(stream.isWriting)
        {
            stream.SendNext(isFire);
            stream.SendNext(animator.GetBool("Fire"));
        }
        else
        {
            isFire = (bool)stream.ReceiveNext();
            animator.SetBool("Fire",(bool)stream.ReceiveNext());
        }
    }

    IEnumerator BulletEffect(Vector3 pos, Quaternion rot)
    {
        GameObject effect = PoolManager.Instance.GetObject(bulletEffect[0].name,pos, rot);
        yield return new WaitForSeconds(0.3f);
        PoolManager.Instance.ReturnObject(bulletEffect[0].name,effect);
    }


}