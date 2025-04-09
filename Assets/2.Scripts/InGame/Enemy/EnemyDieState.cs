using UnityEngine.AI;

public class EnemyDieState : IEnemyState
{
    //public void EnterState(EnemyCtrl enemy)
    //{
    //    enemy.isDead = true;
    //    enemy.navMeshAgent.isStopped = true;
    //    PoolManager.Instance.ReturnObject("Mutant", enemy.gameObject);
    //}
    public void EnterState(EnemyCtrl enemy)
    {
        enemy.isDead = true;

        var agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath(); // 필요 시
        }

        PoolManager.Instance.ReturnObject("Dragoon", enemy.gameObject);
    }

    public void UpdateState(EnemyCtrl enemy)
    {
        // 사망 상태에서는 아무것도 안 함
    }

    public void FixedUPdateState(EnemyCtrl enemy)
    {

    }

    public void ExitState(EnemyCtrl enemy)
    {
        // 사망 상태 해제 불가능
    }
}

