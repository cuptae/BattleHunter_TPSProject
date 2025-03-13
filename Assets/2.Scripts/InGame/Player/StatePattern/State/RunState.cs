using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerCtrl player) : base(player) {}

    public override void EnterState()
    {
        player.animator.SetFloat("Speed", 1f);
        //player.finalSpeed = player.runSpeed;
    }

    public override void UpdateState()
    {


        if (!player.isMove)
        {
            player.ChangeState(new IdleState(player));
        }
    }

    public override void FixedUpdateState()
    {
        //player.Move();
    }

    public override void ExitState() {}
}
