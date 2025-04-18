using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(PlayerCtrl player):base(player){}

    public override void EnterState()
    {
        Debug.Log("Player is dead");
        player.curState = STATE.DEAD;
        player.isDead = true;
        player.animator.SetBool("IsDead", true);
        player.animator.SetTrigger("Die");
    }
    public override void UpdateState()
    {
        
    }
    public override void FixedUpdateState()
    {

    }
    public override void ExitState()
    {

    }
}
