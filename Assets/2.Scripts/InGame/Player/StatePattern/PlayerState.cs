public abstract class PlayerState
{
    protected PlayerCtrl player;

    public PlayerState(PlayerCtrl player)
    {
        this.player = player;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void FixedUpdateState();
}
