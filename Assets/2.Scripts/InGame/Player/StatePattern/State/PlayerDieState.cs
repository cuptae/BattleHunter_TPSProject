using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(PlayerCtrl player):base(player){}

    public override void EnterState()
    {
        player.curState = STATE.DEAD;
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
