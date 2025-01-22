using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    protected int hp = 1000;
    
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetDamage(float damage,Vector3 hitPos, Quaternion hitDir)
    {
        Instantiate(hitEffect, hitPos, hitDir);
        hp -= Mathf.FloorToInt(damage);
        return hp;
    }
}
