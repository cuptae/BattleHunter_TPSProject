public interface IEnemyState
{
    void EnterState(EnemyCtrl enemy);
    void UpdateState(EnemyCtrl enemy);
    void FixedUPdateState(EnemyCtrl enemy);
    void ExitState(EnemyCtrl enemy);
}
