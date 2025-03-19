using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : PlayerState
{
    public DodgeState(PlayerCtrl player) : base (player){}

    public override void EnterState()
    {
        player.curState = STATE.DODGE;
        player.StartCoroutine(Dodge());
    }

    public override void UpdateState() {}

    public override void ExitState() {}
    public override void FixedUpdateState(){}

    IEnumerator Dodge()
    {
        player.animator.SetTrigger("Dodge");
        yield return new WaitForSeconds(player.dodgeTime);
        player.ChangeState(new IdleState(player));
    }
}
