using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    Collider collider;

    void Awake()
    {
        collider = GetComponent<Collider>();
    }

}
