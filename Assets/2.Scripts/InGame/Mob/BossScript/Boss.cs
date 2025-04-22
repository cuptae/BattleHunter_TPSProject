using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject colliderObject1;
    public GameObject colliderObject2;

    public GameObject ColliderObject1 => colliderObject1;
    public GameObject ColliderObject2 => colliderObject2;

    [Header("Ÿ��")]
    public GameObject currentTarget;

    void Start()
    {
        currentTarget = GameObject.FindGameObjectWithTag("Player");
    }

    public void DisableAttackColliders()
    {
        if (ColliderObject1 != null)
        {
            Collider col1 = ColliderObject1.GetComponent<Collider>();
            if (col1 != null) col1.enabled = false;
        }

        if (ColliderObject2 != null)
        {
            Collider col2 = ColliderObject2.GetComponent<Collider>();
            if (col2 != null) col2.enabled = false;
        }

        Debug.Log("�ִϸ��̼� �̺�Ʈ: �ݶ��̴� ��Ȱ��ȭ �Ϸ�");
    }
}
