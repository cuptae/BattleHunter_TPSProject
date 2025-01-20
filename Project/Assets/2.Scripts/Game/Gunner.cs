using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gunner : PlayerCtrl
{
    private Rig aimRig;
    private Ray ray;
    public Transform firePos;
    public GameObject aimObj;

    protected override void Awake()
    {
        base.Awake();
        aimRig = GetComponentInChildren<Rig>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Shooting();
        Aiming();
    }

    void Shooting()
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

    void Aiming()
    {
        // ���콺 ��ġ�� �������� Ray ����
        Ray aimRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        // �ѱ����� Ray�� ���� ����
        Vector3 direction = aimRay.direction.normalized;
        Ray fireRay = new Ray(firePos.position, direction);

        // Ray ����� (�� �信�� Ȯ�� ����)
        Debug.DrawRay(firePos.position, direction * 30f, Color.red);

        // Raycast ó��
        if (Physics.Raycast(fireRay, out RaycastHit hitInfo, Mathf.Infinity))
        {
            aimObj.transform.position = hitInfo.point + Vector3.up * 0.5f;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Debug.Log($"Hit {hit.collider.name}");

        }
    }


}
