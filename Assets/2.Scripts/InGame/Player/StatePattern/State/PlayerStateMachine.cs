public class PlayerStateMachine
{
    private PlayerState currentState;

    public void Initialize(PlayerState initialState)
    {
        currentState = initialState;
        currentState.EnterState();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void Update()
    {
        currentState.UpdateState();
    }

    public void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }
}
