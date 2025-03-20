using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : PlayerState
{
    public DodgeState(PlayerCtrl player) : base (player){}
    private float dodgeForce = 7f;


    public override void EnterState()
    {
        player.curState = STATE.DODGE;
        player.StartCoroutine(Dodge());
    }

    public override void UpdateState() {}

    public override void ExitState()
    {
        player.rigid.velocity = Vector3.zero;        
    }
    public override void FixedUpdateState(){}

    IEnumerator Dodge()
    {
        player.animator.SetTrigger("Dodge");
        Vector3 dodgeDir = player.MoveDir();
        if(player.MoveDir() == Vector3.zero)
            dodgeDir = player.transform.forward;

        if(player.MoveDir() != Vector3.zero)
            player.transform.rotation = Quaternion.LookRotation(dodgeDir);
        else
            player.transform.rotation = Quaternion.LookRotation(dodgeDir);

        if(!player.IsSlope())
            player.rigid.AddForce(dodgeDir*dodgeForce,ForceMode.Impulse);
        else
            player.rigid.AddForce(Vector3.ProjectOnPlane(dodgeDir,player.groundNormal).normalized*dodgeForce,ForceMode.Impulse);
        yield return new WaitForSeconds(player.characterData.dodgeTime);
        player.ChangeState(new IdleState(player));
    }
}
