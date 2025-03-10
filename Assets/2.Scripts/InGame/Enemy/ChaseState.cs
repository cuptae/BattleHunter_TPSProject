using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    public void EnterState(EnemyCtrl enemy)
    {
        enemy.navMeshAgent.isStopped = false;
    }

    public void UpdateState(EnemyCtrl enemy)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            enemy.navMeshAgent.SetDestination(player.transform.position);

            // 플레이어가 공격 범위 내에 있으면 ATTACK 상태로 전환
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2.0f)
            {
                enemy.ChangeState(new AttackState());
            }
        }
    }

    public void ExitState(EnemyCtrl enemy)
    {
        enemy.navMeshAgent.isStopped = true;
    }
}

