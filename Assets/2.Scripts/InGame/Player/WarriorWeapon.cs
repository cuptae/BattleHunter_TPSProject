using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorWeapon : Weapon
{


    protected override void Awake()
    {
        base.Awake();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("데미지 입힘");
            other.GetComponent<EnemyCtrl>().GetDamage(damage);
        }
    }
    public override void EnableWeaponCollider()
    {
        base.EnableWeaponCollider();
    }

    public override void DisableweaponCollider()
    {
        base.DisableweaponCollider();
    }
}
