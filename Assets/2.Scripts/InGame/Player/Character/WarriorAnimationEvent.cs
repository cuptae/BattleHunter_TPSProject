using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAnimationEvent : MonoBehaviour
{
    Warrior player;

    void Awake()
    {
        player = GetComponentInParent<Warrior>();
    }

    public void Attack()
    {
        player.Attack();
    }
}
