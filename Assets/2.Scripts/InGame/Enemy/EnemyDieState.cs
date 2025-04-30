using UnityEngine.AI;

public class EnemyDieState : IEnemyState
{
    public void EnterState(EnemyCtrl enemy)
    {
        enemy.currState = EnemyState.DIE;
        enemy.isDead = true;
        enemy.pv.RPC("PlusPoint", PhotonTargets.All);
        if(enemy.pv.isMine)
        {
            var agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.ResetPath(); // 필요 시
            }
        }
        foreach(var players in GameManager.Instance.players)
        {
            players.pv.RPC("AddExp", PhotonTargets.All, 10);
        }
        enemy.Die();
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

