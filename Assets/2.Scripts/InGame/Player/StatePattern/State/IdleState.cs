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
        if (player.isMove)
        {
            player.ChangeState(new MoveState(player));
        }
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState() {}
}
