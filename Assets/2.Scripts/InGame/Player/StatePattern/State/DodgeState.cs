using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : PlayerState
{
    public DodgeState(PlayerCtrl player) : base (player){}

    public override void EnterState()
    {
        player.StartCoroutine(DodgeRoutine());
    }

    public override void UpdateState() {}

    public override void ExitState() {}
    public override void FixedUpdateState()
    {
        
    }
    private IEnumerator DodgeRoutine()
    {
        player.animator.SetTrigger("Dodge");
        player.isDodge = true;
        yield return new WaitForSeconds(player.dodgeTime);
        player.isDodge = false;
        player.ChangeState(new IdleState(player));
    }
}
