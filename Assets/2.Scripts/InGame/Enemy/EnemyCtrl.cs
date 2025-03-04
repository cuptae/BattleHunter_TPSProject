using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    ROAMING,
    CHASE,
    DIE,
    ATTACK,
}
public class EnemyCtrl : MonoBehaviour
{
    protected int hp = 1000;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(float damage)
    {
        hp -= Mathf.FloorToInt(damage);
    }
}
