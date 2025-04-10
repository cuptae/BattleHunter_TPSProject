using UnityEngine.AI;

public class EnemyDieState : IEnemyState
{
    public void EnterState(EnemyCtrl enemy)
    {
        enemy.currState = EnemyState.DIE;
        enemy.isDead = true;

        if(enemy.pv.isMine)
        {
            var agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.ResetPath(); // 필요 시
            }
        }

        //PoolManager.Instance.ReturnObject("Dragoon", enemy.gameObject);
        enemy.Invoke("Die",0.2f);
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

