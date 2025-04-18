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
        // index가 0, 2, 0, 2 반복
        index = (index == 0) ? 2 : 0;
        attackEffectPrefab[index].gameObject.SetActive(true);
    }
    public void AttackEffect2()
    {
        // index가 1, 3, 1, 3 반복
        index = (index == 1) ? 3 : 1;
        attackEffectPrefab[index].gameObject.SetActive(true);
    }
}
