using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gunner : PlayerCtrl
{
    private Rig aimRig;
    private Ray ray;
    MultiAimConstraint multiAimConstraint;
    private float nextFireTime = 0f;

    public Transform aimingPos;
    public float damage = 56.7f;
    public float fireRate = 0.05f;

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
            if(hitInfo.collider.tag == "Enemy")
            {
                Quaternion hitDir = Quaternion.LookRotation(-direction);
                Debug.Log($"{hitInfo.collider.name}'s HP : {hitInfo.collider.GetComponent<EnemyCtrl>().GetDamage(damage,hitInfo.point,hitDir)}");
            }
        }
    }


}