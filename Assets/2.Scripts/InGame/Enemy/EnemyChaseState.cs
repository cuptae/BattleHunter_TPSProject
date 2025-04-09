using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IEnemyState
{
    public void EnterState(EnemyCtrl enemy)
    {
        enemy.currState = EnemyState.CHASE;
    
        enemy.navMeshAgent.isStopped = false;
        
    }

    public void UpdateState(EnemyCtrl enemy)
    {
        if (enemy.targetPlayer != null)
        {
            enemy.navMeshAgent.SetDestination(enemy.targetPlayer.position);

            // 플레이어가 공격 범위 내에 있으면 ATTACK 상태로 전환
            if (Vector3.Distance(enemy.transform.position, enemy.targetPlayer.position) < enemy.attackRange)
            {
                enemy.ChangeState(new EnemyAttackState());
            }
        }
    }

    public void FixedUPdateState(EnemyCtrl enemy)
    {
        
    }


    public void ExitState(EnemyCtrl enemy)
    {
        var agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }
}

