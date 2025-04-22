using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMantisHP : MonoBehaviour
{
    
    public int maxHP = 400;
    public int currentHP = 400;

    void Awake()
    {
        currentHP = maxHP;
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        // Handle death logic here, such as playing an animation or destroying the object
        Destroy(gameObject);
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }
}
