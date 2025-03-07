using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    IDLE,
    PATROL,
    CHASE,
    HIT,
    ATTACK,
    DIE,
}

public class MonsterAi : MonoBehaviour
{
    protected MonsterState state;


}
