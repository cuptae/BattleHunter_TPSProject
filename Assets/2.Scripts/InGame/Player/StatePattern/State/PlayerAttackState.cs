using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(PlayerCtrl player) : base(player){}

    public override void EnterState()
    {
        player.curState =STATE.ATTACK;
    }

    public override void UpdateState()
    {
        player.Attack();
        player.Rotation();
        if(player.isMove)
        {
            MovingAttack();
            player.animator.SetBool("Move",true);
        }

        else
        {
            player.rigid.velocity = Vector3.zero;
            player.animator.SetBool("Move",false);
        }

        if(!player.isAttack){player.ChangeState(new IdleState(player));}
        if (player.DodgeInput()){player.ChangeState(new DodgeState(player));}
        // Q 스킬 입력 처리
        if (player.QSkillInput() && !player.activeSkills[0].isOnCooldown)
        {
            player.ChangeState(new SkillState(player, player.activeSkills[0]));
        }

        // E 스킬 입력 처리
        if (player.ESkillInput() && !player.activeSkills[1].isOnCooldown)
        {
            player.ChangeState(new SkillState(player, player.activeSkills[1]));
        }

        // R 스킬 입력 처리
        if (player.RSkillInput() && !player.activeSkills[2].isOnCooldown)
        {
            player.ChangeState(new SkillState(player, player.activeSkills[2]));
        }
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        player.rigid.velocity = Vector3.zero;
    }


    void MovingAttack()
    {

        if(!player.IsSlope())
        {
            player.rigid.AddForce(player.MoveDir()*20,ForceMode.Force);
        }
        else
        {
            player.rigid.AddForce(Vector3.ProjectOnPlane(player.MoveDir(),player.groundNormal).normalized*20,ForceMode.Force);
        }
        if (player.rigid.velocity.magnitude > player.characterStat.AttackWalkSpeed)
        {
            player.rigid.velocity = player.rigid.velocity.normalized * player.characterStat.AttackWalkSpeed;
        }
    }
}
    
