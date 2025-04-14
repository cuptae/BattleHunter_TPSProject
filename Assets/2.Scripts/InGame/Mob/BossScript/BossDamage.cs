using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public int damage = 10;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCtrl playerdamege = other.GetComponent<PlayerCtrl>();

            if (playerdamege != null)
            {
                playerdamege.GetDamage(damage);
            }
            
        }
    }
}
