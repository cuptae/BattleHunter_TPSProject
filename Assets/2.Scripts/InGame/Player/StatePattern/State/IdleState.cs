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
        if(player.QSkillInput())
        {
            player.ChangeState(new SkillState(player,player.activeSkills[0]));
        }
        if(player.ESkillInput())
        {
            player.ChangeState(new SkillState(player,player.activeSkills[1]));
        }
        if(player.RSkillInput())
        {
            player.ChangeState(new SkillState(player,player.activeSkills[2]));
        }
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState() {}
}
