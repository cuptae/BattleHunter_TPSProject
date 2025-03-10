public interface IEnemyState
{
    void EnterState(EnemyCtrl enemy);
    void UpdateState(EnemyCtrl enemy);
    void ExitState(EnemyCtrl enemy);
}
