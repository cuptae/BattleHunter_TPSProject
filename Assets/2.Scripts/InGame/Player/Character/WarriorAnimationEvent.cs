using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAnimationEvent : MonoBehaviour
{
    Warrior player;
    public ParticleSystem[] attackEffectPrefab;
    int index = 0;

    void Awake()
    {
        player = GetComponentInParent<Warrior>();
    }

    public void Attack()
    {
        player.Attack();
    }

    public void AttackEffect()
    {
        index = index%3;
        attackEffectPrefab[index].gameObject.SetActive(true);
        index++;
    }
}
