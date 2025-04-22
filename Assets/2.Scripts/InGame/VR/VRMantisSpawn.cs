using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMantisSpawn : MonoBehaviour
{
    public GameObject mantisObj;
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SpawnMantis()
    {
        if(mantisObj.activeSelf)
        {
            return;
        }
        mantisObj.SetActive(true);
    }

}
