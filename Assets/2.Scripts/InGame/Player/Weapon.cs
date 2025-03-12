using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public Collider collider = null;


    protected virtual void Awake()
    {
        collider = GetComponent<Collider>();
    }

    public virtual void EnableWeaponCollider()
    {
        collider.enabled = true;
    }

    public virtual void DisableweaponCollider()
    {
        collider.enabled = false;
    }
}
