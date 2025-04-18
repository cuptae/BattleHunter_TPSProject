using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerCtrl player) : base(player) {}

    public override void EnterState()
    {
        player.curState = STATE.IDLE;
        player.animator.SetFloat("Speed", 0f);
    }

    public override void UpdateState()
    {
        player.Rotation();
        if (player.isMove)
        {
            player.ChangeState(new MoveState(player));
        }
        if(player.DodgeInput())
        {
            player.ChangeState(new DodgeState(player));
        }
        if(player.isAttack){player.ChangeState(new PlayerAttackState(player));}
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

    public override void ExitState() {}
}
